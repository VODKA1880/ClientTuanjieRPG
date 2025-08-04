using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{
    public abstract class AnimationBehaviour : PlayableBehaviour
    {
        public bool IsEnable { get; private set; }
        protected PlayableGraph graph;
        protected float curClipTime = 0;
        protected float curEnterTime = 0;

        public void Init(PlayableGraph graph)
        {
            this.graph = graph;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            if (IsEnable)
            {
                Execute(playable, info);
            }
        }

        public virtual void Execute(Playable playable, FrameData info)
        {

        }

        public virtual void Enable()
        {
            IsEnable = true;
        }

        public virtual void Disable()
        {
            IsEnable = false;
        }

        public virtual float GetAnimationClipLength()
        {
            return this.curClipTime;
        }

        public virtual float GetEnterTime()
        {
            return this.curEnterTime;
        }
    }
}