using RPG.Animation;
using RPG.Input;
using RPG.Movement;
using RPG.StateMachine;
using UnityEngine;

namespace RPG.Character
{
    public abstract class CharacterMono : MonoBehaviour
    {
        public StateMachineMono StateMachineMono { get; protected set; }
        public MovementMono MovementMono { get; protected set; }
        public AnimationMono AnimationMono { get; protected set; }
        public InputMono InputMono { get; protected set; }
    }
}