using UnityEngine;
using Vuforia;
using System.Runtime.InteropServices;
using System.IO;

internal static class OpenCVInterop
{
    [DllImport("native-lib")]
    internal static extern void DetectSkin(int h, int w, ref System.IntPtr pixels, ref System.IntPtr hands, ref System.IntPtr cubes, ref System.IntPtr output);
}

public class CameraImageAccess : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private Vuforia.Image.PIXEL_FORMAT mPixelFormat = Vuforia.Image.PIXEL_FORMAT.UNKNOWN_FORMAT;

    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    Texture2D tex, screenshot;
    public byte[] pixels;

    public RenderTexture rtHands, rtCubes;

    // set to phone camera resolution
    int width = 1280;
    int height = 720;

    bool init = true;
    Camera cam1, cam2, ar;

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
        
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        cam1 = GameObject.Find("Camera").GetComponent<Camera>();
        cam2 = GameObject.Find("CameraObj").GetComponent<Camera>();
        ar = GameObject.Find("ARCamera").GetComponent<Camera>();

        Debug.Log("Supports Depth mode: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth));
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
        if (init)
        {
            float fov = ar.fieldOfView;
            cam1.fieldOfView = fov;
            cam2.fieldOfView = fov;

            float camY = PlayerPrefs.GetFloat("camY", 0);
            GameObject.Find("Camera").transform.localPosition = new Vector3(0, camY, 0);
            GameObject.Find("CameraObj").transform.localPosition = new Vector3(0, camY, 0);

            init = false;
        }

        if (GameObject.Find("QuadHand") != null)
        {
            if (mFormatRegistered)
            {
                if (mAccessCameraImage)
                {
                    Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);

                    if (image != null)
                    {
                    //Debug.Log(
                    //    "\nImage Format: " + image.PixelFormat +
                    //    "\nImage Size:   " + image.Width + "x" + image.Height +
                    //    "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                    //    "\nImage Stride: " + image.Stride + "\n"
                    //);

                        pixels = image.Pixels;

                        if (pixels.Length > 0)
                        {
                            RenderTexture.active = rtHands;
                            screenshot.ReadPixels(new Rect(0, 0, rtHands.width, rtHands.height), 0, 0);
                            screenshot.Apply();
                            
                            byte[] hands = screenshot.GetRawTextureData();
                            
                            RenderTexture.active = rtCubes;
                            screenshot.ReadPixels(new Rect(0, 0, rtCubes.width, rtCubes.height), 0, 0);
                            screenshot.Apply();

                            byte[] cubes = screenshot.GetRawTextureData();

                            if (hands.Length > 0 && cubes.Length > 0)
                            {
                                System.IntPtr pixelsPtr = Marshal.AllocHGlobal(pixels.Length);
                                Marshal.Copy(pixels, 0, pixelsPtr, pixels.Length);
                                System.IntPtr handsPtr = Marshal.AllocHGlobal(hands.Length);
                                Marshal.Copy(hands, 0, handsPtr, hands.Length);
                                System.IntPtr cubesPtr = Marshal.AllocHGlobal(cubes.Length);
                                Marshal.Copy(cubes, 0, cubesPtr, cubes.Length);
                                int l = image.Height * image.Width * 4;
                                byte[] output = new byte[l];
                                System.IntPtr outputPtr = Marshal.AllocHGlobal(l);
                                Marshal.Copy(output, 0, outputPtr, l);
                                
                                OpenCVInterop.DetectSkin(image.Height, image.Width, ref pixelsPtr, ref handsPtr, ref cubesPtr, ref outputPtr);
                                byte[] t = new byte[l];

                                Marshal.Copy(outputPtr, t, 0, t.Length);
                                Marshal.FreeHGlobal(pixelsPtr);
                                Marshal.FreeHGlobal(handsPtr);
                                Marshal.FreeHGlobal(cubesPtr);
                                Marshal.FreeHGlobal(outputPtr);

                                tex.LoadRawTextureData(t);
                                tex.Apply();
                                GameObject.Find("QuadHand").GetComponent<Renderer>().material.mainTexture = tex;
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
