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
    [CalloutInfo("Animal involved in Car Accident", CalloutProbability.Medium)]
    public class AnimalCarCrash : Callout
    {
        Random rand = new Random();
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Ped CarOwner;
        private Vehicle AccidentCar;
        private Vehicle Car;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private bool ReportSchreiben;
        private bool CalloutAnfahrt;
        private Vector3 SpawnPoint;
        private float AccidentCarHeading;
        private Vector3 AccidentCarSpawn;
        private float CarHeading;
        private Blip OwnerBlip;
        private bool PolizeiReport;
        private bool Fotos;
        private Vector3 SpawnPoint5;
        private Blip PolizeiAutoBlip;
        private Vector3 CarSpawn;
        private Blip OwnerBlip2;
        private Rage.Object Camera;
        private Rage.Object Report;
        private bool ReportErstellt;
        private bool MitOwnerReden;
        private bool FotoHinweis;
        private int FotoIndex;
        private bool OwnerMarked;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private Vector3 SpawnPoint8;
        private int WaitCount;
        private Vector3 SpawnPoint9;
        private Vector3 SpawnPoint10;
        private Vector3 SpawnPoint11;
        private Ped Animal;
        private Vector3 SpawnPoint12;
        private Vector3 SpawnPoint13;
        private Vector3 SpawnPoint14;
        private Vector3 SpawnPosition;
        private bool FotosGemacht;
        private bool SpawnAnimal;
        private string CalloutLocation;
        private Blip SpawnBlip;
        string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "BALLER", "ALPHA", "SURGE" };
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
                    if (Vector3.Distance(Game.LocalPlayer.Character.Position, SpawnPoint) > 250f)
                    {
                        break;
                    }
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }

            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f, 500f));
            CalloutMessage = "Parking Accident";
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
                    CarOwner = new Ped(SpawnPoint);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            CarOwner.IsPersistent = true; CarOwner.BlockPermanentEvents = true;
            while (true)
            {
                if (!Camera.Exists())
                {
                    Camera = new Rage.Object("prop_ing_camera_01", Game.LocalPlayer.Character.Position);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            Camera.IsPersistent = true; Camera.IsVisible = false;
            while (true)
            {
                if (!Report.Exists())
                {
                    Report = new Rage.Object("prop_cd_paper_pile1", Game.LocalPlayer.Character.Position);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            Report.IsPersistent = true; Report.IsVisible = false;
            Report.AttachTo(Game.LocalPlayer.Character, 71, new Vector3(0.200000018f, 0.0500000007f, -0.0300000049f), new Rotator(140.000015f, 0f, 20f));
            Camera.AttachTo(Game.LocalPlayer.Character, 42, new Vector3(0.183000192f, 0.0759999752f, 0.162000149f), new Rotator(-19.9999981f, 119.999992f, 9.99999905f));
            Functions.SetVehicleOwnerName(Car, Functions.GetPersonaForPed(CarOwner).FullName);
            SpawnBlip = new Blip(SpawnPoint, 60f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ The caller is waiting for you at the ~y~Location~s~. Respond Code 3.");
            Game.DisplayHelp("You can press ~r~" + Settings.EndCalloutKey + "~s~ anytime to end the callout.");
            CarOwner.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            CalloutAnfahrt = true;
            FotoIndex = 0;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutAnfahrt)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (SpawnAnimal == false)
                    {
                        CarOwner.WarpIntoVehicle(Car, -1);
                        GameFiber.Sleep(500);
                        CarOwner.Tasks.CruiseWithVehicle(30f);
                        GameFiber.Sleep(5000);
                        Car.IsPositionFrozen = true;
                        CarOwner.Tasks.LeaveVehicle(Car, LeaveVehicleFlags.LeaveDoorOpen);
                        Car.IsPositionFrozen = false;

                        while (true)
                        {
                            if (!Animal.Exists())
                            {
                                Animal = new Ped("a_c_husky", Car.GetOffsetPositionFront(3f), 0f);
                                break;
                            }
                            GameFiber.Yield();
                        }
                        Animal.IsPersistent = true; Animal.BlockPermanentEvents = true;
                        Animal.Kill();
                        Utils.DeformFront(Car, 50f, 150f);
                        Car.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                        Car.IsEngineOn = true;
                        CarOwner.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                        SpawnAnimal = true;
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(Car) <= 20f && OwnerMarked == false)
                    {
                        OwnerBlip = CarOwner.AttachBlip();
                        OwnerBlip.Color = Color.Red;
                        SpawnBlip.Delete();
                        OwnerMarked = true;
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(CarOwner) <= 5f)
                    {
                        CarOwner.Tasks.Clear();
                        Utils.TurnToFaceEntity(CarOwner, Game.LocalPlayer.Character, -1);
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(CarOwner) <= 2f && MitOwnerReden == false)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            if (DialogWithOwnerIndex < DialogWithOwner.Count)
                            {
                                Game.DisplaySubtitle(DialogWithOwner[DialogWithOwnerIndex]);
                                DialogWithOwnerIndex++;
                            }
                            if (DialogWithOwnerIndex == DialogWithOwner.Count)
                            {
                                OwnerBlip.Delete();
                                Game.DisplayHelp("Take ~p~Pictures~s~ using ~y~" + Settings.InteractionKey + "~s~ and write a report for the Vehicle Owner");
                                Fotos = true;
                                MitOwnerReden = true;
                            }
                        }
                    }
                    if (Fotos == true)
                    {
                        if (Game.IsKeyDown(Settings.InteractionKey))
                        {
                            Camera.IsVisible = true;
                            Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("anim@mp_player_intincarphotographylow@ds@"), "idle_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                            GameFiber.Sleep(5000);
                            Camera.IsVisible = false;
                            FotosGemacht = true;
                            FotoHinweis = true;
                            FotoIndex++;
                        }
                    }
                    if (FotosGemacht == true)
                    {
                        if (FotoHinweis == true && FotoIndex == 1)
                        {
                            Game.DisplayHelp("If you have all necessary informations go back to your vehicle to fill out the ~p~report.");
                            PolizeiAutoBlip = Game.LocalPlayer.Character.LastVehicle.AttachBlip();
                            PolizeiAutoBlip.Color = Color.Purple;
                            ReportSchreiben = true;
                            break;
                        }
                    }
                }
                while (ReportSchreiben)
                {
                    GameFiber.Yield();
                    GameEnd();
                    CalloutAnfahrt = false;
                    Utils.TurnToFaceEntity(CarOwner, Game.LocalPlayer.Character, -1);
                    if (Fotos == true)
                    {
                        if (Game.IsKeyDown(Settings.InteractionKey))
                        {
                            Camera.IsVisible = true;
                            Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("anim@mp_player_intincarphotographylow@ds@"), "idle_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                            GameFiber.Sleep(5000);
                            Camera.IsVisible = false;
                        }
                    }
                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        if (PolizeiAutoBlip.Exists()) PolizeiAutoBlip.Delete();
                        Report.IsVisible = true;
                        ReportErstellt = true;
                    }
                    if (ReportErstellt == true && Fotos == true)
                    {
                        FotosGemacht = false;
                        Game.DisplayHelp("Give the Carowner the report.");
                        Fotos = false;
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(CarOwner) <= 2f && ReportErstellt == true)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to hand over the report.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplaySubtitle("~b~You:~s~ Here is the report for you. Everything else should arrive via mail in a few days. Have a nice day!");
                            Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_common"), "givetake2_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                            CarOwner.Tasks.PlayAnimation(new AnimationDictionary("mp_common"), "givetake2_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                            GameFiber.Sleep(1500);
                            Report.IsVisible = false;
                            CarOwner.Tasks.EnterVehicle(Car, -1);
                            if (OwnerBlip2.Exists()) OwnerBlip2.Delete();
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                        }
                    }
                }
        });
        }
        public override void End()
        {
            CalloutAnfahrt = false;
            ReportSchreiben = false;
            if (CarOwner.Exists()) CarOwner.Dismiss();
            if (Animal.Exists()) Animal.Dismiss();
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (OwnerBlip.Exists()) OwnerBlip.Delete();
            if (OwnerBlip2.Exists()) OwnerBlip2.Delete();
            if (Camera.Exists()) Camera.Delete();
            if (Report.Exists()) Report.Delete();
            if (AccidentCar.Exists()) AccidentCar.Dismiss();
            if (PolizeiAutoBlip.Exists()) PolizeiAutoBlip.Delete();
            if (Car.Exists()) Car.Dismiss();
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

    }

}
