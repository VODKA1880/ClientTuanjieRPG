using System.Collections.Generic;
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
            AddWalk();
        }
        private Dictionary<int, GraphInputData> graphInputDatas = new Dictionary<int, GraphInputData>();
        private GraphInputType currentType = GraphInputType.None;
        private void AddGraphInput(GraphInputType type, Playable playable, AnimationBehaviour behaviour)
        {
            int inputIdx = rootMixer.AddInput(playable);
            var graphInputData = new GraphInputData
            {
                InputIdx = inputIdx,
                playable = playable,
                behaviour = behaviour
            };
            graphInputDatas.Add((int)type, graphInputData);
        }

        private T GetBehavaiour<T>(GraphInputType type) where T : AnimationBehaviour
        {
            if (graphInputDatas.TryGetValue((int)type, out var data))
            {
                return data.behaviour as T;
            }
            return null;
        }

        public void TransitTo(GraphInputType type)
        {
            if (graphInputDatas.TryGetValue((int)type, out var data))
            {
                if (currentType == type)
                {
                    return; // 已经在这个状态
                }
                else if (currentType == GraphInputType.None)
                {
                    currentType = type;
                    rootMixer.TransitTo(data.InputIdx, 0);
                }
                else if (currentType != GraphInputType.None)
                {
                    // 切换状态
                    currentType = type;
                    var currentBehaviour = GetBehavaiour<AnimationBehaviour>(currentType);
                    currentBehaviour.Enable();
                    var enterTime = currentBehaviour.GetEnterTime();
                    rootMixer.TransitTo(data.InputIdx, enterTime);
                }
            }
        }

        public IdleBehaviourData IdleBehaviourData;
        public void AddIdle()
        {
            var idlePlayable = ScriptPlayable<IdleBehaviour>.Create(graph);
            var idleBehaviour = idlePlayable.GetBehaviour();
            idleBehaviour.Init(graph, idlePlayable, IdleBehaviourData);
            AddGraphInput(GraphInputType.Idle, idlePlayable, idleBehaviour);
            idleBehaviour.Enable();
        }

        public Blend2DData WalkBehaviourData;
        public void AddWalk()
        {
            var walkPlayable = ScriptPlayable<WalkBehaviour>.Create(graph);
            var walkBehaviour = walkPlayable.GetBehaviour();
            walkBehaviour.Init(graph, walkPlayable, ref WalkBehaviourData);
            AddGraphInput(GraphInputType.Walk, walkPlayable, walkBehaviour);
            walkBehaviour.Enable();
        }

        [Header("测试")]
        public float inputX;
        public float inputY;
        private void UpdateGraph()
        {
            if (currentType == GraphInputType.Walk)
            {
                var walkBehaviour = GetBehavaiour<WalkBehaviour>(GraphInputType.Walk);
                if (walkBehaviour != null)
                {
                    walkBehaviour.SetBlend(inputX, inputY);
                }
            }
        }

        private void OnDisableGraph()
        {
            graph.Destroy();
        }
    }

    public enum GraphInputType
    {
        None,
        Idle,
        Walk,
    }

    class GraphInputData
    {
        public int InputIdx;
        public Playable playable;
        public AnimationBehaviour behaviour;
    }
}