using UnityEngine;
using WebSocketSharp;
using System.Threading;

public class WebSocketClientMod : MonoBehaviour
{
    private WebSocket ws;
    private Timer pongTimer;
    private bool isConnected;

    void Start()
    {
        ws = new WebSocket("ws://localhost:7777/Chat");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected.");
            isConnected = true;
            ws.Send("Hello, Server!");

            // 定期检查连接状态
            pongTimer = new Timer(CheckConnection, null, 0, 10000); // 每10秒检查一次连接
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message from server: " + e.Data);
            if (e.Data == "ping")
            {
                ws.Send("pong");
                Debug.Log("Pong sent to server");
            }
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log($"WebSocket closed: {e.Reason}");
            isConnected = false;
            pongTimer?.Dispose();
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError($"WebSocket error: {e.Message}");
            isConnected = false;
            pongTimer?.Dispose();
        };

        try
        {
            ws.Connect();
            Debug.Log("WebSocket connecting...");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception during WebSocket connection: " + ex.Message);
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
            Debug.LogError("Connection lost. Attempting to reconnect...");
            try
            {
                ws.Connect();
                Debug.Log("Reconnecting...");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Reconnection failed: " + ex.Message);
            }
        }
        else
        {
            isConnected = true;
        }
    }
}
