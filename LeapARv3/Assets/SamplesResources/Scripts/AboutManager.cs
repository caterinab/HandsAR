/*============================================================================== 
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;

public class AboutManager : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    public Text aboutText;
    #endregion //PUBLIC_MEMBERS


    #region PUBLIC_METHODS
    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion //PUBLIC_METHODS


    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        UpdateAboutText();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            LoadNextScene();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
			// On Android, the Back button is mapped to the Esc key
			// Exit app
            Application.Quit();
#endif
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS

    void UpdateAboutText()
    {
        if (!aboutText) return;

        string vuforiaVersion = Vuforia.VuforiaUnity.GetVuforiaLibraryVersion();
        string unityVersion = Application.unityVersion;

        string vuforia = Vuforia.VuforiaRuntime.Instance.HasInitialized
                                ? "<color=green>Yes</color>"
                                : "<color=red>No (enable Vuforia in XR Settings)</color>";

        string about =
            "\n<size=26>Description:</size>" +
            "\nThe Occlusion Management sample shows the use of transparent shaders to " +
            "let users partially look inside a real object with an occlusion effect." +
            "\n" +
            "\n<size=26>Key Functionality:</size>" +
            "\n• Manage occlusion" +
            "\n" +
            "\n<size=26>Physical Targets:</size>" +
            "\n• MultiTarget: MarsBox (Included with Sample)" +
            "\n" +
            "\n<size=26>Instructions:</size>" +
            "\n• Point camera at target to view" +
            "\n• Single tap to focus" +
            "\n" +
            "\n<size=26>Build Version Info:</size>" +
            "\n• Vuforia " + vuforiaVersion +
            "\n• Unity " + unityVersion +
            "\n" +
            "\n<size=26>Project Settings Info:</size>" +
            "\n• Vuforia Enabled: " + vuforia +
            "\n" +
            "\n<size=26>Statistics:</size>" +
            "\nData collected is used solely for product quality improvements" +
            "\nhttps://developer.vuforia.com/legal/statistics" +
            "\n" +
            "\n<size=26>Developer Agreement:</size>" +
            "\nhttps://developer.vuforia.com/legal/vuforia-developer-agreement" +
            "\n" +
            "\n<size=26>Privacy Policy:</size>" +
            "\nhttps://developer.vuforia.com/legal/privacy" +
            "\n" +
            "\n<size=26>Terms of Use:</size>" +
            "\nhttps://developer.vuforia.com/legal/EULA" +
            "\n" +
            "\n© 2017 PTC Inc. All Rights Reserved." +
            "\n";

        aboutText.text = about;

        Debug.Log("Vuforia " + vuforiaVersion + "\nUnity " + unityVersion);

    }
}