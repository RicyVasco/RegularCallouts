using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Native;

namespace FX
{

    public enum PTFXNonLoopedParticle { PaletoDoorwaySmoke = 0, FbiFallingDebris = 1, AgencyBuildingSmoke = 2, ShowerSteam = 3 };
    public enum PTFXLoopedParticle { FbiFallingDebris = 0, AgencyBuildingSmoke = 1, ShowerSteam = 2 };

    public class Particle
    {

        private uint handle;


        /*
         * Array of dictionaries for holding asset and particle pair. 
         * Use a value from 'PTFXParticleNonLooped', as index for this array, to get the '{ASSET , PARTICLE}' pair.  
         */
        private static Dictionary<string, string>[] PTFXNonLoopedDictionaries = new Dictionary<string, string>[]{
            new Dictionary<string, string>(){{"scr_paletoscore","scr_paleto_doorway_smoke"}}, //Smoke1 Dictionary, format: '{ASSET , PARTICLE}'
            new Dictionary<string, string>(){{"scr_agencyheist","scr_fbi_falling_debris"}}, //FbiFallingDebris Dictionary, format: '{ASSET , PARTICLE}'
            new Dictionary<string, string>(){{"scr_agencyheistb","scr_agency3b_blding_smoke"}}, //AgencyBuildingSmoke Dictionary, format: '{ASSET , PARTICLE}'
            new Dictionary<string, string>(){{"scr_mp_house","ent_amb_shower_steam"}} //ShowerSteam Dictionary, format: '{ASSET , PARTICLE}'
        };

        private static Dictionary<string, string>[] PTFXLoopedDictionaries = new Dictionary<string, string>[]{
            new Dictionary<string, string>(){{"scr_agencyheist","scr_fbi_falling_debris"}}, //FbiFallingDebris Dictionary, format: '{ASSET , PARTICLE}'
            new Dictionary<string, string>(){{"scr_agencyheistb","scr_agency3b_blding_smoke"}}, //AgencyBuildingSmoke Dictionary, format: '{ASSET , PARTICLE}'
            new Dictionary<string, string>(){{"scr_mp_house","ent_amb_shower_steam"}} //ShowerSteam Dictionary, format: '{ASSET , PARTICLE}'
        };




        /*
         * MARK: - Constructors for a Ped
         */


        /// <summary>
        /// ** Use the static method "SpawnParticle" for spawn non looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of non particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(PTFXLoopedParticle ptfx, Ped character, float scale)
            : this(PTFXLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                   PTFXLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                   character, scale, Vector3.Zero, Vector3.Zero)
        { }

        /// <summary>
        /// ** Use the static method "SpawnParticle" for spawn non looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of non particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(PTFXLoopedParticle ptfx, Ped character, float scale, Vector3 rotation)
            : this(PTFXLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                   PTFXLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                   character, scale, Vector3.Zero, rotation)
        { }

        /// <summary>
        /// ** Use the static method "SpawnParticle" for spawn non looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of non particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="offset">The offset from the Ped</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(PTFXLoopedParticle ptfx, Ped character, float scale, Vector3 rotation, Vector3 offset)
            : this(PTFXLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                   PTFXLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                   character, scale, offset, rotation)
        { }

        /// <summary>
        /// ** Use the static method "SpawnParticle" for spawn non looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfxAssetName">For all other assets/effects than the ones included</param>
        /// <param name="ptfxParticleName">For all other assets/effects than the ones included</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="offset">The offset from the Ped</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(string ptfxAssetName, string ptfxParticleName, Ped character, float scale, Vector3 rotation, Vector3 offset)
        {
            //Preparing the Asset...
            if (!PreparingAsset(ptfxAssetName)) { return; }

            //Everything went OK. Procceding...
            //Set the PTFX asset to ready, and spawn the particle
            //NativeFunction.CallByHash((ulong)Hashes._SET_PTFX_ASSET_NEXT_CALL, null, ptfxAssetName);
            NativeFunction.Natives.x6C38AF3693A69A91(ptfxAssetName);
            handle = NativeFunction.Natives.x1AE42C1660FD6517<uint>(ptfxParticleName, Game.LocalPlayer.Character, offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, scale, false, false, false);
        }




        /*
         * MARK: - Constructors for a position
         */


        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="nonLoopedPTFX">The type of particle effect you want</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(PTFXLoopedParticle ptfx, Vector3 position, float scale)
           : this(PTFXNonLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                  PTFXNonLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                  position, scale, Vector3.Zero)
        { }

        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="nonLoopedPTFX">The type of particle effect you want</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(PTFXLoopedParticle ptfx, Vector3 position, float scale, Vector3 rotation)
           : this(PTFXNonLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                  PTFXNonLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                  position, scale, rotation)
        { }

        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="ptfxAssetName">For all other assets/effects than the ones included</param>
        /// <param name="ptfxParticleName">For all other assets/effects than the ones included</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public Particle(string ptfxAssetName, string ptfxParticleName, Vector3 position, float scale, Vector3 rotation)
        {
            //Preparing the Asset...
            if (!PreparingAsset(ptfxAssetName)) { return; }

            //Everything went OK. Procceding...
            //Set the PTFX asset to ready, and spawn the particle
            //NativeFunction.CallByHash((ulong)Hashes._SET_PTFX_ASSET_NEXT_CALL, null, ptfxAssetName);
            NativeFunction.Natives.x6C38AF3693A69A91(ptfxAssetName);
            this.handle = NativeFunction.Natives.xE184F4F0DC5910E7<uint>(ptfxParticleName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, scale, false, false, false, false);



        }




        /*
         * MARK: - Custom functions
         */


        private bool PreparingAsset(string ptfxAssetName)
        {
            //Request PTFX asset
            //NativeFunction.CallByHash((ulong)Hashes.REQUEST_NAMED_PTFX_ASSET, ptfxAssetName);

            NativeFunction.Natives.xB80D8756B4668AB6(ptfxAssetName);

            //Checking if PTFX asset is loaded
            if (!IsPTFXAssetLoaded(ptfxAssetName))
            {
                //PTFX is not found so sleep for 25ms, and try again
                GameFiber.Wait(25);

                if (!IsPTFXAssetLoaded(ptfxAssetName))
                {
                    Game.Console.Print("PL: PTFX asset could not be found.");
                    return false;
                }
            }
            Game.Console.Print("PL: PTFX asset found and loaded.");
            return true;
        }

        private bool IsPTFXAssetLoaded(string ptfxAssetName)
        {
            bool loaded = NativeFunction.Natives.x8702416E512EC454<bool>(ptfxAssetName);
            if (/*NativeFunction.CallByHash<bool>((ulong)Hashes.HAS_NAMED_PTFX_ASSET_LOADED, ptfxAssetName*/
                loaded)
            {
                return true;
            }
            return false;
        }

        /// <summary>Stops and removes the particle effect</summary>
        /// <returns>Returns true if the task could be completed.</returns> 
        public bool StopLooping()
        {

            if (this.Exists())
            {
                //Stops and removes the particle effect
                NativeFunction.Natives.x8F75998877616996(this.handle, false);
                NativeFunction.Natives.xC401503DFE8D53CF(this.handle, false);
                return true;
            }
            return false;
        }

        public bool Exists()
        {
            if (NativeFunction.Natives.x74AFEF0D2E1E409B<bool>(this.handle))
            {
                return true;
            }
            return false;
        }















        // --------------------------------- STATIC FUNCTIONS --------------------------------------




        /*
         * MARK: - Static functions for spawning particle on entity
         */


        /// <summary>
        /// ** Use the constructor for spawning looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public static void SpawnParticle(PTFXNonLoopedParticle ptfx, Ped character, float scale)
        {
            SpawnParticle(ptfx, character, scale, Vector3.Zero);
        }

        /// <summary>
        /// ** Use the constructor for spawning looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        public static void SpawnParticle(PTFXNonLoopedParticle ptfx, Ped character, float scale, Vector3 rotation)
        {
            SpawnParticle(ptfx, character, scale, rotation, Vector3.Zero);
        }

        /// <summary>
        /// ** Use the constructor for spawning looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfx">The type of particle you want</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="offset">The offset of the particle effect from the ped</param>
        public static void SpawnParticle(PTFXNonLoopedParticle ptfx, Ped character, float scale, Vector3 rotation, Vector3 offset)
        {
            SpawnParticle(PTFXNonLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                          PTFXNonLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                          character, scale, rotation, offset);
        }

        /// <summary>
        /// ** Use the constructor for spawning looped particles **
        /// Spawn a particle effect with a Ped and the following parameters...
        /// </summary>
        /// <param name="ptfxAssetName">For all other assets/effects than the ones included</param>
        /// <param name="ptfxParticleName">For all other assets/effects than the ones included</param>
        /// <param name="character">The Ped you want to spawn the particle effect on</param>
        /// <param name="offset">The offset from the Ped</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public static void SpawnParticle(string ptfxAssetName, string ptfxParticleName, Ped character, float scale, Vector3 rotation, Vector3 offset)
        {
            //Preparing the Asset...
            if (!SPreparingAsset(ptfxAssetName)) { return; }

            //Everything went OK. Procceding...
            //Set the PTFX asset to ready, and spawn the particle
            //NativeFunction.CallByHash((ulong)Hashes._SET_PTFX_ASSET_NEXT_CALL, null, ptfxAssetName);
            NativeFunction.Natives.x6C38AF3693A69A91(ptfxAssetName);
            NativeFunction.Natives.x0D53A3B8DA0809D2<bool>(ptfxParticleName, Game.LocalPlayer.Character, offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, scale, false, false);
        }




        /*
         * MARK: - Static functions for spawning particle on position
         */

        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="nonLoopedPTFX">The type of particle effect you want</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public static void SpawnParticle(PTFXNonLoopedParticle ptfx, Vector3 position, float scale)
        {
            SpawnParticle(ptfx, position, scale, Vector3.Zero);
        }

        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="nonLoopedPTFX">The type of particle effect you want</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public static void SpawnParticle(PTFXNonLoopedParticle ptfx, Vector3 position, float scale, Vector3 rotation)
        {
            SpawnParticle(PTFXNonLoopedDictionaries[(int)ptfx].Keys.ToArray<string>()[0],
                          PTFXNonLoopedDictionaries[(int)ptfx].Values.ToArray<string>()[0],
                          position, scale, rotation);
        }

        /// <summary>Spawn a particle effect with a position and the following parameters...</summary>
        /// <param name="ptfxAssetName">For all other assets/effects than the ones included</param>
        /// <param name="ptfxParticleName">For all other assets/effects than the ones included</param>
        /// <param name="position">The position you want the particle effect to spawn</param>
        /// <param name="rotation">The rotating of the particle effect</param>
        /// <param name="scale">Sets the size of the particle effect</param>
        public static void SpawnParticle(string ptfxAssetName, string ptfxParticleName, Vector3 position, float scale, Vector3 rotation)
        {
            //Preparing the Asset...
            if (!SPreparingAsset(ptfxAssetName)) { return; }

            //Everything went OK. Procceding...
            //Set the PTFX asset to ready, and spawn the particle
            //NativeFunction.CallByHash((ulong)Hashes._SET_PTFX_ASSET_NEXT_CALL, null, ptfxAssetName);
            NativeFunction.Natives.x6C38AF3693A69A91(ptfxAssetName);
            NativeFunction.Natives.x25129531F77B9ED3<bool>(ptfxParticleName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, scale, false, false, false);
        }




        /*
         * MARK: - Custom static functions
         */


        private static bool SPreparingAsset(string ptfxAssetName)
        {
            //Request PTFX asset
            //NativeFunction.CallByHash((ulong)Hashes.REQUEST_NAMED_PTFX_ASSET, ptfxAssetName);


            NativeFunction.Natives.xB80D8756B4668AB6(ptfxAssetName);






            //Checking if PTFX asset is loaded
            if (!SIsPTFXAssetLoaded(ptfxAssetName))
            {
                //PTFX is not found so sleep for 25ms, and try again
                GameFiber.Wait(25);

                if (!SIsPTFXAssetLoaded(ptfxAssetName))
                {
                    Game.Console.Print("PL: PTFX asset could not be found.");
                    return false;
                }
            }
            Game.Console.Print("PL: PTFX asset found and loaded.");
            return true;
        }

        private static bool SIsPTFXAssetLoaded(string ptfxAssetName)
        {
            bool loaded = NativeFunction.Natives.x8702416E512EC454<bool>(ptfxAssetName);
            if (loaded)
            {
                return true;
            }
            return false;
        }

    }
}