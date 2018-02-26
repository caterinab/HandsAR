// ===============================
// AUTHOR     : Mirko Pani
// CREATE DATE     : 24/10/17
// PURPOSE     : Provider dei Frame usato da LeapServiceProvider per ottenere frame
// SPECIAL NOTES: SDK Orion v3.2
// ===============================
// Change History:
//
//==================================




using Leap;
using System.Collections.Generic;
using UnityEngine;

public class FrameProviderNew : MonoBehaviour
{
    WebsocketConnection customFrameProvider;
    public static List<Frame> frameBuffer;
    public int maxBufferSize = 100;
    public bool sorting = false;

    
    public Frame LatestFrame
    {
        get
        {
            return frameBuffer.Count > 2 ? frameBuffer[frameBuffer.Count - 1] : new Frame(); 
        }
        set { LatestFrame = value; }
    }


    void Start()
    {
        frameBuffer = new System.Collections.Generic.List<Frame>();
        customFrameProvider= GetComponent<WebsocketConnection>();
    }

   
    void Update()
    {
        while (customFrameProvider.buffer.Count > 0)
        {
            Frame newFrame = customFrameProvider.buffer[0];
            customFrameProvider.buffer.RemoveAt(0);

            if (newFrame != null)
            {
                //Ordina i frame secondo l'id
                if (sorting && frameBuffer.Count > 1)
                {
                    int i = frameBuffer.Count - 1;
                    while (frameBuffer[i].Id > newFrame.Id && i > 0)
                    {
                        i--;
                    }
                    frameBuffer.Insert(++i, newFrame);
                }
                else
                {
                    frameBuffer.Add(newFrame);
                }


                while (frameBuffer.Count > maxBufferSize)
                {
                    frameBuffer.RemoveAt(0);
                }
            }
        }
    }

}
