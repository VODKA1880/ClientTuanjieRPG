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
            StateMachine = new StateMachine();
            StateMachine.AddState(new IdleState(StateMachine));
            AnimationMono = GetComponent<AnimationMono>();
        }
    }
}