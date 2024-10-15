/*
* FileName:          UIText
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
    public class UIText : UI<Text>
    {
        public Text Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<Text>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            component.raycastTarget = false;
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="contents">文本内容</param>
        public void UpdateContent(string _content)
        {
            component.text = _content;
        }
    }
}