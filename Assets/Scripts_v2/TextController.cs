using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] private Text textUI;

    /// <summary>
    /// 直接顯示指定文字
    /// </summary>
    public void ShowText(string content)
    {
        if (textUI != null)
            textUI.text = content;
    }

    /// <summary>
    /// 清除畫面上的文字
    /// </summary>
    public void Clear()
    {
        if (textUI != null)
            textUI.text = "";
    }

    /// <summary>
    /// 回傳目前顯示的內容（可選）
    /// </summary>
    public string CurrentText => textUI?.text;
}