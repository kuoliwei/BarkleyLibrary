using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class JsonTest : MonoBehaviour
{
    [SerializeField] Text jsonTest;
    // Start is called before the first frame update
    void Start()
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Json.txt"));
        Debug.Log(json);
        ReceivedData.stringForTest = json;
        ReceivedData.bookData = JsonUtility.FromJson<BookData>(json);
        ReceivedData.isReceiveData = true;
        Debug.Log(ReceivedData.bookData.book.index);
        Debug.Log(ReceivedData.bookData.person.index);

        //jsonTest.text = $"Json content:{json} \n index:{ReceivedData.bookData.book.index}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
