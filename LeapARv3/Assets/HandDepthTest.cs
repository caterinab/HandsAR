using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class HandDepthTest : MonoBehaviour
{
    public GameObject palmL, palmR, fingersL, fingersR, cube, quad, cam;
    float palmLDist, palmRDist, cubeDist, fLDist, fRDist;

    void Start()
    {
    }

    void Update()
    {
        palmLDist = Vector3.Distance(palmL.transform.position, cam.transform.position);
        palmRDist = Vector3.Distance(palmR.transform.position, cam.transform.position);
        fLDist = Vector3.Distance(fingersL.transform.position, cam.transform.position);
        fRDist = Vector3.Distance(fingersR.transform.position, cam.transform.position);
        cubeDist = Vector3.Distance(cube.transform.position, cam.transform.position);
        
        if ((palmLDist > cubeDist) || (palmRDist > cubeDist))
        {
            if ((fLDist > cubeDist) || (fRDist > cubeDist))
            {
                cube.GetComponent<Renderer>().material.renderQueue = 2003;
            }
            else
            {
                cube.GetComponent<Renderer>().material.renderQueue = 2001;
            }
        }
        else
        {
            cube.GetComponent<Renderer>().material.renderQueue = 1999;
        }
        /*
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
        */
    }
}