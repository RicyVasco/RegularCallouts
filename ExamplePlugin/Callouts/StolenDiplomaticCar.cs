using LSPD_First_Response.Engine.Scripting.Entities;
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
using System.Windows.Forms;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Stolen Diplomatic Vehicle", CalloutProbability.Medium)]
    public class StolenDiplomaticCar : Callout
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
            public List<string> WitnessLieStuff;
            public bool AddClue;
            public bool WrongAnswer;
            public int PreLieAnswersIndex { get; set; }
            public List<string> PreLieAnswers { get; set; }
        }
        private Clue AccidentLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Accident Location"
        };
        private Clue RaceLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(0f, 0f, 0f),
            LocationSet = false,
            LocationName = "Suspect's current Location"
        };
        private Clue PDLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(447.273376f, -978.486389f, 29.6895943f),
            LocationSet = false,
            LocationName = "LSPD Location"
        };
        private Clue CardealerLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(-36.5651855f, -1098.64197f, 25.4223442f),
            LocationSet = false,
            LocationName = "Cardealer Location"
        };
        private Clue SuspectHouseLocation = new Clue
        {
            HasLocation = false,
            Location = new Vector3(-14.19971f, -1441.728f, 31.20133f),
            LocationSet = false,
            LocationName = "Suspect's House Location"
        };
        private Clue MissingWheel = new Clue
        {
            Name = "Missing Wheel",
            ExaminMessage = "Front right wheel removed from abandoned vehicle.",
            Answer = new List<string>
            {
                
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue PurchaseHistory = new Clue
        {
            Name = "Vehicle purchase history",
            ExaminMessage = "",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue AlledgedBribery = new Clue
        {
            Name = "Alleged Bribery",
            ExaminMessage = "Alledged Deal between the car dealer and the consul general",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue MotiveTheft = new Clue
        {
            Name = "Motive for auto theft",
            ExaminMessage = "",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue LastContactSuspect = new Clue
        {
            Name = "Last contact with ~PlaceHolder~",
            ExaminMessage = "",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Lie",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Breakfast plates"
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue LicensePlateRecovered = new Clue
        {
            Name = "Diplomatic plates recovered",
            ExaminMessage = "License Plates recovered from the suspects garage",
            Answer = new List<string>
            {
                "~b~You:~s~ We've found a license plate matching our stolen vehicle in the garage.",
                "~b~You:~s~ Add in the assortment of parts, and we can make -placeholder- for a dozen other thefts.",
                "~b~You:~s~ It's time to get serious, -placeholder-.",
                "~y~Girlfriend:~s~ You must ask these questions of -placeholder-",
                "~y~Girlfriend:~s~ I know nothing about these car parts.",
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Then tell us where he is!",
                "~b~You:~s~ If your baby is born in prison, -placeholder-, the corrections officers will take it from you.",
                "~b~You:~s~ You'll see your son or daughter through a metal grate for half an hour a week.",
                "~y~Girlfriend:~s~ The start line is on -Placeholderstreet-."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WheelsRecovered = new Clue
        {
            Name = "Wheels from the stolen diplomatic vehicle",
            ExaminMessage = "",
            Answer = new List<string>
            {
                "~b~You:~s~ We've found a license plate matching our stolen vehicle in the garage.",
                "~b~You:~s~ Add in the assortment of parts, and we can make -placeholder- for a dozen other thefts.",
                "~b~You:~s~ It's time to get serious, -placeholder-.",
                "~y~Girlfriend:~s~ You must ask these questions of -placeholder-",
                "~y~Girlfriend:~s~ I know nothing about these car parts.",
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Then tell us where he is!",
                "~b~You:~s~ If your baby is born in prison, -placeholder-, the corrections officers will take it from you.",
                "~b~You:~s~ You'll see your son or daughter through a metal grate for half an hour a week.",
                "~y~Girlfriend:~s~ The start line is on -Placeholderstreet-."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue BreakfastPlates = new Clue
        {
            Name = "Breakfast plates",
            ExaminMessage = "Breakfast plates for two people found in the suspect's home",
            Answer = new List<string>
            {
                "~b~You:~s~ We've found a license plate matching our stolen vehicle in the garage.",
                "~b~You:~s~ Add in the assortment of parts, and we can make -placeholder- for a dozen other thefts.",
                "~b~You:~s~ It's time to get serious, -placeholder-.",
                "~y~Girlfriend:~s~ You must ask these questions of -placeholder-",
                "~y~Girlfriend:~s~ I know nothing about these car parts.",
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Then tell us where he is!",
                "~b~You:~s~ If your baby is born in prison, -placeholder-, the corrections officers will take it from you.",
                "~b~You:~s~ You'll see your son or daughter through a metal grate for half an hour a week.",
                "~y~Girlfriend:~s~ The start line is on -Placeholderstreet-."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue TheftConsuleVehicle = new Clue
        {
            Name = "Theft of consular vehicle",
            ExaminMessage = "",
            Answer = new List<string>
            {
                 "~b~You:~s~ Consul general, we have located your car. Can you tell us how it was stolen?",
                 "~y~Consul:~s~ It must have been stolen from the Consular garage. Terrible inconvenient of course.",
                 "~y~Consul:~s~ I want the perpetrator soundly flogged."
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You have a pretty good idea who stole the car, don't you, Consul General?",
                "~b~You:~s~ Are you gonna tell me or do I shake it out of you?",
                "~y~Consul:~s~ I suspect a disgruntled boy from the car dealership.",
                "~b~You:~s~ You have a name for this kid?",
                "~y~Consul:~s~ -Placeholder-. I have no surname.",
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue AssociationMechanic = new Clue
        {
            Name = "Association with the mechanic",
            ExaminMessage = "",
            Answer = new List<string>
            {
                 "~b~You:~s~ Consul general, we have located your car. Can you tell us how it was stolen?",
                 "~y~Consul:~s~ It must have been stolen from the Consular garage. Terrible inconvenient of course.",
                 "~y~Consul:~s~ I want the perpetrator soundly flogged."
            },
            AnswerIndex = 0,
            HonestyWitness = "Lie",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You have a pretty good idea who stole the car, don't you, Consul General?",
                "~b~You:~s~ Are you gonna tell me or do I shake it out of you?",
                "~y~Consul:~s~ I suspect a disgruntled boy from the car dealership.",
                "~b~You:~s~ You have a name for this kid?",
                "~y~Consul:~s~ -Placeholder-. I have no surname.",
            },
            PreLieAnswersIndex = 0,
            PreLieAnswers = new List<string>
            {
                "~b~You:~s~ You have a pretty good idea who stole the car, don't you, Consul General?",
                "~b~You:~s~ Are you gonna tell me or do I shake it out of you?",
                "~y~Consul:~s~ I suspect a disgruntled boy from the car dealership.",
                "~b~You:~s~ You have a name for this kid?",
                "~y~Consul:~s~ -Placeholder-. I have no surname.",
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Book belongin to Argentenian Consul Member",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue AssociationConsul = new Clue
        {
            Name = "Association with the Consul",
            ExaminMessage = "",
            Answer = new List<string>
            {
                 "~b~You:~s~ This doesn't look like the kind of place to be favored by foreign embassies.",
                 "~b~You:~s~ How do you know -PlaceHolder-?",
                 "~y~Simeon:~s~ I don't know -Placeholder-. The Embassy bought the car.",
                 "~y~Simeon:~s~ All I know is, he must know a quality car when he sees one."
            },
            AnswerIndex = 0,
            HonestyWitness = "Lie",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ And I know a shyster when I see one. You and -Placeholder- are in this together.",
                "~y~Simeon:~s~ Me and -Placeholder-? I hardly know him. He is a racist and I don't like him."
            },
            PreLieAnswersIndex = 0,
            PreLieAnswers = new List<string>
            {
                "~b~You:~s~ We found your contact details in -Placeholder-'s notebook. He had to be calling you for something, Yetarian.",
                "~y~Simeon:~s~ Okay. So I met him in a bar.",
                "~y~Simeon:~s~ We cut a deal and he bought the car through the Embassy.",
                "~y~Simeon:~s~ I cut him some change on the side. It happens all the time.",
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Book belongin to Argentenian Consul Member",
                "Alleged Bribery"
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue MissingTools = new Clue
        {
            Name = "Missing Tools",
            ExaminMessage = "3/4 wrench missing from Yetarian's car dealership",
            Answer = new List<string>
            {
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
            },
            PreLieAnswersIndex = 0,
            PreLieAnswers = new List<string>
            {
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WhereaboutsMechanic = new Clue
        {
            Name = "Whereabouts of the Mechanic",
            ExaminMessage = "",
            Answer = new List<string>
            {
                 "~b~You:~s~ Where can we find -Placeholder-?",
                 "~y~Simeon:~s~ I don't know. He sure as hell isn't here.",
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Address, Simeon. Or we'll take a closer look at your business.",
                "~y~Simeon:~s~ Okay, alright!",
                "~y~Simeon:~s~ -StreetPlaceholder-"
            },
            PreLieAnswersIndex = 0,
            PreLieAnswers = new List<string>
            {
                "~b~You:~s~ We found your contact details in -Placeholder-'s notebook. He had to be calling you for something, Yetarian.",
                "~y~Simeon:~s~ Okay. So I met him in a bar.",
                "~y~Simeon:~s~ We cut a deal and he bought the car through the Embassy.",
                "~y~Simeon:~s~ I cut him some change on the side. It happens all the time.",
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Book belongin to Argentenian Consul Member",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WrenchInTheft = new Clue
        {
            Name = "Wrench used in auto theft",
            ExaminMessage = "",
            Answer = new List<string>
            {
                 "~b~You:~s~ A wrench from this dealership was used to strip the wheels of a consulate's car last night, Mr. Yetarian.",
                 "~b~You:~s~ A couple of Hispanics were seen taking parts.",
                 "~y~Simeon:~s~ We've had a spate of thefts ourselves. Comes with the location.",
                 "~y~Simeon:~s~ Thieving bastards will steal anything the minute your back is turned.",
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ What are you hiding, Yetarian? Spill it. You don't want the LSPD getting too interested in this place.",
                "~y~Simeon:~s~ So I hire a few illegal.",
                "~y~Simeon:~s~ It's cheaper than hiring a few vets, and they have less attitude. Downside is, they're a little light-fingered.",
            },
            PreLieAnswersIndex = 0,
            PreLieAnswers = new List<string>
            {
                "~b~You:~s~ We found your contact details in -Placeholder-'s notebook. He had to be calling you for something, Yetarian.",
                "~y~Simeon:~s~ Okay. So I met him in a bar.",
                "~y~Simeon:~s~ We cut a deal and he bought the car through the Embassy.",
                "~y~Simeon:~s~ I cut him some change on the side. It happens all the time.",
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Book belongin to Argentenian Consul Member",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue Wrench = new Clue
        {
            Name = "Garage Wrench",
            ExaminMessage = "3/4 wrench, marked 'Yeterian Dealership', found near abandoned vehicle.",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue RegSlip = new Clue
        {
            Name = "Registration slip",
            ExaminMessage = "Abandoned vehicle registered to Argentinean Consulate",
            Answer = new List<string>
            {

            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WitnessReport = new Clue
        {
            Name = "Witness Report",
            ExaminMessage = "Witness Report from ...",
            Answer = new List<string>
            {
                "~b~You:~s~ Did you see who stole the car?",
                "~y~Witness:~s~ Hell yes I did! I saw three guys going to work on it."
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Can you tell me what they were doing?",
                "~y~Witness:~s~ They used a sport's car headlight so they could strip the vehicle.",
                "~y~Witness:~s~ I yelled out to them \u0022I'll call the cops!\u0022, so they loaded up their car and drove off. "
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Receipt",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WitnessPossibleSuspects = new Clue
        {
            Name = "Possible Suspects",
            ExaminMessage = "Possible Suspects described by the Witness",
            Answer = new List<string>
            {
                "~b~You:~s~ After the guys left, you didn't go near the car?",
                "~y~Witness:~s~ After I scared them off... no, I didn't go anywhere near that car."
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You went out to the car once they drove off. Your so noisy you couldn't help but check it out yourself.",
                "~y~Witness:~s~ I was curious. Ain't no law against that. So what if I took a look?"
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Receipt",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WitnessPossibleSuspectCar = new Clue
        {
            Name = "Possible Suspects Vehicle",
            ExaminMessage = "Possible Vehicle used by the Suspects described by the Witness",
            Answer = new List<string>
            {
                "~b~You:~s~ Tell me about the car they were driving.",
                "~y~Witness:~s~ It was a sports car. I didn't catch the license plate."
            },
            AnswerIndex = 0,
            HonestyWitness = "Truth",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You look like the type of guy to remember details.",
                "~y~Witness:~s~ You're correct. It was an older car but it had a fresh red paint on it."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Receipt",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue WitnessVehStrippedofParts = new Clue
        {
            Name = "Vehicle stripped of parts",
            ExaminMessage = "",
            Answer = new List<string>
            {
                "~b~You:~s~ What exactly did you see them take?",
                "~y~Witness:~s~ They were working on the tires. That's all that was took."
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ Right. So what did you take? You want me to frisk you down?",
                "~y~Witness:~s~ I found a notebook in the gloves compartment. I was gonna show you! Here I'll drop it for you."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Receipt",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private Clue BookOfConsul = new Clue
        {
            Name = "Book belongin to Argentenian Consul Member",
            ExaminMessage = "Book belong to someone named ...",
            Answer = new List<string>
            {
                "~b~You:~s~ Answer1",
                "~y~Wife:~s~ Answer2"
            },
            AnswerIndex = 0,
            HonestyWitness = "Doubt",
            WitnessHonestyAnswers = new List<string>
            {
                "~b~You:~s~ You were there. We found a receipt in the trunk of the car with your name on it.",
                "~y~Wife:~s~ Alright. That fool has fallen in love for some girl in Vice City. He wanted me to make it look like he was attacked."
            },
            HonestyAnswerIndex = 0,
            WitnessLieStuff = new List<string>
            {
                "Receipt",
            },
            AddClue = false,
            WrongAnswer = false
        };
        private List<Clue> FoundClues = new List<Clue> { };
        private List<Clue> LocationClues = new List<Clue> { };
        private List<string> GoToLocations = new List<string> { };

        #region AllStuff
        private Vector3 SpawnPoint;
        private Blip SpawnBlip;
        private bool SetConsulPos;
        private bool SimeonIntro;
        private Vector3 SpawnPoint1;
        private List<Entity> AllBankHeistEntities = new List<Entity>();
        //private System.Media.SoundPlayer Turklingel = new System.Media.SoundPlayer("LSPDFR/audio/scanner/RegularCallouts/DORBELL.wav");
        private Vehicle SuspectVehicle;
        private Vector3 SuspectVehicleLocation;
        private float SuspectVehicleHeading;
        public string VehOwnerString;
        private static string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private List<Rage.Object> AllSpawnedObjects = new List<Rage.Object>();
        private List<Vehicle> AllSpawnedPoliceVehicles = new List<Vehicle>();
        private List<Ped> AllSpawnedPeds = new List<Ped>();
        private List<Vector3> PolicePedLocations;
        private List<Vector3> PlateLocations;
        private Tuple<Vector3, Rotator> NumberPlateLocation;
        private Vector3 WheelLocation;
        private Vector3 HouseFrontDoorLocation;
        private Vector3 HouseFrontDoorInsideLocation;
        private Vector3 HouseWifeSpawn;
        private List<float> PolicePedHeadings;
        private List<Vector3> BarrierLocations;
        private Ped ConsulPed;
        private Ped Suspect;
        private List<float> BarrierHeadings;
        private bool ChaseActivate;
        private bool SearchTrunk;
        private int SupervisorDialog = 1;
        private Ped Wife;
        private bool NextAnswerStuff2;
        public Vector3 StorageSpawn;
        private bool NextAnswerStuff3;
        private int DoorEnterCheckpoint;
        private int DoorExitCheckpoint;
        private Tuple<uint, Vector3, float> FrontDoor;
        private Tuple<uint, Vector3, float> GarageDoor;
        private bool NextAnswerStuff4;
        private int WitnessIntroDialog = 1;
        private bool LocationRaceSet;
        private bool SimeonLocSet;
        private Vehicle RaceVehicle;
        private bool WifeSetPos;
        private Ped ScientistA;
        private MenuPool _MenuPool;
        private bool ActivatedChase;
        private UIMenu QuestionMenuWitness;
        private UIMenu QuestionMenuConsul;
        private UIMenu QuestionMenuSimeon;
        private UIMenu QuestionMenuGirlfriend;
        private UIMenu TruthMenu;
        private UIMenu AllClues;
        private UIMenu LocationSelect;
        public string CurrentLocation;
        private Blip HouseBlip;
        private Vector3 WrenchLocation;
        private bool TalkedToGFtwo;
        private Vector3 InteriorSpawnPoint;
        private bool NextAnswerStuff;
        private Vector3 ScientistALocation;
        private Ped ScientistB;
        private Vector3 ScientistBLocation;
        private List<Vector3> MarkerLocations;
        private bool LocationMenu;
        private List<float> MarkerHeading;
        private Ped Supervisor;
        private Tuple<Vector3, float> SupervisorLocation;
        private Tuple<Vector3, float> ShowcaseVehicleLocation;
        private Ped Witness;
        private Tuple<Vector3, float> WitnessLocation;
        private Rage.Object EviWrench;
        private Rage.Object EviBook;
        private Vector3 EviBookLocation;
        private Vector3 WrenchbenchLocation;
        private Rage.Object BenchAccident;
        private Tuple<Vector3, float> BenchLocation;
        private Tuple<Vector3, float> SimeonLocation;
        private Vector3 SimeonWalkToLocation;
        private Vector3 CarDealerEnterLocation;
        private bool OwnerOpens;
        private LHandle ChaseStuff;
        private Vector3 CarDealerExitLocation;
        private Ped Simeon;
        private bool WifeEntersHouse;
        private bool SpawnedPlates;
        private Vehicle ShowcaseCar;
        private bool InInterior;
        private bool CalloutRunning;
        private string NextAnswerString;
        private int NextAnswerInt;

        private TimerBarPool AwarenessBarPool = new TimerBarPool();
        private BarTimerBar AwarenessBar = new BarTimerBar("Suspicion");
        private float Awareness = 0f;
        private UIMenuItem MissingWheelEvi;
        private UIMenuItem WrenchEvi;
        private UIMenuItem RegSlipEvi;
        private UIMenuItem WitnessReportEvi;
        private UIMenuItem PossibleSuspectsEvi;
        private UIMenuItem PossibleSuspectVehicleEvi;
        private UIMenuItem VehicleStrippedofPartsEvi;
        private UIMenuItem WitnessReportClue;
        private UIMenuItem MissingWheelClue;
        private UIMenuItem WrenchClue;
        private UIMenuItem RegSlipClue;
        private UIMenuItem BookOfConsulClue;
        private UIMenuItem PurchaseHistoryClue;
        private UIMenuItem AlledgedBriberyClue;
        private UIMenuItem TheftConsuleVehicleClue;
        private UIMenuItem MotiveTheftClue;
        private UIMenuItem LastContactSuspectClue;
        private UIMenuItem LicensePlateRecoveredClue;
        private UIMenuItem BreakfastPlatesClue;
        private UIMenuItem AssociationMechanicClue;
        private UIMenuItem AssociationConsulClue;
        private UIMenuItem WhereaboutsMechanicClue;
        private UIMenuItem WrenchInTheftClue;

        private UIMenuItem Truth;
        private UIMenuItem Doubt;
        private UIMenuItem Lie;

        private UIMenuItem AccidentLocationMenu;
        private UIMenuItem PDLocationMenu;
        private UIMenuItem CardealerLocationMenu;
        private UIMenuItem SuspectHouseLocationMenu;
        private UIMenuItem RaceLocationMenu;

        private int DialogWithConsulIntroIndex;
        private List<string> DialogWithConsulIntro = new List<string>
        {
        };

        private int DialogWithSimeonIntroIndex;
        private List<string> DialogWithSimeonIntro = new List<string>
        {
            "~b~You:~s~ Police, we would to speak to the owner.",
            "~y~Simeon:~s~ That's me. Simeon Yetarian, at your service.",
            "~b~You:~s~ We are investigating the theft of a stolen vehicle belonging to the Argentine Embassy.",
            "~b~You:~s~ Are you missing a combination wrench?",
            "~y~Simeon:~s~ I don't know, Detective. But I know how we can find out. Follow me."
        };
        
            
        private int DialogWithGFTwoIndex;
        private List<string> DialogWithGFTwo = new List<string>
        {
            "~b~You:~s~ What is your name?",
            "~y~Girlfriend:~s~ -Placeholder-.",
            "~b~You:~s~ Is -Placeholder- here, Miss -Placeholder-?",
            "~y~Girlfriend:~s~ No. What do you want with -Placeholder-? Is he in trouble?",
            "~b~You:~s~ Stay were you are, Miss -Placeholder-. I need to take a look around.",
            "~y~Girlfriend:~s~ But he is not here! I have told you!",
        };
        private int DialogWithGFIntroIndex;
        private List<string> DialogWithGFIntro = new List<string>
        {
            "~y~Girlfriend:~s~ Yes?",
            "~b~You:~s~ We're from the police. We're looking for -Placeholder-.",
            "~y~Girlfriend:~s~ -Placeholder-? Could you come inside?",
        };

        private int DialogWithSimeonTwoIndex;
        private List<string> DialogWithSimeonTwo = new List<string>
        {
            "~b~You:~s~ If you don't mind I have a few questions."
        };

        private List<string> WitnessQuestions = new List<string> {};
        private List<string> ConsulQuestions = new List<string> { };
        private List<string> SimeonQuestions = new List<string> { };
        private List<string> GirlfriendQuestions = new List<string> { };
        private List<string> CollectedEvidences = new List<string> { };
        #endregion

        public override bool OnBeforeCalloutDisplayed()
        {
            StorageSpawn = new Vector3(-144.0361f, -592.9335f, 48.24776f);
            SpawnPoint1 = new Vector3(-1051.76001f, -1567.97119f, 3.80698323f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                PolicePedLocations = new List<Vector3>() { new Vector3(-1070.37891f, -1583.16687f, 4.40580463f), new Vector3(-1072.23828f, -1579.88f, 4.40888214f), new Vector3(-1041.01953f, -1558.17163f, 5.04900217f) };
                PolicePedHeadings = new List<float>() { 123.853058f, 123.853058f, -50.3201904f };
                SuspectVehicleLocation = new Vector3(-1056.74817f, -1571.18787f, 4.57142925f);
                SuspectVehicleHeading = -28.6174545f;
                MarkerLocations = new List<Vector3>() {new Vector3(-1051.65515f, -1566.12671f, 3.80536246f), new Vector3(-1055.75171f, -1568.93567f, 3.7087636f), new Vector3(-1058.078f, -1571.06714f, 3.62409329f), new Vector3(-1055.2417f, -1570.80261f, 3.71916556f) };
                MarkerHeading = new List<float>() { -54.6025314f, 140.742386f, -125.193428f, 98.0427399f };
                BarrierLocations = new List<Vector3>() {new Vector3(-1071.00122f, -1584.06165f, 3.39098549f), new Vector3(-1073.31006f, -1580.84375f, 3.40007114f), new Vector3(-1040.33691f, -1557.5531f, 4.09251404f) };
                BarrierHeadings = new List<float>() { 125.758736f, 125.758736f, 125.410439f };
                ScientistALocation = new Vector3(-1052.13525f, -1570.58691f, 4.82131624f);
                ScientistBLocation = new Vector3(-1063.13171f, -1571.33997f, 4.62907124f);
                WrenchLocation = new Vector3(-1051.83667f, -1566.10938f, 3.83178329f);
                AccidentLocation.Location = SpawnPoint;
                LocationClues.Add(AccidentLocation);
                GoToLocations.Add(AccidentLocation.LocationName);
                CurrentLocation = AccidentLocation.LocationName;
                InteriorSpawnPoint = new Vector3(264.9987f, -1000.504f, -99.00864f);
                BenchLocation = new Tuple<Vector3, float>(new Vector3(-1054.85327f, -1563.56018f, 3.83520937f), 35.9392433f);
                SupervisorLocation = new Tuple<Vector3, float>(new Vector3(-1046.67676f, -1566.59363f, 4.8481164f), -55.432579f);
                WitnessLocation = new Tuple<Vector3, float>(new Vector3(-1054.85876f, -1564.26758f, 4.82608747f), -166.087006f);
                EviBookLocation = new Vector3(-1054.40649f, -1563.2373f, 4.30676174f);
                ShowcaseVehicleLocation = new Tuple<Vector3, float>(new Vector3(-38.8529739f, -1096.87f, 26.0398827f), 146.032776f);
                SimeonLocation = new Tuple<Vector3, float>(new Vector3(-34.3104286f, -1102.20874f, 26.4262238f), 126.545677f);
                WrenchbenchLocation = new Vector3(-40.3732452f, -1088.27893f, 26.4260788f);
                SimeonWalkToLocation = new Vector3(-35.5251808f, -1087.73889f, 26.425581f);
                CarDealerEnterLocation = new Vector3(-39.1454926f, -1109.59998f, 25.439373f);
                CarDealerExitLocation = new Vector3(-37.4720001f, -1107.82886f, 25.4361725f);
                PlateLocations = new List<Vector3>() { new Vector3(-8.807716f, -1434.165f, 30.85301f), new Vector3(-8.833232f, -1432.443f, 30.85178f) };
                NumberPlateLocation = new Tuple<Vector3, Rotator>(new Vector3(-26.27295f, -1423.987f, 30.62933f), new Rotator(0.6901628f, 0f, 0.7236542f));
                WheelLocation = new Vector3(-23.66559f, -1425.293f, 29.92377f);
                HouseFrontDoorLocation = new Vector3(-14.19971f, -1441.728f, 31.20133f);
                HouseFrontDoorInsideLocation = new Vector3(-14.23746f, -1440.541f, 31.07373f);
                HouseWifeSpawn = new Vector3(-9.927795f, -1438.712f, 30.76434f);
                GarageDoor = new Tuple<uint, Vector3, float>(703855057, new Vector3(-25.2784f, -1431.061f, 30.83955f), -1f);
                FrontDoor = new Tuple<uint, Vector3, float>(520341586, new Vector3(-14.86892f, -1441.182f, 31.19323f), -1f);
            }

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            CalloutMessage = "Stolen Diplomatic Vehicle";
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
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ The caller found a stolen and dismantled diplomatic vehicle. Respond Code 2");
            Game.DisplayHelp("You can press~r~ " + Stuff.Settings.EndCalloutKey.ToString() + "~s~ anytime to end the callout.");
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
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
                    SpawnConsulPed();
                    GameFiber.Yield();
                    SpawnSimeonPed();
                    GameFiber.Yield();
                    SpawnWitnessAndSupervisor();
                    GameFiber.Yield();
                    SpawnSuspect();
                    GameFiber.Yield();
                    SpawnHousewife();
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    GameFiber.Yield();
                    InitMenu();
                    CollectedEvidences.Add("None");
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        MenuLogic();
                        #region CalloutStart/AccidentLocation
                        if (CurrentLocation == AccidentLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 100f)
                        {
                            if (!SuspectVehicle.Exists())
                            {
                                ClearUnrelatedEntities();
                                SpawnSuspectVehicle();
                            }
                            if (!EviWrench.Exists())
                            {
                                EviWrench = new Rage.Object("xs_prop_arena_torque_wrench_01a", Utils.ToGround(WrenchLocation));
                                EviWrench.IsPersistent = true;
                                EviWrench.MakeMission();
                                AllSpawnedObjects.Add(EviWrench);
                                AllBankHeistEntities.Add(EviWrench);
                            }
                            if (!EviBook.Exists())
                            {
                                EviBook = new Rage.Object("xm_prop_x17_book_bogdan", EviBookLocation);
                                EviBook.IsPersistent = true;
                                EviBook.MakeMission();
                                AllSpawnedObjects.Add(EviBook);
                                AllBankHeistEntities.Add(EviBook);
                            }
                        }
                        if (CurrentLocation == AccidentLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 50f)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 30f && SpawnBlip.Exists())
                            {
                                Game.DisplaySubtitle("~g~Officer:~s~ Hello Sir! The Supervisor is waiting for you!");
                                SpawnBlip.Delete();
                            }
                            if (SuspectVehicle.Exists())
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle.GetBonePosition("wheel_rf")) <= 1.2f && !MissingWheel.Finished)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the tire.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                        Game.DisplayNotification("~b~Clue added:~s~ " + MissingWheel.ExaminMessage);
                                        FoundClues.Add(MissingWheel);
                                        AllClues.AddItem(MissingWheelClue = new UIMenuItem(MissingWheel.Name, MissingWheel.ExaminMessage));
                                        AllClues.RefreshIndex();
                                        MissingWheel.Finished = true;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle.GetBonePosition("boot")) <= 1.5f && SearchTrunk == false)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the trunk.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Utils.VehicleDoorOpen(SuspectVehicle, Utils.VehDoorID.Trunk, false, false);
                                        Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                        Game.DisplayNotification("You found nothing");
                                        SearchTrunk = true;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(SuspectVehicle.GetBonePosition("door_dside_r")) <= 2f && RegSlip.Finished == false)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the car.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                        Game.DisplayNotification("~b~Clue added:~s~ " + RegSlip.ExaminMessage);
                                        FoundClues.Add(RegSlip);
                                        AllClues.AddItem(RegSlipClue = new UIMenuItem(RegSlip.Name, RegSlip.ExaminMessage));
                                        AllClues.RefreshIndex();
                                        RegSlip.Finished = true;
                                    }
                                }
                            }      
                            if (Game.LocalPlayer.Character.DistanceTo(EviWrench) <= 1.5f && !Wrench.Finished)
                            {
                                Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the wrench.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                    Game.DisplayNotification("~b~Clue added:~s~ " + Wrench.ExaminMessage);
                                    FoundClues.Add(Wrench);
                                    AllClues.AddItem(WrenchClue = new UIMenuItem(Wrench.Name, Wrench.ExaminMessage));
                                    AllClues.RefreshIndex();
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ I need an adress for a Yetarian car dealership.");
                                    Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 3000);
                                    GameFiber.Sleep(3000);
                                    Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to Unit:~s~", "The location has been added to your MDT.");
                                    Game.DisplayNotification("~b~New Location added:~s~ Yetarian Car Dealership");
                                    LocationClues.Add(CardealerLocation);
                                    GoToLocations.Add(CardealerLocation.LocationName);
                                    LocationSelect.AddItem(CardealerLocationMenu = new UIMenuItem(CardealerLocation.LocationName));
                                    LocationSelect.RefreshIndex();
                                    Wrench.Finished = true;
                                }
                            }
                            if (Witness.Exists())
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(Witness) <= 2f)
                                {
                                    if (WitnessIntroDialog <4)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the witness.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            switch(WitnessIntroDialog)
                                            {
                                                case 1:
                                                    Game.DisplaySubtitle("~b~You:~s~ Could you tell me what happened here?");
                                                    WitnessIntroDialog++;
                                                    break;
                                                case 2:
                                                    Game.DisplaySubtitle("~y~"+Functions.GetPersonaForPed(Witness).Forename+":~s~ I was looking out the window. I like to keep an eye out on my neighbourhood if you understand.");
                                                    WitnessIntroDialog++;
                                                    break;
                                                case 3:
                                                    Game.DisplaySubtitle("~y~" + Functions.GetPersonaForPed(Witness).Forename + ":~s~ Anyway, I see this vehicle here in the alleyway. Brandnew. But with the tire and door removed.");
                                                    FoundClues.Add(WitnessReport);
                                                    WitnessReport.Name = Functions.GetPersonaForPed(Witness).FullName + " Report";
                                                    WitnessReport.ExaminMessage = Functions.GetPersonaForPed(Witness).FullName + " Report at the accident location.";
                                                    WitnessQuestions.Add(WitnessReport.Name);
                                                    QuestionMenuWitness.AddItem(WitnessReportEvi = new UIMenuItem(WitnessReport.Name, WitnessReport.ExaminMessage));
                                                    QuestionMenuWitness.RefreshIndex();
                                                    AllClues.AddItem(WitnessReportClue = new UIMenuItem(WitnessReport.Name, WitnessReport.ExaminMessage));
                                                    AllClues.RefreshIndex();
                                                    WitnessIntroDialog++;
                                                    break;
                                            }
                                        }
                                    }
                                    if (WitnessQuestions.Count != 0 && WitnessIntroDialog >=4)
                                    {
                                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to talk to the witness.");
                                        if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == false && NextAnswerStuff2 == false)
                                        {
                                            QuestionMenuWitness.Visible = true;
                                        }
                                        if (QuestionMenuWitness.Visible)
                                        {
                                            int QuestionInt = QuestionMenuWitness.CurrentSelection;
                                            if (Game.IsKeyDown(Keys.Enter))
                                            {
                                                Clue Clu = FoundClues.Find(x => x.Name == QuestionMenuWitness.MenuItems[QuestionInt].Text);
                                                NextAnswerString = QuestionMenuWitness.MenuItems[QuestionInt].Text;
                                                NextAnswerInt = QuestionMenuWitness.CurrentSelection;
                                                NextAnswerStuff = true;
                                                if (Clu.AnswerIndex < Clu.Answer.Count)
                                                {
                                                    Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                    Clu.AnswerIndex++;
                                                }
                                                QuestionMenuWitness.Close();
                                            }
                                        }
                                        if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == true && NextAnswerStuff2 == false && !TruthMenu.Visible)
                                        {
                                            Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                            if (Clu.AnswerIndex < Clu.Answer.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                Clu.AnswerIndex++;
                                            }
                                            if (Clu.AnswerIndex == Clu.Answer.Count)
                                            {
                                                switch(Clu.HonestyWitness)
                                                {
                                                    case "Truth":
                                                        Witness.TruthSpeech();
                                                        break;
                                                    case "Doubt":
                                                        Witness.DoubtSpeech();
                                                        break;
                                                    case "Lie":
                                                        Witness.LieSpeech();
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                TruthMenu.Visible = true;
                                            }
                                        }
                                        if (TruthMenu.Visible)
                                        {
                                            int TruthChoice = TruthMenu.CurrentSelection;
                                            Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                            if (Game.IsKeyDown(Keys.Enter))
                                            {
                                                if (Clu.HonestyWitness == "Truth")
                                                {
                                                    if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                    {
                                                        Witness.Tasks.Clear();
                                                        NextAnswerStuff2 = true;
                                                        if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                        {
                                                            Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                            Clu.HonestyAnswerIndex++;
                                                        }
                                                        TruthMenu.Close();
                                                    }
                                                    else
                                                    {
                                                        Witness.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WitnessQuestions.Remove(NextAnswerString);
                                                        QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                        QuestionMenuWitness.RefreshIndex();
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                        TruthMenu.Close();
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Doubt")
                                                {
                                                    if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                    {
                                                        Witness.Tasks.Clear();
                                                        if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                        {
                                                            Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                            Clu.HonestyAnswerIndex++;
                                                        }
                                                        NextAnswerStuff2 = true;
                                                        TruthMenu.Close();
                                                    }
                                                    else
                                                    {
                                                        Witness.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WitnessQuestions.Remove(NextAnswerString);
                                                        QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                        QuestionMenuWitness.RefreshIndex();
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                        TruthMenu.Close();
                                                    }
                                                }
                                                else if (Clu.HonestyWitness == "Lie")
                                                {
                                                    if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                    {
                                                        NextAnswerStuff = false;
                                                        AllClues.Visible = true;
                                                        TruthMenu.Close();
                                                    }
                                                    else
                                                    {
                                                        Witness.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        WitnessQuestions.Remove(NextAnswerString);
                                                        QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                        QuestionMenuWitness.RefreshIndex();
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                        TruthMenu.Close();
                                                    }
                                                }
                                            }
                                        }
                                        if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff2 == true && !TruthMenu.Visible)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                Clu.HonestyAnswerIndex++;
                                            }
                                            if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                            {
                                                WitnessQuestions.Remove(NextAnswerString);
                                                QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                QuestionMenuWitness.RefreshIndex();
                                                Clu.AddClue = true;
                                                NextAnswerStuff2 = false;
                                            }
                                        }
                                        if (AllClues.Visible)
                                        {
                                            int TruthChoice = TruthMenu.CurrentSelection;
                                            Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                            if (Game.IsKeyDown(Keys.Enter))
                                            {
                                                if (Clu.WitnessLieStuff.Contains(AllClues.MenuItems[TruthChoice].Text))
                                                {
                                                    Witness.Tasks.Clear();
                                                    NextAnswerStuff3 = true;
                                                    if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                    {
                                                        Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                        Clu.HonestyAnswerIndex++;
                                                    }
                                                    AllClues.Close();
                                                }
                                                else
                                                {
                                                    Witness.Tasks.Clear();
                                                    Game.DisplaySubtitle("False");
                                                    WitnessQuestions.Remove(NextAnswerString);
                                                    QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                    QuestionMenuWitness.RefreshIndex();
                                                    Clu.WrongAnswer = true;
                                                    NextAnswerStuff = false;
                                                    AllClues.Close();
                                                }
                                            }
                                        }
                                        if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff3 == true && !AllClues.Visible)
                                        {
                                            NextAnswerStuff = false;
                                            Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                            {
                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                Clu.HonestyAnswerIndex++;
                                            }
                                            if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                            {
                                                WitnessQuestions.Remove(NextAnswerString);
                                                QuestionMenuWitness.RemoveItemAt(NextAnswerInt);
                                                QuestionMenuWitness.RefreshIndex();
                                                Clu.AddClue = true;
                                                NextAnswerStuff3 = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (Supervisor.Exists())
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(Supervisor) <= 2f && SupervisorDialog <5)
                                {
                                    Supervisor.Face(Game.LocalPlayer.Character);
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the supervisor.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        switch(SupervisorDialog)
                                        {
                                            case 1:
                                                Supervisor.Tasks.Clear();
                                                Game.DisplaySubtitle("~g~Supervisor:~s~ Hello Officer! The cars right down the alley.");
                                                SupervisorDialog++;
                                                break;
                                            case 2:
                                                Game.DisplaySubtitle("~b~You:~s~ Thanks. Has anyone touched the car yet?");
                                                SupervisorDialog++;
                                                break;
                                            case 3:
                                                if(Witness.Exists())
                                                {
                                                    Game.DisplaySubtitle("~g~Supervisor:~s~ No. But that witness over there, " + Functions.GetPersonaForPed(Witness).Forename + " was here first before we arrived.");
                                                }
                                                else
                                                {
                                                    Game.DisplaySubtitle("~g~Supervisor:~s~ No.");
                                                }
                                                SupervisorDialog++;
                                                break;
                                            case 4:
                                                Game.DisplaySubtitle("~b~You:~s~ We'll talk with him in a moment. Give me some time to look the place over.");
                                                SupervisorDialog++;
                                                break;
                                        }
                                    }
                                }
                            }
                            #region AddClues and Wrongs
                            if (!WitnessReport.Finished)
                            {
                                if (WitnessReport.AddClue || WitnessReport.WrongAnswer)
                                {
                                    WitnessQuestions.Add(WitnessPossibleSuspects.Name);
                                    QuestionMenuWitness.AddItem(PossibleSuspectsEvi = new UIMenuItem(WitnessPossibleSuspects.Name, WitnessPossibleSuspects.ExaminMessage));
                                    QuestionMenuWitness.RefreshIndex();
                                    WitnessQuestions.Add(WitnessPossibleSuspectCar.Name);
                                    QuestionMenuWitness.AddItem(PossibleSuspectVehicleEvi = new UIMenuItem(WitnessPossibleSuspectCar.Name, WitnessPossibleSuspectCar.ExaminMessage));
                                    QuestionMenuWitness.RefreshIndex();
                                    WitnessQuestions.Add(WitnessVehStrippedofParts.Name);
                                    QuestionMenuWitness.AddItem(VehicleStrippedofPartsEvi = new UIMenuItem(WitnessVehStrippedofParts.Name, WitnessVehStrippedofParts.ExaminMessage));
                                    QuestionMenuWitness.RefreshIndex();
                                    FoundClues.Add(WitnessPossibleSuspects);
                                    FoundClues.Add(WitnessPossibleSuspectCar);
                                    FoundClues.Add(WitnessVehStrippedofParts);
                                    WitnessReport.Finished = true;
                                }
                            }
                            if (!WitnessVehStrippedofParts.Finished)
                            {
                                if (WitnessVehStrippedofParts.AddClue || WitnessVehStrippedofParts.WrongAnswer)
                                {
                                            EviBook.Detach();
                                            EviBook.Position = EviBookLocation;
                                            WitnessVehStrippedofParts.Finished = true;
                                }
                                if (Witness.Exists())
                                {
                                    if(Witness.IsCuffed || Witness.IsDead)
                                    {
                                        EviBook.Detach();
                                        EviBook.Position = EviBookLocation;
                                        Witness.Dismiss();
                                        WitnessVehStrippedofParts.Finished = true;
                                    }
                                }
                            }
                            if (WitnessVehStrippedofParts.Finished && !BookOfConsul.Finished)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(EviBook) <= 1.3f)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the book.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                        BookOfConsul.ExaminMessage = "Book belonging to someone named " + Functions.GetPersonaForPed(ConsulPed).FullName+". Contains sexual descriptions of boys including phonenumbers, aswell as a business contact on Mr. Yeterian";
                                        Game.DisplayNotification("~b~Clue added:~s~ " + BookOfConsul.ExaminMessage);
                                        FoundClues.Add(BookOfConsul);
                                        AllClues.AddItem(BookOfConsulClue = new UIMenuItem(BookOfConsul.Name, BookOfConsul.ExaminMessage));
                                        AllClues.RefreshIndex();
                                        Game.DisplayNotification("~b~Attention to Dispatch:~s~ Can you bring " + Functions.GetPersonaForPed(ConsulPed).FullName + " in for questioning?");
                                        Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 3000);
                                        GameFiber.Sleep(3000);
                                        Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to Unit:~s~", Functions.GetPersonaForPed(ConsulPed).FullName + " is already on station and is awaiting the interview.");
                                        Game.DisplayNotification("~b~New Location added:~s~ Police Department");
                                        LocationClues.Add(PDLocation);
                                        GoToLocations.Add(PDLocation.LocationName);
                                        LocationSelect.AddItem(PDLocationMenu = new UIMenuItem(PDLocation.LocationName));
                                        LocationSelect.RefreshIndex();
                                        BookOfConsul.Finished = true;
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #region PD Location
                        if (CurrentLocation == PDLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(PDLocation.Location) <= 100f)
                        {
                            if (ConsulPed.Exists() && !SetConsulPos)
                            {
                                ConsulPed.Position = new Vector3(446.443146f, -976.532227f, 30.6899891f);
                                ConsulPed.Tasks.PlayAnimation("friends@", "pickupwait", 1f, AnimationFlags.Loop);
                                DialogWithConsulIntro = new List<string>
                                {
                                    "~y~Consul:~s~ About time. Are you the officer I requested?",
                                    "~b~You:~s~ I'm Officer "+Functions.GetPersonaForPed(Game.LocalPlayer.Character).Surname+".",
                                    "~y~Consul:~s~ Have you any idea how long I have been waiting to speak with you?",
                                    "~y~Consul:~s~ I am needed back at the Consulate and you keep me here like a common criminal.",
                                    "~b~You:~s~ Mr. "+Functions.GetPersonaForPed(ConsulPed).Surname +"...",
                                    "~y~Consul:~s~ Consul General. I insist on my full title."
                                };
                                SetConsulPos = true;
                            }
                        }
                        if (CurrentLocation == PDLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(PDLocation.Location) <= 50f)
                        {
                            if (ConsulPed.Exists())
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(ConsulPed) <= 2f)
                                {
                                    if (DialogWithConsulIntroIndex < DialogWithConsulIntro.Count)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the consul.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            ConsulPed.Tasks.Clear();
                                            ConsulPed.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                            Game.DisplaySubtitle(DialogWithConsulIntro[DialogWithConsulIntroIndex]);
                                            DialogWithConsulIntroIndex++;
                                        }
                                    }
                                    if (DialogWithConsulIntroIndex == DialogWithConsulIntro.Count)
                                    {
                                        if (!PurchaseHistory.Finished)
                                        {
                                            FoundClues.Add(PurchaseHistory);
                                            PurchaseHistory.Answer = new List<string>
                                            {
                                                "~b~You:~s~ Where did you purchase the car?",
                                                "~y~Consul:~s~ My secretary and driver arranged the purchase.",
                                                "~y~Consul:~s~ A disreputable place - Yeterian by name.",
                                                "~y~Consul:~s~ As soon as I can have it arranged I will have my Inferno brought up from Buenos Aires."
                                            };
                                            PurchaseHistory.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ You bought that car, "+Functions.GetPersonaForPed(ConsulPed).Surname+"? A snob like you doesn't drive that kind of car.",
                                                "~b~You:~s~ I want answers right now or you will spend a long time here.",
                                                "~y~Consul:~s~ The owner offered me a substantial bribe to make a purchase at his establishment.",
                                                "~y~Consul:~s~ It is not unusal to make this kind of transaction in the civil service."
                                            };
                                            AllClues.AddItem(PurchaseHistoryClue = new UIMenuItem(PurchaseHistory.Name, PurchaseHistory.ExaminMessage));
                                            AllClues.RefreshIndex();
                                            ConsulQuestions.Add(PurchaseHistory.Name);
                                            QuestionMenuConsul.AddItem(PurchaseHistoryClue = new UIMenuItem(PurchaseHistory.Name, PurchaseHistory.ExaminMessage));
                                            QuestionMenuConsul.RefreshIndex();
                                            PurchaseHistory.Finished = true;
                                        }
                                        if (!TheftConsuleVehicle.Finished)
                                        {
                                            FoundClues.Add(TheftConsuleVehicle);
                                            TheftConsuleVehicle.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ You have a pretty good idea who stole the car, don't you, Consul General?",
                                                "~b~You:~s~ Are you gonna tell me or do I shake it out of you?",
                                                "~y~Consul:~s~ I suspect a disgruntled boy from the car dealership.",
                                                "~b~You:~s~ You have a name for this kid?",
                                                "~y~Consul:~s~ "+Functions.GetPersonaForPed(Suspect).Forename+". I have no surname."
                                            };
                                            AllClues.AddItem(TheftConsuleVehicleClue = new UIMenuItem(TheftConsuleVehicle.Name, TheftConsuleVehicle.ExaminMessage));
                                            AllClues.RefreshIndex();
                                            ConsulQuestions.Add(TheftConsuleVehicle.Name);
                                            QuestionMenuConsul.AddItem(TheftConsuleVehicleClue = new UIMenuItem(TheftConsuleVehicle.Name, TheftConsuleVehicle.ExaminMessage));
                                            QuestionMenuConsul.RefreshIndex();
                                            TheftConsuleVehicle.Finished = true;
                                        }
                                        if (TheftConsuleVehicle.AddClue && !AssociationMechanic.Finished)
                                        {
                                            FoundClues.Add(AssociationMechanic);
                                            AssociationMechanic.Answer = new List<string>
                                            {
                                                "~b~You:~s~ So tell us about the kid. You had a run in with him?",
                                                "~y~Consul:~s~ Mechanic. A presumptous young man who did not know his place. He presumed to ask me questions."
                                            };
                                            AssociationMechanic.PreLieAnswers = new List<string>
                                            {
                                                "~b~You:~s~ You fuck young boys, "+Functions.GetPersonaForPed(ConsulPed).Surname+"?",
                                                "~y~Consul:~s~ Are you a madman? This will cause an international incident!",
                                            };
                                            AssociationMechanic.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ We found your book. With some ... interesting descriptions. Ring any bells?",
                                                "~y~Consul:~s~ I'm sure we can come to some arrangement?",
                                                "~b~You:~s~ "+Functions.GetPersonaForPed(Suspect).Forename+", spill it.",
                                                "~y~Consul:~s~ A beautiful but impertinent boy.",
                                                "~y~Consul:~s~ I mentioned a rendezvous and the young man went quite insane. I thought he was going to kill me.",
                                                "~y~Consul:~s~ I was prepared to pay."
                                            };
                                            /*AllClues.AddItem(AssociationMechanicClue = new UIMenuItem(AssociationMechanic.Name, AssociationMechanic.ExaminMessage));
                                            AllClues.RefreshIndex();*/
                                            ConsulQuestions.Add(AssociationMechanic.Name);
                                            QuestionMenuConsul.AddItem(AssociationMechanicClue = new UIMenuItem(AssociationMechanic.Name, AssociationMechanic.ExaminMessage));
                                            QuestionMenuConsul.RefreshIndex();
                                            AssociationMechanic.Finished = true;
                                        }
                                        if (PurchaseHistory.AddClue && !AlledgedBribery.Finished)
                                        {
                                            FoundClues.Add(AlledgedBribery);
                                            AllClues.AddItem(AlledgedBriberyClue = new UIMenuItem(AlledgedBribery.Name, AlledgedBribery.ExaminMessage));
                                            AllClues.RefreshIndex();
                                            Game.DisplayNotification("~b~Clue added:~s~ " + AlledgedBribery.ExaminMessage);
                                            AlledgedBribery.Finished = true;
                                        }
                                        if (ConsulQuestions.Count != 0)
                                        {
                                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the consul.");
                                            ShowLANoireMenu(QuestionMenuConsul, ConsulPed, ConsulQuestions);
                                            /*if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == false && NextAnswerStuff2 == false)
                                            {
                                                QuestionMenuConsul.Visible = true;
                                            }
                                            if (QuestionMenuConsul.Visible)
                                            {
                                                int QuestionInt = QuestionMenuConsul.CurrentSelection;
                                                if (Game.IsKeyDown(Keys.Enter))
                                                {
                                                    Clue Clu = FoundClues.Find(x => x.Name == QuestionMenuConsul.MenuItems[QuestionInt].Text);
                                                    NextAnswerString = QuestionMenuConsul.MenuItems[QuestionInt].Text;
                                                    NextAnswerInt = QuestionMenuConsul.CurrentSelection;
                                                    NextAnswerStuff = true;
                                                    if (Clu.AnswerIndex < Clu.Answer.Count)
                                                    {
                                                        Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                        Clu.AnswerIndex++;
                                                    }
                                                    QuestionMenuConsul.Close();
                                                }
                                            }
                                            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == true && NextAnswerStuff2 == false && !TruthMenu.Visible)
                                            {
                                                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                                if (Clu.AnswerIndex < Clu.Answer.Count)
                                                {
                                                    Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                                                    Clu.AnswerIndex++;
                                                }
                                                if (Clu.AnswerIndex == Clu.Answer.Count)
                                                {
                                                    switch (Clu.HonestyWitness)
                                                    {
                                                        case "Truth":
                                                            ConsulPed.TruthSpeech();
                                                            break;
                                                        case "Doubt":
                                                            ConsulPed.DoubtSpeech();
                                                            break;
                                                        case "Lie":
                                                            ConsulPed.LieSpeech();
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    TruthMenu.Visible = true;
                                                }
                                            }
                                            if (TruthMenu.Visible)
                                            {
                                                int TruthChoice = TruthMenu.CurrentSelection;
                                                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                                if (Game.IsKeyDown(Keys.Enter))
                                                {
                                                    if (Clu.HonestyWitness == "Truth")
                                                    {
                                                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                        {
                                                            ConsulPed.Tasks.Clear();
                                                            NextAnswerStuff2 = true;
                                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                            {
                                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                                Clu.HonestyAnswerIndex++;
                                                            }
                                                            TruthMenu.Close();
                                                        }
                                                        else
                                                        {
                                                            ConsulPed.Tasks.Clear();
                                                            Game.DisplaySubtitle("False");
                                                            ConsulQuestions.Remove(NextAnswerString);
                                                            QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                            QuestionMenuConsul.RefreshIndex();
                                                            Clu.WrongAnswer = true;
                                                            NextAnswerStuff = false;
                                                            TruthMenu.Close();
                                                        }
                                                    }
                                                    else if (Clu.HonestyWitness == "Doubt")
                                                    {
                                                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                        {
                                                            ConsulPed.Tasks.Clear();
                                                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                            {
                                                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                                Clu.HonestyAnswerIndex++;
                                                            }
                                                            NextAnswerStuff2 = true;
                                                            TruthMenu.Close();
                                                        }
                                                        else
                                                        {
                                                            ConsulPed.Tasks.Clear();
                                                            Game.DisplaySubtitle("False");
                                                            ConsulQuestions.Remove(NextAnswerString);
                                                            QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                            QuestionMenuConsul.RefreshIndex();
                                                            Clu.WrongAnswer = true;
                                                            NextAnswerStuff = false;
                                                            TruthMenu.Close();
                                                        }
                                                    }
                                                    else if (Clu.HonestyWitness == "Lie")
                                                    {
                                                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                                                        {
                                                            NextAnswerStuff = false;
                                                            AllClues.Visible = true;
                                                            TruthMenu.Close();
                                                        }
                                                        else
                                                        {
                                                            ConsulPed.Tasks.Clear();
                                                            Game.DisplaySubtitle("False");
                                                            ConsulQuestions.Remove(NextAnswerString);
                                                            QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                            QuestionMenuConsul.RefreshIndex();
                                                            Clu.WrongAnswer = true;
                                                            NextAnswerStuff = false;
                                                            TruthMenu.Close();
                                                        }
                                                    }
                                                }
                                            }
                                            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff2 == true && !TruthMenu.Visible)
                                            {
                                                NextAnswerStuff = false;
                                                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                                if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                {
                                                    Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                    Clu.HonestyAnswerIndex++;
                                                }
                                                if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                                {
                                                    ConsulQuestions.Remove(NextAnswerString);
                                                    QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                    QuestionMenuConsul.RefreshIndex();
                                                    Clu.AddClue = true;
                                                    NextAnswerStuff2 = false;
                                                }
                                            }
                                            if (AllClues.Visible)
                                            {
                                                int TruthChoice = TruthMenu.CurrentSelection;
                                                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                                if (Game.IsKeyDown(Keys.Enter))
                                                {
                                                    if (Clu.WitnessLieStuff.Contains(AllClues.MenuItems[TruthChoice].Text))
                                                    {
                                                        ConsulPed.Tasks.Clear();
                                                        NextAnswerStuff3 = true;
                                                        if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                        {
                                                            Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                            Clu.HonestyAnswerIndex++;
                                                        }
                                                        AllClues.Close();
                                                    }
                                                    else
                                                    {
                                                        ConsulPed.Tasks.Clear();
                                                        Game.DisplaySubtitle("False");
                                                        ConsulQuestions.Remove(NextAnswerString);
                                                        QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                        QuestionMenuConsul.RefreshIndex();
                                                        Clu.WrongAnswer = true;
                                                        NextAnswerStuff = false;
                                                        AllClues.Close();
                                                    }
                                                }
                                            }
                                            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff3 == true && !AllClues.Visible)
                                            {
                                                NextAnswerStuff = false;
                                                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                                                if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                                                {
                                                    Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                                    Clu.HonestyAnswerIndex++;
                                                }
                                                if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                                                {
                                                    ConsulQuestions.Remove(NextAnswerString);
                                                    QuestionMenuConsul.RemoveItemAt(NextAnswerInt);
                                                    QuestionMenuConsul.RefreshIndex();
                                                    Clu.AddClue = true;
                                                    NextAnswerStuff3 = false;
                                                }
                                            }*/
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region CarDealer
                        if (CurrentLocation == CardealerLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(CardealerLocation.Location) <= 100f)
                        {
                            if (Simeon.Exists() && !SimeonLocSet)
                            {
                                Simeon.Position = new Vector3(-34.3104286f, -1102.20874f, 26.4262238f);
                                Simeon.Tasks.PlayAnimation("friends@", "pickupwait", 1f, AnimationFlags.Loop);
                                SimeonLocSet = true;
                            }
                            if (!ShowcaseCar.Exists())
                            {
                                SpawnShowcaseCarVehicle();
                            }
                        }
                        if (CurrentLocation == CardealerLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(CardealerLocation.Location) <= 50f)
                        {
                            if (!InInterior)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(CarDealerEnterLocation) <=2f)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + " ~s~to enter the dealership.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.LocalPlayer.Character.Position = CarDealerExitLocation;
                                        GameFiber.Sleep(300);
                                        InInterior = true;
                                    }
                                }
                            }
                            else
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(CarDealerExitLocation) <= 2f)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + " ~s~to exit the dealership.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.LocalPlayer.Character.Position = CarDealerEnterLocation;
                                        GameFiber.Sleep(300);
                                        InInterior = false;
                                    }
                                }
                            }
                            if (Simeon.Exists())
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(Simeon) <= 2f && !SimeonIntro)
                                {
                                    if (DialogWithSimeonIntroIndex < DialogWithSimeonIntro.Count)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the owner.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Simeon.Tasks.Clear();
                                            Simeon.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                            Game.DisplaySubtitle(DialogWithSimeonIntro[DialogWithSimeonIntroIndex]);
                                            DialogWithSimeonIntroIndex++;
                                        }
                                    }
                                    if (DialogWithSimeonIntroIndex == DialogWithSimeonIntro.Count)
                                    {
                                        WhereaboutsMechanic.WitnessHonestyAnswers = new List<string>
                                        {
                                            "~b~You:~s~ Address, Simeon. Or we'll take a closer look at your business.",
                                            "~y~Simeon:~s~ Okay, alright!",
                                            "~y~Simeon:~s~ "+World.GetStreetName(HouseFrontDoorLocation)
                                        };
                                        WhereaboutsMechanic.Answer = new List<string>
                                        {
                                             "~b~You:~s~ Where can we find "+Functions.GetPersonaForPed(Suspect).FullName+"?",
                                             "~y~Simeon:~s~ I don't know. He sure as hell isn't here.",
                                        };
                                        Simeon.Tasks.FollowNavigationMeshToPosition(SimeonWalkToLocation, 0f, 1.3f).WaitForCompletion();
                                        Game.DisplaySubtitle("~y~Simeon:~s~ You can check the Equipment on the wall. I'm in front if you need me again.");
                                        Simeon.Tasks.FollowNavigationMeshToPosition(SimeonLocation.Item1, SimeonLocation.Item2, 1.3f);
                                        SimeonIntro = true;
                                    }
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(Simeon) <= 2f && MissingTools.Finished)
                                {
                                    if (DialogWithSimeonTwoIndex < DialogWithSimeonTwo.Count)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the owner.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Simeon.Tasks.Clear();
                                            Simeon.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                            Game.DisplaySubtitle(DialogWithSimeonTwo[DialogWithSimeonTwoIndex]);
                                            DialogWithSimeonTwoIndex++;
                                        }
                                    }
                                    if (DialogWithSimeonTwoIndex == DialogWithSimeonTwo.Count)
                                    {
                                        if (SimeonQuestions.Count != 0)
                                        {
                                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the owner.");
                                            ShowLANoireMenu(QuestionMenuSimeon, Simeon, SimeonQuestions);
                                        }
                                    }
                                }
                                if (WhereaboutsMechanic.WrongAnswer && !WhereaboutsMechanic.Finished && !SuspectHouseLocation.Finished)
                                {
                                    Game.DisplayNotification("~b~Attention to Dispatch:~s~ I need an adress for "+Functions.GetPersonaForPed(Suspect).FullName);
                                    Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 3000);
                                    GameFiber.Sleep(3000);
                                    Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to Unit:~s~", "The location has been added to your MDT.");
                                    Game.DisplayNotification("~b~New Location added:~s~ Suspects Home");
                                    SuspectHouseLocation.LocationName = Functions.GetPersonaForPed(Suspect).FullName + " House Location";
                                    LocationClues.Add(SuspectHouseLocation);
                                    GoToLocations.Add(SuspectHouseLocation.LocationName);
                                    LocationSelect.AddItem(SuspectHouseLocationMenu = new UIMenuItem(SuspectHouseLocation.LocationName));
                                    LocationSelect.RefreshIndex();
                                    SuspectHouseLocation.Finished = true;
                                }
                                if (WhereaboutsMechanic.AddClue && !SuspectHouseLocation.Finished)
                                {
                                    Game.DisplayNotification("~b~New Location added:~s~ Suspects Home");
                                    SuspectHouseLocation.LocationName = Functions.GetPersonaForPed(Suspect).FullName + " House Location";
                                    LocationClues.Add(SuspectHouseLocation);
                                    GoToLocations.Add(SuspectHouseLocation.LocationName);
                                    LocationSelect.AddItem(SuspectHouseLocationMenu = new UIMenuItem(SuspectHouseLocation.LocationName));
                                    LocationSelect.RefreshIndex();
                                    SuspectHouseLocation.Finished = true;
                                }
                            }
                            if (SimeonIntro)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(WrenchbenchLocation) <= 2f && !MissingTools.Finished)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to search the tools.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                        Game.DisplaySubtitle("~b~You:~s~ " + Functions.GetPersonaForPed(Suspect).FullName + " is missing a three quarter.");
                                        FoundClues.Add(WhereaboutsMechanic);
                                        AllClues.AddItem(WhereaboutsMechanicClue = new UIMenuItem(WhereaboutsMechanic.Name, WhereaboutsMechanic.ExaminMessage));
                                        AllClues.RefreshIndex();
                                        SimeonQuestions.Add(WhereaboutsMechanic.Name);
                                        QuestionMenuSimeon.AddItem(WhereaboutsMechanicClue = new UIMenuItem(WhereaboutsMechanic.Name, WhereaboutsMechanic.ExaminMessage));
                                        QuestionMenuSimeon.RefreshIndex();
                                        FoundClues.Add(WrenchInTheft);
                                        SimeonQuestions.Add(WrenchInTheft.Name);
                                        QuestionMenuSimeon.AddItem(WrenchInTheftClue = new UIMenuItem(WrenchInTheft.Name, WrenchInTheft.ExaminMessage));
                                        QuestionMenuSimeon.RefreshIndex();
                                        MissingTools.Finished = true;
                                    }
                                }
                                if (!AssociationConsul.Finished)
                                {
                                    FoundClues.Add(AssociationConsul);
                                    AssociationConsul.Answer = new List<string>
                                    {
                                         "~b~You:~s~ This doesn't look like the kind of place to be favored by foreign embassies.",
                                         "~b~You:~s~ How do you know "+Functions.GetPersonaForPed(ConsulPed).Surname+"?",
                                         "~y~Simeon:~s~ I don't know "+Functions.GetPersonaForPed(ConsulPed).Surname+". The Embassy bought the car.",
                                         "~y~Simeon:~s~ All I know is, he must know a quality car when he sees one."
                                    };
                                    AssociationConsul.PreLieAnswers = new List<string>
                                    {
                                        "~b~You:~s~ And I know a shyster when I see one. You and "+Functions.GetPersonaForPed(ConsulPed).Surname+" are in this together.",
                                        "~y~Simeon:~s~ Me and "+Functions.GetPersonaForPed(ConsulPed).Surname+"? I hardly know him. He is a racist and I don't like him."
                                    };
                                    AssociationConsul.WitnessHonestyAnswers = new List<string>
                                    {
                                        "~b~You:~s~ We found your contact details in "+Functions.GetPersonaForPed(ConsulPed).Surname+"'s notebook. He had to be calling you for something, Yetarian.",
                                        "~y~Simeon:~s~ Okay. So I met him in a bar.",
                                        "~y~Simeon:~s~ We cut a deal and he bought the car through the Embassy.",
                                        "~y~Simeon:~s~ I cut him some change on the side. It happens all the time.",
                                    };
                                    AllClues.AddItem(AssociationConsulClue = new UIMenuItem(AssociationConsul.Name, AssociationConsul.ExaminMessage));
                                    AllClues.RefreshIndex();
                                    SimeonQuestions.Add(AssociationConsul.Name);
                                    QuestionMenuSimeon.AddItem(AssociationConsulClue = new UIMenuItem(AssociationConsul.Name, AssociationConsul.ExaminMessage));
                                    QuestionMenuSimeon.RefreshIndex();
                                    AssociationConsul.Finished = true;
                                }
                            }
                        }
                        #endregion
                        #region SuspectHouse
                        if (CurrentLocation == SuspectHouseLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SuspectHouseLocation.Location) <= 100f)
                        {
                            if(Wife.Exists() && !WifeSetPos)
                            {
                                Wife.Position = HouseWifeSpawn;
                                WifeSetPos = true;
                            }
                            if (!SpawnedPlates)
                            {
                                SpawnAllPlates();
                                SpawnedPlates = true;
                            }
                        }
                        if (CurrentLocation == SuspectHouseLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(SuspectHouseLocation.Location) <= 50f)
                        {
                            if (Wife.Exists())
                            {
                                if (!WifeEntersHouse)
                                {
                                    if (Game.LocalPlayer.Character.Position.DistanceTo(HouseFrontDoorLocation) <= 2f && OwnerOpens == false)
                                    {
                                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to ring the bell.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Game.DisplayNotification("You rang the door!");
                                            Game.DisplaySubtitle("~y~Girlfriend:~s~ Just a minute!");
                                            DialogWithGFIntro = new List<string>
                                            {
                                                "~y~Girlfriend:~s~ Yes?",
                                                "~b~You:~s~ We're from the police. We're looking for "+Functions.GetPersonaForPed(Suspect).FullName+".",
                                                "~y~Girlfriend:~s~ "+Functions.GetPersonaForPed(Suspect).Forename+"? Could you come inside?",
                                            };
                                            DialogWithGFTwo = new List<string>
                                            {
                                                "~b~You:~s~ What is your name?",
                                                "~y~Girlfriend:~s~ "+Functions.GetPersonaForPed(Wife).FullName+".",
                                                "~b~You:~s~ Is "+Functions.GetPersonaForPed(Suspect).FullName+" here, Miss "+Functions.GetPersonaForPed(Wife).Surname+"?",
                                                "~y~Girlfriend:~s~ No. What do you want with "+Functions.GetPersonaForPed(Suspect).Forename+"? Is he in trouble?",
                                                "~b~You:~s~ Stay were you are, Miss "+Functions.GetPersonaForPed(Wife).Surname+". I need to take a look around.",
                                                "~y~Girlfriend:~s~ But he is not here! I have told you!",
                                            };
                                            Wife.Tasks.FollowNavigationMeshToPosition(HouseFrontDoorInsideLocation, Game.LocalPlayer.Character.Position.X, 1.3f).WaitForCompletion();
                                            Wife.TurnToFaceEntity(Game.LocalPlayer.Character, 1300);
                                            Utils.OpenDoor(FrontDoor.Item1, FrontDoor.Item2, true, FrontDoor.Item3);
                                            Utils.OpenDoor(GarageDoor.Item1, GarageDoor.Item2, true, GarageDoor.Item3);
                                            Wife.Tasks.FollowNavigationMeshToPosition(HouseFrontDoorLocation, Game.LocalPlayer.Character.Position.X, 1.3f).WaitForCompletion();
                                            Wife.TurnToFaceEntity(Game.LocalPlayer.Character, 1300);
                                            OwnerOpens = true;
                                        }
                                    }
                                    if (Game.LocalPlayer.Character.DistanceTo(Wife) <= 2f && OwnerOpens)
                                    {
                                        Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the girlfriend.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            if (DialogWithGFIntroIndex < DialogWithGFIntro.Count)
                                            {
                                                Wife.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                                Game.DisplaySubtitle(DialogWithGFIntro[DialogWithGFIntroIndex]);
                                                DialogWithGFIntroIndex++;
                                            }
                                            if (DialogWithGFIntroIndex == DialogWithGFIntro.Count)
                                            {
                                                Wife.Tasks.FollowNavigationMeshToPosition(HouseWifeSpawn, 0f, 1.3f).WaitForCompletion();
                                                Wife.TurnToFaceEntity(Game.LocalPlayer.Character, 1300);
                                                WifeEntersHouse = true;
                                            }
                                        }
                                    }
                                }
                                if (WifeEntersHouse && !TalkedToGFtwo)
                                {
                                    if (Game.LocalPlayer.Character.DistanceTo(Wife) <= 2f)
                                    {
                                        if (DialogWithGFTwoIndex < DialogWithGFTwo.Count)
                                        {
                                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the girlfriend.");
                                            if (Game.IsKeyDown(Settings.DialogKey))
                                            {
                                                Wife.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                                Game.DisplaySubtitle(DialogWithGFTwo[DialogWithGFTwoIndex]);
                                                DialogWithGFTwoIndex++;
                                            }
                                        }
                                        if (DialogWithGFTwoIndex == DialogWithGFTwo.Count)
                                        {
                                            MotiveTheft.Answer = new List<string>
                                            {
                                                "~b~You:~s~ Why did he steal the car, "+Functions.GetPersonaForPed(Wife).Forename+"?",
                                                "~y~Girlfriend:~s~ The customer insulted him. He has his honor, no?"
                                            };
                                            MotiveTheft.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ His honor, "+Functions.GetPersonaForPed(Wife).Forename+"?",
                                                "~y~Girlfriend:~s~ He said Yetarian's friend tried to make a woman out of him. He no longer respects this man Yetarian.",
                                                "~y~Girlfriend:~s~ He took the car to show this asshole that he is a man."
                                            };
                                            FoundClues.Add(MotiveTheft);
                                            GirlfriendQuestions.Add(MotiveTheft.Name);
                                            QuestionMenuGirlfriend.AddItem(MotiveTheftClue = new UIMenuItem(MotiveTheft.Name, MotiveTheft.ExaminMessage));
                                            QuestionMenuGirlfriend.RefreshIndex();
                                            LastContactSuspect.Name = "Last contact with " + Functions.GetPersonaForPed(Suspect).Forename;
                                            LastContactSuspect.Answer = new List<string>
                                            {
                                                "~b~You:~s~ Tell us the truth, "+Functions.GetPersonaForPed(Wife).Forename+". Has "+Functions.GetPersonaForPed(Suspect).Forename+" been here?",
                                                "~y~Girlfriend:~s~ I haven't seen him for at least three nights."
                                            };
                                            LastContactSuspect.PreLieAnswers = new List<string>
                                            {
                                                "~b~You:~s~ You keep lying to me and I'll send you and your baby to jail.",
                                                "~y~Girlfriend:~s~ He lives here but he hasn't come home. I swear it."
                                            };
                                            LastContactSuspect.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ Enough, "+Functions.GetPersonaForPed(Wife).Forename+". There are signs all over this place that he's been back.",
                                                "~y~Girlfriend:~s~ He was here last night.",
                                                "~y~Girlfriend:~s~ I have never seen him so angry. He went out to the garage and put some things in it.",
                                                "~y~Girlfriend:~s~ I don't know what and I don't want to know. I love him."
                                            };
                                            FoundClues.Add(LastContactSuspect);
                                            GirlfriendQuestions.Add(LastContactSuspect.Name);
                                            QuestionMenuGirlfriend.AddItem(LastContactSuspectClue = new UIMenuItem(LastContactSuspect.Name, LastContactSuspect.ExaminMessage));
                                            QuestionMenuGirlfriend.RefreshIndex();
                                            LicensePlateRecovered.Answer = new List<string>
                                            {
                                                "~b~You:~s~ We've found a license plate matching our stolen vehicle in the garage.",
                                                "~b~You:~s~ Add in the assortment of parts, and we can make "+Functions.GetPersonaForPed(Suspect).Forename+" for a dozen other thefts.",
                                                "~b~You:~s~ It's time to get serious, "+Functions.GetPersonaForPed(Wife).Forename+".",
                                                "~y~Girlfriend:~s~ You must ask these questions of "+Functions.GetPersonaForPed(Suspect).Forename,
                                                "~y~Girlfriend:~s~ I know nothing about these car parts.",
                                            };
                                            LicensePlateRecovered.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ Then tell us where he is!",
                                                "~b~You:~s~ If your baby is born in prison, "+Functions.GetPersonaForPed(Wife).Forename+", the corrections officers will take it from you.",
                                                "~b~You:~s~ You'll see your son or daughter through a metal grate for half an hour a week.",
                                                "~y~Girlfriend:~s~ The start line is on -Placeholderstreet-."
                                            };
                                            GameFiber.Sleep(1500);
                                            TalkedToGFtwo = true;
                                        }
                                    }
                                    
                                }
                                if (TalkedToGFtwo)
                                {
                                    if (Game.LocalPlayer.Character.DistanceTo(Wife) <= 2f)
                                    {
                                        if (GirlfriendQuestions.Count != 0)
                                        {
                                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to talk to the girlfriend.");
                                            ShowLANoireMenu(QuestionMenuGirlfriend, Wife, GirlfriendQuestions);
                                        }
                                    }
                                    if (Game.LocalPlayer.Character.DistanceTo(PlateLocations.ElementAt(1)) <= 2f && !BreakfastPlates.Finished)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to inspect the table.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                            Game.DisplaySubtitle("~b~You:~s~ You should've cleaned up");
                                            FoundClues.Add(BreakfastPlates);
                                            AllClues.AddItem(BreakfastPlatesClue = new UIMenuItem(BreakfastPlates.Name, BreakfastPlates.ExaminMessage));
                                            AllClues.RefreshIndex();
                                            BreakfastPlates.Finished = true;
                                        }
                                    }
                                    if (Game.LocalPlayer.Character.DistanceTo(NumberPlateLocation.Item1) <= 2f && !LicensePlateRecovered.Finished)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to inspect the license plate.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                            FoundClues.Add(LicensePlateRecovered);
                                            AllClues.AddItem(LicensePlateRecoveredClue = new UIMenuItem(LicensePlateRecovered.Name, LicensePlateRecovered.ExaminMessage));
                                            AllClues.RefreshIndex();
                                            LicensePlateRecovered.WitnessHonestyAnswers = new List<string>
                                            {
                                                "~b~You:~s~ Then tell us where he is!",
                                                "~b~You:~s~ If your baby is born in prison, -placeholder-, the corrections officers will take it from you.",
                                                "~b~You:~s~ You'll see your son or daughter through a metal grate for half an hour a week.",
                                                "~y~Girlfriend:~s~ The start line is on "+World.GetStreetName(World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(400, 450)))+"."
                                            };
                                            Game.DisplayNotification("~b~Clue added:~s~ " + LicensePlateRecovered.ExaminMessage);
                                            GirlfriendQuestions.Add(LicensePlateRecovered.Name);
                                            QuestionMenuGirlfriend.AddItem(LicensePlateRecoveredClue = new UIMenuItem(LicensePlateRecovered.Name, LicensePlateRecovered.ExaminMessage));
                                            QuestionMenuGirlfriend.RefreshIndex();
                                            LicensePlateRecovered.Finished = true;
                                        }
                                    }
                                    if (Game.LocalPlayer.Character.DistanceTo(WheelLocation) <= 2f && !WheelsRecovered.Finished)
                                    {
                                        Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to inspect the wheels.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Stuff.Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                            FoundClues.Add(WheelsRecovered);
                                            Game.DisplayNotification("~b~Clue added:~s~ " + WheelsRecovered.Name);
                                            WheelsRecovered.Finished = true;
                                        }
                                    }
                                    if (LicensePlateRecovered.AddClue && !LocationRaceSet)
                                    {
                                        Game.DisplayNotification("~b~New Location added:~s~ " + RaceLocation.LocationName);
                                        RaceLocation.Location = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(400, 450));
                                        LocationClues.Add(RaceLocation);
                                        GoToLocations.Add(RaceLocation.LocationName);
                                        LocationSelect.AddItem(RaceLocationMenu = new UIMenuItem(RaceLocation.LocationName));
                                        LocationSelect.RefreshIndex();
                                        LocationRaceSet = true;
                                    }
                                    if (LicensePlateRecovered.WrongAnswer && !LocationRaceSet)
                                    {
                                        Game.DisplayNotification("char_call911", "char_call911", "Dispatch", "~b~Attention to Unit:~s~", "We have a race in progress at the following location:");
                                        Game.DisplayNotification("~b~New Location added:~s~ "+RaceLocation.LocationName);
                                        Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 3000);
                                        GameFiber.Sleep(3000);
                                        RaceLocation.Location = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(400, 450));
                                        LocationClues.Add(RaceLocation);
                                        GoToLocations.Add(RaceLocation.LocationName);
                                        LocationSelect.AddItem(RaceLocationMenu = new UIMenuItem(RaceLocation.LocationName));
                                        LocationSelect.RefreshIndex();
                                        LocationRaceSet = true;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region RaceLocation
                        if (CurrentLocation == RaceLocation.LocationName && Game.LocalPlayer.Character.DistanceTo(RaceLocation.Location) <= 150f)
                        {
                            if (!RaceVehicle.Exists())
                            {
                                ClearUnrelatedEntities();
                                SpawnRaceVehicle();
                            }
                        }
                        if (CurrentLocation == RaceLocation.LocationName && !ActivatedChase)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(Suspect) <= 15f)
                            {
                                ChaseStuff = Functions.CreatePursuit();
                                Functions.AddPedToPursuit(ChaseStuff, Suspect);
                                Suspect.Tasks.Clear();
                                Functions.SetPursuitIsActiveForPlayer(ChaseStuff, true);
                                ActivatedChase = true;
                            }
                        }
                        if (CurrentLocation == RaceLocation.LocationName && ActivatedChase)
                        {
                            if (!Functions.IsPursuitStillRunning(ChaseStuff))
                            {
                                this.End();
                            }
                        }
                        #endregion
                        #region SelectDestination
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) == true && LocationMenu == true && ChaseActivate == false && !LocationSelect.Visible)
                        {
                            LocationSelect.Visible = true;
                        }
                        if (LocationSelect.Visible == true)
                        {
                            int SelectInt = LocationSelect.CurrentSelection;
                            if (Game.IsKeyDown(Keys.Enter))
                            {
                                Clue c = LocationClues.Find(x => x.LocationName == LocationSelect.MenuItems[SelectInt].Text);
                                if (HouseBlip.Exists()) { HouseBlip.Delete(); }
                                HouseBlip = new Blip(c.Location);
                                HouseBlip.Color = Color.Yellow;
                                HouseBlip.EnableRoute(Color.Yellow);
                                NextAnswerStuff = false;
                                NextAnswerStuff2 = false;
                                NextAnswerStuff3 = false;
                                NextAnswerStuff4 = false;
                                CurrentLocation = c.LocationName;
                                LocationMenu = false;
                                LocationSelect.Close();
                            }
                        }
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) == false && LocationMenu == false && ChaseActivate == false)
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
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (HouseBlip.Exists()) { HouseBlip.Delete(); }
            Utils.DeleteCheckpoint(DoorEnterCheckpoint);
            Utils.DeleteCheckpoint(DoorExitCheckpoint);
            Utils.OpenDoor(FrontDoor.Item1, FrontDoor.Item2, false, 0);
            Utils.OpenDoor(GarageDoor.Item1, GarageDoor.Item2, false, 0);
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
        private void SpawnAllPlates()
        {
            for (int i = 0; i < PlateLocations.Count; i++)
            {
                Rage.Object marker = new Rage.Object("prop_cs_plate_01", PlateLocations[i], 0f);
                marker.IsPersistent = true;
                marker.MakeMission();
                AllSpawnedObjects.Add(marker);
                AllBankHeistEntities.Add(marker);
            }
            Rage.Object LicensePlate = new Rage.Object("prop_cs_cardbox_01", NumberPlateLocation.Item1, 0f);
            LicensePlate.IsPersistent = true;
            LicensePlate.MakeMission();
            AllSpawnedObjects.Add(LicensePlate);
            AllBankHeistEntities.Add(LicensePlate);
        }
        private void SpawnSuspectVehicle()
        {
            SuspectVehicle = new Vehicle("xls2", SuspectVehicleLocation, SuspectVehicleHeading);
            SuspectVehicle.IsPersistent = true;
            SuspectVehicle.MakeMission();
            Functions.SetVehicleOwnerName(SuspectVehicle, "Argentinian Consulate");
            Utils.RemoveVehicleDoor(SuspectVehicle, Utils.VehDoorID.FrontLeft, true);
            SuspectVehicle.LicensePlate = "";
            SuspectVehicle.VehicleTyreBurst(Utils.VehTyreID.FrontRight, true);
            AllSpawnedPoliceVehicles.Add(SuspectVehicle);
            AllBankHeistEntities.Add(SuspectVehicle);
        }
        private void SpawnShowcaseCarVehicle()
        {
            ShowcaseCar = new Vehicle("adder", ShowcaseVehicleLocation.Item1, ShowcaseVehicleLocation.Item2);
            ShowcaseCar.IsPersistent = true;
            ShowcaseCar.MakeMission();
            Functions.SetVehicleOwnerName(ShowcaseCar, "No owner");
            ShowcaseCar.LicensePlate = "";
            AllSpawnedPoliceVehicles.Add(ShowcaseCar);
            AllBankHeistEntities.Add(ShowcaseCar);
        }
        private void SpawnRaceVehicle()
        {
            RaceVehicle = new Vehicle("alpha", RaceLocation.Location, Utils.VehicleNodeHeading(RaceLocation.Location));
            RaceVehicle.IsPersistent = true;
            RaceVehicle.MakeMission();
            RaceVehicle.SetColors(EPaint.Blaze_Red, EPaint.Blaze_Red);
            Functions.SetVehicleOwnerName(RaceVehicle, Functions.GetPersonaForPed(Suspect).FullName);
            AllSpawnedPoliceVehicles.Add(RaceVehicle);
            AllBankHeistEntities.Add(RaceVehicle);
            Suspect.WarpIntoVehicle(RaceVehicle, -1);
            Suspect.Tasks.PerformDrivingManeuver(VehicleManeuver.BurnOut);
        }
        private void SpawnConsulPed()
        {
            ConsulPed = new Ped("a_m_m_business_01", StorageSpawn, 0f);
            ConsulPed.IsPersistent = true;
            ConsulPed.BlockPermanentEvents = true;
            ConsulPed.MakeMission();
            AllSpawnedPeds.Add(ConsulPed);
            AllBankHeistEntities.Add(ConsulPed);
        }
        private void SpawnSuspect()
        {
            Suspect = new Ped("g_m_y_mexgoon_03", StorageSpawn, 0f);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            Suspect.MakeMission();
            AllSpawnedPeds.Add(Suspect);
            AllBankHeistEntities.Add(Suspect);
        }
        private void SpawnHousewife()
        {
            Wife = new Ped("a_f_m_bevhills_01", StorageSpawn, 0f);
            Wife.IsPersistent = true;
            Wife.BlockPermanentEvents = true;
            Wife.MakeMission();
            AllSpawnedPeds.Add(Wife);
            AllBankHeistEntities.Add(Wife);
        }
        private void SpawnSimeonPed()
        {
            Simeon = new Ped("ig_siemonyetarian", StorageSpawn, 0f);
            Simeon.IsPersistent = true;
            Simeon.BlockPermanentEvents = true;
            Simeon.MakeMission();
            Functions.SetPersonaForPed(Simeon, new Persona("Simeon", "Yetarian", LSPD_First_Response.Gender.Male));
            AllSpawnedPeds.Add(Simeon);
            AllBankHeistEntities.Add(Simeon);
            DoorEnterCheckpoint = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, CarDealerEnterLocation, CarDealerEnterLocation, 1.3f, Color.DarkBlue);
            Utils.SetCheckpointHeight(DoorEnterCheckpoint, 1.3f, 1.3f, 1.3f);
            DoorExitCheckpoint = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, CarDealerExitLocation, CarDealerExitLocation, 1.3f, Color.DarkBlue);
            Utils.SetCheckpointHeight(DoorExitCheckpoint, 1.3f, 1.3f, 1.3f);
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
        }
        private void SpawnWitnessAndSupervisor()
        {
            BenchAccident = new Rage.Object("hei_heist_stn_benchshort", BenchLocation.Item1, BenchLocation.Item2);
            BenchAccident.IsPersistent = true;
            BenchAccident.IsPositionFrozen = true;
            AllSpawnedObjects.Add(BenchAccident);
            AllBankHeistEntities.Add(BenchAccident);
            if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
            {
                UltBackupSpawnSupervisor();
            }
            else
            {
                Supervisor = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), SupervisorLocation.Item1, SupervisorLocation.Item2);
                Supervisor.IsPersistent = true;
                Supervisor.BlockPermanentEvents = true;
                Supervisor.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                AllSpawnedPeds.Add(Supervisor);
                AllBankHeistEntities.Add(Supervisor);
            }
            Witness = new Ped(WitnessLocation.Item1, WitnessLocation.Item2);
            Witness.IsPersistent = true;
            Witness.BlockPermanentEvents = true;
            AllSpawnedPeds.Add(Witness);
            AllBankHeistEntities.Add(Witness);
            EviBook = new Rage.Object("xm_prop_x17_book_bogdan", new Vector3(0f, 0f, 0f), 0f);
            EviBook.IsPersistent = true;
            AllSpawnedObjects.Add(EviBook);
            AllBankHeistEntities.Add(EviBook);
            EviBook.AttachTo(Witness, Witness.GetBoneIndex(PedBoneId.RightPhHand), new Vector3(3.54512595e-05f, -0.0299982019f, 0.100021258f), new Rotator(1.38739313e-06f, -9.3375327e-08f, 40.6201935f));
            Witness.Tasks.PlayAnimation("cellphone@", "cellphone_email_read_base", 1f, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask);
            Witness.Tasks.PlayAnimation("anim@heists@heist_safehouse_intro@phone_couch@male", "phone_couch_male_idle", 1f, AnimationFlags.Loop );
        }

        private void SpawnAllEvidences()
        {
            EviWrench = new Rage.Object("xs_prop_arena_torque_wrench_01a", Utils.ToGround(WrenchLocation));
            EviWrench.IsPersistent = true;
            EviWrench.MakeMission();
            AllSpawnedObjects.Add(EviWrench);
            AllBankHeistEntities.Add(EviWrench);
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
        private void UltBackupSpawnSupervisor()
        {
            Supervisor = UltimateBackup.API.Functions.getLocalPatrolPed(SupervisorLocation.Item1, SupervisorLocation.Item2);
            Supervisor.IsPersistent = true;
            Supervisor.BlockPermanentEvents = true;
            Supervisor.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            AllSpawnedPeds.Add(Supervisor);
            AllBankHeistEntities.Add(Supervisor);
        }
        private void InitMenu()
        {
            //Create MenuPool and Add the main menu
            _MenuPool = new MenuPool();

            QuestionMenuWitness = new UIMenu("Question Menu", "");
            QuestionMenuConsul = new UIMenu("Question Menu", "");
            QuestionMenuSimeon = new UIMenu("Question Menu", "");
            QuestionMenuGirlfriend = new UIMenu("Question Menu", "");
            TruthMenu = new UIMenu("Choose", "");
            AllClues = new UIMenu("Found Clues", "");
            LocationSelect = new UIMenu("Select Location", "");
            _MenuPool.Add(QuestionMenuWitness);
            _MenuPool.Add(QuestionMenuConsul);
            _MenuPool.Add(QuestionMenuSimeon);
            _MenuPool.Add(QuestionMenuGirlfriend);
            _MenuPool.Add(TruthMenu);
            _MenuPool.Add(AllClues);
            _MenuPool.Add(LocationSelect);
            TruthMenu.AddItem(Truth = new UIMenuItem("Truth", "Choose this if you believe the person is telling the truth"));
            TruthMenu.AddItem(Doubt = new UIMenuItem("Doubt", "Choose this if you believe the person is lying but you can't proof it"));
            TruthMenu.AddItem(Lie = new UIMenuItem("Lie", "Choose this if you know the person is lying and you can proof it"));
            TruthMenu.RefreshIndex();
            LocationSelect.AddItem(AccidentLocationMenu = new UIMenuItem("Accident Location"));
            LocationSelect.RefreshIndex();
        }
        private void MenuLogic()
        {
            _MenuPool.ProcessMenus();
        }
        private void ShowLANoireMenu(UIMenu QuestioningMenu, Ped ped, List<string> PedQuestions)
        {
            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == false && NextAnswerStuff2 == false && NextAnswerStuff3 == false && NextAnswerStuff4 == false)
            {
                QuestioningMenu.Visible = true;
            }
            if (QuestioningMenu.Visible)
            {
                int QuestionInt = QuestioningMenu.CurrentSelection;
                if (Game.IsKeyDown(Keys.Enter))
                {
                    Clue Clu = FoundClues.Find(x => x.Name == QuestioningMenu.MenuItems[QuestionInt].Text);
                    NextAnswerString = QuestioningMenu.MenuItems[QuestionInt].Text;
                    NextAnswerInt = QuestioningMenu.CurrentSelection;
                    NextAnswerStuff = true;
                    if (Clu.AnswerIndex < Clu.Answer.Count)
                    {
                        Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                        Clu.AnswerIndex++;
                    }
                    QuestioningMenu.Close();
                }
            }
            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff == true && NextAnswerStuff2 == false && !TruthMenu.Visible)
            {
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Clu.AnswerIndex < Clu.Answer.Count)
                {
                    Game.DisplaySubtitle(Clu.Answer[Clu.AnswerIndex]);
                    Clu.AnswerIndex++;
                }
                if (Clu.AnswerIndex == Clu.Answer.Count)
                {
                    switch (Clu.HonestyWitness)
                    {
                        case "Truth":
                            ped.TruthSpeech();
                            break;
                        case "Doubt":
                            ped.DoubtSpeech();
                            break;
                        case "Lie":
                            ped.LieSpeech();
                            break;
                        default:
                            break;
                    }
                    TruthMenu.Visible = true;
                }
            }
            if (TruthMenu.Visible)
            {
                int TruthChoice = TruthMenu.CurrentSelection;
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Game.IsKeyDown(Keys.Enter))
                {
                    if (Clu.HonestyWitness == "Truth")
                    {
                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                        {
                            ped.Tasks.Clear();
                            NextAnswerStuff2 = true;
                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                            {
                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                Clu.HonestyAnswerIndex++;
                            }
                            TruthMenu.Close();
                        }
                        else
                        {
                            ped.Tasks.Clear();
                            Game.DisplaySubtitle("False");
                            PedQuestions.Remove(NextAnswerString);
                            QuestioningMenu.RemoveItemAt(NextAnswerInt);
                            QuestioningMenu.RefreshIndex();
                            Clu.WrongAnswer = true;
                            NextAnswerStuff = false;
                            TruthMenu.Close();
                        }
                    }
                    else if (Clu.HonestyWitness == "Doubt")
                    {
                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                        {
                            ped.Tasks.Clear();
                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                            {
                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                Clu.HonestyAnswerIndex++;
                            }
                            NextAnswerStuff2 = true;
                            TruthMenu.Close();
                        }
                        else
                        {
                            ped.Tasks.Clear();
                            Game.DisplaySubtitle("False");
                            PedQuestions.Remove(NextAnswerString);
                            QuestioningMenu.RemoveItemAt(NextAnswerInt);
                            QuestioningMenu.RefreshIndex();
                            Clu.WrongAnswer = true;
                            NextAnswerStuff = false;
                            TruthMenu.Close();
                        }
                    }
                    else if (Clu.HonestyWitness == "Lie")
                    {
                        if (TruthMenu.MenuItems[TruthChoice].Text == Clu.HonestyWitness)
                        {
                            NextAnswerStuff = false;
                            if (Clu.PreLieAnswersIndex < Clu.PreLieAnswers.Count)
                            {
                                Game.DisplaySubtitle(Clu.PreLieAnswers[Clu.PreLieAnswersIndex]);
                                Clu.PreLieAnswersIndex++;
                            }
                            NextAnswerStuff4 = true;
                            TruthMenu.Close();
                        }
                        else
                        {
                            ped.Tasks.Clear();
                            Game.DisplaySubtitle("False");
                            PedQuestions.Remove(NextAnswerString);
                            QuestioningMenu.RemoveItemAt(NextAnswerInt);
                            QuestioningMenu.RefreshIndex();
                            Clu.WrongAnswer = true;
                            NextAnswerStuff = false;
                            TruthMenu.Close();
                        }
                    }
                }
            }
            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff2 == true && !TruthMenu.Visible)
            {
                NextAnswerStuff = false;
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                {
                    Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                    Clu.HonestyAnswerIndex++;
                }
                if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                {
                    PedQuestions.Remove(NextAnswerString);
                    QuestioningMenu.RemoveItemAt(NextAnswerInt);
                    QuestioningMenu.RefreshIndex();
                    Clu.AddClue = true;
                    NextAnswerStuff2 = false;
                }
            }
            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff4 == true && !TruthMenu.Visible)
            {
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Clu.PreLieAnswersIndex < Clu.WitnessHonestyAnswers.Count)
                {
                    Game.DisplaySubtitle(Clu.PreLieAnswers[Clu.PreLieAnswersIndex]);
                    Clu.PreLieAnswersIndex++;
                }
                if (Clu.PreLieAnswersIndex == Clu.PreLieAnswers.Count)
                {
                    AllClues.Visible = true;
                    NextAnswerStuff4 = false;
                }
            }
            if (AllClues.Visible)
            {
                int TruthChoice = AllClues.CurrentSelection;
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Game.IsKeyDown(Keys.Enter))
                {
                    bool found = false;
                    foreach (string c in Clu.WitnessLieStuff)
                    {
                        if (AllClues.MenuItems[TruthChoice].Text == c)
                        {
                            ped.Tasks.Clear();
                            NextAnswerStuff3 = true;
                            if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                            {
                                Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                                Clu.HonestyAnswerIndex++;
                            }
                            found = true;
                            NextAnswerStuff3 = true;
                            AllClues.Close();
                        }
                    }
                    /*if (Clu.WitnessLieStuff.Contains(AllClues.MenuItems[TruthChoice].Text))
                    {
                        ped.Tasks.Clear();
                        NextAnswerStuff3 = true;
                        if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                        {
                            Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                            Clu.HonestyAnswerIndex++;
                        }
                        NextAnswerStuff3 = true;
                        AllClues.Close();
                    }*/
                    if (!found)
                    {
                        ped.Tasks.Clear();
                        Game.DisplaySubtitle("False");
                        PedQuestions.Remove(NextAnswerString);
                        QuestioningMenu.RemoveItemAt(NextAnswerInt);
                        QuestioningMenu.RefreshIndex();
                        Clu.WrongAnswer = true;
                        NextAnswerStuff = false;
                        AllClues.Close();
                    }
                }
            }
            if (Game.IsKeyDown(Settings.DialogKey) && NextAnswerStuff3 == true && !AllClues.Visible)
            {
                NextAnswerStuff = false;
                Clue Clu = FoundClues.Find(x => x.Name == NextAnswerString);
                if (Clu.HonestyAnswerIndex < Clu.WitnessHonestyAnswers.Count)
                {
                    Game.DisplaySubtitle(Clu.WitnessHonestyAnswers[Clu.HonestyAnswerIndex]);
                    Clu.HonestyAnswerIndex++;
                }
                if (Clu.HonestyAnswerIndex == Clu.WitnessHonestyAnswers.Count)
                {
                    PedQuestions.Remove(NextAnswerString);
                    QuestioningMenu.RemoveItemAt(NextAnswerInt);
                    QuestioningMenu.RefreshIndex();
                    Clu.AddClue = true;
                    NextAnswerStuff3 = false;
                }
            }
        }
    }

}
