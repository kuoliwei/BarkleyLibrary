using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPerformData", menuName = "Story/PerformData")]
public class PerformData : ScriptableObject
{
    [TextArea]
    public string text;
    public AudioClip audioClip;
}
