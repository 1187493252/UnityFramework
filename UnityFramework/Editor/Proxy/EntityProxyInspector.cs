/*
* FileName:          EntityProxyInspector
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(EntityProxy))]
    public class EntityProxyInspector : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EntityProxy script = target as EntityProxy;

            //if (GUILayout.Button("添加高亮代理"))
            //{
            //    if (script.HighlightProxy != null)
            //    {
            //        Debug.Log($"当前物体已有高亮代理");
            //        return;
            //    }
            //    if (script.highlightProxyType == EntityProxy.HighlightProxyType.HighlightEffect)
            //    {
            //        script.gameObject.AddComponent<HighlightEffectProxy>();
            //    }
            //    else
            //    {
            //        script.gameObject.AddComponent<HighlighterProxy>();

            //    }
            //}
            if (GUILayout.Button("添加动画代理"))
            {
                if (script.AnimProxy != null)
                {
                    Debug.Log($"当前物体已有动画代理");
                    return;
                }
                if (script.animProxyType == EntityProxy.AnimProxyType.Animation)
                {
                    script.gameObject.AddComponent<AnimationProxy>();
                }
                else
                {
                    script.gameObject.AddComponent<AnimatorProxy>();
                }
            }
        }


    }
}