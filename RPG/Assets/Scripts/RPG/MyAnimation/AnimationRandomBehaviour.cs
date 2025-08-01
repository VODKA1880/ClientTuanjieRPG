using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{
    public class AnimationRandomBehaviour : AnimationBehaviour
    {
        private ScriptPlayable<AnimationRandomBehaviour> playable;
        private AnimationMixerPlayable mixer;
        private int currentClipIndex = -1;
        private float curAnimationTime;

        public void Init(PlayableGraph graph, ScriptPlayable<AnimationRandomBehaviour> playable, AnimationClip[] clips)
        {
            Init(graph);
            this.playable = playable;

            mixer = AnimationMixerPlayable.Create(graph, clips.Length);
            for (int i = 0; i < clips.Length; i++)
            {
                var clipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
                clipPlayable.GetBehaviour().Init(graph, clipPlayable, clips[i]);
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
                UpdateRandomAnimation();
            }
        }

        private void UpdateRandomAnimation()
        {
            int inputCount = mixer.GetInputCount();
            if (inputCount > 0)
            {
                currentClipIndex = Random.Range(0, inputCount);
                for (int i = 0; i < inputCount; i++)
                {
                    mixer.SetInputWeight(i, i == currentClipIndex ? 1.0f : 0.0f);
                    if (i == currentClipIndex)
                    {
                        AnimationTool.Enable(mixer.GetInput(i));
                        var clipPlayable = (ScriptPlayable<AnimationClipBehaviour>)mixer.GetInput(i);
                        curAnimationTime = clipPlayable.GetBehaviour().GetAnimationClipLength();
                    }
                    else
                    {
                        AnimationTool.Disable(mixer.GetInput(i));
                    }
                }
            }
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0f);
            mixer.SetTime(0f);
            playable.Play();
            mixer.Play();
            UpdateRandomAnimation();
        }

        public override void Disable()
        {
            base.Disable();
            playable.Pause();
            mixer.Pause();
            for (int i = 0; i < mixer.GetInputCount(); i++)
            {
                AnimationTool.Disable(mixer.GetInput(i));
                mixer.SetInputWeight(i, 0.0f);
            }
            currentClipIndex = -1;
        }
    }
}