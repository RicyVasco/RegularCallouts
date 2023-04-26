using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using RegularCallouts.Stuff;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RAGENativeUI.Elements;
using System.IO;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Abandonded Vehicle - Crime suspected", CalloutProbability.Medium)]
    public class AbandondedVehicle : Callout
    {
        private class Clue
        {
            public string Name { get; set; }
            public string LocationName { get; set; }
            public string ExaminMessage { get; set; }
            public bool Finished { get; set; }
            public List<string> Answer { get; set; }
            public int AnswerIndex { get; set; }
            public string HonestyWitness { get; set; }
            public List<string> WitnessHonestyAnswers { get; set; }
            public List<Action> ExeStuff { get; set; }
            public int HonestyAnswerIndex { get; set; }
            public bool HasLocation { get; set; }
            public bool LocationSet { get; set; }
            public Vector3 Location { get; set; }
            public string WitnessLieStuff;
            public bool AddClue;
            public bool WrongAnswer;
            public List<string> FemaleAnswerVoiceLines { get; set; }
        }

        private Clue BloodSplashes = new Clue
        {
            Name = "BloodSplashes",
            ExaminMessage = "Large splashes of blood, found inside abandoned vehicle",
            Finished = false
        };

        private Clue VehicleOwner = new Clue
        {
            Name = "Vehicle Owner: ",
            ExaminMessage = "The owner of the vehicle",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ Does the name ... say anything to you? (1/2)",
            "~y~Caller:~s~ No Sir. That's not a name I'm familiar with. (2/2)"
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ You ever seen the car before? (1/3)",
            "~y~Caller:~s~ Funny enough I did. A couple of nights ago it was over there in the parking lot.(2/3)",
            "~y~Caller:~s~ I know most of the cars that park here regulary, so it kinda stood out.(3/3)"
            },
            HonestyAnswerIndex = 0,
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Vehicle Owner's house",
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_vehicleowner_01",
            ""
            }
        };
        private Clue AccidentLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Accident Location"
        };
        private Clue BarLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Bar Location"
        };
        private Clue Receipt = new Clue
        {
            Name = "Receipt",
            ExaminMessage = "Live hog purchase receipt, signed by F. Morgan, found in abandoned vehicle",
            Finished = false
        };
        private Clue ReceiptWife = new Clue
        {
            Name = "Receipt ",
            ExaminMessage = "Live hog purchase receipt, signed by F. Morgan, found in abandoned vehicle",
            Answer = new List<string>
            {
                "~b~You:~s~ We've found a receipt in the trunk of your husband's car for a live pig.",
                "~y~Wife:~s~ A pig? My husband runs a tool business. That would be ... . God knows what he is up to."
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ What makes you say that about him?",
                "~y~Wife:~s~ He's the foreman at my husband's plant. A very shady character.",
                "~y~Wife:~s~ I've told my husband to keep his distance from the staff. They've always out drinking together."
            },
            HonestyAnswerIndex = 0,
            AddClue = false,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_receiptwife_01",
            ""
            }
        };
        private Clue OwnerFriendsHouseLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Friends House Location"
        };
        private Clue LinkToAccidentOwnerFriend = new Clue
        {
            Name = "Link to Accident",
            ExaminMessage = "",
            Answer = new List<string>
            {
                "~b~You:~s~ We found his car abandoned. Covered in blood. You know anything about that?",
                "~y~Wife:~s~ Hell no... I'm sorry to hear that. He's a good boss."
            },
            AnswerIndex = 0,
            HonestyWitness = "Lie",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = "Receipt",
            AddClue = false,
            WrongAnswer = false,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_linktoaccidentownerfriend_01",
            ""
            }
        };
        private Clue VehicleOwnerCurrentLocation = new Clue
        {
            Name = "... Current Location",
            ExaminMessage = "",
            Answer = new List<string>
            {
                "~b~You:~s~ Where exactly is ... holed up?",
                "~y~Wife:~s~ No idea... I think he wen't to Vice City."
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ I'm tired of your shit. Tell me or I will arrest you for obstruction.",
                "~y~Wife:~s~ He's at my place. Here I will write you the address."
            },
            HonestyAnswerIndex = 0,
            AddClue = false,
            WrongAnswer = false,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_vehicleownercurrentlocation_01",
            ""
            }
        };
        private Clue Wallet = new Clue
        {
            Name = "Wallet",
            ExaminMessage = "Wallet belonging to Adrian Black",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ The wallet next to the car, was there anything inside when you arrived? (1/3)",
            "~y~Caller:~s~ You accusing me of something? (2/2)"
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ Do you want us to frisk you? (1/3)",
            "~y~Caller:~s~ Maybe I checked. Not that i was gonna steal anything. Maybe I took a look.. (2/3)",
            "~y~Caller:~s~ but there was no money in there. Not even change. (3/3)"
            },
            HonestyAnswerIndex = 0,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_wallet_01",
            ""
            }
        };
        private Clue Glasses = new Clue
        {
            Name = "Glasses",
            ExaminMessage = "Stenzel eyeglasses, broken and repaired",
            Finished = false
        };
        private Clue Pipe = new Clue
        {
            Name = "Pipe",
            ExaminMessage = "Length of steel pipe, InstaHeat brand, stained with blood",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ We found a steel pipe with blood on it near the car. Know anything about it? (1/2)",
            "~y~Caller:~s~ No Sir. I called the police when I saw the blood in the car. (2/2)"
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ When you saw the blood, was it wet? bright red? Or darker like it is now? (1/2)",
            "~y~Caller:~s~ Darker. Looked dry already (2/2)"
            },
            HonestyAnswerIndex = 0,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_pipe_01",
            ""
            }
        };
        private Clue AlibiWife = new Clue
        {
            Name = "Alibi for the Wife",
            ExaminMessage = "",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ I think you should come clean with us.",
            "~b~You:~s~ Your husband is missing and after our search I'm willing to call the circumstances suspicious.",
            "~b~You:~s~ Can you account for your movements last night?",
            "~y~Wife:~s~ You're not accusing me, are you? What an awful thing to say!",
            "~y~Wife:~s~ I was here all night, of course, waiting for him to come home!",
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ Is there anyone who can vouch for that?",
            "~y~Wife:~s~ Well... no. I was here alone.",
            "~y~Wife:~s~ I cooked his dinner and waited, but he never came home."
            },
            HonestyAnswerIndex = 0,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_alibiwife_01",
            "f_alibiwife_02",
            "f_alibiwife_03",
            "",
            ""
            }
        };
        private Clue PictureOfAnotherGirl = new Clue
        {
            Name = "Picture of another Girl",
            ExaminMessage = "A picture of another girl together with the missing driver.",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ Can you tell me about the picture in the bedroom?",
            "~y~Wife:~s~ What is there to tell.... it's just a normal picture..."
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ Are you sure that's what you want to tell me? Was your husband gonna leave you for her?",
            "~y~Wife:~s~ I don't know if he wanted to leave me... or if it was just an affair..",
            "~y~Wife:~s~ I only want to know that he's safe."
            },
            HonestyAnswerIndex = 0,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_pictureofanothergirl_01",
            ""
            }
        };
        private Clue WitnessPurposeAtScene = new Clue
        {
            Name = "Caller's purpose at the Crimescene",
            ExaminMessage = "WitnessPurposeAtScene",
            Finished = false,
            Answer = new List<string>
            {
            "~b~You:~s~ Mind if I ask what you're doing out here? (1/2)",
            "~y~Caller:~s~ Well I work here. I was just making my way to my office when I saw the car. (2/2)"
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
            "~b~You:~s~ Did you see anybody else here? Maybe someone hanging around the car? (1/2)",
            "~y~Caller:~s~ No. I haven't seen anyone here until you guys showed up. (2/2)"
            },
            HonestyAnswerIndex = 0,
            FemaleAnswerVoiceLines = new List<string>
            {
            "f_witnesspurposeatscene_01",
            ""
            }
        };
        private List<Clue> FoundClues = new List<Clue> { };
        private List<Clue> LocationClues = new List<Clue> { };
        private List<string> GoToLocations = new List<string> { };
        //private List<Clue> AllPossibleClues = new List<Clue> { BloodSplashes, Receipt, Wallet, Glasses, Pipe, };

        private Vector3 SpawnPoint;
        private Blip SpawnBlip;
        private Vector3 SpawnPoint1;
        private List<Entity> AllBankHeistEntities = new List<Entity>();
        private int Blood;
        private int Blood2;
        private bool WifeInteriorLeave;
        private List<Vector3> CopCarLocation;
        private List<float> CopCarHeading;
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private System.Media.SoundPlayer VoiceTalk = new System.Media.SoundPlayer("plugins/LSPDFR/RegularCallouts/AbandondedVehicle/DORBELL.wav");
        private Vehicle SuspectVehicle;
        private Vehicle FriendVehicle;
        private Vector3 SuspectVehicleLocation;
        private Vector3 FriendVehicleLocation;
        private bool VehOwnerOpensDoor;
        private float SuspectVehicleHeading;
        private float FriendVehicleHeading;
        public string VehOwnerString;
        private static string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private static string[] CarModels = new string[] { "POLICE", "POLICE2", "POLICE3", "POLICE4" };
        private List<Rage.Object> AllSpawnedObjects = new List<Rage.Object>();
        private List<Vehicle> AllSpawnedPoliceVehicles = new List<Vehicle>();
        private List<Ped> AllSpawnedPeds = new List<Ped>();
        private List<Vector3> PolicePedLocations;
        private List<float> PolicePedHeadings;
        private List<Vector3> BarrierLocations;
        private List<float> BarrierHeadings;
        private Ped VehOwner;
        private bool ChaseActivate;
        private bool ExitCar;
        private bool NextAnswerStuff2;
        public Vector3 StorageSpawn;
        private bool NextAnswerStuff3;
        private Ped ScientistA;
        private LHandle Chase;
        public string CurrentLocation;
        private Blip HouseBlip;
        private Ped IntroWitness;
        private Vector3 FriendHouseDoorLocation;
        private Vector3 FriendHouseFleeLocation;
        private bool OpenMap;
        private Vector3 BarkeeperLocation;
        private int BarKeeperDialog = 1;
        private bool BloodSpawn;
        private bool InInterior;
        private Vector3 IntroWitnessLocation;
        private bool VehOwnerHome;
        private Ped VehicleOwnerWife;
        private bool LocationMenu2;
        private Vector3 InteriorSpawnPoint;
        private bool PlayerInCar;
        private bool HouseCleared;
        private bool Bar;
        private bool NextAnswerStuff;
        private float IntroWitnessHeading;
        private Ped OwnerFriend;
        private bool OwnerOpens;
        private Vector3 ScientistALocation;
        private Ped ScientistB;
        private Vector3 ScientistBLocation;
        private List<Vector3> MarkerLocations;
        private int OwnerFriendBeginDialog = 1;
        private bool LocationMenu;
        private List<float> MarkerHeading;
        private bool FollowFriend;
        private Rage.Object EviGlasses;
        private Vector3 DriveToLocation;
        private Vector3 GlassesLocation;
        private Rage.Object EviWallet;
        private Vector3 OwnerFriendBarLocation;
        private Vector3 WalletLocation;
        private int VehOwnerAnswerDoor = 1;
        private int NextAnswerInt;
        private Rage.Object EviPipe;
        private Vector3 PipeLocation;
        private bool CalloutRunning;
        private Vector3 PictureLocation;
        private int WifeDialogEnter = 1;

        private TimerBarPool AwarenessBarPool = new TimerBarPool();
        private BarTimerBar AwarenessBar = new BarTimerBar("Suspicion");
        private float Awareness = 0f;

        private List<string> DialogWithWifeIntro = new List<string>
        {
            "~b~You:~s~ Los Santos Police, may we come in? We have some bad news and we rather discuss this in private. (1/3)",
            "~y~Resident:~s~ Sure. Uhm we can discuss this in the living room. Please come in (2/3)",
            ""
        };
        private int DialogWithWifeIntroIndex;
        private List<string> DialogWithIntroWitness = new List<string>
        {
            "~b~You:~s~ Hello Sir. Can you tell me what happened? (1/3)",
            "~y~Caller:~s~ I found the car like that. I looked into it since the doors where open and saw all the blood, so I called the police. (2/3)",
            ""
        };
        private int DialogWithIntroWitnessIndex;
        private List<string> WitnessQuestions = new List<string> {};
        private List<string> WifeQestions = new List<string> { };
        private List<string> FriendQuestions = new List<string> { };
        private List<string> ChooseTruth = new List<string> {"Truth", "Doubt", "Lie" };
        private List<string> CollectedEvidences = new List<string> { };

        public override bool OnBeforeCalloutDisplayed()
        {
            StorageSpawn = new Vector3(-144.0361f, -592.9335f, 48.24776f);
            SpawnPoint1 = new Vector3(2681.19409f, 1624.40991f, 23.4820423f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                CopCarLocation = new List<Vector3>() { new Vector3(2676.67871f, 1599.74438f, 23.774641f), new Vector3(2682.07422f, 1604.71753f, 23.7915764f), new Vector3(2675.8396f, 1625.53467f, 23.7581291f) };
                CopCarHeading = new List<float>() { 86.8991852f, 142.345108f, 89.75737f };
                PolicePedLocations = new List<Vector3>() { new Vector3(2676.66846f, 1630.48438f, 24.48806f), new Vector3(2675.34326f, 1607.90625f, 24.5020733f), new Vector3(2684.30518f, 1625.69507f, 24.6036644f) };
                PolicePedHeadings = new List<float>() { 38.6772919f, 177.293457f, -66.038681f };
                SuspectVehicleLocation = new Vector3(2679.17432f, 1617.83435f, 24.0138607f);
                SuspectVehicleHeading = 157.18428f;
                MarkerLocations = new List<Vector3>() {new Vector3(2674.18481f, 1615.62231f, 23.5010509f), new Vector3(2676.33569f, 1612.77588f, 23.5010509f), new Vector3(2677.09253f, 1609.99683f, 23.4995823f) };
                MarkerHeading = new List<float>() { 117.688431f, 176.584442f, 153.717392f};
                BarrierLocations = new List<Vector3>() {new Vector3(2685.63989f, 1625.85034f, 23.5961361f), new Vector3(2686.00757f, 1619.46875f, 23.5894413f), new Vector3(2683.52588f, 1610.69727f, 23.5384483f), new Vector3(2679.05542f, 1607.38232f, 23.500452f), new Vector3(2674.81079f, 1607.02368f, 23.5015221f) };
                BarrierHeadings = new List<float>() { -69.0726852f, -82.7574463f, -111.59938f, -146.937668f, -178.291748f };
                ScientistALocation = new Vector3(2673.77295f, 1620.62561f, 24.4967537f);
                ScientistBLocation = new Vector3(2681.81396f, 1618.26599f, 24.488493f);
                IntroWitnessLocation = new Vector3(2675.94482f, 1631.43176f, 24.487257f);
                IntroWitnessHeading = -149.73967f;
                WalletLocation = new Vector3(2674.37769f, 1615.64844f, 23.5081043f);
                PipeLocation = new Vector3(2676.42554f, 1612.92346f, 23.5106773f);
                GlassesLocation = new Vector3(2677.17432f, 1610.15967f, 1610.15967f);
                AccidentLocation.Location = SpawnPoint;
                VehicleOwner.Location = new Vector3(1639.407f, 3731.306f, 35.06713f);
                BarLocation.Location = new Vector3(1991.44f, 3055.963f, 47.21142f);
                LocationClues.Add(AccidentLocation);
                GoToLocations.Add(AccidentLocation.LocationName);
                CurrentLocation = AccidentLocation.LocationName;
                InteriorSpawnPoint = new Vector3(264.9987f, -1000.504f, -99.00864f);
                FriendVehicleLocation = new Vector3(1989.45483f, 3059.89063f, 46.5767174f);
                FriendVehicleHeading = 43.3101807f;
                BarkeeperLocation = new Vector3(1985.95337f, 3053.95776f, 47.2105865f);
                OwnerFriendBarLocation = new Vector3(1993.18127f, 3044.48584f, 47.211071f);
                FriendHouseDoorLocation = new Vector3(2319.445f, 2553.302f, 47.69053f);
                FriendHouseFleeLocation = new Vector3(2314.29f, 2549.445f, 47.43023f);
                DriveToLocation = new Vector3(2308.715f, 2525.771f, 46.18185f);
                PictureLocation = new Vector3(262.6416f, -1002.588f, -99.00864f);
            }

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            CalloutMessage = "Abandonded Vehicle";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_02", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            SpawnBlip = new Blip(SpawnPoint, 40f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            AwarenessBarPool.Add(AwarenessBar);
            CalloutRunning = true;
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ The caller found an abandonded vehicle with signs of a violent crime. Respond Code 2");
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            Utils.AddLog("loading CalloutLos");
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            Utils.AddLog("Base Process");
            base.Process();
            Utils.AddLog("Start New GameFiber");
            GameFiber.StartNew(delegate
            {
                try
                {
                    Utils.AddLog("Entering WhileLoop");
                    while (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) > 350f)
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
                    GameFiber.Yield();
                    ClearUnrelatedEntities();
                    GameFiber.Yield();
                    Game.LogTrivial("Unrelated entities cleared");
                    //Blood = Utils.AddDecalOnGround(DeadBody.FrontPosition, Utils.DecalTypes.porousPool_blood, 2f, 2f, 0.196f, 0f, 0f, 1f, -1f);
                    SpawnOwnerandFriend();
                    Utils.AddLog("Spawned Owner and Friend");
                    GameFiber.Yield();
                    SpawnAllCopCars();
                    GameFiber.Yield();
                    SpawnAllPolicePeds();
                    GameFiber.Yield();
                    SpawnSuspectVehicle();
                    GameFiber.Yield();
                    SpawnAllMarkers();
                    GameFiber.Yield();
                    SpawnScientists();
                    GameFiber.Yield();
                    SpawnAllEvidences();
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    CollectedEvidences.Add("None");
                    Utils.AddLog("Spawned everything, entering next loop");
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        #region CalloutStart/AccidentLocation
                        if (CurrentLocation == AccidentLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 100f)
                        {
                            if (!SuspectVehicle.Exists())
                            {
                                ClearUnrelatedEntities();
                                SpawnSuspectVehicle();
                            }
                            if (!EviWallet.Exists())
                            {
                                ClearUnrelatedEntities();
                                EviWallet = new Rage.Object("prop_ld_wallet_02", WalletLocation);
                                EviWallet.IsPersistent = true;
                                EviWallet.MakeMission();
                                AllSpawnedObjects.Add(EviWallet);
                                AllBankHeistEntities.Add(EviWallet);
                            }
                            if (!EviGlasses.Exists())
                            {
                                EviGlasses = new Rage.Object("prop_cs_sol_glasses", Utils.ToGround(GlassesLocation));
                                EviGlasses.IsPersistent = true;
                                EviGlasses.MakeMission();
                                AllSpawnedObjects.Add(EviGlasses);
                                AllBankHeistEntities.Add(EviGlasses);
                            }
                            if (!EviPipe.Exists())
                            {
                                EviPipe = new Rage.Object("ba_prop_battle_sniffing_pipe", PipeLocation);
                                EviPipe.IsPersistent = true;
                                EviPipe.MakeMission();
                                AllSpawnedObjects.Add(EviPipe);
                                AllBankHeistEntities.Add(EviPipe);
                            }
                        }
                        if (CurrentLocation == AccidentLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 50f)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle) <= 30f && BloodSpawn == false)
                            {
                                Blood = Utils.AddDecalOnGround(SuspectVehicle.Position, Utils.DecalTypes.solidPool_blood, 8f, 8f, 0.196f, 0f, 0f, 1f, -1f);
                                GameFiber.Yield();
                                Blood2 = Utils.AddDecalOnGround(EviPipe.Position, Utils.DecalTypes.porousPool_blood, 1f, 1f, 0.196f, 0f, 0f, 1f, -1f);
                                BloodSpawn = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle) <= 15f && SpawnBlip.Exists())
                            {
                                Game.DisplaySubtitle("~g~Officer:~s~ Hey Officer! This worker found the car an hour ago. I will stay with him so you can take a look around. When you're ready you can question him.");
                                SpawnBlip.Delete();
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(EviWallet) <= 1.5f && Wallet.Finished == false)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the wallet.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                    Wallet.ExaminMessage = "Wallet belonging to " + Functions.GetPersonaForPed(VehOwner).FullName.ToString();
                                    Game.DisplayNotification("~b~Clue added:~s~ " + Wallet.ExaminMessage);
                                    FoundClues.Add(Wallet);
                                    WitnessQuestions.Add(Wallet.Name);
                                    CollectedEvidences.Add(Wallet.Name);
                                    Wallet.Finished = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(EviPipe) <= 1.5f && Pipe.Finished == false)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the pipe.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                    Game.DisplayNotification("~b~Clue added:~s~ " + Pipe.ExaminMessage);
                                    FoundClues.Add(Pipe);
                                    WitnessQuestions.Add(Pipe.Name);
                                    CollectedEvidences.Add(Pipe.Name);
                                    Pipe.Finished = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(EviGlasses) <= 1.5f && Glasses.Finished == false)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the glasses.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                    Game.DisplayNotification("~b~Clue added:~s~ " + Glasses.ExaminMessage);
                                    FoundClues.Add(Glasses);
                                    CollectedEvidences.Add(Glasses.Name);
                                    Glasses.Finished = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle.GetBonePosition("door_dside_r")) <= 2f && BloodSplashes.Finished == false)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the car.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                    Game.DisplayNotification("~b~Clue added:~s~ " + BloodSplashes.ExaminMessage);
                                    FoundClues.Add(BloodSplashes);
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ I need an Owner for the following Vehicle: " + SuspectVehicle.LicensePlate.ToString());
                                    Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 3000);
                                    Game.DisplayNotification("~b~Attention to Unit:~s~ Owner of the Vehicle is a " + Functions.GetPersonaForPed(VehOwner).FullName.ToString());
                                    Game.DisplayNotification("~b~New Location added:~s~ Vehicle Owner's house");
                                    FoundClues.Add(VehicleOwner);
                                    LocationClues.Add(VehicleOwner);
                                    GoToLocations.Add(VehicleOwner.LocationName);
                                    VehicleOwner.Name = "Vehicle Owner: " + Functions.GetPersonaForPed(VehOwner).FullName.ToString();
                                    VehicleOwner.Answer = new List<string>
                                {
                                "~b~You:~s~ Does the name " + Functions.GetPersonaForPed(VehOwner).FullName.ToString() +" say anything to you? (1/2)",
                                "~y~Caller:~s~ No Sir. That's not a name I'm familiar with. (2/2)"
                                };
                                    WitnessQuestions.Add(VehicleOwner.Name);
                                    VehicleOwner.Finished = true;
                                    BloodSplashes.Finished = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle.GetBonePosition("boot")) <= 1.5f && Receipt.Finished == false)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the trunk.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.VehicleDoorOpen(SuspectVehicle, Utils.VehDoorID.Trunk, false, false);
                                    Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                    Receipt.ExaminMessage = "Live hog purchase receipt, signed by " + Functions.GetPersonaForPed(OwnerFriend).FullName.ToString() + ", found in abandoned vehicle";
                                    Game.DisplayNotification("~b~Clue added:~s~ " + Receipt.ExaminMessage);
                                    FoundClues.Add(Receipt);
                                    FoundClues.Add(ReceiptWife);
                                    WifeQestions.Add(ReceiptWife.Name);
                                    CollectedEvidences.Add(Receipt.Name);
                                    ReceiptWife.Answer = new List<string>
                                    {
                                    "~b~You:~s~ We've found a receipt in the trunk of your husband's car for a live pig.",
                                    "~y~Wife:~s~ A pig? My husband runs a tool business. That would be " +Functions.GetPersonaForPed(OwnerFriend).FullName+". God knows what he is up to."
                                    };
                                    Receipt.Finished = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(IntroWitness) <= 2f)
                            {
                                if (WitnessPurposeAtScene.Finished == false)
                                {
                                    FoundClues.Add(WitnessPurposeAtScene);
                                    WitnessQuestions.Add(WitnessPurposeAtScene.Name);
                                    WitnessPurposeAtScene.Finished = true;
                                }
                                if (WitnessQuestions.Count != 0 || DialogWithIntroWitnessIndex != DialogWithIntroWitness.Count)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        IntroWitness.TurnToFaceEntity(Game.LocalPlayer.Character);
                                        if (DialogWithIntroWitnessIndex < DialogWithIntroWitness.Count)
                                        {
                                            Game.DisplaySubtitle(DialogWithIntroWitness[DialogWithIntroWitnessIndex]);
                                            DialogWithIntroWitnessIndex++;
                                        }
                                        if (DialogWithIntroWitnessIndex == DialogWithIntroWitness.Count && NextAnswerStuff == false && WitnessQuestions.Count != 0 && NextAnswerStuff2 == false)
                                        {
                                            int negresult = Utils.DisplayAnswers(WitnessQuestions);
                                            if (negresult == 0)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 1)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 2)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 3)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 4)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 5)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }

                                        }
                                        if (NextAnswerStuff == true && NextAnswerStuff2 == false)
                                        {
                                            Clue Clu = FoundClues.Find(x => x.Name == WitnessQuestions[NextAnswerInt]);
                                            if (Clu.AnswerIndex < Clu.Answer.Count)
                                            {
                                                VoiceTalk.Stop();
                                                Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                if (Functions.GetPersonaForPed(Game.LocalPlayer.Character).Gender == LSPD_First_Response.Gender.Female)
                                                {
                                                    try
                                                    {
                                                        if (File.Exists("plugins/LSPDFR/RegularCallouts/AbandondedVehicle/" + Clu.FemaleAnswerVoiceLines[Clu.AnswerIndex] + ".wav"))
                                                        {
                                                            VoiceTalk = new System.Media.SoundPlayer("plugins/LSPDFR/RegularCallouts/AbandondedVehicle/" + Clu.FemaleAnswerVoiceLines[Clu.AnswerIndex] + ".wav");
                                                            VoiceTalk.Play();
                                                        }
                                                    }
                                                    catch(Exception e)
                                                    {
                                                        Game.LogTrivial("Regular Callout Voiceline missing: " + Clu.Name);
                                                    }

                                                }
                                                Clu.AnswerIndex++;
                                            }
                                            if (Clu.AnswerIndex == Clu.Answer.Count)
                                            {
                                                if (Clu.HonestyWitness == "Truth")
                                                {
                                                    IntroWitness.TruthSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 0)
                                                    {
                                                        IntroWitness.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        IntroWitness.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WitnessQuestions.RemoveAt(NextAnswerInt);
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Doubt")
                                                {
                                                    IntroWitness.DoubtSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 1)
                                                    {
                                                        IntroWitness.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        IntroWitness.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WitnessQuestions.RemoveAt(NextAnswerInt);
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Lie")
                                                {
                                                    IntroWitness.LieSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 2)
                                                    {
                                                        IntroWitness.Tasks.Clear();
                                                        NextAnswerStuff3 = true;
                                                    }
                                                }
                                                /*WitnessQuestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff = false;*/
                                            }
                                        }
                                        if (NextAnswerStuff2 == true)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == WitnessQuestions[NextAnswerInt]);
                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                Clu.HonestyAnswerIndex++;
                                            }
                                            if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                            {
                                                WitnessQuestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff2 = false;
                                            }
                                        }
                                        if (NextAnswerStuff3 == true)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == WitnessQuestions[NextAnswerInt]);
                                            int negresult = Utils.DisplayAnswers(CollectedEvidences);
                                            if (negresult == 0)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.WitnessLieStuff == WitnessQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                IntroWitness.Tasks.Clear();
                                                Game.DisplaySubtitle("False");
                                                WitnessQuestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region VehicleOwnerHouseLocation
                        if (CurrentLocation == VehicleOwner.LocationName && Game.LocalPlayer.Character.DistanceTo(VehicleOwner.Location) <= 100f && HouseCleared == false)
                        {
                            ClearUnrelatedEntities();
                            HouseCleared = true;
                        }
                        if (CurrentLocation == VehicleOwner.LocationName && Game.LocalPlayer.Character.DistanceTo(VehicleOwner.Location) <= 50f)
                        {
                            if (Game.LocalPlayer.Character.Position.DistanceTo(VehicleOwner.Location) <= 2f && OwnerOpens == false)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    VehicleOwnerWife.Position = VehicleOwner.Location;
                                    VehicleOwnerWife.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(1500);
                                    OwnerOpens = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.Position.DistanceTo(VehicleOwnerWife) <= 2f && OwnerOpens == true && InInterior == false)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    VehicleOwnerWife.TurnToFaceEntity(Game.LocalPlayer.Character);
                                    if (DialogWithWifeIntroIndex < DialogWithWifeIntro.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithWifeIntro[DialogWithWifeIntroIndex]);
                                        DialogWithWifeIntroIndex++;
                                    }
                                    if (DialogWithWifeIntroIndex == DialogWithWifeIntro.Count)
                                    {
                                        Game.FadeScreenOut(500);
                                        GameFiber.Sleep(1000);
                                        Game.LocalPlayer.Character.Position = InteriorSpawnPoint;
                                        VehicleOwnerWife.Position = new Vector3(264.535919f, -995.967102f, -99.0129242f);
                                        Game.FadeScreenIn(1500);
                                        InInterior = true;
                                    }
                                }
                            }
                        }
                        if (InInterior == true && CurrentLocation == VehicleOwner.LocationName)
                        {
                            if (WifeDialogEnter < 9)
                            {
                                if (Game.LocalPlayer.Character.Position.DistanceTo(VehicleOwnerWife) <= 2f && InInterior == true)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the wife.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        switch (WifeDialogEnter)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Your husband drives a Primo, Mrs. " + Functions.GetPersonaForPed(VehicleOwnerWife).Surname + "?");
                                                WifeDialogEnter++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~y~Wife:~s~ That's correct.");
                                                WifeDialogEnter++;
                                                break;
                                            case 3:
                                                Game.DisplaySubtitle("~b~You:~s~ The car has been found abandoned, and I'm afraid there are signs of foul play.");
                                                WifeDialogEnter++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~y~Wife:~s~ I knew something was wrong when he didn't come home!");
                                                WifeDialogEnter++;
                                                break;
                                            case 5:
                                                Game.DisplaySubtitle("~b~You:~s~ We believe your husband may be injured. We found a pipe on the scene with blood on it.");
                                                WifeDialogEnter++;
                                                break;
                                            case 6:
                                                Game.DisplaySubtitle("~y~Wife:~s~ Oh no! My poor " + Functions.GetPersonaForPed(VehOwner).Forename);
                                                WifeDialogEnter++;
                                                break;
                                            case 7:
                                                Game.DisplaySubtitle("~b~You:~s~ I'd like you to try and stay calm and remember everything you can about last night.");
                                                WifeDialogEnter++;
                                                break;
                                            case 8:
                                                Game.DisplaySubtitle("~b~You:~s~ My partner and I are going to take a look around.");
                                                WifeDialogEnter++;
                                                FoundClues.Add(AlibiWife);
                                                WifeQestions.Add(AlibiWife.Name);
                                                Game.DisplayHelp("Search the house for more clues");
                                                GameFiber.Sleep(3000);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            if (WifeDialogEnter >= 9)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(VehicleOwnerWife) <= 2f && WifeQestions.Count != 0)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if (NextAnswerStuff == false && WifeQestions.Count != 0 && NextAnswerStuff2 == false)
                                        {
                                            int negresult = Utils.DisplayAnswers(WifeQestions);
                                            if (negresult == 0)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        c.AddClue = true;
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 1)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 2)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 3)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 4)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 5)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }

                                        }
                                        if (NextAnswerStuff == true && NextAnswerStuff2 == false)
                                        {
                                            Clue Clu = FoundClues.Find(x => x.Name == WifeQestions[NextAnswerInt]);
                                            if (Clu.AnswerIndex < Clu.Answer.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                Clu.AnswerIndex++;
                                            }
                                            if (Clu.AnswerIndex == Clu.Answer.Count)
                                            {
                                                if (Clu.HonestyWitness == "Truth")
                                                {
                                                    VehicleOwnerWife.TruthSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 0)
                                                    {
                                                        VehicleOwnerWife.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        VehicleOwnerWife.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WifeQestions.RemoveAt(NextAnswerInt);
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Doubt")
                                                {
                                                    VehicleOwnerWife.DoubtSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 1)
                                                    {
                                                        VehicleOwnerWife.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        VehicleOwnerWife.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WifeQestions.RemoveAt(NextAnswerInt);
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Lie")
                                                {
                                                    VehicleOwnerWife.LieSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 2)
                                                    {
                                                        VehicleOwnerWife.Tasks.Clear();
                                                        NextAnswerStuff3 = true;
                                                    }
                                                }
                                                /*WitnessQuestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff = false;*/
                                            }
                                        }
                                        if (NextAnswerStuff2 == true)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == WifeQestions[NextAnswerInt]);
                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                Clu.HonestyAnswerIndex++;
                                            }
                                            if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                            {
                                                WifeQestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff2 = false;
                                            }
                                        }
                                        if (NextAnswerStuff3 == true)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == WifeQestions[NextAnswerInt]);
                                            int negresult = Utils.DisplayAnswers(CollectedEvidences);
                                            if (negresult == 0)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.WitnessLieStuff == WifeQestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                VehicleOwnerWife.Tasks.Clear();
                                                Game.DisplaySubtitle("False");
                                                WifeQestions.RemoveAt(NextAnswerInt);
                                                NextAnswerStuff = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (WifeInteriorLeave == false && Game.LocalPlayer.Character.DistanceTo(InteriorSpawnPoint) <= 1.5f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to leave the house.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    Game.LocalPlayer.Character.Position = VehicleOwner.Location;
                                    Game.FadeScreenIn(1500);
                                    WifeInteriorLeave = true;
                                }
                            }
                            if (WifeInteriorLeave == true && Game.LocalPlayer.Character.DistanceTo(VehicleOwner.Location) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to enter the house.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    Game.LocalPlayer.Character.Position = InteriorSpawnPoint;
                                    Game.FadeScreenIn(1500);
                                    WifeInteriorLeave = false;
                                }
                            }
                        }
                        if (ReceiptWife.AddClue == true)
                        {
                            Game.DisplayNotification("~b~New Location added:~s~ " + BarLocation.LocationName);
                            LocationClues.Add(BarLocation);
                            GoToLocations.Add(BarLocation.LocationName);
                            OwnerFriend.Position = OwnerFriendBarLocation;
                            ReceiptWife.AddClue = false;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(PictureLocation) <= 1.5f && PictureOfAnotherGirl.Finished == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the picture.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                Game.DisplayNotification("~b~Clue added:~s~ " + PictureOfAnotherGirl.ExaminMessage);
                                FoundClues.Add(PictureOfAnotherGirl);
                                WifeQestions.Add(PictureOfAnotherGirl.Name);
                                CollectedEvidences.Add(PictureOfAnotherGirl.Name);
                                PictureOfAnotherGirl.Finished = true;
                            }
                        }
                        #endregion
                        #region BarLocation
                        if (CurrentLocation == BarLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(BarLocation.Location) <= 100f && FollowFriend == false)
                        {
                            if (!FriendVehicle.Exists())
                            {
                                SpawnFriendVehicle();
                            }
                        }
                        if (CurrentLocation == BarLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(BarLocation.Location) <= 50f && FollowFriend == false)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(BarkeeperLocation) <= 2f && BarKeeperDialog < 3)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the Barkeeper.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    switch(BarKeeperDialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~b~You:~s~ Do you know where I can find " + Functions.GetPersonaForPed(OwnerFriend).FullName+"?");
                                            BarKeeperDialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~y~Barkeeper:~s~ He's in the back, near the pooltable.");
                                            BarKeeperDialog++;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(OwnerFriend) <= 2f && OwnerFriendBeginDialog < 7)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    switch (OwnerFriendBeginDialog)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~b~You:~s~ "+ Functions.GetPersonaForPed(OwnerFriend).FullName + "?");
                                            OwnerFriendBeginDialog++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~y~"+Functions.GetPersonaForPed(OwnerFriend).Forename +":~s~ Who's asking?");
                                            OwnerFriendBeginDialog++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~b~You:~s~ San Andreas Police. I understand you are a friend of "+Functions.GetPersonaForPed(VehOwner).FullName);
                                            OwnerFriendBeginDialog++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(OwnerFriend).Forename + ":~s~ Yeah I know him?");
                                            OwnerFriendBeginDialog++;
                                            break;
                                        case 5:
                                            Game.DisplaySubtitle("~b~You:~s~ Are you aware that he's missing?");
                                            OwnerFriendBeginDialog++;
                                            break;
                                        case 6:
                                            Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(OwnerFriend).Forename + ":~s~ No I hadn't heard that. Tough break.");
                                            OwnerFriendBeginDialog++;
                                            LinkToAccidentOwnerFriend.Answer = new List<string>
                                            {
                                                "~b~You:~s~ We found his car abandoned. Covered in blood. You know anything about that?",
                                                "~y~"+Functions.GetPersonaForPed(OwnerFriend).Forename+":~s~ Hell no... I'm sorry to hear that. He's a good boss."
                                            };
                                            LinkToAccidentOwnerFriend.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                                                "~y~"+Functions.GetPersonaForPed(OwnerFriend).Forename+":~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
                                            };
                                            FoundClues.Add(LinkToAccidentOwnerFriend);
                                            FriendQuestions.Add(LinkToAccidentOwnerFriend.Name);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            if (OwnerFriendBeginDialog >= 7)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(OwnerFriend) <= 2f && FriendQuestions.Count != 0)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if (NextAnswerStuff == false && FriendQuestions.Count != 0 && NextAnswerStuff2 == false)
                                        {
                                            int negresult = Utils.DisplayAnswers(FriendQuestions);
                                            if (negresult == 0)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 1)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 2)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 3)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 4)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }
                                            if (negresult == 5)
                                            {
                                                foreach (Clue c in FoundClues)
                                                {
                                                    if (c.Name == FriendQuestions[negresult])
                                                    {
                                                        NextAnswerInt = negresult;
                                                        NextAnswerStuff = true;
                                                    }
                                                }
                                            }

                                        }
                                        if (NextAnswerStuff == true && NextAnswerStuff2 == false)
                                        {
                                            Clue Clu = FoundClues.Find(x => x.Name == FriendQuestions[NextAnswerInt]);
                                            if (Clu.AnswerIndex < Clu.Answer.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                Clu.AnswerIndex++;
                                            }
                                            if (Clu.AnswerIndex == Clu.Answer.Count)
                                            {
                                                if (Clu.HonestyWitness == "Truth")
                                                {
                                                    OwnerFriend.TruthSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 0)
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        FriendQuestions.RemoveAt(NextAnswerInt);
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Doubt")
                                                {
                                                    OwnerFriend.DoubtSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 1)
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                    }
                                                    else
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        FriendQuestions.RemoveAt(NextAnswerInt);
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Lie")
                                                {
                                                    OwnerFriend.LieSpeech();
                                                    int negresult = Utils.DisplayAnswers(ChooseTruth);
                                                    if (negresult == 2)
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        NextAnswerStuff3 = true;
                                                    }
                                                    else
                                                    {
                                                        OwnerFriend.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        FriendQuestions.RemoveAt(NextAnswerInt);
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (NextAnswerStuff2 == true)
                                        {
                                            NextAnswerStuff = false;
                                            NextAnswerStuff3 = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == FriendQuestions[NextAnswerInt]);
                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                Clu.HonestyAnswerIndex++;
                                            }
                                            if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                            {
                                                FriendQuestions.RemoveAt(NextAnswerInt);
                                                Clu.AddClue = true;
                                                NextAnswerStuff2 = false;
                                            }
                                        }
                                        if (NextAnswerStuff3 == true)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == FriendQuestions[NextAnswerInt]);
                                            int negresult = Utils.DisplayAnswers(CollectedEvidences);
                                            if (negresult == 0)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 1)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 2)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 3)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 4)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 5)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 6)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 7)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else if (negresult == 8)
                                            {
                                                if (Clu.WitnessLieStuff == CollectedEvidences[negresult])
                                                {
                                                    NextAnswerStuff2 = true;
                                                }
                                            }
                                            else
                                            {
                                                OwnerFriend.Tasks.Clear();
                                                Game.DisplaySubtitle("False");
                                                FriendQuestions.RemoveAt(NextAnswerInt);
                                                Clu.WrongAnswer = true;
                                                NextAnswerStuff3 = false;
                                            }
                                        }
                                        if (LinkToAccidentOwnerFriend.AddClue == true)
                                        {
                                            {
                                                VehicleOwnerCurrentLocation.Name = "Location of " + Functions.GetPersonaForPed(VehOwner).FullName;
                                                VehicleOwnerCurrentLocation.Answer = new List<string>
                                                {
                                                    "~b~You:~s~ Where exactly is "+Functions.GetPersonaForPed(VehOwner).Surname+" holed up?",
                                                    "~y~"+Functions.GetPersonaForPed(OwnerFriend).Forename+":~s~ No idea... I think he went to Vice City."
                                                };
                                                VehicleOwnerCurrentLocation.WitnessHonestyAnswers = new List<string>
                                                {
                                                    "~b~You:~s~ I'm tired of your shit. Tell me or I will arrest you for obstruction.",
                                                    "~y~"+Functions.GetPersonaForPed(OwnerFriend).Forename+":~s~ He's at my place. Here I will write you the address."
                                                };
                                                FoundClues.Add(VehicleOwnerCurrentLocation);
                                                FriendQuestions.Add(VehicleOwnerCurrentLocation.Name);
                                                LinkToAccidentOwnerFriend.AddClue = false;
                                            }
                                        }
                                        if (VehicleOwnerCurrentLocation.AddClue == true)
                                        {
                                            OwnerFriendsHouseLocation.LocationName = Functions.GetPersonaForPed(OwnerFriend).FullName + " House";
                                            LocationClues.Add(OwnerFriendsHouseLocation);
                                            GoToLocations.Add(OwnerFriendsHouseLocation.LocationName);
                                            OwnerFriendsHouseLocation.Location = new Vector3(2319.445f, 2553.302f, 47.69053f);
                                            Game.DisplayNotification("~b~New Location added:~s~ " + Functions.GetPersonaForPed(OwnerFriend).FullName + " House");
                                            VehicleOwnerCurrentLocation.AddClue = false;
                                        }
                                        if (LinkToAccidentOwnerFriend.WrongAnswer == true || VehicleOwnerCurrentLocation.WrongAnswer == true)
                                        {
                                            if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                            Game.DisplayHelp("Go back to your vehicle and follow him unnoticed.");
                                            FollowFriend = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (FollowFriend == true)
                        {
                            if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && PlayerInCar == false)
                            {
                                HouseBlip = FriendVehicle.AttachBlip();
                                OwnerFriend.Tasks.FollowNavigationMeshToPosition(FriendVehicle.GetOffsetPosition(new Vector3(-2f, 0f, 0f)), 0f, 1f).WaitForCompletion(15000);
                                OwnerFriend.Tasks.EnterVehicle(FriendVehicle, -1).WaitForCompletion();
                                OwnerFriend.Tasks.DriveToPosition(DriveToLocation, 15f, VehicleDrivingFlags.Normal | VehicleDrivingFlags.StopAtDestination);
                                PlayerInCar = true;
                            }
                            if (PlayerInCar == true && ExitCar == false)
                            {
                                AwarenessBar.Percentage = Awareness;
                                AwarenessBar.ForegroundColor = Color.Red;
                                AwarenessBar.BackgroundColor = ControlPaint.Dark(Color.Red);
                                AwarenessBarPool.Draw();
                                if (Game.LocalPlayer.Character.DistanceTo(OwnerFriend) <= 20f)
                                {
                                    Awareness += 0.0050f;
                                }
                                else
                                {
                                    if (Awareness > 0)
                                    {
                                        Awareness -= 0.0030f;
                                    }
                                }
                                if (Game.LocalPlayer.Character.CurrentVehicle.IsSirenOn == true)
                                {
                                    Awareness += 1f;
                                }
                                if (Awareness >= 1f)
                                {
                                    Game.DisplayHelp("You alerted the Driver!");
                                    this.End();
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(OwnerFriend) > 150f)
                                {
                                    Game.DisplayHelp("You lost the Driver!");
                                    this.End();
                                }
                                if (OwnerFriend.DistanceTo(DriveToLocation) <= 4f)
                                {
                                    OwnerFriend.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion();
                                    OwnerFriend.Tasks.FollowNavigationMeshToPosition(FriendHouseDoorLocation, 0f, 2f).WaitForCompletion();
                                    OwnerFriend.Position = StorageSpawn;
                                    ExitCar = true;
                                }
                            }
                            if (ExitCar == true)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(FriendHouseDoorLocation) <= 2f && VehOwnerOpensDoor == false)
                                {
                                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.DisplayNotification("You rang the door!");
                                        GameFiber.Sleep(3000);
                                        Game.FadeScreenOut(500);
                                        GameFiber.Sleep(1000);
                                        VehOwner.Position = FriendHouseDoorLocation;
                                        VehOwner.Face(Game.LocalPlayer.Character);
                                        Game.FadeScreenIn(1500);
                                        VehOwnerOpensDoor = true;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(VehOwner) < 2f && VehOwnerAnswerDoor < 5)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        switch (VehOwnerAnswerDoor)
                                        {
                                            case 1:
                                                Game.DisplaySubtitle("~b~You:~s~ Mr. " + Functions.GetPersonaForPed(VehOwner).Surname + "?");
                                                VehOwnerAnswerDoor++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(VehOwner).Forename + ":~s~ Yes?");
                                                VehOwnerAnswerDoor++;
                                                break;
                                            case 3:
                                                Game.DisplaySubtitle("~b~You:~s~ You are under arrest for obstruction of justice.");
                                                VehOwnerAnswerDoor++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(VehOwner).Forename + ":~s~ I'm sorry. Give me a few seconds I will just grab my things and come peacefully.");
                                                VehOwnerAnswerDoor++;
                                                VehOwner.Position = VehicleOwner.Location;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                if (VehOwnerAnswerDoor >= 5 && ChaseActivate == false)
                                {
                                    GameFiber.Sleep(3000);
                                    VehOwner.Position = FriendHouseFleeLocation;
                                    Game.DisplaySubtitle("~b~You:~s~ Fuck he's running!");
                                    Chase = Functions.CreatePursuit();
                                    Functions.AddPedToPursuit(Chase, VehOwner);
                                    Functions.SetPursuitIsActiveForPlayer(Chase, true);
                                    ChaseActivate = true;
                                }
                                if (ChaseActivate == true)
                                {
                                    if (Functions.IsPursuitStillRunning(Chase) == false)
                                    {
                                        this.End();
                                    }
                                }
                            }
                        }
                        #endregion
                        #region FriendHouseLocation
                        if (CurrentLocation == OwnerFriendsHouseLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(OwnerFriendsHouseLocation.Location) <= 50f)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(FriendHouseDoorLocation) <= 2f && VehOwnerOpensDoor == false)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Game.DisplayNotification("You rang the door!");
                                    GameFiber.Sleep(3000);
                                    Game.FadeScreenOut(500);
                                    GameFiber.Sleep(1000);
                                    VehOwner.Position = FriendHouseDoorLocation;
                                    VehOwner.Face(Game.LocalPlayer.Character);
                                    Game.FadeScreenIn(1500);
                                    VehOwnerOpensDoor = true;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(VehOwner) < 2f && VehOwnerAnswerDoor < 5)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    switch (VehOwnerAnswerDoor)
                                    {
                                        case 1:
                                            Game.DisplaySubtitle("~b~You:~s~ Mr. " + Functions.GetPersonaForPed(VehOwner).Surname + "?");
                                            VehOwnerAnswerDoor++;
                                            break;
                                        case 2:
                                            Game.DisplaySubtitle("~y~"+ Functions.GetPersonaForPed(VehOwner).Forename + ":~s~ Yes?");
                                            VehOwnerAnswerDoor++;
                                            break;
                                        case 3:
                                            Game.DisplaySubtitle("~b~You:~s~ You are under arrest for obstruction of justice.");
                                            VehOwnerAnswerDoor++;
                                            break;
                                        case 4:
                                            Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(VehOwner).Forename + ":~s~ I'm sorry. Give me a few seconds I will just grab my things and come peacefully.");
                                            VehOwnerAnswerDoor++;
                                            VehOwner.Position = VehicleOwner.Location;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            if (VehOwnerAnswerDoor >= 5 && ChaseActivate == false)
                            {
                                GameFiber.Sleep(3000);
                                VehOwner.Position = FriendHouseFleeLocation;
                                Game.DisplaySubtitle("~b~You:~s~ Fuck he's running!");
                                Chase = Functions.CreatePursuit();
                                Functions.AddPedToPursuit(Chase, VehOwner);
                                Functions.SetPursuitIsActiveForPlayer(Chase, true);
                                ChaseActivate = true;
                            }
                            if (ChaseActivate == true)
                            {
                                if (Functions.IsPursuitStillRunning(Chase) == false)
                                {
                                    this.End();
                                }
                            }
                        }
                            #endregion
                        #region SelectDestination
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) == true && LocationMenu == true && ChaseActivate == false && FollowFriend == false)
                        {
                            int negresult = Utils.DisplayAnswers(GoToLocations);
                            if (negresult == 0)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                            if (negresult == 1)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                            if (negresult == 2)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                            if (negresult == 3)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                            if (negresult == 4)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                            if (negresult == 5)
                            {
                                foreach (Clue c in LocationClues)
                                {
                                    if (c.LocationName == GoToLocations[negresult])
                                    {
                                        if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                        HouseBlip = new Blip(c.Location);
                                        HouseBlip.Color = Color.Yellow;
                                        HouseBlip.EnableRoute(Color.Yellow);
                                        NextAnswerStuff = false;
                                        NextAnswerStuff2 = false;
                                        NextAnswerStuff3 = false;
                                        CurrentLocation = c.LocationName;
                                    }
                                }
                                LocationMenu = false;
                            }
                        }
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) == false && LocationMenu == false && ChaseActivate == false && FollowFriend == false)
                        {
                            LocationMenu = true;
                        }
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    Game.LogTrivial("Regular Callouts Error: "+e);
                }
            });
        }
        public override void End()
        {
            CalloutRunning = false;
            foreach (Vehicle i in AllSpawnedPoliceVehicles)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
                if (i.IsMission())
                {
                    i.RemoveMission();
                }
            }
            Utils.DeleteDecal(Blood);
            Utils.DeleteDecal(Blood2);
            foreach (Ped i in AllSpawnedPeds)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
                if (i.IsMission())
                {
                    i.RemoveMission();
                }
            }
            foreach (Rage.Object i in AllSpawnedObjects)
            {
                if (i.Exists())
                {
                    i.Delete();
                }
                if (i.IsMission())
                {
                    i.RemoveMission();
                }
            }
            /*foreach (Clue c in AllPossibleClues)
            {
                c.Finished = false;
            }*/
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (HouseBlip.Exists()) { HouseBlip.Delete(); }
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
        private void SpawnAllCopCars()
        {
            Utils.AddLog("Spawning copcars");
            for (int i = 0; i < CopCarLocation.Count; i++)
            {
                Vehicle cc = new Vehicle(new Model(CarModels[new Random().Next(CarModels.Length)]), CopCarLocation[i], CopCarHeading[i]);
                cc.IsPersistent = true;
                cc.IsSirenOn = true;
                cc.MakeMission();
                cc.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AllSpawnedPoliceVehicles.Add(cc);
                AllBankHeistEntities.Add(cc);
            }
            Utils.AddLog("spawned copcars");
        }
        private void SpawnAllPolicePeds()
        {
            if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
            {
                UltBackupSpawn();
            }
            else
            {
                for (int i = 0; i < PolicePedLocations.Count; i++)
                {
                    Game.LogTrivial("UltBackup not found");
                    Ped officer = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), PolicePedLocations[i], PolicePedHeadings[i]);
                    officer.IsPersistent = true;
                    officer.BlockPermanentEvents = true;
                    officer.MakeMission();
                    officer.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                    AllSpawnedPeds.Add(officer);
                    AllBankHeistEntities.Add(officer);
                }
            }  
        }
        private void SpawnAllMarkers()
        {
            for (int i = 0; i < MarkerLocations.Count; i++)
            {
                Rage.Object marker = new Rage.Object("ch_prop_ch_fib_01a", MarkerLocations[i], MarkerHeading[i]);
                marker.IsPersistent = true;
                marker.MakeMission();
                AllSpawnedObjects.Add(marker);
                AllBankHeistEntities.Add(marker);
            }
            for (int i = 0; i < BarrierLocations.Count; i++)
            {
                Rage.Object barrier = new Rage.Object("prop_barrier_work05", BarrierLocations[i], BarrierHeadings[i]);
                barrier.IsPersistent = true;
                barrier.MakeMission();
                AllSpawnedObjects.Add(barrier);
                AllBankHeistEntities.Add(barrier);
            }
        }
        private void SpawnSuspectVehicle()
        {
            SuspectVehicle = new Vehicle("primo", SuspectVehicleLocation, SuspectVehicleHeading);
            SuspectVehicle.IsPersistent = true;
            SuspectVehicle.MakeMission();
            Functions.SetVehicleOwnerName(SuspectVehicle, Functions.GetPersonaForPed(VehOwner).FullName.ToString());
            Utils.VehicleDoorOpen(SuspectVehicle, Utils.VehDoorID.FrontRight, false, false);
            Utils.VehicleDoorOpen(SuspectVehicle, Utils.VehDoorID.FrontLeft, false, false);
            AllSpawnedPoliceVehicles.Add(SuspectVehicle);
            AllBankHeistEntities.Add(SuspectVehicle);
        }
        private void SpawnFriendVehicle()
        {
            FriendVehicle = new Vehicle("asterope", FriendVehicleLocation, FriendVehicleHeading);
            Functions.SetVehicleOwnerName(FriendVehicle, Functions.GetPersonaForPed(OwnerFriend).FullName.ToString());
            AllSpawnedPoliceVehicles.Add(FriendVehicle);
            AllBankHeistEntities.Add(FriendVehicle);
        }
        private void SpawnScientists()
        {
            ScientistA = new Ped("s_m_m_scientist_01", ScientistALocation, 0f);
            ScientistA.IsPersistent = true;
            ScientistA.BlockPermanentEvents = true;
            ScientistA.MakeMission();
            ScientistA.Face(SuspectVehicle);
            ScientistA.Tasks.PlayAnimation("amb@world_human_paparazzi@male@idle_a", "idle_c", 1f, AnimationFlags.Loop);
            AllSpawnedPeds.Add(ScientistA);
            AllBankHeistEntities.Add(ScientistA);
            Rage.Object camera = new Rage.Object("ch_prop_ch_camera_01", ScientistA.FrontPosition);
            camera.IsPersistent = true;
            camera.AttachTo(ScientistA, 72, new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f));
            AllSpawnedObjects.Add(camera);
            AllBankHeistEntities.Add(camera);
            ScientistB = new Ped("s_m_m_scientist_01", ScientistBLocation, 0f);
            ScientistB.IsPersistent = true;
            ScientistB.BlockPermanentEvents = true;
            ScientistB.MakeMission();
            ScientistB.Face(SuspectVehicle);
            ScientistB.Tasks.PlayAnimation("amb@world_human_gardener_plant@male@idle_a", "idle_a", 1f, AnimationFlags.Loop);
            AllSpawnedPeds.Add(ScientistB);
            AllBankHeistEntities.Add(ScientistB);
            IntroWitness = new Ped("s_m_y_airworker", IntroWitnessLocation, IntroWitnessHeading);
            IntroWitness.IsPersistent = true;
            IntroWitness.BlockPermanentEvents = true;
            IntroWitness.MakeMission();
            AllSpawnedPeds.Add(IntroWitness);
            AllBankHeistEntities.Add(IntroWitness);
        }
        private void SpawnAllEvidences()
        {
            EviWallet = new Rage.Object("prop_ld_wallet_02", WalletLocation);
            EviWallet.IsPersistent = true;
            EviWallet.MakeMission();
            AllSpawnedObjects.Add(EviWallet);
            AllBankHeistEntities.Add(EviWallet);
            EviPipe = new Rage.Object("ba_prop_battle_sniffing_pipe", PipeLocation);
            EviPipe.IsPersistent = true;
            EviPipe.MakeMission();
            AllSpawnedObjects.Add(EviPipe);
            AllBankHeistEntities.Add(EviPipe);
            EviGlasses = new Rage.Object("prop_cs_sol_glasses", Utils.ToGround(GlassesLocation));
            EviGlasses.IsPersistent = true;
            EviGlasses.MakeMission();
            AllSpawnedObjects.Add(EviGlasses);
            AllBankHeistEntities.Add(EviGlasses);
        }
        private void SpawnOwnerandFriend()
        {
            Utils.AddLog("Spawning ShopOwnerandFriend");
            VehOwner = new Ped("a_m_m_farmer_01", StorageSpawn, 0f);
            VehOwner.IsPersistent = true;
            VehOwner.BlockPermanentEvents = true;
            VehOwner.MakeMission();
            AllSpawnedPeds.Add(VehOwner);
            AllBankHeistEntities.Add(VehOwner);
            Utils.AddLog("spawned owner");
            OwnerFriend = new Ped("s_m_m_gaffer_01", StorageSpawn, 0f);
            OwnerFriend.IsPersistent = true;
            OwnerFriend.BlockPermanentEvents = true;
            OwnerFriend.MakeMission();
            AllSpawnedPeds.Add(OwnerFriend);
            AllBankHeistEntities.Add(OwnerFriend);
            Utils.AddLog("spawned friend");
            //VehicleOwnerWife = new Ped("s_m_m_gaffer_01", StorageSpawn, 0f);
            VehicleOwnerWife = new Ped("u_f_y_hotposh_01", StorageSpawn, 0f);
            Utils.AddLog("spawning wife model");
            VehicleOwnerWife.IsPersistent = true;
            Utils.AddLog("making wife persisten");
            VehicleOwnerWife.BlockPermanentEvents = true;
            Utils.AddLog("making wife block events");
            Functions.SetPersonaForPed(VehicleOwnerWife, new LSPD_First_Response.Engine.Scripting.Entities.Persona(Functions.GetPersonaForPed(VehicleOwnerWife).Forename.ToString(), Functions.GetPersonaForPed(VehOwner).Surname.ToString(), Functions.GetPersonaForPed(VehicleOwnerWife).Gender));
            Utils.AddLog("set wife persona");
            VehicleOwnerWife.MakeMission();
            Utils.AddLog("made wife missionped");
            AllSpawnedPeds.Add(VehicleOwnerWife);
            Utils.AddLog("added wife to list");
            AllBankHeistEntities.Add(VehicleOwnerWife);
            Utils.AddLog("spawned wife... entering next spawner");
        }
        private void UltBackupSpawn()
        {
            for (int i = 0; i < PolicePedLocations.Count; i++)
            {
                Game.LogTrivial("UltBackup found");
                Ped officer = UltimateBackup.API.Functions.getLocalPatrolPed(PolicePedLocations[i], PolicePedHeadings[i]);
                officer.IsPersistent = true;
                officer.BlockPermanentEvents = true;
                officer.MakeMission();
                officer.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                AllSpawnedPeds.Add(officer);
                AllBankHeistEntities.Add(officer);
            }
        }
    }

}
