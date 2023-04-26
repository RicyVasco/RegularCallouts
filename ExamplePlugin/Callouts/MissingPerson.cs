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
    [CalloutInfo("Missing Person", CalloutProbability.Medium)]
    public class MissingPerson : Callout
    {
        private Ped Parent;
        private Ped Child;
        private Vehicle ParentCar;
        private Vector3 InteriorSpawnPoint;
        private Vector3 InteriorSpawnPointParent;
        private Blip Turblip;
        private Vector3 SpawnPointChildPhoto;
        private float InteriorParentHeading;
        private bool CalloutArrived;
        private string Geschlecht;
        private string Pronoun;
        private Vector3 PictureLocation;
        private bool SpokenTo;
        private Blip Childblip;
        private bool BackToParent;
        private string Pronoun2;
        private Ped PhotoPed;
        private bool KidAlive;
        private bool KidDead;
        private Blip SearchArea;
        private bool UnfallAngekommen;
        private bool Angeklingelt;
        private bool FotoAngucken;
        private float ChildPhotoHeading;
        private bool TextMessage;
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private Vector3 SpawnPoint;
        private bool TextAnzeigen;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private Vector3 SpawnPoint8;
        private Vector3 SpawnPoint9;
        private bool Fotoangeguckt;
        private bool ChildArrested;
        private int StartTimer;
        private bool Unfall;
        private bool Exit;
        private Vector3 CameraLocation;
        private string Vorname;
        string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "BALLER", "ALPHA", "SURGE" };
        private int DialogWithParentIndex;
        private List<string> DialogMitParentAlive = new List<string>
        {
            "~b~You:~s~ Hey again, we found your kid. He's heading to the hospital right now. (1/3)",
            "~y~Parent:~s~ Thank you so much Officer! Now if you'd excuse me, I'm heading there too now. (2/3)",
            "~b~You:~s~ Have a nice day. (3/3)"
        };
        private int DialogMitParentAliveIndex;
        private List<string> DialogMitParentDead = new List<string>
        {
            "~b~You:~s~ Hey again. Your kid has been in a car accident... he didn't make it. (1/2)",
            "~y~Parent:~s~ I'd prefer to be alone right now. (2/2)"
        };
        private int DialogMitParentDeadIndex;
        private List<string> DialogMitParentCuffed = new List<string>
        {
            "~b~You:~s~ Hello again. We found your kid. He was in a car accident and we arrested him as of now. (1/2)",
            "~b~You:~s~ Teach him a lesson for wrecking my car! (2/2)"
        };
        private int DialogMitParentAliveCuffedIndex;

        public override bool OnBeforeCalloutDisplayed()
        {

            SpawnPoint1 = new Vector3(-160.992996f, 160.925507f, 81.7058334f);
            SpawnPoint2 = new Vector3(-205.9891f, -7.424248f, 56.62296f);
            SpawnPoint3 = new Vector3(-760.7298f, -919.5276f, 19.00941f);
            SpawnPoint4 = new Vector3(-1308.187f, 449.3423f, 100.9697f);
            SpawnPoint5 = new Vector3(152.4456f, -1823.721f, 27.86866f);
            SpawnPoint6 = new Vector3(1060.748f, -378.3654f, 68.23117f);
            SpawnPoint7 = new Vector3(1845.85f, 3914.543f, 33.46114f);
            SpawnPoint8 = new Vector3(1060.748f, -378.3654f, 68.23117f);
            SpawnPoint9 = new Vector3(1274.755f, -1721.644f, 54.65525f);
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
                SpawnPoint9
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
            }
            if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
            }
            if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
            }
            if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
            }
            if (closestspawnpoint == SpawnPoint6)
            {
                SpawnPoint = SpawnPoint6;
            }
            if (closestspawnpoint == SpawnPoint7)
            {
                SpawnPoint = SpawnPoint7;
            }
            if (closestspawnpoint == SpawnPoint8)
            {
                SpawnPoint = SpawnPoint8;
            }
            if (closestspawnpoint == SpawnPoint9)
            {
                SpawnPoint = SpawnPoint9;
            }
            InteriorSpawnPointParent = new Vector3(259.168396f, -995.701599f, -99.0227432f);
            InteriorParentHeading = 158.248077f;
            InteriorSpawnPoint = new Vector3(264.9987f, -1000.504f, -99.00864f);
            SpawnPointChildPhoto = new Vector3(264.535919f, -995.967102f, -99.0129242f);
            ChildPhotoHeading = -178.770691f;
            PictureLocation = new Vector3(261.3747f, -1002.596f, -99.00864f);
            CameraLocation = new Vector3(264.713684f, -998.105957f, -99.0119324f);
            CalloutMessage = "Missing Person";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Parent = new Ped(InteriorSpawnPointParent, InteriorParentHeading)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
            Parent.Tasks.PlayAnimation(new AnimationDictionary("anim@amb@business@cfm@cfm_machine_no_work@"), "sleep_cycle_v2_operator", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            Child = new Ped(SpawnPointChildPhoto, ChildPhotoHeading)
            {
                IsPersistent = true,
                BlockPermanentEvents = true,
                IsVisible = false
            };
            Turblip = new Blip(SpawnPoint);
            Game.DisplayHelp("You can press ~r~" + Settings.EndCalloutKey + "~s~ anytime to end the callout.");
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ The caller is waiting for you at the ~y~Location~s~. Respond Code 2.");
            Turblip.Color = Color.Yellow;
            StartTimer = 45;
            Turblip.EnableRoute(Color.Yellow);
            Persona MutterPersona = Functions.GetPersonaForPed(Parent);
            Persona CurrentKindPersona = Functions.GetPersonaForPed(Child);
            Persona newKindpersona = new Persona(CurrentKindPersona.Forename, MutterPersona.Surname, CurrentKindPersona.Gender);
            Functions.SetPersonaForPed(Child, newKindpersona);
            //Functions.SetVehicleOwnerName(ParentCar, Functions.GetPersonaForPed(Parent).FullName);
            if (CurrentKindPersona.Gender.ToString() == "Male" || CurrentKindPersona.Gender.ToString() == "male")
            {
                Geschlecht = "son";
                Pronoun = "him";
                Pronoun2 = "He";
                Vorname = CurrentKindPersona.Forename;
            }
            else if (CurrentKindPersona.Gender.ToString() == "Female" || CurrentKindPersona.Gender.ToString() == "female")
            {
                Geschlecht = "daughter";
                Pronoun = "her";
                Pronoun2 = "She";
                Vorname = CurrentKindPersona.Forename;
            }
            CalloutArrived = true;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {



        GameFiber.StartNew(delegate
            {
                while (CalloutArrived)
                {
                    GameFiber.Yield();
                    Anklingeln();
                    GameEnd();
                    if (Angeklingelt)
                    {
                        GameFiber.Sleep(3000);
                        Turblip.Delete();
                        Game.FadeScreenOut(500);
                        Child.IsPositionFrozen = true; Child.IsCollisionEnabled = false;
                        GameFiber.Sleep(1000);
                        Game.LocalPlayer.Character.SetPositionWithSnap(InteriorSpawnPoint);
                        Game.FadeScreenIn(600);
                        CalloutArrived = false;
                        break;
                    }
                }
                while (Angeklingelt)
                {
                    GameFiber.Yield();
                    GameEnd();
                    CalloutArrived = false;
                    if (Parent.DistanceTo(Game.LocalPlayer.Character) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak with the parent.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            switch(DialogWithParentIndex)
                            {
                                case 0:
                                    Game.DisplaySubtitle("~b~You:~s~ Good Day, you called the police? (1/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 1:
                                    Game.DisplaySubtitle("~y~Parent:~s~ Yes *sob* my "+ Geschlecht +" is missing, my car aswell. (2/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 2:
                                    Game.DisplaySubtitle("~b~You:~s~ Can you tell me when they went missing? (3/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 3:
                                    Game.DisplaySubtitle("~y~Parent:~s~ I dont know. I just came home when I noticed it. (4/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 4:
                                    Game.DisplaySubtitle("~b~You:~s~ Do you have any more information about the car and your "+ Geschlecht +" ? (5/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 5:
                                    Game.DisplaySubtitle("~y~Parent:~s~ Of course. My " + Geschlecht + " is called "+ Vorname +", there is a photo in the room behind you.(6/7)");
                                    DialogWithParentIndex++;
                                    break;
                                case 6:
                                    Game.DisplaySubtitle("~b~You:~s~ Ok thanks for the info, I will take a look. (7/7)");
                                    FotoAngucken = true;
                                    DialogWithParentIndex++;
                                    break;
                            }
                        }
                        if (DialogWithParentIndex == 7)
                        {
                            Angeklingelt = false;
                            break;
                        }
                    }
                }
                while (FotoAngucken)
                {
                    GameFiber.Yield();
                    GameEnd();
                    Angeklingelt = false;
                    if (Game.LocalPlayer.Character.Position.DistanceTo(PictureLocation) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to look at the picture.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.LocalPlayer.HasControl = false;
                            Game.FadeScreenOut(1500, true);
                            PhotoPed = Child.Clone();
                            PhotoPed.Face(CameraLocation);
                            Child.IsVisible = false;
                            Game.LocalPlayer.Character.IsVisible = false;
                            Parent.IsVisible = false;
                            Camera cam = new Camera(false);
                            cam.Position = CameraLocation;
                            cam.Active = true;
                            GameFiber.Sleep(2000);
                            Game.FadeScreenIn(1500, true);
                            GameFiber.Sleep(6500);
                            Game.FadeScreenOut(1500, true);
                            cam.Active = false;
                            PhotoPed.Delete();
                            TextAnzeigen = false;
                            Game.LocalPlayer.Character.IsVisible = true;
                            Parent.IsVisible = true;
                            Game.LocalPlayer.HasControl = true;
                            GameFiber.Sleep(2000);
                            Game.FadeScreenIn(1500, true);
                            Fotoangeguckt = true;
                            TextMessage = true;
                            break;
                            
                        }
                    }
                }
                while (Fotoangeguckt)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if(TextMessage == true)
                    {
                        Game.DisplaySubtitle("~y~Parent:~s~ Please find my " + Geschlecht + " Officer");
                        Functions.PlayScannerAudio("ATTENTION_ALL_UNITS WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT UNITS_RESPOND_CODE_03");
                        Game.DisplayNotification("~b~Attention to responding Unit:~s~ We've got a car crash at this ~r~Location~s~. Respond Code 3 immediately");
                        TextMessage = false;
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(InteriorSpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                            while (true)
                            {
                                if (!ParentCar.Exists())
                                {
                                    ParentCar = new Vehicle(Titles[new Random().Next(0, Titles.Length)], World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f)));
                                    Game.LogTrivial("Car Spawned");
                                    break;
                                }
                                GameFiber.Yield();
                            }
                            ParentCar.IsPersistent = true;
                            ParentCar.Deform(ParentCar.Position, 20f, 30f);
                            ParentCar.EngineHealth = 50f;
                            ParentCar.FuelTankHealth = 650f;
                            ParentCar.DirtLevel = 3;
                            ParentCar.AlarmTimeLeft = new TimeSpan(20, 0, 0);
                            Functions.SetVehicleOwnerName(ParentCar, Functions.GetPersonaForPed(Parent).FullName);
                            Utils.Damage(ParentCar, 50f, 400f);
                            Game.FadeScreenIn(600);
                            Child.IsCollisionEnabled = true; Child.IsVisible = true; Child.IsPositionFrozen = false;
                            Child.WarpIntoVehicle(ParentCar, -1);
                            SearchArea = new Blip(ParentCar.Position, 80f);
                            SearchArea.Color = Color.Yellow;
                            SearchArea.Alpha = 0.5f;
                            SearchArea.EnableRoute(Color.Yellow);
                            Unfall = true;
                            Fotoangeguckt = false;
                            break;
                        }
                    }
                }
                while (Unfall)
                {
                    GameFiber.Yield();
                    GameEnd();
                    TextAnzeigen = true;
                    Game.FrameRender += DrawCCTVText;
                    GameFiber.Sleep(1000);
                    StartTimer--;
                    if (StartTimer == 0)
                    {
                        TextAnzeigen = false;
                        Child.Kill();
                    }
                    if(Game.LocalPlayer.Character.DistanceTo(Child) <= 10f)
                    {
                        TextAnzeigen = false;
                        if (SearchArea.Exists()) SearchArea.Delete();
                        Childblip = Child.AttachBlip();
                        Childblip.Color = Color.Red;
                        UnfallAngekommen = true;
                        Unfall = false;
                        break;
                    }
                }
                while (UnfallAngekommen)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Child) <= 2f && Child.IsAlive && SpokenTo == false)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Driver.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplaySubtitle("~r~Driver:~s~ Uhhhh my head.... I can't think straight... (1/1)");
                            GameFiber.Sleep(3000);
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to respond back to the parent or ~b~" + Settings.InteractionKey + "~s~ to let Dispatch handle it.");
                            SpokenTo = true;
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Child) <= 2f && Child.IsDead && SpokenTo == false)
                    {
                        Game.DisplayNotification("~b~Attention to Dispatch:~s~ The Driver is DOA");
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to respond back to the parent or ~b~" + Settings.InteractionKey + "~s~ to let Dispatch handle it.");
                        SpokenTo = true;
                    }
                    if (SpokenTo == true)
                    {
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            if(Child.IsCuffed)
                            {
                                ChildArrested = true;
                            }
                            if (Child.IsAlive)
                            {
                                KidAlive = true;
                            }
                            if (Child.IsDead)
                            {
                                KidDead = true;
                            }
                            if (Childblip.Exists()) Childblip.Delete();
                            Turblip = new Blip(SpawnPoint);
                            Turblip.Color = Color.Yellow;
                            Turblip.EnableRoute(Color.Yellow);
                            BackToParent = true;
                            UnfallAngekommen = false;
                            break;

                        }
                        else if (Game.IsKeyDown(Settings.InteractionKey))
                        {
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                            UnfallAngekommen = false;
                            break;
                        }
                    }
                }
                while (BackToParent)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplayNotification("You rang the door!");
                            GameFiber.Sleep(1500);
                            Turblip.Delete();
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(InteriorSpawnPoint);
                            Game.FadeScreenIn(600);
                            Parent.Tasks.Clear();
                            Parent.Face(Game.LocalPlayer.Character);

                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Parent) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak with the Parent.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {   
                            if(KidDead)
                            {
                                if (DialogMitParentDeadIndex < DialogMitParentDead.Count)
                                {
                                    Game.DisplaySubtitle(DialogMitParentDead[DialogMitParentDeadIndex]);
                                    DialogMitParentDeadIndex++;
                                }
                                if (DialogMitParentDeadIndex == DialogMitParentDead.Count)
                                {
                                    Game.DisplayHelp("You can leave through the Front Door");
                                    Exit = true;
                                    BackToParent = false;
                                    break;
                                }
                            }
                            if (KidAlive && ChildArrested == false)
                            {
                                if (DialogMitParentAliveIndex < DialogMitParentAlive.Count)
                                {
                                    Game.DisplaySubtitle(DialogMitParentAlive[DialogMitParentAliveIndex]);
                                    DialogMitParentAliveIndex++;
                                }
                                if (DialogMitParentAliveIndex == DialogMitParentAlive.Count)
                                {
                                    Game.DisplayHelp("You can leave through the Front Door");
                                    Exit = true;
                                    BackToParent = false;
                                    break;
                                }
                            }
                            if(KidAlive && ChildArrested == true)
                            {
                                if (DialogMitParentAliveCuffedIndex < DialogMitParentCuffed.Count)
                                {
                                    Game.DisplaySubtitle(DialogMitParentCuffed[DialogMitParentAliveCuffedIndex]);
                                    DialogMitParentAliveCuffedIndex++;
                                }
                                if (DialogMitParentAliveCuffedIndex == DialogMitParentCuffed.Count)
                                {
                                    Game.DisplayHelp("You can leave through the Front Door");
                                    Exit = true;
                                    BackToParent = false;
                                    break;
                                }
                            }

                        }
                    }

                }
                while (Exit)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(InteriorSpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                            Game.FadeScreenIn(600);
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                            Exit = false;
                            break;
                        }
                    }
                }

            });
        }
        public override void End()
        {
            base.End();
            CalloutArrived = false;
            Angeklingelt = false;
            FotoAngucken = false;
            Fotoangeguckt = false;
            Unfall = false;
            UnfallAngekommen = false;
            BackToParent = false;
            Exit = false;
            if (Turblip.Exists()) Turblip.Delete();
            if (Parent.Exists()) Parent.Dismiss();
            if (Child.Exists()) Child.Dismiss();
            if (ParentCar.Exists()) ParentCar.Dismiss();
            if (SearchArea.Exists()) SearchArea.Delete();
            if (Childblip.Exists()) Childblip.Delete();
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
        private void Anklingeln()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f && Angeklingelt == false)
                {
                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        Game.DisplayNotification("You rang the door!");
                        Angeklingelt = true;
                    }
                }

            });
        }

        private void DrawCCTVText(System.Object sender, Rage.GraphicsEventArgs e)
        {

            if (TextAnzeigen)
            {
                Rectangle drawRect = new Rectangle(0, 0, 300, 130);
                //e.Graphics.DrawRectangle(drawRect, Color.FromArgb(100, Color.Black));

                e.Graphics.DrawText(StartTimer.ToString()+ " seconds left.", "Aharoni Bold", 35.0f, new PointF(1, 86), Color.White, drawRect);
            }
            else
            {
                Game.FrameRender -= DrawCCTVText;
            }
        }
    }

}
