using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class HandDepthTest : MonoBehaviour
{
    public GameObject palmL, palmR, cube, quad, cam;
    float palmLDist, palmRDist, cubeDist;

    void Start()
    {
    }

    void Update()
    {
        palmLDist = Vector3.Distance(palmL.transform.position, cam.transform.position);
        palmRDist = Vector3.Distance(palmR.transform.position, cam.transform.position);
        cubeDist = Vector3.Distance(cube.transform.position, cam.transform.position);

        //if ((palmLPos.z*(-1000) > cubePos.z) || (palmRPos.z*(-1000) > cubePos.z))
        if ((palmLDist > cubeDist) || (palmRDist > cubeDist))
        {
            if (cube.GetComponent<Renderer>() != null)
            {
                cube.GetComponent<Renderer>().material.renderQueue = 2001;
            }
            else
            {
                foreach (Renderer r in cube.GetComponentsInChildren<Renderer>())
                {
                    r.material.renderQueue = 2001;
                }
            }
        }
        else
        {
            if (cube.GetComponent<Renderer>() != null)
            {
                cube.GetComponent<Renderer>().material.renderQueue = 1999;
            }
            else
            {
                foreach (Renderer r in cube.GetComponentsInChildren<Renderer>())
                {
                    r.material.renderQueue = 1999;
                }
            }
        }
        Debug.Log("palm " + palmLDist + ", cube " + cubeDist);
    }
}