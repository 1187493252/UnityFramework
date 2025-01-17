/*
* FileName:          CoroutineComponent
* CompanyName:       
* Author:            relly
* Description:       
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityFramework.Runtime
{
    public sealed class CoroutineComponent : UnityFrameworkComponent
    {
        private IEnumerator coroutine;
        private Dictionary<string, IEnumerator> dic = new Dictionary<string, IEnumerator>();

        public int Count
        {
            get
            {
                return dic.Count;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            StopAllDelay();
        }
        private void Start()
        {

        }

        public string DelayExecute(float delayTime, UnityAction callBack)
        {
            string _name = callBack.GetHashCode().ToString();
            coroutine = DelayExecuteCommand(delayTime, callBack);
            dic.AddOrReplace(_name, coroutine);
            StartCoroutine(coroutine);
            return _name;
        }

        public void DelayExecute(string name, float delayTime, UnityAction callBack)
        {
            coroutine = DelayExecuteCommand(delayTime, callBack);
            dic.AddOrReplace(name, coroutine);
            StartCoroutine(coroutine);
        }

        public void StopDelay()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        public void StopDelay(string name)
        {
            if (dic.ContainsKey(name))
            {
                StopCoroutine(dic[name]);
                dic.Remove(name);
            }
        }
        public void StopAllDelay()
        {

            foreach (var item in dic)
            {
                StopCoroutine(item.Key);
            }
            dic.Clear();
            coroutine = null;
        }

        public List<string> GetAllCoroutineNames()
        {
            List<string> nameList = new List<string>();
            foreach (var item in dic)
            {
                nameList.Add(item.Key);
            }
            return nameList;
        }

        IEnumerator DelayExecuteCommand(float delayTime, UnityAction callBack)
        {
            yield return new WaitForSeconds(delayTime);
            callBack?.Invoke();
        }

    }
}
