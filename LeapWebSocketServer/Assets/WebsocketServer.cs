// ===============================
// AUTHOR     : Mirko Pani
// CREATE DATE     : 24/10/17
// PURPOSE     : Classe usata per connettersi al Websocket e per gestire e convertire i messaggi ricevuti.
// SPECIAL NOTES: SDK Orion v3.2 e leap json v6
// ===============================
// Change History:
// 5/11 Aggiunta possibilità di registrare frame su file di log
//==================================

using Leap;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using WebSocketSharp;
using System.Threading;
using WebSocketSharp.Server;

public class Laputa : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("Client says: " + e.Data);
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        Debug.Log("Client disconnesso");
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
        base.OnError(e);
        Debug.Log("Error: " + e.Message);
    }
}


public class WebsocketServer : MonoBehaviour
{
    static WebSocketServer wssv;
    static int contatore = 0;
    public FrameJSONConverter converter;

    public void Start()
    {
        wssv = new WebSocketServer(System.Net.IPAddress.Any, 6438);
        wssv.AddWebSocketService<Laputa>("/");
        
        wssv.Start();
        
        Debug.Log("Avviato server.");
    }
    
    public void Send(Frame data)
    {
        ++contatore;
        contatore %= 5;
        if (contatore == 0)
        {
            if (wssv.IsListening)
            {
                string s = converter.FrameToJSON(data);
                wssv.WebSocketServices.Broadcast(s);
                Debug.Log(s);
            }
        }
    }
}