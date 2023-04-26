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

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Welfare Check", CalloutProbability.Medium)]
    public class WelfareCheck : Callout
    {
        private Ped Mutter;
        private Vehicle FeuerTruk;
        private Vehicle Krankenwagen;
        private Vector3 KrankenwagenSpawn;
        private Ped Neighbor;
        private Blip NeighborBlip;
        private Persona MutterPersona;
        private Persona DummyPersona;
        private Ped Feuerwehr1;
        private Ped Dummy;
        private Ped Feuerwehr2;
        private Ped Mutter2;
        private Ped Medic1;
        private Ped Medic2;
        private Ped Polizist;
        private Vector3 SpawnPoint;
        private Vector3 SpawnFenster;
        private Vector3 SpawnFenster2;
        private Vector3 SpawnFenster3;
        private Vector3 SpawnFenster4;
        private Vector3 NeighborSpawn;
        private Vector3 Polizei1Spawn;
        private Vector3 Medic1Spawn;
        private Vector3 Medic2Spawn;
        private Vector3 Feuerwehr1Spawn;
        private Vector3 Feuerwehr2Spawn;
        private Vector3 MutterOutsideSpawn;
        private Vehicle Polizei;
        private Vector3 FeuerTruckStop;
        private Blip SpawnBlip;
        private Blip FesterBlip1;
        private Blip FesterBlip2;
        private Blip FesterBlip3;
        bool TalkedToDeadMutter;
        private Vector3 PolizeiAutoSpawn;
        private Blip FesterBlip4;
        private Vector3 PoolDespawn;
        bool EndCallout;
        public EMuggingState state;
        public Entscheidung state3;
        private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        int rSpawn = new Random().Next(1, 3);
        int rPedSpawnChance = new Random().Next(1, 6);
        int rAnswerDoorChance = new Random().Next(1, 6);
        int rAnswerDoorChance2 = new Random().Next(1, 11);
        bool AufDemWeg;
        bool Geklingelt1;
        bool Geklingelt2;
        bool SpawnZeug;
        bool FensterChecked;
        bool FensterChecked2;
        bool FensterChecked3;
        bool DummyAnswerDoor;
        bool KeinPedZuhause;
        bool WiederDraussen;
        bool TalkedToDummy;
        bool ImHausOhnePed;
        bool ImHaus;
        bool FensterChecked4;
        bool NeighborChecked;
        bool TalkedToMedic;
        bool SawSomething;
        private Vector3 SpawnInterior;
        private Vector3 MutterInteriorSpawn;
        int FensterGeguckt;
        int WasGesehen = new Random().Next(1, 101);
        private List<string> dialogwithneighbor = new List<string>
        {
            "~b~Officer:~s~ Have you seen this person? We are checking out to see if she's ok.",
            "~p~Neighbor:~s~ I'm sorry I haven't seen them in awhile."
        };
        private int dialogwithneighborindex;
        private List<string> dialogwithMutter = new List<string>
        {
            "~b~Officer:~s~ This is the Police. Can you hear me? Hello?",
            "~y~*NO RESPONSE*"
        };
        private int dialogwithMutterIndex;

        private List<string> DialogMitSani = new List<string>
        {
            "~b~Officer:~s~ We have an inconscious individual inside, he isn't responding.",
            "~g~Medic:~s~ Alright we'll take it from here. Thanks for checking it out."
        };
        private int DialogMitSaniIndex;

        private List<string> DialogWithDummyMutter = new List<string>
        {
            "~b~Officer:~s~ Hello, Police here. We've got a call from someone worried about you and we just wanted to make sure you are alright.",
            "~o~Person:~s~ Yeah I'm ok, I just fell asleep on the couch.",
            "~b~Officer:~s~ I see. Well it appears you aren't in any danger. I will let the person know. Have a nice day.",
            "~o~Person:~s~ You too officer."
        };
        private int DialogWithDummyMutterIndex;
        public override bool OnBeforeCalloutDisplayed()
        {
            // Spawn car
            if (rSpawn == 1) //setLS
            {
                SpawnPoint = new Vector3(-1052.161f, 432.2029f, 77.06366f);
                SpawnFenster = new Vector3(-1044.32f, 414.8253f, 73.86372f);
                SpawnFenster2 = new Vector3(-1030.23f, 426.7726f, 72.86394f);
                SpawnFenster3 = new Vector3(-1036.845f, 443.2393f, 72.86394f);
                SpawnFenster4 = new Vector3(-1062.041f, 437.7944f, 73.86369f);
                NeighborSpawn = new Vector3(-1025.786f, 458.7515f, 79.29343f);
                SpawnInterior = new Vector3(266.0513f, -1005.758f, -100.0373f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                FeuerTruckStop = new Vector3(-1069.072f, 442.5192f, 74.04243f);
                KrankenwagenSpawn = new Vector3(-1072.35f, 429.7533f, 72.03616f);
                Medic1Spawn = new Vector3(-1064.363f, 425.0671f, 72.89972f);
                Medic2Spawn = new Vector3(-1064.314f, 422.677f, 72.68153f);
                MutterOutsideSpawn = new Vector3(-1063.361f, 424.2076f, 72.88866f);
                Feuerwehr1Spawn = new Vector3(-1058.404f, 432.2151f, 73.8639f);
                Feuerwehr2Spawn = new Vector3(-1059.422f, 431.0752f, 73.86378f);
                PolizeiAutoSpawn = new Vector3(-1071.045f, 450.7047f, 74.22746f);
                Polizei1Spawn = new Vector3(-1056.922f, 430.407f, 73.86394f);
                NeighborSpawn = new Vector3(-1025.786f, 458.7515f, 79.29343f);
                PoolDespawn = new Vector3(-1019.329f, 426.7336f, 72.86393f);
            }
            else if (rSpawn == 2)
            {
                SpawnPoint = new Vector3(-1052.161f, 432.2029f, 77.06366f);
                SpawnFenster = new Vector3(-1044.32f, 414.8253f, 73.86372f);
                SpawnFenster2 = new Vector3(-1030.23f, 426.7726f, 72.86394f);
                SpawnFenster3 = new Vector3(-1036.845f, 443.2393f, 72.86394f);
                SpawnFenster4 = new Vector3(-1062.041f, 437.7944f, 73.86369f);
                NeighborSpawn = new Vector3(-1025.786f, 458.7515f, 79.29343f);
                SpawnInterior = new Vector3(266.0513f, -1005.758f, -100.0373f);
                MutterInteriorSpawn = new Vector3(256.9986f, -997.8603f, -99.00858f);
                FeuerTruckStop = new Vector3(-1069.072f, 442.5192f, 74.04243f);
                KrankenwagenSpawn = new Vector3(-1072.35f, 429.7533f, 72.03616f);
            }

            if (rPedSpawnChance == 1) //setLS
            {
                KeinPedZuhause = false;
            }
            else if (rSpawn == 2)
            {
                KeinPedZuhause = false;
            }
            else if (rSpawn == 3)
            {
                KeinPedZuhause = false;
            }
            else if (rSpawn == 4)
            {
                KeinPedZuhause = true;
            }
            else if (rSpawn == 5)
            {
                KeinPedZuhause = true;
            }

            Mutter = new Ped(MutterInteriorSpawn, 0f);
            Mutter.IsPersistent = true;
            Mutter.BlockPermanentEvents = true;
            Mutter.Kill();
            Mutter.IsVisible = false;


            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);

            CalloutMessage = "Welfare Check";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            Neighbor = new Ped(NeighborSpawn);
            Neighbor.BlockPermanentEvents = true;
            Neighbor.IsPersistent = true;

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            SpawnBlip = new Blip(SpawnPoint);
            SpawnBlip.Color = Color.Orange;
            SpawnBlip.EnableRoute(Color.Orange);
            state = EMuggingState.EnRoute;
            FensterChecked = false;
            FensterChecked2 = false;
            FensterChecked3 = false;
            FensterChecked4 = false;
            FensterGeguckt = 101;
            AufDemWeg = true;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
            GameFiber.StartNew(delegate
            {
                while (AufDemWeg)
                {
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 15f)
                    {
                        SpawnBlip.DisableRoute();
                        Utils.ClearAreaOfPeds(PoolDespawn, 5f);
                        break;
                    }
                }
                while (AufDemWeg)
                {
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to ring the bell.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y)) 
                        {
                            SpawnBlip.Delete();
                            Turklingel.Play();
                            GameFiber.Sleep(5000);
                            if (rAnswerDoorChance == 1)
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Dummy = new Ped(SpawnPoint);
                                Dummy.Face(Game.LocalPlayer.Character.Position);
                                Game.FadeScreenIn(2000);
                                DummyAnswerDoor = true;
                                break;
                            }
                            else
                            {
                                Geklingelt1 = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (Geklingelt1)
                {
                    AufDemWeg = false;
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to ring the bell.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            Turklingel.Play();
                            GameFiber.Sleep(3000);
                            if (rAnswerDoorChance == 1)
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Dummy = new Ped(SpawnPoint);
                                Dummy.Face(Game.LocalPlayer.Character.Position);
                                Game.FadeScreenIn(2000);
                                DummyAnswerDoor = true;
                                break;
                            }
                            else
                            {
                                Game.DisplaySubtitle("Find clues about the homeowner.");
                                GameFiber.Sleep(2000);
                                Geklingelt2 = true;
                                FesterBlip1 = new Blip(SpawnFenster);
                                FesterBlip2 = new Blip(SpawnFenster2);
                                FesterBlip3 = new Blip(SpawnFenster3);
                                FesterBlip4 = new Blip(SpawnFenster4);
                                FesterBlip1.Color = Color.Purple;
                                FesterBlip2.Color = Color.Purple;
                                FesterBlip3.Color = Color.Purple;
                                FesterBlip4.Color = Color.Purple;
                                Neighbor.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                NeighborBlip = Neighbor.AttachBlip();
                                NeighborBlip.Color = Color.Purple;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (Geklingelt2)
                {
                    Geklingelt1 = false;
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnFenster) <= 2f && FensterChecked == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to look through the glass.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            FensterGeguckt-=10;
                            FesterBlip1.Delete();
                            if (new Random().Next(1, FensterGeguckt) < 40 && KeinPedZuhause == false)
                            {
                                Game.DisplayHelp("You saw something inside. Go to the Front Door to force entry.");
                                SawSomething = true;
                                if (FesterBlip1.Exists()) FesterBlip1.Delete();
                                if (FesterBlip2.Exists()) FesterBlip2.Delete();
                                if (FesterBlip3.Exists()) FesterBlip3.Delete();
                                if (FesterBlip4.Exists()) FesterBlip4.Delete();
                                if (NeighborBlip.Exists()) NeighborBlip.Delete();
                                if (Neighbor.Exists()) Neighbor.Dismiss();
                                if (Neighbor.Exists()) Neighbor.Tasks.ClearImmediately();
                                GameFiber.Wait(2500);
                                break;
                            }
                            else
                            {
                                Game.DisplayHelp("You looked inside but didn't notice anything");
                            }
                            GameFiber.Sleep(3000);
                            FensterChecked = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnFenster2) <= 2f && FensterChecked2 == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to look through the glass.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            FesterBlip2.Delete();
                            FensterGeguckt -= 10;
                            if (new Random().Next(1, FensterGeguckt) > 40 && KeinPedZuhause == false)
                            {
                                Game.DisplayHelp("You saw something inside. Go to the Front Door to force entry.");
                                SawSomething = true;
                                if (FesterBlip1.Exists()) FesterBlip1.Delete();
                                if (FesterBlip2.Exists()) FesterBlip2.Delete();
                                if (FesterBlip3.Exists()) FesterBlip3.Delete();
                                if (FesterBlip4.Exists()) FesterBlip4.Delete();
                                if (NeighborBlip.Exists()) NeighborBlip.Delete();
                                if (Neighbor.Exists()) Neighbor.Dismiss();
                                if (Neighbor.Exists()) Neighbor.Tasks.ClearImmediately();
                                GameFiber.Wait(2500);
                                break;
                            }
                            else
                            {
                                Game.DisplayHelp("You looked inside but didn't notice anything");
                            }
                            GameFiber.Sleep(3000);
                            FensterChecked2 = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnFenster3) <= 2f && FensterChecked3 == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to look through the glass.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            FensterGeguckt -= 10;
                            FesterBlip3.Delete();
                            if (new Random().Next(1, FensterGeguckt) < 40 && KeinPedZuhause == false)
                            {
                                Game.DisplayHelp("You saw something inside. Go to the Front Door to force entry.");
                                SawSomething = true;
                                if (FesterBlip1.Exists()) FesterBlip1.Delete();
                                if (FesterBlip2.Exists()) FesterBlip2.Delete();
                                if (FesterBlip3.Exists()) FesterBlip3.Delete();
                                if (FesterBlip4.Exists()) FesterBlip4.Delete();
                                if (NeighborBlip.Exists()) NeighborBlip.Delete();
                                if (Neighbor.Exists()) Neighbor.Dismiss();
                                if (Neighbor.Exists()) Neighbor.Tasks.ClearImmediately();
                                GameFiber.Wait(2500);
                                break;
                            }
                            else
                            {
                                Game.DisplayHelp("You looked inside but didn't notice anything");
                            }
                            GameFiber.Sleep(3000);
                            FensterChecked3 = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnFenster4) <= 2f && FensterChecked4 == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to look through the glass.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            FensterGeguckt -= 10;
                            FesterBlip4.Delete();
                            if (new Random().Next(1, FensterGeguckt) < 40 && KeinPedZuhause == false)
                            {
                                Game.DisplayHelp("You saw something inside. Go to the Front Door to force entry.");
                                SawSomething = true;
                                if (FesterBlip1.Exists()) FesterBlip1.Delete();
                                if (FesterBlip2.Exists()) FesterBlip2.Delete();
                                if (FesterBlip3.Exists()) FesterBlip3.Delete();
                                if (FesterBlip4.Exists()) FesterBlip4.Delete();
                                if (NeighborBlip.Exists()) NeighborBlip.Delete();
                                if (Neighbor.Exists()) Neighbor.Dismiss();
                                if (Neighbor.Exists()) Neighbor.Tasks.ClearImmediately();
                                GameFiber.Wait(2500);
                                break;
                            }
                            else
                            {
                                Game.DisplayHelp("You looked inside but didn't notice anything");
                            }
                            GameFiber.Sleep(3000);
                            FensterChecked4 = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(Neighbor) <=2f && NeighborChecked == false)
                    {
                        Neighbor.Tasks.Clear();
                        Neighbor.Face(Game.LocalPlayer.Character);
                        Game.DisplayHelp("Press ~b~ Y ~s~ to speak to the Neighbor.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            if (dialogwithneighborindex < dialogwithneighbor.Count)
                            {
                                Game.DisplaySubtitle(dialogwithneighbor[dialogwithneighborindex]);
                                dialogwithneighborindex++;
                            }
                            if (dialogwithneighborindex == dialogwithneighbor.Count)
                            {
                                if (new Random().Next(1, FensterGeguckt) < 40 && KeinPedZuhause == false)
                                {
                                    Game.DisplayHelp("You got a hint that the Person might still be at home. Go to the Front Door to Force Entry.");
                                    SawSomething = true;
                                    if (FesterBlip1.Exists()) FesterBlip1.Delete();
                                    if (FesterBlip2.Exists()) FesterBlip2.Delete();
                                    if (FesterBlip3.Exists()) FesterBlip3.Delete();
                                    if (FesterBlip4.Exists()) FesterBlip4.Delete();
                                    if (NeighborBlip.Exists()) NeighborBlip.Delete();
                                    if (Neighbor.Exists()) Neighbor.Dismiss();
                                    if (Neighbor.Exists()) Neighbor.Tasks.ClearImmediately();
                                    GameFiber.Wait(2500);
                                    break;
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    else if (FensterChecked == true && FensterChecked2 == true && FensterChecked3 == true && FensterChecked4 == true && NeighborChecked == true)
                    {
                        Game.DisplayHelp("You didn't see anything. You can still choose to Force Entry at the Front Door.");
                        if (Mutter.Exists()) Mutter.Delete();
                        GameFiber.Wait(2500);
                        if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                        {
                            Game.DisplayHelp("Press ~b~ Y ~s~ to force entry.");
                            if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                                Game.FadeScreenIn(600);
                                Game.DisplayHelp("If you are done, you can exit the house at the Front Door by pressing ~b~ Y");
                                GameFiber.Sleep(8000);
                                ImHausOhnePed = true;
                                break;
                            }
                        }
                        else
                        {
                            Game.HideHelp();
                        }

                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (SawSomething)
                {
                    Geklingelt2 = false;
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to force entry.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Mutter.IsVisible = true;
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnInterior);
                            Game.FadeScreenIn(600);
                            ImHaus = true;
                            break;
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (ImHaus)
                {
                    SawSomething = false;
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 2f && TalkedToDeadMutter == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to speak to the person.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            if (dialogwithMutterIndex < dialogwithMutter.Count)
                            {
                                Game.DisplaySubtitle(dialogwithMutter[dialogwithMutterIndex]);
                                dialogwithMutterIndex++;
                            }
                            
                        }
                        if (dialogwithMutterIndex == dialogwithMutter.Count)
                        {
                            TalkedToDeadMutter = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(Mutter) <= 2f && TalkedToDeadMutter == true)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to request Medical Assistance and meet up with Backup.");
                            if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                            {
                                Game.FadeScreenOut(500);
                                GameFiber.Sleep(1000);
                                Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                                SpawnZeug = true;
                            FeuerTruk = new Vehicle("FIRETRUK", FeuerTruckStop, 299.5982f);
                            Krankenwagen = new Vehicle("AMBULANCE", KrankenwagenSpawn, 15.85691f);
                            Polizei = new Vehicle("POLICE2", PolizeiAutoSpawn, 116.6981f);
                            Krankenwagen.IsSirenOn = true; FeuerTruk.IsSirenOn = true; Polizei.IsSirenOn = true;
                            Krankenwagen.IsPersistent = true; FeuerTruk.IsPersistent = true; Polizei.IsPersistent = true;
                            Feuerwehr1 = new Ped("s_m_y_fireman_01", Feuerwehr1Spawn, 116.6981f);
                            Feuerwehr2 = new Ped("s_m_y_fireman_01", Feuerwehr2Spawn, 116.6981f);
                            Polizist = new Ped("s_m_y_cop_01", Polizei1Spawn, 116.6981f);
                            Medic2 = new Ped("s_m_m_paramedic_01", Medic2Spawn, 116.6981f);
                            Medic2.IsPersistent = true; Medic2.BlockPermanentEvents = true;
                            Feuerwehr1.IsPersistent = true; Feuerwehr1.BlockPermanentEvents = true;
                            Feuerwehr2.IsPersistent = true; Feuerwehr2.BlockPermanentEvents = true;
                            Polizist.IsPersistent = true; Polizist.BlockPermanentEvents = true;
                            Polizist.Face(Feuerwehr1);
                            Feuerwehr2.Face(Feuerwehr1);
                            Feuerwehr1.Face(Polizist);
                            SpawnZeug = false;
                            WiederDraussen = true;
                                Game.FadeScreenIn(600);
                                break;
                            }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (WiederDraussen)
                {
                    ImHaus = false;
                    GameFiber.Yield();
                    /*if(SpawnZeug == true)
                    {
                        FeuerTruk = new Vehicle("FIRETRUK", FeuerTruckStop, 299.5982f);
                        Krankenwagen = new Vehicle("AMBULANCE", KrankenwagenSpawn, 15.85691f);
                        Polizei = new Vehicle("POLICE2", PolizeiAutoSpawn, 116.6981f);
                        Krankenwagen.IsSirenOn = true; FeuerTruk.IsSirenOn = true; Polizei.IsSirenOn = true;
                        Krankenwagen.IsPersistent = true; FeuerTruk.IsPersistent = true; Polizei.IsPersistent = true;
                        Feuerwehr1 = new Ped("s_m_y_fireman_01", Feuerwehr1Spawn, 116.6981f);
                        Feuerwehr2 = new Ped("s_m_y_fireman_01", Feuerwehr2Spawn, 116.6981f);
                        Polizist = new Ped("s_m_y_cop_01", Polizei1Spawn, 116.6981f);
                        Medic2 = new Ped("s_m_m_paramedic_01", Medic2Spawn, 116.6981f);
                        Medic2.IsPersistent = true; Medic2.BlockPermanentEvents = true;
                        Feuerwehr1.IsPersistent = true; Feuerwehr1.BlockPermanentEvents = true;
                        Feuerwehr2.IsPersistent = true; Feuerwehr2.BlockPermanentEvents = true;
                        Polizist.IsPersistent = true; Polizist.BlockPermanentEvents = true;
                        Polizist.Face(Feuerwehr1);
                        Feuerwehr2.Face(Feuerwehr1);
                        Feuerwehr1.Face(Polizist);
                        SpawnZeug = false;
                    }*/
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Medic2) <= 2f && TalkedToMedic == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to brief the medic.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            if (DialogMitSaniIndex < DialogMitSani.Count)
                            {
                                Medic2.TurnToFaceEntity(Game.LocalPlayer.Character);
                                Game.DisplaySubtitle(DialogMitSani[DialogMitSaniIndex]);
                                DialogMitSaniIndex++;
                            }

                        }
                        if (DialogMitSaniIndex == DialogMitSani.Count)
                        {
                            TalkedToMedic = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(Medic2) <= 2f && TalkedToMedic == true)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to request Medical Assistance and meet up with Backup.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Medic2.WarpIntoVehicle(Krankenwagen, -1);
                            Feuerwehr1.WarpIntoVehicle(FeuerTruk, -1);
                            Feuerwehr2.WarpIntoVehicle(FeuerTruk, 0);
                            Polizist.WarpIntoVehicle(Polizei, -1);
                            Game.FadeScreenIn(600);

                            this.End();
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (DummyAnswerDoor)
                {
                    Geklingelt1 = false;
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(Dummy) <= 2f && TalkedToDummy == false)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to talk to the person.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            if (DialogWithDummyMutterIndex < DialogWithDummyMutter.Count)
                            {
                                Dummy.TurnToFaceEntity(Game.LocalPlayer.Character);
                                Game.DisplaySubtitle(DialogWithDummyMutter[DialogWithDummyMutterIndex]);
                                DialogWithDummyMutterIndex++;
                            }

                        }
                        if (DialogWithDummyMutterIndex == DialogWithDummyMutter.Count)
                        {
                            TalkedToDummy = true;
                        }
                    }
                    else if (Game.LocalPlayer.Character.Position.DistanceTo(Dummy) <= 2f && TalkedToDummy == true)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to talk to the person.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Dummy.Delete();
                            Game.FadeScreenIn(2000);
                            this.End();
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }
                while (ImHausOhnePed)
                {
                    GameFiber.Yield();
                    if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 2f)
                    {
                        Game.DisplayHelp("Press ~b~ Y ~s~ to exit the house.");
                        if (Game.IsKeyDown(System.Windows.Forms.Keys.Y))
                        {
                            Game.FadeScreenOut(500);
                            GameFiber.Sleep(1000);
                            Mutter.IsVisible = true;
                            Game.LocalPlayer.Character.SetPositionWithSnap(SpawnPoint);
                            Game.FadeScreenIn(600);
                            this.End();
                        }
                    }
                    else
                    {
                        Game.HideHelp();
                    }
                }

            });
        }
        public override void End()
        {
            AufDemWeg = false;
            Geklingelt1 = false;
            Geklingelt2 = false;
            WiederDraussen = false;
            SawSomething = false;
            DummyAnswerDoor = false;
            ImHaus = false;
            ImHausOhnePed = false;
            EndCallout = false;
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (FesterBlip1.Exists()) FesterBlip1.Delete();
            if (FesterBlip2.Exists()) FesterBlip2.Delete();
            if (FesterBlip3.Exists()) FesterBlip3.Delete();
            if (FesterBlip4.Exists()) FesterBlip4.Delete();
            if (NeighborBlip.Exists()) NeighborBlip.Delete();
            if (Neighbor.Exists()) Neighbor.Dismiss();
            if (Polizist.Exists()) Polizist.Dismiss();
            if (Medic1.Exists()) Medic1.Dismiss();
            if (Medic2.Exists()) Medic2.Dismiss();
            if (Feuerwehr1.Exists()) Feuerwehr1.Dismiss();
            if (Feuerwehr2.Exists()) Feuerwehr2.Dismiss();
            if (Polizei.Exists()) Polizei.Dismiss();
            if (Krankenwagen.Exists()) Krankenwagen.Dismiss();
            if (FeuerTruk.Exists()) FeuerTruk.Dismiss();
            if (Dummy.Exists()) Dummy.Dismiss();
            Game.FadeScreenIn(1);
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }


    }

}
