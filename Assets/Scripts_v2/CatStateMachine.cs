using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStateMachine : MonoBehaviour
{
    [Header("外部控制器")]
    [SerializeField] private CatPresentationController presentation;
    private ICatState currentState;
    private Coroutine playPerformGroupCoroutine;
    private CatState currentCatState;

    private IEnumerator stateMainRoutine;
    private Coroutine stateMainCoroutine;
    private IEnumerator playPerformGroupRoutine;

    public ICatState CurrentState => currentState;
    public CatPresentationController Presentation => presentation;
    public CatState CurrentCatState => currentCatState;
    public void Start()
    {
        //currentCatState = CatState.SayByeState;
        //TransitionTo(new GreetingState());
        //TransitionTo(new AskToPickBookState());
        //TransitionTo(new ReadStoryState(1));
        //TransitionTo(new SayByeState());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    TransitionTo(new GreetingState());
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    TransitionTo(new AskToPickBookState());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    TransitionTo(new ReadStoryState(0));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    TransitionTo(new ReadStoryState(1));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    TransitionTo(new ReadStoryState(2));
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    TransitionTo(new SayByeState());
        //}
    }
    public void switchToGreetingState()
    {
        Debug.Log(CurrentCatState + "," + currentCatState);
        currentCatState = CatState.GreetingState;
        TransitionTo(new GreetingState());
    }
    public void switchToAskToPickBookState()
    {
        currentCatState = CatState.AskToPickBookState;
        TransitionTo(new AskToPickBookState());
    }
    public void switchToReadStoryState(int bookIndex)
    {
        currentCatState = CatState.ReadStoryState;
        TransitionTo(new ReadStoryState(bookIndex));
    }
    public void switchToSayByeState()
    {
        currentCatState = CatState.SayByeState;
        TransitionTo(new SayByeState());
    }
    public void HandleEvent(CatEvent catEvent)
    {
        currentState?.HandleEvent(catEvent);
    }

    public void TransitionTo(ICatState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }
    public void SetStateMainRoutine(IEnumerator coroutine)
    {
        stateMainRoutine = coroutine;
    }
    public void SetPlayPerformGroupRoutine(IEnumerator coroutine)
    {
        playPerformGroupRoutine = coroutine;
    }
    public void RunStateMainCoroutine()
    {
        stateMainCoroutine = StartCoroutine(stateMainRoutine);
    }
    public Coroutine RunPlayPerformGroupCoroutine()
    {
        playPerformGroupCoroutine = StartCoroutine(playPerformGroupRoutine);
        return playPerformGroupCoroutine;
    }
    public void StopStateMainCoroutine()
    {
        if (stateMainCoroutine != null)
            StopCoroutine(stateMainCoroutine);
    }
    public void StopPlayPerformGroupCoroutine()
    {
        if (playPerformGroupCoroutine != null)
            StopCoroutine(playPerformGroupCoroutine);
    }
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
