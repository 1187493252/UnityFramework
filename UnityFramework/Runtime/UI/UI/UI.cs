using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.UI
{
    public abstract class UI<T> : MonoBehaviour where T : Component
    {
        protected T component;
        private void Awake()
        {
            OnInit();
        }
        protected virtual void OnInit()
        {
            component = GetComponent<T>();
        }
        public void Show()
        {
            SetVisible(true);
        }
        public void Hide()
        {
            SetVisible(false);
        }
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}