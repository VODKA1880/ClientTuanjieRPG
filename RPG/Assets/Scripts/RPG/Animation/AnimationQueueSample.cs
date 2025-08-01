using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation
{
    public class AnimationQueueBehaviour : PlayableBehaviour
    {
        private AnimationMixerPlayable mixer;
        private int currentIndex = 0;
        private float currentTime = 0f;
        public void Init(PlayableGraph graph, Playable playable, AnimationClip[] clips)
        {
            playable.SetInputCount(1);
            mixer = AnimationMixerPlayable.Create(graph, clips.Length);
            for (int i = 0; i < clips.Length; i++)
            {
                var clipPlayable = AnimationClipPlayable.Create(graph, clips[i]);
                graph.Connect(clipPlayable, 0, mixer, i);
            }
            graph.Connect(mixer, 0, playable, 0);
            playable.SetInputWeight(0, 1);

            currentIndex = 0;
            mixer.SetInputWeight(currentIndex, 1);
            currentTime = ((AnimationClipPlayable)mixer.GetInput(0)).GetAnimationClip().length;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);

            currentTime -= info.deltaTime;
            if (currentTime < 0 && currentIndex < mixer.GetInputCount() - 1)
            {
                mixer.SetInputWeight(currentIndex++, 0);
                mixer.SetInputWeight(currentIndex, 1);
                currentTime = ((AnimationClipPlayable)mixer.GetInput(currentIndex)).GetAnimationClip().length;
            }
        }
    }

    public class AnimationQueueSample : MonoBehaviour
    {
        public AnimationClip[] clips;
        private PlayableGraph graph;

        void Start()
        {
            graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            var playable = ScriptPlayable<AnimationQueueBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.Init(graph, playable, clips);

            var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", GetComponent<Animator>());
            playableOutput.SetSourcePlayable(playable);

            graph.Play();
        }

        void OnDisable()
        {
            graph.Destroy();
        }
    }
}

