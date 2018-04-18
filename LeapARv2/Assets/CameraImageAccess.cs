using UnityEngine;
using Vuforia;
using System.Runtime.InteropServices;

internal static class OpenCVInterop
{
    [DllImport("native-lib")]
    internal static extern void DetectSkin(int h, int w, ref System.IntPtr pixels, ref System.IntPtr bytes);
}

public class CameraImageAccess : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private Vuforia.Image.PIXEL_FORMAT mPixelFormat = Vuforia.Image.PIXEL_FORMAT.UNKNOWN_FORMAT;

    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    Texture2D tex, tex2, screenshot;
    
    public RenderTexture rt;

    // set to phone camera resolution
    int width = 1280;
    int height = 720;

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
        
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex2 = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        //camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
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
        if (GameObject.Find("Quad+1") != null)
        {
            if (mFormatRegistered)
            {
                if (mAccessCameraImage)
                {
                    Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);

                    if (image != null)
                    {/*
                    Debug.Log(
                        "\nImage Format: " + image.PixelFormat +
                        "\nImage Size:   " + image.Width + "x" + image.Height +
                        "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                        "\nImage Stride: " + image.Stride + "\n"
                    );*/

                        byte[] pixels = image.Pixels;

                        if (pixels != null && pixels.Length > 0)
                        {
                            RenderTexture.active = rt;
                            screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                            screenshot.Apply();
                            
                            byte[] bytes = screenshot.GetRawTextureData();

                            if (bytes != null && bytes.Length > 0)
                            {
                                System.IntPtr pixelsPtr = Marshal.AllocHGlobal(pixels.Length);
                                Marshal.Copy(pixels, 0, pixelsPtr, pixels.Length);
                                System.IntPtr bytesPtr = Marshal.AllocHGlobal(bytes.Length);
                                Marshal.Copy(bytes, 0, bytesPtr, bytes.Length);

                                OpenCVInterop.DetectSkin(image.Height, image.Width, ref pixelsPtr, ref bytesPtr);
                                byte[] t = new byte[pixels.Length];
                                byte[] u = new byte[bytes.Length];

                                Marshal.Copy(pixelsPtr, t, 0, t.Length);
                                Marshal.FreeHGlobal(pixelsPtr);
                                Marshal.Copy(bytesPtr, u, 0, u.Length);
                                Marshal.FreeHGlobal(bytesPtr);
                                
                                //tex.LoadRawTextureData(t);
                                //tex.Apply();
                                //GameObject.Find("QuadBase").GetComponent<Renderer>().material.mainTexture = tex;
                                tex2.LoadRawTextureData(u);
                                tex2.Apply();
                                GameObject.Find("Quad+1").GetComponent<Renderer>().material.mainTexture = tex2;
                            }
                        }
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
    public void UnregisterFormat()
    {
        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }

    void OnDestroy()
    {
        Debug.Log("Camera access script was destroyed");
        UnregisterFormat();
    }

    #endregion //PRIVATE_METHODS
}
