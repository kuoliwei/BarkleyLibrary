using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/BranchingStoryData")]
public class BranchingStoryData : ScriptableObject
{
    public string storyId;
    public Sprite bookImage;
    public PerformDataGroup introGroup;      // 開頭段 A（固定播放）
    public PerformDataGroup[] branchGroups;  // 結尾段 B~H（隨機一組播放）
}
