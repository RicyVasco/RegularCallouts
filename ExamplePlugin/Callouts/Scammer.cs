using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using RAGENativeUI;
using RegularCallouts.Stuff;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Scammer", CalloutProbability.Medium)]
    public class Scammer : Callout
    {
        private Vector3 SpawnPoint;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SpawnPoint5;
        private Vector3 SpawnPoint6;
        private Vector3 DoorLocation;
        private Ped Scammer1;
        private Ped Scammer2;
        private Vehicle ScammerCar;
        private Ped HomeOwner;
        private Blip SpawnBlip;
        private List<Entity> CalloutEnts;
        private string CalloutIs;
        private bool CleanUp;
        private static string[] CalloutPossibility = new string[] { "Talking", "TalkingEscape", "Fleeing", "Fighting" };

        #region FleeStuff
        private LHandle ScammerFlee;
        private bool PursuitActivated;
        #endregion
        #region DialogStuff
        private int Dialog = 1;
        private int Dialog2 = 1;
        private int Dialog3 = 1;
        private int Dialog4 = 1;
        private bool ShowEndTip;
        private int DoorCheckPoint;
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private bool Angeklingelt;
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            CalloutEnts = new List<Entity>();
            SpawnPoint1 = new Vector3(2166.98096f, 3381.8877f, 47.6721992f);
            SpawnPoint2 = new Vector3(-2994.63306f, 682.352844f, 25.0375881f);
            SpawnPoint3 = new Vector3(-371.351776f, 343.59726f, 109.926628f);
            SpawnPoint4 = new Vector3(-1225.89038f, -1208.24316f, 8.2638092f);
            SpawnPoint5 = new Vector3(1265.60413f, -648.369385f, 67.9176025f);
            SpawnPoint6 = new Vector3(-407.303223f, 6314.15137f, 28.9449997f);
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

            CalloutMessage = "Potential Scammer at House";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 40f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
            CalloutIs = CalloutPossibility[new Random().Next(CalloutPossibility.Length)];
            if(SpawnPoint == SpawnPoint1)
            {
                ScammerCar = new Vehicle("pony", new Vector3(2167.54956f, 3365.42554f, 45.3678932f), 119.180183f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(2167.11426f, 3378.06494f, 46.43116f), 39.4428558f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(2168.0957f, 3380.50391f, 46.4313545f), 104.229622f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(2166.35132f, 3380.11011f, 46.4304466f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            else if (SpawnPoint == SpawnPoint2)
            {
                ScammerCar = new Vehicle("pony", new Vector3(-3008.97754f, 688.794373f, 22.5496197f), 51.1378212f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(-2996.84888f, 687.305786f, 25.4620171f), -151.691284f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(-2998.29907f, 684.794067f, 25.1066818f), -95.3050156f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(-2994.63306f, 682.352844f, 25.0375881f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            else if (SpawnPoint == SpawnPoint3)
            {
                ScammerCar = new Vehicle("pony", new Vector3(-381.493591f, 348.797852f, 108.882111f), 35.163002f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(-373.105499f, 347.649078f, 109.320389f), -151.146088f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(-370.237762f, 348.272369f, 109.387268f), -173.838638f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(-371.351776f, 343.59726f, 109.926628f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            else if (SpawnPoint == SpawnPoint4)
            {
                ScammerCar = new Vehicle("pony", new Vector3(-1217.76941f, -1209.11499f, 7.47653723f), -151.726868f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(-1221.96387f, -1208.97339f, 7.69116449f), 77.139061f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(-1222.66101f, -1206.06531f, 7.68350601f), 134.910263f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(-1225.89038f, -1208.24316f, 8.2638092f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            else if (SpawnPoint == SpawnPoint5)
            {
                ScammerCar = new Vehicle("pony", new Vector3(1277.50513f, -654.121277f, 67.4503326f), -85.264328f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(1268.22131f, -646.324463f, 67.8697433f), 130.986008f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(1266.57385f, -644.786987f, 67.9098206f), 165.717148f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(1265.60413f, -648.369385f, 67.9176025f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            else if (SpawnPoint == SpawnPoint6)
            {
                ScammerCar = new Vehicle("pony", new Vector3(-396.273499f, 6311.74121f, 28.7473392f), -141.914917f);
                Scammer1 = new Ped("s_m_m_postal_01", new Vector3(-404.377502f, 6312.62305f, 28.9519768f), 78.3542557f);
                Scammer1.BlockPermanentEvents = true;
                Scammer2 = new Ped("s_m_m_postal_01", new Vector3(-404.333252f, 6313.89941f, 28.9504299f), 95.1567459f);
                Scammer2.BlockPermanentEvents = true;
                DoorLocation = Utils.ToGround(new Vector3(-407.303223f, 6314.15137f, 28.9449997f));
                CalloutEnts.Add(ScammerCar);
                CalloutEnts.Add(Scammer1);
                CalloutEnts.Add(Scammer2);
                SpawnBlip = new Blip(SpawnPoint);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.EnableRoute(Color.Yellow);
            }
            Game.DisplayHelp($"Press ~{Settings.EndCalloutKey.GetInstructionalId()}~ to end the Callout anytime.");
            Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to responding Unit" + "", "~y~Caller~s~ was advised to stay indoors. Respond ~r~Code 3");
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
                if(CalloutIs == "Fleeing")
                {
                    try
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 200f && !CleanUp)
                        {
                            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
                            CleanUp = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 15f && !PursuitActivated)
                        {
                            Game.DisplaySubtitle("~r~Scammer:~s~ Oh shit the cops!");
                            ScammerFlee = Functions.CreatePursuit();
                            Functions.AddPedToPursuit(ScammerFlee, Scammer1);
                            Functions.AddPedToPursuit(ScammerFlee, Scammer2);
                            Functions.SetPursuitIsActiveForPlayer(ScammerFlee, true);
                            SpawnBlip.Delete();
                            PursuitActivated = true;
                        }
                        else if(Functions.IsPursuitStillRunning(ScammerFlee) == false)
                        {
                            this.End();
                        }
                    }
                    catch(Exception e)
                    {
                        Game.LogTrivial("Regular Callouts - Scammer: " + e);
                    }
                    
                }
                if (CalloutIs == "Fighting")
                {
                    try
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 200f && !CleanUp)
                        {
                            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
                            CleanUp = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 7f && !PursuitActivated)
                        {
                            Game.DisplaySubtitle("~r~Scammer:~s~ Oh shit the cops!");
                            ScammerFlee = Functions.CreatePursuit();
                            Scammer1.Inventory.GiveNewWeapon(WeaponHash.Knife, 1, true);
                            Scammer1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            Scammer2.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            SpawnBlip.Delete();
                        }
                        else if (Scammer1.IsDead || Scammer1.IsCuffed && Scammer2.IsDead || Scammer2.IsCuffed)
                        {
                            this.End();
                        }
                    }
                    catch (Exception e)
                    {
                        Game.LogTrivial("Regular Callouts - Scammer: " + e);
                    }

                }
                if (CalloutIs == "Talking")
                {
                    try
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 200f && !CleanUp)
                        {
                            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
                            CleanUp = true;
                        }
                        if (Dialog < 6)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(Scammer1) <= 2f || Game.LocalPlayer.Character.DistanceTo(Scammer2) <= 2f)
                            {
                                Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the scammers.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Scammer1.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    Scammer2.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    switch (Dialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~b~You:~s~ Hello Gentlemen, can I ask what you're doing here?");
                                            SpawnBlip.Delete();
                                            Dialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~r~Scammer:~s~ We are just doing a basic survey, nothing illegal about that.");
                                            Dialog++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~b~You:~s~ Sure. Do you have identification on you?");
                                            Dialog++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~r~Scammer:~s~ Yeah it's in the car. We have to grab it.");
                                            Dialog++;
                                            break;
                                        case 5:
                                            Game.DisplaySubtitle("~b~You:~s~ Wait by the car for me.");
                                            Scammer1.Tasks.FollowNavigationMeshToPosition(ScammerCar.GetOffsetPosition(new Vector3(2f, -2f, 0f)), ScammerCar.Position.X, 1f);
                                            Scammer2.Tasks.FollowNavigationMeshToPosition(ScammerCar.GetOffsetPosition(new Vector3(2f, 2f, 0f)), ScammerCar.Position.X, 1f);
                                            DoorCheckPoint = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, DoorLocation, DoorLocation, 2f, Color.DarkBlue);
                                            Utils.SetCheckpointHeight(DoorCheckPoint, 2f, 2f, 2f);
                                            Dialog++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        if (Dialog >= 6)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(DoorLocation) <= 2f && Angeklingelt == false)
                            {
                                Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Utils.DeleteCheckpoint(DoorCheckPoint);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    HomeOwner = new Ped(DoorLocation);
                                    HomeOwner.BlockPermanentEvents = true;
                                    CalloutEnts.Add(HomeOwner);
                                    HomeOwner.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(600);
                                    Angeklingelt = true;
                                }
                            }
                            if (Dialog2 < 6 && Angeklingelt == true)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(HomeOwner) <= 2f)
                                {
                                    Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the homeowner.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        HomeOwner.TurnToFaceEntity(Game.LocalPlayer.Character);
                                        switch (Dialog2)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Hello. Did you call us?");
                                                Dialog2++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~y~Home Owner:~s~ Yes Sir, those people rang and demanded payment because I ordered something apparently!~n~Obviously I did not order anything and they refused to show me the order.");
                                                Dialog2++;
                                                break;
                                            case 3:
                                                Game.DisplaySubtitle("~b~You:~s~ Did they threaten you in any way?");
                                                Dialog2++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~y~Home Owner:~s~ They kept banging the door loudly when I closed it and threatened to sue me, but no physical violence.");
                                                Dialog2++;
                                                break;
                                            case 5:
                                                Game.DisplaySubtitle("~b~You:~s~ Alright hang tight for me.");
                                                Dialog2++;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            if (Dialog2 >= 6)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(Scammer1) <= 2f && Dialog3 < 5)
                                {
                                    Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the homeowner.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Scammer1.TurnToFaceEntity(Game.LocalPlayer.Character);
                                        switch(Dialog3)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Alright let me see your ID.");
                                                Scammer1.ShowID();
                                                Dialog3++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~b~You:~s~ Ok Mister "+Functions.GetPersonaForPed(Scammer1).FullName.ToString()+", wanna tell me the truth about your scam?");
                                                Dialog3++;
                                                break;
                                            case 3:
                                                Game.DisplaySubtitle("~r~Scammer:~s~ It was all his idea! He said we would make quick money!");
                                                Dialog3++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~b~You:~s~ You should be ashamed of your self, I'll make sure you get proper punishment");
                                                Dialog3++;
                                                break;
                                        }
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(Scammer2) <= 2f && Dialog4 < 5)
                                {
                                    Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the homeowner.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Scammer2.TurnToFaceEntity(Game.LocalPlayer.Character);
                                        switch (Dialog4)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Alright let me see your ID.");
                                                Scammer2.ShowID();
                                                Dialog4++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~b~You:~s~ Ok Mister " + Functions.GetPersonaForPed(Scammer1).FullName.ToString() + ", wanna tell me the truth about your scam?");
                                                Dialog4++;
                                                break;
                                            case 3:
                                                Game.DisplaySubtitle("~r~Scammer:~s~ It was my idea. I wanted to make some quick buck. Nothing else I promise.");
                                                Dialog4++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~b~You:~s~ You should be ashamed of your self, I'll make sure you get proper punishment");
                                                Dialog4++;
                                                break;
                                        }
                                    }
                                }
                                if (Dialog4 >= 5 && Dialog3 >= 5 && !ShowEndTip)
                                {
                                    Game.DisplayHelp($"Press ~{Settings.EndCalloutKey.GetInstructionalId()}~ when you are done to end the callout.");
                                    ShowEndTip = true;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Game.LogTrivial("Regular Callouts - Scammer: " + e);
                    }

                }
                if (CalloutIs == "TalkingEscape")
                {
                    try
                    {
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 200f && !CleanUp)
                        {
                            Utils.ClearUnrelatedEntities(SpawnPoint, CalloutEnts);
                            CleanUp = true;
                        }
                        if (Dialog < 6)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(Scammer1) <= 2f || Game.LocalPlayer.Character.DistanceTo(Scammer2) <= 2f)
                            {
                                Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the scammers.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Scammer1.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    Scammer2.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    switch (Dialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~b~You:~s~ Hello Gentlemen, can I ask what you're doing here?");
                                            SpawnBlip.Delete();
                                            Dialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~r~Scammer:~s~ We are just doing a basic survey, nothing illegal about that.");
                                            Dialog++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~b~You:~s~ Sure. Do you have identification on you?");
                                            Dialog++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~r~Scammer:~s~ Yeah it's in the car. We have to grab it.");
                                            Dialog++;
                                            break;
                                        case 5:
                                            Game.DisplaySubtitle("~b~You:~s~ Wait by the car for me.");
                                            Scammer1.Tasks.FollowNavigationMeshToPosition(ScammerCar.GetOffsetPosition(new Vector3(2f, -2f, 0f)), ScammerCar.Position.X, 1f);
                                            Scammer2.Tasks.FollowNavigationMeshToPosition(ScammerCar.GetOffsetPosition(new Vector3(2f, 0f, 0f)), ScammerCar.Position.X, 1f);
                                            DoorCheckPoint = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, DoorLocation, DoorLocation, 2f, Color.DarkBlue);
                                            Utils.SetCheckpointHeight(DoorCheckPoint, 2f, 2f, 2f);
                                            Dialog++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        if (Dialog >= 6)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(DoorLocation) <= 2f && Angeklingelt == false)
                            {
                                Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Utils.DeleteCheckpoint(DoorCheckPoint);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    HomeOwner = new Ped(DoorLocation);
                                    HomeOwner.BlockPermanentEvents = true;
                                    CalloutEnts.Add(HomeOwner);
                                    HomeOwner.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(600);
                                    Angeklingelt = true;
                                }
                            }
                            if (Dialog2 < 3 && Angeklingelt == true)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(HomeOwner) <= 2f)
                                {
                                    Game.DisplayHelp($"Press ~{Settings.DialogKey.GetInstructionalId()}~ to talk to the homeowner.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        HomeOwner.TurnToFaceEntity(Game.LocalPlayer.Character);
                                        switch (Dialog2)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Hello. Did you call us?");
                                                Dialog2++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~y~Home Owner:~s~ Yes Sir, those people rang... Officer they are trying to flee!");
                                                Scammer1.Tasks.EnterVehicle(ScammerCar, -1, 3f);
                                                Scammer2.Tasks.EnterVehicle(ScammerCar, 0, 3f);
                                                Dialog2++;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            if (Dialog2 >= 3)
                            {
                                if (Scammer1.IsInAnyVehicle(false) && Scammer2.IsInAnyVehicle(false) && !PursuitActivated)
                                {
                                    ScammerFlee = Functions.CreatePursuit();
                                    Functions.AddPedToPursuit(ScammerFlee, Scammer1);
                                    Functions.AddPedToPursuit(ScammerFlee, Scammer2);
                                    Functions.SetPursuitIsActiveForPlayer(ScammerFlee, true);
                                    PursuitActivated = true;
                                }
                                else if (Functions.IsPursuitStillRunning(ScammerFlee) == false)
                                {
                                    this.End();
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Game.LogTrivial("Regular Callouts - Scammer: " + e);
                    }

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
            Utils.DeleteCheckpoint(DoorCheckPoint);
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
