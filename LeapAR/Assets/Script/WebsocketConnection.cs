// ===============================
// AUTHOR     : Mirko Pani
// CREATE DATE     : 24/10/17
// PURPOSE     : Classe usata per connettersi al Websocket e per gestire e convertire i messaggi ricevuti.
// SPECIAL NOTES: SDK Orion v3.2 e leap json v6
// ===============================
// Change History:
// 5/11 Aggiunta possibilità di registrare frame su file di log
//==================================


using CustomLeap;
using Leap;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using WebSocketSharp;

public class WebsocketConnection : MonoBehaviour {
	public StateMachine stateMachine;
    public int maxBufferSize = 100;
    public bool isConnected;

    //Oggetti necessari per la cattura di frame
    private WebSocket ws;
    [HideInInspector]
    public List<Frame> buffer;
    private FrameConverter converter;

    //ws://192.168.1.101:6437/v6.json
    [Tooltip("Websocket server IP to connect to.")]

    public string websocketIP= "192.168.41.67";

    //Oggetti Per registrare su file i timestamp dei pacchetti
    StreamWriter writer;

    [Tooltip("When enabled, frames received and their timestamp will be written to a log file.\n Time received - original timestamp ")]
    public bool recordToLog = false;

    private string path = "Assets/Resources/log.txt";

    DateTime epoch = new DateTime(1970, 1, 1, 0/*h*/, 0/*m*/, 0/*s*/, DateTimeKind.Utc);

    void Start() {

        if(recordToLog)
            writer = new StreamWriter(path, true);

        buffer = new System.Collections.Generic.List<Frame>();
        //ws = new WebSocket("ws://"+websocketIP+ ":6437/v6.json");
        ws = new WebSocket("ws://" + websocketIP + ":6438");
        converter = new FrameConverter();

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

	private void OnOpenHandler(object sender, System.EventArgs e) {
		Debug.Log("WebSocket connected!");
        isConnected = true;
        stateMachine.Transition(State.Connected);
	}

	private void OnMessageHandler(object sender, MessageEventArgs e) {
       // writer.WriteLine((DateTime.UtcNow - epoch).TotalMilliseconds + " - BEFORE");
        string json= e.Data;
        
        /*Switch per il possibile json che ci arriva.
        Tre tipi:
        c: framedata
        e: event
        s: serviceVersion
        */
        switch(json[2])
        {
            case 'c': HandleJsonFrameData(json);break;
            case 'e': break;
            case 's': break;
            default:
                Debug.Log("Invalid json");
                break;
        }
	}

	private void OnCloseHandler(object sender, CloseEventArgs e) {
		Debug.Log("WebSocket closed with reason: " + e.Reason);

        if(recordToLog)
            writer.Close();

        stateMachine.Transition(State.Done);
	}

	private void OnSendComplete(bool success) {
		Debug.Log("Message sent successfully? " + success);
	}

    private void HandleJsonFrameData(string jsonFrame)
    {
 
       Frame frame = converter.ConvertFromString(jsonFrame);

        if (recordToLog)
            writer.WriteLine((DateTime.UtcNow-epoch).TotalMilliseconds + "-"+frame.Timestamp);

        buffer.Add(frame);
    }

    public Frame LatestFrame
    {
        get
        {
            return buffer.Count > 2 ? buffer[buffer.Count - 1] : new Frame();
        }
        set { LatestFrame = value; }
    }

    void Update()
    {
        while (buffer.Count > maxBufferSize)
        {
            buffer.RemoveAt(0);
        }
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
    
    void ConnectClient() {
        stateMachine.Run();
    }

    void DisconnectClient()
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
