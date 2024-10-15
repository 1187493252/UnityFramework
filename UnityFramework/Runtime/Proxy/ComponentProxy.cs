/*
* FileName:          ComponentProxy
* CompanyName:       
* Author:            relly
* Description:       组件代理器基类
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public abstract class ComponentProxy<T> where T : Component
    {
        public T Agent { get; private set; }
        public ComponentProxy(GameObject gameObject)
        {
            Agent = gameObject.GetComponent<T>();
        }
    }
}
