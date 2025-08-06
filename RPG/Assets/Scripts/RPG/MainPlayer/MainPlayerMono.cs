using RPG.Animation;
using RPG.Input;
using RPG.Movement;
using RPG.StateMachine;
using RPG.Character;
using Unity.VisualScripting;

namespace RPG.MainPlayer
{
    public partial class MainPlayerMono : CharacterMono
    {
        private void Awake()
        {
            StateMachineMono.AddComponent<StateMachineMono>();
            MovementMono = gameObject.AddComponent<MovementMono>();
            // AnimationMono = gameObject.AddComponent<AnimationMono>();
            InputMono = gameObject.AddComponent<InputMono>();
            InitComponents();
        }

        private void InitComponents()
        {
            var stateMachine = new StateMachine();
            StateMachineMono.SetStateMachine(stateMachine);
            StateMachineMono.AddState(new IdleState(this));
            StateMachineMono.AddState(new WalkState(this));
        }

        private void Start()
        {
            StateMachineMono.ChangeState((int)StateId.Idle);
        }

        private void Update()
        {

        }
    }
}