using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private float delta = 0.005f;
    private float delta2 = 1f;
    private float delta3 = 0.001f;

    private void Start()
    {
        //impostazioni in base alle preferenze precedenti
        float x = PlayerPrefs.GetFloat("sensorX", 0);
        float y = PlayerPrefs.GetFloat("sensorY", 0);
        float z = PlayerPrefs.GetFloat("sensorZ", 0);
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(x, y, z);
        Debug.Log("Sensor: " + x + " " + y + " " + z);

        if (GameObject.Find("QuadHand") != null)
        {
            float quadHandX = PlayerPrefs.GetFloat("quadHandX", 0);
            float quadHandY = PlayerPrefs.GetFloat("quadHandY", 0);
            GameObject.Find("QuadHand").transform.localPosition = new Vector3(quadHandX, quadHandY, 0);
            Debug.Log("Hand texture: " + quadHandX + " " + quadHandY);
        }

        float camY = PlayerPrefs.GetFloat("camY", 0);
        GameObject.Find("Camera").transform.localPosition = new Vector3(0, camY, 0);
        GameObject.Find("CameraObj").transform.localPosition = new Vector3(0, camY, 0);
        Debug.Log("CamY: " + camY);

    }

    public void CamUp()
    {
        Vector3 p = GameObject.Find("Camera").transform.localPosition;

        p.y += delta3;

        GameObject.Find("Camera").transform.localPosition = p;
        GameObject.Find("CameraObj").transform.localPosition = p;
        PlayerPrefs.SetFloat("camY", p.y);
        PlayerPrefs.Save();
        Debug.Log("CamY: " + p.y);
    }

    public void CamDown()
    {
        Vector3 p = GameObject.Find("Camera").transform.localPosition;

        p.y -= delta3;

        GameObject.Find("Camera").transform.localPosition = p;
        GameObject.Find("CameraObj").transform.localPosition = p;
        PlayerPrefs.SetFloat("camY", p.y);
        PlayerPrefs.Save();
        Debug.Log("CamY: " + p.y);
    }

    public void ResetCam()
    {
        GameObject.Find("Camera").transform.localPosition = new Vector3(0, 0, 0);
        GameObject.Find("CameraObj").transform.localPosition = new Vector3(0, 0, 0);
        PlayerPrefs.Save();
    }

    public void SaveQuadHandOffset(float x, float y)
    {
        PlayerPrefs.SetFloat("quadHandX", x);
        PlayerPrefs.SetFloat("quadHandY", y);
        PlayerPrefs.Save();
    }

    public void ResetQuadHand() {
        GameObject.Find("QuadHand").transform.localPosition = new Vector3(0, 0, 0);
        SaveQuadHandOffset(0, 0);
    }

    public void QuadHandUp()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.y += delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
        Debug.Log("Hand texture: " + p.x + " " + p.y);
    }

    public void QuadHandDown()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.y -= delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
        Debug.Log("Hand texture: " + p.x + " " + p.y);
    }

    public void QuadHandRight()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.x += delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
    }

    public void QuadHandLeft()
    {
        Vector3 p = GameObject.Find("QuadHand").transform.localPosition;

        p.x -= delta2;

        GameObject.Find("QuadHand").transform.localPosition = p;

        SaveQuadHandOffset(p.x, p.y);
    }

    public void StartMainScene() {
        GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().DisconnectClient();
        SceneManager.LoadSceneAsync("main");
    }

    public void StartMenuScene()
    {
        GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().DisconnectClient();
        SceneManager.LoadSceneAsync("menu");
    }

    public void ResetOffset()
    {
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(0, 0, 0);
    }

    public void SaveOffset() {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;
        PlayerPrefs.SetFloat("sensorX", p.x);
        PlayerPrefs.SetFloat("sensorY", p.y);
        PlayerPrefs.SetFloat("sensorZ", p.z);
        PlayerPrefs.Save();
    }

    public void IncrementX() {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void IncrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void IncrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z += delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementX()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }

    public void DecrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = p;
    }
}
