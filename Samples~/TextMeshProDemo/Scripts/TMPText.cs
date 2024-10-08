/*
* FileName:          TMPTextUI
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityFramework.UI
{
    [DisallowMultipleComponent]
    public class TMPText : UI<TMP_Text>, IPointerClickHandler
    {
        public TMP_Text Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<TMP_Text>();
                }
                return component;
            }
        }

        [Serializable]
        public class LinkClickEvent : UnityEvent<string, string, int> { }
        [SerializeField]
        private LinkClickEvent m_OnLinkClick = new LinkClickEvent();

        /// <summary>
        /// 链接点击事件,一定要在OnEnable里监听
        /// </summary>
        public LinkClickEvent OnLinkClick
        {
            get { return m_OnLinkClick; }
            set { m_OnLinkClick = value; }
        }

        private Camera m_Camera;
        private Canvas m_Canvas;

        protected override void OnInit()
        {
            base.OnInit();

            if (Component.GetType() == typeof(TextMeshProUGUI))
            {
                m_Canvas = gameObject.GetComponentInParent<Canvas>();
                if (m_Canvas != null)
                {
                    if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        m_Camera = null;
                    else
                        m_Camera = m_Canvas.worldCamera;
                }
            }
            else
            {
                m_Camera = Camera.main;
            }
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="contents">文本内容</param>
        public void UpdateContent(string _content)
        {
            Component.text = _content;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex =
                TMP_TextUtilities.FindIntersectingLink(Component, eventData.position, m_Camera);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = GetComponent<TMP_Text>().textInfo.linkInfo[linkIndex];
                OnLinkClick?.Invoke(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
            }
        }

        private void OnMouseDown()
        {
            int linkIndex =
             TMP_TextUtilities.FindIntersectingLink(Component, Input.mousePosition, m_Camera);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = GetComponent<TMP_Text>().textInfo.linkInfo[linkIndex];
                OnLinkClick?.Invoke(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
            }
        }
    }
}
