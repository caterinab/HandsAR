using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIUtilities : MonoBehaviour {

    public FrameProviderNew frameProvider;

    [Header("What to draw")]

    public bool FPS = false;
    public bool FrameTimestamp = false;


    // Use this for initialization
    void Start () {
		
	}
	
    void Update() { 
    }

	// Update is called once per frame
	void OnGUI () {
        if(FrameTimestamp)
            GUI.Label(new Rect(10, 10, 300, 20), "Timestamp lastFrame in buffer:" + frameProvider.LatestFrame.Timestamp.ToString());
        if(FPS)
            GUI.Label(new Rect(10, 0, 100, 100), "FPS:" + (1.0f / Time.smoothDeltaTime).ToString());
    }


}
