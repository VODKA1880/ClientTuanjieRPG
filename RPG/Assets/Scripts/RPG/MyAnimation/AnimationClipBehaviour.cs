using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{
    public class AnimationClipBehaviour : AnimationBehaviour
    {
        private AnimationClipPlayable animationClipPlayable;
        private ScriptPlayable<AnimationClipBehaviour> playable;

        public void Init(PlayableGraph graph, ScriptPlayable<AnimationClipBehaviour> playable, AnimationClip clip)
        {
            Init(graph);
            this.playable = playable;
            animationClipPlayable = AnimationClipPlayable.Create(graph, clip);
            playable.AddInput(animationClipPlayable, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            animationClipPlayable.SetTime(0);
            playable.Play();
            animationClipPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            playable.Pause();
            animationClipPlayable.Pause();
        }

        public float GetAnimationClipLength()
        {
            return animationClipPlayable.GetAnimationClip().length;
        }
    }
}