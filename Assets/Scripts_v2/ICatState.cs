public interface ICatState
{
    void Enter(CatStateMachine fsm);             // 進入狀態時呼叫
    void HandleEvent(CatEvent catEvent);         // 收到事件時呼叫
    void Exit();                                 // 離開狀態時（可選）
}
