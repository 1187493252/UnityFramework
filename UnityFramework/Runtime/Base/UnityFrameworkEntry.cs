/*
* FileName:          UnityFramework
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UnityFramework.Runtime
{
    public static class UnityFrameworkEntry
    {
        private static readonly FrameworkLinkedList<UnityFrameworkComponent> s_FrameworkComponents = new FrameworkLinkedList<UnityFrameworkComponent>();

        /// <summary>
        /// 获取框架组件。
        /// </summary>
        /// <typeparam name="T">要获取的框架组件类型。</typeparam>
        /// <returns>要获取的框架组件。</returns>
        public static T GetComponent<T>() where T : UnityFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取框架组件。
        /// </summary>
        /// <param name="type">要获取的框架组件类型。</param>
        /// <returns>要获取的框架组件。</returns>
        public static UnityFrameworkComponent GetComponent(Type type)
        {
            LinkedListNode<UnityFrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }
            return null;
        }

        /// <summary>
        /// 获取框架组件。
        /// </summary>
        /// <param name="typeName">要获取的框架组件类型名称。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static UnityFrameworkComponent GetComponent(string typeName)
        {
            LinkedListNode<UnityFrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                Type type = current.Value.GetType();
                if (type.FullName == typeName || type.Name == typeName)
                {
                    return current.Value;
                }

                current = current.Next;
            }
            return null;
        }

        /// <summary>
        /// 关闭框架。
        /// </summary>
        /// <param name="shutdownType">关闭游戏框架类型。</param>
        public static void Shutdown(ShutdownType shutdownType)
        {
            Log.Debug("Shutdown UnityFramework ({0})...", shutdownType.ToString());
            BaseComponent baseComponent = GetComponent<BaseComponent>();
            if (baseComponent != null)
            {
                baseComponent.Shutdown();
                baseComponent = null;
            }

            s_FrameworkComponents.Clear();

            if (shutdownType == ShutdownType.None)
            {
                return;
            }

            if (shutdownType == ShutdownType.Restart)
            {
                SceneManager.LoadScene("FrameworkLauncher");
                return;
            }

            if (shutdownType == ShutdownType.Quit)
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
        }

        /// <summary>
        /// 注册游戏框架组件。
        /// </summary>
        /// <param name="gameFrameworkComponent">要注册的游戏框架组件。</param>
        internal static void RegisterComponent(UnityFrameworkComponent gameFrameworkComponent)
        {
            if (gameFrameworkComponent == null)
            {
                Log.Error(" UnityFramework component is invalid.");
                return;
            }
            Type type = gameFrameworkComponent.GetType();
            LinkedListNode<UnityFrameworkComponent> current = s_FrameworkComponents.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error(" UnityFramework component type '{0}' is already exist.", type.FullName);
                    return;
                }
                current = current.Next;
            }
            s_FrameworkComponents.AddLast(gameFrameworkComponent);
        }
    }
}