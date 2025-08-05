using RPG.Animation.BaseBehaviour;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Animation.Behaviour
{
    public class WalkBehaviour : AnimationBehaviour
    {
        private ScriptPlayable<WalkBehaviour> playable;
        private BlendTree2DBehaviour blendTree2DBehaviour;
        public void Init(PlayableGraph graph, ScriptPlayable<WalkBehaviour> playable, ref Blend2DData data)
        {
            Init(graph);
            this.playable = playable;

            var blend2D = ScriptPlayable<BlendTree2DBehaviour>.Create(graph);
            blendTree2DBehaviour = blend2D.Init(graph, ref data);
            playable.AddInput(blend2D, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            playable.SetSpeed(1);
            blendTree2DBehaviour.Enable();
        }

        public override void Disable()
        {
            base.Disable();
            playable.SetSpeed(0);
            blendTree2DBehaviour.Disable();
        }

        public void SetBlend(float x, float y)
        {
            blendTree2DBehaviour.SetBlend(x, y);
        }

        public float GetBlendX()
        {
            return blendTree2DBehaviour.GetBlendX();
        }

        public float GetBlendY()
        {
            return blendTree2DBehaviour.GetBlendY();
        }
    }
}