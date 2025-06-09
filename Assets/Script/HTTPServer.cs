using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Net.Sockets;

public class SimpleHTTPServer : MonoBehaviour
{
    private HttpListener listener;
    private Thread serverThread;
    private string htmlFilePath;
    public int port = 7777; // 端口号，可以根据需要更改

    void Start()
    {
        // 获取HTML文件的路径
        htmlFilePath = Path.Combine(Application.dataPath, "index.html");

        serverThread = new Thread(new ThreadStart(StartServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void StartServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        try
        {
            listener.Start();
            Debug.Log($"HTTP Server started on port {port}");

            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string responseString = File.ReadAllText(htmlFilePath);
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
        }
        catch (HttpListenerException e)
        {
            Debug.LogError("HttpListenerException: " + e.Message);
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.Message);
        }
        catch (ThreadAbortException e)
        {
            Debug.LogWarning("Thread aborted: " + e.Message);
        }
        finally
        {
            if (listener != null && listener.IsListening)
            {
                listener.Stop();
                listener.Close();
            }
        }
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    void OnDisable()
    {
        StopServer();
    }

    void StopServer()
    {
        if (listener != null)
        {
            listener.Stop();
            listener.Close();
            listener = null;
        }

        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Abort();
        }
        Debug.Log("HTTP Server stopped.");
    }
}
