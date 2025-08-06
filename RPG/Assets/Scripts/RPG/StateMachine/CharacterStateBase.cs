using Common.StateMachine;
using RPG.Character;

namespace RPG.StateMachine
{
    public class CharacterStateBase : StateBase
    {
        private CharacterMono character;
        public CharacterStateBase(int id, string name, CharacterMono character) : base(id, name)
        {
            this.character = character;
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