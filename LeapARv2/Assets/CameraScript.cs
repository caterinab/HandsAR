using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    Camera cam;
    bool done = false;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        if (!done) {
            float fov = gameObject.GetComponent<Camera>().fieldOfView;
            Debug.Log("AR fov: " + fov);
            cam.fieldOfView = fov;
            Debug.Log("cam fov: " + cam.fieldOfView);
            cam.transform.localPosition = new Vector3(0, 0, 0);
            done = true;
        }
    }
}
