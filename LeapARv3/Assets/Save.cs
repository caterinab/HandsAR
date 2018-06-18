using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour {

    int save = 0;
    Texture2D screenshot;
    public RenderTexture rtHands, rtCubes;

    // Use this for initialization
    void Start ()
    {
        screenshot = new Texture2D(1280, 720, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture.active = rtHands;
        screenshot.ReadPixels(new Rect(0, 0, rtHands.width, rtHands.height), 0, 0);
        screenshot.Apply();

        byte[] hands = screenshot.GetRawTextureData();

        Debug.Log("Saving 1...");
        File.WriteAllBytes("C:\\Users\\cbattisti\\Documents\\hands_depth" + save + ".png", screenshot.EncodeToPNG());

        RenderTexture.active = rtCubes;
        screenshot.ReadPixels(new Rect(0, 0, rtCubes.width, rtCubes.height), 0, 0);
        screenshot.Apply();

        byte[] cubes = screenshot.GetRawTextureData();

        Debug.Log("Saving 2...");
        File.WriteAllBytes("C:\\Users\\cbattisti\\Documents\\cube_depth" + save + ".png", screenshot.EncodeToPNG());

        save++;
    }
}
