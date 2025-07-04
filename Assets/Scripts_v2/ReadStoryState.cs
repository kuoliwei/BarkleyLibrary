using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadStoryState : ICatState
{
    private CatStateMachine fsm;
    //private Coroutine readStoryRoutine;
    private int currentBookIndex;
    public int CurrentBookIndex => currentBookIndex;

    public ReadStoryState(int bookIndex)
    {
        this.currentBookIndex = bookIndex;
    }
    public void Enter(CatStateMachine fsm)
    {
        Debug.Log("Enter ReadStoryState");
        this.fsm = fsm;
        // ��ܹϤ�
        var story = fsm.Presentation.BranchingStorys[currentBookIndex];
        fsm.Presentation.BookImage.Show(story.bookImage);

        //readStoryRoutine = fsm.RunCoroutine(PlayBranchingStory());

        fsm.SetStateMainRoutine(PlayBranchingStory());
        fsm.RunStateMainCoroutine();
    }

    public void HandleEvent(CatEvent catEvent)
    {
        if (catEvent == CatEvent.UserLeft)
        {
            fsm.TransitionTo(new SayByeState());
        }
    }

    public void Exit()
    {
        //if (readStoryRoutine != null)
        //    fsm.StopCoroutine(readStoryRoutine);

        fsm.StopStateMainCoroutine();
        fsm.StopPlayPerformGroupCoroutine();

        fsm.Presentation.Clear();
    }

    private IEnumerator PlayBranchingStory()
    {
        var stories = fsm.Presentation.BranchingStorys;

        if (stories == null || currentBookIndex < 0 || currentBookIndex >= stories.Length)
        {
            Debug.LogWarning($"�L�Ī� bookIndex�]{currentBookIndex}�^�A�Ω|���]�w BranchingStorys");
            yield break;
        }

        var story = stories[currentBookIndex];
        var introRoutine = fsm.Presentation.PlayPerformGroup(story.introGroup);
        fsm.SetPlayPerformGroupRoutine(introRoutine);
        yield return fsm.RunPlayPerformGroupCoroutine();

        // �H������@�q Branch �q�]B~H�^
        if (story.branchGroups != null && story.branchGroups.Length > 0)
        {
            int i = Random.Range(0, story.branchGroups.Length);
            var branchRoutine = fsm.Presentation.PlayPerformGroup(story.branchGroups[i]);
            fsm.SetPlayPerformGroupRoutine(branchRoutine);
            yield return fsm.RunPlayPerformGroupCoroutine();
        }
        yield return new WaitForSeconds(1f);
        // �G�Ƽ��� �� �������A
        fsm.switchToSayByeState();
    }


    private IEnumerator PlayGroup(PerformDataGroup group)
    {
        yield return fsm.RunCoroutine(fsm.Presentation.PlayPerformGroup(group));
    }
}
