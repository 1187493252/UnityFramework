/*
* FileName:          UISlider
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
    public class UISlider : UI<Slider>
    {
        public Slider Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<Slider>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            component.onValueChanged.AddListener(ValueChangeEvent);
        }

        /// <summary>
        /// 获取滑动条值 
        /// </summary>
        /// <returns>返回当前滑动条的值</returns>
        public float GetSliderValue() { return component.value; }

        /// <summary>
        /// 滑动条值变事件
        /// </summary>
        /// <param name="_value"></param>
        protected virtual void ValueChangeEvent(float _value) { }
    }
}