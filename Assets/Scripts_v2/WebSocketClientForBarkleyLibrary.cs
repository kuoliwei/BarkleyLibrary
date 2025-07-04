using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading;

public class WebSocketClientForBarkleyLibrary : MonoBehaviour
{
    private WebSocket ws;
    private Timer pongTimer;
    private bool isConnected;
    BookAndPersonData data;
    void Start()
    {
        data = new BookAndPersonData
        {
            person = "null",
            book = "null"
        };
        ws = new WebSocket("ws://localhost:7777/Chat");

        ws.OnOpen += (sender, e) =>
        {
            //Debug.Log("WebSocket connected.");
            isConnected = true;
            ws.Send("Hello, Server!");

            // 定期檢查連接狀態
            pongTimer = new Timer(CheckConnection, null, 0, 10000); // 每十秒檢查一次連接狀態
        };

        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("Message from server: " + e.Data);
            if (e.Data == "ping")
            {
                ws.Send("pong");
                //Debug.Log("Pong sent to server");
            }
        };

        ws.OnClose += (sender, e) =>
        {
            //Debug.Log($"WebSocket closed: {e.Reason}");
            isConnected = false;
            pongTimer?.Dispose();
        };

        ws.OnError += (sender, e) =>
        {
            //Debug.LogError($"WebSocket error: {e.Message}");
            isConnected = false;
            pongTimer?.Dispose();
        };

        try
        {
            ws.Connect();
            //Debug.Log("WebSocket connecting...");
        }
        catch (System.Exception ex)
        {
            //Debug.LogError("Exception during WebSocket connection: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
        pongTimer?.Dispose();
    }

    private void CheckConnection(object state)
    {
        if (ws == null || !ws.IsAlive)
        {
            isConnected = false;
            //Debug.LogError("Connection lost. Attempting to reconnect...");
            try
            {
                ws.Connect();
                //Debug.Log("Reconnecting...");
            }
            catch (System.Exception ex)
            {
                //Debug.LogError("Reconnection failed: " + ex.Message);
            }
        }
        else
        {
            isConnected = true;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            data.person = data.person == "null" ? "sit-down" : "null";
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            data.book = data.book == "null" ? "0" : "null";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            data.book = data.book == "null" ? "1" : "null";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            data.book = data.book == "null" ? "2" : "null";
        }
        ws.Send(JsonUtility.ToJson(data));
    }
}
