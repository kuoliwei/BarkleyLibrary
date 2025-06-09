using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPServer : MonoBehaviour
{
    private TcpListener server;
    private Thread serverThread;

    void Start()
    {
        serverThread = new Thread(new ThreadStart(StartServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void StartServer()
    {
        try
        {
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
            Debug.Log("Server started...");

            Byte[] bytes = new Byte[256];
            String data = null;

            while (true)
            {
                Debug.Log("Waiting for a connection...");
                TcpClient client = server.AcceptTcpClient();
                Debug.Log("Connected!");

                data = null;
                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Debug.Log("Received: " + data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Debug.Log("Sent: " + data);
                }

                client.Close();
            }
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.ToString());
        }
        finally
        {
            server.Stop();
        }
    }

    void OnApplicationQuit()
    {
        if (server != null)
        {
            server.Stop();
        }
        if (serverThread != null)
        {
            serverThread.Abort();
        }
    }
}
