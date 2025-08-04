using UnityEngine;

namespace RPG.Animation
{
    public partial class AnimationMono : MonoBehaviour
    {
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            StartGraph();
        }

        void Update()
        {
            this.UpdateGraph();
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
            OnDisableEvent();
            OnDisableGraph();
        }
    }
}