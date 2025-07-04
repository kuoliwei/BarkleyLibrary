using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story/BranchingStoryData")]
public class BranchingStoryData : ScriptableObject
{
    public string storyId;
    public Sprite bookImage;
    public PerformDataGroup introGroup;      // �}�Y�q A�]�T�w����^
    public PerformDataGroup[] branchGroups;  // �����q B~H�]�H���@�ռ���^
}
