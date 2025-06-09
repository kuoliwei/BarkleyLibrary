using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public static TextHandler TH { get; private set; }
    public Text UIText;
    private CultureInfo cultureInfo = new CultureInfo("en-US");
    private static readonly Queue<Action> executionQueue = new Queue<Action>();
    // public Text ConnectionList;
    // public GameObject AllConnections;
    private void Awake()
    {
        if (TH == null)
        {
            TH = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     Test();
        // }

    }

    // private void Test()
    // {
    //     ShowTextOnUI("TESTING");
    // }

    public void Clear()
    {
        UIText.text = "<color=#00ff00>WebSocket Server Received Messages:</color>";
    }
    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }
    public void ShowTextOnUI(string t, string color)
    {
        string time = $"<size=45><color=#{color}>{DateTime.Now.ToString(cultureInfo)}</color></size>";
        string info = $"<color=#{color}>{t}</color>";
        UIText.text += $"\n{time}\n{info}";
        if (UIText.preferredHeight > UIText.rectTransform.rect.height)
        {
            Clear();
            ShowTextOnUI(t, color);
        }
    }
    // public void UpdateList(string t)
    // {
    //     ConnectionList.text += t + "\n" + "-------------------------------" + "\n";
    // }

    // public void SwitchList()
    // {
    //     AllConnections.SetActive(!AllConnections.activeSelf);
    // }
}
