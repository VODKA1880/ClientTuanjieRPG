using RPG.Animation;
using UnityEngine;

namespace RPG.MainPlayer
{
    public class MainPlayerMono : MonoBehaviour
    {
        public StateMachine StateMachine { get; private set; }
        public AnimationMono AnimationMono { get; private set; }
        private void Awake()
        {
            StateMachine = new StateMachine(this);
            StateMachine.AddState(new IdleState(StateMachine));
            StateMachine.AddState(new WalkState(StateMachine));
            AnimationMono = GetComponent<AnimationMono>();
        }

        private void Start()
        {
            StateMachine.ChangeState((int)StateId.Idle);
        }

        private void Update()
        {
            StateMachine.Update();
        }
    }
}