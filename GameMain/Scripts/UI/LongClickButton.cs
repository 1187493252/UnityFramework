
/*=========================================
* Author: springDong
* Description: SpringGUI.LongClickButton,you can use it like use normal button by addListener.
==========================================*/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityFramework.UI
{
    [AddComponentMenu("UI/LongClickButton")]
    public class LongClickButton : Button
    {
        [Header("触发时间:毫秒")]
        public float m_milliSeconds = 600f;

        [Serializable]
        public class LongClickEvent : UnityEvent { }

        [SerializeField]
        private LongClickEvent m_onLongClick = null;
        public LongClickEvent onLongClick
        {
            get { return m_onLongClick; }
            set { m_onLongClick = value; }
        }

        private DateTime m_firstTime = default(DateTime);
        private DateTime m_secondTime = default(DateTime);

        private void Press()
        {
            if (null != onLongClick)
                onLongClick.Invoke();
            resetTime();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (m_firstTime.Equals(default(DateTime)))
                m_firstTime = DateTime.Now;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!m_firstTime.Equals(default(DateTime)))
                m_secondTime = DateTime.Now;
            if (!m_firstTime.Equals(default(DateTime)) && !m_secondTime.Equals(default(DateTime)))
            {
                var intervalTime = m_secondTime - m_firstTime;
                int milliSeconds = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
                if (milliSeconds > m_milliSeconds) Press();
                else resetTime();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            resetTime();
        }

        private void resetTime()
        {
            m_firstTime = default(DateTime);
            m_secondTime = default(DateTime);
        }
    }
}