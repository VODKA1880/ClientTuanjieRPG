using UnityEngine.Playables;

namespace RPG.Animation
{
    public class AnimationAdapter : PlayableBehaviour
    {
        private AnimationBehaviour animationBehaviour;

        public void Init(AnimationBehaviour behaviour)
        {
            animationBehaviour = behaviour;
        }

        public void Enable()
        {
            animationBehaviour?.Enable();
        }

        public void Disable()
        {
            animationBehaviour?.Disable();
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            animationBehaviour?.Execute(playable, info);
        }
    }
}