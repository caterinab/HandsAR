using Leap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour
{
    public StateMachine stateMachine;
    public int maxBufferSize = 10;
    private bool isConnected;

    //Oggetti necessari per la cattura di frame
    private WebSocket ws;
    [HideInInspector]
    List<Frame> buffer;  // stack

    //ws://192.168.1.101:6437/v6.json
    [Tooltip("Websocket server IP to connect to.")]

    //public string websocketIP= "192.168.41.67";
    public string websocketIP = "192.168.85.215";

    void Start()
    {
        buffer = new List<Frame>();
        ws = new WebSocket("ws://" + websocketIP + ":6438");

        ws.OnOpen += OnOpenHandler;
        ws.OnMessage += OnMessageHandler;
        ws.OnClose += OnCloseHandler;

        stateMachine.AddHandler(State.Running, () => {
            ws.ConnectAsync();
        });

        stateMachine.AddHandler(State.Connected, () => {
            stateMachine.Transition(State.Ping);
        });

        stateMachine.AddHandler(State.Ping, () => {
            ws.SendAsync("Connected", OnSendComplete);
        });

        stateMachine.AddHandler(State.Pong, () => {
            ws.CloseAsync();
        });

        ConnectClient();
    }

    private void OnOpenHandler(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected!");
        isConnected = true;
        stateMachine.Transition(State.Connected);
    }

    private void OnMessageHandler(object sender, MessageEventArgs e)
    {
        Frame f = FrameJSONConverter.JSONToFrame(e.Data); 
        buffer.Add(f);
    }

    private void OnCloseHandler(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket closed with reason: " + e.Reason);
        
        stateMachine.Transition(State.Done);
    }

    private void OnSendComplete(bool success)
    {
        Debug.Log("Message sent successfully? " + success);
    }
    
    public Frame GetLatestFrame()
    {
        // return the newest converted frame
        return buffer.Count > 2 ? buffer[buffer.Count - 1] : null;
    }

    void Update()
    {
        if (buffer.Count >= maxBufferSize)
            buffer.RemoveRange(0, maxBufferSize / 2);

        /*
        while (buffer.Count >= maxBufferSize) {
            buffer.RemoveAt(0);
        }
        */
    }

    void OnApplicationFocus(bool focus)
    {
#if !UNITY_EDITOR
            if (focus && !this.isConnected)
            {
                this.ConnectClient();
            }
            else if (!focus && this.isConnected)
            {
                this.DisconnectClient();
            }
#endif
    }

    void OnApplicationPause(bool pause)
    {
#if !UNITY_EDITOR
            if (pause && this.isConnected)
            {
                this.DisconnectClient();
            }
            else if (!pause && !this.isConnected)
            {
                this.ConnectClient();
            }
#endif
    }

    public void ConnectClient()
    {
        stateMachine.Run();
    }

    public void DisconnectClient()
    {
        ws.SendAsync("Disconnecting", OnSendComplete);
        isConnected = false;
        ws.CloseAsync();
    }

    void OnApplicationQuit()
    {
        DisconnectClient();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
