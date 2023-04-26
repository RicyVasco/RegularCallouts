
using Rage.Native;

namespace LucasRitter.Scaleforms
{
    internal class Natives
    {
        internal struct Graphics
        {
            #region Scaleform
            #region Drawing
            /// <summary>
            /// Draws a Scaleform from a given <paramref name="handle"/> on the screen at a given position and size.
            /// </summary>
            /// <param name="handle">The handle of the Scaleform.</param>
            /// <param name="posX">The horizontal position of the Scaleform (0 - 1.0).</param>
            /// <param name="posY">The vertical position of the Scaleform (0 - 1.0).</param>
            /// <param name="width">The width of the Scaleform (0 - 1.0).</param>
            /// <param name="height">The height of the Scaleform (0 - 1.0).</param>
            /// <param name="red">Determines how much of the red channel should be visible.</param>
            /// <param name="green">Determines how much of the green channel should be visible.</param>
            /// <param name="blue">Determines how much of the blue channel should be visible.</param>
            /// <param name="alpha">Determines the opacity of the Scaleform..</param>
            public static void DrawScaleformMovie2D(int handle,
                float posX, float posY, float width, float height,
                int red, int green, int blue, int alpha)
            {
                    Rage.Native.NativeFunction.Natives.x54972ADAF0294A93(
                        handle, posX, posY, width, height, red, green, blue, alpha, 0);
            }

            /// <summary>
            /// Draws a Scaleform from a given <paramref name="handle"/> to cover the entire screen.
            /// </summary>
            /// <param name="handle">The handle of the Scaleform.</param>
            /// <param name="red">Determines how much of the red channel should be visible.</param>
            /// <param name="green">Determines how much of the green channel should be visible.</param>
            /// <param name="blue">Determines how much of the blue channel should be visible.</param>
            /// <param name="alpha">Determines the opacity of the Scaleform..</param>
            public static void DrawScaleformMovie2DFullscreen(int handle,
                int red, int green, int blue, int alpha)
            {
                Rage.Native.NativeFunction.Natives.x0DF606929C105BE1(
                    handle, red, green, blue, alpha, 0);
            }

            /// <summary>
            /// Draws a Scaleform from a given <paramref name="handle"/> at a specific point in 3d space.
            /// </summary>
            /// <param name="handle">The handle of the Scaleform.</param>
            /// <param name="posX">The X position of the Scaleform.</param>
            /// <param name="posY">The Y position of the Scaleform.</param>
            /// <param name="posZ">The Z position of the Scaleform.</param>
            /// <param name="rotX">The X rotation of the Scaleform.</param>
            /// <param name="rotY">The Y rotation of the Scaleform.</param>
            /// <param name="rotZ">The Z rotation of the Scaleform.</param>
            /// <param name="scaleX">The X scale of the Scaleform.</param>
            /// <param name="scaleY">The Y scale of the Scaleform.</param>
            /// <param name="scaleZ">The Z scale of the Scaleform.</param>
            /// <param name="fuzziness">Determines how fuzzy the Scaleform needs to be (lower = fuzzier).</param>
            /// <param name="sharpness">Determines how sharp the Scaleform needs to be (higher = sharper).</param>
            public static void DrawScaleformMovie3D(int handle, float posX, float posY, float posZ,
                float rotX, float rotY, float rotZ,
                float scaleX, float scaleY, float scaleZ, float fuzziness = 5.0f, float sharpness = 1.0f)
            {
                Rage.Native.NativeFunction.Natives.x1CE592FDC749D6F5(
                    handle, posX, posY, posZ, rotX, rotY, rotZ, fuzziness, sharpness, 0, scaleX, scaleY, scaleZ, 2);
            }

            #endregion
            #region Functions

            /// <summary>
            /// Gets the returned <see cref="bool"/> of a Scaleform movie function with a given <paramref name="returnHandle"/>.
            /// </summary>
            /// <param name="returnHandle">The handle to access the function's return value.</param>
            /// <returns>The returned <see cref="bool"/> of the Scaleform movie function.</returns>
            public static bool GetScaleformMovieFunctionReturnBool(int returnHandle)
            {
                return Rage.Native.NativeFunction.Natives.x768FF8961BA904D6<bool>(returnHandle);
            }

            /// <summary>
            /// Gets the returned <see cref="int"/> of a Scaleform movie function with a given <paramref name="returnHandle"/>.
            /// </summary>
            /// <param name="returnHandle">The handle to access the function's return value.</param>
            /// <returns>The returned <see cref="int"/> of the Scaleform movie function.</returns>
            public static int GetScaleformMovieFunctionReturnInt(int returnHandle)
            {
                return Rage.Native.NativeFunction.Natives.x2DE7EFA66B906036<int>(returnHandle);
            }

            /// <summary>
            /// Pops / Calls the latest Scaleform movie function pushed onto the stack.
            /// </summary>
            public static void PopScaleformMovieFunctionVoid()
            {
                Rage.Native.NativeFunction.Natives.xC6796A8FFA375E53();
            }

            /// <summary>
            /// Pops / Calls the latest Scaleform movie function pushed onto the
            /// stack and gets the handle of the return value.
            /// </summary>
            /// <returns>The handle for the return value.</returns>
            public static int PopScaleformMovieFunction()
            {
                return Rage.Native.NativeFunction.Natives.xC50AA39A577AF886<int>();
            }

            /// <summary>
            /// Pushes a Scaleform movie function with a given name to the stack
            /// for the Scaleform movie with a given handle.
            /// </summary>
            /// <param name="handle">The handle for the Scaleform movie.</param>
            /// <param name="function">The name of the function.</param>
            public static void PushScaleformMovieFunction(int handle, string function)
            {
                Rage.Native.NativeFunction.Natives.xF6E48914C7A8694E(handle, function);
            }

            /// <summary>
            /// Pushes a Scaleform movie function with a given name to the stack for a HUD Component.
            /// </summary>
            /// <param name="component">The handle for the HUD Component.</param>
            /// <param name="function">The name of the function.</param>
            public static void PushScaleformMovieFunctionFromHudComponent(int component, string function)
            {
                Rage.Native.NativeFunction.Natives.x98C494FD5BDFBFD5(component, function);
            }

            // Todo: Add comments.
            #region Parameters

            /// <summary>
            /// Pushes a <see cref="bool"/> parameter to the currently pushed Scaleform movie function.
            /// </summary>
            /// <param name="boolean">The boolean that will be added as a parameter.</param>
            public static void PushScaleformMovieParameterBool(bool boolean)
            {
                Rage.Native.NativeFunction.Natives.xC58424BA936EB458(boolean);
            }

            /// <summary>
            /// Pushes a <see cref="float"/> parameter to the currently pushed Scaleform movie function.
            /// </summary>
            /// <param name="number">The floating value that will be added as a parameter.</param>
            public static void PushScaleformMovieParameterFloat(float number)
            {
                Rage.Native.NativeFunction.Natives.xD69736AAE04DB51A(number);
            }

            /// <summary>
            /// Pushes a <see cref="int"/> parameter to the currently pushed Scaleform movie function.
            /// </summary>
            /// <param name="number">The <see cref="int"/> that will be added as a parameter.</param>
            public static void PushScaleformMovieParameterInt(int number)
            {
                Rage.Native.NativeFunction.Natives.xC3D0841A0CC546A6(number);
            }

            /// <summary>
            /// Pushes a <see cref="string"/> parameter to the currently pushed Scaleform movie function.
            /// </summary>
            /// <param name="text">The <see cref="string"/> that will be added as a parameter.</param>
            public static void PushScaleformMovieParameterString(string text)
            {
                Rage.Native.NativeFunction.Natives.xBA7148484BD90365(text);
            }

            #endregion
            #endregion
            #region Management

            /// <summary>
            /// Checks if the Scaleform movie with a given handle has loaded in yet.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            public static bool HasScaleformMovieLoaded(int handle)
            {
                {
                    return Rage.Native.NativeFunction.Natives.HasScaleformMovieLoaded<bool>(handle);
                }
            }

            /// <summary>
            /// Requests the handle for a Scaleform movie with a given <paramref name="identifier" />.
            /// </summary>v
            /// <param name="identifier">The identifier of the Scaleform movie.</param>
            /// <returns>The handle to access the Scaleform movie.</returns>
            public static int RequestScaleformMovie(string identifier)
            {
                return Rage.Native.NativeFunction.Natives.RequestScaleformMovie<int>(identifier);
            }
            public static int RequestScaleformMovieInteractive(string identifier)
            {
                return Rage.Native.NativeFunction.Natives.xBD06C611BB9048C2<int>(identifier);
            }

            /// <summary>
            /// Requests a handle for a Scaleform movie instance with a given <paramref name="identifier" />.
            /// </summary>
            /// <param name="identifier">The identifier of the Scaleform movie.</param>
            /// <returns>A handle to access the Scaleform movie.</returns>
            public static int RequestScaleformMovieInstance(string identifier)
            {
                return Rage.Native.NativeFunction.Natives.RequestScaleformMovieInstance<int>(identifier);
            }

            /// <summary>
            /// Dismisses the Scaleform movie with a given <paramref name="handle"/>.
            /// </summary>
            /// <param name="handle">The handle of the Scaleform movie.</param>
            public static void SetScaleformMovieAsNoLongerNeeded(int handle)
            {
                NativeFunction.Natives.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED(handle);
            }

            #endregion
            #endregion

            #region Others

            /// <summary>
            /// Begins a text component.
            /// </summary>
            /// <param name="type">The type of the component.</param>
            public static void BeginTextComponent(string type)
            {
                Rage.Native.NativeFunction.Natives.x80338406F3475E55(type);
            }

            /// <summary>
            /// Ends a text component.
            /// </summary>
            public static void EndTextComponent()
            {
                Rage.Native.NativeFunction.Natives.x362E2D3FE93A9959();
            }

            #endregion
        }

    }
}