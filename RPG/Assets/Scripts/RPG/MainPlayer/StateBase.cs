namespace RPG.MainPlayer
{
    public abstract class StateBase : Common.StateMachine.StateBase
    {
        protected StateMachine StateMachine;
        protected MainPlayerMono MainPlayerMono => StateMachine.MainPlayerMono;
        protected StateBase(int id, string name, StateMachine stateMachine) : base(id, name)
        {
            StateMachine = stateMachine;
        }
    }
}