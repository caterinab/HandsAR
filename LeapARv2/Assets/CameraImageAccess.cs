using UnityEngine;
using Vuforia;
using System.Runtime.InteropServices;

internal static class OpenCVInterop
{
    [DllImport("native-lib")]
    unsafe internal static extern char* DetectSkin(int h, int w, char* pixels);
}

public class CameraImageAccess : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private Vuforia.Image.PIXEL_FORMAT mPixelFormat = Vuforia.Image.PIXEL_FORMAT.UNKNOWN_FORMAT;

    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    //private RawImage rawImage;
    
    #endregion // PRIVATE_MEMBERS

    #region MONOBEHAVIOUR_METHODS

    void Start()
    {

#if UNITY_EDITOR
        mPixelFormat = Vuforia.Image.PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
#else
        mPixelFormat = Vuforia.Image.PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
#endif

        // Register Vuforia life-cycle callbacks:
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);

    }

    #endregion // MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS

    void OnVuforiaStarted()
    {

        // Try register camera image format
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());

            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError(
                "\nFailed to register pixel format: " + mPixelFormat.ToString() +
                "\nThe format may be unsupported by your device." +
                "\nConsider using a different pixel format.\n");

            mFormatRegistered = false;
        }

    }

    /// <summary>
    /// Called each time the Vuforia state is updated
    /// </summary>
    void OnTrackablesUpdated()
    {
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                
                if (image != null)
                {
                    Debug.Log(
                        "\nImage Format: " + image.PixelFormat +
                        "\nImage Size:   " + image.Width + "x" + image.Height +
                        "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                        "\nImage Stride: " + image.Stride + "\n"
                    );

                    byte[] pixels = image.Pixels;

                    if (pixels != null && pixels.Length > 0)
                    {
                        Debug.Log(
                            "\nImage pixels: " +
                            pixels[0] + ", " +
                            pixels[1] + ", " +
                            pixels[2] + ", ...\n"
                        );

                        unsafe
                        {
                            fixed (char* input = System.Text.Encoding.UTF8.GetString(pixels).ToCharArray())
                            {
                                char* output = OpenCVInterop.DetectSkin(image.Height, image.Width, input);

                                Debug.Log("\nTEST: " + output[0] + output[1] + output[2] + output[3] + output[4]);
                            }
                        }
                        /*
                        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); // RGB24
                        tex.LoadRawTextureData(pixels);
                        tex.Apply();
                        rawImage.texture = tex;
                        rawImage.material.mainTexture = tex;
                        */
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when app is paused / resumed
    /// </summary>
    void OnPause(bool paused)
    {
        if (paused)
        {
            Debug.Log("App was paused");
            UnregisterFormat();
        }
        else
        {
            Debug.Log("App was resumed");
            RegisterFormat();
        }
    }

    /// <summary>
    /// Register the camera pixel format
    /// </summary>
    void RegisterFormat()
    {
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = false;
        }
    }

    /// <summary>
    /// Unregister the camera pixel format (e.g. call this when app is paused)
    /// </summary>
    void UnregisterFormat()
    {
        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }

    #endregion //PRIVATE_METHODS
}
