using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation
{
    public class AnimationUnit : AnimationBehaviour
    {
        private AnimationClipPlayable animationClipPlayable;
        public AnimationUnit(PlayableGraph graph, AnimationClip clip) : base(graph)
        {
            animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
            adapterPlayable.AddInput(animationClipPlayable, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            adapterPlayable.SetTime(0);
            animationClipPlayable.SetTime(0);
            adapterPlayable.Play();
            animationClipPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            adapterPlayable.Pause();
            animationClipPlayable.Pause();
        }

        public Playable GetAdapterPlayable()
        {
            return adapterPlayable;
        }
    }
}