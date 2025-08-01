using System;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{
    public abstract class AnimationBehaviour : PlayableBehaviour
    {
        public bool IsEnable { get; private set; }
        protected PlayableGraph graph;

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
    }
}