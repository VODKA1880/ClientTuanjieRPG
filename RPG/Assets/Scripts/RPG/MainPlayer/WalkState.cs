using RPG.Animation;
using UnityEngine;

namespace RPG.MainPlayer
{
    public class WalkState : StateBase
    {
        public WalkState(StateMachine stateMachine) : base((int)StateId.Walk, "Walk", stateMachine)
        {

        }


        public override void Enter(int lastStateId, params object[] args)
        {
            MainPlayerMono.AnimationMono.TransitTo(GraphInputType.Walk);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            if (inputX == 0 && inputY == 0)
            {
                StateMachine.ChangeState((int)StateId.Idle);
            }
            else
            {
                MainPlayerMono.AnimationMono.inputX = inputX;
                MainPlayerMono.AnimationMono.inputY = inputY;
            }
        }
    }
}