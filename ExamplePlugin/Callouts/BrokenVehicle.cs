using System;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Windows;
using System.Windows.Forms;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Engine;
using System.Net.Mail;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using RegularCallouts.Stuff;
using UltimateBackup;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Broken Vehicle", CalloutProbability.Medium)]
    public class BrokenVehicle : Callout
    {
        private Ped Driver;
        private Vehicle AccidentCar;
        private bool CalloutRunning;
        private bool OutroAnimation;
        private Blip TowBlip;
        private bool SpokenWithDriver;
        private bool TruckArrived;
        private Vector3 SpawnPoint;
        private Ped Towtruckdriver;
        private Vehicle TowTruck;
        private Blip SpawnBlip;
        private bool CarTowed;
        private int WaitCount;
        private string[] Titles = new string[] {"DUKES", "BALLER", "BALLER2", "BISON", "BISON2", "BJXL", "CAVALCADE", "CHEETAH", "COGCABRIO", "ASEA", "ADDER", "FELON", "FELON2", "ZENTORNO",
        "WARRENER", "RAPIDGT", "INTRUDER", "FELTZER2", "FQ2", "RANCHERXL", "REBEL", "SCHWARZER", "COQUETTE", "CARBONIZZARE", "EMPEROR", "SULTAN", "EXEMPLAR", "MASSACRO",
        "DOMINATOR", "ASTEROPE", "PRAIRIE", "NINEF", "WASHINGTON", "CHINO", "CASCO", "INFERNUS", "ZTYPE", "DILETTANTE", "VIRGO", "F620", "PRIMO", "FBI"};
        private List<string> DialogWithDriver = new List<string>
        {
            "~b~You:~s~ Hello Sir. We've got a call for a vehicle accident. Is everything alright? (1/5)",
            "~y~Driver:~s~ Yeah I'm fine. This shitty car died on me for the third time now and I can't fix that thing. (2/5)",
            "~b~You:~s~ Did you call a mechanic to take a look at it yet? (3/5)",
            "~y~Driver:~s~ No I left my phone at home. (4/5)",
            "~b~You:~s~ Alright I will call a mechanic for you, hang tight. You should probably get off the road though. (5/5)"
        };
        private int DialogWithDriverIndex;


        private List<Entity> AllBankHeistEntities = new List<Entity>();
        private List<Ped> AllSpawnedPeds = new List<Ped>();
        private List<Vehicle> AllSpawnedVehicles = new List<Vehicle>();

        public override bool OnBeforeCalloutDisplayed()
        {

            while (true)
            {
                SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(50f, 250f));
                {
                    if (Game.LocalPlayer.Character.DistanceTo2D(SpawnPoint) > 49f)
                    {
                        break;
                    }
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }

            //SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 500f));
            CalloutMessage = "Broken Vehicle";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            CalloutRunning = true;
            GameFiber.StartNew(delegate
            {
                try
                {
                    SpawnBlip = new Blip(SpawnPoint, 40f);
                    SpawnBlip.Color = Color.Yellow;
                    SpawnBlip.Alpha = 0.5f;
                    SpawnBlip.EnableRoute(Color.Yellow);
                    TruckArrived = true;
                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        AllBankHeistEntities.Add(Game.LocalPlayer.Character.CurrentVehicle);
                        Ped[] passengers = Game.LocalPlayer.Character.CurrentVehicle.Passengers;
                        if (passengers.Length > 0)
                        {
                            foreach (Ped passenger in passengers)
                            {
                                AllBankHeistEntities.Add(passenger);
                            }
                        }
                    }
                    GameFiber.Yield();
                    CreateSpeedZone();
                    GameFiber.Yield();
                    SpawnAccidentStuff();
                    GameFiber.Yield();
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        //StuckTruck();
                        if (Game.LocalPlayer.Character.DistanceTo(AccidentCar) <= 10f && OutroAnimation == false)
                        {
                            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
                            Game.DisplaySubtitle("~y~Driver:~s~God fucking dammit! That shitty thing never works!");
                            Game.DisplayHelp("Park behind the vehicle and investigate.");
                            Driver.Tasks.Clear();
                            OutroAnimation = true;
                        }
                        if(Game.LocalPlayer.Character.DistanceTo(Driver) <= 5f && OutroAnimation == true)
                        {
                            Driver.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                        }
                        if(Game.LocalPlayer.Character.DistanceTo(Driver) <= 2f && SpokenWithDriver == false)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithDriverIndex < DialogWithDriver.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithDriver[DialogWithDriverIndex]);
                                    DialogWithDriverIndex++;
                                }
                                if (DialogWithDriverIndex == DialogWithDriver.Count)
                                {
                                    Driver.Tasks.FollowNavigationMeshToPosition(AccidentCar.RightPosition, AccidentCar.Heading, 1f);
                                    Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 5000);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ I need a Towtruck at " + World.GetStreetName(Game.LocalPlayer.Character.Position));
                                    GameFiber.Sleep(5000);
                                    Game.DisplayNotification("~b~Attention to Unit:~s~ Towtruck is en route.");
                                    Game.DisplayHelp("If the Truck is stuck, press ~r~" + Settings.InteractionKey + " ~s~to teleport him.");
                                    TruckArrived = false;
                                    SpawnTowTruckStuff();
                                }
                            }
                        }
                        if(SpokenWithDriver == true && CarTowed == false)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(Towtruckdriver) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Towtruckdriver.Tasks.Clear();
                                    Towtruckdriver.Tasks.EnterVehicle(TowTruck, -1);
                                    if(Driver.IsCuffed == false || Driver.IsDead == false || Driver.Exists())
                                    {
                                        Driver.Tasks.Clear();
                                        Driver.Tasks.EnterVehicle(TowTruck, 0);
                                    }
                                    CarTowed = true;
                                }
                            }
                        }
                        if(CarTowed == true)
                        {
                            if (Towtruckdriver.IsInAnyVehicle(true))
                            {
                                TowTruck.TowVehicle(AccidentCar, true);
                                this.End();
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Game.DisplayNotification("Callout ran into a problem and terminated itself. Please submit your Log to the Regular Callouts LSPDFR Topic.");
                    Game.LogTrivial("Crash averted:"+ e);
                    this.End();
                }
        });
        }
        public override void End()
        {
            CalloutRunning = false;
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (TowBlip.Exists()) { TowBlip.Delete(); }
            foreach (Ped i in AllSpawnedPeds)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Vehicle i in AllSpawnedVehicles)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
                if (i.IsMission())
                {
                    i.RemoveMission();
                }
            }
            base.End();
        }
        private void GameEnd()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Settings.EndCalloutKey))
                {
                    this.End();
                }

            });
        }

        private void SpawnAccidentStuff()
        {
            AccidentCar = new Vehicle(Titles[new Random().Next(0, Titles.Length)], SpawnPoint, Utils.VehicleNodeHeading(SpawnPoint));
            AccidentCar.IsPersistent = true;
            AccidentCar.MakeMission();
            AllBankHeistEntities.Add(AccidentCar);
            AllSpawnedVehicles.Add(AccidentCar);
            Vector3 NewSpawn = AccidentCar.GetOffsetPositionRight(5f);
            AccidentCar.Position = NewSpawn;
            AccidentCar.EngineHealth = 250f;
            AccidentCar.IsEngineOn = true;
            AccidentCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            Driver = new Ped(AccidentCar.FrontPosition, AccidentCar.Heading + 180f);
            Driver.Position = Driver.GetOffsetPositionFront(-0.2f);
            AllBankHeistEntities.Add(Driver);
            AllSpawnedPeds.Add(Driver);
            Driver.IsPersistent = true;
            Driver.BlockPermanentEvents = true;
            Driver.Tasks.PlayAnimation("missfbi3_electrocute", "electrocute_both_loop_player", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            AccidentCar.IsStolen = false;
            Persona.FromExistingPed(Driver).ELicenseState = ELicenseState.Valid;
            Persona.FromExistingPed(Driver).Wanted = false;
            Functions.SetVehicleOwnerName(AccidentCar, Functions.GetPersonaForPed(Driver).FullName.ToString());
            Utils.VehicleDoorOpen(AccidentCar, Utils.VehDoorID.Hood, false, false);
        }

        private void CreateSpeedZone()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {
                    GameFiber.Yield();

                    foreach (Vehicle veh in World.GetEntities(SpawnPoint, 75f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludeFiretrucks | GetEntitiesFlags.ExcludeAmbulances))
                    {
                        GameFiber.Yield();
                        if (AllBankHeistEntities.Contains(veh))
                        {
                            continue;
                        }
                        if (veh != null)
                        {
                            if (veh.Exists())
                            {
                                if (veh != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!veh.CreatedByTheCallingPlugin)
                                    {
                                        if (!AllBankHeistEntities.Contains(veh))
                                        {
                                            if (veh.Velocity.Length() > 20f)
                                            {
                                                Vector3 velocity = veh.Velocity;
                                                velocity.Normalize();
                                                velocity *= 20f;
                                                veh.Velocity = velocity;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private void StuckTruck()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Settings.InteractionKey) && TruckArrived == false)
                {
                    Towtruckdriver.Tasks.ClearImmediately();
                    TowTruck.Position = AccidentCar.GetOffsetPositionFront(10f);
                    TowTruck.Heading = AccidentCar.Heading;
                    Towtruckdriver.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                    Game.DisplaySubtitle("~y~Trucker:~s~ Good day to yall, let me now when you're finished Officer so I can hook up the car");
                    Towtruckdriver.Tasks.FollowNavigationMeshToPosition(AccidentCar.FrontPosition, AccidentCar.Heading + 180f, 1f);
                    TruckArrived = true;
                }
            });
        }

        private void SpawnTowTruckStuff()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                Vector3 NewSpawn = World.GetNextPositionOnStreet(AccidentCar.Position.Around(50f, 150f));
                TowTruck = new Vehicle("towtruck", NewSpawn, Utils.VehicleNodeHeading(NewSpawn));
                TowTruck.IsPersistent = true;
                TowTruck.IsInvincible = true;
                TowTruck.MakeMission();
                TowTruck.IsSirenOn = true;
                TowBlip = TowTruck.AttachBlip();
                TowBlip.Sprite = BlipSprite.TowTruck2;
                TowBlip.Color = Color.Yellow;
                TowBlip.Flash(500, -1);
                AllBankHeistEntities.Add(TowTruck);
                AllSpawnedVehicles.Add(TowTruck);
                Towtruckdriver = TowTruck.CreateRandomDriver();
                Towtruckdriver.IsPersistent = true;
                Towtruckdriver.BlockPermanentEvents = true;
                AllBankHeistEntities.Add(Towtruckdriver);
                AllSpawnedPeds.Add(Towtruckdriver);
                SpokenWithDriver = true;
                Game.LogTrivial("Towtruck spawned successfull. Setting driving task");
                Towtruckdriver.Tasks.DriveToPosition(AccidentCar.LeftPosition, 5f, VehicleDrivingFlags.DriveAroundObjects | VehicleDrivingFlags.AllowWrongWay | VehicleDrivingFlags.Emergency | VehicleDrivingFlags.AllowMedianCrossing | VehicleDrivingFlags.DriveAroundPeds | VehicleDrivingFlags.DriveAroundVehicles).WaitForCompletion();
                Towtruckdriver.Tasks.ParkVehicle(AccidentCar.GetOffsetPositionFront(5f), AccidentCar.Heading).WaitForCompletion();
                //Towtruckdriver.Tasks.ParkVehicle(AccidentCar.GetOffsetPositionFront(5f), AccidentCar.Heading);
                TruckArrived = true;
                Towtruckdriver.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                Game.DisplaySubtitle("~y~Trucker:~s~ Good day to yall, let me now when you're finished Officer so I can hook up the car");
                Towtruckdriver.Tasks.FollowNavigationMeshToPosition(AccidentCar.FrontPosition, AccidentCar.Heading + 180f, 1f).WaitForCompletion();
            });
        }
    }

}
