/*
* FileName:          LongClickButtonInspector
* CompanyName:  
* Author:            relly
* Description:       自定义脚本Inspector面板
* 
*/
#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;
using UnityFramework.UI;

namespace UnityFramework.Editor
{
    [CustomEditor(typeof(DoubleClickButton), true)]
    public class DoubleClickButtonInspector : ButtonEditor
    {
        SerializedObject obj;
        SerializedProperty m_milliSeconds;
        SerializedProperty m_onDoubleClick;
        protected override void OnEnable()
        {
            base.OnEnable();
            obj = new SerializedObject(target);
            m_milliSeconds = obj.FindProperty("m_milliSeconds");
            m_onDoubleClick = obj.FindProperty("m_onDoubleClick");

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            obj.Update();
            EditorGUILayout.PropertyField(m_milliSeconds);
            EditorGUILayout.PropertyField(m_onDoubleClick);

            obj.ApplyModifiedProperties();

        }
    }


}
#endif