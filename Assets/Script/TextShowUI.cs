using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShowUI : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();
    public Text text;
    public static TextShowUI TSU { get; private set; }

    private void Awake()
    {
        if (TSU == null)
        {
            TSU = this;
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
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    public void TextShow(string jsonString)
    {
        text.text += "\n" + DateTime.Now + ": " + jsonString;
    }
}
