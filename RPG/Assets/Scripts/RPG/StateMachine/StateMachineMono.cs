using Common.StateMachine;
using UnityEngine;

namespace RPG.StateMachine
{
    public partial class StateMachineMono : MonoBehaviour
    {
        private StateMachineBase stateMachine;
        public int StateId => stateMachine?.CurStateId ?? -1;
        public string StateName => stateMachine?.CurStateName ?? "None";

        void Awake()
        {

        }

        public void SetStateMachine(StateMachineBase stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void AddState(StateBase state)
        {
            if (stateMachine == null) return;
            stateMachine.AddState(state);
        }

        public void ChangeState(int stateId, params object[] args)
        {
            if (stateMachine == null) return;
            stateMachine.ChangeState(stateId, args);
        }

        public void UpdateStateMachine()
        {
            if (stateMachine == null) return;
            stateMachine.Update();
        }
    }
}