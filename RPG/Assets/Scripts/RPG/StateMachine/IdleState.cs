using Common.StateMachine;
using RPG.Character;
using UnityEngine;

namespace RPG.StateMachine
{
    public class IdleState : CharacterStateBase
    {
        public IdleState(CharacterMono character) : base((int)StateId.Idle, "Idle", character)
        {

        }

        public override void Enter(int lastStateId, params object[] args)
        {

        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }
    }
}