using Common.StateMachine;
using RPG.Character;
using RPG.StateMachine;

namespace RPG.MainPlayer
{
    public class WalkState : CharacterStateBase
    {
        public WalkState(CharacterMono character) : base((int)StateId.Move, "Walk", character)
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