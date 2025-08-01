using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{
    [System.Serializable]
    public struct Blend2DClipData
    {
        public AnimationClip clip;
        public float x;
        public float y;
    }

    public class BlendTree2DBehaviour : AnimationBehaviour
    {
        public struct BlendTree2DAnimationClip
        {
            public AnimationClip clip;
            public float x;
            public float y;
        }

        private ScriptPlayable<BlendTree2DBehaviour> playable;
        private AnimationMixerPlayable mixer;
        private BlendTree2DAnimationClip[] clips;
        private float curBlendX;
        private float curBlendY;

        public void Init(PlayableGraph graph, ScriptPlayable<BlendTree2DBehaviour> playable, Blend2DClipData[] clips)
        {
            Init(graph);
            this.playable = playable;
            this.clips = new BlendTree2DAnimationClip[clips.Length];
            for (int i = 0; i < clips.Length; i++)
            {
                this.clips[i] = new BlendTree2DAnimationClip
                {
                    clip = clips[i].clip,
                    x = clips[i].x,
                    y = clips[i].y
                };
            }

            mixer = AnimationMixerPlayable.Create(graph, clips.Length);
            for (int i = 0; i < clips.Length; i++)
            {
                var clipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
                clipPlayable.GetBehaviour().Init(graph, clipPlayable, clips[i].clip);
                graph.Connect(clipPlayable, 0, mixer, i);
            }
            playable.AddInput(mixer, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            mixer.SetTime(0);
            playable.Play();
            mixer.Play();
            for (int i = 0; i < mixer.GetInputCount(); i++)
            {
                AnimationTool.Enable(mixer.GetInput(i));
            }
            curBlendX = 0f;
            curBlendY = 0f;
            UpdateWeights();
        }

        public override void Disable()
        {
            base.Disable();
            playable.Pause();
            mixer.Pause();
            for (int i = 0; i < mixer.GetInputCount(); i++)
            {
                AnimationTool.Disable(mixer.GetInput(i));
            }
            curBlendX = 0f;
            curBlendY = 0f;
        }

        public void SetBlend(float x, float y)
        {
            if (x == curBlendX && y == curBlendY) return;

            curBlendX = x;
            curBlendY = y;

            UpdateWeights();
        }

        private void UpdateWeights()
        {
            float allWeight = 0f;
            for (int i = 0; i < clips.Length; i++)
            {
                var weight = CalculateWeight(clips[i].x, clips[i].y);
                mixer.SetInputWeight(i, weight);
                allWeight += weight;
            }
            Debug.Log(allWeight);
        }

        private float CalculateWeight(float clipX, float clipY)
        {
            float dx = clipX - curBlendX;
            float dy = clipY - curBlendY;
            float distance = Mathf.Sqrt(dx * dx + dy * dy);
            if (distance < 0.01f)
            {
                return 1f;
            }
            return Mathf.Max(0f, 1f - distance);
        }
    }
}