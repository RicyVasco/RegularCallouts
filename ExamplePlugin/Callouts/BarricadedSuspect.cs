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
    [CalloutInfo("Barricaded Suspect", CalloutProbability.Medium)]
    public class BarricadedSuspect : Callout
    {
        private bool CalloutRunning;
        private bool SuperiorBlipped;
        private bool NegotiationHangup;
        private bool SpokenWithSuperior;
        private bool SpokenWithSuperior2;
        private bool NegoFirstAnswer;
        private bool NegotiateWithSuspect;
        private bool TaterReaction1;
        private LHandle Pursuit;
        private bool GetAway4;
        private bool SendPeopleIn;
        private bool SendSwatIn;
        private bool MobileAnruf;
        private bool GoBackToSupervisor;
        private bool SuspectComesOutShooting;
        private bool SuspectComesOutShooting2;
        private bool SuspectSurrenders1;
        private bool GetAway1;
        private bool GetAway2;
        private bool GetAway3;
        private bool GetAway5;
        private bool Relative1;
        private bool Relative2;
        private bool Relative3;
        private bool SuspectSurrenders2;
        private bool SupervisorSendsPeopleIn;
        private bool SupervisorInDone;
        private Vector3 SpawnPoint;
        private Ped Parent;
        private Vehicle GetAwayCar;
        private Vector3 GetAwayDesignation;
        private Vehicle TransportCar;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private Vector3 SuspectSpawn;
        private Vector3 SuperiorSpawn;
        private Ped Superior;
        private Ped Suspect;
        private Blip SpawnBlip;
        private Blip SuperiorBlip;

        private List<Entity> AllBankHeistEntities = new List<Entity>();
        private static string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private static string[] CarModels = new string[] { "POLICE", "POLICE2", "POLICE3" };

        private List<Vector3> PoliceOfficersLocations;
        private List<float> PoliceOfficersHeadings;
        private List<Ped> PoliceOfficersSpawned = new List<Ped>();

        private List<Ped> SuspectSpawned = new List<Ped>();

        //Swat teams
        private List<Vector3> SWATAimingTeamLocations;
        private List<float> SWATAimingTeamHeadings;
        private List<Ped> SWATAimingTeam = new List<Ped>();
        private string[] SWATWeapons = new string[] { "WEAPON_CARBINERIFLE", "WEAPON_ASSAULTSMG" };
        private List<Ped> SWATUnitsSpawned = new List<Ped>();
        private List<Vector3> SWATFollowTeamLocations;
        private List<float> SWATFollowTeamHeadings;
        private List<Ped> SWATFollowTeam = new List<Ped>();

        private Vector3 GetAwaySpawn;

        //EMS &Fire
        private List<Vector3> AmbulanceLocations;
        private List<float> AmbulanceHeadings;
        private List<Vehicle> AmbulancesList = new List<Vehicle>();

        private List<Vector3> ParamedicLocations;
        private List<float> ParamedicHeadings;
        private List<Ped> ParamedicsList = new List<Ped>();

        private List<Vector3> FireTruckLocations;
        private List<float> FireTruckHeadings;
        private List<Vehicle> FireTrucksList = new List<Vehicle>();
        private List<Ped> FiremenList = new List<Ped>();

        //Police Barriers
        private List<Ped> BarrierPeds = new List<Ped>();
        private List<Rage.Object> InvisWalls = new List<Rage.Object>();
        private List<Rage.Object> Barriers = new List<Rage.Object>();
        private List<Vector3> BarrierLocations;
        private List<float> BarrierHeadings;

        //Police Vehicles
        private List<Vector3> POLICECarLocations;
        private List<float> POLICECarHeadings;
        private List<Vector3> FBICarLocations;
        private List<float> FBICarHeadings;
        private List<Vector3> RiotLocations;
        private List<float> RiotHeadings;
        private List<Vector3> TransporterLocations;
        private List<float> TransporterHeading;
        private List<Vehicle> AllSpawnedPoliceVehicles = new List<Vehicle>();

        //DialogWindow
        private List<string> AlarmAnswers = new List<string>() { "You: I wanna try calling the suspect.", "You: Enough holding out, let's breach!", "You: Have you tried contacting a relative?" };

        private List<string> PhoneAnswers = new List<string>() { "You: This is the Police. We have you surrounded. Come out with your hands up!", "You: I'm a Police Officer. How can we make this end peacefully?" };

        private List<string> NegotiateAnswers = new List<string>() { "You: I can get you an escape vehicle if you leave your guns on the ground.", "You: I can't offer you anything. It's gonna end either way.", "You: I can't promise anything. But if you surrender I will make sure to support your case with the DA" };

        private List<string> DialogWithSuperior = new List<string>
        {
            "~g~Supervisor:~s~ Hello Officer. The suspect resisted an arrest warrant earlier and opened fire on the officers executing the warrant. (1/5)",
            "~g~Supervisor:~s~ The suspect is still in the house but he locked himself in. We only setup a perimeter and we are waiting for your command. (2/5)",
            "~b~You:~s~ What are my options? (3/5)",
            "~g~Supervisor:~s~ You can try to call the suspect and see if he agrees to surrender. Otherwise we might've to storm the building. (4/5)",
            "~b~You:~s~ Alright let me think about it for a second (5/5)"

        };
        private int DialogWithSuperiorIndex;

        private List<string> DialogWithParent = new List<string>
        {
            "~p~Parent:~s~ I heard what happened Officer. I will do anything to help. (1/3)",
            "~b~You:~s~ Could you call them and tell them to surrender? (2/3)",
            "~p~Parent:~s~ Absolutely, give me a second. (3/3)"

        };
        private int DialogWithParentIndex;

        private List<string> DialogWithSuperiorCallSuspect = new List<string>
        {
            "~g~Supervisor:~s~ Alright good luck with that Officer. (2/2)"
        };
        private int DialogWithSuperiorCallSuspectIndex;

        private List<string> DialogWithSuspectGetaway = new List<string>
        {
            "~r~Suspect:~s~ Alright I'll come back once the vehicle is in front of the house. (2/3)",
            "~b~You:~s~ Sure hang tight for me. ~y~Hangs up.~s~ (3/3)"
        };
        private int DialogWithSuspectGetawayIndex;

        private List<string> DialogWithSuperiorSuspectHangedUp = new List<string>
        {
            "~b~You:~s~ The suspect hang up. What should we do now? (1/2)",
            "~g~Supervisor:~s~ You had your chance. I'm sending people in. (2/2)"
        };
        private int DialogWithSuperiorSuspectHangedUpIndex;
        private List<string> DialogWithSuspect = new List<string>
        {
            "~r~Suspect:~s~ I want all of you to leave! (1/3)",
            "~b~You:~s~ I'm afraid this won't happen. Is there anything else I could do? (2/3)",
            "~r~Suspect:~s~ Give me some options"
        };
        private int DialogWithSuspectIndex;

        public override bool OnBeforeCalloutDisplayed()
        {

            SpawnPoint1 = new Vector3(1326.56006f, -538.609314f, 72.1684799f);
            SpawnPoint2 = new Vector3(91.8354721f, 3732.91699f, 39.7218361f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                AmbulanceLocations = new List<Vector3>() { new Vector3(1309.572884f, -558.768311f, 71.2965012f) };
                AmbulanceHeadings = new List<float>() { -107.810974f };
                ParamedicLocations = new List<Vector3>() { new Vector3(1309.12354f, -561.3172f, 71.7345352f), new Vector3(1310.72986f, -561.814819f, 71.8579559f) };
                ParamedicHeadings = new List<float>() { -108.709549f, 76.2984238f };
                FireTruckLocations = new List<Vector3>() { new Vector3(1291.61133f, -556.554199f, 70.4374313f) };
                FireTruckHeadings = new List<float>() { -103.667336f };
                PoliceOfficersLocations = new List<Vector3>() { new Vector3(1298.787f, -553.5432f, 70.76094f), new Vector3(1325.97546f, -567.641663f, 73.046402f), new Vector3(1324.18579f, -566.66333f, 72.8253784f), new Vector3(1310.72986f, -561.814819f, 71.8579559f) };
                PoliceOfficersHeadings = new List<float>() { 74.70585f, 60.4133682f, -119.570435f, -116.622841f };
                SWATAimingTeamLocations = new List<Vector3>() { new Vector3(1338.81836f, -564.892273f, 73.895256f), new Vector3(1333.37097f, -557.329346f, 73.1342926f), new Vector3(1318.39819f, -553.910889f, 72.1531372f)};
                SWATAimingTeamHeadings = new List<float>() { 16.6002483f, 13.9149809f, -43.8569603f};
                SWATFollowTeamLocations = new List<Vector3>() { new Vector3(1314.81128f, -552.560974f, 71.8649902f), new Vector3(1316.25562f, -553.069519f, 71.9788742f), new Vector3(1315.203f, -553.718506f, 71.9476166f) };
                SWATFollowTeamHeadings = new List<float>() { -106.306274f, 66.8353271f, -17.2670307f };
                BarrierLocations = new List<Vector3>() { new Vector3(1298.16602f, -555.042236f, 69.6660767f), new Vector3(1342.7572f, -562.842102f, 72.9636688f), new Vector3(1340.95923f, -566.372742f, 73.0828552f), new Vector3(1339.52319f, -569.708862f, 72.9746628f)};
                BarrierHeadings = new List<float>() { -104.992409f, -115.125008f, -115.125008f, -115.125008f, 25.01121f, 1.558372f, 255.0954f, 255.0954f, 267.3944f };
                POLICECarLocations = new List<Vector3>() { new Vector3(1300.80994f, -555.419739f, 70.5500488f), new Vector3(1339.64514f, -561.598572f, 73.4075394f) };
                POLICECarHeadings = new List<float>() { -13.5922108f, 155.17308f};
                FBICarLocations = new List<Vector3>() { new Vector3(1317.20886f, -551.181641f, 71.5538254f)};
                FBICarHeadings = new List<float>() { -101.410675f};
                RiotLocations = new List<Vector3>() { new Vector3(1329.64075f, -554.011047f, 73.112915f), new Vector3(1325.92395f, -564.43573f, 73.0535126f) };
                RiotHeadings = new List<float>() { 37.0742493f, -111.657524f };
                TransporterLocations = new List<Vector3>() { new Vector3(1318.52075f, -561.894958f, 72.5897446f) };
                TransporterHeading = new List<float>() { -109.892479f };
                SuspectSpawn = new Vector3(1328.31006f, -535.513123f, 72.4376221f);
                SuperiorSpawn = new Vector3(1328.00037f, -555.274048f, 72.7297287f);
                GetAwaySpawn = new Vector3(1271.633f, -575.767f, 69.01827f);
                GetAwayDesignation = new Vector3(1322.584f, -558.3494f, 71.86897f);
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                AmbulanceLocations = new List<Vector3>() { new Vector3(102.111046f, 3728.94751f, 39.4410553f) };
                AmbulanceHeadings = new List<float>() { 29.3038826f };
                ParamedicLocations = new List<Vector3>() { new Vector3(103.815895f, 3724.02148f, 39.6806755f), new Vector3(105.68132f, 3723.98413f, 39.6800652f) };
                ParamedicHeadings = new List<float>() { -84.8187256f, 84.4330063f };
                FireTruckLocations = new List<Vector3>() { new Vector3(83.5297623f, 3733.88013f, 39.7871513f) };
                FireTruckHeadings = new List<float>() { 54.3083611f };
                PoliceOfficersLocations = new List<Vector3>() { new Vector3(106.017494f, 3717.84277f, 39.7273483f), new Vector3(70.3314819f, 3739.77954f, 39.7230072f), new Vector3(107.053223f, 3724.28296f, 39.5734749f), new Vector3(86.3677139f, 3738.13403f, 39.7136002f), new Vector3(87.8371735f, 3736.96021f, 39.7010651f) };
                PoliceOfficersHeadings = new List<float>() { -179.054718f, 105.070465f, 83.0382767f, -122.834854f, 51.1730003f };
                SWATAimingTeamLocations = new List<Vector3>() { new Vector3(105.648239f, 3747.16772f, 39.7557793f), new Vector3(91.6509933f, 3736.63574f, 39.6622849f), new Vector3(92.3508072f, 3728.03174f, 39.5602074f), new Vector3(101.933731f, 3734.39063f, 39.5009995f) };
                SWATAimingTeamHeadings = new List<float>() { 103.487572f, -4.00085068f, -9.54402351f, 30.3847637f };
                SWATFollowTeamLocations = new List<Vector3>() { new Vector3(87.9595184f, 3727.62036f, 39.6112823f), new Vector3(88.442215f, 3726.39307f, 39.6454468f), new Vector3(89.6908035f, 3727.19922f, 39.5661278f) };
                SWATFollowTeamHeadings = new List<float>() { -99.6055298f, -12.6447334f, 80.4651794f };
                BarrierLocations = new List<Vector3>() { new Vector3(107.471664f, 3717.32568f, 38.7032166f), new Vector3(99.2292099f, 3716.96411f, 38.6478424f), new Vector3(95.8770752f, 3718.07373f, 38.5994568f), new Vector3(69.9952927f, 3736.91699f, 38.6949005f), new Vector3(68.7022324f, 3741.69482f, 38.7851105f) };
                BarrierHeadings = new List<float>() { 2.83466101f, 2.83466101f, -29.8188324f, 105.731712f, 105.731712f, 1.558372f, 255.0954f, 255.0954f, 267.3944f };
                POLICECarLocations = new List<Vector3>() { new Vector3(107.214226f, 3719.27002f, 39.3736992f), new Vector3(72.0164566f, 3740.19971f, 39.3891716f), new Vector3(103.613045f, 3745.04321f, 39.4243889f) };
                POLICECarHeadings = new List<float>() { 91.9239502f, -166.071899f, -6.25951576f };
                FBICarLocations = new List<Vector3>() { new Vector3(89.5262222f, 3729.92285f, 39.2636795f) };
                FBICarHeadings = new List<float>() { 56.4068527f };
                RiotLocations = new List<Vector3>() { new Vector3(97.4947662f, 3734.31445f, 39.8317337f) };
                RiotHeadings = new List<float>() { 88.3440018f };
                TransporterLocations = new List<Vector3>() { new Vector3(88.346138f, 3738.75195f, 40.0269165f) };
                TransporterHeading = new List<float>() { 58.5520859f };
                SuspectSpawn = new Vector3(93.8639755f, 3744.1665f, 39.8019714f);
                SuperiorSpawn = new Vector3(96.823967f, 3732.49048f, 39.7060547f);
                GetAwaySpawn = new Vector3(84.2662735f, 3698.87549f, 38.9874687f);
                GetAwayDesignation = new Vector3(91.8354721f, 3732.91699f, 39.7218361f);
            }

            CalloutMessage = "Barricaded Suspect";
            CalloutPosition = SpawnPoint;
            AddMinimumDistanceCheck(60f, SpawnPoint);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            CalloutRunning = true;
            GameFiber.StartNew(delegate
            {
                try
                {
                    SpawnBlip = new Blip(SpawnPoint, 40f);
                    SpawnBlip.Color = Color.Yellow;
                    SpawnBlip.Alpha = 0.5f;
                    SpawnBlip.EnableRoute(Color.Yellow);
                    LoadModels();
                    while (Vector3.Distance(Game.LocalPlayer.Character.Position, SpawnPoint) > 350f)
                    {
                        GameFiber.Yield();
                        GameEnd();
                    }
                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        AllBankHeistEntities.Add(Game.LocalPlayer.Character.CurrentVehicle);
                        Ped[] passengers = Game.LocalPlayer.Character.CurrentVehicle.Passengers;
                        if (passengers.Length > 0)
                        {
                            foreach (Ped passenger in passengers)
                            {
                                AllBankHeistEntities.Add(passenger);
                            }
                        }
                    }
                    GameFiber.Yield();
                    CreateSpeedZone();
                    ClearUnrelatedEntities();
                    Game.LogTrivial("Unrelated entities cleared");
                    GameFiber.Yield();
                    SpawnSuspect();
                    GameFiber.Yield();
                    SpawnAllBarriers();
                    GameFiber.Yield();
                    SpawnAllPoliceCars();
                    GameFiber.Yield();
                    SpawnBothSwatTeams();
                    GameFiber.Yield();
                    SpawnAllPoliceOfficers();
                    GameFiber.Yield();
                    SpawnEMSAndFire();
                    GameFiber.Yield();
                    SpawnSuperior();
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    Game.LogTrivial("Initialisation complete, entering loop");
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();

                        Game.LocalPlayer.Character.CanAttackFriendlies = false;
                        Game.SetRelationshipBetweenRelationshipGroups("COP", "SUSPECT", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "COP", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("SUSPECT", "PLAYER", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "SUSPECT", Relationship.Hate);
                        Game.SetRelationshipBetweenRelationshipGroups("COP", "PLAYER", Relationship.Respect);
                        Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "COP", Relationship.Respect);
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 30f && SuperiorBlipped == false)
                        {
                            SpawnBlip.Delete();
                            SuperiorBlip = Superior.AttachBlip();
                            SuperiorBlip.Color = Color.Green;
                            Superior.Face(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Speak with your ~g~Supervisor~s~ to plan your tactics.");
                            SuperiorBlipped = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Superior) <= 2f && SuperiorBlipped == true && SpokenWithSuperior == false && SendSwatIn == false && Relative1 == false)
                        {
                            Superior.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSuperiorIndex < DialogWithSuperior.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSuperior[DialogWithSuperiorIndex]);
                                    DialogWithSuperiorIndex++;
                                }
                                if (DialogWithSuperiorIndex == DialogWithSuperior.Count)
                                {
                                    int negresult = Utils.DisplayAnswers(AlarmAnswers);
                                    if (negresult == 0)
                                    {
                                        if (NegoFirstAnswer == false)
                                        {
                                            Game.DisplaySubtitle("~b~You:~s~ I wanna try calling the suspect. (1/2)");
                                            SpokenWithSuperior = true;
                                        }
                                    }
                                    else if (negresult == 1)
                                    {
                                        Game.DisplaySubtitle("~b~You:~s~ Enough holding out, let's breach!");
                                        Game.DisplayNotification("~b~Attention to SWAT Units:~s~ You are cleared to enter the building.");
                                        foreach (Ped i in SWATFollowTeam)
                                        {
                                            i.Tasks.PlayAnimation("anim@weapons@rifle@lo@dbshot", "aim_med_loop", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly);
                                            i.Tasks.FollowNavigationMeshToPosition(SuspectSpawn, 1f, 1f);
                                        }
                                        SendSwatIn = true;
                                    }
                                    else if (negresult == 2)
                                    {
                                        Game.DisplaySubtitle("~b~You:~s~ Have you tried contacting a relative?");
                                        Relative1 = true;
                                    }
                                }
                            }
                        }
                        if (Relative1 == true && Relative2 == false)
                        {
                            Superior.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplaySubtitle("~g~Supervisor:~s~ One of their parents is already on the way.");
                            SuperiorBlip.Delete();
                            SpawnParent();
                            Relative2 = true;
                        }
                        if (Relative2 == true && SuspectSurrenders2 == false)
                        {
                            if(Parent.IsInAnyVehicle(false) == false && SuperiorBlip.Exists() == false)
                            {
                                SuperiorBlip = Parent.AttachBlip();
                                SuperiorBlip.Color = Color.Purple;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(Parent) <= 2f)
                            {
                                Parent.TurnToFaceEntity(Game.LocalPlayer.Character);
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogWithParentIndex < DialogWithParent.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithParent[DialogWithParentIndex]);
                                        DialogWithParentIndex++;
                                    }
                                    if (DialogWithParentIndex == DialogWithParent.Count)
                                    {
                                        ToggleMobilePhone(Parent, true);
                                        GameFiber.Sleep(5000);
                                        ToggleMobilePhone(Parent, false);
                                        SuperiorBlip.Delete();
                                        Game.DisplaySubtitle("~p~Parent:~s~ They are coming out. (1/1)");
                                        Suspect.IsVisible = true;
                                        Suspect.Face(Superior);
                                        Suspect.Tasks.PlayAnimation("busted", "idle_2_hands_up_2h", 1f, AnimationFlags.StayInEndFrame | AnimationFlags.SecondaryTask);
                                        Suspect.Inventory.EquippedWeapon.Drop();
                                        SuspectSurrenders2 = true;
                                    }
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Superior) <=2f && SpokenWithSuperior == true && SpokenWithSuperior2 == false)
                        {
                            Superior.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSuperiorCallSuspectIndex < DialogWithSuperiorCallSuspect.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSuperiorCallSuspect[DialogWithSuperiorCallSuspectIndex]);
                                    DialogWithSuperiorCallSuspectIndex++;
                                }
                                if (DialogWithSuperiorIndex == DialogWithSuperior.Count)
                                {
                                    Superior.Tasks.AimWeaponAt(SuspectSpawn, -1);
                                    SuperiorBlip.Delete();
                                    GameFiber.Sleep(1);
                                    MobileAnruf = true;
                                    SpokenWithSuperior2 = true;
                                }
                            }
                        }
                        if (MobileAnruf == true && TaterReaction1 == false)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to call the Suspect.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                ToggleMobilePhone(Game.LocalPlayer.Character, true);
                                Game.DisplaySubtitle("~y~Calling...", 3000);
                                GameFiber.Wait(3000);
                                Game.DisplaySubtitle("~r~Suspect:~s~ Who's there?");
                                TaterReaction1 = true;
                            }
                        }
                        if (TaterReaction1 == true && NegotiationHangup == false && NegotiateWithSuspect == false)
                        {
                            int phoneresult = Utils.DisplayAnswers(PhoneAnswers);
                            if (phoneresult == 0)
                            {
                                Game.DisplaySubtitle("~b~You:~s~ This is the Police. We have you surrounded. Come out with your hands up! (1/2)");
                                NegotiationHangup = true;
                            }
                            else if (phoneresult == 1)
                            {
                                Game.DisplaySubtitle("~b~You:~s~ I'm a Police Officer. How can we make this end peacefully?");
                                NegotiateWithSuspect = true;
                            }
                        }
                        if (NegotiateWithSuspect && SuspectComesOutShooting == false && SuspectSurrenders1 == false && GetAway1 == false)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSuspectIndex < DialogWithSuspect.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSuspect[DialogWithSuspectIndex]);
                                    DialogWithSuspectIndex++;
                                }
                                if (DialogWithSuspectIndex == DialogWithSuspect.Count)
                                {
                                    int talkresult = Utils.DisplayAnswers(NegotiateAnswers, false);
                                    if (talkresult == 0)
                                    {
                                        Game.DisplaySubtitle("~b~You:~s~ I can get you an escape vehicle if you leave your guns on the ground. (1/3)");
                                        GetAway1 = true;
                                    }
                                    else if(talkresult == 1)
                                    {
                                        Game.DisplaySubtitle("~b~You:~s~ I can't offer you anything. It's gonna end either way. (1/2)");
                                        SuspectComesOutShooting = true;
                                    }
                                    else if (talkresult == 2)
                                    {
                                        Game.DisplaySubtitle("~b~You:~s~ I can't promise anything. But if you surrender I will make sure to support your case with the DA (1/2)");
                                        SuspectSurrenders1 = true;
                                    }
                                }
                            }

                        }
                        if (GetAway1 == true && GetAway2 == false)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSuspectGetawayIndex < DialogWithSuspectGetaway.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSuspectGetaway[DialogWithSuspectGetawayIndex]);
                                    DialogWithSuspectGetawayIndex++;
                                }
                                if (DialogWithSuspectGetawayIndex == DialogWithSuspectGetaway.Count)
                                {
                                    ToggleMobilePhone(Game.LocalPlayer.Character, false);
                                    GetAway2 = true;
                                }
                            }
                        }
                        if (GetAway2 == true && GetAway3 == false)
                        {
                            SpawnGetAwayStuff();
                            Game.DisplayNotification("~b~Attention to all Units:~s~ Getaway Vehicle is on the way.");
                            foreach(Ped i in SWATAimingTeam)
                            {
                                i.Tasks.AimWeaponAt(Suspect, -1);
                            }
                            Superior.Tasks.AimWeaponAt(Suspect, -1);
                            GetAway3 = true;
                        }
                        if (GetAway3 == true && GetAway4 == false)
                        {
                            if (Vector3.Distance(GetAwayCar.Position, GetAwayDesignation) < 6f)
                            {
                                Suspect.IsVisible = true;
                                Suspect.Tasks.FollowNavigationMeshToPosition(GetAwayDesignation, 1f, 2f);
                                Suspect.Inventory.EquippedWeapon.Drop();
                                if (Suspect.IsDead || Suspect.IsCuffed)
                                {
                                    this.End();
                                }
                                GetAway4 = true;
                            }
                        }
                        if (GetAway4 == true && GetAway5 == false)
                        {
                            if (Suspect.DistanceTo(GetAwayCar) < 6f)
                            {
                                Suspect.Tasks.EnterVehicle(GetAwayCar, -1).WaitForCompletion();
                                Pursuit = Functions.CreatePursuit();
                                Functions.AddPedToPursuit(Pursuit, Suspect);
                                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                                GetAway5 = true;
                            }
                        }
                        if (GetAway5 == true)
                        {
                            if (Suspect.IsCuffed || Suspect.IsDead)
                            {
                                this.End();
                            }
                        }
                        if (SuspectSurrenders1 == true && SuspectSurrenders2 == false)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.DisplaySubtitle("~r~Suspect:~s~ Ok I'm coming out. Please don't shoot! (2/2)");
                                ToggleMobilePhone(Game.LocalPlayer.Character, false);
                                GameFiber.Sleep(3000);
                                Suspect.IsVisible = true;
                                Suspect.Face(Superior);
                                Suspect.Tasks.PlayAnimation("busted", "idle_2_hands_up_2h", 1f, AnimationFlags.StayInEndFrame | AnimationFlags.SecondaryTask);
                                Suspect.Inventory.EquippedWeapon.Drop();
                                SuspectSurrenders2 = true;
                            }
                        }
                        if (SuspectSurrenders2 == true)
                        {
                            if(Suspect.IsCuffed || Suspect.IsDead)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.EndCalloutKey + " ~s~ to end the callout.");
                            }
                        }
                        if (SuspectComesOutShooting == true && SuspectComesOutShooting2 == false)
                        {
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.DisplaySubtitle("~y~ Hangs up.");
                                ToggleMobilePhone(Game.LocalPlayer.Character, false);
                                GameFiber.Sleep(4000);
                                Suspect.IsVisible = true;
                                Suspect.Tasks.FightAgainstClosestHatedTarget(50f);
                                foreach (Ped i in PoliceOfficersSpawned)
                                {
                                    i.Tasks.ClearImmediately();
                                    i.KeepTasks = true;
                                    i.Tasks.FightAgainstClosestHatedTarget(100f);
                                }
                                SuspectComesOutShooting2 = true;
                            }
                        }
                        if (SuspectComesOutShooting2 == true)
                        {
                            if(Suspect.IsDead || Suspect.IsCuffed)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.EndCalloutKey + " ~s~ to end the callout.");
                            }
                        }
                        if (NegotiationHangup == true && GoBackToSupervisor == false)
                        {
                            Game.DisplaySubtitle("~y~Suspect hangs up. (2/2)");
                            ToggleMobilePhone(Game.LocalPlayer.Character, false);
                            SuperiorBlip = Superior.AttachBlip();
                            SuperiorBlip.Color = Color.Green;
                            GoBackToSupervisor = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Superior) <= 2f && GoBackToSupervisor == true && SupervisorSendsPeopleIn == false)
                        {
                            Superior.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSuperiorSuspectHangedUpIndex < DialogWithSuperiorSuspectHangedUp.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSuperiorSuspectHangedUp[DialogWithSuperiorSuspectHangedUpIndex]);
                                    DialogWithSuperiorSuspectHangedUpIndex++;
                                }
                                if (DialogWithSuperiorSuspectHangedUpIndex == DialogWithSuperiorSuspectHangedUp.Count)
                                {
                                    Game.DisplayNotification("~b~Attention to SWAT Units:~s~ You are cleared to enter the building.");
                                    SupervisorSendsPeopleIn = true;
                                }
                            }
                        }
                        if (SupervisorSendsPeopleIn == true && SendPeopleIn == false)
                        {
                            foreach(Ped i in SWATFollowTeam)
                            {
                                i.Tasks.PlayAnimation("anim@weapons@rifle@lo@dbshot", "aim_med_loop", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask |AnimationFlags.UpperBodyOnly);
                                i.Tasks.FollowNavigationMeshToPosition(SuspectSpawn, 1f, 1f);
                                //i.Tasks.GoToWhileAiming(Suspect, 1f, 5f);
                                SendPeopleIn = true;
                            }
                        }
                        if(SendPeopleIn == true && SupervisorInDone == false)
                        {
                            foreach(Ped i in SWATFollowTeam)
                            {
                                if (i.DistanceTo(Suspect) <= 2f)
                                {
                                    i.IsVisible = false;
                                } 
                                if(SWATFollowTeam.All(officer => officer.IsVisible == false))
                                {
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ SWAT Team Alpha is inside. We're clearing room by room.");
                                    GameFiber.Sleep(5000);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ We are experiencing heavy resistance! One of us has been hit!");
                                    GameFiber.Sleep(3000);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ Suspect is down.");
                                    GameFiber.Sleep(2000);
                                    int index = new Random().Next(SWATFollowTeam.Count);
                                    SWATFollowTeam.ElementAt(index).Kill();
                                    i.Tasks.ClearImmediately();
                                    i.Tasks.ClearSecondary();
                                    Suspect.Kill();
                                    GameFiber.Wait(1000);
                                    Suspect.IsVisible = true;
                                    SupervisorInDone = true;
                                }
                            }

                        }
                        if(SupervisorInDone == true)
                        {
                            foreach(Ped i in SWATFollowTeam)
                            {
                                i.IsVisible = true;
                            }
                            Game.DisplayHelp("Clean up the scene. When you're done press~y~ " + Settings.EndCalloutKey);
                        }
                        if(SendSwatIn == true && SupervisorInDone == false)
                        {
                            foreach (Ped i in SWATFollowTeam)
                            {
                                if (i.DistanceTo(Suspect) <= 2f)
                                {
                                    i.IsVisible = false;
                                }
                                if (SWATFollowTeam.All(officer => officer.IsVisible == false))
                                {
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ SWAT Team Alpha is inside. We're clearing room by room.");
                                    GameFiber.Sleep(5000);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ We've found and arrested the Suspect. We are coming out.");
                                    Suspect.Face(Superior);
                                    Suspect.Tasks.PlayAnimation("busted", "idle_2_hands_up_2h", 1f, AnimationFlags.StayInEndFrame | AnimationFlags.SecondaryTask);
                                    GameFiber.Sleep(3000);
                                    i.Tasks.ClearImmediately();
                                    i.Tasks.ClearSecondary();
                                    Suspect.IsVisible = true;
                                    SupervisorInDone = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Game.LogTrivial("Crash averted" + e);
                }
            });
        }
        public override void End()
        {
            base.End();
            CalloutRunning = false;
            ToggleMobilePhone(Game.LocalPlayer.Character, false);
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (Superior.Exists()) { Superior.Dismiss(); }
            if (SuperiorBlip.Exists()) { SuperiorBlip.Delete(); }
            if (Suspect.Exists()) { Suspect.Dismiss(); }
            if (MobilePhone.Exists()) { MobilePhone.Delete(); }
            if (GetAwayCar.Exists()) { GetAwayCar.Dismiss(); }
            if (Parent.Exists()) { Parent.Dismiss(); }
            foreach (Ped i in FiremenList)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Ped i in ParamedicsList)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Ped i in PoliceOfficersSpawned)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Ped i in SWATAimingTeam)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Ped i in SWATFollowTeam)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Vehicle i in AllSpawnedPoliceVehicles)
            {
                if (i.Exists()) { i.Dismiss(); }
            }
            foreach (Vehicle i in FireTrucksList)
            {
                if (i.Exists()) { i.Dismiss(); }
            }
            foreach (Vehicle i in AmbulancesList)
            {
                if (i.Exists()) { i.Dismiss(); }
            }
            foreach (Rage.Object i in Barriers)
            {
                if (i.Exists()) { i.Delete(); }
            }
            foreach (Rage.Object i in InvisWalls)
            {
                if (i.Exists()) { i.Delete(); }
            }
            foreach (Ped i in BarrierPeds)
            {
                if (i.Exists())
                {
                    i.Delete();
                }
            }
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

        private void SpawnEMSAndFire()
        {
            for (int i = 0; i < AmbulanceLocations.Count; i++)
            {
                Vehicle ambulance = new Vehicle(new Model("AMBULANCE"), AmbulanceLocations[i], AmbulanceHeadings[i]);
                ambulance.IsPersistent = true;
                ambulance.IsSirenOn = true;
                ambulance.IsSirenSilent = true;
                ambulance.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AmbulancesList.Add(ambulance);
                AllBankHeistEntities.Add(ambulance);
            }
            for (int i = 0; i < ParamedicLocations.Count; i++)
            {
                Ped para = new Ped(new Model("S_M_M_PARAMEDIC_01"), ParamedicLocations[i], ParamedicHeadings[i]);
                para.IsPersistent = true;
                para.BlockPermanentEvents = true;
                ParamedicsList.Add(para);
                AllBankHeistEntities.Add(para);

            }
            for (int i = 0; i < FireTruckLocations.Count; i++)
            {
                Vehicle firetruck = new Vehicle(new Model("FIRETRUK"), FireTruckLocations[i], FireTruckHeadings[i]);
                firetruck.IsPersistent = true;
                firetruck.IsSirenOn = true;
                firetruck.IsSirenSilent = true;
                firetruck.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AmbulancesList.Add(firetruck);
                AllBankHeistEntities.Add(firetruck);
                Ped fireman = new Ped(new Model("S_M_Y_FIREMAN_01"), SpawnPoint, 0f);
                fireman.WarpIntoVehicle(firetruck, -1);
                fireman.BlockPermanentEvents = true;
                fireman.IsPersistent = true;
                FiremenList.Add(fireman);
                AllBankHeistEntities.Add(fireman);
            }
        }
        private void SpawnAllPoliceCars()
        {
            for (int i = 0; i < POLICECarLocations.Count; i++)
            {
                Vehicle car = new Vehicle(CarModels[new Random().Next(CarModels.Length)], POLICECarLocations[i], POLICECarHeadings[i]);
                car.IsPersistent = true;
                car.IsSirenOn = true;
                car.IsSirenSilent = true;
                car.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AllSpawnedPoliceVehicles.Add(car);
                AllBankHeistEntities.Add(car);
            }
            for (int i = 0; i < FBICarLocations.Count; i++)
            {
                Vehicle fbicar = new Vehicle(new Model("FBI2"), FBICarLocations[i], FBICarHeadings[i]);
                fbicar.IsPersistent = true;
                fbicar.IsSirenOn = true;
                fbicar.IsSirenSilent = true;
                fbicar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                fbicar.Wash();
                AllSpawnedPoliceVehicles.Add(fbicar);
                AllBankHeistEntities.Add(fbicar);
            }
            for (int i = 0; i < RiotLocations.Count; i++)
            {
                Vehicle policeriot = new Vehicle(new Model("RIOT"), RiotLocations[i], RiotHeadings[i]);
                policeriot.IsPersistent = true;
                policeriot.IsSirenOn = true;
                policeriot.IsSirenSilent = true;
                policeriot.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                policeriot.Wash();
                AllSpawnedPoliceVehicles.Add(policeriot);
                AllBankHeistEntities.Add(policeriot);
            }
            for (int i = 0; i < TransporterLocations.Count; i++)
            {
                Vehicle ptransport = new Vehicle(new Model("policet"), TransporterLocations[i], TransporterHeading[i]);
                ptransport.IsPersistent = true;
                ptransport.IsSirenOn = true;
                ptransport.IsSirenSilent = true;
                ptransport.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AllSpawnedPoliceVehicles.Add(ptransport);
                AllBankHeistEntities.Add(ptransport);
            }
        }
        private void SpawnAllPoliceOfficers()
        {
            for (int i = 0; i < PoliceOfficersLocations.Count; i++)
            {
                Ped officer = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), PoliceOfficersLocations[i], PoliceOfficersHeadings[i]);
                Functions.SetPedAsCop(officer);
                Functions.SetCopAsBusy(officer, true);
                officer.CanBeTargetted = false;
                officer.IsPersistent = true;
                officer.BlockPermanentEvents = true;
                officer.Inventory.GiveNewWeapon("WEAPON_PISTOL50", 10000, false);
                officer.RelationshipGroup = "COP";
                PoliceOfficersSpawned.Add(officer);
                AllBankHeistEntities.Add(officer);
                officer.CanAttackFriendlies = false;

            }

        }
        private void SpawnBothSwatTeams()
        {
            for (int i = 0; i < SWATAimingTeamLocations.Count; i++)
            {

                Ped unit = new Ped("s_m_y_swat_01", SWATAimingTeamLocations[i], SWATAimingTeamHeadings[i]);
                Functions.SetPedAsCop(unit);
                Functions.SetCopAsBusy(unit, true);
                unit.CanBeTargetted = false;
                unit.BlockPermanentEvents = true;
                unit.IsPersistent = true;
                unit.Inventory.GiveNewWeapon(new WeaponAsset(SWATWeapons[new Random().Next(SWATWeapons.Length)]), 10000, true);
                unit.RelationshipGroup = "COP";
                unit.Tasks.AimWeaponAt(SuspectSpawn, -1);
                Rage.Native.NativeFunction.Natives.SET_PED_PROP_INDEX(unit, 0, 0, 0, 2);
                Rage.Native.NativeFunction.Natives.SetPedCombatAbility(unit, 2);
                unit.CanAttackFriendlies = false;

                unit.Health = 209;
                unit.Armor = 92;

                PoliceOfficersSpawned.Add(unit);
                SWATUnitsSpawned.Add(unit);
                SWATAimingTeam.Add(unit);
                AllBankHeistEntities.Add(unit);
            }
            for (int i = 0; i < SWATFollowTeamLocations.Count; i++)
            {

                Ped unit = new Ped("s_m_y_swat_01", SWATFollowTeamLocations[i], SWATFollowTeamHeadings[i]);
                Functions.SetPedAsCop(unit);
                Functions.SetCopAsBusy(unit, true);
                unit.CanBeTargetted = false;
                unit.BlockPermanentEvents = true;
                unit.IsPersistent = true;
                unit.Inventory.GiveNewWeapon(new WeaponAsset(SWATWeapons[new Random().Next(SWATWeapons.Length)]), 10000, true);
                unit.RelationshipGroup = "COP";
                Rage.Native.NativeFunction.Natives.SET_PED_PROP_INDEX(unit, 0, 0, 0, 2);
                Rage.Native.NativeFunction.Natives.SetPedCombatAbility(unit, 2);
                unit.CanAttackFriendlies = false;

                unit.Health = 209;
                unit.Armor = 92;

                PoliceOfficersSpawned.Add(unit);
                SWATUnitsSpawned.Add(unit);
                SWATFollowTeam.Add(unit);
                AllBankHeistEntities.Add(unit);
            }
        }
        private void SpawnAllBarriers()
        {
            for (int i = 0; i < BarrierLocations.Count; i++)
            {
                Rage.Object Barrier = PlaceBarrier(BarrierLocations[i], BarrierHeadings[i]);
                Barriers.Add(Barrier);
                AllBankHeistEntities.Add(Barrier);


            }

        }
        private void SpawnSuperior()
        {
                Superior = new Ped("s_m_m_fibsec_01", SuperiorSpawn, 0f);
                Functions.SetPedAsCop(Superior);
                Functions.SetCopAsBusy(Superior, true);
                Superior.CanBeTargetted = false;
                Superior.IsPersistent = true;
                Superior.BlockPermanentEvents = true;
                Superior.Inventory.GiveNewWeapon("WEAPON_SMG", 10000, true);
                Superior.RelationshipGroup = "COP";
                PoliceOfficersSpawned.Add(Superior);
                AllBankHeistEntities.Add(Superior);
                Superior.CanAttackFriendlies = false;

        }
        private void SpawnSuspect()
        {
            Suspect = new Ped(SuspectSpawn);
            Suspect.IsVisible = false;
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            Suspect.Inventory.GiveNewWeapon("weapon_heavypistol", 10000, true);
            Suspect.RelationshipGroup = "SUSPECT";
            Persona.FromExistingPed(Suspect).Wanted = true;
            SuspectSpawned.Add(Suspect);
            AllBankHeistEntities.Add(Suspect);

        }
        private Rage.Object PlaceBarrier(Vector3 Location, float Heading)
        {
            Rage.Object Barrier = new Rage.Object("prop_barrier_work05", Location);
            Barrier.Heading = Heading;
            Barrier.IsPositionFrozen = true;
            Barrier.IsPersistent = true;
            Rage.Object invWall = new Rage.Object("p_ice_box_01_s", Barrier.Position);
            Ped invPed = new Ped(invWall.Position);
            invPed.IsVisible = false;
            invPed.IsPositionFrozen = true;
            invPed.BlockPermanentEvents = true;
            invPed.IsPersistent = true;
            invWall.Heading = Heading;
            invWall.IsVisible = false;
            invWall.IsPersistent = true;

            InvisWalls.Add(invWall);
            BarrierPeds.Add(invPed);
            return Barrier;

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

                                        if (!AllBankHeistEntities.Contains(entity))
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

                                        if (!AllBankHeistEntities.Contains(entity))
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
                        if (AllBankHeistEntities.Contains(entity))
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

                                                if (!AllBankHeistEntities.Contains(entity))
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
        private void SpawnGetAwayStuff()
        {
            GetAwayCar = new Vehicle("rhapsody", GetAwaySpawn);
            GetAwayCar.IsPersistent = true;
            AllBankHeistEntities.Add(GetAwayCar);
            Ped GetAwayDriver = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), GetAwaySpawn, 0f);
            Functions.SetPedAsCop(GetAwayDriver);
            Functions.SetCopAsBusy(GetAwayDriver, true);
            GetAwayDriver.WarpIntoVehicle(GetAwayCar, -1);
            GetAwayDriver.BlockPermanentEvents = true;
            GetAwayDriver.IsPersistent = true;
            AllBankHeistEntities.Add(GetAwayDriver);
            PoliceOfficersSpawned.Add(GetAwayDriver);
            GetAwayDriver.Tasks.DriveToPosition(GetAwayDesignation, 5f, VehicleDrivingFlags.DriveAroundVehicles | VehicleDrivingFlags.DriveAroundObjects | VehicleDrivingFlags.DriveAroundPeds);
            while (true)
            {
                GameFiber.Yield();
                if (Vector3.Distance(GetAwayCar.Position, GetAwayDesignation) < 6f)
                {
                    break;
                }
            }
            GetAwayDriver.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
            GetAwayDriver.Tasks.FollowNavigationMeshToPosition(Superior.GetOffsetPosition(Vector3.RelativeRight * 1.5f), Game.LocalPlayer.Character.Heading, 1.9f);
        }

        private void SpawnParent()
        {
            GetAwayCar = new Vehicle(CarModels[new Random().Next(CarModels.Length)], GetAwaySpawn);
            GetAwayCar.IsPersistent = true;
            GetAwayCar.IsSirenOn = true;
            GetAwayCar.IsSirenSilent = true;
            AllBankHeistEntities.Add(GetAwayCar);
            Ped GetAwayDriver = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), GetAwaySpawn, 0f);
            Parent = new Ped(GetAwaySpawn, 0f);
            Parent.IsPersistent = true;
            Parent.BlockPermanentEvents = true;
            Functions.SetPedAsCop(GetAwayDriver);
            Functions.SetCopAsBusy(GetAwayDriver, true);
            Parent.WarpIntoVehicle(GetAwayCar, 0);
            GetAwayDriver.WarpIntoVehicle(GetAwayCar, -1);
            GetAwayDriver.BlockPermanentEvents = true;
            GetAwayDriver.IsPersistent = true;
            AllBankHeistEntities.Add(GetAwayDriver);
            AllBankHeistEntities.Add(Parent);
            PoliceOfficersSpawned.Add(GetAwayDriver);
            GetAwayDriver.Tasks.DriveToPosition(GetAwayDesignation, 5f, VehicleDrivingFlags.DriveAroundVehicles | VehicleDrivingFlags.DriveAroundObjects | VehicleDrivingFlags.DriveAroundPeds);
            while (true)
            {
                GameFiber.Yield();
                if (Vector3.Distance(GetAwayCar.Position, GetAwayDesignation) < 6f)
                {
                    break;
                }
            }
            Parent.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
            Parent.Tasks.FollowNavigationMeshToPosition(Superior.GetOffsetPosition(Vector3.RelativeRight * 1.7f), Game.LocalPlayer.Character.Heading, 1.9f);
        }
        private void LoadModels()
        {
            foreach (string s in LSPDModels)
            {
                GameFiber.Yield();
                new Model(s).Load();
            }
        }
        private void CreateSpeedZone()
        {
            GameFiber.StartNew(delegate
            {
                while (CalloutRunning)
                {
                    GameFiber.Yield();

                    foreach (Vehicle veh in World.GetEntities(SpawnPoint, 75f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludeFiretrucks | GetEntitiesFlags.ExcludeAmbulances))
                    {
                        GameFiber.Yield();
                        if (AllBankHeistEntities.Contains(veh))
                        {
                            continue;
                        }
                        if (veh != null)
                        {
                            if (veh.Exists())
                            {
                                if (veh != Game.LocalPlayer.Character.CurrentVehicle)
                                {
                                    if (!veh.CreatedByTheCallingPlugin)
                                    {
                                        if (!AllBankHeistEntities.Contains(veh))
                                        {
                                            if (veh.Velocity.Length() > 0f)
                                            {
                                                Vector3 velocity = veh.Velocity;
                                                velocity.Normalize();
                                                velocity *= 0f;
                                                veh.Velocity = velocity;
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
        private Rage.Object MobilePhone;
        private void ToggleMobilePhone(Ped ped, bool toggle)
        {

            if (toggle)
            {
                NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, false);
                ped.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), -1, true);
                MobilePhone = new Rage.Object(new Model("prop_police_phone"), new Vector3(0, 0, 0));
                //int boneIndex = NativeFunction.Natives.GET_PED_BONE_INDEX<int>(ped, (int)PedBoneId.RightPhHand);
                int boneIndex =  ped.GetBoneIndex(PedBoneId.RightPhHand);
                MobilePhone.AttachTo(ped, boneIndex, new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f));
                //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(MobilePhone, ped, boneIndex, 0f, 0f, 0f, 0f, 0f, 0f, true, true, false, false, 2, 1);
                ped.Tasks.PlayAnimation("cellphone@", "cellphone_call_listen_base", 1.3f, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);

            }
            else
            {
                NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, true);
                ped.Tasks.Clear();
                if (GameFiber.CanSleepNow)
                {
                    GameFiber.Wait(800);
                }
                if (MobilePhone.Exists()) { MobilePhone.Delete(); }
            }
        }
    }

}
