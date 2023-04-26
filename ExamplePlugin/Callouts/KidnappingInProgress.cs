using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RegularCallouts.Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Kidnapping in Progress", CalloutProbability.Medium)]
    public class KidnappingInProgress : Callout
    {
        private Vector3 SpawnPoint;
        private Vector3 SpawnPoint1;
        private Blip SpawnBlip;
        private Blip CarBlip;
        private List<Entity> CalloutEnts;
        private bool CleanUp;

        private Ped Harasser;
        private Ped Girl;
        private Vehicle SusCar;
        private bool FollowSus;
        private bool DrawBar;
        private LHandle Chase;
        private bool ChaseActive;
        private Vector3 DropOffPoint;
        private TimerBarPool AwarenessBarPool = new TimerBarPool();
        private BarTimerBar AwarenessBar = new BarTimerBar("Suspicion");
        private float Awareness = 0f;

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutEnts = new List<Entity>();
            SpawnPoint1 = new Vector3(1989.89954f, 3055.06494f, 47.2132912f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
            }
            
            CalloutMessage = "Potential Kidnapping in Progress";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 40f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
            if(SpawnPoint == SpawnPoint1)
            {
                SusCar = new Vehicle("taxi",new Vector3(1999.80786f, 3054.17578f, 46.7189369f), -31.323f);
                Harasser = SusCar.CreateRandomDriver();
                Harasser.BlockPermanentEvents = true;
                Girl = new Ped("a_f_y_bevhills_02", new Vector3(1991.14001f, 3044.70605f, 47.2182274f), 10.5683279f);
                Girl.BlockPermanentEvents = true;
                Girl.WarpIntoVehicle(SusCar, 0);
                CalloutEnts.Add(Harasser);
                CalloutEnts.Add(Girl);
                CalloutEnts.Add(SusCar);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
                DropOffPoint = new Vector3(1823.392f, 3691.546f, 34.22428f);
                AwarenessBarPool.Add(AwarenessBar);
            }
            Game.DisplayHelp($"Press ~{Settings.EndCalloutKey.GetInstructionalId()}~ to end the Callout anytime.");
            Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to responding Unit" + "", "The ~r~suspect~s~ is driving a ~y~"+ SusCar.GetColors().PrimaryColorName +" "+ SusCar.Model.Name+" ~n~~s~Respond ~r~without lights or sirens!");
            return base.OnCalloutAccepted();
        }
        public override void OnCalloutNotAccepted()
        {
            foreach(Entity e in CalloutEnts)
            {
                if(e.Exists() && e.IsValid())
                {
                    e.Delete();
                }
            }
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                GameEnd();
                try
                {
                    if (Game.LocalPlayer.Character.DistanceTo(SusCar) <= 100f && FollowSus == false)
                    {
                        Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to responding Unit" + "", "The ~r~suspect~s~ is moving! Follow them, but remain out of sight for now.");
                        Harasser.Tasks.DriveToPosition(DropOffPoint, 30f, VehicleDrivingFlags.Normal | VehicleDrivingFlags.StopAtDestination);
                        CarBlip = SusCar.AttachBlip();
                        CarBlip.Sprite = BlipSprite.GangVehicle;
                        CarBlip.Color = Color.Red;
                        DrawBar = true;
                        FollowSus = true;
                    }
                    if (DrawBar)
                    {
                        AwarenessBar.Percentage = Awareness;
                        AwarenessBar.ForegroundColor = Color.Red;
                        AwarenessBar.BackgroundColor = ControlPaint.Dark(Color.Red);
                        AwarenessBarPool.Draw();
                    }
                    if (Game.LocalPlayer.Character.DistanceTo(SusCar) <= 20f && FollowSus == true)
                    {
                        CarBlip.Flash(500, -1);
                        Awareness += 0.0050f;
                    }
                    else
                    {
                        CarBlip.StopFlashing();
                    }
                    if (Game.LocalPlayer.Character.CurrentVehicle.IsSirenOn == true && FollowSus == true)
                    {
                        Awareness += 1f;
                    }
                    if (Awareness >= 1f && FollowSus == true)
                    {
                        Game.DisplayHelp("You alerted the Driver!");
                        Chase = Functions.CreatePursuit();
                        Functions.AddPedToPursuit(Chase, Harasser);
                        Functions.SetPursuitIsActiveForPlayer(Chase, true);
                        ChaseActive = true;
                        FollowSus = false;
                    }
                    if (ChaseActive == true && !Functions.IsPursuitStillRunning(Chase))
                    {
                        this.End();
                    }
                }
                catch(Exception e)
                {
                    Game.LogTrivial("Regular Callouts Error: " + e);
                }
            });
            base.Process();
        }
        public override void End()
        {
            foreach (Entity e in CalloutEnts)
            {
                if (e.Exists() && e.IsValid())
                {
                    e.Dismiss();
                }
            }
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (CarBlip.Exists()) { CarBlip.Delete(); }
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
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
