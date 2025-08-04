using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.BaseBehaviour
{
    public class AnimationRandomBehaviour : AnimationBehaviour
    {
        private ScriptPlayable<AnimationRandomBehaviour> playable;
        private AnimationMixerPlayable mixer;
        private int currentClipIndex = -1;
        private float curAnimationTime;

        public void Init(PlayableGraph graph, ScriptPlayable<AnimationRandomBehaviour> playable, AnimationClipData[] animationClipDatas)
        {
            Init(graph);
            this.playable = playable;

            mixer = AnimationMixerPlayable.Create(graph, animationClipDatas.Length);
            for (int i = 0; i < animationClipDatas.Length; i++)
            {
                var clipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
                AnimationTool.Init(clipPlayable, graph, animationClipDatas[i]);
                graph.Connect(clipPlayable, 0, mixer, i);
            }

            playable.AddInput(mixer, 0, 1.0f);
            Disable();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            curAnimationTime -= info.deltaTime;
            if (curAnimationTime <= 0)
            {
                UpdateRandomAnimationIdx();
                PlayCurAnimationIdx();
            }
        }

        private void UpdateRandomAnimationIdx()
        {
            int inputCount = mixer.GetInputCount();
            if (inputCount > 0)
            {
                currentClipIndex = Random.Range(0, inputCount);
                var clipPlayable = (ScriptPlayable<AnimationClipBehaviour>)mixer.GetInput(currentClipIndex);
                var behaviour = clipPlayable.GetBehaviour();
                curClipTime = behaviour.GetAnimationClipLength();
                curEnterTime = behaviour.GetEnterTime();

                curAnimationTime = curClipTime;
            }
        }

        private void PlayCurAnimationIdx()
        {
            int inputCount = mixer.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                mixer.SetInputWeight(i, i == currentClipIndex ? 1.0f : 0.0f);
                if (i == currentClipIndex)
                {
                    AnimationTool.Enable(mixer.GetInput(i));
                }
                else
                {
                    AnimationTool.Disable(mixer.GetInput(i));
                }
            }
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0f);
            mixer.SetTime(0f);
            playable.SetSpeed(1);
            mixer.SetSpeed(1);
            UpdateRandomAnimationIdx();
            PlayCurAnimationIdx();
        }

        public override void Disable()
        {
            base.Disable();
            playable.SetSpeed(0);
            mixer.SetSpeed(0);
            for (int i = 0; i < mixer.GetInputCount(); i++)
            {
                AnimationTool.Disable(mixer.GetInput(i));
                mixer.SetInputWeight(i, 0.0f);
            }
            currentClipIndex = -1;
        }
    }
}