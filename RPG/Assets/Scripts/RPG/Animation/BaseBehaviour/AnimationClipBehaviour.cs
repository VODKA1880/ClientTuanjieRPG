using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{
    public class AnimationClipBehaviour : AnimationBehaviour
    {
        private AnimationClipPlayable animationClipPlayable;
        private ScriptPlayable<AnimationClipBehaviour> playable;

        public void Init(PlayableGraph graph, ScriptPlayable<AnimationClipBehaviour> playable, AnimationClipData animationClipData)
        {
            Init(graph);
            this.playable = playable;
            this.curClipTime = animationClipData.clip.length;
            this.curEnterTime = animationClipData.enterTime;
            animationClipPlayable = AnimationClipPlayable.Create(graph, animationClipData.clip);
            playable.AddInput(animationClipPlayable, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            animationClipPlayable.SetTime(0);
            playable.SetSpeed(1);
            animationClipPlayable.SetSpeed(1);
        }

        public override void Disable()
        {
            base.Disable();
            playable.SetSpeed(0);
            animationClipPlayable.SetSpeed(0);
        }
    }
}