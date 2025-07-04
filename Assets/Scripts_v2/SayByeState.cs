using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayByeState : ICatState
{
    private CatStateMachine fsm;
    //private Coroutine sayByeRoutine;

    public void Enter(CatStateMachine fsm)
    {
        Debug.Log("Enter SayByeState");
        this.fsm = fsm;
        //sayByeRoutine = fsm.RunCoroutine(PlayGoodbye());

        fsm.SetStateMainRoutine(PlayGoodbye());
        fsm.RunStateMainCoroutine();
    }

    public void HandleEvent(CatEvent catEvent)
    {
        // 此狀態不再處理事件（靜止）
    }

    public void Exit()
    {
        //if (sayByeRoutine != null)
        //    fsm.StopCoroutine(sayByeRoutine);

        fsm.StopStateMainCoroutine();
        fsm.StopPlayPerformGroupCoroutine();

        fsm.Presentation.Clear();
    }

    private IEnumerator PlayGoodbye()
    {
        var group = fsm.Presentation.SayByeGroup;

        //yield return fsm.RunCoroutine(fsm.Presentation.PlayPerformGroup(group));

        var routine = fsm.Presentation.PlayPerformGroup(group);
        fsm.SetPlayPerformGroupRoutine(routine);
        yield return fsm.RunPlayPerformGroupCoroutine();
        Debug.Log("SayByeState finish");
        // 若日後要 reset，可在這裡加 delay + return to GreetingState
        yield return new WaitForSeconds(3f);
        fsm.switchToGreetingState();
    }
}

