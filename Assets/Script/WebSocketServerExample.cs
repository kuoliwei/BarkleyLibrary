using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocketServerExample : MonoBehaviour
{
    private WebSocketServer wss;

    void Awake()
    {
        // create WebSocket Server, Listen on port 7777
        wss = new WebSocketServer(7777);
        wss.AddWebSocketService<Chat>("/Chat");
        wss.Start();
        Debug.Log("WebSocket server started on ws://localhost:7777/Chat");
        StartCoroutine(ServerStartNote());
    }

    void OnApplicationQuit()
    {
        if (wss != null)
        {
            wss.Stop();
        }
    }

    IEnumerator ServerStartNote()
    {
        yield return new WaitUntil(() => TextHandler.TH != null);
        TextHandler.TH.Enqueue(() => TextHandler.TH.ShowTextOnUI("WebSocket server started on ws://localhost:7777/Chat", "ffff00"));
        TextHandler.TH.Enqueue(() => TextHandler.TH.ShowTextOnUI("WebSocket server listening...", "ffff00"));
    }
}

public class Chat : WebSocketBehavior
{
    private Timer pingTimer;
    private static Dictionary<string, (string address, int port)> clientInfo = new Dictionary<string, (string address, int port)>();
    protected override void OnOpen()
    {
        Debug.Log("Client connected");
        var clientEndpoint = Context.UserEndPoint;
        string clientId = ID;
        clientInfo[clientId] = (clientEndpoint.Address.ToString(), clientEndpoint.Port);
        TextHandler.TH.Enqueue(() => TextHandler.TH.ShowTextOnUI($"client connected from: {clientEndpoint.Address}|Port[{clientEndpoint.Port}]", "00ffff"));
        pingTimer = new Timer(SendPing, null, 0, 10000);
        // 每10秒發送一次ping確認連線
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        TextHandler.TH.Enqueue(() => TextHandler.TH.ShowTextOnUI(e.Data, "00ff00"));
        Debug.Log($"Received message: {e.Data}");
        if (e.Data == "pong")
        {
            Debug.Log("Received pong from client");
        }
        else
        {
            Send($"Echo: {e.Data}");
        }
        //{
        //    ReceivedData.stringForTest = e.Data;
        //    ReceivedData.bookData = JsonUtility.FromJson<BookData>(e.Data);
        //    ReceivedData.isReceiveData = true;
        //    Debug.Log(ReceivedData.bookData.book.index);
        //    Debug.Log(ReceivedData.bookData.person.index);
        //}   //  v1

        {
            BookAndPersonData bookAndPersonData = JsonUtility.FromJson<BookAndPersonData>(e.Data);
            Debug.Log(bookAndPersonData.book);
            Debug.Log(bookAndPersonData.person);
        }   //  v2
    }

    protected override void OnClose(CloseEventArgs e)
    {
        if (clientInfo.TryGetValue(ID, out var info))
        {
            Debug.Log($"Connection closed: {e.Reason}");
            // var clientEndpoint = Context.UserEndPoint;
            TextHandler.TH.Enqueue(() => TextHandler.TH.ShowTextOnUI($"client disconnected from: {info.address}|Port[{info.port}]", "ff00ff"));
            clientInfo.Remove(ID);
        }
        pingTimer?.Dispose();
    }

    protected override void OnError(ErrorEventArgs e)
    {
        Debug.LogError($"WebSocket error: {e.Message}");
        Debug.LogError($"Exception: {e.Exception}");
        pingTimer?.Dispose();
    }

    private void SendPing(object state)
    {
        if (State == WebSocketState.Open)
        {
            Send("ping");
            Debug.Log("Ping sent to client");
        }
        else
        {
            pingTimer?.Dispose();
        }
    }
    // private void UpdateClientInfo()
    // {
    //     Debug.Log("Current connected clients:");
    //     foreach (var client in clientInfo)
    //     {
    //         // TextHandler.TH.Enqueue(() => TextHandler.TH.UpdateList($"Connect time : {client.Value.connectTime}, ID: {client.Key}, IP: {client.Value.address}, Port: {client.Value.port}"));
    //     }
    // }
}
