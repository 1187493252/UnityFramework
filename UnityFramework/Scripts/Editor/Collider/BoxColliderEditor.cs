/*
* FileName:          BoxColliderEditor
* CompanyName:  
* Author:            Relly
* Description:       
* 
*/

using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace UnityFramework.Editor
{

    [CustomEditor(typeof(BoxCollider2D))]
    public class BoxColliderExtendEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor m_Editor;
        private void OnEnable()
        {
            Init();

        }
        private void Init()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            System.Type type = assembly.GetType("UnityEditor.BoxCollider2DEditor", false);
            if (type == null)
            {
                return;
            }
            m_Editor = UnityEditor.Editor.CreateEditor(target, type);
        }

        public override void OnInspectorGUI()
        {
            if (m_Editor == null) return;
            m_Editor.OnInspectorGUI();

            BoxCollider2D boxCollider2D = target as BoxCollider2D;

            RectTransform rt = boxCollider2D.GetComponent<RectTransform>();

            boxCollider2D.offset = rt.rect.center;//把box collider设置到物体的中心
            boxCollider2D.size = new Vector2(rt.rect.width, rt.rect.height);//改变collider大小




        }
    }

}


public partial class UnityEditorTools
{
    [MenuItem("GameObject/Collider/给规则物体的父级添加BoxCollider", false, 1)]
    private static void AutoBoxCollider()
    {
        //如果未选中任何物体 返回
        GameObject gameObject = Selection.activeGameObject;
        if (gameObject == null) return;
        //计算中心点
        Vector3 center = Vector3.zero;
        var renders = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            center += renders[i].bounds.center;
        }
        center /= renders.Length;
        //创建边界盒
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (var render in renders)
        {
            bounds.Encapsulate(render.bounds);
        }
        //先判断当前是否有碰撞器 进行销毁
        var currentCollider = gameObject.GetComponent<Collider>();
        if (currentCollider != null) UnityEngine.Object.DestroyImmediate(currentCollider);
        //添加BoxCollider 设置中心点及大小
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - gameObject.transform.position;
        boxCollider.size = bounds.size;
    }


}
