using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Net.Sockets;

public class WebDataReceiver : MonoBehaviour
{
    private HttpListener listener;
    private Thread serverThread;
    public string htmlFilePath = "index.html"; // 确保您的HTML文件路径正确
    public int port = 7777; // 端口号

    void Start()
    {
        htmlFilePath = Path.Combine(Application.dataPath, "index.html");
        Debug.Log("HTML File Path: " + htmlFilePath);
        serverThread = new Thread(new ThreadStart(StartServer))
        {
            IsBackground = true
        };
        serverThread.Start();
    }

    void StartServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        listener.Prefixes.Add($"http://localhost:{port}/");
        try
        {
            listener.Start();
            Debug.Log($"HTTP Server started on port {port}");

            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");

                if (request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string jsonText = reader.ReadToEnd();
                        Debug.Log("Received JSON data: " + jsonText);

                        JsonData jsonData = JsonUtility.FromJson<JsonData>(jsonText);
                        string data = jsonData.data;
                        Debug.Log("Parsed Data: " + data);

                        TextShowUI.TSU.Enqueue(() => TextShowUI.TSU.TextShow(data));
                    }
                    byte[] responseBytes = Encoding.UTF8.GetBytes("Received");
                    response.ContentType = "text/plain";
                    response.ContentLength64 = responseBytes.Length;
                    response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                }
                else if (request.HttpMethod == "GET")
                {
                    if (File.Exists(htmlFilePath))
                    {
                        string responseString = File.ReadAllText(htmlFilePath);
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        response.ContentType = "text/html";
                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.StatusDescription = "File Not Found";
                        byte[] buffer = Encoding.UTF8.GetBytes("404 - File Not Found");
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
                else if (request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = 200;
                    response.StatusDescription = "OK";
                }
                else
                {
                    response.StatusCode = 405; // Method Not Allowed
                    response.StatusDescription = "Method Not Allowed";
                }

                response.Close();
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
    [System.Serializable]
    public class JsonData
    {
        public string data;
    }
}
