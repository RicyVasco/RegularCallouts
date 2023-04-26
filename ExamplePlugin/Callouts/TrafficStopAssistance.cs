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
    [CalloutInfo("Traffic Stop Assistance", CalloutProbability.Medium)]
    public class TrafficStopAssistance : Callout
    {
        private Ped Driver;
        private Vehicle AccidentCar;
        private Ped BackupCop;
        private Vehicle CopCar;
        private bool PerformingPullover;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private Vector3 SpawnPoint;
        private string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private bool CalloutRunning;
        private Blip SpawnBlip;

        private List<string> DialogWithBackupCop = new List<string>
        {
            "~g~Cop:~s~ Glad you arrived so quickly.",
            "~b~You:~s~ Of course. What's happening?",
            "~g~Cop:~s~ I saw this person driving quite erratically, I wanted backup in case they try anything funny.",
            "~b~You:~s~ Sure go ahead I'll cover you.",
            "~g~Cop~s~ Actually, could you go first? I'm new here and I'm still a bit nervous."
        };
        private int DialogWithBackupCopIndex;
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(400, 650));
            CalloutMessage = "Traffic Stop Assistance";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_OFFICER_IN_NEED_OF_ASSISTANCE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
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
                    CreateSpeedZone();
                    GameFiber.Yield();
                    SpawnAccidentStuff();
                    GameFiber.Yield();
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (Game.LocalPlayer.Character.DistanceTo(BackupCop) <= 15f && !PerformingPullover && Game.LocalPlayer.Character.IsInAnyPoliceVehicle)
                        {
                            Functions.StartPulloverOnParkedVehicle(AccidentCar, true, true);
                            PerformingPullover = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(BackupCop) <= 8f && Game.LocalPlayer.Character.IsOnFoot && SpawnBlip.Exists())
                        {
                            BackupCop.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion();
                            BackupCop.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                            SpawnBlip.Delete();
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(BackupCop) <= 2f && !SpawnBlip.Exists() && DialogWithBackupCopIndex != DialogWithBackupCop.Count)
                        {
                            BackupCop.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithBackupCopIndex < DialogWithBackupCop.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithBackupCop[DialogWithBackupCopIndex]);
                                    DialogWithBackupCopIndex++;
                                }
                                if (DialogWithBackupCopIndex == DialogWithBackupCop.Count)
                                {

                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Game.LogTrivial("Regular Callout Error:"+ e);
                }
        });
        }
        public override void End()
        {
            CalloutRunning = false;
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            foreach (Entity e in AllCalloutEntities)
            {
                if (e.Exists())
                {
                    e.Dismiss();
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
            AccidentCar = new Vehicle("primo", SpawnPoint, Utils.VehicleNodeHeading(SpawnPoint));
            AccidentCar.Position = AccidentCar.GetOffsetPositionRight(3f);
            AccidentCar.IsPersistent = true;
            AccidentCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
            AllCalloutEntities.Add(AccidentCar);
            Driver = AccidentCar.CreateRandomDriver();
            Driver.IsPersistent = true;
            AllCalloutEntities.Add(Driver);
            if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
            {
                UBPoliceStuff();
            }
            else
            {
                CopCar = new Vehicle("police", AccidentCar.GetOffsetPositionFront(-10f), AccidentCar.Heading);
                CopCar.IsSirenOn = true;
                CopCar.IsSirenSilent = true;
                CopCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
                CopCar.IsPersistent = true;
                AllCalloutEntities.Add(CopCar);
                BackupCop = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), CopCar.AbovePosition, 0f);
                BackupCop.WarpIntoVehicle(CopCar, -1);
                BackupCop.IsPersistent = true;
                AllCalloutEntities.Add(BackupCop);
            }
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
                        if (AllCalloutEntities.Contains(veh))
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
                                        if (!AllCalloutEntities.Contains(veh))
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

        private void UBPoliceStuff()
        {
            CopCar = UltimateBackup.API.Functions.getLocalPatrolUnit(AccidentCar.GetOffsetPositionFront(-10f), 0).Item1;
            CopCar.Heading = AccidentCar.Heading;
            CopCar.Position = AccidentCar.GetOffsetPositionFront(-10f);
            CopCar.IsSirenOn = true;
            CopCar.IsSirenSilent = true;
            CopCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
            CopCar.IsPersistent = true;
            AllCalloutEntities.Add(CopCar);
            BackupCop = UltimateBackup.API.Functions.getLocalPatrolPed(CopCar.AbovePosition, 0f);
            BackupCop.WarpIntoVehicle(CopCar, -1);
            BackupCop.IsPersistent = true;
            AllCalloutEntities.Add(BackupCop);
        }
    }

}
