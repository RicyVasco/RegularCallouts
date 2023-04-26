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
    [CalloutInfo("Suspicious Vehicle", CalloutProbability.Low)]
    public class SuspiciousVehicle : Callout
    {
        Random rand = new Random();
        private Ped CarOwner;
        private Vehicle AccidentCar;
        private Vehicle Car;
        private bool ReportSchreiben;
        private bool CalloutAnfahrt;
        private Vector3 SpawnPoint;
        private bool TrafficCheck;
        private Rage.Object IDCard;
        private int WaitCount;
        private Ped Animal;
        private bool FotosGemacht;
        private TaskSequence FBIZeug;
        private bool SpawnAnimal;
        private bool IDValid;
        private bool SetTasks;
        private Blip SpawnBlip;
        private string[] Titles = new string[] {"DUKES", "BALLER", "BALLER2", "BISON", "BISON2", "BJXL", "CAVALCADE", "CHEETAH", "COGCABRIO", "ASEA", "ADDER", "FELON", "FELON2", "ZENTORNO",
        "WARRENER", "RAPIDGT", "INTRUDER", "FELTZER2", "FQ2", "RANCHERXL", "REBEL", "SCHWARZER", "COQUETTE", "CARBONIZZARE", "EMPEROR", "SULTAN", "EXEMPLAR", "MASSACRO",
        "DOMINATOR", "ASTEROPE", "PRAIRIE", "NINEF", "WASHINGTON", "CHINO", "CASCO", "INFERNUS", "ZTYPE", "DILETTANTE", "VIRGO", "F620", "PRIMO", "FBI"};
        private List<string> DialogWithOwner = new List<string>
        {
            "~b~You:~s~ Good morning Sir, can you tell me what happened? (1/5)",
            "~r~Driver:~s~ This dog just ran out o the street, I had no chance to react anymore. I'm sorry I'm still a bit shocked. (2/5)",
            "~b~You:~s~ Do you need an ambulance? (3/5)",
            "~r~Driver:~s~ No thanks. (4/5)",
            "~b~You:~s~ I'll have a look around, stay here for a moment (5/5)"
        };
        private int DialogWithOwnerIndex;

        public override bool OnBeforeCalloutDisplayed()
        {

            while (true)
            {
                SpawnPoint = (World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 400f)));
                {
                    if (Game.LocalPlayer.Character.DistanceTo2D(SpawnPoint) > 250f)
                    {
                        break;
                    }
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }

            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 500f));
            CalloutMessage = "Suspicious Vehicle";
            CalloutPosition = SpawnPoint;
            AddMinimumDistanceCheck(20f, SpawnPoint);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            while (true)
            {
                if (!Car.Exists())
                {
                    Car = new Vehicle(Titles[new Random().Next(0, Titles.Length)], SpawnPoint);
                    break;
                }
                Car.IsStolen = false;
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            Car.IsPersistent = true;
            while (true)
            {
                if (!CarOwner.Exists())
                {
                    CarOwner = Car.CreateRandomDriver();
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            CarOwner.IsPersistent = true; CarOwner.BlockPermanentEvents = true;
            Persona.FromExistingPed(CarOwner).Wanted = false; Persona.FromExistingPed(CarOwner).ELicenseState = ELicenseState.Valid;
            WaitCount = 0;
            while (true)
            {
                if (!IDCard.Exists())
                {
                    IDCard = new Rage.Object("p_ld_id_card_01", SpawnPoint);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            IDCard.IsPersistent = true;
            Functions.SetVehicleOwnerName(Car, "Government owned");
            CarOwner.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.FollowTraffic);
            IDCard.AttachTo(CarOwner, 50, new Vector3(0.139999986f, 0.0199999996f, -0.0399999991f), new Rotator(0f, 0f, 180f));
            Functions.AddPedContraband(CarOwner, ContrabandType.Narcotics, "Legitimate police badge");
            SpawnBlip = Car.AttachBlip();
            SpawnBlip.Color = Color.Red;
            SpawnBlip.EnableRoute(Color.Red);
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ Pullover and control the reported ~r~Vehicle~s~. Respond Code 3.");
            Game.DisplayHelp("You can press ~r~" + Settings.EndCalloutKey + "~s~ anytime to end the callout.");
            Functions.SetPedResistanceChance(CarOwner, 0);
            CalloutAnfahrt = true;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            GameFiber.StartNew(delegate
            {
                try
                { 
                while (CalloutAnfahrt)
                {
                    GameFiber.Yield();
                    GameEnd();
                    EndingConditions();
                    if(Functions.IsPlayerPerformingPullover())
                    {
                        if(!Game.LocalPlayer.Character.IsInAnyVehicle(false) && TrafficCheck == false)
                        {
                            if(SetTasks == false)
                            {
                                FBIZeug = new TaskSequence(CarOwner);
                                FBIZeug.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen);
                                FBIZeug.Tasks.PlayAnimation("melee@holster", "unholster", 1f, AnimationFlags.SecondaryTask);
                                FBIZeug.Tasks.AchieveHeading(Game.LocalPlayer.Character.Heading);
                                FBIZeug.Tasks.PlayAnimation("anim@heists@humane_labs@finale@keycards", "ped_b_enter_loop", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                SetTasks = true;
                            }
                            FBIZeug.Execute();
                            FBIZeug.Dispose();
                            GameFiber.Sleep(4000);
                            Utils.TurnToFaceEntity(CarOwner, Game.LocalPlayer.Character);
                            Game.DisplaySubtitle("~r~Driver:~s~ I'm an undercover officer!");
                            TrafficCheck = true;
                        }
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(CarOwner) <= 2f && IDCard.IsVisible)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Functions.DisplayPedId(CarOwner, true);
                            Game.DisplayNotification("~y~Received:~s~ Police ID");
                            Game.DisplayHelp("Go back to your vehicle the ID");
                            CarOwner.Tasks.ClearImmediately();
                            IDCard.IsVisible = false;
                        }
                    }
                    if(IDCard.IsVisible == false && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && IDValid == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to check with Dispatch.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplaySubtitle("~b~You:~s~ Dispatch I have a identification card that needs to be checked. Owner is " + Functions.GetPersonaForPed(CarOwner).FullName.ToString());
                            GameFiber.Sleep(500);
                            Game.DisplayNotification("~b~Dispatch:~s~ Checking ID...");
                            GameFiber.Sleep(2000);
                            Game.DisplayNotification("~b~Dispatch:~s~ ID is valid.");
                            IDValid = true;
                        }
                    }
                    if(Game.LocalPlayer.Character.DistanceTo(CarOwner) <= 2f && IDValid == true)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplaySubtitle("~r~Driver:~s~ Took long enough. Good day to you.");
                                Functions.ForceEndCurrentPullover();
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                        }
                    }
                }
                }
                catch(Exception e)
                {
                    Game.LogTrivial("Crash averted");
                }
        });
        }
        public override void End()
        {
            CalloutAnfahrt = false;
            if (CarOwner.Exists()) CarOwner.Dismiss();
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (Car.Exists()) Car.Dismiss();
            if (IDCard.Exists()) IDCard.Delete();
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
        private void EndingConditions()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (CarOwner.IsDead)
                {
                    Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                    this.End();
                }
                if (CarOwner.IsCuffed)
                {
                    Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                    this.End();
                }

            });
        }

    }

}
