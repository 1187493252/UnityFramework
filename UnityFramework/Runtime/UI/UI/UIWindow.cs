/*
* FileName:          UIWindow
* CompanyName:       
* Author:            
* Description:       
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    private EventSystem eventSystem;
    private GraphicRaycaster graphicRaycaster;
    public UnityEvent onClose;
    private void Awake()
    {
        // 获取 EventSystem 和 GraphicRaycaster
        eventSystem = FindObjectOfType<EventSystem>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    private void Start()
    {

    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;

#elif UNITY_IOS || UNITY_ANDROID
  
         Touch touch0 = Input.GetTouch(0);
        // 检测触摸是否在 UI 上
        if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId))
        {
            Vector2 mousePosition =touch0.position;

#endif

            // 创建 PointerEventData 并执行射线检测
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            // 检查结果
            bool clickedOnUI = false;

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.transform.IsChildOf(this.transform))
                {
                    clickedOnUI = true;
                    break;
                }
            }

            // 如果没有点击到 UI 窗口或其子元素，关闭 UI 窗口
            if (!clickedOnUI)
            {
                CloseUIWindow(this.gameObject);
            }
        }
    }


    public void CloseUIWindow(GameObject uiWindow)
    {
        // 关闭 UI 窗口
        uiWindow.SetActive(false);
        onClose?.Invoke();
    }
}
