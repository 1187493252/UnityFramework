using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace K_UnityGF
{
    /// <summary>
    /// EventTrigger 拓展
    /// </summary>
    public static class EventTriggerExtension
    {
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="eventTrigger">eventTrigger</param>
        /// <param name="type">类型</param>
        /// <param name="action">回调</param>
        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            UnityAction<BaseEventData> callBack = new UnityAction<BaseEventData>(delegate { action(); });
            entry.callback.AddListener(callBack);
            eventTrigger.triggers.Add(entry);
        }
    }
}
