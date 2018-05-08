using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingRange : MonoBehaviour {
    GameObject[] gos;
    float near = 1000.0f, far = -1.0f;
    Camera camera, cameraObj;
    Vector3 cameraPos;

    // Use this for initialization
    void Start () {
        gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        cameraObj = GameObject.Find("CameraObj").GetComponent<Camera>();
        cameraPos = camera.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        float pos;
        foreach (GameObject go in gos)
        {
            if (go.layer == 8 || go.layer == 9)  // 8 = Cubes, 9 = Hands
            {
                pos = go.transform.localPosition.z;

                pos -= 0.1f;

                if (pos < near) {
                    near = pos;
                    camera.nearClipPlane = near;
                    cameraObj.nearClipPlane = near;
                }

                pos += 0.2f;

                if (pos > far)
                {
                    far = pos;
                    camera.farClipPlane = far;
                    cameraObj.nearClipPlane = near;
                }
            }
        }
    }
}
