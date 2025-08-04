using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{
    public class AnimationMixerBehaviour : AnimationBehaviour
    {
        private AnimationMixerPlayable animationMixerPlayable;
        private ScriptPlayable<AnimationMixerBehaviour> playable;
        public int CurIdx => curIdx;
        private int curIdx;
        public int TargetIdx => targetIdx;
        private int targetIdx;
        private List<int> transitionIdxs = new List<int>();
        public float TransitionTime => transitionTime;
        private float transitionTime;
        public bool IsTransition => isTransition;
        private bool isTransition;

        public virtual void Init(PlayableGraph graph, ScriptPlayable<AnimationMixerBehaviour> playable)
        {
            Init(graph);
            this.playable = playable;
            animationMixerPlayable = AnimationMixerPlayable.Create(graph);
            playable.AddInput(animationMixerPlayable, 0, 1.0f);
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            animationMixerPlayable.SetTime(0);
            playable.SetSpeed(1); ;
            animationMixerPlayable.SetSpeed(1); ;
        }

        public override void Disable()
        {
            base.Disable();
            playable.SetSpeed(0);
            animationMixerPlayable.SetSpeed(0);
            for (int i = 0; i < animationMixerPlayable.GetInputCount(); i++)
            {
                AnimationTool.Disable(animationMixerPlayable.GetInput(i));
            }

            curIdx = -1;
            targetIdx = -1;
            transitionIdxs.Clear();
            transitionTime = 0f;
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            if (isTransition)
            {
                float targetWeight = 1f;
                for (int i = 0; i < transitionIdxs.Count; i++)
                {
                    int idx = transitionIdxs[i];
                    float weight = animationMixerPlayable.GetInputWeight(idx);
                    float newWeight;
                    if (idx == curIdx)
                    {
                        newWeight = Mathf.Max(weight - 1 / transitionTime * info.deltaTime, 0);
                    }
                    else
                    {
                        newWeight = Mathf.Max(weight - 2 / transitionTime * info.deltaTime, 0);
                    }

                    this.SetInputWeight(idx, newWeight);
                    if (newWeight == 0.0f)
                    {
                        AnimationTool.Disable(animationMixerPlayable.GetInput(idx));
                        transitionIdxs.RemoveAt(i--);
                    }
                    targetWeight -= newWeight;
                }

                this.SetInputWeight(targetIdx, targetWeight);
                if (targetWeight == 1.0f)
                {
                    isTransition = false;
                    curIdx = targetIdx;
                    targetIdx = -1;
                }
            }
        }

        public int AddInput(Playable input)
        {
            return animationMixerPlayable.AddInput(input, 0);
        }

        public Playable GetInput(int idx)
        {
            return animationMixerPlayable.GetInput(idx);
        }

        public void TransitTo(int idx, float transitionTime)
        {
            if (idx == curIdx) return;
            this.transitionTime = transitionTime;
            targetIdx = idx;
            if (transitionTime == 0 || curIdx == -1)
            {
                curIdx = idx;
                animationMixerPlayable.SetInputWeight(idx, 1.0f);
                for (int i = 0; i < animationMixerPlayable.GetInputCount(); i++)
                {
                    this.SetInputWeight(i, i == idx ? 1.0f : 0.0f);
                }
            }
            else
            {
                isTransition = true;
                transitionIdxs.Clear();

                float curMaxWeight = 1.0f;
                int curMaxWeightIdx = -1;
                for (int i = 0; i < animationMixerPlayable.GetInputCount(); i++)
                {
                    var weight = animationMixerPlayable.GetInputWeight(i);
                    if (weight > 0.0f && i != targetIdx)
                    {
                        transitionIdxs.Add(i);
                    }
                    if (weight > curMaxWeight)
                    {
                        curMaxWeight = weight;
                        curMaxWeightIdx = i;
                    }
                }
                if (curMaxWeightIdx != -1)
                {
                    curIdx = curMaxWeightIdx;
                }
            }
        }

        private void SetInputWeight(int idx, float weight)
        {
            if (idx < 0 || idx >= animationMixerPlayable.GetInputCount())
            {
                return;
            }
            animationMixerPlayable.SetInputWeight(idx, weight);
        }
    }
}