/*
* FileName:          TextMeshProEditor
* CompanyName:       
* Author:            relly
* Description:       
*/

using System.Reflection;
using TMPro;
using UnityEditor;
using UnityFramework.UI;

[CustomEditor(typeof(TextMeshPro))]
public class TextMeshProEditor : Editor
{
    private UnityEditor.Editor m_Editor;

    private void OnEnable()
    {
        Init();

    }
    private void Init()
    {
        Assembly assembly = Assembly.Load("Unity.TextMeshPro.Editor");

        System.Type type = assembly.GetType("TMPro.EditorUtilities.TMP_EditorPanel", false);
        if (type == null)
        {
            return;
        }
        m_Editor = UnityEditor.Editor.CreateEditor(target, type);
        TextMeshPro tmp = target as TextMeshPro;
        TMPText tMPText = tmp.GetComponent<TMPText>();
        if (!tMPText)
        {
            tmp.gameObject.AddComponent<TMPText>();
        }
    }
    public override void OnInspectorGUI()
    {
        if (m_Editor == null) return;
        m_Editor.OnInspectorGUI();
    }
}
