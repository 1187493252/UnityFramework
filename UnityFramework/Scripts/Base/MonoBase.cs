/*
* FileName:          MonoBase
* CompanyName:  
* Author:            relly
* Description:
* 
*/

using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class MonoBase : MonoBehaviour
    {





        public virtual void Show()
        {
            SetVisible(true);
        }
        public virtual void Hide(bool isDestroy = false)
        {
            SetVisible(false);
        }
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
        public Transform FindChildByName(string _targetName)
        {
            Transform child = null;
            child = transform.FindChildByName(_targetName);
            return child;
        }
        public T GetComponentByChild<T>(string childName) where T : Component
        {
            T component = null;
            component = transform.GetComponentByChild<T>(childName);
            return component;
        }
    }
}