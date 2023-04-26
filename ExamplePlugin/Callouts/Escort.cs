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

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Escort", CalloutProbability.Medium)]
    public class Escort : Callout
    {
        private Ped EscortDriver;
        private Ped LimoDriver;
        private Vehicle Limo;
        private Vehicle PoliceEscort;
        private Vector3 SpawnPoint;
        private Vector3 Destination;
        private int CheckPoint1;
        private Blip SpawnBlip;
        private bool EscortActive;
        private int WaitCount;
        private bool CalloutRunning;

        private List<Entity> AllBankHeistEntities = new List<Entity>();
        private List<Ped> AllSpawnedPeds = new List<Ped>();
        private List<Vehicle> AllSpawnedVehicles = new List<Vehicle>();

        public override bool OnBeforeCalloutDisplayed()
        {

            while (true)
            {
                SpawnPoint = new Vector3(924.566711f, -3.5994966f, 78.3484802f);
                {
                    if (Game.LocalPlayer.Character.DistanceTo2D(SpawnPoint) > 49f)
                    {
                        break;
                    }
                }
                Destination = new Vector3(-500.587006f, 258.837494f, 82.3357468f);
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }

            //SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 500f));
            CalloutMessage = "Escort";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            //Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);

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
                    SpawnBlip = new Blip(SpawnPoint, 10f);
                    SpawnBlip.Color = Color.Yellow;
                    SpawnBlip.Alpha = 0.5f;
                    SpawnBlip.EnableRoute(Color.Yellow);
                    CheckPoint1 = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, SpawnPoint, SpawnPoint, 3f, Color.Red);
                    Utils.SetCheckpointHeight(CheckPoint1, 3f, 3f, 3f);
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
                    GameFiber.Yield();
                    SpawnAccidentStuff();
                    GameFiber.Yield();
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if(Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 2f && EscortActive == false)
                        {
                            SpawnBlip.Delete();
                            Utils.DeleteCheckpoint(CheckPoint1);
                            Utils.VehicleEscort(LimoDriver, Limo, Game.LocalPlayer.Character.CurrentVehicle, Utils.TypeOfEscort.Behind, 40f, Utils.DriveStyleType.Rushed, 1f, -1, 15f);
                            Utils.VehicleEscort(EscortDriver, PoliceEscort, Limo, Utils.TypeOfEscort.Behind, 40f, Utils.DriveStyleType.Rushed, 3f, -1, 15f);
                            SpawnBlip = new Blip(Destination);
                            SpawnBlip.EnableRoute(Color.Yellow);

                        }
                        if (EscortActive == true)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(Destination) <= 2f)
                            {
                                this.End();
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Game.DisplayNotification("Callout ran into a problem and terminated itself. Please submit your Log to the Regular Callouts LSPDFR Topic.");
                    Game.LogTrivial("Crash:"+ e);
                    this.End();
                }
        });
        }
        public override void End()
        {
            CalloutRunning = false;
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            try { Utils.DeleteCheckpoint(CheckPoint1); } catch { }
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
            Limo = new Vehicle("stretch", new Vector3(931.033386f, -7.39656973f, 78.4502029f), 59.8298531f);
            AllBankHeistEntities.Add(Limo);
            AllSpawnedVehicles.Add(Limo);
            Limo.IsPersistent = true;
            Limo.IsEngineOn = true;
            Limo.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            PoliceEscort = new Vehicle("police", new Vector3(937.457092f, -11.307374f, 78.4411011f), 58.7037659f);
            AllBankHeistEntities.Add(PoliceEscort);
            AllSpawnedVehicles.Add(PoliceEscort);
            PoliceEscort.IsPersistent = true;
            PoliceEscort.IsEngineOn = true;
            PoliceEscort.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            PoliceEscort.IsSirenOn = true;
            Limo.IsSirenOn = true;
            Limo.IsSirenSilent = true;
            PoliceEscort.IsSirenSilent = true;
            EscortDriver = PoliceEscort.CreateRandomDriver();
            AllBankHeistEntities.Add(EscortDriver);
            AllSpawnedPeds.Add(EscortDriver);
            EscortDriver.IsPersistent = true;
            EscortDriver.BlockPermanentEvents = true;
            LimoDriver = Limo.CreateRandomDriver();
            AllBankHeistEntities.Add(LimoDriver);
            AllSpawnedPeds.Add(LimoDriver);
            LimoDriver.IsPersistent = true;
            LimoDriver.BlockPermanentEvents = true;
            Limo.IsStolen = false;
            Persona.FromExistingPed(LimoDriver).ELicenseState = ELicenseState.Valid;
            Persona.FromExistingPed(LimoDriver).Wanted = false;
        }
    }

}
