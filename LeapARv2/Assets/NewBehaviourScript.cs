using UnityEngine;
using System.Runtime.InteropServices;

internal static class OpenCV4AndroidInterop
{
    [DllImport("opencv2unity")]
    internal unsafe static extern int ocv_get_image(int xres, int yres, uint8_t* z, int* delay, int filt, void** result2);
}

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
