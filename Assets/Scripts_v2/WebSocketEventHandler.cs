using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebSocketEventHandler : MonoBehaviour
{
    [SerializeField] CatStateMachine catStateMachine;
    [SerializeField] Text jsonText;
    BookAndPersonData bookAndPersonData;
    void OnEnable()
    {
        ChatForBarkleyLibrary.OnWebSocketMessageReceived += HandleWebSocketMessage;
    }

    void OnDisable()
    {
        ChatForBarkleyLibrary.OnWebSocketMessageReceived -= HandleWebSocketMessage;
    }

    private void Update()
    {
        jsonText.text = $"book:{bookAndPersonData.book}, person:{bookAndPersonData.person}";
        if (bookAndPersonData.person == "null")
        {
            if (catStateMachine.CurrentCatState == CatState.ReadStoryState)
            {
                // 中途離席 → 馬上說再見
                catStateMachine.switchToSayByeState();
            }
            else if (catStateMachine.CurrentCatState != CatState.GreetingState)
            {
                //Debug.Log(catStateMachine.CurrentCatState);
                Debug.Log("重覆與民眾打招呼");
                catStateMachine.switchToGreetingState();
            }
            return;
        }
        if (bookAndPersonData.person == "sit-down")
        {
            if (bookAndPersonData.book == "null")
            {
                if (catStateMachine.CurrentCatState != CatState.AskToPickBookState)
                {
                    //Debug.Log(catStateMachine.CurrentCatState);
                    Debug.Log("民眾已入座，開始介紹書本");
                    catStateMachine.switchToAskToPickBookState();
                }
                return;
            }
            else
            {
                // 嘗試解析 book 為整數
                if (!int.TryParse(bookAndPersonData.book, out int result) || result < 0 || result > 2)
                {
                    Debug.LogWarning($"[WebSocket] book 非法數字：{bookAndPersonData.book}");
                    return;
                }

                if (catStateMachine.CurrentCatState != CatState.ReadStoryState)
                {
                    catStateMachine.switchToReadStoryState(result);
                }
                else
                {
                    ReadStoryState readStoryState = catStateMachine.CurrentState as ReadStoryState;
                    if (result != readStoryState.CurrentBookIndex)
                    {
                        catStateMachine.switchToReadStoryState(result);
                    }
                }
                return;
            }
        }
    }

    void HandleWebSocketMessage(string message)
    {
        BookAndPersonData bookAndPersonData;
        try
        {
            bookAndPersonData = JsonUtility.FromJson<BookAndPersonData>(message);
            if(string.IsNullOrWhiteSpace(bookAndPersonData.book) || string.IsNullOrWhiteSpace(bookAndPersonData.person))
            {
                Debug.Log("Data of book or person is illegal");
                return;
            }
            else
            {
                this.bookAndPersonData = bookAndPersonData;
                //Debug.Log($"book:{bookAndPersonData.book}, person:{bookAndPersonData.person}");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return;
        }

        //if (bookAndPersonData.person == "null")
        //{
        //    if (catStateMachine.CurrentCatState == CatState.ReadStoryState)
        //    {
        //        // 中途離席 → 馬上說再見
        //        catStateMachine.switchToSayByeState();
        //    }
        //    else if (catStateMachine.CurrentCatState != CatState.GreetingState)
        //    {
        //        //Debug.Log(catStateMachine.CurrentCatState);
        //        Debug.Log("重覆與民眾打招呼");
        //        catStateMachine.switchToGreetingState();
        //    }
        //    return;
        //}
        //if (bookAndPersonData.person == "sit-down")
        //{
        //    if(bookAndPersonData.book == "null")
        //    {
        //        if (catStateMachine.CurrentCatState != CatState.AskToPickBookState)
        //        {
        //            //Debug.Log(catStateMachine.CurrentCatState);
        //            Debug.Log("民眾已入座，開始介紹書本");
        //            catStateMachine.switchToAskToPickBookState();
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        // 嘗試解析 book 為整數
        //        if (!int.TryParse(bookAndPersonData.book, out int result) || result < 0 || result > 2)
        //        {
        //            Debug.LogWarning($"[WebSocket] book 非法數字：{bookAndPersonData.book}");
        //            return;
        //        }

        //        if (catStateMachine.CurrentCatState != CatState.ReadStoryState)
        //        {
        //            catStateMachine.switchToReadStoryState(result);
        //        }
        //        else
        //        {
        //            ReadStoryState readStoryState = catStateMachine.CurrentState as ReadStoryState;
        //            if (result != readStoryState.CurrentBookIndex)
        //            {
        //                catStateMachine.switchToReadStoryState(result);
        //            }
        //        }
        //        return;
        //    }
        //}
    }
}
