using System.Collections.Generic;

namespace Common.StateMachine
{
    public abstract class StateMachineBase
    {
        private int curStateId = -1;
        public int CurStateId => curStateId;
        private int lastStateId = -1;
        private Dictionary<int, StateBase> states = new Dictionary<int, StateBase>();
        public void AddState(StateBase state)
        {
            if (state == null) return;
            if (states.ContainsKey(state.Id))
            {
                states[state.Id] = state;
            }
            else
            {
                states.Add(state.Id, state);
            }
        }

        public void ChangeState(int stateId, params object[] args)
        {
            if (stateId < 0 || !states.ContainsKey(stateId)) return;
            if (curStateId == stateId) return;

            if (curStateId >= 0 && states.ContainsKey(curStateId))
            {
                states[curStateId].Exit();
            }
            lastStateId = curStateId;
            curStateId = stateId;
            states[curStateId].Enter(lastStateId, args);
        }

        public void Update()
        {
            if (states.Count == 0) return;
            if (curStateId < 0 || !states.ContainsKey(curStateId)) return;
            states[curStateId].Update();
        }

        public void Destroy()
        {
            if (this.curStateId >= 0 && states.ContainsKey(this.curStateId))
            {
                states[this.curStateId].Exit();
            }

            states.Clear();
        }
    }
}