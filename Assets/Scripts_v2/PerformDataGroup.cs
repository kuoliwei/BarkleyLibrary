using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Story/PerformDataGroup")]
public class PerformDataGroup : ScriptableObject
{
    public string groupId;                   // �ѧO�N��
    public PerformData[] lines;              // �C�y���e�]��r + �y���^
}
