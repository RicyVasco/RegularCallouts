using LSPD_First_Response.Engine.Scripting;
using LSPD_First_Response.Mod.API;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegularCallouts.Stuff
{
    internal static class Settings
    {
        internal static bool AccidentOnHighway;
        internal static bool AnimalCarCrash;
        internal static bool AssistancePark;
        internal static bool DomesticDisturbance;
        internal static bool FakeCall;
        internal static bool MissingPerson;
        internal static bool NoiseDisturbance;
        internal static bool ParkedCarCrash;
        internal static bool SearchWarrant;
        internal static bool TreeOnStreet;
        internal static bool SuspiciousVehicle;
        internal static bool BarricadedSuspect;
        internal static bool RobberyInProgress;
        internal static bool BrokenVehicle;
        internal static bool Escort;
        internal static bool Homicide;
        internal static bool Scammer;
        internal static bool AbandondedVehicle;
        internal static bool PotentialBomb;
        internal static bool StolenDiplomaticCar;
        internal static bool TrafficStopAssistance;
        internal static bool LostSuspect;
        public static readonly string CalloutVersion = "1.1.5";

        internal static Keys EndCalloutKey;
        internal static Keys DialogKey;
        internal static Keys InteractionKey;

        internal static void Initialize()
        {
            InitializationFile initializationFile = new InitializationFile("Plugins/LSPDFR/RegularCallouts.ini"); //This is where you put the path to the ini, starting in GTA folder
            initializationFile.Create();
            Game.LogTrivial("Initializing Config for Regular Callouts");
            Settings.FakeCall = initializationFile.ReadBoolean("Callout Enable/Disable", "FakeCall", false);
            Settings.AccidentOnHighway = initializationFile.ReadBoolean("Callout Enable/Disable", "AccidentOnHighway", true);
            Settings.AnimalCarCrash = initializationFile.ReadBoolean("Callout Enable/Disable", "AnimalCarCrash", true);
            Settings.AssistancePark = initializationFile.ReadBoolean("Callout Enable/Disable", "AssistancePark", true);
            Settings.DomesticDisturbance = initializationFile.ReadBoolean("Callout Enable/Disable", "DomesticDisturbance", true);
            Settings.MissingPerson = initializationFile.ReadBoolean("Callout Enable/Disable", "MissingPerson", true);
            Settings.NoiseDisturbance = initializationFile.ReadBoolean("Callout Enable/Disable", "NoiseDisturbance", true);
            Settings.ParkedCarCrash = initializationFile.ReadBoolean("Callout Enable/Disable", "ParkedCarCrash", true);
            Settings.SearchWarrant = initializationFile.ReadBoolean("Callout Enable/Disable", "SearchWarrant", true);
            Settings.TreeOnStreet = initializationFile.ReadBoolean("Callout Enable/Disable", "TreeOnStreet", true);
            Settings.SuspiciousVehicle = initializationFile.ReadBoolean("Callout Enable/Disable", "SuspiciousVehicle", true);
            Settings.BarricadedSuspect = initializationFile.ReadBoolean("Callout Enable/Disable", "BarricadedSuspect", true);
            Settings.RobberyInProgress = initializationFile.ReadBoolean("Callout Enable/Disable", "RobberyInProgress", true);
            Settings.BrokenVehicle = initializationFile.ReadBoolean("Callout Enable/Disable", "BrokenVehicle", true);
            Settings.Escort = initializationFile.ReadBoolean("Callout Enable/Disable", "Escort", true);
            Settings.Homicide = initializationFile.ReadBoolean("Callout Enable/Disable", "Homicide", true);
            Settings.Scammer = initializationFile.ReadBoolean("Callout Enable/Disable", "Scammer", true);
            Settings.AbandondedVehicle = initializationFile.ReadBoolean("Callout Enable/Disable", "AbandondedVehicle", true);
            Settings.PotentialBomb = initializationFile.ReadBoolean("Callout Enable/Disable", "PotentialBomb", true);
            Settings.StolenDiplomaticCar = initializationFile.ReadBoolean("Callout Enable/Disable", "StolenDiplomaticCar", true);
            Settings.TrafficStopAssistance = initializationFile.ReadBoolean("Callout Enable/Disable", "TrafficStopAssistance", true);
            Settings.LostSuspect = initializationFile.ReadBoolean("Callout Enable/Disable", "LostSuspect", true);
            //In the parantheses the first part is the label in the ini file, the second is the name of the variable, and the third is the default value
            EndCalloutKey = initializationFile.ReadEnum("Keybinds", "EndCalloutKey", Keys.End);
            DialogKey = initializationFile.ReadEnum("Keybinds", "DialogKey", Keys.Y);
            InteractionKey = initializationFile.ReadEnum("Keybinds", "InteractionKey", Keys.X);

        }
    }
}