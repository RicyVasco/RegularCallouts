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
    [CalloutInfo("Parking Accident", CalloutProbability.High)]
    public class ParkedCarCrash : Callout
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
        private Vector3 SpawnPoint9;
        private Vector3 SpawnPoint10;
        private Vector3 SpawnPoint11;
        private Vector3 SpawnPoint12;
        private Vector3 SpawnPoint13;
        private Vector3 SpawnPoint14;
        private bool FotosGemacht;
        private string CalloutLocation;
        private int WaitCount;
        private Blip SpawnBlip;
        string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "BALLER", "ALPHA", "SURGE" };
        private List<string> DialogWithOwner = new List<string>
        {
            "~r~Driver:~s~ Good morning Officer. (1/4)",
            "~b~You:~s~ Good day, are you the caller? (2/4)",
            "~r~Driver:~s~ Yes Sir, I wanted to park here but accidently bumped into the other car. (3/4)",
            "~b~You:~s~ Alright give me a moment and I will handle it. (4/4)"
        };
        private int DialogWithOwnerIndex;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(238.914841f, -1753.13562f, 29.1002903f); //LS Gang
            SpawnPoint2 = new Vector3(-1006.541f, -1631.618f, 4.539834f); //LS Strand
            SpawnPoint3 = new Vector3(386.3749f, -743.505f, 29.29405f); //LS Downtown
            SpawnPoint4 = new Vector3(1289.379f, -1554.119f, 48.73795f); //LS Rechts
            SpawnPoint5 = new Vector3(86.61806f, -213.7926f, 54.49158f); //LS ObenMitte
            SpawnPoint6 = new Vector3(-1327.69f, -390.0517f, 36.55363f); //LS ObenLinks
            SpawnPoint7 = new Vector3(570.5524f, 2722.158f, 42.06024f); //KartenMitte
            SpawnPoint8 = new Vector3(2009.834f, 3058.432f, 47.05028f); //ObenDart
            SpawnPoint9 = new Vector3(1885.496f, 3717.229f, 32.88697f); //Sandy
            SpawnPoint10 = new Vector3(1705.143f, 3745.178f, 33.76025f); //Sandy2
            SpawnPoint11 = new Vector3(2767.242f, 3455.069f, 55.72393f); //SuperStore
            SpawnPoint12 = new Vector3(1691.459f, 4786.97f, 41.92151f); //Grape
            SpawnPoint13 = new Vector3(-17.88832f, 6509.208f, 31.26519f); //Pale
            SpawnPoint14 = new Vector3(-1012.851f, -2583.876f, 18.47306f); //Airport
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5,
                SpawnPoint6,
                SpawnPoint7,
                SpawnPoint8,
                SpawnPoint9,
                SpawnPoint10,
                SpawnPoint11,
                SpawnPoint12,
                SpawnPoint13,
                SpawnPoint14
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                //LS Gang
                SpawnPoint = SpawnPoint1;
                AccidentCarSpawn = new Vector3(236.3415f, -1759.042f, 28.69617f);
                AccidentCarHeading = 138.4537f;
                CarSpawn = new Vector3(237.1722f, -1753.627f, 29.0549f);
                CarHeading = 176.8215f;
            }
            else if (closestspawnpoint == SpawnPoint2)
            {
                //LS Strand
                SpawnPoint = SpawnPoint2;
                AccidentCarSpawn = new Vector3(-1006.835f, -1636.842f, 3.722049f);
                AccidentCarHeading = 237.8412f;
                CarSpawn = new Vector3(-1008.294f, -1632.736f, 4.508426f);
                CarHeading = 216.1371f;
            }
            else if (closestspawnpoint == SpawnPoint3)
            {
                //LS Downtown
                SpawnPoint = SpawnPoint3;
                AccidentCarSpawn = new Vector3(383.9141f, -739.8918f, 29.27163f);
                AccidentCarHeading = 2.067432f;
                CarSpawn = new Vector3(384.7565f, -744.7258f, 28.86152f);
                CarHeading = 220.5362f;
            }
            else if (closestspawnpoint == SpawnPoint4)
            {
                //LS Rechts
                SpawnPoint = SpawnPoint4;
                AccidentCarSpawn = new Vector3(1289.91f, -1560.089f, 49.00296f);
                AccidentCarHeading = 206.8631f;
                CarSpawn = new Vector3(1288.477f, -1555.47f, 48.79589f);
                CarHeading = 212.5249f;
            }
            else if (closestspawnpoint == SpawnPoint5)
            {
                //LS ObenMitte
                SpawnPoint = SpawnPoint5;
                AccidentCarSpawn = new Vector3(82.14388f, -213.6144f, 54.05947f);
                AccidentCarHeading = 159.8649f;
                CarSpawn = new Vector3(85.00805f, -212.7251f, 54.47105f);
                CarHeading = 141.6703f;
            }
            else if (closestspawnpoint == SpawnPoint6)
            {
                //LS ObenLinks
                SpawnPoint = SpawnPoint6;
                AccidentCarSpawn = new Vector3(-1325.726f, -395.1076f, 36.01535f);
                AccidentCarHeading = 214.4163f;
                CarSpawn = new Vector3(-1325.54f, -390.4454f, 36.49993f);
                CarHeading = 342.1625f;
            }
            else if (closestspawnpoint == SpawnPoint7)
            {
                //Kartenmitte
                SpawnPoint = SpawnPoint7;
                AccidentCarSpawn = new Vector3(565.801f, 2719.521f, 41.6295f);
                AccidentCarHeading = 182.2413f;
                CarSpawn = new Vector3(568.4031f, 2722.677f, 42.03871f);
                CarHeading = 159.6774f;
            }
            else if (closestspawnpoint == SpawnPoint8)
            {
                //ObenDart
                SpawnPoint = SpawnPoint8;
                AccidentCarSpawn = new Vector3(2011.994f, 3055.318f, 46.61463f);
                AccidentCarHeading = 235.6193f;
                CarSpawn = new Vector3(2012.556f, 3059.25f, 47.02796f);
                CarHeading = 2.141935f;
            }
            else if (closestspawnpoint == SpawnPoint9)
            {
                //Sandy
                SpawnPoint = SpawnPoint9;
                AccidentCarSpawn = new Vector3(1888.423f, 3717.145f, 32.40438f);
                AccidentCarHeading = 296.5898f;
                CarSpawn = new Vector3(1885.635f, 3718.921f, 32.84385f);
                CarHeading = 91.87832f;
            }
            else if (closestspawnpoint == SpawnPoint10)
            {
                //Sandy2
                SpawnPoint = SpawnPoint10;
                AccidentCarSpawn = new Vector3(1701.578f, 3747.095f, 33.57132f);
                AccidentCarHeading = 42.03089f;
                CarSpawn = new Vector3(1704.854f, 3747.705f, 33.90715f);
                CarHeading = 69.37116f;
            }
            else if (closestspawnpoint == SpawnPoint11)
            {
                //SuperStore
                SpawnPoint = SpawnPoint11;
                AccidentCarSpawn = new Vector3(2763.042f, 3458.629f, 55.32046f);
                AccidentCarHeading = 248.1167f;
                CarSpawn = new Vector3(2766.947f, 3456.703f, 55.68961f);
                CarHeading = 67.85841f;
            }
            else if (closestspawnpoint == SpawnPoint12)
            {
                //Grapeseed
                SpawnPoint = SpawnPoint12;
                AccidentCarSpawn = new Vector3(1691.825f, 4782.104f, 41.49334f);
                AccidentCarHeading = 267.6883f;
                CarSpawn = new Vector3(1691.466f, 4784.878f, 41.89993f);
                CarHeading = 242.277f;
            }
            else if (closestspawnpoint == SpawnPoint13)
            {
                //Pale
                SpawnPoint = SpawnPoint13;
                AccidentCarSpawn = new Vector3(-19.84936f, 6505.512f, 30.84944f);
                AccidentCarHeading = 312.8018f;
                CarSpawn = new Vector3(-16.25008f, 6508.329f, 31.27121f);
                CarHeading = 315.2898f;
            }
            else if (closestspawnpoint == SpawnPoint14)
            {
                //Airport
                SpawnPoint = SpawnPoint14;
                AccidentCarSpawn = new Vector3(-1011.849f, -2579.405f, 17.49283f);
                AccidentCarHeading = 59.11716f;
                CarSpawn = new Vector3(-1011.587f, -2582.54f, 18.2425f);
                CarHeading = 38.08036f;
            }

            CalloutMessage = "Parking Accident";
            CalloutPosition = SpawnPoint;
            AddMinimumDistanceCheck(20f, SpawnPoint);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            while (true)
            {
                if (!AccidentCar.Exists())
                {
                    AccidentCar = new Vehicle(Titles[new Random().Next(0, Titles.Length)], AccidentCarSpawn, AccidentCarHeading);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            while (true)
            {
                if (!Car.Exists())
                {
                    Car = new Vehicle(Titles[new Random().Next(0, Titles.Length)], CarSpawn, CarHeading);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
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
            WaitCount = 0;
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
            Camera.IsPersistent = true; Camera.IsVisible = false;
            WaitCount = 0;
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
            Report.IsPersistent = true; Report.IsVisible = false;
            Report.AttachTo(Game.LocalPlayer.Character, 71, new Vector3(0.200000018f, 0.0500000007f, -0.0300000049f), new Rotator(140.000015f, 0f, 20f));
            Camera.AttachTo(Game.LocalPlayer.Character, 42, new Vector3(0.183000192f, 0.0759999752f, 0.162000149f), new Rotator(-19.9999981f, 119.999992f, 9.99999905f));
            CarOwner.IsPersistent = true; CarOwner.BlockPermanentEvents = true; AccidentCar.IsPersistent = true; Car.IsPersistent = true;
            Functions.SetVehicleOwnerName(Car, Functions.GetPersonaForPed(CarOwner).FullName);
            SpawnBlip = new Blip(SpawnPoint, 60f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ The caller is waiting for you at the ~y~Location~s~. Respond Code 2.");
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
                    if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 20f && OwnerMarked == false)
                    {
                        OwnerBlip = CarOwner.AttachBlip();
                        OwnerBlip.Color = Color.Red;
                        SpawnBlip.Delete();
                        OwnerMarked = true;
                    }
                    CarOwner.Face(Game.LocalPlayer.Character);
                    if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 5f)
                    {
                        CarOwner.Tasks.Clear();
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
