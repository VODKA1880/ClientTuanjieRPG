using UnityEngine.Playables;

namespace RPG.Animation
{
    public abstract class AnimationBehaviour
    {
        public bool IsEnable { get; private set; }
        protected Playable adapterPlayable;

        public AnimationBehaviour() { }

        public AnimationBehaviour(PlayableGraph graph)
        {
            adapterPlayable = ScriptPlayable<AnimationAdapter>.Create(graph);
            ((ScriptPlayable<AnimationAdapter>)adapterPlayable).GetBehaviour().Init(this);
        }

        public virtual void Enable()
        {
            IsEnable = true;
        }

        public virtual void Disable()
        {
            IsEnable = false;
        }

        public virtual void Execute(Playable playable, FrameData info)
        {
            if (!IsEnable) return;
        }
    }
}