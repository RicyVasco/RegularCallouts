using LSPD_First_Response.Engine.Scripting;
using LSPD_First_Response.Mod.API;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegularCallouts.Stuff
{
    internal static class Utils
    {
        public static Ped Player => Game.LocalPlayer.Character;

        public static readonly Random Random = new Random();

        public static Vector3 StoreSpawn = new Vector3(-144.0361f, -592.9335f, 48.24776f);

        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            return Functions.GetAllUserPlugins().Select(assembly => assembly.GetName()).Where(an => string.Equals(an.Name, Plugin, StringComparison.CurrentCultureIgnoreCase)).Any(an => minversion == null || an.Version.CompareTo(minversion) >= 0);
        }

        public static void AddLog(this string s)
        {
            Game.LogTrivial($"[Regular Callouts] {s}");
        }

        public static void ShootBulletBetweenCoord(Vector3 source, Vector3 destination, int damage, bool p7, ulong weapon, Ped owner, bool IsAudible, bool isinvisible, float speed)
        {
            NativeFunction.Natives.SHOOT_SINGLE_BULLET_BETWEEN_COORDS(source.X, source.Y, source.Z, destination.X, destination.Y, destination.Z, damage, p7, weapon, owner, IsAudible, isinvisible, speed);
        }
        public static void ToggleThermalVision(bool TurnOn)
        {
            if (TurnOn)
            {
                NativeFunction.Natives.SET_SEETHROUGH(true);
            }
            else
            {
                NativeFunction.Natives.SET_SEETHROUGH(false);
            }
        }
        public static void Spotlight(Vector3 origin, Vector3 destination, float brightness)
        {
            NativeFunction.Natives.DRAW_SPOT_LIGHT(origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 255, 255, 255, 1000f, brightness, 1f, 10f, 3f);
        }
        public static void TurnRotorsOn(this Vehicle v)
        {
            NativeFunction.Natives.SET_HELI_BLADES_FULL_SPEED(v);
        }
        public static void TurnRotorsoff(this Vehicle v)
        {
            NativeFunction.Natives.SET_HELI_BLADES_SPEED(v, 0f);
        }
        public static void FlyChopper(this Ped driver, Vehicle chopper, Vector3 position, int missiontype, float speed, float landingradius, float heading, int landingflags)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(driver, chopper, 0, 0, position.X, position.Y, position.Z, missiontype, speed, landingradius, heading, -1, -1, -1, landingflags);
        }
        public static void MakeMission(this Entity entity)
        {
            NativeFunction.Natives.SET_ENTITY_AS_MISSION_ENTITY(entity, true, true);
        }
        public static void VehicleExtra(this Vehicle veh, int extraID, int toogle)
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA(veh, extraID, toogle);
        }
        public static void PlaySoundFromCoord(int SoundID, string AudioName, Vector3 Position, string AudioRef)
        {
            NativeFunction.Natives.PLAY_SOUND_FROM_COORD(SoundID, AudioName, Position.X, Position.Y, Position.Z, AudioRef, 0, 0, 0);
            Game.DisplaySubtitle("yes");
        }
        public static void RemoveMission(this Entity entity)
        {
            NativeFunction.Natives.SET_ENTITY_AS_NO_LONGER_NEEDED(entity);
        }
        public static bool IsMission(this Entity entity)
        {
            return NativeFunction.Natives.IS_ENTITY_A_MISSION_ENTITY<bool>(entity);
        }
        public static float GetControlNormal(int InPutGroup, int control)
        {
            return NativeFunction.Natives.GET_CONTROL_NORMAL<float>(InPutGroup, control);
        }

        public static bool IsDisabledControlJustPressed(int InPutGroup, int control)
        {
            return NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(InPutGroup, control);
        }
        public static void DisableControlAction(int InPutGroup, int control, bool disable)
        {
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(InPutGroup, control, disable);
        }
        public static void ApplyBlood(Ped ped, int boneIndex, float xRot, float yRot, float zRot, string woundType = "wound_sheet")
        {
            NativeFunction.Natives.APPLY_PED_BLOOD(ped,boneIndex, xRot, yRot, zRot, woundType);
        }
        public static void LockVehicleDoorInside(this Vehicle veh, int lockstatus)
        {
            NativeFunction.Natives.SET_VEHICLE_DOORS_LOCKED(veh, lockstatus);
        }
        public static void OpenDoor(uint type, Vector3 Location, bool locked, float heading)
        {
            NativeFunction.Natives.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE(type, Location.X, Location.Y, Location.Z, locked, heading, 0);
        }
        public static void ThrowProjectile(this Ped ped, Vector3 Location)
        {
            NativeFunction.Natives.TASK_THROW_PROJECTILE(ped, Location.X, Location.Y, Location.Z);
        }
        public static Rage.Object CloseObject { get; private set; }
        public static Rage.Object ClosestObject(uint type, Vector3 Location)
        {
            CloseObject = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Location.X, Location.Y, Location.Z, 10f, type, false , 0, 0);
            return CloseObject;
        }
        public static uint GetHash(string type)
        {
            return NativeFunction.Natives.GET_HASH_KEY<uint>(type);
        }
        public static float GetGroundZ(this Vector3 position)
        {
            NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(position.X, position.Y, position.Z, out float height, false);
            return height;
        }
        public static Vector3 PedSidewalkSpawn(Vector3 position)
        {
            NativeFunction.Natives.GET_SAFE_COORD_FOR_PED(position.X, position.Y, position.Z, true, out Vector3 Location, 16);
            return Location;
        }
        public static int CamViewMode { get; private set; }
        public static int GetCamViewMode()
        {
            CamViewMode = NativeFunction.Natives.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE<int>();
            return CamViewMode;
        }
        public static int CamViewModeOnFoot { get; private set; }
        public static int GetCamViewModeOnFoot()
        {
            CamViewModeOnFoot = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
            return CamViewModeOnFoot;
        }
        public static int DrawableID { get; private set; }
        public static int GetPlayerClothID(this Ped ped, typeOfClothing ClothID)
        {
            DrawableID = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(ped, (int)ClothID);
            return DrawableID;
        }
        public static int TextureID { get; private set; }
        public static int GetPlayerTextureID(this Ped ped, typeOfClothing ClothID)
        {
            TextureID = NativeFunction.Natives.GET_PED_TEXTURE_VARIATION<int>(ped, (int)ClothID);
            return TextureID;
        }

        public static int DrawableIDHat { get; private set; }
        public static int GetPlayerAccessoireID(this Ped ped, typeOfAccesssoir ClothID)
        {
            DrawableIDHat = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(ped, (int)ClothID);
            return DrawableIDHat;
        }
        public static int DrawableTextureHat { get; private set; }
        public static int GetPlayerAccessoireTexture(this Ped ped, typeOfAccesssoir ClothID)
        {
            DrawableTextureHat = NativeFunction.Natives.GET_PED_PROP_TEXTURE_INDEX<int>(ped, (int)ClothID);
            return DrawableTextureHat;
        }
        public static void SetCamViewMode(int ViewMode)
        {
            NativeFunction.Natives.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE(ViewMode);
        }
        public static void SetCamViewModeOnFoot(int ViewMode)
        {
            NativeFunction.Natives.SET_FOLLOW_PED_CAM_VIEW_MODE(ViewMode);
        }
        public static void StopLoopedFX(int FXNumber)
        {
            NativeFunction.Natives.STOP_PARTICLE_FX_LOOPED(FXNumber, 0);
        }
        public static void LieSpeech(this Ped ped)
        {
            ped.Tasks.PlayAnimation("facials@gen_female@variations@stressed", "mood_stressed_1", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            ped.Tasks.PlayAnimation("clothingtrousers", "check_out_c", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop);
        }
        public static void DoubtSpeech(this Ped ped)
        {
            ped.Tasks.PlayAnimation("facials@gen_male@base", "effort_3", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            ped.Tasks.PlayAnimation("clothingspecs", "try_glasses_negative_a", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop);
        }
        public static void TruthSpeech(this Ped ped)
        {
            ped.Tasks.PlayAnimation("mp_corona_idles@male_e@base", "base", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.SecondaryTask);
            ped.Tasks.PlayAnimation("clothingtrousers", "check_out_c", 1f, AnimationFlags.UpperBodyOnly | AnimationFlags.Loop);
        }
        public static void ApplyDamagePack(this Ped ped, string DamagePack, float damage, float multiplier)
        {
            NativeFunction.Natives.APPLY_PED_DAMAGE_PACK(ped, DamagePack, damage, multiplier);
        }
        public static Vector3 ToGround(this Vector3 position)
        {
            return new Vector3(position.X, position.Y, position.GetGroundZ());
        }
        public static void PlayAudio(string RadioChannel)
        {
            NativeFunction.Natives.SET_FRONTEND_RADIO_ACTIVE(true);
            NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(1);
            NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(1);
            NativeFunction.Natives.SET_RADIO_TO_STATION_NAME(RadioChannel);
            NativeFunction.Natives.SET_RADIO_TRACK(RadioChannel);
        }
        public static void StopAudio()
        {
            NativeFunction.Natives.SET_FRONTEND_RADIO_ACTIVE(false);
            NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(0);
            NativeFunction.Natives.SET_MOBILE_PHONE_RADIO_STATE(0);
        }
        public static void VehicleDoorOpen(this Vehicle vehicle, VehDoorID DoorIndex, bool loose, bool OpenInstantly)
        {
            NativeFunction.Natives.SET_VEHICLE_DOOR_OPEN(vehicle, (int)DoorIndex, loose, OpenInstantly);
        }
        public static void RemoveVehicleDoor(this Vehicle vehicle, VehDoorID DoorIndex, bool CreateDoorObject)
        {
            NativeFunction.Natives.SET_VEHICLE_DOOR_BROKEN(vehicle, (int)DoorIndex, CreateDoorObject);
        }
        public static void VehicleDoorClose(Vehicle vehicle, VehDoorID DoorIndex, bool CloseInstantly)
        {
            NativeFunction.Natives.SET_VEHICLE_DOOR_SHUT(vehicle, (int)DoorIndex, CloseInstantly);
        }
        public static void VehicleTyreBurst(this Vehicle vehicle, VehTyreID TyreIndex, bool OnRim)
        {
            NativeFunction.Natives.SET_VEHICLE_TYRE_BURST(vehicle, (int)TyreIndex, OnRim, 1000);
        }

        public static float NextFloat(float min, float max)
        {
            Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }
        public static int LoopedInt { get; private set; }
        public static int StartParticleFxLoopedOnEntity(string ptfxAsset, string effectName, Entity entity, Vector3 offset, Rotator rotation, float scale)
        {
            const ulong SetPtfxAssetNextCall = 0x6c38af3693a69a91;

            NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<uint>(ptfxAsset);
            while (!NativeFunction.Natives.HAS_NAMED_PTFX_ASSET_LOADED<bool>(ptfxAsset))
            {
                GameFiber.Sleep(25);
                NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<int>(ptfxAsset);
                GameFiber.Yield();
            }

            NativeFunction.CallByHash<uint>(SetPtfxAssetNextCall, ptfxAsset);
            LoopedInt = NativeFunction.Natives.START_PARTICLE_FX_LOOPED_ON_ENTITY<int>(effectName, entity, offset.X, offset.Y, offset.Z, rotation.Pitch, rotation.Roll, rotation.Yaw, scale, false, false, false);
            return LoopedInt;
            // or START_PARTICLE_FX_NON_LOOPED_AT_COORD
        }
        public static Vector3 RotToDir(Vector3 Rot)
        {
            float z = Rot.Z;
            float retz = z * 0.0174532924F;
            float x = Rot.X;
            float retx = x * 0.0174532924F;
            float absx = (float)System.Math.Abs(System.Math.Cos(retx));
            return new Vector3((float)-System.Math.Sin(retz) * absx, (float)System.Math.Cos(retz) * absx, (float)System.Math.Sin(retx));
        }
        public static HitResult RayCastForward()
        {
            return World.TraceLine(NC_Get_Cam_Position(), NC_Get_Cam_Position() + RotToDir(NC_Get_Cam_Rotation()) * 1000f, TraceFlags.IntersectEverything, Game.LocalPlayer.Character);
        }
        public static Vector3 NC_Get_Cam_Position()
        {
            return NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        }
        public static Vector3 NC_Get_Cam_Rotation()
        {
            return NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
        }
        public static int LoopedBoneInt { get; private set; }
        public static int StartParticleFxLoopedOnBone(string ptfxAsset, string effectName, Ped ped, Vector3 offset, Rotator rotation, float scale, int BoneIndex)
        {
            const ulong SetPtfxAssetNextCall = 0x6c38af3693a69a91;

            NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<uint>(ptfxAsset);
            while (!NativeFunction.Natives.HAS_NAMED_PTFX_ASSET_LOADED<bool>(ptfxAsset))
            {
                GameFiber.Sleep(25);
                NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<int>(ptfxAsset);
                GameFiber.Yield();
            }

            NativeFunction.CallByHash<uint>(SetPtfxAssetNextCall, ptfxAsset);
            LoopedBoneInt = NativeFunction.Natives.START_PARTICLE_FX_LOOPED_ON_PED_BONE<int>(effectName, ped, offset.X, offset.Y, offset.Z, rotation.Pitch, rotation.Roll, rotation.Yaw, BoneIndex ,scale, false, false, false);
            return LoopedBoneInt;
        }
        public static int NonLoopedInt { get; private set; }
        public static int StartParticleFxNonLoopedOnEntity(string ptfxAsset, string effectName, Entity entity, Vector3 offset, Rotator rotation, float scale)
        {
            const ulong SetPtfxAssetNextCall = 0x6c38af3693a69a91;

            NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<uint>(ptfxAsset);
            while (!NativeFunction.Natives.HAS_NAMED_PTFX_ASSET_LOADED<bool>(ptfxAsset))
            {
                GameFiber.Sleep(25);
                NativeFunction.Natives.REQUEST_NAMED_PTFX_ASSET<uint>(ptfxAsset);
                GameFiber.Yield();
            }

            NativeFunction.CallByHash<uint>(SetPtfxAssetNextCall, ptfxAsset);
            NonLoopedInt = NativeFunction.Natives.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY<int>(effectName, entity, offset.X, offset.Y, offset.Z, rotation.Pitch, rotation.Roll, rotation.Yaw, scale, false, false, false);
            return NonLoopedInt;
        }
        public static void RemoveParticle(int ptfxHandle)
        {
            NativeFunction.Natives.REMOVE_PARTICLE_FX(ptfxHandle, true);
        }

        public static void GroundSearchAnim(Ped p)
        {
            p.Tasks.PlayAnimation("amb@world_human_gardener_plant@male@enter", "enter", 1f, AnimationFlags.StayInEndFrame);
            GameFiber.Sleep(2100);
            p.Tasks.PlayAnimation("amb@world_human_gardener_plant@male@idle_a", "idle_a", 1f, AnimationFlags.None);
            GameFiber.Sleep(3000);
            p.Tasks.PlayAnimation("amb@world_human_gardener_plant@male@exit", "exit", 1f, AnimationFlags.None);
        }

        public static int Handle { get; private set; }
        public static int CreateCheckpoint(CheckpointType typeOfCheckPoint,
         Vector3 position, Vector3 directTo, float radius, Color color)
        {
            Handle = NativeFunction.Natives.CREATE_CHECKPOINT<int>(
               (int)typeOfCheckPoint,
               position.X,
               position.Y,
               position.Z,
               directTo.X,
               directTo.Y,
               directTo.Z,
               radius,
               color.R,
               color.G,
               color.B,
               color.A,
               0); //Unknown parameter. Default in GTA V scripts is 0.
            return Handle;
        }
        public static void SetPedCloth(this Ped ped, typeOfClothing clothing, int ClothID, int textureID)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, (int)clothing, ClothID, textureID, 0);
        }
        public static void SetPedAccessoire(this Ped ped, typeOfAccesssoir prop, int ClothID, int textureID)
        {
            NativeFunction.Natives.SET_PED_PROP_INDEX(ped, (int)prop, ClothID, textureID, true);
        }

        public static int SceneSyncID { get; private set; }
        public static int CreateSyncScene(Vector3 position,
         Rotator rotation)
        {
            SceneSyncID = NativeFunction.Natives.CREATE_SYNCHRONIZED_SCENE<int>(
               position.X,
               position.Y,
               position.Z,
               rotation.Roll,
               rotation.Pitch,
               rotation.Yaw,
               2); //Unknown parameter. Default in GTA V scripts is 2.
            return SceneSyncID;
        }

        public static void AddPedToSyncScene(this Ped ped, int SceneID, string AnimDict, string AnimName, float speed, float speedmultip, int headingflag, int flag, float playback)
        {
              NativeFunction.Natives.TASK_SYNCHRONIZED_SCENE(
              ped,
              SceneID,
               AnimDict,
               AnimName,
               speed,
               speedmultip,
               headingflag,
               flag,
               playback,
               0); //Unknown parameter. Default in GTA V scripts is 0.
        }

        public static void StartSyncScene(int sceneID)
        {
            NativeFunction.Natives.SET_SYNCHRONIZED_SCENE_PHASE(
                sceneID,
                0.0);
        } 
        public static void PlayRingtone(this Ped ped)
        {
            NativeFunction.Natives.PLAY_PED_RINGTONE("Dial_and_Remote_Ring", ped, 1);
        }
        public static void MobilePhoneTimed(this Ped ped, int duration)
        {
            NativeFunction.Natives.TASK_USE_MOBILE_PHONE_TIMED(ped, duration);
        }
        public static void UseMobilePhone(this Ped ped)
        {
            NativeFunction.Natives.TASK_USE_MOBILE_PHONE(ped, -1, -1);
        }
        public static void StopRingtone(this Ped ped)
        {
            NativeFunction.Natives.STOP_PED_RINGTONE(ped);
        }
        public static void ClearUnrelatedEntities(Vector3 Location, List<Entity> list)
        {

            foreach (Ped entity in World.GetEntities(Location, 50f, GetEntitiesFlags.ConsiderAllPeds))
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

                                        if (!list.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, Location) < 50f)
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
            foreach (Vehicle entity in World.GetEntities(Location, 50f, GetEntitiesFlags.ConsiderGroundVehicles))
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

                                        if (!list.Contains(entity))
                                        {
                                            if (Vector3.Distance(entity.Position, Location) < 50f)
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
        public static int Handle2 { get; private set; }
        public static int AddDecal(Vector3 pos, DecalTypes decalType, float width = 1.0f, float height = 1.0f, float rCoef = 0.1f, float gCoef = 0.1f, float bCoef = 0.1f, float opacity = 1.0f, float timeout = 20.0f)
        {
           Handle2 = NativeFunction.Natives.ADD_DECAL<int>(
           (int)decalType, 
           pos.X, 
           pos.Y, 
           pos.Z, 
           0, 
           0, 
           -1.0, 
           0, 
           1.0, 
           0, 
           width, 
           height, 
           rCoef, 
           gCoef, 
           bCoef, 
           opacity, 
           timeout, 
           0, 
           0, 
           0);
           return Handle2;
        }
        public static int Handle3 { get; private set; }
        public static int AddDecalOnGround(Vector3 pos, DecalTypes decalType, float width = 1.0f, float height = 1.0f, float rCoef = 0.1f, float gCoef = 0.1f, float bCoef = 0.1f, float opacity = 1.0f, float timeout = 20.0f)
        {
            Handle3 = NativeFunction.Natives.ADD_DECAL<int>((int)decalType, pos.X, pos.Y, GetGroundZ(pos), 0f, 0f, -1.0f, 0f, 1.0f, 0f, width, height, rCoef, gCoef, bCoef, opacity, timeout, 0f, 0f, 0f);
            return Handle3;
            //GRAPHICS::ADD_DECAL(1110, 1923.866f, 3986.804f, 31.2484f, 0f, 0f, -1f, func_1439(0f, 1f, 0f), 1f, 1f, 0.196f, 0f, 0f, 1f, -1f, 0, 0, 0);
        }
        public static void DeleteDecal(int Decal)
        {
            NativeFunction.Natives.REMOVE_DECAL(Decal);
        }
        public static void FollowPed(Ped ped, Entity target, Vector3 offset, float movementspeed,int timeout, float stoppingrange, bool PersistFollowing)
        {
            NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(ped, target, offset.X, offset.Y, offset.Z, movementspeed, -1, timeout, PersistFollowing);
        }
        public static void VehicleEscort(Ped ped, Vehicle vehicle, Vehicle targetvehicle, TypeOfEscort mode, float speed, DriveStyleType drivestyle, float mindistance, int p7, float noroadsdistance)
        {
            NativeFunction.Natives.TASK_VEHICLE_ESCORT(
            ped,
            vehicle,
            targetvehicle,
            (int)mode,
            speed,
            (int)drivestyle,
            mindistance,
            p7,
            noroadsdistance);
        }
        public static void TurnToFaceEntity(this Ped ped, Entity entity, int duration = -1)
        {
            NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(ped, entity, duration);
        }

        public static void DeleteCheckpoint(int Checkpoint)
        {
            NativeFunction.Natives.DELETE_CHECKPOINT(Checkpoint);
        }

        public static float VehicleNodeHeading(this Vector3 Position)
        {
            bool get_property_success = false;
            Vector3 outPosition;
            float outHeading;


                get_property_success = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out outPosition, out outHeading, 1, 3, 0);

            if (get_property_success)
            {
                return outHeading;
            }
            else
            {
                return -1;
            }

        }
        public static void ShowID(this Ped ped)
        {
            GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                Rage.Object id = new Rage.Object("p_ld_id_card_01", new Vector3(0f, 0f, 0f));
                id.AttachTo(ped, ped.GetBoneIndex(PedBoneId.LeftPhHand), new Vector3(0f, 0f, 0f), new Rotator(0f, 0f, 0f));
                ped.Tasks.PlayAnimation("anim@heists@humane_labs@finale@keycards", "ped_a_enter", 1f, AnimationFlags.DisableRootMotion | AnimationFlags.StayInEndFrame);
                GameFiber.Sleep(1500);
                Functions.DisplayPedId(ped, true);
                ped.Tasks.PlayAnimation("anim@heists@humane_labs@finale@keycards", "ped_a_exit", 1f, AnimationFlags.DisableRootMotion).WaitForCompletion();
                id.Delete();
            });
        }
        public static void SetCheckpointHeight(int Checkpoint, float nearheight, float farheight, float radius)
        {
            NativeFunction.Natives.SET_CHECKPOINT_CYLINDER_HEIGHT(Checkpoint, nearheight, farheight, radius);
        }

        public static string GetPolicePedModelForPosition(Vector3 position)
        {
            var result = string.Empty;
            var array1 = new[]
            {
                "s_m_y_cop_01",
                "s_f_y_cop_01"
            };
            var array2 = new[]
            {
                "s_m_y_sheriff_01",
                "s_f_y_sheriff_01"
            };
            result = Functions.GetZoneAtPosition(position).County == EWorldZoneCounty.LosSantos
                ? array1[Random.Next(array1.Length)]
                : array2[Random.Next(array2.Length)];
            return result;
        }

        public static void DeformFront(this Vehicle vehicle, float radius, float amount)
        {
            // Get dimensions we want to deform
            var dimensions = vehicle.Model.Dimensions;
            var halfWidth = (dimensions.X / 2) * 0.6f;
            var halfLength = dimensions.Y / 2;
            var halfHeight = (dimensions.Z / 2) * 0.7f;

            // Apply random deformities within the ranges of the model
            var num = new Random().Next(15, 45);
            for (var index = 0; index < num; ++index)
            {
                // We use half values here, since this is an OFFSET from center
                var randomInt1 = MathHelper.GetRandomSingle(-halfWidth, halfWidth); // Full width
                var randomInt2 = MathHelper.GetRandomSingle(halfLength * 0.85f, halfLength); // Front end
                var randomInt3 = MathHelper.GetRandomSingle(-halfHeight, 0); // Lower half height
                vehicle.Deform(new Vector3(randomInt1, randomInt2, randomInt3), radius, amount);
            }
        }


        public static string GetPoliceVehicleModelForPosition(Vector3 position)
        {
            var result = string.Empty;
            var array1 = new[]
            {
                "police",
                "police2",
                "police3",
                "police4"
            };
            var array2 = new[]
            {
                "sheriff",
                "sheriff2"
            };
            result = Functions.GetZoneAtPosition(position).County == EWorldZoneCounty.LosSantos ? array1[Random.Next(array1.Length)] : array2[Random.Next(array2.Length)];
            return result;
        }

        public static void RandomizeLicensePlate(this Vehicle vehicle)
        {
            if (vehicle)
                vehicle.LicensePlate = MathHelper.GetRandomInteger(0, 9).ToString() +
                                       MathHelper.GetRandomInteger(0, 9) +
                                       Convert.ToChar(Convert.ToInt32(Math.Floor(26 * MathHelper.GetRandomDouble(0, 1) + 65))) +
                                       Convert.ToChar(Convert.ToInt32(Math.Floor(26 * MathHelper.GetRandomDouble(0, 1) + 65))) +
                                       Convert.ToChar(Convert.ToInt32(Math.Floor(26 * MathHelper.GetRandomDouble(0, 1) + 65))) +
                                       MathHelper.GetRandomInteger(0, 9) +
                                       MathHelper.GetRandomInteger(0, 9) +
                                       MathHelper.GetRandomInteger(0, 9);
        }

        public static void Damage(this Vehicle vehicle, float radius, float amount)
        {
            var model = vehicle.Model;
            model.GetDimensions(out var vector3_1, out var vector3_2);
            var num = new Random().Next(10, 45);
            for (var index = 0; index < num; ++index)
            {
                var randomInt1 = MathHelper.GetRandomSingle(vector3_1.X, vector3_2.X);
                var randomInt2 = MathHelper.GetRandomSingle(vector3_1.Y, vector3_2.Y);
                var randomInt3 = MathHelper.GetRandomSingle(vector3_1.Z, vector3_2.Z);
                vehicle.Deform(new Vector3(randomInt1, randomInt2, randomInt3), radius, amount);
            }
        }

        public static float GetRandomHeading()
        {
            return MathHelper.GetRandomSingle(0, 360);
        }

        public static void SetLivery(this Vehicle vehicle, int liveryIndex)
        {
            NativeFunction.Natives.SET_VEHICLE_LIVERY(vehicle, liveryIndex);
        }

        public static void ClearAreaOfPeds(Vector3 position, float radius)
        {
            var allPeds = World.GetAllPeds();
            foreach (var ped in allPeds)
            {
                if (!ped) continue;
                if (!ped.IsPersistent) continue;
                if (!(ped.DistanceTo2D(position) <= radius)) continue;
                if (ped != Game.LocalPlayer.Character)
                {
                    ped.Delete();
                }
            }
        }

        public static void ClearAreaOfVehicles(Vector3 position, float radius)
        {
            var allVehicles = World.GetAllVehicles();
            foreach (var vehicle in allVehicles)
            {
                if (!vehicle) continue;
                if (!vehicle.IsPersistent) continue;
                if (!(vehicle.DistanceTo2D(position) <= radius)) continue;
                if (vehicle != Game.LocalPlayer.Character.LastVehicle)
                {
                    vehicle.Delete();
                }
            }
        }


        public static void PlayRadioAnimation(this Ped ped)
        {
            if (ped && ped.IsAlive)
            {
                GameFiber.StartNew(delegate
                {
                    ped.Tasks.Clear();
                    var animTask1 = ped.Tasks.PlayAnimation(new AnimationDictionary("random@arrests"), "generic_radio_enter", 1f, AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly);
                    var length1 = animTask1.Length;
                    GameFiber.Sleep((int)length1 * 1000);
                    ped.Tasks.PlayAnimation(new AnimationDictionary("random@arrests"), "generic_radio_chatter", 1f, AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly);
                    GameFiber.Sleep(5000);
                    var animTask2 = ped.Tasks.PlayAnimation(new AnimationDictionary("random@arrests"), "generic_radio_exit", 1f, AnimationFlags.SecondaryTask | AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly);
                    var length2 = animTask2.Length;
                    GameFiber.Sleep((int)length2 * 1000);
                    ped.Tasks.Clear();
                });
            }
        }

        public static bool DisplayTime = false;
        private static List<string> Answers;
        //public enum AnswersResults { Positive, Negative, Neutral, Null};
        public static int DisplayAnswers(List<string> PossibleAnswers, bool Shuffle = true)
        {
            Game.RawFrameRender += DrawAnswerWindow;
            DisplayTime = true;
            Answers = new List<string>(PossibleAnswers);

            string AnswerGiven = "";
            //Game.LocalPlayer.Character.IsPositionFrozen = true;

            GameFiber.StartNew(delegate
            {
                while (DisplayTime)
                {
                    GameFiber.Yield();

                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D1))
                        {
                        if (Answers.Count >= 1)
                        {
                            AnswerGiven = Answers[0];

                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D2))
                    {
                        if (Answers.Count >= 2)
                        {
                            AnswerGiven = Answers[1];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D3))
                    {
                        if (Answers.Count >= 3)
                        {
                            AnswerGiven = Answers[2];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D4))
                    {
                        if (Answers.Count >= 4)
                        {
                            AnswerGiven = Answers[3];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D5))
                    {
                        if (Answers.Count >= 5)
                        {
                            AnswerGiven = Answers[4];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D6))
                    {
                        if (Answers.Count >= 6)
                        {
                            AnswerGiven = Answers[5];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D7))
                    {
                        if (Answers.Count >= 7)
                        {
                            AnswerGiven = Answers[6];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D8))
                    {
                        if (Answers.Count >= 8)
                        {
                            AnswerGiven = Answers[7];
                        }
                    }
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.D9))
                    {
                        if (Answers.Count >= 9)
                        {
                            AnswerGiven = Answers[8];
                        }
                    }
                }
            });
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(Game.LocalPlayer.Character, false);
            Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
            float PlayerHeading = Game.LocalPlayer.Character.Heading;
            while (AnswerGiven == "")
            {
                GameFiber.Yield();
                if (Vector3.Distance(Game.LocalPlayer.Character.Position, PlayerPos) > 4f)
                {
                    Game.LocalPlayer.Character.Tasks.FollowNavigationMeshToPosition(PlayerPos, PlayerHeading, 1.2f).WaitForCompletion(1500);
                }
                /*if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    Game.LocalPlayer.Character.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(1800);
                }*/
                if (!DisplayTime) { break; }
            }
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(Game.LocalPlayer.Character, true);
            DisplayTime = false;
            //Game.LocalPlayer.Character.IsPositionFrozen = false;

            return PossibleAnswers.IndexOf(AnswerGiven);


        }

        private static void DrawAnswerWindow(System.Object sender, Rage.GraphicsEventArgs e)
        {
            if (DisplayTime)
            {
                Rectangle drawRect = new Rectangle(Game.Resolution.Width / 5, Game.Resolution.Height / 7, 700, 180);
                Rectangle drawBorder = new Rectangle(Game.Resolution.Width / 5 - 5, Game.Resolution.Height / 7 - 5, 700, 180);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawRectangle(drawBorder, Color.FromArgb(90, Color.Black));
                e.Graphics.DrawRectangle(drawRect, Color.Black);

                e.Graphics.DrawText("Select with Number Keys", "Aharoni Bold", 18.0f, new PointF(drawBorder.X + 150, drawBorder.Y + 2), Color.White, drawBorder);

                int YIncreaser = 30;
                for (int i = 0; i < Answers.Count; i++)
                {

                    e.Graphics.DrawText("[" + (i + 1).ToString() + "] " + Answers[i], "Arial Bold", 15.0f, new PointF(drawRect.X + 10, drawRect.Y + YIncreaser), Color.White, drawRect);
                    YIncreaser += 25;
                }


            }
            else
            {
                Game.FrameRender -= DrawAnswerWindow;
            }


        }

        public enum DecalTypes
        {
            splatters_blood = 1010,
            splatters_blood_dir = 1015,
            splatters_blood_mist = 1017,
            splatters_mud = 1020,
            splatters_paint = 1030,
            splatters_water = 1040,
            splatters_water_hydrant = 1050,
            splatters_blood2 = 1110,
            weapImpact_metal = 4010,
            weapImpact_concrete = 4020,
            weapImpact_mattress = 4030,
            weapImpact_mud = 4032,
            weapImpact_wood = 4050,
            weapImpact_sand = 4053,
            weapImpact_cardboard = 4040,
            weapImpact_melee_glass = 4100,
            weapImpact_glass_blood = 4102,
            weapImpact_glass_blood2 = 4104,
            weapImpact_shotgun_paper = 4200,
            weapImpact_shotgun_mattress,
            weapImpact_shotgun_metal,
            weapImpact_shotgun_wood,
            weapImpact_shotgun_dirt,
            weapImpact_shotgun_tvscreen,
            weapImpact_shotgun_tvscreen2,
            weapImpact_shotgun_tvscreen3,
            weapImpact_melee_concrete = 4310,
            weapImpact_melee_wood = 4312,
            weapImpact_melee_metal = 4314,
            burn1 = 4421,
            burn2,
            burn3,
            burn4,
            burn5,
            bang_concrete_bang = 5000,
            bang_concrete_bang2,
            bang_bullet_bang,
            bang_bullet_bang2 = 5004,
            bang_glass = 5031,
            bang_glass2,
            solidPool_water = 9000,
            solidPool_blood,
            solidPool_oil,
            solidPool_petrol,
            solidPool_mud,
            porousPool_water,
            porousPool_blood,
            porousPool_oil,
            porousPool_petrol,
            porousPool_mud,
            porousPool_water_ped_drip,
            liquidTrail_water = 9050
        }


        public enum CheckpointType
        {
#pragma warning disable 1591 // cuts off nagging about missing XML comments.
            Traditional = 0,
            SmallArrow = 5,
            DoubleArrow = 6,
            TripleArrow = 7,
            CycleArrow = 8,
            ArrowInCircle = 10,
            DoubleArrowInCircle = 11,
            TripleArrowInCircle = 12,
            CycleArrowInCircle = 13,
            CheckerInCircle = 14,
            Arrow = 15,
            Cylinder = 47
        }

        public enum DriveStyleType
        {
            AvoidTraffic = 6,
            Rushed = 1074528293
        }

        public enum TypeOfEscort
        {
            Behind = -1,
            Ahead = 0,
            Left = 1,
            Right = 2,
            BackLeft = 3,
            BackRight = 4
        }
        public enum typeOfClothing
        {
            PED_VARIATION_FACE = 0,
            PED_VARIATION_HEAD = 1,
            PED_VARIATION_HAIR = 2,
            PED_VARIATION_TORSO = 3,
            PED_VARIATION_LEGS = 4,
            PED_VARIATION_HANDS = 5,
            PED_VARIATION_FEET = 6,
            PED_VARIATION_EYES = 7,
            PED_VARIATION_ACCESSORIES = 8,
            PED_VARIATION_TASKS = 9,
            PED_VARIATION_TEXTURES = 10,
            PED_VARIATION_TORSO2 = 11
        }
        public enum typeOfAccesssoir
        {
            PED_PROP_HATS = 0,
            PED_PROP_GLASSES = 1,
            PED_PROP_EARS = 2
        }

        public enum VehDoorID
        {
            FrontLeft = 0,
            FrontRight = 1,
            RearLeft = 2,
            RearRight = 3,
            Hood = 4,
            Trunk = 5,
            Back = 6
        }
        public enum VehTyreID
        {
            FrontLeft = 0,
            FrontRight = 1,
            MiddleLeft = 2,
            MiddleRight = 3,
            BackLeft = 4,
            BackRight = 5,
        }
    }
}