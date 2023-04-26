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
using System.Collections.Generic;
using System.Linq;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Person in Need of Assistance", CalloutProbability.Medium)]
    public class AssistanceParkRemastered : Callout
    {
        private bool CalloutRunning;
        private Ped Mutter;
        private Ped Tater;
        private Vehicle MutterAuto;
        private Vehicle TaterAuto;
        private Vector3 SpawnPoint;
        private Vector3 SpawnPointTater;
        private Vector3 SpawnPointTaterAuto;
        private float MutterAutoFloat;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private float TaterAutoFloat;
        private Blip MutterBlip;
        private Blip TaterAutoBlip;
        public EMuggingState state;
        public Entscheidung state2;
        private Vector3 TaterTeleport;
        int rTaterSpawn = new Random().Next(1, 3);
        private static string[] TaterAutoString = new string[] { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "LANDSTALKER", "ASTEROPE", "INGOT", "STANIER", "BUFFALO" };


        private List<string> dialogWithCop = new List<string>
        {
            "~b~Officer:~y~ *knocks on the window*~s~ You know that you can't park here right?",
            "~r~Driver of the parked car:~s~ I'm sorry I just needed to pee real quick."
        };
        private int dialogWithCopIndex;

        private List<string> DialogWithDriverAfterTow = new List<string>
        {
            "~b~Officer:~s~ We towed the vehicle, you can go now.",
            "~o~Caller:~s~ Thanks! Have a nice day!",
            "~b~Officer:~s~ You too!"
        };
        private int DialogWithDriverAfterTowIndex;

        private List<string> DialogAfterTater = new List<string>
        {
            "~b~Officer:~s~ Looks like the problem solved itself. Have a nice day",
            "~o~Caller:~s~ You too.",
        };
        private int DialogAfterTaterIndex;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(-300.4023f, 632.5836f, 175.4855f);
            SpawnPoint2 = new Vector3(1303.098f, -1707.675f, 54.62236f);
            SpawnPoint3 = new Vector3(-1614.514f, -374.9115f, 42.89611f);
            SpawnPoint4 = new Vector3(-305.9707f, 377.5796f, 109.8508f);
            SpawnPoint5 = new Vector3(-203.2566f, 6403.839f, 31.37434f);
            Vector3[] spawnpoints = new Vector3[]
                {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5
                };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                SpawnPointTaterAuto = new SpawnPoint(116.6544f, new Vector3(-307.6219f, 629.6758f, 175.0731f));
                MutterAutoFloat = 116.6544f;
                TaterAutoFloat = 62.27253f;
                TaterTeleport = new SpawnPoint(116.6544f, new Vector3(-318.444f, 636.1242f, 173.639f));
            }
            else if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                SpawnPointTaterAuto = new SpawnPoint(116.6544f, new Vector3(1306.746f, -1716.912f, 53.79569f));
                MutterAutoFloat = 200.997f;
                TaterAutoFloat = 113.6925f;
                TaterTeleport = new SpawnPoint(116.6544f, new Vector3(1305.296f, -1731.605f, 54.2662f));
            }
            else if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
                SpawnPointTaterAuto = new SpawnPoint(116.6544f, new Vector3(-1607.089f, -384.7516f, 42.66404f));
                MutterAutoFloat = 219.9386f;
                TaterAutoFloat = 108.5608f;
                TaterTeleport = new SpawnPoint(116.6544f, new Vector3(-1613.651f, -389.8118f, 42.4678f));
            }
            else if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
                SpawnPointTaterAuto = new SpawnPoint(116.6544f, new Vector3(-306.54f, 385.6664f, 109.6831f));
                MutterAutoFloat = 17.68228f;
                TaterAutoFloat = 288.595f;
                TaterTeleport = new SpawnPoint(116.6544f, new Vector3(-299.2961f, 387.3369f, 110.2941f));
            }
            else if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
                SpawnPointTaterAuto = new SpawnPoint(116.6544f, new Vector3(-210.3561f, 6414.208f, 30.9684f));
                MutterAutoFloat = 45.76071f;
                TaterAutoFloat = 319.7675f;
                TaterTeleport = new SpawnPoint(116.6544f, new Vector3(-203.4253f, 6423.033f, 31.44653f));
            }
            if (SpawnPoint.DistanceTo(Game.LocalPlayer.Character) <= 100f)
            {
                return false;
            }
            if (rTaterSpawn == 1)
            {
                state2 = Entscheidung.TaterKommt;
            }
            else if (rTaterSpawn == 2)
            {
                state2 = Entscheidung.KeinTater;
            }
            TaterAuto = new Vehicle(TaterAutoString[new Random(Guid.NewGuid().GetHashCode()).Next(TaterAutoString.Length)], SpawnPointTaterAuto, TaterAutoFloat);
            MutterAuto = new Vehicle(TaterAutoString[new Random(Guid.NewGuid().GetHashCode()).Next(TaterAutoString.Length)], SpawnPoint, MutterAutoFloat);
            Mutter = new Ped(MutterAuto.GetOffsetPosition(new Vector3(0, 3f, 0)));
            try
            {
                TaterAuto.IsPersistent = true;
                Mutter.IsPersistent = true;
                MutterAuto.IsPersistent = true;
                Mutter.BlockPermanentEvents = true;
                Persona MutterPersona = Functions.GetPersonaForPed(Mutter);
                Functions.SetVehicleOwnerName(MutterAuto, Functions.GetPersonaForPed(Mutter).FullName);
            }
            catch (Exception e)
            {
                CrashHandler(e);
            }
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            CalloutMessage = "Person in Need of Assistance";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            MutterBlip = Mutter.AttachBlip();
            MutterBlip.Color = Color.Orange;
            MutterBlip.EnableRoute(Color.Orange);
            state = EMuggingState.EnRoute;
            CalloutRunning = true;
            Main();
            return base.OnCalloutAccepted();
        }
        private void Main()
        {
            while (CalloutRunning)
            {
                GameFiber.Yield();
                if (state == EMuggingState.EnRoute)
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 15)
                    {
                        MutterBlip.DisableRoute();
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 2f)
                        {
                            Game.DisplayHelp("Press ~b~" + Stuff.Settings.DialogKey + "~s~ to speak.");
                            GameFiber.Wait(1);
                            while (true)
                            {
                                GameFiber.Wait(1);
                                if (Game.IsKeyDown(Stuff.Settings.DialogKey)) { break; }
                            }
                            Mutter.Face(Game.LocalPlayer.Character.Position);
                            Mutter.Tasks.PlayAnimation(new AnimationDictionary("mp_facial"), "mic_chatter", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                            Game.DisplaySubtitle("~o~Caller:~s~ This idiot parked right infront of my driveway, could you tow the car please so I can get to work?");
                            while (true)
                            {
                                GameFiber.Wait(1);
                                if (Game.IsKeyDown(Stuff.Settings.DialogKey)) { break; }
                            }
                            Mutter.Tasks.Clear();
                            Game.DisplaySubtitle("~b~Officer:~s~ Do you know for how long the car was standing here?");
                            while (true)
                            {
                                GameFiber.Wait(1);
                                if (Game.IsKeyDown(Stuff.Settings.DialogKey)) { break; }
                            }
                            Mutter.Tasks.PlayAnimation(new AnimationDictionary("mp_facial"), "mic_chatter", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                            Game.DisplaySubtitle("~o~Caller:~s~ No idea, does it matter? I just wanna go to work");
                            while (true)
                            {
                                GameFiber.Wait(1);
                                if (Game.IsKeyDown(Stuff.Settings.DialogKey)) { break; }
                            }
                            Mutter.Tasks.Clear();
                            Game.DisplaySubtitle("~b~Officer:~s~ I'll see what I can do");
                            while (true)
                            {
                                GameFiber.Wait(1);
                                if (Game.IsKeyDown(Stuff.Settings.DialogKey)) { break; }
                            }

                            MutterBlip.Delete();
                            TaterAutoBlip = TaterAuto.AttachBlip();
                            TaterAutoBlip.Color = Color.Red;
                            Game.DisplayHelp("Investigate the situation.");
                            state = EMuggingState.GoingToKind;

                        }
                    }
                }

                if (state == EMuggingState.GoingToKind)
                {

                    if (state2 == Entscheidung.KeinTater)
                    {
                        Tater = new Ped();
                        Tater.IsPersistent = true;
                        Tater.BlockPermanentEvents = true;
                        state2 = Entscheidung.TalkedwithTater;
                    }

                    else if (state2 == Entscheidung.TaterKommt)
                    {
                        Tater = new Ped(TaterTeleport);
                        Tater.IsPositionFrozen = false;
                        Tater.IsPersistent = true;
                        Tater.BlockPermanentEvents = true;
                        Tater.WarpIntoVehicle(TaterAuto, -1);
                        Game.DisplaySubtitle("~r~Driver of the parked car:~s~ Sorry I will drive it away right now!");
                        state2 = Entscheidung.TalkingwithTater;

                    }

                    if (Game.LocalPlayer.Character.Position.DistanceTo(Tater) <= 2f && state2 == Entscheidung.TalkingwithTater)
                    {
                        while (!Game.IsKeyDown(Stuff.Settings.DialogKey)) GameFiber.Yield();
                        {
                            Game.HideHelp();
                            if (dialogWithCopIndex < dialogWithCop.Count)
                            {
                                Game.DisplaySubtitle(dialogWithCop[dialogWithCopIndex]);
                                dialogWithCopIndex++;
                            }
                            if (dialogWithCopIndex == dialogWithCop.Count)
                            {
                                TaterAutoBlip.Delete();
                                TaterAuto.IsPersistent = false;
                                MutterBlip = Mutter.AttachBlip();
                                MutterBlip.Color = Color.Orange;
                                MutterBlip.EnableRoute(Color.Orange);
                                state = EMuggingState.GoingToParent;
                                Game.DisplayHelp("Inform the Driver.");
                            }
                        }
                    }

                    else if (state2 == Entscheidung.TalkedwithTater)
                    {
                        while (true)
                        {
                            GameFiber.Wait(1);
                            Game.DisplayHelp("Press ~b~End~s~ if you are done investigating.");
                            if (Game.IsKeyDown(System.Windows.Forms.Keys.End))
                            {
                                TaterAutoBlip.Delete();
                                TaterAuto.IsPersistent = false;
                                MutterBlip = Mutter.AttachBlip();
                                MutterBlip.Color = Color.Orange;
                                MutterBlip.EnableRoute(Color.Orange);
                                state = EMuggingState.GoingToParent;
                                Game.DisplayHelp("Inform the Driver.");
                                break;
                            }
                        }

                    }

                }


                if (state == EMuggingState.GoingToParent)
                {
                    if (state == EMuggingState.GoingToParent && Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 10f)
                    {
                        MutterBlip.DisableRoute();
                    }

                    if (Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 2f)
                    {
                        while (!Game.IsKeyDown(Stuff.Settings.DialogKey)) GameFiber.Yield();
                        {
                            Game.HideHelp();

                            if (state2 == Entscheidung.TalkedwithTater)
                            {
                                if (DialogWithDriverAfterTowIndex < DialogWithDriverAfterTow.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithDriverAfterTow[DialogWithDriverAfterTowIndex]);
                                    DialogWithDriverAfterTowIndex++;
                                }
                                if (DialogWithDriverAfterTowIndex == DialogWithDriverAfterTow.Count)
                                {
                                    Mutter.Tasks.EnterVehicle(MutterAuto, -1);
                                    Mutter.Tasks.CruiseWithVehicle(30f);
                                    Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                                    this.End();
                                }
                            }
                            else if (state2 == Entscheidung.TalkingwithTater)
                            {
                                if (DialogAfterTaterIndex < DialogAfterTater.Count)
                                {
                                    Game.DisplaySubtitle(DialogAfterTater[DialogAfterTaterIndex]);
                                    DialogAfterTaterIndex++;
                                }
                                if (DialogAfterTaterIndex == DialogAfterTater.Count)
                                {
                                    Mutter.Tasks.EnterVehicle(MutterAuto, -1);
                                    Mutter.Tasks.CruiseWithVehicle(30f);
                                    Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                                    this.End();
                                }
                            }
                        }
                    }
                }
            }
            this.End();
        }
        public override void End()
        {
            base.End();
            if (MutterBlip.Exists()) MutterBlip.Delete();
            if (Mutter.Exists()) Mutter.Dismiss();
            if (MutterAuto.Exists()) MutterAuto.Dismiss();
            if (TaterAuto.Exists()) TaterAuto.Dismiss();
            if (TaterAutoBlip.Exists()) TaterAutoBlip.Delete();
            if (Tater.Exists()) Tater.Dismiss();
        }
        private void CrashHandler(Exception e)
        {
            Game.LogTrivial("Regular Callouts [AssistancePark] Crash: " + e);
            Game.DisplayNotification("Regular Callout ~r~Assistance Park~w~ has crashed. Please submit your ~b~Rage Log~w~ and a ~b~short description what happened~w~ in the Regular Callouts topic in LSPDFR.");
            this.End();
        }
    }

    public enum Entscheidung
    {
        TaterKommt,
        KeinTater,
        TalkingwithTater,
        TalkedwithTater
    }
    public enum EMuggingState
    {
        EnRoute,
        OnScene,
        GoingToKind,
        GoingToParent,
        GoingToTheEnd,
        DecisionMade
    }


}
