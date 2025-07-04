using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPresentationController : MonoBehaviour
{
    [SerializeField] private AudioController audioController;
    [SerializeField] private TextController textController;
    [SerializeField] private PerformDataGroup greetingGroup;
    [SerializeField] private PerformDataGroup askToPickBookGroup;
    [SerializeField] private BranchingStoryData[] branchingStorys;
    [SerializeField] private PerformDataGroup sayByeGroup;
    [SerializeField] private ImageController bookImageController;

    public AudioController Audio => audioController;
    public TextController Text => textController;
    public PerformDataGroup GreetingGroup => greetingGroup;
    public PerformDataGroup AskToPickBookGroup => askToPickBookGroup;
    public BranchingStoryData[] BranchingStorys => branchingStorys;
    public PerformDataGroup SayByeGroup => sayByeGroup;
    public ImageController BookImage => bookImageController;

    public void Say(string content)
    {
        Debug.Log("[小貓說] " + content);
        textController?.ShowText(content);
        // 可搭配播放語音
    }

    public void Clear()
    {
        textController?.Clear();
        audioController?.Stop();
        bookImageController?.Hide();
    }
    public IEnumerator PlayPerformGroup(PerformDataGroup group)
    {
        if (group == null || group.lines == null || group.lines.Length == 0)
            yield break;

        foreach (var line in group.lines)
        {
            if (line == null) continue;

            Text.ShowText(line.text);
            Audio.PlayAudio(line.audioClip);

            float duration = line.audioClip != null ? line.audioClip.length : 1f;
            yield return new WaitForSeconds(duration);

            Text.Clear();
        }
    }
}
