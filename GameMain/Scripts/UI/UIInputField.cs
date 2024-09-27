/*
* FileName:          UIInputField
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
    public class UIInputField : UI<InputField>
    {
        public InputField Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<InputField>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            component.onValueChanged.AddListener(ValueChangeEvent);
            component.onEndEdit.AddListener(EndEvent);
        }

        /// <summary>
        /// 获取输入内容
        /// </summary>
        /// <returns>返回在输入框所输入的内容</returns>
        public string GetInputValue()
        {
            return component.text;
        }

        /// <summary>
        /// 输入时执行事件
        /// </summary>
        /// <param name="_content"></param>
        protected virtual void ValueChangeEvent(string _content)
        {

        }

        /// <summary>
        /// 输入完成后执行事件
        /// </summary>
        /// <param name="_content"></param>
        protected virtual void EndEvent(string _content)
        {

        }
    }
}