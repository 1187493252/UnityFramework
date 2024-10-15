/*
* FileName:          RayProxy
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class RayProxy
    {
        public Action<object, object> OnPointerEnter;
        public Action<object, object> OnPointerHover;
        public Action<object, object> OnPointerExit;
        public Action<object, object> OnPointerClick;
        public Action<object, object> OnPointerDown;
        public Action<object, object> OnPointerUp;

        public void Clear()
        {
            OnPointerEnter = null;
            OnPointerHover = null;
            OnPointerExit = null;
            OnPointerClick = null;
            OnPointerDown = null;
            OnPointerUp = null;
        }
    }
}
