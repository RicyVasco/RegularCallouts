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
using System.Media;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;
using RegularCallouts.Stuff;
using System.Linq;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Domestic Disturbance", CalloutProbability.Medium)]
    public class DomesticDisturbance : Callout
    {
        private Vector3 SpawnPoint;
        private Blip SpawnBlip;
        private bool CalloutAnfahrt;
        private String CalloutMessageS;
        private Ped TodSchrei;
        bool PedSpawnSchrei;
        private Ped Victim;
        private Vector3 MutterInteriorSpawn;
        private Ped Attacker;
        bool SchreiGehort;
        private Ped HomeOwner;
        bool HomeOwnerAbortedGespawnt;
        private bool DomesticDisturbanceCall;
        private bool AbortedCall;
        private Blip VictimBlip;
        private Ped AttackerClone;
        bool DomesticInteriorEntscheidung2;
        bool DomesticCallGeklingelt;
        bool HomeOwnerGesprochenAborted;
        bool VictimGesprochenArrested;
        bool VictimGesprochenKilled;
        bool AbortInteriorLeicheSpawn;
        private Vector3 SpawnPunktLeiche;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        bool Chase;
        private Blip ChaseBlip;
        bool DomesticHomeownerGespawnt;
        bool DomesticInteriorChase;
        int DomesticTurAntwortOutcome = new Random().Next(1, 4);
        private bool AbortedCallGeklingelt;
        private Vector3 SpawnInterior;
        bool DomesticKeineAntwortTurInterior;
        int rSpawnLocations = new Random().Next(1, 7);
        bool DomesticKeineAntwortTur;
        int rDomesticTurReaction = new Random().Next(1, 3);
        int rDomesticInteriorWhathappens = new Random().Next(1, 3);
        int rCallDisrupted = new Random().Next(1, 3);
        bool DomesticAttackPlayer;
        int r911DisruptTurReaction = new Random().Next(1, 3);
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private bool InvestigateHouse911;
        bool DomesticHomeownerInterior;
        private bool SchreiGehortInterior;
        bool DomesticInteriorEntscheidung;
        bool DomesticAttackerGesprochen;
        bool DomesticVictimGesprochen;
        private LHandle pursuit;
        bool MitPaarGesprochen;
        int rDeadBodyAbortedChance = new Random().Next(1, 3);
        bool HomeOwnerAbortedInterior;
        private List<string> DialogWithHomeownerAbortedAccident = new List<string>
        {
            "~b~You:~s~ Good morning. You've called 911 but didn't answer. Is everything alright? (1/2)",
            "~o~Homeowner:~s~ Oh yeah I must've called it by accident. I'm sorry. (2/2)",
        };
        private int DialogWithHomeownerAbortedAccidentIndex;

        private List<string> DialogAttackerDoorEnter= new List<string>
        {
            "~b~You:~s~ We've received a call about a domestic disturbance. Can I come in for a moment? (1/2)",
            "~o~Homeowner:~s~ Sure come in. (2/2)",
        };
        private int DialogAttackerDoorEnterIndex;

        private List<string> DialogAttackerDoorAttack = new List<string>
        {
            "~b~You:~s~ We've received a call about a domestic disturbance. Can I come in for a moment? (1/2)",
            "~r~Homeowner:~s~ Die you fucking pig just like my wife! (2/2)",
        };
        private int DialogAttackerDoorAttackIndex;

        private List<string> DialogDomesticVictim = new List<string>
        {
            "~b~You:~s~ Can you tell me what happened? (1/4)",
            "~o~Victim:~s~ He's/She's abusing and hurting me constantly. (2/4)",
            "~r~Abuser:~s~ Well that's only because you are driving me mad all the time! (3/4)",
            "~b~You:~s~ Alright, that's enough. You'll have to leave the house for now. I'll escort you out. (4/4)",
        };
        private int DialogDomesticVictimIndex;


        private List<string> DialogWithVictimAbortedArrested = new List<string>
        {
            "~o~Victim:~s~ Thank you! He/She has been punching and attacking me for month now? (1/3)",
            "~b~You:~s~ Don't worry about it, he/she won't be coming back anytime soon. I'll have an ambulance on the way to check you out (2/3)",
            "~o~Victim:~s~ Thank you so much!! (3/3)",
        };
        private int DialogWithVictimAbortedArrestedIndex;
        private List<string> DialogWithVictimAbortedKilled = new List<string>
        {
            "~o~Victim:~s~ *sobbing* Oh my god... (1/2)",
            "~b~You:~s~ There was no other way. I'll have assistance head to you shortly. I'm sorry. (2/2)",
        };
        private int DialogWithVictimAbortedKilledIndex;


        public override bool OnBeforeCalloutDisplayed()
        {

            SpawnPoint1 = new Vector3(-775.017822f, 313.000946f, 84.698288f);
            SpawnPoint2 = new Vector3(1832.609f, 3868.611f, 34.29747f);
            SpawnPoint3 = new Vector3(-238.5714f, 6423.432f, 31.45727f);
            SpawnPoint4 = new Vector3(479.8957f, -1735.843f, 29.15103f);
            SpawnPoint5 = new Vector3(-595.642f, 393.0718f, 101.8819f);
            SpawnPoint6 = new Vector3(1369.079f, -1734.96f, 65.64688f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5,
                SpawnPoint6
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();

            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            else if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            else if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            else if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            else if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            else if (closestspawnpoint == SpawnPoint6)
            {
                SpawnPoint = SpawnPoint6;
                SpawnInterior = new Vector3(264.9987f, -1000.504f, -99.00864f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                SpawnPunktLeiche = new Vector3(255.2264f, -1000.973f, -99.0097f);
            }
            switch (rCallDisrupted)
            {
                case 1:
                    CalloutMessageS = "Aborted 911 call";
                    AbortedCall = true;
                    break;
                case 2:
                    CalloutMessageS = "Domestic Disturbance";
                    DomesticDisturbanceCall = true;
                    break;
                default:
                    CalloutMessageS = "Domestic Disturbance";
                    DomesticDisturbanceCall = true;
                    break;
            }
            CalloutMessage = CalloutMessageS;
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS UNITS_RESPOND_CODE_03", SpawnPoint);          


            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {

            SpawnBlip = new Blip(SpawnPoint, 20f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            CalloutAnfahrt = true;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
            GameFiber.StartNew(delegate
            {
                while(AbortedCall)
                {
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 20f && InvestigateHouse911 == false)
                    {
                        SpawnBlip.DisableRoute();
                        SpawnBlip.Delete();
                        SpawnBlip = new Blip(SpawnPoint);
                        SpawnBlip.Color = Color.Yellow;
                        InvestigateHouse911 = true;
                    }
                    if (InvestigateHouse911 == true)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                        {
                            Game.DisplayHelp("Press ~b~"+ Settings.DialogKey +"~s~ to ring the bell.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.DisplayNotification("You rang the door!");
                                GameFiber.Sleep(3000);
                                AbortedCallGeklingelt = true;
                                AbortedCall = false;
                            }
                        }
                    }
                }
                while (AbortedCallGeklingelt)
                {
                    GameFiber.Yield();
                    AbortedCall = false;
                    switch (r911DisruptTurReaction)
                    {
                        case 1:
                            TodSchrei = new Ped(SpawnPoint);
                            TodSchrei.IsVisible = false; TodSchrei.IsCollisionEnabled = false;
                            TodSchrei.Kill();
                            GameFiber.Sleep(1500);
                            if (TodSchrei.Exists()) TodSchrei.Delete();
                            SchreiGehort = true;
                            AbortedCallGeklingelt = false;
                            break;
                        case 2:
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            HomeOwner = new Ped(SpawnPoint);
                            HomeOwner.IsPersistent = true; HomeOwner.BlockPermanentEvents = true;
                            Utils.TurnToFaceEntity(HomeOwner, Game.LocalPlayer.Character);
                            Game.FadeScreenIn(2000);
                            HomeOwnerAbortedGespawnt = true;
                            AbortedCallGeklingelt = false;
                            break;
                        default:
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            HomeOwner = new Ped(SpawnPoint);
                            HomeOwner.IsPersistent = true; HomeOwner.BlockPermanentEvents = true;
                            Utils.TurnToFaceEntity(HomeOwner, Game.LocalPlayer.Character);
                            Game.FadeScreenIn(2000);
                            HomeOwnerAbortedGespawnt = true;
                            AbortedCallGeklingelt = false;
                            break;
                    }
                }
                while (HomeOwnerAbortedGespawnt)
                {
                    GameFiber.Yield();
                    AbortedCallGeklingelt = false;
                    if (Game.LocalPlayer.Character.Position.DistanceTo(HomeOwner) <= 2f && HomeOwnerGesprochenAborted == false)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Homeowner.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            if (DialogWithHomeownerAbortedAccidentIndex < DialogWithHomeownerAbortedAccident.Count)
                            {
                                Game.DisplaySubtitle(DialogWithHomeownerAbortedAccident[DialogWithHomeownerAbortedAccidentIndex]);
                                DialogWithHomeownerAbortedAccidentIndex++;
                            }
                            if (DialogWithHomeownerAbortedAccidentIndex == DialogWithHomeownerAbortedAccident.Count)
                            {
                                HomeOwnerGesprochenAborted = true;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(HomeOwner) <= 2f && HomeOwnerGesprochenAborted == true)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to leave or press ~b~" + Settings.InteractionKey + "~s~ to ask to enter the house.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.DisplaySubtitle("~b~You:~s~ It's ok, make sure it doesn't happen again. Have a good day. (1/1)");
                            GameFiber.Sleep(2000);
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            if (HomeOwner.Exists()) HomeOwner.Delete();
                            Game.FadeScreenIn(2000);
                            this.End();
                            HomeOwnerAbortedGespawnt = false;
                        }
                        else if (Game.IsKeyDown(Settings.InteractionKey))
                        {
                            Game.DisplaySubtitle("~b~You:~s~ I would like to make sure you are ok, let's go inside for a minute. (1/1)");
                            GameFiber.Sleep(2000);
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                            HomeOwner.SetPositionWithSnap(MutterInteriorSpawn);
                            Utils.TurnToFaceEntity(HomeOwner, Game.LocalPlayer.Character);
                            Game.FadeScreenIn(2000);
                            Game.DisplaySubtitle("~b~You:~s~ I will look around, hang tight for a moment. (1/1)");
                            Game.DisplayHelp("When you are finished investigating, go to the ~y~ Door ~s~ to exit.");
                            SpawnBlip = new Blip(SpawnInterior);
                            SpawnBlip.Color = Color.Yellow;
                            HomeOwnerAbortedInterior = true;
                            AbortInteriorLeicheSpawn = true;
                            HomeOwnerAbortedGespawnt = false;
                        }
                    }
                }
                while (HomeOwnerAbortedInterior)
                {
                    GameFiber.Yield();
                    NachDraussenGehen();
                    HomeOwnerAbortedGespawnt = false;
                    if (AbortInteriorLeicheSpawn == true)
                    {
                        switch(rDeadBodyAbortedChance)
                        {
                            case 1:
                                AbortInteriorLeicheSpawn = false;
                                break;
                            case 2:
                                TodSchrei = new Ped(SpawnPunktLeiche);
                                TodSchrei.IsPersistent = true; TodSchrei.BlockPermanentEvents = true;
                                TodSchrei.Kill();
                                GameFiber.Sleep(500);
                                TodSchrei.IsPositionFrozen = true;
                                AbortInteriorLeicheSpawn = false;
                                break;
                            default:
                                AbortInteriorLeicheSpawn = false;
                                break;
                        }
                    }

                }
                while (SchreiGehort)
                {
                    GameFiber.Yield();
                    AbortedCallGeklingelt = false;
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to force entry.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                            Game.FadeScreenIn(2000);
                            SchreiGehortInterior = true;
                            SchreiGehort = false;
                        }
                    }

                }
                while (SchreiGehortInterior)
                {
                    GameFiber.Yield();
                    SchreiGehort = false;
                    if (PedSpawnSchrei == false)
                    {
                        if (SpawnBlip.Exists()) SpawnBlip.Delete();
                        Victim = new Ped(MutterInteriorSpawn);
                        Victim.IsPersistent = true; Victim.BlockPermanentEvents = true;
                        Attacker = new Ped(MutterInteriorSpawn);
                        Attacker.IsPersistent = true; Attacker.BlockPermanentEvents = true;
                        Victim.KeepTasks = true;
                        Attacker.Inventory.GiveNewWeapon("WEAPON_KNIFE", 1, true);
                        Utils.TurnToFaceEntity(Attacker, Victim, -1);
                        Attacker.Tasks.Clear();
                        Attacker.Tasks.PlayAnimation(new AnimationDictionary("switch@trevor@jerking_off"), "trev_jerking_off_loop", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                        Victim.Tasks.Cower(100000);
                        PedSpawnSchrei = true;
                    }
                    if (Attacker.IsCuffed)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenArrested == false)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to talk to the Victim.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {

                                if (DialogWithVictimAbortedArrestedIndex < DialogWithVictimAbortedArrested.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithVictimAbortedArrested[DialogWithVictimAbortedArrestedIndex]);
                                    DialogWithVictimAbortedArrestedIndex++;
                                }
                                if (DialogWithVictimAbortedArrestedIndex == DialogWithVictimAbortedArrested.Count)
                                {
                                    GameFiber.Sleep(2000);
                                    VictimBlip = Victim.AttachBlip();
                                    VictimBlip.Color = Color.Orange;
                                    VictimGesprochenArrested = true;
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenArrested == true)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                                Attacker.SetPositionWithSnap(SpawnPoint);
                                Game.FadeScreenIn(2000);
                                this.End();
                                SchreiGehortInterior = false;
                            }
                        }
                    }
                    if (Attacker.IsDead)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenKilled == false)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to talk to the Victim.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {

                                if (DialogWithVictimAbortedKilledIndex < DialogWithVictimAbortedKilled.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithVictimAbortedKilled[DialogWithVictimAbortedKilledIndex]);
                                    DialogWithVictimAbortedKilledIndex++;
                                }
                                if (DialogWithVictimAbortedKilledIndex == DialogWithVictimAbortedKilled.Count)
                                {
                                    GameFiber.Sleep(2000);
                                    VictimBlip = Victim.AttachBlip();
                                    VictimBlip.Color = Color.Orange;
                                    VictimGesprochenKilled = true;
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenKilled == true)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                                Attacker.SetPositionWithSnap(SpawnPoint);
                                Game.FadeScreenIn(2000);
                                this.End();
                                SchreiGehortInterior = false;
                            }
                        }
                    }
                }


                while (DomesticDisturbanceCall)
                {
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 20f && InvestigateHouse911 == false)
                    {
                        SpawnBlip.DisableRoute();
                        SpawnBlip.Delete();
                        SpawnBlip = new Blip(SpawnPoint);
                        SpawnBlip.Color = Color.Yellow;
                        InvestigateHouse911 = true;
                    }
                    if (InvestigateHouse911 == true)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.DisplayNotification("You rang the door!");
                                GameFiber.Sleep(3000);
                                DomesticCallGeklingelt = true;
                                DomesticDisturbanceCall = false;
                            }
                        }
                    }
                }
                while (DomesticCallGeklingelt)
                {
                    GameFiber.Yield();
                    DomesticDisturbanceCall = false;
                    switch(rDomesticTurReaction)
                    {
                        case 1:
                            DomesticKeineAntwortTur = true;
                            DomesticCallGeklingelt = false;
                            break;
                        case 2:
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Attacker = new Ped(SpawnPoint);
                            Attacker.IsPersistent = true; Attacker.BlockPermanentEvents = true;
                            Utils.TurnToFaceEntity(Attacker, Game.LocalPlayer.Character);
                            Game.FadeScreenIn(2000);
                            DomesticHomeownerGespawnt = true;
                            DomesticCallGeklingelt = false;
                            break;
                        default:
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Attacker = new Ped(SpawnPoint);
                            Attacker.IsPersistent = true; Attacker.BlockPermanentEvents = true;
                            Utils.TurnToFaceEntity(Attacker, Game.LocalPlayer.Character);
                            Game.FadeScreenIn(2000);
                            DomesticHomeownerGespawnt = true;
                            DomesticCallGeklingelt = false;
                            break;
                    }
                }
                while (DomesticKeineAntwortTur)
                {
                    GameFiber.Yield();
                    DomesticCallGeklingelt = false;
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to force entry.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                            Game.FadeScreenIn(2000);
                            DomesticKeineAntwortTurInterior = true;
                            DomesticKeineAntwortTur = false;
                        }
                    }
                }
                while (DomesticKeineAntwortTurInterior)
                {
                    GameFiber.Yield();
                    DomesticKeineAntwortTur = false;
                    if (PedSpawnSchrei == false)
                    {
                        if (SpawnBlip.Exists()) SpawnBlip.Delete();
                        Victim = new Ped(MutterInteriorSpawn);
                        Victim.IsPersistent = true; Victim.BlockPermanentEvents = true;
                        Attacker = new Ped(MutterInteriorSpawn);
                        Attacker.IsPersistent = true; Attacker.BlockPermanentEvents = true;
                        Victim.KeepTasks = true;
                        Attacker.Inventory.GiveNewWeapon("WEAPON_KNIFE", 1, true);
                        Utils.TurnToFaceEntity(Attacker, Victim, -1);
                        Attacker.Tasks.Clear();
                        Attacker.Tasks.PlayAnimation(new AnimationDictionary("switch@trevor@jerking_off"), "trev_jerking_off_loop", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                        Victim.Tasks.Cower(100000);
                        PedSpawnSchrei = true;
                    }
                    if (Attacker.IsCuffed)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenArrested == false)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to talk to the Victim.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {

                                if (DialogWithVictimAbortedArrestedIndex < DialogWithVictimAbortedArrested.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithVictimAbortedArrested[DialogWithVictimAbortedArrestedIndex]);
                                    DialogWithVictimAbortedArrestedIndex++;
                                }
                                if (DialogWithVictimAbortedArrestedIndex == DialogWithVictimAbortedArrested.Count)
                                {
                                    GameFiber.Sleep(2000);
                                    VictimBlip = Victim.AttachBlip();
                                    VictimBlip.Color = Color.Orange;
                                    VictimGesprochenArrested = true;
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenArrested == true)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                                Attacker.SetPositionWithSnap(SpawnPoint);
                                Game.FadeScreenIn(2000);
                                this.End();
                                DomesticKeineAntwortTurInterior = false;
                            }
                        }
                    }
                    if (Attacker.IsDead)
                    {
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenKilled == false)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to talk to the Victim.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {

                                if (DialogWithVictimAbortedKilledIndex < DialogWithVictimAbortedKilled.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithVictimAbortedKilled[DialogWithVictimAbortedKilledIndex]);
                                    DialogWithVictimAbortedKilledIndex++;
                                }
                                if (DialogWithVictimAbortedKilledIndex == DialogWithVictimAbortedKilled.Count)
                                {
                                    GameFiber.Sleep(2000);
                                    VictimBlip = Victim.AttachBlip();
                                    VictimBlip.Color = Color.Orange;
                                    VictimGesprochenKilled = true;
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f && VictimGesprochenKilled == true)
                        {
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to exit.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                                Attacker.SetPositionWithSnap(SpawnPoint);
                                Game.FadeScreenIn(2000);
                                this.End();
                                DomesticKeineAntwortTurInterior = false;
                            }
                        }
                    }
                }
                while (DomesticHomeownerGespawnt)
                {
                    GameFiber.Yield();
                    DomesticCallGeklingelt = false;
                    switch(DomesticTurAntwortOutcome)
                    {
                        case 1:
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Attacker) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Homeowner.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogAttackerDoorAttackIndex < DialogAttackerDoorAttack.Count)
                                    {
                                        Game.DisplaySubtitle(DialogAttackerDoorAttack[DialogAttackerDoorAttackIndex]);
                                        DialogAttackerDoorAttackIndex++;
                                    }
                                    if (DialogAttackerDoorAttackIndex == DialogAttackerDoorAttack.Count)
                                    {
                                        Attacker.Inventory.GiveNewWeapon("WEAPON_KNIFE", 1, true);
                                        Attacker.RelationshipGroup = "ATTACKER";
                                        Game.SetRelationshipBetweenRelationshipGroups("ATTACKER", "PLAYER", Relationship.Hate);
                                        Game.SetRelationshipBetweenRelationshipGroups("ATTACKER", "COP", Relationship.Hate);
                                        Attacker.Tasks.ClearImmediately();
                                        Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
                                        DomesticAttackPlayer = true;
                                        DomesticHomeownerGespawnt = false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Attacker) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Homeowner.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogAttackerDoorEnterIndex < DialogAttackerDoorEnter.Count)
                                    {
                                        Game.DisplaySubtitle(DialogAttackerDoorEnter[DialogAttackerDoorEnterIndex]);
                                        DialogAttackerDoorEnterIndex++;
                                    }
                                    if (DialogAttackerDoorEnterIndex == DialogAttackerDoorEnter.Count)
                                    {
                                        SpawnBlip.Delete();
                                        GameFiber.Sleep(2000);
                                        Game.FadeScreenOut(500);
                                        GameFiber.Sleep(1000);
                                        Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                                        Attacker.SetPositionWithSnap(MutterInteriorSpawn);
                                        Game.FadeScreenIn(2000);
                                        Game.DisplayHelp("When you are finished investigating, go to the ~y~ Door ~s~ to exit.");
                                        SpawnBlip = new Blip(SpawnInterior);
                                        SpawnBlip.Color = Color.Yellow;
                                        DomesticHomeownerInterior = true;
                                        DomesticHomeownerGespawnt = false;
                                    }
                                }
                            }
                            break;
                        default:
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Attacker) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Homeowner.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogAttackerDoorAttackIndex < DialogAttackerDoorAttack.Count)
                                    {
                                        Game.DisplaySubtitle(DialogAttackerDoorAttack[DialogAttackerDoorAttackIndex]);
                                        DialogAttackerDoorAttackIndex++;
                                    }
                                    if (DialogAttackerDoorAttackIndex == DialogAttackerDoorAttack.Count)
                                    {
                                        Attacker.Inventory.GiveNewWeapon("WEAPON_KNIFE", 1, true);
                                        Attacker.RelationshipGroup = "ATTACKER";
                                        Game.SetRelationshipBetweenRelationshipGroups("ATTACKER", "PLAYER", Relationship.Hate);
                                        Game.SetRelationshipBetweenRelationshipGroups("ATTACKER", "COP", Relationship.Hate);
                                        Attacker.Tasks.ClearImmediately();
                                        DomesticAttackPlayer = true;
                                        DomesticHomeownerGespawnt = false;
                                    }
                                }
                            }
                            break;
                    }
                }
                while (DomesticHomeownerInterior)
                {
                    GameFiber.Yield();
                    DomesticHomeownerGespawnt = false;
                    NachDraussenGehen2();
                    if (DomesticInteriorEntscheidung == false)
                    {
                        switch (rDomesticInteriorWhathappens)
                        {
                            case 1:
                                Victim = new Ped(SpawnPunktLeiche);
                                Victim.IsPersistent = true; Victim.BlockPermanentEvents = true;
                                Victim.Kill();
                                GameFiber.Sleep(1500);
                                Victim.IsPositionFrozen = true;
                                DomesticInteriorEntscheidung = true;
                                DomesticInteriorEntscheidung2 = true;
                                break;
                            case 2:
                                Victim = new Ped(MutterInteriorSpawn);
                                Victim.IsPersistent = true; Victim.BlockPermanentEvents = true;
                                Utils.TurnToFaceEntity(Victim, Game.LocalPlayer.Character);
                                DomesticInteriorEntscheidung = true;
                                DomesticInteriorEntscheidung2 = true;
                                break;
                            default:
                                Victim = new Ped(MutterInteriorSpawn);
                                Victim.IsPersistent = true; Victim.BlockPermanentEvents = true;
                                DomesticInteriorEntscheidung = true;
                                DomesticInteriorEntscheidung2 = true;
                                break;
                        }
                    }
                    if (DomesticInteriorEntscheidung2 == true)
                    {
                        switch (rDomesticInteriorWhathappens)
                        {
                            case 1:
                                if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f)
                                {
                                    Game.DisplayHelp("The Suspect is fleeing, get him!");
                                    Game.DisplaySubtitle("~r~Suspect: ~s~ You'll never catch me!");
                                    Attacker.IsVisible = false;
                                    DomesticInteriorChase = true;
                                    DomesticHomeownerInterior = false;
                                }
                                break;
                            case 2:
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f || Game.LocalPlayer.Character.Position.DistanceTo(Attacker) <= 2f && MitPaarGesprochen == false)
                                {
                                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Victim.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if (DialogDomesticVictimIndex < DialogDomesticVictim.Count)
                                        {
                                            Game.DisplaySubtitle(DialogDomesticVictim[DialogDomesticVictimIndex]);
                                            DialogDomesticVictimIndex++;
                                        }
                                        if (DialogDomesticVictimIndex == DialogDomesticVictim.Count)
                                        {
                                            Game.DisplayHelp("You can leave through the front door ones finished.");
                                            MitPaarGesprochen = true;
                                            DomesticInteriorEntscheidung2 = false;
                                        }
                                    }
                                }
                                break;
                            default:
                                if (Game.LocalPlayer.Character.Position.DistanceTo(Victim) <= 2f || Game.LocalPlayer.Character.Position.DistanceTo(Attacker) <= 2f && MitPaarGesprochen == false)
                                {
                                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Victim.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if (DialogDomesticVictimIndex < DialogDomesticVictim.Count)
                                        {
                                            Game.DisplaySubtitle(DialogDomesticVictim[DialogDomesticVictimIndex]);
                                            DialogDomesticVictimIndex++;
                                        }
                                        if (DialogDomesticVictimIndex == DialogDomesticVictim.Count)
                                        {
                                            Game.DisplayHelp("You can leave through the front door ones finished.");
                                            MitPaarGesprochen = true;
                                            DomesticInteriorEntscheidung2 = false;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                while (DomesticInteriorChase)
                {
                    GameFiber.Yield();
                    DomesticHomeownerInterior = false;
                    NachDraussenGehenChase();
                    ChaseBlip = Attacker.AttachBlip();
                    ChaseBlip.Color = Color.Red;
                    Chase = true;
                    DomesticInteriorChase = false;
                }
                while (Chase)
                {
                    if (Attacker.IsCuffed || Attacker.IsDead)
                    {
                        this.End();
                        Chase = false;
                    }
                }
                while (DomesticAttackPlayer)
                {
                    GameFiber.Yield();
                    DomesticHomeownerGespawnt = false;
                    if (Attacker.IsDead || Attacker.IsCuffed)
                    {
                        this.End();
                        DomesticAttackPlayer = false;
                    }
                }
            });
        }
        public override void End()
        {
            AbortedCall = false;
            DomesticDisturbanceCall = false;
            HomeOwnerAbortedGespawnt = false;
            AbortedCallGeklingelt = false;
            SchreiGehort = false;
            SchreiGehortInterior = false;
            HomeOwnerAbortedInterior = false;
            DomesticCallGeklingelt = false;
            DomesticKeineAntwortTur = false;
            DomesticHomeownerGespawnt = false;
            DomesticKeineAntwortTurInterior = false;
            DomesticHomeownerInterior = false;
            DomesticInteriorChase = false;
            DomesticAttackPlayer = false;
            Chase = false;
            if (ChaseBlip.Exists()) ChaseBlip.Delete();
            if (HomeOwner.Exists()) HomeOwner.Dismiss();
            if (VictimBlip.Exists()) VictimBlip.Delete();
            if (Attacker.Exists()) Attacker.Dismiss();
            if (Victim.Exists()) Victim.Dismiss();
            if (TodSchrei.Exists()) TodSchrei.Dismiss();
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }

        private void NachDraussenGehen()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnInterior) <= 2f)
                {
                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to leave the house.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        Game.FadeScreenOut(500);
                        GameFiber.Sleep(1000);
                        if (HomeOwner.IsCuffed == true) HomeOwner.SetPositionWithSnap(SpawnPoint);
                        if (HomeOwner.IsDead == true) HomeOwner.SetPositionWithSnap(SpawnPoint);
                        Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                        Game.FadeScreenIn(2000);
                        this.End();
                    }
                }
            });

        }

        private void NachDraussenGehenChase()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnInterior) <= 2f)
                {
                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to leave the house.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        Game.FadeScreenOut(500);
                        GameFiber.Sleep(1000);
                        if (Attacker.Exists()) Attacker.SetPositionWithSnap(SpawnPoint); Attacker.IsVisible = true;
                        Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                        Game.FadeScreenIn(2000);
                        this.pursuit = Functions.CreatePursuit();
                        Functions.AddPedToPursuit(this.pursuit, Attacker);
                    }
                }
            });

        }

        private void NachDraussenGehen2()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnInterior) <= 2f)
                {
                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to leave the house.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        Game.FadeScreenOut(500);
                        GameFiber.Sleep(1000);
                        if (Attacker.Exists()) Attacker.SetPositionWithSnap(SpawnPoint);
                        if (Attacker.Exists())
                        Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                        Game.FadeScreenIn(2000);
                        this.End();
                    }
                }
            });

        }
    }
}
