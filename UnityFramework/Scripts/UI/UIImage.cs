/*
* FileName:          UIImage
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
    public class UIImage : UI<Image>
    {
        public Image Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<Image>();
                }
                return component;
            }
        }
        public float alphaHitMiniThreshold = 0;

        protected override void OnInit()
        {
            base.OnInit();
            component.alphaHitTestMinimumThreshold = alphaHitMiniThreshold;
        }
        /// <summary>
        /// 更新精灵图片
        /// </summary>
        /// <param name="_sprite"></param>
        public void ChangeSprite(Sprite _sprite)
        {
            component.sprite = _sprite;
        }
    }
}