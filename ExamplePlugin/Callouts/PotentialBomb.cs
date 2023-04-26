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
using LucasRitter.Scaleforms.Generic;
using LucasRitter.Scaleforms;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Potential Bomb", CalloutProbability.Medium)]
    public class PotentialBomb : Callout
    {
        private Vector3 SpawnPoint;
        private Vector3 CallerLocation;
        private Blip SpawnBlip;
        private string CalloutIs;
        private Vector3 SpawnPoint1;
        private Vector3 SpawnPoint2;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private List<Rage.Object> AllCalloutObjects = new List<Rage.Object>();
        private bool CalloutRunning;
        private bool ClickedConnect;
        private Rage.Object Bomb;
        private Ped Supervisor;
        private Vehicle Transporter;
        private Vehicle BomCar;
        private Ped Clone;
        private bool SuperVisorBlipped;
        private bool SpokenWithSupervisor;
        private int viewmode;
        private int viewmodeonfoot;
        private bool RobotActive;
        private bool BombExplode;
        private bool BombDefused;
        private Hacking dashboard;
        private Rage.Object TabletPlayer;
        private int lives = 5;
        private int ClickReturn;
        private bool HackingGamebool;
        private bool IsInRobot;
        private bool ClickedGame;
        private bool ShowScreen;
        private List<Vector3> POLICECarLocations;
        private List<float> POLICECarHeadings;
        private List<Tuple<Vector3, float>> BarrierSpawns;
        private Tuple<Vector3, float> PoliceRiotLoc;
        private Tuple<Vector3, float> AmbulanceLoc;
        private Tuple<Vector3, float> FireTrukLoc;
        private Tuple<Vector3, float> TransporterLoc;
        private Tuple<Vector3, float> BomcarLoc;
        private Tuple<Vector3, float> BombLoc;
        private Tuple<Vector3, float> SuperVisorLoc;
        private Tuple<Vector3, float> CloneLoc;
        private static string[] CarModels;

        private List<string> DialogWithSupervisor = new List<string>
        {
            "~b~You:~s~ What's the situation?",
            "~g~Supervisor:~s~ We've found a bag near the traintracks.",
            "~g~Supervisor:~s~ We don't know if it's anything dangerous but I don't wanna take any chances.",
            "~g~Supervisor:~s~ The bomb defusal robot is already in position, you can control it as soon as you want. (Use ~y~Left Mouse Click~s~ to control the menu's)",
            "~b~You:~s~ Alright let me take it from here."
        };
        private int DialogWithSupervisorIndex;

        private static string[] CalloutPossibility = new string[] { "Bomb" };
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(-184.016342f, -2006.37854f, 26.0708904f);
            SpawnPoint2 = new Vector3(2313.30005f, 3774.49731f, 37.4183578f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                //Stadium Spawn
                SpawnPoint = SpawnPoint1;
                CallerLocation = new Vector3(-5.07373047f, -1380.30212f, 29.3123722f);
                POLICECarLocations = new List<Vector3>() { new Vector3(-164.531387f, -2004.28699f, 22.5336018f), new Vector3(-186.614792f, -2018.28967f, 26.8967495f), new Vector3(-186.700912f, -2009.19727f, 26.8672562f) };
                POLICECarHeadings = new List<float>() { 141.063126f, 85.0704575f, 10.6443615f };
                PoliceRiotLoc = new Tuple<Vector3, float>(new Vector3(-186.615005f, -2002.62708f, 27.7897453f), 147.463318f);
                AmbulanceLoc = new Tuple<Vector3, float>(new Vector3(-174.255341f, -2002.18408f, 25.0559921f), 83.9392166f);
                FireTrukLoc = new Tuple<Vector3, float>(new Vector3(-192.3255f, -1996.01868f, 27.7659969f), 127.66938f);
                TransporterLoc = new Tuple<Vector3, float>(new Vector3(-195.218903f, -2015.40906f, 27.8521175f), -65.8586044f);
                BomcarLoc = new Tuple<Vector3, float>(new Vector3(-201.077667f, -2018.00232f, 27.0490437f), 112.764549f);
                BombLoc = new Tuple<Vector3, float>(new Vector3(-228.983475f, -2039.56934f, 26.7568531f), 92.2426758f);
                SuperVisorLoc = new Tuple<Vector3, float>(new Vector3(-184.855316f, -2003.97424f, 27.320343f), -126.212166f);
                CarModels = new string[] { "POLICE", "POLICE2", "POLICE3", "POLICE4" };
                BarrierSpawns = new List<Tuple<Vector3, float>>() { new Tuple<Vector3, float>(new Vector3(-162.88475f, -2000.15259f, 22.1760788f), -40.8666229f), new Tuple<Vector3, float>(new Vector3(-161.62674f, -2003.38916f, 22.0382385f), -95.8664093f), new Tuple<Vector3, float>(new Vector3(-161.837769f, -2007.34802f, 21.7421932f), -95.8664093f), new Tuple<Vector3, float>(new Vector3(-163.346985f, -2015.13782f, 21.8186684f), -121.85025f), new Tuple<Vector3, float>(new Vector3(-166.367462f, -2015.56458f, 22.2705193f), 179.772064f) };
                Game.LogTrivial("SpawnPoint 1 Chosen");
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                //Sandy Spawn
                SpawnPoint = SpawnPoint2;
                CallerLocation = new Vector3(-5.07373047f, -1380.30212f, 29.3123722f);
                POLICECarLocations = new List<Vector3>() { new Vector3(2320.70801f, -3774.51367f, 37.7013512f), new Vector3(2293.21948f, -3759.81909f, 37.3284836f), new Vector3(2310.43726f, 3769.02197f, 37.6412086f) };
                POLICECarHeadings = new List<float>() { 125.558662f, 73.3647842f, 133.269913f };
                PoliceRiotLoc = new Tuple<Vector3, float>(new Vector3(2311.71558f, 3775.84668f, 38.1632957f), 90.9996262f);
                AmbulanceLoc = new Tuple<Vector3, float>(new Vector3(2319.94507f, 3782.03735f, 37.6665955f), 127.360085f);
                FireTrukLoc = new Tuple<Vector3, float>(new Vector3(2292.82764f, 3767.70459f, 38.0157394f), 102.430283f);
                TransporterLoc = new Tuple<Vector3, float>(new Vector3(2311.64575f, 3780.90796f, 37.8366356f), -76.391861f);
                BomcarLoc = new Tuple<Vector3, float>(new Vector3(2305.57886f, 3779.69092f, 37.0505447f), -104.734238f);
                BombLoc = new Tuple<Vector3, float>(new Vector3(2270.30884f, 3770.16748f, 37.3294029f), -119.004143f);
                SuperVisorLoc = new Tuple<Vector3, float>(new Vector3(2311.42383f, 3773.95166f, 38.0400887f), -155.207397f);
                CarModels = new string[] { "SHERIFF", "SHERIFF2" };
                BarrierSpawns = new List<Tuple<Vector3, float>>() { new Tuple<Vector3, float>(new Vector3(2296.49609f, 3755.78711f, 36.8512955f), -165.357407f), new Tuple<Vector3, float>(new Vector3(2301.75854f, 3759.27539f, 37.1687737f), -139.112823f), new Tuple<Vector3, float>(new Vector3(2307.90747f, 3764.23804f, 37.188755f), -141.537643f), new Tuple<Vector3, float>(new Vector3(2316.97339f, 3770.93262f, 36.9001808f), -122.655045f), new Tuple<Vector3, float>(new Vector3(2320.44604f, 3776.479f, 36.9984055f), -120.749039f) };
                Game.LogTrivial("SpawnPoint 2 Chosen");
            }
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 40f);
            CalloutMessage = "Potential Bomb found";
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
            SpawnBlip.EnableRoute(Color.Yellow);
            dashboard = new Hacking();
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
                    ClearUnrelatedEntities();
                    Game.LogTrivial("Unrelated entities cleared");
                    GameFiber.Yield();
                    MakeNearbyPedsFlee();
                    GameFiber.Yield();
                    CreateSpeedZone();
                    GameFiber.Yield();
                    SpawnAllEntities();
                    GameFiber.Yield();
                    SpawnVehicles();
                    GameFiber.Yield();
                    Game.LogTrivial("Initialisation complete, Spawning Callout");
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 20f && !SuperVisorBlipped)
                        {
                            SpawnBlip.Delete();
                            SpawnBlip = Supervisor.AttachBlip();
                            SpawnBlip.Color = Color.Green;
                            Game.DisplayHelp("Go speak to the ~g~Supervisor~s~ to get a status report");
                            SuperVisorBlipped = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Supervisor) <= 2f && !SpokenWithSupervisor)
                        {
                            #region HackingXStuff
                            /*if (Game.IsKeyDown(Settings.InteractionKey))
                            {
                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LABELS");
                                Natives.Graphics.PushScaleformMovieParameterString("Local Disk (C:)");
                                Natives.Graphics.PushScaleformMovieParameterString("Network");
                                Natives.Graphics.PushScaleformMovieParameterString("External Device (J:)");
                                Natives.Graphics.PushScaleformMovieParameterString("HackConnect.exe");
                                Natives.Graphics.PushScaleformMovieParameterString("BruteForce.exe");
                                Natives.Graphics.PushScaleformMovieParameterString("Test.exe");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_BACKGROUND");
                                Natives.Graphics.PushScaleformMovieParameterInt(3);
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "ADD_PROGRAM");
                                Natives.Graphics.PushScaleformMovieParameterFloat(1.0f);
                                Natives.Graphics.PushScaleformMovieParameterFloat(4.0f);
                                Natives.Graphics.PushScaleformMovieParameterString("Bomb Defusal Robot");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "ADD_PROGRAM");
                                Natives.Graphics.PushScaleformMovieParameterFloat(6.0f);
                                Natives.Graphics.PushScaleformMovieParameterFloat(6.0f);
                                Natives.Graphics.PushScaleformMovieParameterString("Power Off");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(0);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(150, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(1);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(160, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(2);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(170, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(3);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(180, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(4);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(190, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(5);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(200, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(6);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(210, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(7);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();
                                HackingGamebool = true;

                                while (HackingGamebool)
                                {
                                    GameFiber.Yield();
                                    HackingGame();
                                    dashboard.Draw();
                                    Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_CURSOR");
                                    Natives.Graphics.PushScaleformMovieParameterFloat(Utils.GetControlNormal(0, 239));
                                    Natives.Graphics.PushScaleformMovieParameterFloat(Utils.GetControlNormal(0, 240));
                                    Natives.Graphics.PopScaleformMovieFunctionVoid();
                                    if (Utils.IsDisabledControlJustPressed(0, 24))
                                    {
                                        Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_INPUT_EVENT_SELECT");
                                        ClickReturn = Natives.Graphics.PopScaleformMovieFunction();
                                    }
                                    else if (Utils.IsDisabledControlJustPressed(0, 25))
                                    {
                                        Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_INPUT_EVENT_BACK");
                                        Natives.Graphics.PopScaleformMovieFunctionVoid();
                                    }
                                }
                            }*/
                            #endregion
                            Supervisor.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                            Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (DialogWithSupervisorIndex < DialogWithSupervisor.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSupervisor[DialogWithSupervisorIndex]);
                                    DialogWithSupervisorIndex++;
                                }
                                if (DialogWithSupervisorIndex == DialogWithSupervisor.Count)
                                {
                                    SpawnBlip.Delete();
                                    SpawnBlip = new Blip(Bomb.Position, 10f);
                                    SpawnBlip.Color = Color.Red;
                                    SpawnBlip.Alpha = 0.5f;
                                    TabletPlayer = new Rage.Object("prop_cs_tablet", new Vector3(0f, 0f, 0f));
                                    TabletPlayer.IsPersistent = true;
                                    AllCalloutEntities.Add(TabletPlayer);
                                    AllCalloutObjects.Add(TabletPlayer);
                                    TabletPlayer.AttachTo(Game.LocalPlayer.Character, 61, new Vector3(2.3720786e-05f, 0.00895381533f, 1.40741467e-05f), new Rotator(-1.87982096e-07f, -97.9999847f, 1.07260566e-05f));
                                    Game.LocalPlayer.Character.Tasks.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_c", 1f, AnimationFlags.Loop);
                                    GameFiber.Sleep(2500);
                                    DisplayStuff();
                                    //ControlBombCar();
                                    SpokenWithSupervisor = true;
                                }
                            }
                        }
                        if (RobotActive && !BombExplode)
                        {
                            if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
                            {
                                Game.LocalPlayer.Character.WarpIntoVehicle(BomCar, -1);
                            }
                            if (Utils.GetCamViewMode() != 4)
                            {
                                Utils.SetCamViewMode(4);
                                Game.FadeScreenIn(1500, true);
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(Bomb) <= 4f)
                            {
                                Game.DisplayHelp("Press ~y~" + Settings.DialogKey + "~s~ to destroy the bag.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    DisplayStuff();
                                    //BombExplode = true;
                                }
                            }
                        }
                        if (BombExplode)
                        {
                            RobotActive = false;
                            HackingGamebool = false;
                            if (BomCar.Exists())
                            {
                                Clone.Delete();
                                NativeFunction.CallByName<uint>("CLEAR_TIMECYCLE_MODIFIER");
                                Game.LocalPlayer.Character.IsVisible = true;
                                Game.LocalPlayer.Character.Position = CloneLoc.Item1;
                                Game.LocalPlayer.Character.Heading = CloneLoc.Item2;
                                while (Utils.GetCamViewMode() != 2)
                                {
                                    GameFiber.Yield();
                                    Utils.SetCamViewMode(2);
                                    Game.LogTrivial("Set View Mode Car");
                                }
                                /*while (Utils.GetCamViewModeOnFoot() != 2)
                                {
                                    GameFiber.Yield();
                                    Utils.SetCamViewModeOnFoot(2);
                                    Game.LogTrivial("Set View Mode Foot");
                                }*/
                                BomCar.Delete();
                            }
                            GameFiber.Sleep(3000);
                            World.SpawnExplosion(Bomb.Position, 9, 10f, true, false, 4f);
                            this.End();
                        }
                        if (BombDefused)
                        {
                            RobotActive = false;
                            HackingGamebool = false;
                            if (BomCar.Exists())
                            {
                                Clone.Delete();
                                NativeFunction.CallByName<uint>("CLEAR_TIMECYCLE_MODIFIER");
                                Game.LocalPlayer.Character.IsVisible = true;
                                Game.LocalPlayer.Character.Position = CloneLoc.Item1;
                                Game.LocalPlayer.Character.Heading = CloneLoc.Item2;
                                while (Utils.GetCamViewMode() != 2)
                                {
                                    GameFiber.Yield();
                                    Utils.SetCamViewMode(2);
                                    Game.LogTrivial("Set View Mode Car");
                                }
                                /*while (Utils.GetCamViewModeOnFoot() != 2)
                                {
                                    GameFiber.Yield();
                                    Utils.SetCamViewModeOnFoot(2);
                                    Game.LogTrivial("Set View Mode Foot");
                                }*/
                                BomCar.Delete();
                            }
                            Game.DisplaySubtitle("~g~Supervisor:~s~ Outstanding job! Well done!");
                            this.End();
                        }
                    }
                });
            }
            catch(Exception e)
            {
                Game.LogTrivial(e.ToString() + " Callout Crashed");
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
        private void SpawnVehicles()
        {
                if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
                {
                    UltBackupSpawn();
                }
                else
                {
                for (int i = 0; i < POLICECarLocations.Count; i++)
                {
                    Vehicle car = new Vehicle(CarModels[new Random().Next(CarModels.Length)], POLICECarLocations[i], POLICECarHeadings[i]);
                    car.IsPersistent = true;
                    car.IsSirenOn = true;
                    car.IsSirenSilent = true;
                    car.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                    Ped p = new Ped("s_m_y_cop_01", car.LeftPosition, Bomb.Position.X);
                    p.IsPersistent = true;
                    p.BlockPermanentEvents = true;
                    p.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                    Ped p2 = new Ped("s_m_y_cop_01", car.RightPosition, Bomb.Position.X);
                    p2.IsPersistent = true;
                    p2.BlockPermanentEvents = true;
                    p2.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                    AllCalloutEntities.Add(p);
                    AllCalloutEntities.Add(p2);
                    AllCalloutEntities.Add(car);
                }
                }
        }
        private void SpawnAllEntities()
        {
            #region Objects
            Bomb = new Rage.Object("bkr_prop_duffel_bag_01a", BombLoc.Item1, BombLoc.Item2);
            Bomb.IsPersistent = true;
            Bomb.IsPositionFrozen = true;
            AllCalloutEntities.Add(Bomb);
            AllCalloutObjects.Add(Bomb);

            for (int i = 0; i < BarrierSpawns.Count; i++)
            {
                Rage.Object barrier = new Rage.Object("prop_barrier_work05", BarrierSpawns[i].Item1, BarrierSpawns[i].Item2);
                barrier.IsPersistent = true;
                barrier.IsPositionFrozen = true;
                AllCalloutEntities.Add(barrier);
                AllCalloutObjects.Add(barrier);
            }
            #endregion
            #region PoliceRiot
            Vehicle PoliceRiot = new Vehicle("riot", PoliceRiotLoc.Item1, PoliceRiotLoc.Item2);
            PoliceRiot.IsPersistent = true;
            PoliceRiot.IsSirenOn = true;
            PoliceRiot.IsSirenSilent = true;
            PoliceRiot.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            AllCalloutEntities.Add(PoliceRiot);
            #endregion
            #region Ambulance
            if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
            {
                UltBackupSpawnAmb();
            }
            else
            {
                Vehicle Ambulance = new Vehicle("ambulance", AmbulanceLoc.Item1, AmbulanceLoc.Item2);
                Ambulance.IsPersistent = true;
                Ambulance.IsSirenOn = true;
                Ambulance.IsSirenSilent = true;
                Ambulance.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AllCalloutEntities.Add(Ambulance);
                Ped p = new Ped("s_m_m_paramedic_01", new Vector3(AmbulanceLoc.Item1.X - 1.5f, AmbulanceLoc.Item1.Y, AmbulanceLoc.Item1.Z), Bomb.Position.X);
                p.IsPersistent = true;
                p.BlockPermanentEvents = true;
                p.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                Ped p2 = new Ped("s_m_m_paramedic_01", new Vector3(AmbulanceLoc.Item1.X + 1.5f, AmbulanceLoc.Item1.Y, AmbulanceLoc.Item1.Z), Bomb.Position.X);
                p2.IsPersistent = true;
                p2.BlockPermanentEvents = true;
                p2.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                AllCalloutEntities.Add(p);
                AllCalloutEntities.Add(p2);
            }
            #endregion
            #region FireTruck
                Vehicle FireTruck = new Vehicle("firetruk", FireTrukLoc.Item1, FireTrukLoc.Item2);
                FireTruck.IsPersistent = true;
                FireTruck.IsSirenOn = true;
                FireTruck.IsSirenSilent = true;
                FireTruck.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                AllCalloutEntities.Add(FireTruck);
                Ped pe = new Ped("s_m_y_fireman_01", new Vector3(FireTrukLoc.Item1.X - 1.5f, FireTrukLoc.Item1.Y, FireTrukLoc.Item1.Z), Bomb.Position.X);
                pe.IsPersistent = true;
                pe.BlockPermanentEvents = true;
                pe.Inventory.GiveNewWeapon(WeaponHash.FireExtinguisher, 10000, true);
                Ped pe2 = new Ped("s_m_y_fireman_01", new Vector3(FireTrukLoc.Item1.X + 1.5f, FireTrukLoc.Item1.Y, FireTrukLoc.Item1.Z), Bomb.Position.X);
                pe2.IsPersistent = true;
                pe2.BlockPermanentEvents = true;
                pe2.Inventory.GiveNewWeapon(WeaponHash.FireExtinguisher, 10000, true);
                AllCalloutEntities.Add(pe);
                AllCalloutEntities.Add(pe2);
            #endregion
            #region transporter
            Transporter = new Vehicle("mule", TransporterLoc.Item1, TransporterLoc.Item2);
            Transporter.IsPersistent = true;
            Transporter.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            AllCalloutEntities.Add(Transporter);
            Transporter.SetLivery(-1);
            Transporter.VehicleExtra(1, 0);
            Transporter.VehicleExtra(2, 1);
            Transporter.VehicleExtra(3, 1);
            Transporter.VehicleExtra(4, 1);
            Transporter.VehicleExtra(5, 1);
            Transporter.VehicleExtra(6, 1);
            Transporter.VehicleExtra(7, 1);
            Ped t = Transporter.CreateRandomDriver();
            t.IsPersistent = true;
            t.BlockPermanentEvents = true;
            AllCalloutEntities.Add(t);
            #endregion
            #region Peds
            if (Utils.IsLSPDFRPluginRunning("UltimateBackup"))
            {
                UltBackupSpawnSuper();
            }
            else
            {
                Supervisor = new Ped("s_m_y_swat_01", SuperVisorLoc.Item1, SuperVisorLoc.Item2);
                Supervisor.IsPersistent = true;
                Supervisor.BlockPermanentEvents = true;
                AllCalloutEntities.Add(Supervisor);
            }
            #endregion
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
            HackingGamebool = false;
            if (BomCar.Exists())
            {
                if (Clone.Exists()) Clone.Delete();
                NativeFunction.CallByName<uint>("CLEAR_TIMECYCLE_MODIFIER");
                Game.LocalPlayer.Character.IsVisible = true;
                Utils.SetCamViewMode(viewmode);
                Utils.SetCamViewModeOnFoot(viewmodeonfoot);
                BomCar.Delete();
            }
            if (TabletPlayer.Exists())
            {
                TabletPlayer.Delete();
            }
            foreach (Entity e in AllCalloutEntities)
            {
                if (e.Exists()) e.Dismiss();
            }
            foreach (Rage.Object e in AllCalloutObjects)
            {
                if (e.Exists()) e.Delete();
            }
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            Game.LocalPlayer.Character.IsPositionFrozen = false;
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            Dispose(dashboard);

            base.End();
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
                        if (AllCalloutEntities.Contains(veh))
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
                                        if (!AllCalloutEntities.Contains(veh))
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
        private void ControlBombCar()
        {
            viewmode = Utils.GetCamViewMode();
            viewmodeonfoot = Utils.GetCamViewModeOnFoot();
            CloneLoc = new Tuple<Vector3, float>(Game.LocalPlayer.Character.Position, Game.LocalPlayer.Character.Heading);
            Game.FadeScreenOut(1500, true);
            HackingGamebool = false;
            IsInRobot = true;
            Game.LocalPlayer.Character.Tasks.ClearImmediately();
            if (TabletPlayer.Exists())
            {
                TabletPlayer.Delete();
            }
            Clone = Game.LocalPlayer.Character.Clone();
            Clone.IsPersistent = true;
            Clone.BlockPermanentEvents = true;
            Clone.IsPositionFrozen = true;
            AllCalloutEntities.Add(Clone);
            NativeFunction.Natives.SET_TIMECYCLE_MODIFIER("NG_filmic03");
            BomCar = new Vehicle("airtug", BomcarLoc.Item1, Transporter.Heading + 180f);
            BomCar.IsPersistent = true;
            BomCar.IsVisible = false;
            BomCar.IsInvincible = true;
            AllCalloutEntities.Add(BomCar);
            Game.LocalPlayer.Character.Heading = BomCar.Heading;
            //cam.Face(BomCar.FrontPosition);
            Game.LocalPlayer.Character.WarpIntoVehicle(BomCar, -1);
            Game.LocalPlayer.Character.IsVisible = false;
            Clone.Position = CloneLoc.Item1;
            Clone.Heading = CloneLoc.Item2;
            Rage.Object Tablet = new Rage.Object("prop_cs_tablet", new Vector3(0f, 0f, 0f));
            Tablet.IsPersistent = true;
            AllCalloutEntities.Add(Tablet);
            AllCalloutObjects.Add(Tablet);
            Tablet.AttachTo(Clone, 61, new Vector3(2.3720786e-05f, 0.00895381533f, 1.40741467e-05f), new Rotator(-1.87982096e-07f, -97.9999847f, 1.07260566e-05f));
            Clone.Tasks.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_c", 1f, AnimationFlags.Loop);
            Transporter.VehicleDoorOpen(Utils.VehDoorID.RearLeft, false, false);
            Transporter.VehicleDoorOpen(Utils.VehDoorID.RearRight, false, false);
            Utils.SetCamViewMode(4);
            /*while (!Natives.Graphics.HasScaleformMovieLoaded(_dashboard.Handle))
            {
                GameFiber.Yield();
            }*/
            GameFiber.Sleep(2000);
            RobotActive = true;
            /*cam.Delete();
            GameFiber.Sleep(2000);
            NativeFunction.CallByName<uint>("CLEAR_TIMECYCLE_MODIFIER");
            Game.FadeScreenIn(1500, true);*/
        }
        private void UltBackupSpawn()
        {
            for (int i = 0; i < POLICECarLocations.Count; i++)
            {
                Vehicle car = new Vehicle(CarModels[new Random().Next(CarModels.Length)], POLICECarLocations[i], POLICECarHeadings[i]);
                car.IsPersistent = true;
                car.IsSirenOn = true;
                car.IsSirenSilent = true;
                car.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                car.Position = POLICECarLocations[i];
                Ped p = UltimateBackup.API.Functions.getLocalPatrolPed(car.LeftPosition, Bomb.Position.X); //new Ped("s_m_y_cop_01", car.LeftPosition, Bomb.Position.X);
                p.IsPersistent = true;
                p.BlockPermanentEvents = true;
                p.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                Ped p2 = UltimateBackup.API.Functions.getLocalPatrolPed(car.RightPosition, Bomb.Position.X);
                p2.IsPersistent = true;
                p2.BlockPermanentEvents = true;
                p2.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                AllCalloutEntities.Add(p);
                AllCalloutEntities.Add(p2);
                AllCalloutEntities.Add(car);
            }
        }
        private void UltBackupSpawnAmb()
        {
            Vehicle Ambulance = new Vehicle("ambulance", AmbulanceLoc.Item1, AmbulanceLoc.Item2);
            Ambulance.IsPersistent = true;
            Ambulance.IsSirenOn = true;
            Ambulance.IsSirenSilent = true;
            Ambulance.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            AllCalloutEntities.Add(Ambulance);
            Game.LogTrivial("Crashtest");
            Ped p = UltimateBackup.API.Functions.getAmbulancePed(new Vector3(AmbulanceLoc.Item1.X - 2.5f, AmbulanceLoc.Item1.Y, AmbulanceLoc.Item1.Z), Bomb.Position.X); //new Ped("s_m_y_cop_01", car.LeftPosition, Bomb.Position.X);
            p.IsPersistent = true;
            p.BlockPermanentEvents = true;
            p.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
            Ped p2 = UltimateBackup.API.Functions.getAmbulancePed(new Vector3(AmbulanceLoc.Item1.X + 2.5f, AmbulanceLoc.Item1.Y, AmbulanceLoc.Item1.Z), Bomb.Position.X);
            p2.IsPersistent = true;
            p2.BlockPermanentEvents = true;
            p2.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
            AllCalloutEntities.Add(p);
            AllCalloutEntities.Add(p2);
        }
        private void UltBackupSpawnSuper()
        {
            Supervisor = UltimateBackup.API.Functions.getLocalPatrolPed(SuperVisorLoc.Item1, SuperVisorLoc.Item2);
            Supervisor.IsPersistent = true;
            Supervisor.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Supervisor);
        }

        private void HackingGame()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Natives.Graphics.HasScaleformMovieLoaded(dashboard.Handle) && HackingGamebool)
                {
                    Game.LocalPlayer.Character.IsPositionFrozen = true;
                    Utils.DisableControlAction(0, 24, true);
                    Utils.DisableControlAction(0, 25, true);
                    if (Natives.Graphics.GetScaleformMovieFunctionReturnBool(ClickReturn))
                    {
                        int programid = Natives.Graphics.GetScaleformMovieFunctionReturnInt(ClickReturn);
                        Game.LogTrivial(programid.ToString());
                        if (programid == 82 && !ClickedConnect)
                        {
                            ConnectToRobot();
                        }
                        else if (programid == 83 && IsInRobot && !ClickedGame)
                        {
                            PlayGame();
                        }
                        else if (programid == 87)
                        {
                            lives -= 1;
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_ROULETTE_WORD");
                            Natives.Graphics.PushScaleformMovieParameterString("BDEFUSED");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LIVES");
                            Natives.Graphics.PushScaleformMovieParameterInt(lives);
                            Natives.Graphics.PushScaleformMovieParameterInt(5);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                        }
                        else if (programid == 92)
                        {
                        }
                        else if (programid == 86)
                        {
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_ROULETTE_OUTCOME");
                            Natives.Graphics.PushScaleformMovieParameterBool(true);
                            Natives.Graphics.PushScaleformMovieParameterString("DEFUSAL SUCCESSFUL!");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(2800);
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "CLOSE_APP");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_LOADING_PROGRESS");
                            Natives.Graphics.PushScaleformMovieParameterBool(true);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_PROGRESS");
                            Natives.Graphics.PushScaleformMovieParameterInt(35);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_TIME");
                            Natives.Graphics.PushScaleformMovieParameterInt(35);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_MESSAGE");
                            Natives.Graphics.PushScaleformMovieParameterString("Bypassing security...");
                            Natives.Graphics.PushScaleformMovieParameterFloat(2.0f);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(2800);
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_MESSAGE");
                            Natives.Graphics.PushScaleformMovieParameterString("Overwriting data...");
                            Natives.Graphics.PushScaleformMovieParameterFloat(2.0f);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_TIME");
                            Natives.Graphics.PushScaleformMovieParameterInt(15);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_PROGRESS");
                            Natives.Graphics.PushScaleformMovieParameterInt(75);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(2800);
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_LOADING_PROGRESS");
                            Natives.Graphics.PushScaleformMovieParameterBool(false);
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_ERROR_POPUP");
                            Natives.Graphics.PushScaleformMovieParameterBool(true);
                            Natives.Graphics.PushScaleformMovieParameterString("BOMB SUCCESSFULLY DEFUSED, DEVICE SHUTTING DOWN");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(3500);
                            //Natives.Graphics.SetScaleformMovieAsNoLongerNeeded(dashboard.Handle);
                            //Natives.Graphics.PopScaleformMovieFunctionVoid();
                            Game.LocalPlayer.Character.IsPositionFrozen = false;
                            Utils.DisableControlAction(0, 24, false);
                            Utils.DisableControlAction(0, 25, false);
                            BombDefused = true;
                            HackingGamebool = false;
                        }
                        else if (programid == 6)
                        {
                            /*GameFiber.Sleep(500);
                            // Natives.Graphics.SetScaleformMovieAsNoLongerNeeded(dashboard.Handle);
                            //Natives.Graphics.PopScaleformMovieFunctionVoid();
                            Game.LocalPlayer.Character.IsPositionFrozen = false;
                            Utils.DisableControlAction(0, 24, false);
                            Utils.DisableControlAction(0, 25, false);
                            HackingGamebool = false;*/
                        }
                        if (lives == 0)
                        {
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_ROULETTE_OUTCOME");
                            Natives.Graphics.PushScaleformMovieParameterBool(false);
                            Natives.Graphics.PushScaleformMovieParameterString("DEFUSAL FAILED!");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(3500);
                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "CLOSE_APP");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();

                            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_ERROR_POPUP");
                            Natives.Graphics.PushScaleformMovieParameterBool(true);
                            Natives.Graphics.PushScaleformMovieParameterString("EXPLOSION IMMINENT, DEVICE SHUTTING DOWN");
                            Natives.Graphics.PopScaleformMovieFunctionVoid();
                            GameFiber.Sleep(3500);
                            //Natives.Graphics.SetScaleformMovieAsNoLongerNeeded(dashboard.Handle);
                            //Natives.Graphics.PopScaleformMovieFunctionVoid();
                            Game.LocalPlayer.Character.IsPositionFrozen = false;
                            Utils.DisableControlAction(0, 24, false);
                            Utils.DisableControlAction(0, 25, false);
                            BombExplode = true;
                            HackingGamebool = false;
                        }
                    }
                }

            });
        }

        private void Dispose(Scaleform t)
        {
            unsafe
            {
                int handle = t.Handle;
                Game.LogTrivial("Handle = " + handle.ToString());
                NativeFunction.CallByName<uint>("SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED", &handle);
                Game.LogTrivial("Handle freed");
            }
            t = null;
        }

        private void DisplayStuff()
        {
            SpokenWithSupervisor = true;
                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LABELS");
                                Natives.Graphics.PushScaleformMovieParameterString("Local Disk (C:)");
                                Natives.Graphics.PushScaleformMovieParameterString("Network");
                                Natives.Graphics.PushScaleformMovieParameterString("Remote Control");
                                Natives.Graphics.PushScaleformMovieParameterString("ControlRobot.exe");
                                Natives.Graphics.PushScaleformMovieParameterString("BombDefusal.exe");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_BACKGROUND");
                                Natives.Graphics.PushScaleformMovieParameterInt(3);
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "ADD_PROGRAM");
                                Natives.Graphics.PushScaleformMovieParameterFloat(1.0f);
                                Natives.Graphics.PushScaleformMovieParameterFloat(4.0f);
                                Natives.Graphics.PushScaleformMovieParameterString("Bomb Defusal Robot");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "ADD_PROGRAM");
                                Natives.Graphics.PushScaleformMovieParameterFloat(6.0f);
                                Natives.Graphics.PushScaleformMovieParameterFloat(6.0f);
                                Natives.Graphics.PushScaleformMovieParameterString("Power Off");
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(0);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(150, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(1);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(160, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(2);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(170, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(3);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(180, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(4);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(190, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(5);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(200, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(6);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(210, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();

                                Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_COLUMN_SPEED");
                                Natives.Graphics.PushScaleformMovieParameterInt(7);
                                Natives.Graphics.PushScaleformMovieParameterInt(new Random().Next(245, 255));
                                Natives.Graphics.PopScaleformMovieFunctionVoid();
                                HackingGamebool = true;

                                while (HackingGamebool)
                                {
                                    GameFiber.Yield();
                                    HackingGame();
                                    dashboard.Draw();
                                    Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_CURSOR");
                                    Natives.Graphics.PushScaleformMovieParameterFloat(Utils.GetControlNormal(0, 239));
                                    Natives.Graphics.PushScaleformMovieParameterFloat(Utils.GetControlNormal(0, 240));
                                    Natives.Graphics.PopScaleformMovieFunctionVoid();
                                    if (Utils.IsDisabledControlJustPressed(0, 24))
                                    {
                                        Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_INPUT_EVENT_SELECT");
                                        ClickReturn = Natives.Graphics.PopScaleformMovieFunction();
                                    }
                                    else if (Utils.IsDisabledControlJustPressed(0, 25))
                                    {
                                        Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_INPUT_EVENT_BACK");
                                        Natives.Graphics.PopScaleformMovieFunctionVoid();
                                    }
                                }
        }

        private void ConnectToRobot()
        {
            ClickedConnect = true;
            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_LOADING_PROGRESS");
            Natives.Graphics.PushScaleformMovieParameterBool(true);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_PROGRESS");
            Natives.Graphics.PushScaleformMovieParameterInt(35);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_TIME");
            Natives.Graphics.PushScaleformMovieParameterInt(35);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_MESSAGE");
            Natives.Graphics.PushScaleformMovieParameterString("Starting remote connection...");
            Natives.Graphics.PushScaleformMovieParameterFloat(2.0f);
            Natives.Graphics.PopScaleformMovieFunctionVoid();
            GameFiber.Sleep(2800);
            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_MESSAGE");
            Natives.Graphics.PushScaleformMovieParameterString("Connecting...");
            Natives.Graphics.PushScaleformMovieParameterFloat(2.0f);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_TIME");
            Natives.Graphics.PushScaleformMovieParameterInt(15);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_LOADING_PROGRESS");
            Natives.Graphics.PushScaleformMovieParameterInt(75);
            Natives.Graphics.PopScaleformMovieFunctionVoid();
            GameFiber.Sleep(2800);
            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "OPEN_LOADING_PROGRESS");
            Natives.Graphics.PushScaleformMovieParameterBool(false);
            Natives.Graphics.PopScaleformMovieFunctionVoid();
            Game.LocalPlayer.Character.IsPositionFrozen = false;
            Utils.DisableControlAction(0, 24, false);
            Utils.DisableControlAction(0, 25, false);
            ControlBombCar();
        }

        private void PlayGame()
        {
            ClickedGame = true;
            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "RUN_PROGRAM");
            Natives.Graphics.PushScaleformMovieParameterFloat(83.0f);
            Natives.Graphics.PopScaleformMovieFunctionVoid();

            Natives.Graphics.PushScaleformMovieFunction(dashboard.Handle, "SET_ROULETTE_WORD");
            Natives.Graphics.PushScaleformMovieParameterString("BDEFUSED");
            Natives.Graphics.PopScaleformMovieFunctionVoid();
        }

    }

}
