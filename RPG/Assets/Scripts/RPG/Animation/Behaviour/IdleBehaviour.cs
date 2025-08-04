using RPG.Animation.BaseBehaviour;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.Animation.Behaviour
{
    public class IdleBehaviour : AnimationBehaviour
    {
        private ScriptPlayable<IdleBehaviour> playable;
        private IdleBehaviourData idleBehaviourData;
        private AnimationMixerPlayable mixer;
        private AnimationClipBehaviour idleClipBehaviour;
        private int idleIdx;
        private AnimationRandomBehaviour randomBehaviour;
        private int randomIdx;
        private int curIdx;
        private float curWeight;
        private int targetIdx;
        private float curTime;
        private bool isTransition;

        public void Init(PlayableGraph graph, ScriptPlayable<IdleBehaviour> playable, IdleBehaviourData data)
        {
            Init(graph);
            this.playable = playable;
            idleBehaviourData = data;

            mixer = AnimationMixerPlayable.Create(graph, 2);
            playable.AddInput(mixer, 0, 1.0f);

            var animationClipPlayable = ScriptPlayable<AnimationClipBehaviour>.Create(graph);
            idleClipBehaviour = animationClipPlayable.Init(graph, idleBehaviourData.IdleAnim);
            idleIdx = mixer.AddInput(animationClipPlayable, 0);

            var animationRandomPlayable = ScriptPlayable<AnimationRandomBehaviour>.Create(graph);
            randomBehaviour = animationRandomPlayable.Init(graph, idleBehaviourData.IdleRandomAnims);
            randomIdx = mixer.AddInput(animationRandomPlayable, 0);
            // Disable();
        }

        public override void Enable()
        {
            base.Enable();
            playable.SetTime(0);
            mixer.SetTime(0);
            playable.SetSpeed(1);
            mixer.SetSpeed(1);
            curIdx = idleIdx;
            targetIdx = randomIdx;
            curWeight = 1f;
            curTime = idleBehaviourData.RandomTime;
            isTransition = false;
            UpdateWeights();
            AnimationTool.Enable(mixer.GetInput(idleIdx));
            AnimationTool.Disable(mixer.GetInput(randomIdx));
        }

        public override void Disable()
        {
            base.Disable();
            playable.SetSpeed(0);
            mixer.SetSpeed(0);
            AnimationTool.Disable(mixer.GetInput(idleIdx));
            AnimationTool.Disable(mixer.GetInput(randomIdx));
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            curTime -= info.deltaTime;
            if (curTime < 0)
            {
                if (isTransition)
                {
                    if (targetIdx == randomIdx)
                    {
                        // idle -> random
                        var enterTime = randomBehaviour.GetEnterTime();
                        curWeight -= 1 / enterTime * info.deltaTime;
                        if (curWeight <= 0f)
                        {
                            curWeight = 1;
                            isTransition = false;
                            curIdx = targetIdx;
                            targetIdx = idleIdx;
                            curTime = randomBehaviour.GetAnimationClipLength() - idleClipBehaviour.GetEnterTime();
                        }
                    }
                    else
                    {
                        // random -> idle
                        var enterTime = idleClipBehaviour.GetEnterTime();
                        curWeight -= 1 / enterTime * info.deltaTime;
                        if (curWeight <= 0f)
                        {
                            curWeight = 1;
                            isTransition = false;
                            curIdx = targetIdx;
                            targetIdx = randomIdx;
                            curTime = idleBehaviourData.RandomTime;
                        }
                    }
                    UpdateWeights();
                }
                else
                {
                    curWeight = 1f;
                    if (curIdx == idleIdx)
                    {
                        randomBehaviour.Enable();
                        var enterTime = randomBehaviour.GetEnterTime();
                        if (enterTime == 0)
                        {
                            curIdx = randomIdx;
                            targetIdx = idleIdx;
                            curTime = randomBehaviour.GetAnimationClipLength();
                            UpdateWeights();
                        }
                        else
                        {
                            isTransition = true;
                            targetIdx = randomIdx;
                            curTime = enterTime;
                        }
                    }
                    else
                    {
                        idleClipBehaviour.Enable();
                        var enterTime = idleClipBehaviour.GetEnterTime();
                        if (enterTime == 0)
                        {
                            curIdx = idleIdx;
                            targetIdx = randomIdx;
                            curTime = idleBehaviourData.RandomTime;
                            UpdateWeights();
                        }
                        else
                        {
                            isTransition = true;
                            targetIdx = idleIdx;
                            curTime = enterTime;
                        }
                    }
                }
            }
        }

        private void UpdateWeights()
        {
            mixer.SetInputWeight(curIdx, curWeight);
            mixer.SetInputWeight(targetIdx, 1f - curWeight);
            if (curWeight == 0f)
            {
                AnimationTool.Disable(mixer.GetInput(curIdx));
            }
            else if (curWeight == 1f)
            {
                AnimationTool.Disable(mixer.GetInput(targetIdx));
            }
        }

        public override float GetAnimationClipLength()
        {
            if (curIdx == idleIdx)
            {
                return idleClipBehaviour.GetAnimationClipLength();
            }
            else
            {
                return randomBehaviour.GetAnimationClipLength();
            }
        }

        public override float GetEnterTime()
        {
            if (curIdx == idleIdx)
            {
                return idleClipBehaviour.GetEnterTime();
            }
            else
            {
                return randomBehaviour.GetEnterTime();
            }
        }
    }
}