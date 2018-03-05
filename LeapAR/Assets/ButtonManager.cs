using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private float delta = 0.005f;

    private void Start()
    {
        //impostazioni in base alle preferenze precedenti
        float x = PlayerPrefs.GetFloat("sensorX", 0);
        float y = PlayerPrefs.GetFloat("sensorY", 0);
        float z = PlayerPrefs.GetFloat("sensorZ", 0);
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(x, y, z);
        //PlayerPrefs.SetString("hostIP", GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().websocketIP);
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
        PlayerPrefs.SetFloat("sensorX", 0);
        PlayerPrefs.SetFloat("sensorY", 0);
        PlayerPrefs.SetFloat("sensorZ", 0);
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(0, 0, 0);
    }

    public void IncrementX() {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x += delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorX", p.x);
    }

    public void IncrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y += delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorY", p.y);
    }

    public void IncrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z += delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorZ", p.z);
    }

    public void DecrementX()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.x -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorX", p.x);
    }

    public void DecrementY()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.y -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorY", p.y);
    }

    public void DecrementZ()
    {
        Vector3 p = GameObject.Find("LeapHandController").transform.localPosition;

        p.z -= delta;

        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(p.x, p.y, p.z);

        PlayerPrefs.SetFloat("sensorZ", p.z);
    }
}
