using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    private float delta = 0.005f;
    private float delta2 = 1f;
    private float delta3 = 0.01f;

    private void Start()
    {
        //impostazioni in base alle preferenze precedenti
        float x = PlayerPrefs.GetFloat("sensorX", 0);
        float y = PlayerPrefs.GetFloat("sensorY", 0);
        float z = PlayerPrefs.GetFloat("sensorZ", 0);
        GameObject.Find("LeapHandController").transform.localPosition = new Vector3(x, y, z);
        //PlayerPrefs.SetString("hostIP", GameObject.Find("LeapHandController").GetComponent<WebsocketConnection>().websocketIP);

        float quadX = PlayerPrefs.GetFloat("quadX", 0);
        float quadY = PlayerPrefs.GetFloat("quadY", 0);
        GameObject.Find("Quad").transform.localPosition = new Vector2(quadX, quadY);
    }

    public void LightDown()
    {
        GameObject.Find("DirectionalLight").GetComponent<Light>().intensity -= delta3;
    }

    public void LightUp()
    {
        GameObject.Find("DirectionalLight").GetComponent<Light>().intensity += delta3;
    }

    public void SaveQuadOffset(float x, float y)
    {
        PlayerPrefs.SetFloat("quadX", x);
        PlayerPrefs.SetFloat("quadY", y);
    }

    public void ResetQuad() {
        GameObject.Find("Quad").transform.localPosition = new Vector2(0, 0);
        SaveQuadOffset(0, 0);
    }

    public void QuadUp()
    {
        Vector2 p = GameObject.Find("Quad").transform.localPosition;

        p.y += delta2;

        GameObject.Find("Quad").transform.localPosition = p;

        SaveQuadOffset(p.x, p.y);
    }

    public void QuadDown()
    {
        Vector2 p = GameObject.Find("Quad").transform.localPosition;

        p.y -= delta2;

        GameObject.Find("Quad").transform.localPosition = p;

        SaveQuadOffset(p.x, p.y);
    }

    public void QuadRight()
    {
        Vector2 p = GameObject.Find("Quad").transform.localPosition;

        p.x += delta2;

        GameObject.Find("Quad").transform.localPosition = p;

        SaveQuadOffset(p.x, p.y);
    }

    public void QuadLeft()
    {
        Vector2 p = GameObject.Find("Quad").transform.localPosition;

        p.x -= delta2;

        GameObject.Find("Quad").transform.localPosition = p;

        SaveQuadOffset(p.x, p.y);
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
