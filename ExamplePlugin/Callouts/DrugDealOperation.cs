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
using UltimateBackup;

namespace RegularCallouts.Callouts
{
    [CalloutInfo("Drug Deal Ops", CalloutProbability.Medium)]
    public class DrugDealOperation : Callout
    {
        private Ped Driver;
        private Vehicle DrugCar;
        private Vehicle CopCar;
        private List<Entity> AllCalloutEntities = new List<Entity>();
        private Vector3 SpawnPoint;
        private Vector3 GangsterDropOff;
        private Vector3 GangsterSpawn;
        private string[] LSPDModels = new string[] { "s_m_y_cop_01", "S_F_Y_COP_01" };
        private bool CalloutRunning;
        private Blip SpawnBlip;
        private Blip SupBlip;
        private Vector3 DeliveryPoint;
        private Rage.Object MobilePhone;
        private EDrugDealing state;

        private int TORSOID;
        private int TORSOTEXTURE;
        private int LEGSID;
        private int LEGSTEXTURE;
        private int HANDSID;
        private int HANDSTEXTURE;
        private int FEETID;
        private int FEETTEXTURE;
        private int EYESID;
        private int EYESTEXTURE;
        private int ACCESSID;
        private int ACCESSTEXTURE;
        private int TASKID;
        private int TASKTEXTURE;
        private int TEXTUREID;
        private int TEXTURETEXTURE;
        private int TORSO2ID;
        private int TORSO2TEXTURE;
        private int HATID;
        private int HATTEXTURE;
        private int DDLocationCheckpoint;

        private List<string> DialogWithBackupCop = new List<string>
        {
            "~g~Agent:~s~ Nice to see you officer.",
            "~b~You:~s~ What's the plan?",
            "~g~Agent:~s~ We've organized a drugdeal but we still need to make sure everyone is willing to buy it before we can make arrests.",
            "~g~Agent:~s~ The car is filled with drugs. We need you to make the deal and notify us when you exchange goods.",
            "~g~Agent:~s~ We got you a fake identity and a set of cloth hidden in the trunk. Make sure to be equipped before making the call.",
            "~g~Agent:~s~ If anything goes down, backup is just around the corner. Good luck.",
        };
        private int DialogWithBackupCopIndex;

        private List<string> DialogWithDrugsPhone = new List<string>
        {
            "~b~You:~s~ I'm at the meeting point, I've got your delivery.",
            "~r~Gangster:~s~ Be there in a minute."
        };
        private int DialogWithDrugsPhoneIndex;
        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = new Vector3(153.2521f, -1304.496f, 28.82673f);
            DeliveryPoint = new Vector3(107.886292f, -1402.42615f, 28.0944786f);
            GangsterSpawn = new Vector3(63.1778755f, -1390.44421f, 28.9423218f);
            GangsterDropOff = new Vector3(93.9759216f, -1407.00818f, 28.7043419f);
            CalloutMessage = "Drug Deal Operation";
            CalloutPosition = SpawnPoint;
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 60f);
            Functions.PlayScannerAudioUsingPosition("UNITS_RESPOND_CODE_02", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
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
                    CreateSpeedZone();
                    GameFiber.Yield();
                    Utils.ClearUnrelatedEntities(SpawnPoint, AllCalloutEntities);
                    GameFiber.Yield();
                    SpawnDrugPoliceStuff();
                    GameFiber.Yield();
                    SaveOutfit();
                    state = EDrugDealing.OnSceneWithCops;
                    while (CalloutRunning)
                    {
                        GameFiber.Yield();
                        GameEnd();
                        if (state == EDrugDealing.OnSceneWithCops)
                        {
                            if (Game.LocalPlayer.Character.DistanceTo(CopCar) <= 15f && SpawnBlip.Exists() && DialogWithBackupCopIndex <= 0)
                            {
                                SupBlip = Driver.AttachBlip();
                                SupBlip.Color = Color.Green;
                                Driver.Tasks.LeaveVehicle(LeaveVehicleFlags.LeaveDoorOpen).WaitForCompletion();
                                Driver.Tasks.PlayAnimation("friends@frj@ig_1", "wave_a", 1f, AnimationFlags.Loop | AnimationFlags.SecondaryTask);
                                SpawnBlip.Delete();
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(Driver) > 2f && DialogWithBackupCopIndex <= 0 && !Driver.IsInAnyVehicle(false))
                            {
                                Driver.Face(Game.LocalPlayer.Character);
                            }
                            if (Game.LocalPlayer.Character.DistanceTo(Driver) <= 2f && DialogWithBackupCopIndex != DialogWithBackupCop.Count)
                            {
                                Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to talk to the agent.");
                                if (Game.IsKeyDown(Settings.DialogKey))
                                {
                                    Driver.Tasks.Clear();
                                    Driver.TurnToFaceEntity(Game.LocalPlayer.Character, 1500);
                                    if (DialogWithBackupCopIndex < DialogWithBackupCop.Count)
                                    {
                                        Game.DisplaySubtitle(DialogWithBackupCop[DialogWithBackupCopIndex]);
                                        DialogWithBackupCopIndex++;
                                    }
                                }
                            }
                            if (DialogWithBackupCopIndex == DialogWithBackupCop.Count)
                            {
                                if (SupBlip.Exists()) 
                                { 
                                    Game.DisplayHelp("Interact with the trunk of the ~y~prepared car~s~ to get ready.");
                                    SpawnBlip = DrugCar.AttachBlip();
                                    SpawnBlip.Color = Color.Yellow;
                                    SupBlip.Delete();
                                }
                                if (Game.LocalPlayer.Character.DistanceTo(DrugCar.GetBonePosition("boot")) <= 1.5f)
                                {
                                    Game.DisplayHelp("Press~y~ " + Settings.DialogKey + "~s~ to switch into undercover gear.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        Utils.VehicleDoorOpen(DrugCar, Utils.VehDoorID.Trunk, false, false);
                                        Game.LocalPlayer.Character.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 1f, AnimationFlags.UpperBodyOnly).WaitForCompletion();
                                        EquipUndercoverOutfit();
                                        Utils.VehicleDoorClose(DrugCar, Utils.VehDoorID.Trunk, false);
                                        Utils.ClearUnrelatedEntities(DeliveryPoint, AllCalloutEntities);
                                        DDLocationCheckpoint = Utils.CreateCheckpoint(Utils.CheckpointType.Cylinder, DeliveryPoint, DeliveryPoint, 2f, Color.IndianRed);
                                        Utils.SetCheckpointHeight(DDLocationCheckpoint, 2f, 2f, 2f);
                                        if (SupBlip.Exists()) { SupBlip.Delete(); }
                                        if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
                                        SpawnBlip = new Blip(DeliveryPoint);
                                        SpawnBlip.Color = Color.IndianRed;
                                        SpawnBlip.EnableRoute(Color.IndianRed);
                                        DrugCar.LockStatus = VehicleLockStatus.Unlocked;
                                        Game.DisplayHelp("Deliver the car to the ~r~dropoff point~s~");
                                        state = EDrugDealing.DrivingToDrugDeal;
                                    }
                                }
                            }
                        }
                        if (state == EDrugDealing.DrivingToDrugDeal)
                        {
                            if (Game.LocalPlayer.Character.CurrentVehicle == DrugCar && Game.LocalPlayer.Character.DistanceTo(DeliveryPoint) <= 2f)
                            {
                                Utils.DeleteCheckpoint(DDLocationCheckpoint);
                                if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
                                while (DrugCar.Speed > 0f)
                                {
                                    GameFiber.Yield();
                                }
                                DrugCar.IsEngineOn = false;
                                Game.LocalPlayer.Character.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion();
                                DrugCar.SetLockedForPlayer(Game.LocalPlayer, true);
                                Game.LocalPlayer.Character.PlayRingtone();
                                ToggleMobilePhone(Game.LocalPlayer.Character, true);
                                GameFiber.Sleep(3000);
                                Game.LocalPlayer.Character.StopRingtone();
                                while (DialogWithDrugsPhoneIndex != DialogWithDrugsPhone.Count)
                                {
                                    GameFiber.Yield();
                                    GameEnd();
                                    Game.DisplayHelp("Press ~b~ " + Settings.DialogKey.ToString() + " ~s~ to talk to the buyer.");
                                    if (Game.IsKeyDown(Settings.DialogKey))
                                    {
                                        if (DialogWithDrugsPhoneIndex < DialogWithDrugsPhone.Count)
                                        {
                                            Game.DisplaySubtitle(DialogWithDrugsPhone[DialogWithDrugsPhoneIndex]);
                                            DialogWithDrugsPhoneIndex++;
                                        }
                                    }
                                }
                                ToggleMobilePhone(Game.LocalPlayer.Character, false);
                                SpawnDrugDealGangster();
                            }
                        }

                    }
                }
                catch(Exception e)
                {
                    Game.LogTrivial("Regular Callout Error:"+ e);
                }
        });
        }
        public override void End()
        {
            CalloutRunning = false;
            Functions.PlayScannerAudio("WE_ARE_CODE_4 NO_FURTHER_UNITS_REQUIRED");
            if (SpawnBlip.Exists()) { SpawnBlip.Delete(); }
            if (SupBlip.Exists()) { SupBlip.Delete(); }
            if (MobilePhone.Exists()) { MobilePhone.Delete(); }
            foreach (Entity e in AllCalloutEntities)
            {
                if (e.Exists())
                {
                    e.Dismiss();
                }
            }
            Utils.DeleteCheckpoint(DDLocationCheckpoint);
            EquipNormalOutfit();
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

        private void SpawnDrugPoliceStuff()
        {
            CopCar = new Vehicle("fbi", new Vector3(153.2521f, -1304.496f, 28.82673f), 4.977823f);
            CopCar.IsPersistent = true;
            AllCalloutEntities.Add(CopCar);
            Driver = CopCar.CreateRandomDriver();
            Driver.IsPersistent = true;
            Driver.BlockPermanentEvents = true;
            AllCalloutEntities.Add(Driver);
            DrugCar = new Vehicle("premier", new Vector3(150.9101f, -1309.236f, 28.75012f), -4.801068f);
            Array values = Enum.GetValues(typeof(EPaint));
            Random random = new Random();
            EPaint randomBar = (EPaint)values.GetValue(random.Next(values.Length));
            DrugCar.SetColors(randomBar, randomBar);
            DrugCar.IsPersistent = true;
            DrugCar.LockStatus = VehicleLockStatus.Locked;
            Functions.SetVehicleOwnerName(DrugCar, "Los Santos Police Department");
            AllCalloutEntities.Add(DrugCar);
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
                                            if (veh.Velocity.Length() > 20f)
                                            {
                                                Vector3 velocity = veh.Velocity;
                                                velocity.Normalize();
                                                velocity *= 20f;
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

        private void UBPoliceStuff()
        {
            
        }
        private void EquipUndercoverOutfit()
        {
            if (Game.LocalPlayer.Character.IsMale)
            {
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_LEGS, 1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_HANDS, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_FEET, 1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_EYES, -1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_ACCESSORIES, -1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TASKS, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TEXTURES, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO2, 16, 0);
                Game.LocalPlayer.Character.SetPedAccessoire(Utils.typeOfAccesssoir.PED_PROP_HATS, -1, 0);
            }
            else
            {
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_LEGS, 1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_HANDS, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_FEET, 1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_EYES, -1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_ACCESSORIES, -1, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TASKS, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TEXTURES, 0, 0);
                Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO2, 16, 0);
                Game.LocalPlayer.Character.SetPedAccessoire(Utils.typeOfAccesssoir.PED_PROP_HATS, -1, 0);
            }
        }
        private void SaveOutfit()
        {
            TORSOID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_TORSO);
            TORSOTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_TORSO);

            LEGSID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_LEGS);
            LEGSTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_LEGS);

            HANDSID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_HANDS);
            HANDSTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_HANDS);

            FEETID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_FEET);
            FEETTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_FEET);

            EYESID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_EYES);
            EYESTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_EYES);

            ACCESSID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_ACCESSORIES);
            ACCESSTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_ACCESSORIES);

            TASKID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_TASKS);
            TASKTEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_TASKS);

            TEXTUREID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_TEXTURES);
            TEXTURETEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_TEXTURES);

            TORSO2ID = Game.LocalPlayer.Character.GetPlayerClothID(Utils.typeOfClothing.PED_VARIATION_TORSO2);
            TORSO2TEXTURE = Game.LocalPlayer.Character.GetPlayerTextureID(Utils.typeOfClothing.PED_VARIATION_TORSO2);

            HATID = Game.LocalPlayer.Character.GetPlayerAccessoireID(Utils.typeOfAccesssoir.PED_PROP_HATS);
            HATTEXTURE = Game.LocalPlayer.Character.GetPlayerAccessoireTexture(Utils.typeOfAccesssoir.PED_PROP_HATS);
        }

        private void EquipNormalOutfit()
        {
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO, TORSOID, TORSOTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_LEGS, LEGSID, LEGSTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_HANDS, HANDSID, HANDSTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_FEET, FEETID, FEETTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_EYES, EYESID, EYESTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_ACCESSORIES, ACCESSID, ACCESSTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TASKS, TASKID, TASKTEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TEXTURES, TEXTUREID, TEXTURETEXTURE);
            Game.LocalPlayer.Character.SetPedCloth(Utils.typeOfClothing.PED_VARIATION_TORSO2, TORSO2ID, TORSO2TEXTURE);
            Game.LocalPlayer.Character.SetPedAccessoire(Utils.typeOfAccesssoir.PED_PROP_HATS, HATID, HATTEXTURE);
        }

        private enum EDrugDealing
        {
            OnSceneWithCops,
            DrivingToDrugDeal,
            WaitingForDealer,
            DrugDealInProgress,
            Action
        }
        private void ToggleMobilePhone(Ped ped, bool toggle)
        {

            if (toggle)
            {
                NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, false);
                ped.Inventory.GiveNewWeapon(new WeaponAsset("WEAPON_UNARMED"), -1, true);
                MobilePhone = new Rage.Object(new Model("prop_police_phone"), new Vector3(0, 0, 0));
                //int boneIndex = NativeFunction.Natives.GET_PED_BONE_INDEX<int>(ped, (int)PedBoneId.RightPhHand);
                int boneIndex = ped.GetBoneIndex(PedBoneId.RightPhHand);
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

        private void SpawnDrugDealGangster()
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                Vehicle test = new Vehicle("police", GangsterSpawn);
                test.IsPersistent = true;
                AllCalloutEntities.Add(test);
                Ped DriverPed = test.CreateRandomDriver();
                DriverPed.IsPersistent = true;
                DriverPed.BlockPermanentEvents = true;
                AllCalloutEntities.Add(DriverPed);
                Game.DisplayNotification("~g~Agent:~s~ Suspect in sight!");
                SpawnBlip = test.AttachBlip();
                SpawnBlip.Sprite = BlipSprite.GetawayCar;
                SpawnBlip.Color = Color.Red;
                SpawnBlip.Flash(500, 30000);
                DriverPed.Tasks.DriveToPosition(GangsterDropOff, 10f, VehicleDrivingFlags.Emergency, 1f).WaitForCompletion(30000);
                state = EDrugDealing.DrugDealInProgress;
            });
        }
    }

}
