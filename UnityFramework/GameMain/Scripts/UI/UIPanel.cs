/*
* FileName:          UIPanel
* CompanyName:  
* Author:            
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.UI
{
    public class UIPanel : UI<UIPanel>
    {
        public UIButton[] UIButtons;
        protected override void OnInit()
        {
            base.OnInit();
            UIButtons = GetComponentsInChildren<UIButton>();
        }

    }
}