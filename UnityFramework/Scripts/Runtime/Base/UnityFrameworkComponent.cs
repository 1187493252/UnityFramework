/*
* FileName:          FrameworkComponent
* CompanyName:  
* Author:            relly
* Description:       
* 
*/


using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 框架组件抽象类
    /// </summary>
    public abstract class UnityFrameworkComponent : MonoBehaviour
    {
        /// <summary>
        /// 框架组件初始化。
        /// </summary>
        protected virtual void Awake()
        {
            UnityFrameworkEntry.RegisterComponent(this);
        }
    }
}