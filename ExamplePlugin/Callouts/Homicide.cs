using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using RegularCallouts.Stuff;
using System.Collections.Generic;
using System.Linq;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Homicide", CalloutProbability.Medium)]
    public class Homicide : Callout
    {
        private Vector3 SpawnPoint;
        private Blip SpawnBlip;
        private bool MainScreenShown;
        private Vector3 SpawnPoint1;
        private Vector3 EyeWitnessLocation;
        private float EyeWitnessHeading;
        private bool CheckedWeapon;
        private Vector3 DeadBodyLocation;
        private float DeadBodyHeading;
        private List<Entity> AllBankHeistEntities = new List<Entity>();
        private Ped EyeWitness;
        private Ped GunshotsWitness;
        private Rage.Object GunShells;
        private Rage.Object Bin;
        private Rage.Object Gun;
        private bool SpokenWithSoundWitness;
        private int Blood;
        private int IntroSpeech = 0;
        List<string> IntroQuestions = new List<string>() { "You: Do you know the victim?" };
        private Vector3 BinLocation;
        private Vector3 GunLocation;
        private Rotator GunRotation;
        private Ped Murderer;
        private float BinHeading;
        private Vector3 GunShotWitnessLocation;
        private bool Blipped;
        private float GunShotWitnessHeading;
        private Ped DeadBody;
        private bool ChaseStarted;
        private Vehicle CopCar;
        private Vector3 CopCarLocation;
        private bool GunFound;
        private float CopCarHeading;
        private static string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private static string[] EyeWitnessModels = new string[] { "a_f_m_bevhills_01", "a_f_y_bevhills_01", "a_f_m_bevhills_02", "a_f_y_bevhills_02", "a_f_y_bevhills_03", "a_f_y_bevhills_04" };
        private static string[] CarModels = new string[] { "POLICE", "POLICE2", "POLICE3", "POLICE4" };
        private List<Rage.Object> AllSpawnedObjects = new List<Rage.Object>();
        private List<Vehicle> AllSpawnedPoliceVehicles = new List<Vehicle>();
        private List<Ped> AllSpawnedPeds = new List<Ped>();
        private List<Vector3> PolicePedLocations;
        private List<Vector3> ShellLocations;
        private bool GunShellsFound;
        private List<float> PolicePedHeadings;
        private List<string> DialogWithSoundWitness = new List<string>
        {
            "~b~You:~s~ Hello Sir. Can you tell me what you saw? (1/5)",
            "~y~Witness:~s~ I was just around the corner when I heard the shots. I ran around and saw that poor guy on the ground. (2/5)",
            "~b~You:~s~ Did you saw anyone? (3/5)",
            "~y~Witness:~s~ I saw a young girl run into the store over there. (4/5)",
            "~b~You:~s~ Ok thank you for the information. (5/5)"
        };
        private int DialogWithSoundWitnessIndex;
        private List<string> DialogWithEyeWitnessIntro = new List<string>
        {
            "~b~You:~s~ Hello Ma'am. Can you tell me what happened? (1/6)",
            "~y~Witness:~s~ I came back from my lunchbreak and was about to open the door when I heard the shots (2/6)",
            "~b~You:~s~ And then what? (3/6)",
            "~y~Witness:~s~ I saw the person on the ground bleeding. I ran inside and hid myself. (4/6)",
            "~b~You:~s~ Did you saw who shot him? (5/6)"
        };
        private int DialogWithEyeWitnessIntroIndex;
        private bool EyeWitnessIntroDone;
        private Blip MurdererBlip;
        private bool EyeWitnessName;
        private bool CalloutRunning;
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint1 = new Vector3(-1206.83948f, -782.614014f, 17.0920582f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                DeadBodyLocation = new Vector3(-1206.83948f, -782.614014f, 17.0920582f);
                DeadBodyHeading = -144.148056f;
                BinLocation = new Vector3(-1210.57886f, -782.318542f, 16.2600746f);
                BinHeading = 37.512249f;
                GunLocation = new Vector3(-1210.54895f, -782.406128f, 16.727684f);
                GunRotation = new Rotator(38.0006332f, 147.051498f, 1.08346367e-05f);
                EyeWitnessLocation = new Vector3(-1197.33044f, -773.605652f, 17.3257751f);
                EyeWitnessHeading = 148.284027f;
                GunShotWitnessLocation = new Vector3(-1203.40234f, -787.3172f, 16.9036884f);
                GunShotWitnessHeading = 37.764061f;
                PolicePedLocations = new List<Vector3>() { new Vector3(-1210.48779f, -778.887817f, 17.2582664f), new Vector3(-1204.55188f, -786.050171f, 16.9770584f) };
                ShellLocations = new List<Vector3>() { new Vector3(-1208.43091f, -782.208008f, 16.1900578f), new Vector3(-1208.50012f, -782.05188f, 16.1879482f), new Vector3(-1208.38733f, -782.11322f, 16.1878262f), new Vector3(-1208.42322f, -781.96637f, 16.1855183f), new Vector3(-1208.28394f, -782.114868f, 16.1867104f) };
                PolicePedHeadings = new List<float>() { 36.5511398f, -143.519348f };
                CopCarLocation = new Vector3(-1209.51477f, -786.654358f, 16.5974483f);
                CopCarHeading = 34.8847198f;
            }

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            CalloutMessage = "Homicide";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_DISTURBING_THE_PEACE IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            SpawnBlip = new Blip(SpawnPoint, 40f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            CalloutRunning = true;
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ We've got a civilian shot infront of the ~y~Store~s~. Respond Code 3");
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
                    SpawnEyeWitness();
                    GameFiber.Yield();
                    SpawnGunShotWitness();
                    GameFiber.Yield();
                    SpawnMurderer();
                    GameFiber.Yield();
                    SpawnDeadBody();
                    GameFiber.Yield();
                    SpawnCopCar();
                    GameFiber.Yield();
                    Blood = Utils.AddDecalOnGround(DeadBody.FrontPosition, Utils.DecalTypes.porousPool_blood, 2f, 2f, 0.196f, 0f, 0f, 1f, -1f);
                    GameFiber.Yield();
                    DeadBody.Position = DeadBody.RearPosition;
                    GameFiber.Yield();
                    SpawnProps();
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (Game.LocalPlayer.Character.DistanceTo(DeadBody) <= 15f && SpawnBlip.Exists())
                        {
                            Game.DisplayHelp("Investigate the scene");
                            Game.DisplaySubtitle("~g~Officer:~s~ Hey Officer! We've secured the scene for you. Take a look around.");
                            SpawnBlip.Delete();
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(DeadBody) <= 2f && MainScreenShown == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to search the body");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                Game.DisplaySubtitle("~b~You:~s~ His driver's license...");
                                Game.DisplayNotification("~b~Clue~s~ found!");
                                //Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly);
                                MainScreenShown = true;
                            }
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(GunShells) <= 1.25f && GunShellsFound == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to investigate the gunshells");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Utils.GroundSearchAnim(Game.LocalPlayer.Character);
                                Game.DisplaySubtitle("~b~You:~s~ 5 rounds. The suspect made sure he died.");
                                Game.DisplayNotification("~b~Clue~s~ found!");
                                GunShellsFound = true;
                            }
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Bin) <= 1.5f && GunFound == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to inspect the bin");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                Game.DisplaySubtitle("~b~You:~s~ A pistol? This could be the murder weapon...");
                                Game.DisplayNotification("~b~Clue~s~ found!");
                                Game.DisplayHelp("Press ~y~ " + Settings.InteractionKey + " ~s~ to inspect the weapon further");
                                Gun.Delete();
                                GunFound = true;
                            }
                        }
                        if (GunFound == true && CheckedWeapon == false)
                        {
                            if (Game.IsKeyDown(Settings.InteractionKey))
                            {
                                Rage.Object pistol = new Rage.Object("w_pi_combatpistol", new Vector3(0f, 0f, 0f));
                                int boneIndex = Game.LocalPlayer.Character.GetBoneIndex(PedBoneId.RightPhHand);
                                pistol.AttachTo(Game.LocalPlayer.Character, boneIndex, new Vector3(0.08f, 0.02f, 0.01f), new Rotator(0f, 0f, 0f));
                                Game.LocalPlayer.Character.Tasks.PlayAnimation("missmic4premiere", "pap_idle_action_02", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                pistol.Delete();
                                Functions.PlayPlayerRadioAction(Functions.GetPlayerRadioAction(), 4000);
                                Game.DisplaySubtitle("~b~You:~s~ Dispatch I need an owner for the following Gun Serial Number: 4EA49C7");
                                GameFiber.Sleep(3000);
                                Game.DisplayNotification("~b~Dispatch:~s~ The gun belongs to " + Functions.GetPersonaForPed(Murderer).FullName.ToString());
                                CheckedWeapon = true;
                            }
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(EyeWitness) <= 2f && EyeWitnessIntroDone == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to speak to the girl");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                EyeWitness.TurnToFaceEntity(Game.LocalPlayer.Character, -1);
                                if (DialogWithEyeWitnessIntroIndex < DialogWithEyeWitnessIntro.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithEyeWitnessIntro[DialogWithEyeWitnessIntroIndex]);
                                    DialogWithEyeWitnessIntroIndex++;
                                }
                                if (DialogWithEyeWitnessIntroIndex == DialogWithEyeWitnessIntro.Count)
                                {
                                    EyeWitnessIntroDone = true;
                                }
                            }

                        }
                        if (Game.LocalPlayer.Character.DistanceTo(EyeWitness) <= 2f && EyeWitnessIntroDone == true && EyeWitnessName == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to speak to the girl");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                Game.DisplaySubtitle("~y~Witness:~s~ It was my Boss, " + Functions.GetPersonaForPed(Murderer).FullName.ToString() + " (6/6)");
                                EyeWitnessName = true;
                            }

                        }
                        if (Blipped == false && EyeWitnessName && CheckedWeapon)
                        {
                            Game.DisplayNotification("~b~Attention to Unit:~s~ We've found the gunowner. The location is marked on your GPS");
                            MurdererBlip = Murderer.AttachBlip();
                            MurdererBlip.Color = Color.Red;
                            Blipped = true;
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(Murderer) <= 10f && Game.LocalPlayer.Character.IsOnFoot && ChaseStarted == false && Blipped == true)
                        {
                            LHandle Pursuit = Functions.CreatePursuit();
                            Functions.AddPedToPursuit(Pursuit, Murderer);
                            Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                            ChaseStarted = true;
                        }
                        if (Murderer.IsDead || Murderer.IsCuffed || Functions.IsPedArrested(Murderer))
                        {
                            this.End();
                        }
                        if (Game.LocalPlayer.Character.DistanceTo(GunshotsWitness) <= 2f && SpokenWithSoundWitness == false)
                        {
                            Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to speak to the person");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                GunshotsWitness.TurnToFaceEntity(Game.LocalPlayer.Character);
                                if (DialogWithSoundWitnessIndex < DialogWithSoundWitness.Count)
                                {
                                    Game.DisplaySubtitle(DialogWithSoundWitness[DialogWithSoundWitnessIndex]);
                                    DialogWithSoundWitnessIndex++;
                                }
                                if (DialogWithSoundWitnessIndex == DialogWithSoundWitness.Count)
                                {
                                    Game.DisplayNotification("~b~Clue~s~ found!");
                                    SpokenWithSoundWitness = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Game.LogTrivial(e.ToString() + " Callout Crashed");
                    this.End();
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
            }
            Utils.DeleteDecal(Blood);
            foreach (Ped i in AllSpawnedPeds)
            {
                if (i.Exists())
                {
                    i.Dismiss();
                }
            }
            foreach (Rage.Object i in AllSpawnedObjects)
            {
                if (i.Exists())
                {
                    i.Delete();
                }
            }
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (MurdererBlip.Exists()) { MurdererBlip.Delete(); }
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

        private void TruthAnswer(Ped p)
        {

        }
        private void PressAnswer(Ped p)
        {

        }
        private void LieAnswer(Ped p, bool activate)
        {
            if (activate)
            {
                GameFiber.StartNew(delegate
                {
                    GameFiber.Yield();
                Found:
                    GameFiber.Sleep(new Random().Next(1000, 5000));
                    p.Tasks.PlayAnimation("friends@frm@ig_1", "impatient_idle_a", 1f, AnimationFlags.Loop);
                    GameFiber.Sleep(new Random().Next(5000, 10000));
                    p.Tasks.Clear();
                    GameFiber.Sleep(new Random().Next(5000, 10000));
                    goto Found;
                });
            }
            else
            {
                p.Tasks.Clear();
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

        private void LoadModels()
        {
            foreach (string s in LSPDModels)
            {
                GameFiber.Yield();
                new Model(s).Load();
            }
        }

        private void SpawnEyeWitness()
        {
            EyeWitness = new Ped(new Model(EyeWitnessModels[new Random().Next(EyeWitnessModels.Length)]), EyeWitnessLocation, EyeWitnessHeading);
            EyeWitness.IsPersistent = true;
            EyeWitness.BlockPermanentEvents = true;
            AllSpawnedPeds.Add(EyeWitness);
            AllBankHeistEntities.Add(EyeWitness);
        }
        private void SpawnGunShotWitness()
        {
            GunshotsWitness = new Ped(GunShotWitnessLocation, GunShotWitnessHeading);
            GunshotsWitness.IsPersistent = true;
            GunshotsWitness.BlockPermanentEvents = true;
            GunshotsWitness.Tasks.PlayAnimation("clothingtie", "check_out_b", 1f, AnimationFlags.Loop);
            AllSpawnedPeds.Add(GunshotsWitness);
            AllBankHeistEntities.Add(GunshotsWitness);
        }
        private void SpawnDeadBody()
        {
            DeadBody = new Ped(DeadBodyLocation, DeadBodyHeading);
            DeadBody.IsPersistent = true;
            DeadBody.BlockPermanentEvents = true;
            DeadBody.Kill();
            DeadBody.ApplyDamagePack("Explosion_Large", 1f, 1f);
            AllSpawnedPeds.Add(DeadBody);
            AllBankHeistEntities.Add(DeadBody);
        }
        private void SpawnMurderer()
        {
            Murderer = new Ped(World.GetNextPositionOnStreet(SpawnPoint.Around(10f)));
            Murderer.IsPersistent = true;
            Murderer.BlockPermanentEvents = true;
            Murderer.Tasks.Wander();
            AllSpawnedPeds.Add(Murderer);
            AllBankHeistEntities.Add(Murderer);
        }
        private void SpawnCopCar()
        {
            CopCar = new Vehicle(CarModels[new Random().Next(CarModels.Length)], CopCarLocation, CopCarHeading);
            CopCar.IsPersistent = true;
            CopCar.IsSirenOn = true;
            CopCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            AllSpawnedPoliceVehicles.Add(CopCar);
            AllBankHeistEntities.Add(CopCar);
        }
        private void SpawnProps()
        {
            Bin = new Rage.Object("ch_prop_casino_bin_01a", BinLocation, BinHeading);
            Bin.IsPersistent = true;
            Bin.IsPositionFrozen = true;
            AllSpawnedObjects.Add(Bin);
            AllBankHeistEntities.Add(Bin);
            Gun = new Rage.Object("w_pi_combatpistol", GunLocation);
            Gun.IsPositionFrozen = true;
            Gun.Position = GunLocation;
            Gun.Rotation = GunRotation;
            Gun.IsPersistent = true;
            AllSpawnedObjects.Add(Gun);
            AllBankHeistEntities.Add(Gun);
            for (int i = 0; i < ShellLocations.Count; i++)
            {
                GunShells = new Rage.Object("w_pi_flaregun_shell", ShellLocations[i]);
                //GunShells.Heading = new Random().Next(0, 359);
                GunShells.IsPersistent = true;
                GunShells.IsPositionFrozen = true;
                GunShells.SetRotationYaw(Utils.NextFloat(1f, 349f));
                AllSpawnedObjects.Add(GunShells);
                AllBankHeistEntities.Add(GunShells);
            }
        }
        private void SpawnAllPolicePeds()
        {
            for (int i = 0; i < PolicePedLocations.Count; i++)
            {
                if(Utils.IsLSPDFRPluginRunning("UltimateBackup"))
                {
                    Ped officer = UltimateBackup.API.Functions.getLocalPatrolPed(PolicePedLocations[i], PolicePedHeadings[i]);
                    officer.IsPersistent = true;
                    officer.BlockPermanentEvents = true;
                    officer.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                    AllSpawnedPeds.Add(officer);
                    AllBankHeistEntities.Add(officer);
                }
                else
                {
                    Ped officer = new Ped(new Model(LSPDModels[new Random().Next(LSPDModels.Length)]), PolicePedLocations[i], PolicePedHeadings[i]);
                    officer.IsPersistent = true;
                    officer.BlockPermanentEvents = true;
                    officer.Tasks.PlayAnimation("amb@world_human_stand_impatient@male@no_sign@idle_a", "idle_a", 1f, AnimationFlags.Loop);
                    AllSpawnedPeds.Add(officer);
                    AllBankHeistEntities.Add(officer);
                }
            }
        }
    }
}
