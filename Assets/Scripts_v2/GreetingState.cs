using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreetingState : ICatState
{
    private CatStateMachine fsm;
    //private Coroutine greetingRoutine;
    private static readonly float greetingInterval = 10f; // �C 10 ���@��

    public void Enter(CatStateMachine fsm)
    {
        Debug.Log("Enter GreetingState");
        this.fsm = fsm;
        //greetingRoutine = fsm.RunCoroutine(GreetingLoop());
        // TODO: ����۩I�y���]�i�]�p���`���^
        fsm.SetStateMainRoutine(GreetingLoop());
        fsm.RunStateMainCoroutine();
    }

    public void HandleEvent(CatEvent catEvent)
    {
        if (catEvent == CatEvent.UserSatDown)
        {
            fsm.TransitionTo(new AskToPickBookState());
        }
    }

    public void Exit()
    {
        fsm.StopStateMainCoroutine();
        fsm.StopPlayPerformGroupCoroutine();
        fsm.Presentation.Clear();
    }
    private IEnumerator GreetingLoop()
    {
        var group = fsm.Presentation.GreetingGroup;
        while (true)
        {
            var routine = fsm.Presentation.PlayPerformGroup(group);
            fsm.SetPlayPerformGroupRoutine(routine);
            yield return fsm.RunPlayPerformGroupCoroutine();
            yield return new WaitForSeconds(greetingInterval);
        }
    }
}
