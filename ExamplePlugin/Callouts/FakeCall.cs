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
    [CalloutInfo("Fake Call", CalloutProbability.Medium)]
    public class FakeCall : Callout
    {
        private Ped Caller;
        private Ped Terminator;
        private Vector3 SpawnPoint;
        private int Particle1;
        private int Particle2;
        private Vector3 CallerLocation;
        private Blip SpawnBlip;
        private string CalloutIs;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 SpawnPoint7;
        private bool FacePlayer;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private List<Ped> AllCalloutPeds = new List<Ped>();
        private bool TalkedToCaller;
        private bool CalloutRunning;
        private bool AttackPlayer;
        DateTime StartTimer;
        bool GotSlapped;

        private List<string> DialogWithCaller = new List<string>
        {
            "~y~Caller:~s~ Thank god you came so quickly! I've heard weird noises from the back. Sounded like explosions and lightning or something. (1/5)",
            "~b~You:~s~ Did you check it out? (2/5)",
            "~y~Caller:~s~ No Sir, I'm way too scared that's why I called you. (3/5)",
            "~b~You:~s~ Alright I'll take a look. (4/5)",
            "~y~Caller:~s~ Be careful Officer! (5/5)"
        };
        private int DialogWithCallerIndex;

        private static string[] CalloutPossibility = new string[] { "TerminatorEE"};
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(8.59966373f, -1403.96606f, 29.276453f);
            SpawnPoint2 = new Vector3(-471.204071f, -456.906921f, 34.2045364f);
            SpawnPoint3 = new Vector3(1155.33154f, -1405.4696f, 34.6990356f);
            SpawnPoint4 = new Vector3(-1564.6095f, -418.16626f, 38.0900688f);
            SpawnPoint5 = new Vector3(2059.31226f, 3178.74902f, 45.1727524f);
            SpawnPoint6 = new Vector3(416.747528f, 6507.84033f, 27.7311096f);
            SpawnPoint7 = new Vector3(2870.73657f, 4465.68945f, 48.4055405f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4,
                SpawnPoint5,
                SpawnPoint6,
                SpawnPoint7
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                CallerLocation = new Vector3(-5.07373047f, -1380.30212f, 29.3123722f);
                Game.LogTrivial("SpawnPoint 1 Chosen");
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                CallerLocation = new Vector3(-513.670959f, -442.332642f, 34.2594337f);
                Game.LogTrivial("SpawnPoint 2 Chosen");
            }
            if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
                CallerLocation = new Vector3(1192.49585f, -1418.2821f, 35.196434f);
                Game.LogTrivial("SpawnPoint 3 Chosen");
            }
            if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
                CallerLocation = new Vector3(-1593.67004f, -397.84668f, 43.1694603f);
                Game.LogTrivial("SpawnPoint 4 Chosen");
            }
            if (closestspawnpoint == SpawnPoint5)
            {
                SpawnPoint = SpawnPoint5;
                CallerLocation = new Vector3(2034.39478f, 3176.68457f, 45.1272011f);
                Game.LogTrivial("SpawnPoint 5 Chosen");
            }
            if (closestspawnpoint == SpawnPoint6)
            {
                SpawnPoint = SpawnPoint6;
                CallerLocation = new Vector3(420.898132f, 6537.87891f, 27.7253971f);
                Game.LogTrivial("SpawnPoint 6 Chosen");
            }
            if (closestspawnpoint == SpawnPoint7)
            {
                SpawnPoint = SpawnPoint7;
                CallerLocation = new Vector3(2888.69067f, 4448.37891f, 48.4844666f);
                Game.LogTrivial("SpawnPoint 7 Chosen");
            }
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 40f);
            CalloutMessage = "Peace Disturbance";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            CalloutIs = CalloutPossibility[new Random().Next(CalloutPossibility.Length)];
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            SpawnBlip = new Blip(SpawnPoint, 40f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            Game.LocalPlayer.Character.RelationshipGroup = "PLAYER";
            SpawnBlip.EnableRoute(Color.Yellow);
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
            CalloutRunning = true;
            try
            {
                GameFiber.StartNew(delegate
                {
                    while (Vector3.Distance(Game.LocalPlayer.Character.Position, SpawnPoint) > 350f)
                    {
                        GameFiber.Yield();
                        GameEnd();
                    }
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
                    GameFiber.Yield();
                    ClearUnrelatedEntities();
                    Game.LogTrivial("Unrelated entities cleared");
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    GameFiber.Yield();
                    Game.LogTrivial("Initialisation complete, Spawning Callout");
                    if (CalloutIs == "TerminatorEE")
                    {
                        SpawnTerminatorPeds();
                        GameFiber.Yield();
                    }
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (CalloutIs == "TerminatorEE")
                        {
                            Caller.Face(Game.LocalPlayer.Character);
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Caller) <= 10f && FacePlayer == false)
                            {
                                SpawnBlip.Delete();
                                SpawnBlip = Caller.AttachBlip();
                                SpawnBlip.Color = Color.Yellow;
                                FacePlayer = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(Caller) <= 5f)
                            {
                                Caller.Tasks.Clear();
                            }
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Caller) <= 2f && TalkedToCaller == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Caller.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    if (DialogWithCallerIndex < DialogWithCaller.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithCaller[DialogWithCallerIndex]);
                                        DialogWithCallerIndex++;
                                    }
                                    if (DialogWithCallerIndex == DialogWithCaller.Count)
                                    {
                                        SpawnBlip.Delete();
                                        SpawnBlip = Terminator.AttachBlip();
                                        SpawnBlip.Color = Color.Red;
                                        TalkedToCaller = true;
                                    }
                                }
                            }
                            if(Game.LocalPlayer.Character.DistanceTo(Terminator) <= 5f && Game.LocalPlayer.Character.IsOnFoot && TalkedToCaller == true && AttackPlayer == false)
                            {
                                Game.LocalPlayer.HasControl = false;
                                Game.LocalPlayer.Character.TurnToFaceEntity(Terminator);
                                Terminator.Face(Game.LocalPlayer.Character);
                                Game.DisplaySubtitle("~b~You:~s~ What the...");
                                Particle1 = Utils.StartParticleFxNonLoopedOnEntity("scr_xs_props", "scr_xs_exp_mine_sf", Terminator, new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f), 2f);
                                GameFiber.Sleep(500);
                                Terminator.IsVisible = true;
                                Particle2 = Utils.StartParticleFxLoopedOnEntity("scr_rcbarry1", "scr_alien_charging", Terminator, new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f), 2f);
                                GameFiber.Sleep(4000);
                                Game.DisplaySubtitle("~r~???:~s~ I have no business with you. I'm looking for Sarah Connor.");
                                Utils.RemoveParticle(Particle1);
                                Utils.StopLoopedFX(Particle2);
                                Utils.RemoveParticle(Particle2);
                                Functions.SetPedArrestIgnoreGroup(Terminator, true);
                                Terminator.KeepTasks = true;
                                Terminator.Tasks.FightAgainst(Game.LocalPlayer.Character);
                                StartTimer = DateTime.Now;
                                AttackPlayer = true;
                            }
                            if(AttackPlayer == true)
                            {
                                if (StartTimer.AddSeconds(8) < DateTime.Now && !Game.LocalPlayer.Character.HasBeenDamagedByAnyPed && !GotSlapped)
                                {
                                    GotSlapped = true;
                                    Game.LocalPlayer.HasControl = true;
                                    Game.LocalPlayer.Character.IsRagdoll = true;
                                    Game.FadeScreenOut(1000, true);
                                    Terminator.Delete();
                                    Caller.Delete();
                                    World.TimeOfDay = World.TimeOfDay + TimeSpan.FromHours(6);
                                    GameFiber.Sleep(1000);
                                    Game.FadeScreenIn(1000, true);
                                    Game.LocalPlayer.Character.IsRagdoll = false;
                                    Game.DisplaySubtitle("~b~You:~s~ What was that??");
                                    this.End();
                                }
                                if (Game.LocalPlayer.Character.HasBeenDamagedByAnyPed && !GotSlapped)
                                {
                                    GotSlapped = true;
                                    Game.LocalPlayer.HasControl = true;
                                    Game.LocalPlayer.Character.IsRagdoll = true;
                                    Game.FadeScreenOut(1000, true);
                                    Terminator.Delete();
                                    Caller.Delete();
                                    World.TimeOfDay = World.TimeOfDay + TimeSpan.FromHours(6);
                                    GameFiber.Sleep(1000);
                                    Game.FadeScreenIn(1000, true);
                                    Game.LocalPlayer.Character.IsRagdoll = false;
                                    Game.DisplaySubtitle("~b~You:~s~ What was that??");
                                    this.End();
                                }
                            }
                        }
                    }
                });
            }
            catch(Exception e)
            {
                Game.LogTrivial(e.ToString() + " Callout Crashed");
                this.End();
            }
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

                                        if (!AllCalloutEntities.Contains(entity))
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

                                        if (!AllCalloutEntities.Contains(entity))
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
        private void MakeNearbyPedsFlee()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {

                    GameFiber.Yield();

                    foreach (Ped entity in World.GetEntities(SpawnPoint, 80f, GetEntitiesFlags.ConsiderAllPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ExcludePoliceOfficers))
                    {
                        GameFiber.Yield();
                        if (AllCalloutEntities.Contains(entity))
                        {
                            continue;
                        }
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

                                                if (!AllCalloutEntities.Contains(entity))
                                                {
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 74f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle != null)
                                                            {

                                                                entity.Tasks.PerformDrivingManeuver(VehicleManeuver.Wait);


                                                            }
                                                        }
                                                        else
                                                        {
                                                            Rage.Native.NativeFunction.CallByName<uint>("TASK_SMART_FLEE_COORD", entity, SpawnPoint.X, SpawnPoint.Y, SpawnPoint.Z, 75f, 6000, true, true);
                                                        }

                                                    }
                                                    if (Vector3.Distance(entity.Position, SpawnPoint) < 65f)
                                                    {
                                                        if (entity.IsInAnyVehicle(false))
                                                        {
                                                            if (entity.CurrentVehicle.Exists())
                                                            {
                                                                entity.CurrentVehicle.Delete();
                                                            }
                                                        }
                                                        if (entity.Exists())
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
                }
            });
        }
        private void SpawnTerminatorPeds()
        {
            Caller = new Ped(CallerLocation);
            Caller.IsPersistent = true;
            Caller.BlockPermanentEvents = true;
            AllCalloutPeds.Add(Caller);
            Caller.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            Terminator = new Ped("a_m_y_musclbeac_01", SpawnPoint, 0f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true,
                IsInvincible = true,
                IsVisible = false
        };
            
            AllCalloutPeds.Add(Terminator);
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
        public override void End()
        {
            CalloutRunning = false;
            GotSlapped = true;
            foreach (Ped p in AllCalloutPeds)
            {
                if(p.Exists())
                {
                    p.Dismiss();
                }
            }
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            Game.LocalPlayer.HasControl = true;
            Game.LocalPlayer.Character.IsPositionFrozen = false;
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }


    }

}
