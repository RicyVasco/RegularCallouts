using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RegularCallouts.Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Noise Disturbance", CalloutProbability.Medium)]
    public class NoiseDisturbance : Callout
    {
        private List<Entity> CalloutEntity = new List<Entity>();
        //private SoundPlayer Turklingel = new SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private Vector3 InteriorSpawnPoint;
        private Vector3 SpawnPoint;
        private Vector3 SpawnPointParty;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Ped Sister;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private Vector3 SpawnPoint8;
        private bool CalloutRunning;
        private string TalkingAnimationsHead = "missfbi3_party_d";
        private string[] TalkingAnimations = new string[] { "stand_talk_loop_a_male1", "stand_talk_loop_a_male2", "stand_talk_loop_b_male3" };
        private string MouthAnimationHead = "mp_facial";
        private string MouthAnimation = "mic_chatter";
        private string[] CalloutPossibility = new string[] { "PartyEndsPeacefully" };
        private string CalloutIs;
        private Ped PartyOwner;
        private int PartyOwnerDialog = 1;
        private int PartyOwnerDialogTurnsOn = 0;
        private bool RangDoor;
        private bool TurnMusicOnAgain;
        private bool PlaySound;
        private Rage.Object BoomBox;
        private Blip CalloutBlip;
        private bool EndForcefully;
        private List<Rage.Object> AllCalloutObjects = new List<Rage.Object>();
        private List<Tuple<Vector3, float>> ShelfSpawns;
        private Tuple<Vector3, float> SisterSpawn;

        public override bool OnBeforeCalloutDisplayed()
        {
            InteriorSpawnPoint = new Vector3(-781.709778f, 318.109955f, 187.909683f);
            SisterSpawn = new Tuple<Vector3, float>(new Vector3(-783.900208f, 337.504639f, 187.11731f), 51.8639793f);
            SpawnPoint1 = new Vector3(843.9687f, -562.7008f, 57.99269f);
            SpawnPoint2 = new Vector3(-1113.557f, -1068.939f, 2.150359f);
            SpawnPoint3 = new Vector3(295.9819f, -1971.607f, 22.90078f);
            SpawnPoint4 = new Vector3(67.66263f, -73.54023f, 66.70218f);
            SpawnPoint5 = new Vector3(1826.01f, 3729.139f, 33.96194f);
            SpawnPoint6 = new Vector3(1662.082f, 4776.086f, 42.07681f);
            SpawnPoint7 = new Vector3(-247.9059f, 6370.194f, 31.84802f);
            SpawnPoint8 = new Vector3(-3093.802f, 349.3705f, 7.544598f);
            Vector3[] spawnpoints = new Vector3[]
                {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5,
                SpawnPoint6,
                SpawnPoint7,
                SpawnPoint8
                };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
            }
            else if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
            }
            else if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
            }
            else if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
            }
            else if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
            }
            else if (closestspawnpoint == SpawnPoint6)
            {
                SpawnPoint = SpawnPoint6;
            }
            else if (closestspawnpoint == SpawnPoint7)
            {
                SpawnPoint = SpawnPoint7;
            }
            else if (closestspawnpoint == SpawnPoint8)
            {
                SpawnPoint = SpawnPoint8;
            }
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            CalloutMessage = "Noise Disturbance";
            CalloutPosition = SpawnPoint;
            AddMinimumDistanceCheck(20f, SpawnPoint);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            CalloutIs = CalloutPossibility[new Random().Next(CalloutPossibility.Length)];
            CalloutBlip = new Blip(SpawnPoint);
            CalloutBlip.Color = Color.Yellow;
            CalloutBlip.EnableRoute(Color.Yellow);
            CalloutRunning = true;
            CalloutLos();
            return base.OnCalloutAccepted();
        }

        private void CalloutLos()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) > 350f)
                    {
                        GameFiber.Yield();
                        GameEnd();
                    }
                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        CalloutEntity.Add(Game.LocalPlayer.Character.CurrentVehicle);
                        Ped[] passengers = Game.LocalPlayer.Character.CurrentVehicle.Passengers;
                        if (passengers.Length > 0)
                        {
                            foreach (Ped passenger in passengers)
                            {
                                CalloutEntity.Add(passenger);
                            }
                        }
                    }
                    GameFiber.Yield();
                    ClearUnrelatedEntities();
                    GameFiber.Yield();
                    SpawnStuff();
                    GameFiber.Yield();
                    Game.LogTrivial("Unrelated entities cleared");
                    while (CalloutRunning)
                    {
                        if (CalloutIs == "PartyEndsPeacefully")
                        {
                            GameFiber.Yield();
                            GameEnd();
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 40f && PlaySound == false)
                            {
                                Utils.PlayAudio("RADIO_02_POP");
                                PlaySound = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 3f && RangDoor == false)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    PartyOwner.Position = SpawnPoint;
                                    PartyOwner.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(1500);
                                    RangDoor = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(PartyOwner) <= 2f && RangDoor == true && PartyOwnerDialog <= 7)
                            {
                                PartyOwner.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to talk to the person.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    switch (PartyOwnerDialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ Hello? Can I help you?");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~b~You:~s~ We've got a call about your party. Would you be so kind to turn lower the volume?");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ It's my sister's 18th birthday. Can't you let it slide? It's just one day!");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~b~You:~s~ I understand it's a special day and I don't wanna disrupt it. Just lower the volume a bit, otherwise we turn it off completly.");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 5:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ Ok, ok... I'll turn it down a bit.");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 6:
                                            Game.DisplaySubtitle("~b~You:~s~ Alright, have a nice party!");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 7:
                                            GameFiber.Sleep(3000);
                                            Game.FadeScreenOut(500);
                                            GameFiber.Sleep(1000);
                                            PartyOwner.Position = Utils.StoreSpawn;
                                            PartyOwner.Face(Game.LocalPlayer.Character);
                                            Game.FadeScreenIn(1500);
                                            PartyOwnerDialog++;
                                            break;
                                        default:
                                            Game.DisplaySubtitle("~b~You:~s~ Alright, have a nice party! Bye.");
                                            break;
                                    }
                                }
                            }
                            if (PartyOwnerDialog > 7)
                            {
                                Utils.StopAudio();
                                this.End();
                            }
                        }
                        if (CalloutIs == "TurnsMusicOnAgain")
                        {
                            GameFiber.Yield();
                            GameEnd();
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 40f && PlaySound == false)
                            {
                                Utils.PlayAudio("RADIO_02_POP");
                                PlaySound = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 3f && RangDoor == false)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    PartyOwner.Position = SpawnPoint;
                                    PartyOwner.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(1500);
                                    RangDoor = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(PartyOwner) <= 2f && RangDoor == true && PartyOwnerDialog <= 7)
                            {
                                PartyOwner.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to talk to the person.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    switch (PartyOwnerDialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ Hello? Can I help you?");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~b~You:~s~ We've got a call about your party. Would you be so kind to turn lower the volume?");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ It's my sister's 18th birthday. Can't you let it slide? It's just one day!");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~b~You:~s~ I understand it's a special day and I don't wanna disrupt it. Just lower the volume a bit, otherwise we turn it off completly.");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 5:
                                            Game.DisplaySubtitle("~y~Home Owner:~s~ Ok, ok... I'll turn it down a bit.");
                                            PartyOwner.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
                                            PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                            PartyOwnerDialog++;
                                            break;
                                        case 6:
                                            Game.DisplaySubtitle("~b~You:~s~ Alright, have a nice party!");
                                            PartyOwner.Tasks.Clear();
                                            PartyOwner.Tasks.ClearSecondary();
                                            PartyOwnerDialog++;
                                            break;
                                        case 7:
                                            GameFiber.Sleep(3000);
                                            Game.FadeScreenOut(500);
                                            GameFiber.Sleep(1000);
                                            PartyOwner.Position = Utils.StoreSpawn;
                                            PartyOwner.Face(Game.LocalPlayer.Character);
                                            Game.FadeScreenIn(1500);
                                            Utils.StopAudio();
                                            PartyOwnerDialog++;
                                            break;
                                        default:
                                            Game.DisplaySubtitle("~b~You:~s~ Alright, have a nice party!");
                                            break;
                                    }
                                }
                            }
                            if (PartyOwnerDialog > 7 && TurnMusicOnAgain == false)
                            {
                                if(Game.LocalPlayer.Character.IsInAnyVehicle(true))
                                {
                                    Utils.PlayAudio("RADIO_02_POP");
                                    TurnMusicOnAgain = true;
                                }
                            }
                            if (TurnMusicOnAgain == true)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 3f && PartyOwnerDialogTurnsOn == 0)
                                {
                                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to ring the bell.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.DisplayNotification("You rang the door!");
                                        GameFiber.Sleep(3000);
                                        Game.FadeScreenOut(500);
                                        GameFiber.Sleep(1000);
                                        PartyOwner.Position = SpawnPoint;
                                        PartyOwner.Face(Game.LocalPlayer.Character);
                                        Game.FadeScreenIn(1500);
                                        PartyOwnerDialogTurnsOn++;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(PartyOwner) <= 2f && PartyOwnerDialogTurnsOn <= 4)
                                {
                                    PartyOwner.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey.ToString() + "~s~ to talk to the person.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        switch (PartyOwnerDialogTurnsOn)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Didn't I make myself clear enough?");
                                                PartyOwnerDialogTurnsOn++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~y~Home Owner:~s~ And I told you it was a special day, didn't I?");
                                                PartyOwner.Tasks.PlayAnimation(MouthAnimationHead, MouthAnimation, 1f, AnimationFlags.SecondaryTask | AnimationFlags.Loop);
                                                PartyOwnerDialogTurnsOn++;
                                                break;
                                            case 3:
                                                PartyOwner.Tasks.Clear();
                                                Game.DisplaySubtitle("~b~You:~s~ I gave you a fair chance to turn it off, you didn't listen. i'm going to take your music now. You can collect it tomorrow at the police station.");
                                                PartyOwnerDialogTurnsOn++;
                                                break;
                                            case 4:
                                                PartyOwnerDialogTurnsOn++;
                                                break;
                                            default:
                                                Game.DisplaySubtitle("~y~Home Owner:~s~ And I told you it was a special day, didn't I?");
                                                break;

                                        }
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(PartyOwner) <= 2f && PartyOwnerDialogTurnsOn > 4 && EndForcefully == false)
                                {
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    Game.LocalPlayer.Character.Position = InteriorSpawnPoint;
                                    Sister.Position = SisterSpawn.Item1;
                                    Sister.Heading = SisterSpawn.Item2;
                                    PartyOwner.Position = Utils.StoreSpawn;
                                    SpawnWindowHideInterior();
                                    SpawnBangingTeens();
                                    SpawnTalkingPeds();
                                    Game.FadeScreenIn(1500);
                                    EndForcefully = true;
                                }
                                if (EndForcefully)
                                {
                                    if (PartyOwner.IsDead || PartyOwner.IsCuffed)
                                    {
                                        this.End();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Game.LogTrivial("Regular Callouts Error: " + ex);
                }
            });
            base.Process();
        }

        public override void End()
        {
           CalloutRunning = false;
            foreach(Entity e in CalloutEntity)
            {
                if(e.Exists())
                {
                    e.Dismiss();
                }
            }
            if (CalloutBlip.Exists()) { CalloutBlip.Delete(); }
            Utils.StopAudio();
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }

        private void ClearUnrelatedEntities()
        {

            foreach (Ped entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderAllPeds))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!CalloutEntity.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Vehicle entity in World.GetEntities(SpawnPoint, 50f, GetEntitiesFlags.ConsiderGroundVehicles))
            {
                GameFiber.Yield();
                if (entity != null)
                {
                    if (entity.IsValid())
                    {
                        if (entity.Exists())
                        {
                            if (entity != Game.LocalPlayer.Character)
                            {
                                if (entity != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!entity.CreatedByTheCallingPlugin)
                                    {

                                        if (!CalloutEntity.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, SpawnPoint) < 50f)
                                            {
                                                entity.Delete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GameEnd()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (!Game.IsKeyDown(Settings.EndCalloutKey))
                    return;
                this.End();
            });
        }

        private void SpawnStuff()
        {
            PartyOwner = new Ped(Utils.StoreSpawn);
            PartyOwner.BlockPermanentEvents = true;
            CalloutEntity.Add(PartyOwner);

            Sister = new Ped("ig_tracydisanto", Utils.StoreSpawn, 0f);
            Sister.BlockPermanentEvents = true;
            CalloutEntity.Add(Sister);
            Functions.SetPersonaForPed(Sister, new LSPD_First_Response.Engine.Scripting.Entities.Persona(Functions.GetPersonaForPed(Sister).Forename, Functions.GetPersonaForPed(PartyOwner).Surname, LSPD_First_Response.Gender.Female, new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)));
        }

        private void SpawnWindowHideInterior()
        {
            ShelfSpawns = new List<Tuple<Vector3, float>>() { new Tuple<Vector3, float>(new Vector3(-788.942627f, 343.590485f, 186.019424f), 0.511348784f), new Tuple<Vector3, float>(new Vector3(-786.343262f, 343.590485f, 186.019424f), -0.0886512846f), new Tuple<Vector3, float>(new Vector3(-783.743896f, 343.590485f, 186.019424f), -0.0886512846f), new Tuple<Vector3, float>(new Vector3(-781.144531f, 343.590485f, 186.019424f), -0.0886512846f) };
            for (int i = 0; i < ShelfSpawns.Count; i++)
            {
                Rage.Object barrier = new Rage.Object("apa_mp_h_str_shelffloorm_02", ShelfSpawns[i].Item1, ShelfSpawns[i].Item2);
                barrier.IsPersistent = true;
                barrier.IsPositionFrozen = true;
                CalloutEntity.Add(barrier);
                AllCalloutObjects.Add(barrier);
            }
            List<Tuple<Vector3, float>> ArtWall = new List<Tuple<Vector3, float>>() { new Tuple<Vector3, float>(new Vector3(-799.064636f, 340.299683f, 190.874207f), 0.0735071301f), new Tuple<Vector3, float>(new Vector3(-796.833008f, 340.249359f, 190.874207f), -0.790681303f), new Tuple<Vector3, float>(new Vector3(-794.574463f, 340.24649f, 190.875671f), 0.248716488f), new Tuple<Vector3, float>(new Vector3(-797.527588f, 331.343964f, 190.874207f), 179.342529f)};
            List<string> ArtWallName = new List<string>() {"apa_mp_h_acc_artwalll_01", "ex_mp_h_acc_artwalll_03", "ex_p_h_acc_artwalll_03" };
            for (int i = 0; i < ArtWall.Count; i++)
            {
                Rage.Object barrier = new Rage.Object(ArtWallName[new Random().Next(ArtWallName.Count)], ArtWall[i].Item1, ArtWall[i].Item2);
                barrier.IsPersistent = true;
                barrier.IsPositionFrozen = true;
                CalloutEntity.Add(barrier);
                AllCalloutObjects.Add(barrier);
            }
            Rage.Object beerbox = new Rage.Object("ba_prop_battle_crate_beer_03", new Vector3(-793.316345f, 323.966095f, 186.315613f), 91.0020065f);
            beerbox.IsPersistent = true;
            beerbox.IsPositionFrozen = true;
            CalloutEntity.Add(beerbox);
            AllCalloutObjects.Add(beerbox);

            Rage.Object beerbox2 = new Rage.Object("ba_prop_battle_crate_beer_02", new Vector3(-793.630676f, 324.11496f, 186.921265f), 77.2280121f);
            beerbox2.IsPersistent = true;
            beerbox2.IsPositionFrozen = true;
            CalloutEntity.Add(beerbox2);
            AllCalloutObjects.Add(beerbox2);

            List<Tuple<Vector3, float>> BeerRow = new List<Tuple<Vector3, float>>()
            {
                new Tuple<Vector3, float>(new Vector3(-784.124634f, 327.712952f, 187.264923f), -90.9227448f),
                new Tuple<Vector3, float>(new Vector3(-784.108643f, 328.702759f, 187.264252f), -90.9227448f),
                new Tuple<Vector3, float>(new Vector3(-787.53009f, 327.042633f, 187.102081f), 86.3541336f),
                new Tuple<Vector3, float>(new Vector3(-787.819458f, 328.889923f, 187.102081f), 119.613602f),
                new Tuple<Vector3, float>(new Vector3(-785.014221f, 339.074829f, 186.616486f), -177.558167f),
                new Tuple<Vector3, float>(new Vector3(-787.619019f, 342.810913f, 186.114227f), -0.660797894f),
                new Tuple<Vector3, float>(new Vector3(-785.969299f, 342.791809f, 186.114227f), -0.660797894f),
                new Tuple<Vector3, float>(new Vector3(-795.885132f, 337.83313f, 189.714172f), 113.61145f),
                new Tuple<Vector3, float>(new Vector3(-805.423584f, 331.493744f, 190.620026f), 90.6548996f)
            };
            List<string> BeerRowName = new List<string>() { "beerrow_local", "beerrow_world" };
            for (int i = 0; i < BeerRow.Count; i++)
            {
                Rage.Object BeersRow = new Rage.Object(BeerRowName[new Random().Next(BeerRowName.Count)], BeerRow[i].Item1, BeerRow[i].Item2);
                BeersRow.IsPersistent = true;
                BeersRow.IsPositionFrozen = true;
                CalloutEntity.Add(BeersRow);
                AllCalloutObjects.Add(BeersRow);
            }
            BoomBox = new Rage.Object("prop_boombox_01", new Vector3(-780.883362f, 339.722809f, 187.05275f), -90.2557526f);
            BoomBox.IsPersistent = true;
            BoomBox.IsPositionFrozen = true;
            CalloutEntity.Add(BoomBox);
            AllCalloutObjects.Add(BoomBox);
        }

        private void SpawnBangingTeens()
        {
            Ped BangFemale = new Ped("a_f_y_topless_01", new Vector3(-806.217041f, 332.854462f, 190.713409f), -177.84938f);
            BangFemale.BlockPermanentEvents = true;
            CalloutEntity.Add(BangFemale);
            BangFemale.Tasks.PlayAnimation("rcmpaparazzo_2", "shag_loop_poppy", 1f, AnimationFlags.Loop);
            BangFemale.IsPositionFrozen = true;

            Ped BangMale = new Ped("a_m_y_beachvesp_01", new Vector3(-806.364136f, 333.080475f, 190.738174f), -165.014084f);
            BangMale.BlockPermanentEvents = true;
            CalloutEntity.Add(BangMale);
            BangMale.Tasks.PlayAnimation("rcmpaparazzo_2", "shag_loop_a", 1f, AnimationFlags.Loop);
            BangMale.IsPositionFrozen = true;
            BangMale.CollisionIgnoredEntity = BangFemale;
            BangMale.Position = new Vector3(-806.364136f, 333.080475f, 190.738174f);
            BangMale.Heading = -165.014084f;
            BangFemale.IsPositionFrozen = false;
            BangMale.IsPositionFrozen = false;
        }

        private void SpawnTalkingPeds()
        {
            List<Tuple<Vector3, float>> TalkingPeds = new List<Tuple<Vector3, float>>()
            {
                new Tuple<Vector3, float>(new Vector3(-784.679016f, 332.052277f, 187.309555f), -62.7938271f),
                new Tuple<Vector3, float>(new Vector3(-783.808533f, 333.011047f, 187.30986f), -178.368454f),
                new Tuple<Vector3, float>(new Vector3(-783.044128f, 331.838196f, 187.308823f), 70.6646042f),
                new Tuple<Vector3, float>(new Vector3(-798.246826f, 338.820862f, 190.720245f), -116.177048f),
                new Tuple<Vector3, float>(new Vector3(-798.218933f, 338.303833f, 190.720047f), -47.405014f),
                new Tuple<Vector3, float>(new Vector3(-797.599609f, 338.0177f, 190.723511f), -0.633372962f),
                new Tuple<Vector3, float>(new Vector3(-797.490295f, 338.504059f, 190.72023f), 106.767578f),
                new Tuple<Vector3, float>(new Vector3(-783.891541f, 342.600891f, 187.099426f), -96.0269547f),
                new Tuple<Vector3, float>(new Vector3(-782.502502f, 342.388519f, 187.107803f), 89.2404633f)
            };
            List<string> ModelName = new List<string>() { "a_f_y_bevhills_01", "a_m_y_bevhills_01", "a_f_y_bevhills_02", "a_m_y_bevhills_02", "a_f_y_bevhills_03" };
            for (int i = 0; i < TalkingPeds.Count; i++)
            {
                Ped Talk = new Ped(ModelName[new Random().Next(ModelName.Count)], TalkingPeds[i].Item1, TalkingPeds[i].Item2);
                Talk.BlockPermanentEvents = true;
                CalloutEntity.Add(Talk);
                Talk.Tasks.PlayAnimation(TalkingAnimationsHead, TalkingAnimations[new Random().Next(TalkingAnimations.Length)], 1f, AnimationFlags.Loop);
            }
            List<Tuple<Vector3, float>> DrinkingPeds = new List<Tuple<Vector3, float>>()
            {
                new Tuple<Vector3, float>(new Vector3(-784.743774f, 327.71463f, 187.298218f), 93.2230988f),
                new Tuple<Vector3, float>(new Vector3(-786.387939f, 327.079529f, 187.326813f), -67.4461365f),
                new Tuple<Vector3, float>(new Vector3(-789.942322f, 333.923126f, 187.309464f), -63.5503998f),
                new Tuple<Vector3, float>(new Vector3(-789.409546f, 332.984467f, 187.310181f), -46.4872093f),
                new Tuple<Vector3, float>(new Vector3(-787.381409f, 334.248108f, 187.110321f), 90.8825912f),
                new Tuple<Vector3, float>(new Vector3(-790.793762f, 342.860748f, 187.150009f), -160.643463f),
                new Tuple<Vector3, float>(new Vector3(-792.839844f, 341.377747f, 187.150208f), -108.674347f),
                new Tuple<Vector3, float>(new Vector3(-786.747498f, 340.365448f, 187.571793f), -130.499084f),
                new Tuple<Vector3, float>(new Vector3(-791.531006f, 332.773376f, 190.710129f), 92.0431976f),
                new Tuple<Vector3, float>(new Vector3(-795.28241f, 334.639374f, 190.696198f), 88.403656f),
                new Tuple<Vector3, float>(new Vector3(-799.781494f, 339.235413f, 190.700333f), -100.67511f),
                new Tuple<Vector3, float>(new Vector3(-801.3302f, 332.246765f, 190.698578f), -29.9971924f),
                new Tuple<Vector3, float>(new Vector3(-782.53833f, 326.81308f, 187.309479f), 31.8470116f)
            };
            for (int i = 0; i < DrinkingPeds.Count; i++)
            {
                Ped BeerPed = new Ped(ModelName[new Random().Next(ModelName.Count)], DrinkingPeds[i].Item1, DrinkingPeds[i].Item2);
                BeerPed.BlockPermanentEvents = true;
                CalloutEntity.Add(BeerPed);
                BeerPed.Tasks.PlayAnimation("amb@world_human_drinking@beer@male@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                Rage.Object BeerBottle = new Rage.Object("ng_proc_beerbottle_01a", Utils.StoreSpawn);
                BeerBottle.IsPersistent = true;
                AllCalloutObjects.Add(BeerBottle);
                CalloutEntity.Add(BeerBottle);
                BeerBottle.AttachTo(BeerPed, 66, new Vector3(-1.42704666e-05f, 4.05788451e-05f, 2.28351128e-05f), new Rotator(0f, 0f, -2.70962773e-05f));
            }
            List<Tuple<Vector3, float>> DancingPeds = new List<Tuple<Vector3, float>>()
            {
                new Tuple<Vector3, float>(new Vector3(-784.192871f, 336.87207f, 187.121017f), 35.6014595f),
                new Tuple<Vector3, float>(new Vector3(-785.370178f, 337.056824f, 187.117783f), -6.15345478f),
                new Tuple<Vector3, float>(new Vector3(-783.724487f, 338.37027f, 187.119385f), 91.3738174f),
                new Tuple<Vector3, float>(new Vector3(-783.823059f, 339.360413f, 187.11763f), 115.192337f)
            };
            for (int i = 0; i < DancingPeds.Count; i++)
            {
                Ped DancePed = new Ped(ModelName[new Random().Next(ModelName.Count)], DancingPeds[i].Item1, DancingPeds[i].Item2);
                DancePed.BlockPermanentEvents = true;
                CalloutEntity.Add(DancePed);
                DancePed.Tasks.PlayAnimation("amb@world_human_cheering@female_b", "base", 1f, AnimationFlags.Loop);
            }
            Ped StripperPed = new Ped("a_f_y_beach_01", new Vector3(-785.15918f, 338.414703f, 187.719086f), -130.475204f);
            StripperPed.BlockPermanentEvents = true;
            CalloutEntity.Add(StripperPed);
            StripperPed.Tasks.PlayAnimation("anim@amb@nightclub@peds@", "mini_strip_club_lap_dance_ld_girl_a_song_a_p1", 1f, AnimationFlags.Loop);
        }
    }
}
