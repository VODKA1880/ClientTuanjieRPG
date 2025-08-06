using System;
using System.Collections;
using Common.Singleton;

namespace Common.Update
{
    public class UpdateMono : SingletonMono<UpdateMono>
    {
        private Action updateAction;
        private Action lateUpdateAction;
        private Action fixedUpdateAction;

        public void RegisterUpdate(Action action)
        {
            updateAction += action;
        }

        public void UnregisterUpdate(Action action)
        {
            updateAction -= action;
        }

        public void RegisterLateUpdate(Action action)
        {
            lateUpdateAction += action;
        }

        public void UnregisterLateUpdate(Action action)
        {
            lateUpdateAction -= action;
        }

        public void RegisterFixedUpdate(Action action)
        {
            fixedUpdateAction += action;
        }

        public void UnregisterFixedUpdate(Action action)
        {
            fixedUpdateAction -= action;
        }

        private void Update()
        {
            updateAction?.Invoke();
        }
        private void LateUpdate()
        {
            lateUpdateAction?.Invoke();
        }
        private void FixedUpdate()
        {
            fixedUpdateAction?.Invoke();
        }

        public void DoCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void DeletCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            updateAction = null;
            lateUpdateAction = null;
            fixedUpdateAction = null;
        }
    }
}