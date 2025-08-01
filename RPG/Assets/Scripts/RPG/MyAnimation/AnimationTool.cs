using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.MyAnimation
{
    public static class AnimationTool
    {
        public static PlayableGraph GetAnimGraph(DirectorUpdateMode value = DirectorUpdateMode.GameTime)
        {
            var graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(value);
            return graph;
        }

        public static void SetSourcePlayable(this PlayableGraph graph, Playable playable, Animator animator)
        {
            var output = AnimationPlayableOutput.Create(graph, "Output", animator);
            output.SetSourcePlayable(playable);
        }

        public static AnimationClipBehaviour Init(this ScriptPlayable<AnimationClipBehaviour> playable, PlayableGraph graph, AnimationClip clip)
        {
            var behaviour = playable.GetBehaviour();
            behaviour.Init(graph, playable, clip);
            return behaviour;
        }

        public static AnimationRandomBehaviour Init(this ScriptPlayable<AnimationRandomBehaviour> playable, PlayableGraph graph, AnimationClip[] clips)
        {
            var behaviour = playable.GetBehaviour();
            behaviour.Init(graph, playable, clips);
            return behaviour;
        }

        public static AnimationMixerBehaviour Init(this ScriptPlayable<AnimationMixerBehaviour> playable, PlayableGraph graph)
        {
            var behaviour = playable.GetBehaviour();
            behaviour.Init(graph, playable);
            return behaviour;
        }

        public static BlendTree2DBehaviour Init(this ScriptPlayable<BlendTree2DBehaviour> playable, PlayableGraph graph, Blend2DClipData[] clips)
        {
            var behaviour = playable.GetBehaviour();
            behaviour.Init(graph, playable, clips);
            return behaviour;
        }

        public static float GetAnimationClipLength(this ScriptPlayable<AnimationClipBehaviour> clipPlayable)
        {
            return clipPlayable.GetBehaviour().GetAnimationClipLength();
        }

        public static void Enable(Playable playable)
        {
            if (typeof(AnimationClipBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var clipPlayable = (ScriptPlayable<AnimationClipBehaviour>)playable;
                clipPlayable.GetBehaviour().Enable();
            }
            else if (typeof(AnimationRandomBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var randomPlayable = (ScriptPlayable<AnimationRandomBehaviour>)playable;
                randomPlayable.GetBehaviour().Enable();
            }
            else if (typeof(AnimationMixerBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var mixerPlayable = (ScriptPlayable<AnimationMixerBehaviour>)playable;
                mixerPlayable.GetBehaviour().Enable();
            }
            else
            {
                Debug.LogWarning("Unsupported playable type for enabling.");
            }
        }

        public static void Disable(Playable playable)
        {
            if (typeof(AnimationClipBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var clipPlayable = (ScriptPlayable<AnimationClipBehaviour>)playable;
                clipPlayable.GetBehaviour().Disable();
            }
            else if (typeof(AnimationRandomBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var randomPlayable = (ScriptPlayable<AnimationRandomBehaviour>)playable;
                randomPlayable.GetBehaviour().Disable();
            }
            else if (typeof(AnimationMixerBehaviour).IsAssignableFrom(playable.GetPlayableType()))
            {
                var mixerPlayable = (ScriptPlayable<AnimationMixerBehaviour>)playable;
                mixerPlayable.GetBehaviour().Disable();
            }
            else
            {
                Debug.LogWarning("Unsupported playable type for disabling.");
            }
        }
    }
}