using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameJSONConverter : MonoBehaviour {
    public static string FrameToJSON(Frame frame)
    {
        return JsonUtility.ToJson(frame);
    }

    public static Frame JSONToFrame(string json)
    {
        return JsonUtility.FromJson<Frame>(json);
    }
}
