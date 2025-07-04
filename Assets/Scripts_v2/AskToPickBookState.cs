using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AskToPickBookState : ICatState
{
    private CatStateMachine fsm;
    //private Coroutine askToPickBookRoutine;

    public void Enter(CatStateMachine fsm)
    {
        Debug.Log("Enter AskToPickBookState");
        this.fsm = fsm;
        //askToPickBookRoutine = fsm.RunCoroutine(PlayInstructions());

        fsm.SetStateMainRoutine(PlayInstructions());
        fsm.RunStateMainCoroutine();
    }

    public void HandleEvent(CatEvent catEvent)
    {
        if (catEvent == CatEvent.BookSelected)
        {
            fsm.TransitionTo(new ReadStoryState(0));
        }
        else if (catEvent == CatEvent.UserLeft)
        {
            fsm.TransitionTo(new SayByeState());
        }
    }

    public void Exit()
    {
        //if (askToPickBookRoutine != null)
        //    fsm.StopCoroutine(askToPickBookRoutine);

        fsm.StopStateMainCoroutine();
        fsm.StopPlayPerformGroupCoroutine();

        fsm.Presentation.Clear();
    }
    
    private IEnumerator PlayInstructions()
    {
        var group = fsm.Presentation.AskToPickBookGroup;

        //yield return fsm.RunCoroutine(fsm.Presentation.PlayPerformGroup(group));

        var routine = fsm.Presentation.PlayPerformGroup(group);
        fsm.SetPlayPerformGroupRoutine(routine);
        yield return fsm.RunPlayPerformGroupCoroutine();
        Debug.Log("AskToPickBookState finish");
        // 播完後就靜候事件觸發
    }
}