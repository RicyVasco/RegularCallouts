using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;

namespace RegularCallouts
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("Plugin RegularCallouts "+ Stuff.Settings.CalloutVersion +" has been initialised.");
            Game.LogTrivial("Go on duty to fully load RegularCallouts.");
            RegularCallouts.Stuff.Settings.Initialize();

        }
        public override void Finally()
        {
            Game.LogTrivial("RegularCallouts has been cleaned up.");
        }
        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                Game.DisplayNotification("Thanks for installing ~b~RegularCallouts~r~ "+ Stuff.Settings.CalloutVersion+"~w~ by ~b~RicyVasco~w~!");
                VersionCheck.Check.isUpdateAvailable();
                RegisterCallouts();
            }
        }
        private static void RegisterCallouts()
        {
            if(Stuff.Settings.AccidentOnHighway == true)
            {
                Functions.RegisterCallout(typeof(Callouts.AccidentOnHighway));
            }
            if(Stuff.Settings.AnimalCarCrash == true)
            {
                Functions.RegisterCallout(typeof(Callouts.AnimalCarCrash));
            }
            if(Stuff.Settings.AssistancePark == true)
            {
                Functions.RegisterCallout(typeof(Callouts.AssistanceParkRemastered));
            }
            if(Stuff.Settings.DomesticDisturbance == true)
            {
                Functions.RegisterCallout(typeof(Callouts.DomesticDisturbance));
            }
            if(Stuff.Settings.FakeCall == true)
            {
                Functions.RegisterCallout(typeof(Callouts.FakeCall));
            }
            if(Stuff.Settings.MissingPerson == true)
            {
                Functions.RegisterCallout(typeof(Callouts.MissingPerson));
            }
            if(Stuff.Settings.NoiseDisturbance == true)
            {
                Functions.RegisterCallout(typeof(Callouts.NoiseDisturbance));
            }
            if(Stuff.Settings.ParkedCarCrash == true)
            {
                Functions.RegisterCallout(typeof(Callouts.ParkedCarCrash));
            }
            if(Stuff.Settings.SearchWarrant == true)
            {
                Functions.RegisterCallout(typeof(Callouts.SearchWarrant));
            }
            if (Stuff.Settings.TreeOnStreet == true)
            {
                Functions.RegisterCallout(typeof(Callouts.TreeOnStreet));
            }
            if (Stuff.Settings.SuspiciousVehicle == true)
            {
                Functions.RegisterCallout(typeof(Callouts.SuspiciousVehicle));
            }
            if (Stuff.Settings.BarricadedSuspect == true)
            {
                Functions.RegisterCallout(typeof(Callouts.BarricadedSuspect));
            }
            if (Stuff.Settings.RobberyInProgress == true)
            {
                Functions.RegisterCallout(typeof(Callouts.RobberyInProgress));
            }
            if (Stuff.Settings.BrokenVehicle == true)
            {
                Functions.RegisterCallout(typeof(Callouts.BrokenVehicle));
            }
            if (Stuff.Settings.Homicide == true)
            {
                Functions.RegisterCallout(typeof(Callouts.Homicide));
            }
            if (Stuff.Settings.Scammer == true)
            {
                Functions.RegisterCallout(typeof(Callouts.Scammer));
            }
            if (Stuff.Settings.AbandondedVehicle == true)
            {
                Functions.RegisterCallout(typeof(Callouts.AbandondedVehicle));
            }
            if (Stuff.Settings.PotentialBomb == true)
            {
                Functions.RegisterCallout(typeof(Callouts.PotentialBomb));
            }
            if (Stuff.Settings.StolenDiplomaticCar == true)
            {
                Functions.RegisterCallout(typeof(Callouts.StolenDiplomaticCar));
            }
            if (Stuff.Settings.TrafficStopAssistance == true)
            {
                Functions.RegisterCallout(typeof(Callouts.TrafficStopAssistance));
            }
            if (Stuff.Settings.LostSuspect == true)
            {
                Functions.RegisterCallout(typeof(Callouts.LostSuspect));
            }
            //Functions.RegisterCallout(typeof(Callouts.DrugDealOperation));
            //Functions.RegisterCallout(typeof(Callouts.KidnappingInProgress));
            //Functions.RegisterCallout(typeof(Callouts.Escort));
        }

    }
}
