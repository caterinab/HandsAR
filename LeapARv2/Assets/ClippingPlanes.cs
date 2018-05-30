using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPlanes : MonoBehaviour
{
    GameObject[] gos;
    List<GameObject> gos2 = new List<GameObject>();
    float dist = 1f;
    public Camera cameraHands, cameraObjects;

    public void Farther()
    {
        cameraHands.farClipPlane += 0.01f;
        cameraObjects.farClipPlane += 0.01f;
        Debug.Log("far: " + cameraHands.farClipPlane);
    }


    public void Closer()
    {
        cameraHands.farClipPlane -= 0.01f;
        cameraObjects.farClipPlane -= 0.01f;
        Debug.Log("far: " + cameraHands.farClipPlane);
    }

    public void Start()
    {
    }

    public void Update()
    {
        /*
        gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        dist = 1f;

        foreach (GameObject go in gos2)
        {
            if (go.layer == 8 || go.layer == 9)
            {
                float d = Vector3.Distance(cameraHands.transform.position, go.transform.position);
                Debug.Log("d: " + d);
                if (d < dist)
                {
                    dist = d;
                }
            }
        }

        cameraHands.farClipPlane = dist;
        cameraObjects.farClipPlane = dist;*/
    }
}