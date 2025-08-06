namespace Common.StateMachine
{
    public abstract class StateBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected StateBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract void Enter(int lastStateId, params object[] args);

        public abstract void Exit();

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
    }
}