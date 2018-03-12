using UnityEngine;
using Leap;

public class GUIUtilities : MonoBehaviour {

    public WebsocketConnection frameProvider;
    public int counter;
    public string fps;

    [Header("On screen data")]

    public bool enable = false;
    //public bool FPS = false;
    //public bool FrameTimestamp = false;
    //public bool delay = false;

    // Use this for initialization
    void Start () {
        counter = 0;
        fps = "";
    }
	
    void Update() {
    }

	// Update is called once per frame
	void OnGUI ()
    {
        if (enable) {
            //  GUI.Label(new Rect(10, 20, 600, 40), "Timestamp newest frame in buffer: " + frameProvider.LatestFrame.Timestamp.ToString());
            counter++;
            counter %= 100;
            if (counter == 0) {
                fps = (1.0f / Time.smoothDeltaTime).ToString();
            }
            GUI.skin.label.fontSize = 28;
            GUI.Label(new Rect(10, 0, 200, 40), "FPS: " + fps);
            //GUI.Label(new Rect(10, 20, 600, 40), "");
        }
    }
}
