/*
* FileName:          UIButton
* CompanyName:  
* Author:            
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityFramework;


namespace UnityFramework.UI
{
    public class UIButton : UI<Button>
    {

        GameObject canvas;
        private UIElement uiElement;
        [Header("事件类型")]
        [SerializeField]
        private EventType eventType;

        [Header("面板ID")]
        [SerializeField]
        private int panelIndex;

        [Header("场景名称")]
        public string sceneName;
        public Button Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<Button>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            canvas = GetComponentInParent<Canvas>().gameObject;
            uiElement = new UIElement(canvas);
            AddEventListener(ButtonEvent);

        }

        /// <summary>
        /// 设置按钮交互状态
        /// </summary>
        /// <param name="_state">true/false</param>
        public void SetInteraction(bool _state)
        {
            component.interactable = _state;
        }
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="callBack"></param>
        public void AddEventListener(UnityAction callBack)
        {
            component.onClick.AddListener(callBack);
        }
        public void RemoveEventListener(UnityAction callBack)
        {
            component.onClick.RemoveListener(callBack);
        }
        public void RemoveAllEventListener()
        {
            component.onClick.RemoveAllListeners();
        }
        /// <summary>
        /// 按钮事件
        /// </summary>
        private void ButtonEvent()
        {
            switch (eventType)
            {
                case EventType.MessagePanel:
                    uiElement.OnlyShow(panelIndex);
                    break;
                case EventType.ShowPanel:
                    uiElement.Show(panelIndex);
                    break;
                case EventType.HidePanel:
                    uiElement.Hide(panelIndex);
                    break;
                case EventType.LoadScene:
                    SceneManager.LoadScene(sceneName);

                    break;
                case EventType.ExitGame:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
                    break;
                default:
                    break;
            }
        }
        private enum EventType
        {
            Null,               //空
            MessagePanel,       //通知面板
            ShowPanel,       //显示面板
            HidePanel,        //隐藏面板
            LoadScene,          //加载场景
            ExitGame            //退出游戏
        }
    }
}