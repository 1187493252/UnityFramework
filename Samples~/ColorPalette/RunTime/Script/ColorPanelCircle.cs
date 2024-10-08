using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace K_UnityGF.ColorPalette
{
    /// <summary>
    /// 颜色面板 圆圈
    /// </summary>
    public class ColorPanelCircle : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        private RectTransform rect;
        private int width = 256;
        private int height = 256;

        public RectTransform Rect { get => rect; }

        public event Action<Vector2> Event_UpdatePosition;

        private void Start()
        {
            rect = transform.GetChild(0).GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdatePosition(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector3 wordPos;

            //将UGUI的坐标转为世界坐标
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(Rect, eventData.position, eventData.pressEventCamera, out wordPos))
            {
                Rect.position = wordPos;
            }

            if (Rect.anchoredPosition.x <= 0)
                Rect.anchoredPosition = new Vector2(0, Rect.anchoredPosition.y);
            if (Rect.anchoredPosition.y <= 0)
                Rect.anchoredPosition = new Vector2(Rect.anchoredPosition.x, 0);
            if (Rect.anchoredPosition.x >= width - 1)
                Rect.anchoredPosition = new Vector2(width - 1, Rect.anchoredPosition.y);
            if (Rect.anchoredPosition.y >= height - 1)
                Rect.anchoredPosition = new Vector2(Rect.anchoredPosition.x, height - 1);

            UpdatePosition(eventData);
        }

        void UpdatePosition(PointerEventData eventData)
        {
            Vector3 wordPos;
            //将UGUI的坐标转为世界坐标  
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(Rect, eventData.position, eventData.pressEventCamera, out wordPos))
            {
                Rect.position = wordPos;
                Event_UpdatePosition(Rect.anchoredPosition);
            }

            if (Rect.anchoredPosition.x <= 0)
                Rect.anchoredPosition = new Vector2(0, Rect.anchoredPosition.y);
            if (Rect.anchoredPosition.y <= 0)
                Rect.anchoredPosition = new Vector2(Rect.anchoredPosition.x, 0);
            if (Rect.anchoredPosition.x >= width - 1)
                Rect.anchoredPosition = new Vector2(width - 1, Rect.anchoredPosition.y);
            if (Rect.anchoredPosition.y >= height - 1)
                Rect.anchoredPosition = new Vector2(Rect.anchoredPosition.x, height - 1);
        }
    }
}
