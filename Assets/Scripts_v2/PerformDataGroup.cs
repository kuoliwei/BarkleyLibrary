using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Story/PerformDataGroup")]
public class PerformDataGroup : ScriptableObject
{
    public string groupId;                   // 識別代號
    public PerformData[] lines;              // 每句內容（文字 + 語音）
}
