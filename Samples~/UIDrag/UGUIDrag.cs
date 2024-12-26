using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MinorFunction
{
    public enum DragMode
    {
        // 拖拽模式枚举
        Free, // 自由拖拽
        WithinScreen, // 在屏幕内拖拽
        WithinParent, // 在父对象内拖拽
        OutsideScreen, // 在屏幕外拖拽
        OutsideParent // 在父对象外拖拽
    }
    // 实现拖动功能
    public class UGUIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        public DragMode dragMode = DragMode.WithinScreen;
        // 拖动置顶层级
        public bool dragTop = false;
        // 是否偏移
        public bool isOffset = true;
        // 偏移向量
        private Vector2 offset = Vector2.zero;

        public UnityEvent<Vector2> onDragStart;
        public UnityEvent<Vector2> onDrag;
        public UnityEvent<Vector2> onDragEnd;

        bool clickSelectable;//是否点击可交互UI,比如按钮
        Selectable selectable;

        public void OnPointerEnter(PointerEventData eventData)
        {


        }
        public void OnBeginDrag(PointerEventData eventData)
        {

            onDragStart?.Invoke(eventData.position);
            if (dragTop)
            {
                transform.SetAsLastSibling();
            }

            // 根据点击位置计算偏移量
            offset = eventData.position - (Vector2)transform.position;

        }


        public void OnDrag(PointerEventData eventData)
        {
            if (clickSelectable)
            {
                return;
            }

            onDrag?.Invoke(eventData.position);

            switch (dragMode)
            {
                case DragMode.Free:
                    SetFreeDrag(eventData);
                    break;
                case DragMode.WithinScreen:
                    SetDragWithinScreen(eventData);
                    break;
                case DragMode.WithinParent:
                    SetDragWithinParent(eventData);
                    break;
                case DragMode.OutsideScreen:
                    SetDragOutsideScreen(eventData);
                    break;
                case DragMode.OutsideParent:
                    SetDrayOutsideParent(eventData);
                    break;
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (clickSelectable)
            {
                return;
            }

            onDragEnd?.Invoke(eventData.position);

        }

        public void SetDragTop(bool dragTop)
        {
            this.dragTop = dragTop;
        }


        #region 拖动模式
        // 自由拖动
        public void SetFreeDrag(PointerEventData eventData)
        {
            if (isOffset)
            {
                transform.position = eventData.position - offset;
            }
            else
            {
                transform.position = eventData.position;
            }
        }
        // 设置屏幕内拖动
        public void SetDragWithinScreen(PointerEventData eventData)
        {
            RectTransform rectTransform = transform as RectTransform;
            Vector2 pos = Vector2.zero;
            if (isOffset)
            {
                pos = eventData.position - offset;
            }
            else
            {
                pos = eventData.position;
            }


            // 屏幕范围
            float parentMinX = 0;
            float parentMinY = 0;
            float parentMaxX = Screen.width;
            float parentMaxY = Screen.height;

            var pivotDistance = GetPivotDistance(rectTransform);

            float pivotMinX = pivotDistance.pivotMinX;
            float pivotMinY = pivotDistance.pivotMinY;
            float pivotMaxX = pivotDistance.pivotMaxX;
            float pivotMaxY = pivotDistance.pivotMaxY;



            // 限制范围
            float minX = parentMinX + pivotMinX;
            float minY = parentMinY + pivotMinY;
            float maxX = parentMaxX - pivotMaxX;
            float maxY = parentMaxY - pivotMaxY;


            // 限制拖动范围
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


            transform.position = pos;
        }
        // 限制矩形内拖动
        public void SetDragWithinParent(PointerEventData eventData)
        {
            RectTransform rectTransform = transform as RectTransform;
            RectTransform parentRectTransform = transform.parent as RectTransform;
            Vector2 pos = Vector2.zero;
            if (isOffset)
            {
                pos = eventData.position - offset;
            }
            else
            {
                pos = eventData.position;
            }




            var parentMinMax = SetMinMax(parentRectTransform);

            float parentMinX = parentMinMax.minX;
            float parentMinY = parentMinMax.minY;
            float parentMaxX = parentMinMax.maxX;
            float parentMaxY = parentMinMax.maxY;

            var pivotDistance = GetPivotDistance(rectTransform);

            float pivotMinX = pivotDistance.pivotMinX;
            float pivotMinY = pivotDistance.pivotMinY;
            float pivotMaxX = pivotDistance.pivotMaxX;
            float pivotMaxY = pivotDistance.pivotMaxY;

            // 限制范围
            float minX = parentMinX + pivotMinX;
            float minY = parentMinY + pivotMinY;
            float maxX = parentMaxX - pivotMaxX;
            float maxY = parentMaxY - pivotMaxY;


            // 限制拖动范围
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


            transform.position = pos;
        }
        // 限制屏幕外拖动
        public void SetDragOutsideScreen(PointerEventData eventData)
        {
            RectTransform rectTransform = transform as RectTransform;
            Vector2 pos = Vector2.zero;
            if (isOffset)
            {
                pos = eventData.position - offset;
            }
            else
            {
                pos = eventData.position;
            }




            // 屏幕范围
            float parentMinX = 0;
            float parentMinY = 0;
            float parentMaxX = Screen.width;
            float parentMaxY = Screen.height;


            var pivotDistance = GetPivotDistance(rectTransform);

            float pivotMinX = pivotDistance.pivotMinX;
            float pivotMinY = pivotDistance.pivotMinY;
            float pivotMaxX = pivotDistance.pivotMaxX;
            float pivotMaxY = pivotDistance.pivotMaxY;


            //限制范围
            float minX = parentMaxX - pivotMaxX;
            float minY = parentMaxY - pivotMaxY;
            float maxX = parentMinX + pivotMinX;
            float maxY = parentMinY + pivotMinY;


            // 限制拖动范围
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


            transform.position = pos;
        }
        // 限制父物体外拖动
        public void SetDrayOutsideParent(PointerEventData eventData)
        {
            RectTransform rectTransform = transform as RectTransform;
            RectTransform parentRectTransform = transform.parent as RectTransform;
            Vector2 pos = Vector2.zero;
            if (isOffset)
            {
                pos = eventData.position - offset;
            }
            else
            {
                pos = eventData.position;
            }

            var parentMinMax = SetMinMax(parentRectTransform);

            float parentMinX = parentMinMax.minX;
            float parentMinY = parentMinMax.minY;
            float parentMaxX = parentMinMax.maxX;
            float parentMaxY = parentMinMax.maxY;


            var pivotDistance = GetPivotDistance(rectTransform);

            float pivotMinX = pivotDistance.pivotMinX;
            float pivotMinY = pivotDistance.pivotMinY;
            float pivotMaxX = pivotDistance.pivotMaxX;
            float pivotMaxY = pivotDistance.pivotMaxY;

            // 限制范围
            float minX = parentMaxX - pivotMaxX;
            float minY = parentMaxY - pivotMaxY;
            float maxX = parentMinX + pivotMinX;
            float maxY = parentMinY + pivotMinY;


            // 限制拖动范围
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


            transform.position = pos;


        }
        #endregion

        // 计算物体中心点到四条边的距离
        public (float pivotMinX, float pivotMinY, float pivotMaxX, float pivotMaxY) GetPivotDistance(RectTransform rectTransform)
        {
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            // 注：这里一定要使用全局坐标，不然计算会受到父物体缩放的影响
            float scaleX = rectTransform.lossyScale.x;
            float scaleY = rectTransform.lossyScale.y;

            Vector2 pivot = rectTransform.pivot;
            float pivotMinX = width * pivot.x * scaleX;
            float pivotMinY = height * pivot.y * scaleY;
            float pivotMaxX = width * (1 - pivot.x) * scaleX;
            float pivotMaxY = height * (1 - pivot.y) * scaleY;
            return (pivotMinX, pivotMinY, pivotMaxX, pivotMaxY);
        }

        // 计算物体X，Y最大值最小值
        public (float minX, float minY, float maxX, float maxY) SetMinMax(RectTransform rectTransform)
        {

            float parentX = rectTransform.position.x;
            float parentY = rectTransform.position.y;

            var pivotDistance = GetPivotDistance(rectTransform);

            float pivotMinX = pivotDistance.pivotMinX;
            float pivotMinY = pivotDistance.pivotMinY;
            float pivotMaxX = pivotDistance.pivotMaxX;
            float pivotMaxY = pivotDistance.pivotMaxY;

            float minX = parentX - pivotMinX;
            float minY = parentY - pivotMinY;
            float maxX = parentX + pivotMaxX;
            float maxY = parentY + pivotMaxY;

            return (minX, minY, maxX, maxY);
        }
    }
}

