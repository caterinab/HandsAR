using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class OpenCVImport : MonoBehaviour
{
    //Lets make our calls from the Plugin
    [DllImport("OpenCVProject")]
    private static extern int openCVFunction();

    void Start()
    {
        openCVFunction();
    }
}
