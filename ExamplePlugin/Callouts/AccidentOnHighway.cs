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

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Accident on Highway", CalloutProbability.Medium)]
    public class AccidentOnHighway : Callout
    {
        private Ped Driver1;
        private Ped Driver2;
        private Ped Driver3;
        private Ped Driver4;
        private Ped Driver5;
        private Ped Cop1;
        private Ped Cop2;
        private Ped Medic1;
        private Ped Medic2;
        private Ped Firef1;
        private Ped Firef2;
        bool Feuerwehr1steigtein;
        bool Feuerwehr1steigtein3;
        private Ped Firef3;
        private Ped Firef4;
        private Ped Firef5;
        private Ped Firef6;
        private Vector3 SpawnPoint;
        private Blip SpawnBlip;
        private Blip SupervisorBlip;
        private Blip Medic1Blip;
        private Blip Victim2Blip;
        private Blip Fire5Blip;
        bool AufDemWeg;
        private Vehicle Rhino1;
        private Vehicle Rhino2;
        private Vehicle Rhino3;
        private Vehicle Auto1;
        private Vehicle Auto2;
        private Vehicle Auto3;
        private Vehicle Auto4;
        private Vehicle Feuerwehr1;
        private Vehicle Polizei;
        private Vehicle Krankenwagen;
        private Vehicle Feuerwehr2;
        private Rage.Object TrafficCone1;
        private Rage.Object TrafficCone2;
        private Rage.Object TrafficCone3;
        private Rage.Object TrafficCone4;
        private Rage.Object TrafficWand;
        bool AfterCall;
        bool BlockadeGespawnt;
        bool AutosGespawnt;
        bool Feuerwehr1steigtein2;
        bool MitLeutenSprechen;
        bool MitSupervisorGesprochen;
        bool MitFire5Geredet;
        bool MitVictim2geredet;
        bool MitMedicgeredet1;
        bool MitMedicgeredet2;
        bool MitMedicgeredet3;
        bool MitMedicgeredet4;
        private Vector3 MedicHelp1;
        private Vector3 MedicHelp2;
        private Vector3 VictimHelp1;
        private Vector3 VictimHelp4;
        private bool MitCop2geredet;

        string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "INFERNUS", "ALPHA", "SURGE" };

        private List<string> dialogwithFire5 = new List<string>
        {
            "~g~Firefighter:~s~ Hello Officer, we already freed everyone. Two of them require medical assistance and one of them died on impact. (1/3)",
            "~g~Firefighter:~s~ Everything is secure from our side, we'll let you handle the cleanups. (2/3)",
            "~b~You:~s~ I'll take care of it. Thanks. (3/3)"
        };
        private int dialogwithfire5index;

        private List<string> dialogwithdriver2 = new List<string>
        {
            "~b~You:~s~ Good day to you. Can you tell me what happened? (1/4)",
            "~o~Driver:~s~ The car on the roof lost control and I hit it afterwards, when I tried to break the other cars drove into me. (2/4)",
            "~b~You:~s~ Could you please stay if I have further questions? (3/4)",
            "~o~Driver:~s~ Absolutely. (4/4)"
        };
        private int dialogwithdriver2index;

        private List<string> dialogwithcop2 = new List<string>
        {
            "~g~Officer:~s~ Hey good to see you here. As you can see we already secured the scene (1/4)",
            "~g~Officer:~s~ We still don't know what happened, you can talk to the victims and medics. (2/4)",
            "~g~Officer:~s~ When you are done you can tell the other officer to remove the roadblock. (3/4)",
            "~b~You:~s~ Alright thanks for the information (4/4)."
        };
        private int dialogwithcop2index;

        private List<string> dialogwithmedic = new List<string>
        {
            "~b~You:~s~ What information can you give me? (1/3)",
            "~g~Medic:~s~ This one died when we arrived, two of them have minor injures and the ones sitting on the ground need to go to the hospital. (2/3)",
            "~g~Medic:~s~ If you are done questioning them come back to me so we can transport them. (3/3)",
        };
        private int dialogwithmedicindex;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = new Vector3(750.059f, -50.3238678f, 58.8495979f);
            AddMinimumDistanceCheck(101f, SpawnPoint);
            AddMaximumDistanceCheck(1000f, SpawnPoint);
            CalloutMessage = "Major Accident on Highway";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {

                SpawnBlip = new Blip(SpawnPoint, 80f);
                SpawnBlip.Color = Color.Yellow;
                SpawnBlip.Alpha = 0.5f;
                SpawnBlip.EnableRoute(Color.Yellow);
                AufDemWeg = true;
                CalloutLos();
                return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            base.Process();
            GameFiber.StartNew(delegate
            {
            while (AufDemWeg)
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 500f && BlockadeGespawnt == false)
                {
                    SpawnBlockade();
                    BlockadeGespawnt = true;
                }
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 400f && AutosGespawnt == false)
                {
                    SpawnAutos();
                    AutosGespawnt = true;
                }
                if (Game.LocalPlayer.Character.Position.DistanceTo(SpawnPoint) <= 20f)
                {
                    SpawnBlip.DisableRoute();
                    SpawnBlip.Delete();
                    Game.DisplayHelp("Speak with the ~g~ Supervisor");
                    SupervisorBlip = Cop2.AttachBlip();
                    SupervisorBlip.Color = Color.Green;
                    GameFiber.Sleep(2000);
                    AfterCall = true;
                    break;

                }
            }
            while (AfterCall)
            {
                AufDemWeg = false;
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(Cop2) <= 2f && MitCop2geredet == false)
                {
                    Cop2.Tasks.Clear();
                    Cop2.TurnToFaceEntity(Game.LocalPlayer.Character);
                    Game.DisplayHelp("Press ~b~"+ Settings.DialogKey +"~s~ to speak to the Supervisor.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        if (dialogwithcop2index < dialogwithcop2.Count)
                        {
                            Game.DisplaySubtitle(dialogwithcop2[dialogwithcop2index]);
                            dialogwithcop2index++;
                        }
                        if (dialogwithcop2index == dialogwithcop2.Count)
                        {
                            SupervisorBlip.Delete();
                            Cop2.TurnToFaceEntity(Driver5);
                            MitCop2geredet = true;
                            MitSupervisorGesprochen = true;
                            break;
                        }
                    }
                }
            }
            while (MitSupervisorGesprochen)
                {
                    AfterCall = false;
                    GameFiber.Yield();
                    BlockadeFrei();
                    if (MitCop2geredet == true)
                    {
                        Medic1Blip = Medic2.AttachBlip();
                        Medic1Blip.Color = Color.Green;
                        Fire5Blip = Firef5.AttachBlip();
                        Fire5Blip.Color = Color.Green;
                        Victim2Blip = Driver2.AttachBlip();
                        Victim2Blip.Color = Color.Orange;
                        MitLeutenSprechen = true;
                    }
                    if (MitLeutenSprechen == true)
                    {
                        MitCop2geredet = false;
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Firef5) <= 2f && MitFire5Geredet == false)
                        {
                            Firef5.Tasks.Clear();
                            Firef5.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Fireman.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (dialogwithfire5index < dialogwithFire5.Count)
                                {
                                    Game.DisplaySubtitle(dialogwithFire5[dialogwithfire5index]);
                                    dialogwithfire5index++;
                                }
                            }
                        }
                        if (dialogwithfire5index == dialogwithFire5.Count && Feuerwehr1steigtein2 == false)
                        {
                            MitFire5Geredet = true;
                            Feuerwehr1steigtein = true;
                            if (Fire5Blip.Exists()) Fire5Blip.Delete();
                        }
                        while (Feuerwehr1steigtein)
                        {
                            FeuerwehrEnter1();
                            break;
                        }
                        while (Firef3.CurrentVehicle != null && Firef4.CurrentVehicle != null && Firef5.CurrentVehicle != null && Feuerwehr1steigtein3 == false)
                        {
                            Firef5.Tasks.CruiseWithVehicle(60f);
                            Feuerwehr1.IsSirenOn = false;
                            Feuerwehr1steigtein3 = true;
                            break;
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Driver2) <= 2f && MitVictim2geredet == false)
                        {
                            Driver2.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Driver.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (dialogwithdriver2index < dialogwithdriver2.Count)
                                {
                                    Game.DisplaySubtitle(dialogwithdriver2[dialogwithdriver2index]);
                                    dialogwithdriver2index++;
                                }
                                if (dialogwithdriver2index == dialogwithdriver2.Count)
                                {
                                    MitVictim2geredet = true;
                                    if (Victim2Blip.Exists()) Victim2Blip.Delete();
                                }
                            }
                        }
                        if (Game.LocalPlayer.Character.Position.DistanceTo(Medic2) <= 2f && MitMedicgeredet1 == false)
                        {
                            Medic2.TurnToFaceEntity(Game.LocalPlayer.Character);
                            Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to speak to the Medic.");
                            if (Game.IsKeyDown(Settings.DialogKey))
                            {
                                if (dialogwithmedicindex < dialogwithmedic.Count)
                                {
                                    Game.DisplaySubtitle(dialogwithmedic[dialogwithmedicindex]);
                                    dialogwithmedicindex++;
                                }
                            }
                        }
                        if (dialogwithmedicindex == dialogwithmedic.Count && MitMedicgeredet2 == false)
                        {
                            MitMedicgeredet1 = true;
                            if (Game.LocalPlayer.Character.Position.DistanceTo(Medic2) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~" + Settings.InteractionKey + "~s~ to allow the medic to drive away.");
                                if (Game.IsKeyDown(Settings.InteractionKey))
                                {
                                    MitMedicgeredet3 = true;
                                    if (Medic1Blip.Exists()) Medic1Blip.Delete();
                                }
                            }
                        }
                    while (MitMedicgeredet3)
                    {
                        MedicEnter();
                        break;
                    }
                    while (Driver3.CurrentVehicle != null && Driver4.CurrentVehicle != null && Medic1.CurrentVehicle != null && Medic2.CurrentVehicle != null && MitMedicgeredet4 == false)
                    {
                        Medic1.Tasks.CruiseWithVehicle(60f);
                        MitMedicgeredet4 = true;
                        break;
                    }
                    }
                }

            });
        }
        public override void End()
        {
            AufDemWeg = false;
            AfterCall = false;
            MitSupervisorGesprochen = false;
            if (Cop1.Exists()) Cop1.Tasks.EnterVehicle(Polizei, 6000 , -1 );
            if (Firef1.Exists()) Firef1.Tasks.EnterVehicle(Feuerwehr2, 6000, -1, EnterVehicleFlags.None);
            if (Firef2.Exists()) Firef2.Tasks.EnterVehicle(Feuerwehr2, 6000, 0, EnterVehicleFlags.None);
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (Fire5Blip.Exists()) Fire5Blip.Delete();
            if (Medic1Blip.Exists()) Medic1Blip.Delete();
            if (SupervisorBlip.Exists()) SupervisorBlip.Delete();
            if (Victim2Blip.Exists()) Victim2Blip.Delete();
            if (Krankenwagen.Exists()) Krankenwagen.Dismiss();
            if (Polizei.Exists()) Polizei.Dismiss();
            if (Feuerwehr1.Exists()) Feuerwehr1.Dismiss();
            if (Feuerwehr2.Exists()) Feuerwehr2.Dismiss();
            if (Auto1.Exists()) Auto1.Dismiss();
            if (Auto2.Exists()) Auto2.Dismiss();
            if (Auto3.Exists()) Auto3.Dismiss();
            if (Auto4.Exists()) Auto4.Dismiss();
            if (Cop1.Exists()) Cop1.Dismiss();
            if (Cop2.Exists()) Cop2.Dismiss();
            if (Medic1.Exists()) Medic1.Dismiss();
            if (Medic2.Exists()) Medic2.Dismiss();
            if (Driver1.Exists()) Driver1.Dismiss();
            if (Driver2.Exists()) Driver2.Dismiss();
            if (Driver3.Exists()) Driver3.Dismiss();
            if (Driver4.Exists()) Driver4.Dismiss();
            if (Driver5.Exists()) Driver5.Dismiss();
            if (Firef1.Exists()) Firef1.Dismiss();
            if (Firef2.Exists()) Firef2.Dismiss();
            if (Firef3.Exists()) Firef3.Dismiss();
            if (Firef4.Exists()) Firef4.Dismiss();
            if (Firef5.Exists()) Firef5.Dismiss();
            if (Firef6.Exists()) Firef6.Dismiss();
            if (Rhino1.Exists()) Rhino1.Delete();
            if (Rhino2.Exists()) Rhino2.Delete();
            if (Rhino3.Exists()) Rhino3.Delete();
            if (TrafficCone1.Exists()) TrafficCone1.Delete();
            if (TrafficCone2.Exists()) TrafficCone2.Delete();
            if (TrafficCone3.Exists()) TrafficCone3.Delete();
            if (TrafficCone4.Exists()) TrafficCone4.Delete();
            if (TrafficWand.Exists()) TrafficWand.Delete();
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }

        private void SpawnBlockade()
        {
            GameFiber.StartNew(delegate
            {
                Rhino1 = new Vehicle("RHINO",new Vector3(750.059f, -50.3238678f, 58.8495979f), 243.501556f);
                Rhino2 = new Vehicle("RHINO", new Vector3(757.189758f, -53.6917f, 58.7745934f), 242.520325f);
                Rhino3 = new Vehicle("RHINO", new Vector3(763.5843f, -56.3579f, 58.4879f), 188.296967f);
                Rhino1.IsPersistent = true; Rhino1.IsPositionFrozen = true; Rhino1.IsCollisionEnabled = false; Rhino1.IsCollisionProof = false; Rhino1.IsEngineOn = false; Rhino1.IsVisible = false;
                Rhino2.IsPersistent = true; Rhino2.IsPositionFrozen = true; Rhino2.IsCollisionEnabled = false; Rhino2.IsCollisionProof = false; Rhino2.IsEngineOn = false; Rhino2.IsVisible = false;
                Rhino3.IsPersistent = true; Rhino3.IsPositionFrozen = true; Rhino3.IsCollisionEnabled = false; Rhino3.IsCollisionProof = false; Rhino3.IsEngineOn = false; Rhino3.IsVisible = false;
            });

        }

        private void SpawnAutos()
        {
            GameFiber.StartNew(delegate
            {
                Auto1 = new Vehicle(Titles[new Random().Next(0, Titles.Length)], new Vector3(762.0261f, -28.0242157f, 60.4221535f), 346.7226f);
                Auto2 = new Vehicle(Titles[new Random().Next(0, Titles.Length)], new Vector3(758.7832f, -29.6843929f, 60.215786f), 295.218f);
                Auto3 = new Vehicle(Titles[new Random().Next(0, Titles.Length)], new Vector3(763.8887f, -24.8061142f, 60.74571f), 321.090851f);
                Auto4 = new Vehicle(Titles[new Random().Next(0, Titles.Length)], new Vector3(773.986633f, -21.7346649f, 61.0619965f), 18.220686f);
                Driver1 = new Ped(new Vector3(762.549133f, -23.04418f, 61.18625f), 5.56240368f);
                Driver2 = new Ped(new Vector3(759.181763f, -26.77467f, 60.8709755f), 234.650757f);
                Cop1 = new Ped("s_m_y_cop_01", new Vector3(760.7119f, -53.07504f, 58.904686f), 149.002365f);
                Cop2 = new Ped("s_m_y_cop_01", new Vector3(765.6879f, -31.0178661f, 60.7851219f), 345.1757f);
                Cop1.IsPersistent = true; Cop1.BlockPermanentEvents = true; Cop2.IsPersistent = true; Cop2.BlockPermanentEvents = true;
                Cop1.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_car_park_attendant@male@base"), "base", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                Cop2.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_cop_idles@male@base"), "base", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                TrafficWand = new Rage.Object("prop_parking_wand_01", new Vector3(762.349915f, -56.5816002f, 58.5255241f)); TrafficWand.IsPersistent = true;
                TrafficWand.AttachTo(Cop1, 50, new Vector3(0.0900000036f, 0.0400000066f, 0f), new Rotator(148.599991f, -172.999985f, 1.59740448e-05f));
                Medic1 = new Ped("s_m_m_paramedic_01", new Vector3(766.872253f, -28.5691f, 60.9884338f), 139.0529f);
                Medic1.Tasks.PlayAnimation(new AnimationDictionary("amb@medic@standing@tendtodead@base"), "base", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                Medic2 = new Ped("s_m_m_paramedic_01", new Vector3(762.9195f, -20.2090015f, 61.40792f), 178.0925f);
                Medic1.IsPersistent = true; Medic1.BlockPermanentEvents = true; Medic2.IsPersistent = true; Medic2.BlockPermanentEvents = true;
                Firef1 = new Ped("s_m_y_fireman_01", new Vector3(763.239441f, -28.6553612f, 60.8586f), 78.64051f);
                Firef2 = new Ped("s_m_y_fireman_01", new Vector3(759.0459f, -30.958847f, 60.5574646f), 31.86722f);
                Firef3 = new Ped("s_m_y_fireman_01", new Vector3(774.5949f, -24.537426f, 61.3478928f), 7.22994852f);
                Firef4 = new Ped("s_m_y_fireman_01", new Vector3(777.1998f, -28.2153416f, 60.9933929f), 60.0580559f);
                Firef5 = new Ped("s_m_y_fireman_01", new Vector3(775.6583f, -30.6529331f, 60.7970734f), 91.13778f);
                Firef6 = new Ped("s_m_y_fireman_01", new Vector3(764.26416f, -39.46632f, 60.09967f), 258.2771f);
                Firef1.IsPersistent = true; Firef1.BlockPermanentEvents = true; Firef2.IsPersistent = true; Firef2.BlockPermanentEvents = true;
                Firef3.IsPersistent = true; Firef3.BlockPermanentEvents = true; Firef4.IsPersistent = true; Firef4.BlockPermanentEvents = true;
                Firef5.IsPersistent = true; Firef5.BlockPermanentEvents = true; Firef6.IsPersistent = true; Firef6.BlockPermanentEvents = true;
                Driver3 = new Ped(new Vector3(771.9358f, -22.765049f, 61.4645576f), 222.079987f);
                Driver3.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_picnic@male@idle_a"), "idle_c", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                Driver4 = new Ped(new Vector3(775.711243f, -21.5770435f, 61.5670624f), 198.489243f);
                Driver4.Tasks.PlayAnimation(new AnimationDictionary("amb@world_human_picnic@male@idle_a"), "idle_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                Driver5 = new Ped(new Vector3(766.1963f, -29.0343266f, 60.942585f), 92.4126053f);
                Driver1.IsPersistent = true; Driver1.BlockPermanentEvents = true; Driver2.IsPersistent = true; Driver2.BlockPermanentEvents = true;
                Driver3.IsPersistent = true; Driver3.BlockPermanentEvents = true; Driver4.IsPersistent = true; Driver4.BlockPermanentEvents = true;
                Driver5.IsPersistent = true; Driver5.BlockPermanentEvents = true; Driver5.Health = 0;
                Feuerwehr1 = new Vehicle("FIRETRUK", new Vector3(778.4146f, -29.2580452f, 61.01458f), 330.642578f);
                Krankenwagen = new Vehicle("AMBULANCE", new Vector3(772.0595f, -35.68858f, 60.1525879f), 337.4881f);
                Polizei = new Vehicle("POLICE", new Vector3(763.571f, -53.245903f, 58.1078224f), 355.608368f);
                Feuerwehr2 = new Vehicle("FIRETRUK", new Vector3(765.932739f, -39.43169f, 60.2531433f), 345.583344f);
                Auto1.IsPersistent = true; Auto2.IsPersistent = true; Auto3.IsPersistent = true; Auto4.IsPersistent = true;
                Feuerwehr1.IsPersistent = true; Feuerwehr2.IsPersistent = true; Polizei.IsPersistent = true; Krankenwagen.IsPersistent = true;
                Feuerwehr1.IsSirenOn = true; Feuerwehr2.IsSirenOn = true; Polizei.IsSirenOn = true; Krankenwagen.IsSirenOn = true;
                Utils.Damage(Auto1, 25f, 1000f); Utils.Damage(Auto2, 25f, 1000f); Utils.Damage(Auto3, 25f, 1000f); Utils.Damage(Auto4, 25f, 1000f);
                Auto4.SetRotationPitch(180f);
                TrafficCone1 = new Rage.Object("prop_roadcone01b", new Vector3(762.349915f, -56.5816002f, 57.5255241f));
                TrafficCone2 = new Rage.Object("prop_roadcone01b", new Vector3(759.640137f, -55.3501167f, 57.6982079f));
                TrafficCone3 = new Rage.Object("prop_roadcone01b", new Vector3(756.975403f, -54.0347481f, 57.7867088f));
                TrafficCone4 = new Rage.Object("prop_roadcone01b", new Vector3(754.313416f, -52.7141037f, 57.8699303f));
                TrafficCone1.IsPersistent = true; TrafficCone2.IsPersistent = true; TrafficCone3.IsPersistent = true; TrafficCone4.IsPersistent = true;
                MedicHelp1 = new Vector3(771.0105f, -31.78431f, 60.7668f);
                MedicHelp2 = new Vector3(771.0105f, -31.78431f, 60.7668f);
                VictimHelp1 = new Vector3(771.0105f, -31.78431f, 60.7668f);
                VictimHelp4 = new Vector3(771.0105f, -31.78431f, 60.7668f);
            });

        }

        private void FeuerwehrEnter1()
        {
            GameFiber.StartNew(delegate
            {
                Firef5.Tasks.EnterVehicle(Feuerwehr1, 6000, -1, EnterVehicleFlags.None);
                Firef4.Tasks.EnterVehicle(Feuerwehr1, 6000, 0, EnterVehicleFlags.None);
                Firef3.Tasks.EnterVehicle(Feuerwehr1, 6000, 1, EnterVehicleFlags.None);
                Feuerwehr1steigtein2 = true;
                Feuerwehr1steigtein = false;
            });

        }
        private void MedicEnter()
        {
            GameFiber.StartNew(delegate
            {
                MitMedicgeredet2 = true;
                Driver3.Tasks.ClearImmediately();
                Driver4.Tasks.ClearImmediately();
                Medic2.Tasks.ClearImmediately();
                Medic1.Tasks.ClearImmediately();
                Driver3.Tasks.EnterVehicle(Krankenwagen, 6000, 1, EnterVehicleFlags.None);
                Driver4.Tasks.EnterVehicle(Krankenwagen, 6000, 2, EnterVehicleFlags.None);
                Medic1.Tasks.EnterVehicle(Krankenwagen, 6000, -1, EnterVehicleFlags.None);
                Medic2.Tasks.EnterVehicle(Krankenwagen, 6000, 0, EnterVehicleFlags.None);
                MitMedicgeredet3 = false;
            });

        }

        private void BlockadeFrei()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.Position.DistanceTo(Cop1) <= 2f)
                {
                    Cop1.Tasks.Clear();
                    if (TrafficWand.Exists()) TrafficWand.Delete();
                    Cop1.TurnToFaceEntity(Game.LocalPlayer.Character);
                    Game.DisplayHelp("Press ~b~" + Settings.DialogKey + "~s~ to end the roadblock and the callout.");
                    if (Game.IsKeyDown(Settings.DialogKey))
                    {
                        this.End();
                    }
                }
            });

        }
    }

}
