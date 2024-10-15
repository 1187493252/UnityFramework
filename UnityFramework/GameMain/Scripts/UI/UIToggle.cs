/*
* FileName:          UIToggle
* CompanyName:  
* Author:            
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityFramework;


namespace UnityFramework.UI
{
    public class ToggleEvent : UnityEvent<bool, List<GameObject>> { };
    public class UIToggle : UI<Toggle>
    {
        public ToggleEvent ToggleEvent;
        public List<GameObject> Target;
        public Toggle Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<Toggle>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            if (ToggleEvent == null)
            {
                ToggleEvent = new ToggleEvent();
            }
            component.onValueChanged.AddListener(ValueChangeEvent);
        }

        /// <summary>
        /// 值变事件
        /// </summary>
        /// <param name="_bool"></param>
        protected virtual void ValueChangeEvent(bool _bool)
        {
            ToggleEvent?.Invoke(_bool, Target);
        }
    }
}