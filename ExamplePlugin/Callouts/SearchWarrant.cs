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
using RAGENativeUI;
using RAGENativeUI.Elements;
using RegularCallouts.Stuff;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Search Warrant", CalloutProbability.Medium)]
    public class SearchWarrant : Callout
    {
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private Blip Turblip;
        private Vector3 SpawnPoint;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private Vector3 SpawnPoint8;
        private LHandle Pursuit;
        private Vector3 SpawnPoint9;
        private Vector3 InteriorSpawnPoint;
        private Vector3 SpawnPointChildPhoto;
        private bool CalloutArrived;
        private bool SearchWarrantPrinted;
        private Rage.Object SWarrant;
        private Ped Suspect;
        private bool Angeklingelt;
        private bool SuspectOpensDoor;
        private bool NixGefundenReden;
        private bool ImHaus;
        private string WarrantReason;
        private bool NixGefunden;
        private string SearchArea;
        private string SearchPersonResult;
        private Blip SuspectBlip;

        string[] Titles = { "a computer", "a mobile phone", "a crowbar", "credit cards", "a camera", "electronic devices" };
        string[] SearchBereich = { "Kitchen", "Living Room", "Bedroom", "Living+Bedroom", "Kitchen", "Bed+Kitchen", "AllRooms" };
        string[] SearchPerson = { "", "and the Suspect" };

        private List<string> DialogWithSuspectFirstTime = new List<string>
        {
            "~b~You:~s~ Hello Sir, I have Search Warrant for your Property. (1/3)",
            "~r~Suspect:~s~ A search warrant?! Can I see it first? (2/3)",
            "~b~You:~s~ Absolutely Sir. (3/3)"
        };
        private int DialogWithSuspectFirstTimeIndex;

        private List<string> DialogWithSuspectFoundEvidence = new List<string>
        {
            "~b~You:~s~ Do you wanna tell me what this is? (1/3)",
            "~r~Suspect:~s~ I'm not saying anything without my lawyer (2/3)",
            "~b~You:~s~ You will receive mail from us soon. Your lawyer will help you understand the next steps. Have a nice day. (3/3)"
        };
        private int DialogWithSuspectFoundEvidenceIndex;

        private Vector3 KitchenSpawn1;
        private Vector3 KitchenSpawn2;
        private bool TalkEvidencefound;
        private Vector3 BathSpawn1;
        private bool SusAttacks;
        private Vector3 BedSpawn1;
        private Vector3 BedSpawn2;
        private Vector3 LivingSpawn1;
        private Vector3 LivingSpawn2;
        private Vector3 LivingSpawn3;
        private Blip KitchenBlip1;
        private Blip KitchenBlip2;
        private Blip BathBlip1;
        private Blip BedBlip1;
        private Blip BedBlip2;
        private Blip LivingBlip1;
        private Blip LivingBlip2;
        private Blip LivingBlip3;
        private int WaitCount;
        private bool Kitchen1Searched;
        private bool Kitchen2Searched;
        private bool BathSearched;
        private bool Bed1Searched;
        private bool Bed2Searched;
        private bool Living1Searched;
        private bool Living2Searched;
        private bool Living3Searched;
        private bool HausDurchsuchen;
        private bool TatWaffeGefunden;
        private bool PedFlieht;
        private bool ArrestSuspect;

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
            InteriorSpawnPoint = new Vector3(264.9987f, -1000.504f, -99.00864f);
            SpawnPointChildPhoto = new Vector3(264.535919f, -995.967102f, -99.0129242f);
            KitchenSpawn1 = new Vector3(265.719574f, -996.071533f, -99.0048065f);
            KitchenSpawn2 = new Vector3(263.644165f, -995.815918f, -99.0121918f);
            LivingSpawn1 = new Vector3(257.15387f, -996.079468f, -99.0046921f);
            LivingSpawn2 = new Vector3(262.238953f, -996.441772f, -99.004776f);
            LivingSpawn3 = new Vector3(265.597565f, -999.370056f, -99.0048676f);
            BathSpawn1 = new Vector3(255.703506f, -1000.83307f, 99.0137787f);
            BedSpawn1 = new Vector3(262.657867f, -1002.9386f, -99.0085754f);
            BedSpawn2 = new Vector3(259.884979f, -1003.98663f, -99.0085678f);

            CalloutMessage = "Search Warrant";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            while (true)
            {
                if (!Suspect.Exists())
                {
                    Suspect = new Ped(InteriorSpawnPoint);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            while (true)
            {
                if (!SWarrant.Exists())
                {
                    SWarrant = new Rage.Object("prop_cd_paper_pile1", Game.LocalPlayer.Character.Position);
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            SWarrant.AttachTo(Game.LocalPlayer.Character, 71, new Vector3(0.200000018f, 0.0500000007f, -0.0300000049f), new Rotator(140.000015f, 0f, 20f));
            SWarrant.IsVisible = false; Suspect.IsPersistent = true; Suspect.BlockPermanentEvents = true;
            Game.DisplayHelp("You can press ~r~" + Settings.EndCalloutKey + "~s~ anytime to end the callout.");
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ You have to enforce the ~r~Search Issue~s~ at the following ~y~Location~s~. Respond Code 2.");
            Turblip = new Blip(SpawnPoint);
            Turblip.Color = Color.Yellow;
            Turblip.EnableRoute(Color.Yellow);
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
                    PrintWarrant();
                    GameEnd();
                    if(SearchWarrantPrinted && Game.LocalPlayer.Character.IsInAnyVehicle(false) == false)
                    {
                        SWarrant.IsVisible = true;
                    }
                    else
                    {
                        SWarrant.IsVisible = false;
                    }
                    if(Angeklingelt == true)
                    {
                        GameFiber.Sleep(3000);
                        if (Turblip.Exists()) Turblip.Delete();
                        Game.FadeScreenOut(500, true);
                        Suspect.Position = SpawnPoint;
                        Suspect.Face(Game.LocalPlayer.Character);
                        GameFiber.Sleep(500);
                        Game.FadeScreenIn(500, true);
                        Game.DisplaySubtitle("~r~Suspect:~s~ Hello? Can I help you?");
                        Game.DisplayHelp("Press ~y~" + Settings.DialogKey + "~s~ to interact");
                        SuspectOpensDoor = true;
                        CalloutArrived = false;
                        break;
                    }
                }   
                while (SuspectOpensDoor)
                {
                    GameFiber.Yield();
                    PrintWarrant();
                    GameEnd();
                    if (SearchWarrantPrinted && Game.LocalPlayer.Character.IsInAnyVehicle(false) == false)
                    {
                        SWarrant.IsVisible = true;
                    }
                    else
                    {
                        SWarrant.IsVisible = false;
                    }
                    Utils.TurnToFaceEntity(Suspect, Game.LocalPlayer.Character, -1);
                    if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to speak to the person.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            if (DialogWithSuspectFirstTimeIndex < DialogWithSuspectFirstTime.Count)
                            {
                                Game.DisplaySubtitle(DialogWithSuspectFirstTime[DialogWithSuspectFirstTimeIndex]);
                                DialogWithSuspectFirstTimeIndex++;
                            }
                            if (DialogWithSuspectFirstTimeIndex == DialogWithSuspectFirstTime.Count)
                            {
                                if (SWarrant.IsVisible == true)
                                {
                                    Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("mp_common"), "givetake2_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                                    Suspect.Tasks.PlayAnimation(new AnimationDictionary("mp_common"), "givetake2_a", 1f, AnimationFlags.None | AnimationFlags.SecondaryTask);
                                    GameFiber.Sleep(1500);
                                    SWarrant.IsVisible = false;
                                    Game.DisplaySubtitle("~r~Suspect:~s~ Alright come in. (1/1)");
                                    GameFiber.Sleep(1500);
                                    Game.FadeScreenOut(500, true);
                                    Suspect.Position = InteriorSpawnPoint;
                                    Game.LocalPlayer.Character.Position = InteriorSpawnPoint;
                                    GameFiber.Sleep(1000);
                                    Game.FadeScreenIn(500, true);
                                    Suspect.Tasks.FollowNavigationMeshToPosition(SpawnPointChildPhoto, 0f, 1f).WaitForCompletion();
                                    Game.DisplaySubtitle("~b~You:~s~ Hang tight for me I'll take a look around");
                                    ImHaus = true;
                                    SuspectOpensDoor = false;
                                    break;
                                }
                                if (SWarrant.IsVisible == false)
                                {
                                            Game.DisplaySubtitle("~b~You:~s~ I have to get it from my car, hang tight for me. (3/3)");
                                            PedFlieht = true;
                                }
                            }
                        }
                    }
                    if (PedFlieht)
                    {
                            if (Game.LocalPlayer.Character.IsInAnyVehicle(true))
                            {
                                Pursuit = Functions.CreatePursuit();
                                Functions.AddPedToPursuit(Pursuit, Suspect);
                                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                                this.End();
                                SuspectOpensDoor = false;
                                break;
                            }
                    }
                }
                while (ImHaus)
                {
                    GameFiber.Yield();
                    GameEnd();
                    Suspect.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                    switch (SearchArea)
                    {
                        case "Kitchen":
                            KitchenBlip1 = new Blip(KitchenSpawn1);
                            KitchenBlip2 = new Blip(KitchenSpawn2);
                            KitchenBlip1.Color = Color.Purple; KitchenBlip2.Color = Color.Purple;
                            break;
                        case "Living Room":
                            LivingBlip1 = new Blip(LivingSpawn1);
                            LivingBlip2 = new Blip(LivingSpawn2);
                            LivingBlip3 = new Blip(LivingSpawn3);
                            LivingBlip1.Color = Color.Purple; LivingBlip2.Color = Color.Purple; LivingBlip3.Color = Color.Purple;
                            break;
                        case "Bedroom":
                            BedBlip1 = new Blip(BedSpawn1);
                            BedBlip2 = new Blip(BedSpawn2);
                            BedBlip1.Color = Color.Purple; BedBlip2.Color = Color.Purple;
                            break;
                        case "Living+Bedroom":
                            LivingBlip1 = new Blip(LivingSpawn1);
                            LivingBlip2 = new Blip(LivingSpawn2);
                            LivingBlip3 = new Blip(LivingSpawn3);
                            BedBlip1 = new Blip(BedSpawn1);
                            BedBlip2 = new Blip(BedSpawn2);
                            LivingBlip1.Color = Color.Purple; LivingBlip2.Color = Color.Purple; LivingBlip3.Color = Color.Purple; BedBlip1.Color = Color.Purple; BedBlip2.Color = Color.Purple;
                            break;
                        case "Bed+Kitchen":
                            KitchenBlip1 = new Blip(KitchenSpawn1);
                            KitchenBlip2 = new Blip(KitchenSpawn2);
                            BedBlip1 = new Blip(BedSpawn1);
                            BedBlip2 = new Blip(BedSpawn2);
                            KitchenBlip1.Color = Color.Purple; KitchenBlip2.Color = Color.Purple; BedBlip1.Color = Color.Purple; BedBlip2.Color = Color.Purple;
                            break;
                        case "AllRooms":
                            KitchenBlip1 = new Blip(KitchenSpawn1);
                            KitchenBlip2 = new Blip(KitchenSpawn2);
                            LivingBlip1 = new Blip(LivingSpawn1);
                            LivingBlip2 = new Blip(LivingSpawn2);
                            LivingBlip3 = new Blip(LivingSpawn3);
                            BedBlip1 = new Blip(BedSpawn1);
                            BedBlip2 = new Blip(BedSpawn2);
                            BathBlip1 = new Blip(BathSpawn1);
                            BathBlip1.Color = Color.Purple; KitchenBlip1.Color = Color.Purple; KitchenBlip2.Color = Color.Purple; LivingBlip1.Color = Color.Purple; LivingBlip2.Color = Color.Purple; LivingBlip3.Color = Color.Purple; BedBlip1.Color = Color.Purple; BedBlip2.Color = Color.Purple;
                            break;
                    }
                    HausDurchsuchen = true;
                    ImHaus = false;
                    break;
                }
                while (HausDurchsuchen)
                {
                    GameFiber.Yield();
                    GameEnd();
                    Durchsuchen();
                    Suspect.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                    if (TatWaffeGefunden)
                    {
                        int AttackChance = new Random().Next(0, 4);
                        switch(AttackChance)
                        {
                            case 0:
                                SuspectBlip = new Blip(Suspect);
                                SuspectBlip.Color = Color.Red;
                                ArrestSuspect = true;
                                HausDurchsuchen = false;
                                break;
                            case 1:
                                SuspectBlip = new Blip(Suspect);
                                SuspectBlip.Color = Color.Red;
                                ArrestSuspect = true;
                                HausDurchsuchen = false;
                                break;
                            case 2:
                                SuspectBlip = new Blip(Suspect);
                                SuspectBlip.Color = Color.Red;
                                ArrestSuspect = true;
                                HausDurchsuchen = false;
                                break;
                            case 3:
                                Suspect.Inventory.GiveNewWeapon(WeaponHash.Knife, 1, true);
                                SusAttacks = true;
                                HausDurchsuchen = false;
                                break;
                        }
                        
                    }
                    if (NixGefunden)
                    {
                        SuspectBlip = new Blip(Suspect);
                        SuspectBlip.Color = Color.Red;
                        NixGefundenReden = true;
                        HausDurchsuchen = false;
                        Game.DisplayHelp("You can leave through the Front Door");
                        Game.DisplaySubtitle("~b~You:~s~ I couldn't find anything. I'm sorry for the inconvenience. Have a nice day.");
                        break;
                    }
                }
                while (ArrestSuspect)
                {
                    GameFiber.Yield();
                    GameEnd();
                    Suspect.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                    if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 2f && TalkEvidencefound == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to speak to the person.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            if (DialogWithSuspectFoundEvidenceIndex < DialogWithSuspectFoundEvidence.Count)
                            {
                                Game.DisplaySubtitle(DialogWithSuspectFoundEvidence[DialogWithSuspectFoundEvidenceIndex]);
                                DialogWithSuspectFoundEvidenceIndex++;
                            }
                            if (DialogWithSuspectFoundEvidenceIndex == DialogWithSuspectFoundEvidence.Count)
                            {
                                Game.DisplayHelp("You can leave through the Front Door");
                                if (SuspectBlip.Exists()) SuspectBlip.Delete();
                                TalkEvidencefound = true;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(InteriorSpawnPoint) <= 2f && TalkEvidencefound == true)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to exit.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {

                            Game.FadeScreenOut(500, true);
                            Game.LocalPlayer.Character.Position = SpawnPoint;
                            if(Suspect.IsCuffed || Suspect.IsDead)
                            {
                                Suspect.Position = SpawnPoint;
                            }
                            GameFiber.Sleep(1000);
                            Game.FadeScreenIn(500, true);
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                            break;
                        }
                    }
                }
                while (SusAttacks)
                {
                    GameFiber.Yield();
                    GameEnd();
                    Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    if (Suspect.IsCuffed || Suspect.IsDead)
                    {
                        Game.DisplayNotification("You can leave through the Front Door");
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(InteriorSpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to exit.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {

                            Game.FadeScreenOut(500, true);
                            Game.LocalPlayer.Character.Position = SpawnPoint;
                            if (Suspect.IsCuffed || Suspect.IsDead)
                            {
                                Suspect.Position = SpawnPoint;
                            }
                            GameFiber.Sleep(1000);
                            Game.FadeScreenIn(500, true);
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
                            break;
                        }
                    }
                }
                while (NixGefundenReden)
                {
                    GameFiber.Yield();
                    GameEnd();
                    if (Game.LocalPlayer.Character.DistanceTo(InteriorSpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to exit.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {

                            Game.FadeScreenOut(500, true);
                            Game.LocalPlayer.Character.Position = SpawnPoint;
                            if (Suspect.IsCuffed || Suspect.IsDead)
                            {
                                Suspect.Position = SpawnPoint;
                            }
                            GameFiber.Sleep(1000);
                            Game.FadeScreenIn(500, true);
                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                            this.End();
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
            SuspectOpensDoor = false;
            ImHaus = false;
            HausDurchsuchen = false;
            ArrestSuspect = false;
            SusAttacks = false;
            NixGefundenReden = false;
            if (Turblip.Exists()) Turblip.Delete();
            if (Suspect.Exists()) Suspect.Dismiss();
            if (SuspectBlip.Exists()) SuspectBlip.Delete();
            if (BathBlip1.Exists()) BathBlip1.Delete();
            if (BedBlip1.Exists()) BedBlip1.Delete();
            if (BedBlip2.Exists()) BedBlip2.Delete();
            if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
            if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
            if (LivingBlip1.Exists()) LivingBlip1.Delete();
            if (LivingBlip2.Exists()) LivingBlip2.Delete();
            if (LivingBlip3.Exists()) LivingBlip3.Delete();
            if (SWarrant.Exists()) SWarrant.Delete();
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
        private void PrintWarrant()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if(Game.LocalPlayer.Character.IsInAnyPoliceVehicle && SearchWarrantPrinted == false)
                {
                    Game.DisplayHelp("Press ~y~" + Settings.DialogKey + "~s~ to print the ~r~Search Warrant");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        SearchWarrantPrinted = true;
                        WarrantReason = Titles[new Random().Next(0, Titles.Length)];
                        SearchArea = SearchBereich[new Random().Next(0, SearchBereich.Length)];
                        SearchPersonResult = SearchPerson[new Random().Next(0, SearchPerson.Length)];
                        Game.DisplayNotification("Search Warrant issued for~r~ " + WarrantReason + "~s~ You are permitted to search the following Areas:~r~ " + SearchArea +" "+SearchPersonResult);
                    }
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
                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to ring the bell.");
                    if (Game.IsKeyDown(Settings.DialogKey) && Angeklingelt == false)
                    {
                        Game.DisplayNotification("You rang the door!");
                        Angeklingelt = true;
                    }
                }

            });
        }
        private void Durchsuchen()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (SearchArea == "Kitchen")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn1) <= 2f && Kitchen1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch(WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen1Searched = true;
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " +WarrantReason);
                                    Kitchen1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn2) <= 2f && Kitchen2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen2Searched = true;
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Kitchen2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Kitchen1Searched && Kitchen2Searched && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
                if (SearchArea == "Living Room")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn1) <= 2f && Living1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living1Searched = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn2) <= 2f && Living2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living2Searched = true;
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn3) <= 2f && Living3Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living3Searched = true;
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living3Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                            }
                        }
                    }
                    if (Living1Searched && Living2Searched && Living3Searched  && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
                if (SearchArea == "Bedroom")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn1) <= 2f && Bed1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed1Searched = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn2) <= 2f && Bed2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed2Searched = true;
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Bed1Searched && Bed2Searched && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
                if (SearchArea == "Living+Bedroom")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn1) <= 2f && Living1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living1Searched = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn2) <= 2f && Living2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living2Searched = true;
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn3) <= 2f && Living3Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living3Searched = true;
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living3Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn1) <= 2f && Bed1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed1Searched = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn2) <= 2f && Bed2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed2Searched = true;
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Bed1Searched && Bed2Searched && Living1Searched && Living2Searched && Living3Searched && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
                if (SearchArea == "Bed+Kitchen")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn1) <= 2f && Bed1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed1Searched = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn2) <= 2f && Bed2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed2Searched = true;
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn1) <= 2f && Kitchen1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen1Searched = true;
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Kitchen1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn2) <= 2f && Kitchen2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen2Searched = true;
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Kitchen2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                            }
                        }
                    }
                    if (Bed1Searched && Bed2Searched && Kitchen1Searched && Kitchen2Searched && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
                if (SearchArea == "AllRooms")
                {
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn1) <= 2f && Living1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living1Searched = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn2) <= 2f && Living2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living2Searched = true;
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(LivingSpawn3) <= 2f && Living3Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Living3Searched = true;
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Living3Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn1) <= 2f && Bed1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed1Searched = true;
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BedSpawn2) <= 2f && Bed2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Bed2Searched = true;
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Bed2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn1) <= 2f && Kitchen1Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen1Searched = true;
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Kitchen1Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(KitchenSpawn2) <= 2f && Kitchen2Searched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    Kitchen2Searched = true;
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    Kitchen2Searched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Game.LocalPlayer.Character.Position.DistanceTo(BathSpawn1) <= 2f && BathSearched == false)
                    {
                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey + " ~s~ to search this area.");
                        if (Game.IsKeyDown(Settings.DialogKey))
                        {
                            int WasGefunden = new Random().Next(0, 2);
                            switch (WasGefunden)
                            {
                                case 0:
                                    Game.DisplayNotification("You found nothing relevant.");
                                    BathSearched = true;
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                                case 1:
                                    Game.DisplayNotification("You found~r~ " + WarrantReason);
                                    BathSearched = true;
                                    TatWaffeGefunden = true;
                                    if (LivingBlip1.Exists()) LivingBlip1.Delete();
                                    if (LivingBlip2.Exists()) LivingBlip2.Delete();
                                    if (LivingBlip3.Exists()) LivingBlip3.Delete();
                                    if (BedBlip1.Exists()) BedBlip1.Delete();
                                    if (BedBlip2.Exists()) BedBlip2.Delete();
                                    if (KitchenBlip1.Exists()) KitchenBlip1.Delete();
                                    if (KitchenBlip2.Exists()) KitchenBlip2.Delete();
                                    if (BathBlip1.Exists()) BathBlip1.Delete();
                                    break;
                            }
                        }
                    }
                    if (Bed1Searched && Bed2Searched && Kitchen1Searched && Kitchen2Searched && Living1Searched && Living2Searched && Living3Searched && BathSearched && TatWaffeGefunden == false)
                    {
                        NixGefunden = true;
                    }
                }
            });
        }
    }

}
