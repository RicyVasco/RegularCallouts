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
using LucasRitter.Scaleforms.Generic;
using RegularCallouts.Stuff;
using UltimateBackup;
using LucasRitter.Scaleforms;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Lost Suspect", CalloutProbability.Medium)]
    public class LostSuspect : Callout
    {
        private Vector3 SpawnPoint;
        private Vector3 SpawnPoint1;
        private bool CalloutRunning;
        private bool ChopperIn;
        private bool ToggleThermal = false;
        private bool ToggleNight = true;
        private Ped Driver;
        private HeliCam HeliCamera;
        private Vehicle Chopper;
        private Vehicle PolCar;
        private Vector3 SearchArea;
        private Ped Cop1;
        private Ped Cop2;
        private Ped Suspect;
        private Ped ClonePed;
        private Blip SpawnBlip;
        private bool TalkedToPilot;
        private ELostPerson state;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private Vector3[] SuspectLocations;
        private Tuple<Vector3, float> ChopperPosition;
        private Tuple<Vector3, float> PolCarPositon;
        private List<string> DialogWithPilot = new List<string>
        {
            "~g~Pilot:~s~ Glad you arrived so quickly Sir.",
            "~b~You:~s~ What's going on?",
            "~g~Pilot:~s~ A unit chased a suspect into this area. However they can't find him. We are flying overhead to spot them.",
            "~g~Pilot:~s~ My Co-Pilot called in sick today so I have no one that can control my camera. That's why I called you.",
            "~b~You:~s~ Okay I see. Anything else I need to know?",
            "~g~Pilot:~s~ You probably learned how to use the helicopter's camera in training, but in case you forgot I'll give you a quick rundown.",
            "~g~Pilot:~s~ Press ~y~Left Mouse/Attack~s~ to guide me to a new location",
            "~g~Pilot:~s~ Press ~y~Right Mouse/Aim~s~ to guide the Ground Units",
            "~g~Pilot:~s~ Press ~y~Middle Mouse/Phone~s~ to toggle between different kind of visions.",
            "~g~Pilot:~s~ Are you all set?",
            "~b~You:~s~ Yeah, let's go.",
        };
        private int DialogWithPilotIndex;
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(-1397.67358f, 122.945625f, 54.8972168f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1) //LS Golf
            {
                SpawnPoint = SpawnPoint1;
                SearchArea = new Vector3(-1240.9436f, 83.250351f, 53.2911682f);
                ChopperPosition = new Tuple<Vector3, float>(new Vector3(-1397.67358f, 122.945625f, 54.8972168f), -85.4449692f);
                PolCarPositon = new Tuple<Vector3, float>(new Vector3(-1343.80969f, 127.864159f, 55.7063332f), -88.063446f);
                SuspectLocations = new Vector3[] { new Vector3(-1282.92834f, 139.080582f, 58.4147682f), new Vector3(-1263.71094f, 124.801064f, 57.6068687f), new Vector3(-1284.20996f, 57.4435501f, 52.108284f), new Vector3(-1203.30261f, 26.4463139f, 50.3469849f), new Vector3(-1155.22803f, -54.7844048f, 44.6705399f), new Vector3(-1066.51917f, -13.4376764f, 50.428978f)};
            }
            CalloutMessage = "Lost Suspect";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_SUSPECT_ON_THE_RUN IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            HeliCamera = new HeliCam();
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            SpawnBlip = new Blip(SpawnPoint, 40f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            state = ELostPerson.DrivingToScene;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
            CalloutRunning = true;
            GameFiber.StartNew(delegate
            {
                while (Vector3.Distance(Game.LocalPlayer.Character.Position, SpawnPoint) > 350f)
                {
                    GameFiber.Yield();
                    GameEnd();
                }
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    AllCalloutEntities.Add(Game.LocalPlayer.Character.CurrentVehicle);
                    Ped[] passengers = Game.LocalPlayer.Character.CurrentVehicle.Passengers;
                    if (passengers.Length > 0)
                    {
                        foreach (Ped passenger in passengers)
                        {
                            AllCalloutEntities.Add(passenger);
                        }
                    }
                }
                GameFiber.Yield();
                ClearUnrelatedEntities();
                Game.LogTrivial("Unrelated entities cleared");
                GameFiber.Yield();
                MakeNearbyPedsFlee();
                GameFiber.Yield();
                SpawnStuff();
                GameFiber.Yield();
                Game.LogTrivial("Initialisation complete, Spawning Callout");
                while (CalloutRunning)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (state == ELostPerson.DrivingToScene)
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(Driver) <= 30f)
                        {
                            SpawnBlip.Delete();
                            SpawnBlip = Driver.AttachBlip();
                            SpawnBlip.Color = Color.Green;
                            Game.DisplayHelp("Talk with the ~g~Pilot~s~");
                            state = ELostPerson.ArrivedAtScene;
                        }
                    }
                    if (state == ELostPerson.ArrivedAtScene)
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(Driver) <= 2f && !TalkedToPilot)
                        {
                            Driver.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithPilotIndex < DialogWithPilot.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithPilot[DialogWithPilotIndex]);
                                    DialogWithPilotIndex++;
                                }
                                if (DialogWithPilotIndex == DialogWithPilot.Count)
                                {
                                    Driver.Tasks.EnterVehicle(Chopper, -1);
                                    SpawnBlip.Delete();
                                    TalkedToPilot = true;
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Chopper) <= 5f && TalkedToPilot)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to enter the helicopter.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.LocalPlayer.Character.Tasks.EnterVehicle(Chopper, 0).WaitForCompletion(10000);
                                state = ELostPerson.TalkedToPilot;
                            }
                        }
                    }
                    if (state == ELostPerson.TalkedToPilot)
                    {
                        if (Driver.IsInVehicle(Chopper, false))
                        {
                            Chopper.TurnRotorsOn();
                            Game.DisplaySubtitle("~g~Pilot:~s~ I'll bring the helicopter into position and then you can have full control!");
                            Game.LocalPlayer.Character.IsVisible = false;
                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(LeaveVehicleFlags.WarpOut).WaitForCompletion();
                            Game.LocalPlayer.Character.AttachTo(Chopper, 0, new Vector3(0f, 0f, -3.5f), new Rotator(0, 0f, 0f));
                            Driver.FlyChopper(Chopper, Game.LocalPlayer.Character.GetOffsetPositionUp(100f), 4, 50f, 10f, 0f, 0);
                            GameFiber.Sleep(3000);
                            Game.DisplaySubtitle("~g~Pilot:~s~ She's all yours. Make it count!");
                            ClonePed = Game.LocalPlayer.Character.Clone();
                            ClonePed.IsPersistent = true;
                            ClonePed.BlockPermanentEvents = true;
                            AllCalloutEntities.Add(ClonePed);
                            ClonePed.WarpIntoVehicle(Chopper, 0);
                            ChopperIn = true;
                            SpawnBlip = new Blip(SearchArea, 300f);
                            SpawnBlip.Color = Color.Red;
                            SpawnBlip.Alpha = 0.5f;
                            SpawnSuspect();
                            ClearUnrelatedEntitiesSearchArea();
                            state = ELostPerson.InTheAir;
                        }
                    }
                    if (state == ELostPerson.InTheAir)
                    {
                        if (ChopperIn)
                        {
                            HeliCamera.Draw();
                            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(Game.LocalPlayer.Character, false);
                            Game.DisableControlAction(0, GameControl.Phone, true);
                            if (Game.IsControlJustPressed(0, GameControl.Attack))
                            {
                                HitResult t = Utils.RayCastForward();
                                Driver.FlyChopper(Chopper, new Vector3(t.HitPosition.X, t.HitPosition.Y, t.HitPosition.Z + 100f), 4, 50f, 10f, Utils.NC_Get_Cam_Rotation().Z, 0);
                            }
                            if (Game.IsControlJustPressed(0, GameControl.Aim))
                            {
                                HitResult t = Utils.RayCastForward();
                                Utils.ShootBulletBetweenCoord(new Vector3(t.HitPosition.X, t.HitPosition.Y, t.HitPosition.Z + 1f), t.HitPosition, 1, true, 0x497FACC3, Game.LocalPlayer.Character, true, false, 5f);
                                if (Cop1.DistanceTo(t.HitPosition) > 100f)
                                {
                                    Game.DisplayNotification("~g~Ground Unit:~s~ I can't spot the marker! Please move it closer to our current position!");
                                }
                                else
                                {
                                    Cop1.Tasks.FollowNavigationMeshToPosition(t.HitPosition, 0f, 3f);
                                    Cop2.Tasks.FollowNavigationMeshToPosition(t.HitPosition, 0f, 3f);
                                    Game.DisplayNotification("~g~Ground Unit:~s~ Moving to position!");
                                }
                            }
                            if (NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(0, 27) == true)
                            {
                                if (ToggleNight && !ToggleThermal)
                                {
                                    NativeFunction.Natives.SET_NIGHTVISION(true);
                                    ToggleNight = false;
                                }
                                else if (!ToggleNight && !ToggleThermal)
                                {
                                    NativeFunction.Natives.SET_NIGHTVISION(false);
                                    NativeFunction.Natives.SET_SEETHROUGH(true);
                                    ToggleThermal = true;
                                }
                                else
                                {
                                    NativeFunction.Natives.SET_SEETHROUGH(false);
                                    ToggleNight = true;
                                    ToggleThermal = false;
                                }
                            }
                        }
                        if (Cop1.DistanceTo(Suspect) <= 5f)
                        {
                            Game.DisplayNotification("~g~Ground Unit:~s~ We've found the suspect. Thanks for your help!");
                            ClonePed.Delete();
                            if (SpawnBlip.Exists()) SpawnBlip.Delete();
                            Dispose(HeliCamera);
                            Game.LocalPlayer.Character.WarpIntoVehicle(Chopper, 0);
                            Driver.FlyChopper(Chopper, ChopperPosition.Item1, 4, 50f, 10f, ChopperPosition.Item2, 32);
                            Game.LocalPlayer.Character.IsVisible = true;
                            NativeFunction.Natives.SET_NIGHTVISION(false);
                            NativeFunction.Natives.SET_SEETHROUGH(false);
                            state = ELostPerson.Ending;
                        }
                    }
                    if (state == ELostPerson.Ending)
                    {
                        if (Chopper.DistanceTo(ChopperPosition.Item1) <= 10f)
                        {
                            Chopper.TurnRotorsoff();
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                            this.End();
                        }
                        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
                        {
                            this.End();
                        }
                    }
                }
            });
        }
        public override void End()
        {
            CalloutRunning = false;
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (ClonePed.Exists()) ClonePed.Delete();
            foreach (Entity e in AllCalloutEntities)
            {
                if (e.Exists()) e.Dismiss();
            }
            Game.LocalPlayer.Character.IsVisible = true;
            if (state == ELostPerson.InTheAir)
            {
                Game.LocalPlayer.Character.Detach();
                Game.LocalPlayer.Character.Position = ChopperPosition.Item1;
            }
            NativeFunction.Natives.SET_NIGHTVISION(false);
            NativeFunction.Natives.SET_SEETHROUGH(false);
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            Game.DisableControlAction(0, GameControl.Phone, false);
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(Game.LocalPlayer.Character, true);
            Dispose(HeliCamera);
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
        private void Dispose(Scaleform t)
        {
            unsafe
            {
                int handle = t.Handle;
                Game.LogTrivial("Handle = " + handle.ToString());
                NativeFunction.CallByName<uint>("SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED", &handle);
                Game.LogTrivial("Handle freed");
            }
            t = null;
        }
        private void SpawnStuff()
        {
            Chopper = new Vehicle("polmav", ChopperPosition.Item1, ChopperPosition.Item2);
            Chopper.IsPersistent = true;
            AllCalloutEntities.Add(Chopper);
            PolCar = new Vehicle("police", PolCarPositon.Item1, PolCarPositon.Item2);
            PolCar.IsPersistent = true;
            AllCalloutEntities.Add(PolCar);
            Driver = new Ped("s_f_y_cop_01", Chopper.RightPosition, Chopper.Heading);
            Driver.IsPersistent = true;
            Driver.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Driver);
            Cop1 = new Ped("s_f_y_cop_01", PolCar.LeftPosition, PolCar.Heading);
            Cop1.IsPersistent = true;
            Cop1.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Cop1);
            Cop2 = new Ped("s_f_y_cop_01", PolCar.RightPosition, PolCar.Heading);
            Cop2.IsPersistent = true;
            Cop2.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Cop2);
        }
        private void SpawnSuspect()
        {
            Suspect = new Ped("s_f_y_cop_01", SuspectLocations[new Random().Next(SuspectLocations.Length)], 0f);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Suspect);
        }
        private void ClearUnrelatedEntities()
        {

            foreach (Ped entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderAllPeds))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Vehicle entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderGroundVehicles))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ClearUnrelatedEntitiesSearchArea()
        {

            foreach (Ped entity in World.GetEntities(SearchArea, 300f, GetEntitiesFlags.ConsiderAllPeds))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SearchArea) < 300f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Vehicle entity in World.GetEntities(SearchArea, 300f, GetEntitiesFlags.ConsiderGroundVehicles))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!AllCalloutEntities.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SearchArea) < 300f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void MakeNearbyPedsFlee()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {

                    GameFiber.Yield();

                    foreach (Ped entity in World.GetEntities(SpawnPoint, 80f, GetEntitiesFlags.ConsiderAllPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ExcludePoliceOfficers))
                    {
                        GameFiber.Yield();
                        if (AllCalloutEntities.Contains(entity))
                        {
                            continue;
                        }
                        if (entity != null)
                        {
                            if (entity.IsValid())
                            {

                                if (entity.Exists())
                                {
                                    if (entity != Game.LocalPlayer.Character)
                                    {
                                        if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                        {

                                            if (!entity.CreatedByTheCallingPlugin)
                                            {

                                                if (!AllCalloutEntities.Contains(entity))
                                                {
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 74f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle != null)
                                                            {

                                                                entity.Tasks.PerformDrivingManeuver(VehicleManeuver.Wait);


                                                            }
                                                        }
                                                        else
                                                        {
                                                            Rage.Native.NativeFunction.CallByName<uint>("TASK_SMART_FLEE_COORD", entity, SpawnPoint.X, SpawnPoint.Y, SpawnPoint.Z, 75f, 6000, true, true);
                                                        }

                                                    }
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 65f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle.Exists())
                                                            {
                                                                entity.CurrentVehicle.Delete();
                                                            }
                                                        }
                                                        if (entity.Exists())
                                                        {
                                                            entity.Delete();
                                                        }

                                                    }
                                                }
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
        private enum ELostPerson
        {
            DrivingToScene,
            ArrivedAtScene,
            TalkedToPilot,
            InTheAir,
            Ending
        }
    }

}
