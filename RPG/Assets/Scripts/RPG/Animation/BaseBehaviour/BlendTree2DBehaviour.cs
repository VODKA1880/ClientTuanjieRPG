using System.Collections.Generic;
using System.Linq;
using Common.CalcuateTool;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{


    public class BlendTree2DBehaviour : AnimationBehaviour
    {

        private ScriptPlayable<BlendTree2DBehaviour> playable;
        private AnimationMixerPlayable mixer;
        private Blend2DData data;
        private float curBlendX;
        private float curBlendY;

        public void Init(PlayableGraph graph, ScriptPlayable<BlendTree2DBehaviour> playable, ref Blend2DData data)
        {
            Init(graph);
            this.playable = playable;
            this.data = data;

            if (this.data.playables == null || this.data.playables.Length == 0)
            {
                this.data.playables = new Blend2DPlayableData[this.data.clips.Length];
                for (int i = 0; i < this.data.clips.Length; i++)
                {
                    var clipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
                    AnimationTool.Init(clipPlayable, graph, this.data.clips[i].clip);
                    this.data.playables[i] = new Blend2DPlayableData(clipPlayable, this.data.clips[i].x, this.data.clips[i].y);
                }
            }

            mixer = AnimationMixerPlayable.Create(graph, this.data.playables.Length);
            for (int i = 0; i < this.data.playables.Length; i++)
            {
                graph.Connect(this.data.playables[i].playable, 0, mixer, i);
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

        public float GetBlendX()
        {
            return curBlendX;
        }

        public float GetBlendY()
        {
            return curBlendY;
        }

        private void UpdateWeights()
        {
            var disCache = new float[data.playables.Length];
            var total = 0f;
            for (int i = 0; i < data.playables.Length; i++)
            {
                var clipX = data.playables[i].x;
                var clipY = data.playables[i].y;
                var dis = CalculateDis(clipX, clipY);
                if (dis < data.maxDirection)
                {
                    disCache[i] = dis;
                    total += dis;
                }
                else
                {
                    disCache[i] = data.maxDirection;
                }
            }

            for (int i = 0; i < data.playables.Length; i++)
            {
                var dis = disCache[i];
                if (dis < data.maxDirection)
                {
                    if (dis == total)
                    {
                        mixer.SetInputWeight(i, 1);

                    }
                    else
                    {
                        var weight = 1 - dis / total;
                        mixer.SetInputWeight(i, weight);
                    }
                }
                else
                {
                    mixer.SetInputWeight(i, 0f);
                }
            }
        }

        private float CalculateDis(float clipX, float clipY)
        {
            return Vector2.Distance(new Vector2(clipX, clipY), new Vector2(curBlendX, curBlendY));
        }

        public int AddInput(Blend2DPlayableData data)
        {

            var newDatas = new Blend2DPlayableData[this.data.playables.Length + 1];
            for (int i = 0; i < this.data.playables.Length; i++)
            {
                newDatas[i] = this.data.playables[i];
            }
            newDatas[this.data.playables.Length] = data;
            this.data.playables = newDatas;

            var idx = mixer.AddInput(data.playable, 0);
            return idx;
        }
    }
}