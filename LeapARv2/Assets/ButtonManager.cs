using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private float delta = 0.005f;
    private float delta2 = 1f;
    private float delta3 = 0.001f;
    private float delta4 = 0.01f;
    bool isActive = false;
    public GameObject canvas;
    private int screenshotCount = 0;
    private int photoCount = 0;
    public RenderTexture rtCubes;

    protected const string MEDIA_STORE_IMAGE_MEDIA = "android.provider.MediaStore$Images$Media";
    protected static AndroidJavaObject m_Activity;

    private void Start()
    {
        //impostazioni in base alle preferenze precedenti
        float x = PlayerPrefs.GetFloat("sensorX", 0);
        float y = PlayerPrefs.GetFloat("sensorY", 0);
        float z = PlayerPrefs.GetFloat("sensorZ", 0);
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(x, y, z);
        Debug.Log("Sensor: " + x + " " + y + " " + z);

        if (GameObject.Find("QuadHand") != null)
        {
            float quadHandX = PlayerPrefs.GetFloat("quadHandX", 0);
            float quadHandY = PlayerPrefs.GetFloat("quadHandY", 0);
            GameObject.Find("QuadHand").transform.localPosition = new Vector3(quadHandX, quadHandY, 0);
            Debug.Log("Hand texture: " + quadHandX + " " + quadHandY);
        }

        float camY = PlayerPrefs.GetFloat("camY", 0);
        GameObject.Find("Camera").transform.localPosition = new Vector3(0, camY, 0);
        GameObject.Find("CameraObj").transform.localPosition = new Vector3(0, camY, 0);
        Debug.Log("CamY: " + camY);

        GameObject.Find("CanvasButtons").SetActive(isActive);
    }

    public void PlaneXPlus()
    {
        Vector3 p = GameObject.Find("Plane").transform.localPosition;
        p.x += delta4;
        GameObject.Find("Plane").transform.localPosition = p;
    }

    public void PlaneXMinus()
    {
        Vector3 p = GameObject.Find("Plane").transform.localPosition;
        p.x -= delta4;
        GameObject.Find("Plane").transform.localPosition = p;
    }

    public void PlaneYPlus()
    {
        Vector3 p = GameObject.Find("Plane").transform.localPosition;
        p.y += delta4;
        GameObject.Find("Plane").transform.localPosition = p;
    }

    public void PlaneYMinus()
    {
        Vector3 p = GameObject.Find("Plane").transform.localPosition;
        if ((p.y - delta4) >= 0)
        {
            p.y -= delta4;
            GameObject.Find("Plane").transform.localPosition = p;
        }
    }

    public void PlaneRPlus()
    {
        GameObject.Find("Plane").transform.Rotate(0, 5, 0, Space.World);
    }

    public void PlaneRMinus()
    {
        GameObject.Find("Plane").transform.Rotate(0, -5, 0, Space.World);
    }

    public void CaptureSingleScreenshot()
    {
        //StartCoroutine(Waiter());
        StartCoroutine(CaptureScreenshotCoroutine(Screen.width, Screen.height, 0));
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(10);
    }

    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine(Screen.width, Screen.height, 0));

        //Debug.Log(GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().jsonFrameString);   // gets cut at 1KB

        StartCoroutine(CaptureScreenshotCoroutine(1280, 720, 1));
        StartCoroutine(CaptureScreenshotCoroutine(1280, 720, 2));
    }

    private IEnumerator CaptureScreenshotCoroutine(int width, int height, int type)
    {
        screenshotCount++;
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        switch (type)
        {
            case 0:
                tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                break;
            case 1:
                tex.LoadRawTextureData(GameObject.Find("CameraImageAccess").GetComponent<CameraImageAccess>().pixels);
                break;
            case 2:
                RenderTexture.active = rtCubes;
                tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                break;
            default:
                break;
        }

        tex.Apply();

        yield return tex;
        string path = SaveImageToGallery(tex, "screenshot" + screenshotCount, "Description");
        Debug.Log("Screenshot has been saved at:\n" + path);
    }

    protected static string SaveImageToGallery(Texture2D a_Texture, string a_Title, string a_Description)
    {
        using (AndroidJavaClass mediaClass = new AndroidJavaClass(MEDIA_STORE_IMAGE_MEDIA))
        {
            using (AndroidJavaObject contentResolver = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                AndroidJavaObject image = Texture2DToAndroidBitmap(a_Texture);
                return mediaClass.CallStatic<string>("insertImage", contentResolver, image, a_Title, a_Description);
            }
        }
    }

    protected static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D a_Texture)
    {
        byte[] encodedTexture = a_Texture.EncodeToPNG();
        using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", encodedTexture, 0, encodedTexture.Length);
        }
    }

    protected static AndroidJavaObject Activity
    {
        get
        {
            if (m_Activity == null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_Activity;
        }
    }

    public void ShowHide()
    {
        isActive = !isActive;
        canvas.SetActive(isActive);
    }

    public void CamUp()
    {
        Vector3 p = GameObject.Find("Camera").transform.localPosition;

        p.y += delta3;

        GameObject.Find("Camera").transform.localPosition = p;
        GameObject.Find("CameraObj").transform.localPosition = p;
        PlayerPrefs.SetFloat("camY", p.y);
        PlayerPrefs.Save();
        Debug.Log("CamY: " + p.y);
    }

    public void CamDown()
    {
        Vector3 p = GameObject.Find("Camera").transform.localPosition;

        p.y -= delta3;

        GameObject.Find("Camera").transform.localPosition = p;
        GameObject.Find("CameraObj").transform.localPosition = p;
        PlayerPrefs.SetFloat("camY", p.y);
        PlayerPrefs.Save();
        Debug.Log("CamY: " + p.y);
    }

    public void ResetCam()
    {
        GameObject.Find("Camera").transform.localPosition = new Vector3(0, 0, 0);
        GameObject.Find("CameraObj").transform.localPosition = new Vector3(0, 0, 0);
        PlayerPrefs.Save();
    }

    public void SaveQuadHandOffset(float x, float y)
    {
        PlayerPrefs.SetFloat("quadHandX", x);
        PlayerPrefs.SetFloat("quadHandY", y);
        PlayerPrefs.Save();
    }

    public void ResetQuadHand() {
        GameObject.Find("QuadHand").transform.localPosition = new Vector3(0, 0, 0);
        SaveQuadHandOffset(0, 0);
    }

    public void QuadHandUp()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.y += delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
        Debug.Log("Hand texture: " + p.x + " " + p.y);
    }

    public void QuadHandDown()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.y -= delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
        Debug.Log("Hand texture: " + p.x + " " + p.y);
    }

    public void QuadHandRight()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.x += delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
    }

    public void QuadHandLeft()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.x -= delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
    }

    public void StartMainScene() {
        GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().DisconnectClient();
        SceneManager.LoadSceneAsync("main");
    }

    public void StartMenuScene()
    {
        GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().DisconnectClient();
        SceneManager.LoadSceneAsync("menu");
    }

    public void ResetOffset()
    {
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(0, 0, 0);
    }

    public void SaveOffset() {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;
        PlayerPrefs.SetFloat("sensorX", p.x);
        PlayerPrefs.SetFloat("sensorY", p.y);
        PlayerPrefs.SetFloat("sensorZ", p.z);
        PlayerPrefs.Save();
    }

    public void IncrementX() {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void IncrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void IncrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementX()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }
}
