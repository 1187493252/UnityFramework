/*
* FileName:          MonoComponent
* CompanyName:       
* Author:            relly
* Description:       用于非继承 MonoBehaviour 脚本监听 Mono生命周期与逻辑执行
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UnityFramework.Runtime
{
    public sealed class MonoBehaviourComponent : UnityFrameworkComponent
    {
        private UnityEvent OnFixedUpdate;
        private UnityEvent OnUpdate;
        private UnityEvent OnLateUpdate;

        protected override void Awake()
        {
            base.Awake();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        private void Update()
        {
            OnUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        public void AddFixedUpdateListener(UnityAction unityAction)
        {
            OnFixedUpdate.AddListener(unityAction);
        }
        public void RemoveFixedUpdateListener(UnityAction unityAction)
        {
            OnFixedUpdate.RemoveListener(unityAction);
        }
        public void RemoveAllFixedUpdateListener(UnityAction unityAction)
        {
            OnFixedUpdate.RemoveAllListeners();
        }

        public void AddUpdateListener(UnityAction unityAction)
        {
            OnUpdate.AddListener(unityAction);
        }
        public void RemoveUpdateListener(UnityAction unityAction)
        {
            OnUpdate.RemoveListener(unityAction);
        }
        public void RemoveAllUpdateListener(UnityAction unityAction)
        {
            OnUpdate.RemoveAllListeners();
        }

        public void AddLateUpdateListener(UnityAction unityAction)
        {
            OnLateUpdate.AddListener(unityAction);
        }
        public void RemoveLateUpdateListener(UnityAction unityAction)
        {
            OnLateUpdate.RemoveListener(unityAction);
        }
        public void RemoveAllLateUpdateListener(UnityAction unityAction)
        {
            OnLateUpdate.RemoveAllListeners();
        }
    }
}
