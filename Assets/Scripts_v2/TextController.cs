using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] private Text textUI;

    /// <summary>
    /// ������ܫ��w��r
    /// </summary>
    public void ShowText(string content)
    {
        if (textUI != null)
            textUI.text = content;
    }

    /// <summary>
    /// �M���e���W����r
    /// </summary>
    public void Clear()
    {
        if (textUI != null)
            textUI.text = "";
    }

    /// <summary>
    /// �^�ǥثe��ܪ����e�]�i��^
    /// </summary>
    public string CurrentText => textUI?.text;
}