using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{


    public class BlendTree2DBehaviour : AnimationBehaviour
    {

        private ScriptPlayable<BlendTree2DBehaviour> playable;
        private AnimationMixerPlayable mixer;
        private Blend2DPlayableData[] datas;
        private float curBlendX;
        private float curBlendY;

        public void Init(PlayableGraph graph, ScriptPlayable<BlendTree2DBehaviour> playable, Blend2DPlayableDataClip dataClip)
        {
            var data = new Blend2DPlayableData[dataClip.clips.Length];
            for (int i = 0; i < dataClip.clips.Length; i++)
            {
                var clipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
                AnimationTool.Init(clipPlayable, graph, dataClip.clips[i]);
                data[i] = new Blend2DPlayableData(clipPlayable, dataClip.x, dataClip.y);
            }
            Init(graph, playable, data);
        }

        public void Init(PlayableGraph graph, ScriptPlayable<BlendTree2DBehaviour> playable, Blend2DPlayableData[] datas)
        {
            Init(graph);
            this.playable = playable;
            this.datas = datas;

            mixer = AnimationMixerPlayable.Create(graph, datas.Length);
            for (int i = 0; i < datas.Length; i++)
            {
                graph.Connect(datas[i].playable, 0, mixer, i);
            }
            playable.AddInput(mixer, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            mixer.SetTime(0);
            playable.SetSpeed(1);
            mixer.SetSpeed(1);
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
            playable.SetSpeed(0);
            mixer.SetSpeed(0);
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
            for (int i = 0; i < datas.Length; i++)
            {
                var weight = CalculateWeight(datas[i].x, datas[i].y);
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

        public int AddInput(Blend2DPlayableData data)
        {
            var newDatas = new Blend2DPlayableData[datas.Length + 1];
            for (int i = 0; i < datas.Length; i++)
            {
                newDatas[i] = datas[i];
            }
            newDatas[datas.Length] = data;
            datas = newDatas;

            var idx = mixer.AddInput(data.playable, 0);
            return idx;
        }
    }
}