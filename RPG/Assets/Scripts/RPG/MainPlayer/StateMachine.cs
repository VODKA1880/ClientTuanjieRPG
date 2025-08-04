using Common.StateMachine;

namespace RPG.MainPlayer
{
    public enum StateId
    {
        Idle = 0,
        Move = 1,
    }
    public class StateMachine : StateMachineBase
    {
        public MainPlayerMono MainPlayerMono { get; private set; }
    }
}