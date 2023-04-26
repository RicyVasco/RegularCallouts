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
    [CalloutInfo("Tree has fallen on Street", CalloutProbability.Medium)]
    public class TreeOnStreet : Callout
    {
        Random rand = new Random();
        private Vector3 SpawnPoint1;
        private Vector3 TreeSpawn;
        private Rotator TreeRotation;
        private Rage.Object Bush;
        private Rage.Object InvisBarrier;
        private Vector3 BushSpawn;
        private Rotator BushRotation;
        private Vehicle FireTruck;
        private Vector3 FireTruckSpawn;
        private Vector3 InvisBarrierSpawn;
        private float FireTruckHeading;
        private Rage.Object Light;
        private Vector3 LightSpawn;
        private Vector3 SawWorkLocation;
        private float SawWorkFloat;
        private float TreeWorkFloat;
        private Vector3 TreeWorkLocation;
        private Rotator LightRotation;
        private Rage.Object Barrier;
        private Rage.Object Saw;
        private Rage.Object Blower;
        private Vector3 BlowWorkLocation;
        private float BlowWorkFloat;
        private Rage.Object Marker;
        private Ped CarDriver;
        private Blip MarkerBlip;
        private TaskSequence BlowerWork;
        private bool GoToWork;
        private bool TextAnzeigen;
        private int StartTimer;
        private bool CountDown;
        private Vector3 BarrierSpawn;
        private Rotator BarrierRotation;
        private Ped FireSaw;
        private Ped FireTree;
        private Ped FireDriver;
        private Ped FireBlower;
        private Ped CarOwner;
        private bool GoToWork2;
        private Vehicle Car;
        private bool CalloutAnfahrt;
        private Vector3 SpawnPoint;
        private Rage.Object Camera;
        private Rage.Object Report;
        private Rage.Object Tree;
        private bool BlockTraffic;
        private Vector3 SpawnPoint2;
        private TaskSequence SawWork;
        private TaskSequence TreeWork;
        private bool LeftVehicle;
        private bool AnimationKilled;
        private Vehicle CarCrashed;
        private bool UnfallGemacht;
        private Blip FireFighterBlip;
        private bool GoToVehicle;
        private bool GoToVehicle2;
        private bool GoToWork3;
        private bool KeinUnfall;
        private float CarFloat;
        private bool GoToWork4;
        private Ped CrashedPed;
        private Vector3 CarCrashLocation;
        private Blip SpawnBlip;
        private int WaitCount;
        private Vector3 SpawnPoint3;
        private Vector3 SpawnPoint4;
        private bool StartWork;
        string[] Titles = { "ASEA", "PRAIRIE", "CHINO", "TAMPA", "HABANERO", "NEON", "BALLER", "ALPHA", "SURGE" };
        private List<string> DialogWithFireDriver = new List<string>
        {
            "~g~Firefighter:~s~ Good to see you here! As you can see we need this road blocked since those drivers don't care about us. (1/2)",
            "~b~You:~s~ No worries I will close the road off, let me know when you're finished. (2/2)",
        };
        private int DialogWithFireDriverIndex;

        public override bool OnBeforeCalloutDisplayed()
        {

            SpawnPoint1 = new Vector3(-829.90509f, 5449.75146f, 33.616291f);
            SpawnPoint2 = new Vector3(232.525146f, -1641.2417f, 29.2177105f);
            SpawnPoint3 = new Vector3(2384.93091f, 5164.97314f, 49.2918892f);
            SpawnPoint4 = new Vector3(2384.93091f, 5164.97314f, 49.2918892f);
            Vector3[] spawnpoints = new Vector3[]
            {
                SpawnPoint1,
                SpawnPoint2,
                SpawnPoint3,
                SpawnPoint4
            };
            Vector3 closestspawnpoint = spawnpoints.OrderBy(x => x.DistanceTo(Game.LocalPlayer.Character)).First();
            if (closestspawnpoint == SpawnPoint1)
            {
                SpawnPoint = SpawnPoint1;
                TreeSpawn = new Vector3(-805.134705f, 5459.50928f, 33.4347f);
                TreeRotation = new Rotator(1.01777744e-13f, 9.86468194e-07f, 136.84288f);
                BushSpawn = new Vector3(-809.11322f, 5463.31055f, 32.5448189f);
                BushRotation = new Rotator(7.17118382f, 2.16778435f, -140.038589f);
                FireTruckSpawn = new Vector3(-820.043945f, 5453.66992f, 34.0146332f);
                FireTruckHeading = -59.4314461f;
                LightSpawn = new Vector3(-811.746338f, 5455.43799f, 32.8467f);
                LightRotation = new Rotator(0f, -0f, 167.109085f);
                BarrierSpawn = new Vector3(-828.299927f, 5454.08203f, 32.8748474f);
                BarrierRotation = new Rotator(0f, 0f, -60.8570976f);
                InvisBarrierSpawn = new Vector3(-828.0560f, 5452.5854f, 32.8992f);
                TreeWorkLocation = new Vector3(-809.6653f, 5462.603f, 33.86684f);
                SawWorkLocation = new Vector3(-806.5891f, 5459.552f, 33.87737f);
                TreeWorkFloat = 312.0045f;
                SawWorkFloat = 278.5683f;
                CarCrashLocation = new Vector3(-826.943481f, 5458.00879f, 33.247654f);
                CarFloat = -43.3876228f;
                BlowWorkLocation = new Vector3(-814.769043f, 5463.16504f, 33.8239479f);
                BlowWorkFloat = -95.2579651f;
            }
            if (closestspawnpoint == SpawnPoint2)
            {
                SpawnPoint = SpawnPoint2;
                TreeSpawn = new Vector3(244.145386f, -1656.47278f, 29.0618477f);
                TreeRotation = new Rotator(0f, 0f, 68.5270157f);
                BushSpawn = new Vector3(247.195663f, -1650.42981f, 28.154026f);
                BushRotation = new Rotator(0f, 0f, 177.749344f);
                FireTruckSpawn = new Vector3(237.345337f, -1646.02222f, 29.337038f);
                FireTruckHeading = -130.32869f;
                LightSpawn = new Vector3(238.483917f, -1651.42676f, 28.3823376f);
                LightRotation = new Rotator(0f, -0f, 87.4996262f);
                BarrierSpawn = new Vector3(243.317169f, -1654.80383f, 29.3217926f);
                BarrierRotation = new Rotator(0f, 0f, -151.148987f);
                InvisBarrierSpawn = new Vector3(231.304596f, -1640.4635f, 28.2086239f);
                TreeWorkLocation = new Vector3(245.358887f, -1650.69861f, 29.3422222f);
                SawWorkLocation = new Vector3(243.317169f, -1654.80383f, 29.3217926f);
                TreeWorkFloat = -123.606354f;
                SawWorkFloat = -151.148987f;
                CarCrashLocation = new Vector3(238.167511f, -1639.43164f, 29.2172852f);
                CarFloat = -114.256493f;
                BlowWorkLocation = new Vector3(244.023605f, -1646.95007f, 29.3404732f);
                BlowWorkFloat = 174.291275f;
            }
            if (closestspawnpoint == SpawnPoint3)
            {
                SpawnPoint = SpawnPoint3;
                TreeSpawn = new Vector3(2387.53467f, 5153.17334f, 48.1320953f);
                TreeRotation = new Rotator(0f, 0f, 47.6414261f);
                BushSpawn = new Vector3(2391.83203f, 5158.04395f, 47.0866318f);
                BushRotation = new Rotator(-4.99998903f, -3.99995589f, -169.56514f);
                FireTruckSpawn = new Vector3(2384.93091f, 5164.97314f, 49.2918892f);
                FireTruckHeading = -163.383255f;
                LightSpawn = new Vector3(2382.13647f, 5161.01221f, 47.9317627f);
                LightRotation = new Rotator(0f, -0f, 58.645443f);
                BarrierSpawn = new Vector3(2389.18359f, 5171.92529f, 48.8716621f);
                BarrierRotation = new Rotator(0f, 0f, -168.573807f);
                InvisBarrierSpawn = new Vector3(2386.36353f, 5170.53027f, 48.7703171f);
                TreeWorkLocation = new Vector3(2392.88525f, 5161.32715f, 48.472187f);
                SawWorkLocation = new Vector3(2391.07397f, 5159.09082f, 48.3700943f);
                TreeWorkFloat = -135.716644f;
                SawWorkFloat = -156.160263f;
                CarCrashLocation = new Vector3(2391.38428f, 5168.6582f, 49.0116539f);
                CarFloat = -152.15918f;
                BlowWorkLocation = new Vector3(2390.19507f, 5163.75293f, 48.8695946f);
                BlowWorkFloat = 154.902695f;
            }
            if (closestspawnpoint == SpawnPoint4)
            {
                SpawnPoint = SpawnPoint4;
                TreeSpawn = new Vector3(782.819153f, 3626.99927f, 32.6240082f);
                TreeRotation = new Rotator(0f, 0f, -50.1257019f);
                BushSpawn = new Vector3(785.377197f, 3623.28955f, 31.791441f);
                BushRotation = new Rotator(0f, 0f, -97.0692825f);
                FireTruckSpawn = new Vector3(795.435059f, 3626.57153f, 33.0102043f);
                FireTruckHeading = 109.647491f;
                LightSpawn = new Vector3(788.600098f, 3630.23999f, 32.324131f);
                LightRotation = new Rotator(0f, -0f, -15.1313887f);
                BarrierSpawn = new Vector3(802.711304f, 3624.9248f, 31.8903675f);
                BarrierRotation = new Rotator(0f, 0f, 110.545143f);
                InvisBarrierSpawn = new Vector3(800.673462f, 3626.50684f, 31.9759712f);
                TreeWorkLocation = new Vector3(787.884033f, 3622.45581f, 33.0514412f);
                SawWorkLocation = new Vector3(784.331787f, 3626.91772f, 33.0163155f);
                TreeWorkFloat = 142.414276f;
                SawWorkFloat = 103.959732f;
                CarCrashLocation = new Vector3(799.885681f, 3622.71338f, 32.2014275f);
                CarFloat = 125.775497f;
                BlowWorkLocation = new Vector3(791.764954f, 3620.71094f, 33.0417519f);
                BlowWorkFloat = 54.9336815f;
            }
            CalloutMessage = "Tree fallen onto street";
            CalloutPosition = SpawnPoint;
            AddMinimumDistanceCheck(20f, SpawnPoint);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_MOTOR_VEHICLE_ACCIDENT IN_OR_ON_POSITION UNITS_RESPOND_CODE_03", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Utils.ClearAreaOfVehicles(SpawnPoint, 40f);
            while (true)
            {
                if(!Tree.Exists())
                {
                    Tree = new Rage.Object("hei_prop_hei_tree_fallen_02", TreeSpawn);
                    Game.LogTrivial("Tree Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Tree.Rotation = TreeRotation;
            Tree.IsPersistent = true; Tree.IsPositionFrozen = true;
            Tree.Position = TreeSpawn;
            WaitCount = 0;
            while (true)
            {
                if (!Bush.Exists())
                {
                    Bush = new Rage.Object("prop_plant_group_06b", BushSpawn);
                    Game.LogTrivial("Bush Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Bush.Rotation = BushRotation;
            Bush.IsPersistent = true; Bush.IsPositionFrozen = true;
            Bush.Position = BushSpawn;
            WaitCount = 0;
            while (true)
            {
                if (!FireTruck.Exists())
                {
                    FireTruck = new Vehicle("FIRETRUK", FireTruckSpawn, FireTruckHeading);
                    Game.LogTrivial("FireT Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            FireTruck.IsPersistent = true; 
            FireTruck.IsSirenOn = true;
            FireTruck.IsSirenSilent = true;
            FireTruck.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
            WaitCount = 0;
            while (true)
            {
                if (!Light.Exists())
                {
                    Light = new Rage.Object("ch_prop_ch_tunnel_worklight", LightSpawn);
                    Game.LogTrivial("Light Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Light.Rotation = LightRotation;
            Light.IsPersistent = true;
            WaitCount = 0;
            while (true)
            {
                if (!Barrier.Exists())
                {
                    Barrier = new Rage.Object("prop_barrier_work02a", BarrierSpawn);
                    Game.LogTrivial("Barrier Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Barrier.Rotation = BarrierRotation;
            Barrier.IsCollisionProof = true;
            Barrier.IsPositionFrozen = true;
            Barrier.IsPersistent = true;
            WaitCount = 0;
            while (true)
            {
                if (!Saw.Exists())
                {
                    Saw = new Rage.Object("prop_tool_consaw", SpawnPoint);
                    Game.LogTrivial("Saw Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Saw.IsPersistent = true;
            WaitCount = 0;
            while (true)
            {
                if (!Blower.Exists())
                {
                    Blower = new Rage.Object("prop_leaf_blower_01", SpawnPoint);
                    Game.LogTrivial("Blower Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            Blower.IsPersistent = true;
            WaitCount = 0;
            while (true)
            {
                if (!FireBlower.Exists() && !FireDriver.Exists() && !FireSaw.Exists() && !FireTree.Exists())
                {
                    FireBlower = new Ped("s_m_y_fireman_01", SpawnPoint, 0f);
                    FireDriver = new Ped("s_m_y_fireman_01", SpawnPoint, 0f);
                    FireSaw = new Ped("s_m_y_fireman_01", SpawnPoint, 0f);
                    FireTree = new Ped("s_m_y_fireman_01", SpawnPoint, 0f);
                    Game.LogTrivial("Actors Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            WaitCount = 0;
            while (true)
            {
                if (!InvisBarrier.Exists())
                {
                    InvisBarrier = new Rage.Object("xs_prop_barrier_10m_01a", InvisBarrierSpawn);
                    Game.LogTrivial("Invisible Barrier Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            InvisBarrier.IsPositionFrozen = true; InvisBarrier.IsVisible = false;
            WaitCount = 0;
            while (true)
            {
                if (!CarCrashed.Exists())
                {
                    CarCrashed = new Vehicle(Titles[new Random().Next(0, Titles.Length)], CarCrashLocation, CarFloat);
                    Game.LogTrivial("Car Spawned");
                    break;
                }
                GameFiber.Yield();
                WaitCount++;
                if (WaitCount > 10) { return false; }
            }
            CarCrashed.IsPersistent = true; CarCrashed.IsPositionFrozen = true; CarCrashed.IsCollisionEnabled = false; CarCrashed.IsVisible = false;
            FireBlower.IsPersistent = true; FireBlower.BlockPermanentEvents = true; FireTree.IsPersistent = true; FireTree.BlockPermanentEvents = true; FireDriver.IsPersistent = true; FireDriver.BlockPermanentEvents = true; FireSaw.IsPersistent = true; FireSaw.BlockPermanentEvents = true;
            FireBlower.WarpIntoVehicle(FireTruck, 0);
            FireDriver.WarpIntoVehicle(FireTruck, -1);
            FireSaw.WarpIntoVehicle(FireTruck, 1);
            FireTree.WarpIntoVehicle(FireTruck, 2);
            Saw.IsVisible = false;
            Blower.IsVisible = false;
            Game.LogTrivial("Actors warped");
            Blower.AttachTo(FireBlower, 50, new Vector3(0.0900000185f, 3.7252903e-09f, 0f), new Rotator(-105.000008f, -72.6000061f, -51f));
            Saw.AttachTo(FireSaw, 50, new Vector3(0.0800000131f, 0.0400000028f, 0f), new Rotator(84.7499695f, 124.000015f, 88.0000076f));
            Game.LogTrivial("Tools attached");
            Game.LogTrivial("All things spawned correctly.");
            SpawnBlip = new Blip(SpawnPoint, 60f);
            SpawnBlip.Color = Color.Yellow;
            SpawnBlip.Alpha = 0.5f;
            SpawnBlip.EnableRoute(Color.Yellow);
            Game.DisplayNotification("~b~Attention to responding Unit:~s~ A Fire Unit is already on scene. Help ~y~secure~s~ the scene as fast as possible.");
            Game.DisplayHelp("You can press~r~ "+ Stuff.Settings.EndCalloutKey.ToString() +"~s~ anytime to end the callout.");
            CalloutAnfahrt = true;
            TreeWork = new TaskSequence(FireTree);
            TreeWork.Tasks.GoStraightToPosition(TreeWorkLocation, 1f, TreeWorkFloat, 1f, 15000);
            TreeWork.Tasks.PlayAnimation("anim@amb@warehouse@toolbox@", "idle", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            StartTimer = 45;
            CalloutLos();
            return base.OnCalloutAccepted();
        }
        private void CalloutLos()
        {
            GameFiber.StartNew(delegate
            {
                try 
                {
                    while (CalloutAnfahrt)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 20f && LeftVehicle == false)
                        {
                            FireSaw.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                            FireTree.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                            FireDriver.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion();
                            FireDriver.Tasks.PlayAnimation(new AnimationDictionary("friends@frj@ig_1"), "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                            Saw.IsVisible = true;
                            SpawnBlip.Delete();
                            InvisBarrier.IsCollisionEnabled = false;
                            LeftVehicle = true;
                        }
                        while (LeftVehicle == true)
                        {
                            GameFiber.Yield();
                            GameEnd();
                            FireDriver.Face(Game.LocalPlayer.Character);
                            if (GoToWork == false)
                            {
                                TreeWork.Execute();
                                TreeWork.Dispose();
                                SawWork = new TaskSequence(FireSaw);
                                SawWork.Tasks.GoStraightToPosition(SawWorkLocation, 1f, SawWorkFloat, 1f, 15000);
                                SawWork.Tasks.PlayAnimation("weapons@heavy@minigun", "fire_med", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                GoToWork = true;
                            }
                            if (GoToWork2 == false)
                            {
                                SawWork.Execute();
                                SawWork.Dispose();
                                TreeWork = new TaskSequence(FireTree);
                                TreeWork.Tasks.GoStraightToPosition(FireTruck.GetOffsetPositionFront(5f), 1f, TreeWorkFloat, 1f, 15000);
                                TreeWork.Tasks.EnterVehicle(FireTruck, 2);
                                GoToWork2 = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(FireDriver) <= 2f && AnimationKilled == false)
                            {
                                FireDriver.Tasks.ClearImmediately();
                                AnimationKilled = true;
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(FireDriver) <= 2f)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    if (DialogWithFireDriverIndex < DialogWithFireDriver.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithFireDriver[DialogWithFireDriverIndex]);
                                        DialogWithFireDriverIndex++;
                                    }
                                    if (DialogWithFireDriverIndex == DialogWithFireDriver.Count)
                                    {
                                        FireDriver.TurnToFaceEntity(FireTree);
                                        while (true)
                                        {
                                            if (!Marker.Exists())
                                            {
                                                Marker = new Rage.Object("prop_mp_halo_sm", SpawnPoint);
                                                Game.LogTrivial("Marker Spawned");
                                                break;
                                            }
                                            GameFiber.Yield();
                                        }
                                        Marker.IsPersistent = true;
                                        MarkerBlip = Marker.AttachBlip();
                                        MarkerBlip.Color = Color.Purple;
                                        Game.DisplayNotification("Go to the ~p~position~s~ to block the traffic from interrupting the Firefighters.");
                                        BlockTraffic = true;
                                        LeftVehicle = false;
                                        break;
                                    }
                                }
                            }
                        }
                        while (BlockTraffic == true)
                        {
                            GameFiber.Yield();
                            GameEnd();

                            while (CountDown)
                            {
                                GameFiber.Yield();
                                GameEnd();
                                TextAnzeigen = true;
                                Game.FrameRender += DrawCCTVText;
                                GameFiber.Sleep(1000);
                                StartTimer--;
                                if (StartTimer == 0)
                                {
                                    TextAnzeigen = false;
                                    KeinUnfall = true;
                                    BlockTraffic = false;
                                    break;
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) > 2f && !Marker.Exists() && !CarCrashed.IsOnScreen)
                                {
                                    CarCrashSpawning();
                                    Utils.DeformFront(CarCrashed, 50f, 150f);
                                    CarCrashed.SetRotationPitch(180f);
                                    while (true)
                                    {
                                        GameFiber.Yield();
                                        if (!CarDriver.Exists())
                                        {
                                            CarDriver = new Ped(CarCrashed.GetOffsetPositionFront(3f), 0f)
                                            {
                                                IsPersistent = true,
                                                BlockPermanentEvents = true,
                                            };
                                            break;
                                        }
                                    }
                                    CarDriver.Kill();
                                    Functions.SetVehicleOwnerName(CarCrashed, Functions.GetPersonaForPed(CarDriver).FullName);
                                    CarCrashed.IsVisible = true;
                                    TextAnzeigen = false;
                                    UnfallGemacht = true;
                                    BlockTraffic = false;
                                    break;
                                }
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 2f && Marker.Exists())
                            {
                                MarkerBlip.Delete();
                                Game.DisplayNotification("Wait until the Firefighters are finished.");
                                CountDown = true;
                                Marker.Delete();
                            }
                        }
                        while (UnfallGemacht)
                        {
                            GameFiber.Yield();
                            GameEnd();
                            if (GoToVehicle == false)
                            {
                                FireTree.Tasks.ClearImmediately();
                                TreeWork.Execute();
                                TreeWork.Dispose();
                                Tree.Delete();
                                SawWork = new TaskSequence(FireSaw);
                                SawWork.Tasks.GoStraightToPosition(FireTruck.GetOffsetPositionFront(5f), 1f, TreeWorkFloat, 1f, 15000);
                                SawWork.Tasks.EnterVehicle(FireTruck, 1);
                                GoToVehicle = true;
                            }
                            if(GoToVehicle2 == false)
                            {
                                FireSaw.Tasks.ClearImmediately();
                                SawWork.Execute();
                                SawWork.Dispose();
                                Saw.IsVisible = false;
                                Blower.IsVisible = true;
                                BlowerWork = new TaskSequence(FireBlower);
                                BlowerWork.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                                BlowerWork.Tasks.GoStraightToPosition(BlowWorkLocation, 1f, BlowWorkFloat, 1f, 15000);
                                BlowerWork.Tasks.PlayAnimation("amb@world_human_gardener_leaf_blower@base", "base", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                GoToVehicle2 = true;
                            }
                            if(GoToWork3 == false)
                            {
                                BlowerWork.Execute();
                                BlowerWork.Dispose();
                                SawWork = new TaskSequence(FireBlower);
                                SawWork.Tasks.GoStraightToPosition(FireTruck.GetOffsetPositionFront(5f), 1f, TreeWorkFloat, 1f, 15000);
                                SawWork.Tasks.EnterVehicle(FireTruck, 0);
                                FireDriver.Tasks.GoStraightToPosition(CarDriver.Position, 1f, CarDriver.Heading, 1f, 15000);
                                Game.DisplaySubtitle("~g~Firefighter:~s~ Goddamit! I told you to watch the traffic!");
                                Game.DisplayHelp("Talk with the Firefighter when you are done");
                                FireFighterBlip = FireDriver.AttachBlip();
                                FireFighterBlip.Color = Color.Green;
                                GoToWork3 = true;
                            }
                            if(GoToWork4 == false)
                            {
                                if (Game.LocalPlayer.Character.DistanceTo(FireDriver) <= 2f)
                                {
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Game.DisplaySubtitle("~g~Firefighter:~s~ Hope your shift doesn't stay like that. Bye!");
                                        Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                                        FireBlower.Tasks.ClearImmediately();
                                        SawWork.Execute();
                                        SawWork.Dispose();
                                        FireDriver.Tasks.EnterVehicle(FireTruck, -1);
                                        Blower.Delete();
                                        Bush.Delete();
                                        Light.Delete();
                                        Barrier.Delete();
                                        FireFighterBlip.Delete();
                                        GameFiber.Wait(10000);
                                        this.End();
                                        break;
                                    }
                                }
                        }
                            while (KeinUnfall)
                            {
                                GameFiber.Yield();
                                GameEnd();
                                if (GoToVehicle == false)
                                {
                                    FireTree.Tasks.ClearImmediately();
                                    TreeWork.Execute();
                                    TreeWork.Dispose();
                                    Tree.Delete();
                                    SawWork = new TaskSequence(FireSaw);
                                    SawWork.Tasks.GoStraightToPosition(FireTruck.GetOffsetPositionFront(5f), 1f, TreeWorkFloat, 1f, 15000);
                                    SawWork.Tasks.EnterVehicle(FireTruck, 1);
                                    GoToVehicle = true;
                                }
                                if (GoToVehicle2 == false)
                                {
                                    FireSaw.Tasks.ClearImmediately();
                                    SawWork.Execute();
                                    SawWork.Dispose();
                                    Saw.IsVisible = false;
                                    Blower.IsVisible = true;
                                    BlowerWork = new TaskSequence(FireBlower);
                                    BlowerWork.Tasks.LeaveVehicle(LeaveVehicleFlags.None);
                                    BlowerWork.Tasks.GoStraightToPosition(BlowWorkLocation, 1f, BlowWorkFloat, 1f, 15000);
                                    BlowerWork.Tasks.PlayAnimation("amb@world_human_gardener_leaf_blower@base", "base", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                    GoToVehicle2 = true;
                                }
                                if (GoToWork3 == false)
                                {
                                    BlowerWork.Execute();
                                    BlowerWork.Dispose();
                                    FireDriver.Tasks.EnterVehicle(FireTruck, -1);
                                    SawWork = new TaskSequence(FireBlower);
                                    SawWork.Tasks.GoStraightToPosition(FireTruck.GetOffsetPositionFront(5f), 1f, TreeWorkFloat, 1f, 15000);
                                    SawWork.Tasks.EnterVehicle(FireTruck, 0);
                                    GoToWork3 = true;
                                }
                                if (GoToWork3 == true && GoToWork4 == false)
                                {
                                    Game.DisplayNotification("Talk to the Firefighter");
                                    FireFighterBlip = FireBlower.AttachBlip();
                                    FireFighterBlip.Color = Color.Green;
                                    GoToWork4 = true;
                                }
                                if (GoToWork4 == true)
                                {
                                    if (Game.LocalPlayer.Character.DistanceTo(FireBlower) <= 2f)
                                    {
                                        Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to speak.");
                                        if (Game.IsKeyDown(Settings.DialogKey))
                                        {
                                            Game.DisplaySubtitle("~g~Firefighter:~s~ Thanks for your help! Hope you have a quiet shift. Bye!");
                                            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
                                            FireBlower.Tasks.ClearImmediately();
                                            SawWork.Execute();
                                            SawWork.Dispose();
                                            Blower.Delete();
                                            Bush.Delete();
                                            CarCrashed.Delete();
                                            Light.Delete();
                                            Barrier.Delete();
                                            FireFighterBlip.Delete();
                                            GameFiber.Wait(10000);
                                            this.End();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch(Exception e)
                {
                    Game.LogTrivial("Error" + e);
                }
        });
        }
        public override void End()
        {
            CalloutAnfahrt = false;
            LeftVehicle = false;
            BlockTraffic = false;
            CountDown = false;
            KeinUnfall = false;
            UnfallGemacht = false;
            if (FireTruck.Exists()) FireTruck.Dismiss();
            if (SpawnBlip.Exists()) SpawnBlip.Delete();
            if (Marker.Exists()) Marker.Delete();
            if (MarkerBlip.Exists()) MarkerBlip.Delete();
            if (Bush.Exists()) Bush.Delete();
            if (Tree.Exists()) Tree.Delete();
            if (FireFighterBlip.Exists()) FireFighterBlip.Delete();
            if (Light.Exists()) Light.Delete();
            if (Barrier.Exists()) Barrier.Delete();
            if (Saw.Exists()) Saw.Delete();
            if (Blower.Exists()) Blower.Delete();
            if (FireBlower.Exists()) FireBlower.Dismiss();
            if (CarCrashed.Exists()) CarCrashed.Dismiss();
            if (CarDriver.Exists()) CarDriver.Dismiss();
            if (FireDriver.Exists()) FireDriver.Dismiss();
            if (FireSaw.Exists()) FireSaw.Dismiss();
            if (FireTree.Exists()) FireTree.Dismiss();
            if (InvisBarrier.Exists()) InvisBarrier.Delete();
            base.End();
        }
        private void GameEnd()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Stuff.Settings.EndCalloutKey))
                {
                    this.End();
                }

            });
        }
        private void CarCrashSpawning()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (!CarCrashed.IsOnScreen)
                {
                    CarCrashed.IsCollisionEnabled = true; CarCrashed.IsPositionFrozen = false; CarCrashed.Position = CarCrashLocation; Utils.Damage(CarCrashed, 25f, 1000f); CarCrashed.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both; CarCrashed.AlarmTimeLeft = TimeSpan.FromSeconds(15);
                }

            });
        }

        private void DrawCCTVText(System.Object sender, Rage.GraphicsEventArgs e)
        {

            if (TextAnzeigen)
            {
                Rectangle drawRect = new Rectangle(0, 0, 3000, 130);
                //e.Graphics.DrawRectangle(drawRect, Color.FromArgb(100, Color.Black));

                e.Graphics.DrawText(StartTimer.ToString() + " seconds remaining.", "Aharoni Bold", 35.0f, new PointF(1, 86), Color.White, drawRect);
            }
            else
            {
                Game.FrameRender -= DrawCCTVText;
            }
        }

    }

}
