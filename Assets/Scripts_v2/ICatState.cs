public interface ICatState
{
    void Enter(CatStateMachine fsm);             // �i�J���A�ɩI�s
    void HandleEvent(CatEvent catEvent);         // ����ƥ�ɩI�s
    void Exit();                                 // ���}���A�ɡ]�i��^
}
