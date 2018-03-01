using UnityEngine;
using Leap;

public class GUIUtilities : MonoBehaviour {

    public WebsocketConnection frameProvider;
    public Controller controller;
    public int counter;
    public string fps;
    public string delay;

    [Header("On screen data")]

    public bool enable = false;
    //public bool FPS = false;
    //public bool FrameTimestamp = false;
    //public bool delay = false;

    // Use this for initialization
    void Start () {
        controller = new Controller();
        counter = 0;
        fps = "-1";
        delay = "-1";
    }
	
    void Update() {
    }

	// Update is called once per frame
	void OnGUI ()
    {
        if (enable) {
            //  GUI.Label(new Rect(10, 20, 600, 40), "Timestamp newest frame in buffer: " + frameProvider.LatestFrame.Timestamp.ToString());
            counter++;

            if (counter % 100 == 0) {
                counter = 0;
                fps = (1.0f / Time.smoothDeltaTime).ToString();
                //delay = ((controller.Now() - frameProvider.LatestFrame.Timestamp) / 1000).ToString();
            }

            GUI.Label(new Rect(10, 0, 200, 40), "FPS: " + fps);
            //GUI.Label(new Rect(10, 20, 600, 40), "Delay: " + delay + " ms");
        }
    }
}
