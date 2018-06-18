using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameJSONConverter : MonoBehaviour {
    public string FrameToJSON(Frame frame)
    {
        return JsonUtility.ToJson(frame);
    }

    public Frame JSONToFrame(string json)
    {
        return JsonUtility.FromJson<Frame>(json);
    }
}
