using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class HandDepthTest : MonoBehaviour
{
    public GameObject palmL, palmR, cube, quad;
    Vector3 palmLPos, palmRPos, cubePos;

    void Start()
    {/*
        palmL = GameObject.Find("RigidRoundHand_L").transform.GetChild(5).gameObject;
        palmR = GameObject.Find("RigidRoundHand_R").transform.GetChild(5).gameObject;
        cube =  GameObject.Find("Cube");
        quad = GameObject.Find("Canvas").transform.GetChild(0).gameObject;*/
    }

    void Update()
    {
        palmLPos = palmL.transform.localPosition;
        palmRPos = palmR.transform.localPosition;
        cubePos = cube.transform.position;

        if ((palmLPos.z*1000 > cubePos.z) || (palmRPos.z*1000 > cubePos.z))
        {
            quad.GetComponent<Renderer>().material.renderQueue = 1999;
        }
        else
        {
            quad.GetComponent<Renderer>().material.renderQueue = 3000;
        }
        Debug.Log("palm " + palmLPos.z*1000 + ", cube " + cubePos.z);
    }
}