using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Animation
{
    public partial class AnimationMono : MonoBehaviour
    {
        #region Animation Events
        private Dictionary<int, List<Action>> animEventDic = new Dictionary<int, List<Action>>();

        public void OnAnimationEvent(int eventId)
        {
            if (animEventDic.ContainsKey(eventId))
            {
                foreach (var action in animEventDic[eventId])
                {
                    action?.Invoke();
                }
            }
        }

        public void RegisterAnimationEvent(int eventId, Action action)
        {
            if (!animEventDic.ContainsKey(eventId))
            {
                animEventDic[eventId] = new List<Action>();
            }
            animEventDic[eventId].Add(action);
        }

        public void UnregisterAnimationEvent(int eventId, Action action)
        {
            if (animEventDic.ContainsKey(eventId))
            {
                animEventDic[eventId].Remove(action);
                if (animEventDic[eventId].Count == 0)
                {
                    animEventDic.Remove(eventId);
                }
            }
        }
        #endregion
        #region Animator Move
        private Action<Vector3, Quaternion> animatorMoveAction;
        public void OnAnimatorMove()
        {
            Vector3 deltaPosition = animator.deltaPosition;
            Quaternion deltaRotation = animator.deltaRotation;
            animatorMoveAction?.Invoke(deltaPosition, deltaRotation);
        }

        public void RegisterAnimatorMove(Action<Vector3, Quaternion> action)
        {
            animatorMoveAction += action;
        }

        public void UnregisterAnimatorMove(Action<Vector3, Quaternion> action)
        {
            animatorMoveAction -= action;
        }
        #endregion

        private void OnDisableEvent()
        {
            foreach (var kvp in animEventDic)
            {
                kvp.Value.Clear();
            }
            animEventDic.Clear();

            if (animatorMoveAction != null)
            {
                animatorMoveAction = null;
            }
        }
    }
}