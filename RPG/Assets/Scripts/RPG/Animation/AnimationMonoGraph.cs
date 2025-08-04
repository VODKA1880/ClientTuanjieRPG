using RPG.Animation.BaseBehaviour;
using RPG.Animation.Behaviour;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Animation
{
    public partial class AnimationMono : MonoBehaviour
    {
        private PlayableGraph graph;
        private AnimationMixerBehaviour rootMixer;
        private void StartGraph()
        {
            graph = AnimationTool.GetAnimGraph();

            var animPlayable = ScriptPlayable<AnimationMixerBehaviour>.Create(graph);
            rootMixer = animPlayable.GetBehaviour();
            rootMixer.Init(graph, animPlayable);
            rootMixer.Enable();

            graph.SetSourcePlayable(animPlayable, GetComponent<Animator>());
            graph.Play();

            AddIdle();
        }

        public IdleBehaviourData IdleBehaviourData;
        private int idleIdx = 0;

        public void AddIdle()
        {
            var idlePlayable = ScriptPlayable<IdleBehaviour>.Create(graph);
            var idleBehaviour = idlePlayable.GetBehaviour();
            idleBehaviour.Init(graph, idlePlayable, IdleBehaviourData);
            idleIdx = rootMixer.AddInput(idlePlayable);
            idleBehaviour.Enable();

            rootMixer.TransitTo(idleIdx, 0);
        }

        private void UpdateGraph()
        {

        }

        private void OnDisableGraph()
        {
            graph.Destroy();
        }
    }
}