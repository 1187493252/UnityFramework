/*
* FileName:          UIScrollRect
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
using UnityEngine.UI;
using UnityFramework;


namespace UnityFramework.UI
{
    public class UIScrollRect : UI<ScrollRect>
    {
        public ScrollRect Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<ScrollRect>();
                }
                return component;
            }
        }
        private RectTransform viewport;                 //viewport
        private RectTransform content;                  //Content
        private List<RectTransform> itemRectList;       //元素列表
        private List<int> pageIndexList;                //分页列表
        private int pageIndex;                          //页数索引
        protected override void OnInit()
        {
            base.OnInit();
            itemRectList = new List<RectTransform>();
            pageIndexList = new List<int>();

            viewport = Component.viewport;
            content = Component.content;
            component.onValueChanged.AddListener(ValueChangeEvent);
        }

        /// <summary>
        /// 初始化元素列表
        /// </summary>
        public void InitItemRectList()
        {
            itemRectList.Clear();
            foreach (Transform child in content.transform)
            {
                itemRectList.Add(child.GetComponent<RectTransform>());
            }
        }

        /// <summary>
        /// 刷新到最上方
        /// </summary>
        public void PageTop() { Component.verticalNormalizedPosition = 1; }

        /// <summary>
        /// 刷新到最下方
        /// </summary>
        public void PageButtom() { Component.verticalNormalizedPosition = 0; }

        /// <summary>
        /// 刷新到最左侧
        /// </summary>
        public void PageLeft() { Component.horizontalNormalizedPosition = 0; }

        /// <summary>
        /// 刷新到最右侧
        /// </summary>
        public void PageRight() { Component.horizontalNormalizedPosition = 1; }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="list">分页索引列表</param>
        public void SetPage(List<int> list)
        {
            pageIndexList = list;
            pageIndex = 0;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public void PreviousPage()
        {
            if (pageIndex >= 1)
            {
                Nevigate(pageIndexList[--pageIndex]);
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public void NextPage()
        {
            if (pageIndex < pageIndexList.Count - 1)
            {
                Nevigate(pageIndexList[++pageIndex]);
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="itemIndex"></param>
        public void Nevigate(int itemIndex)
        {
            if (itemIndex >= 0 && itemIndex < itemRectList.Count)
            {
                Nevigate(itemRectList[itemIndex]);
            }
            else
            {
                Debug.LogError("元素超索引");
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="item"></param>
        public void Nevigate(RectTransform item)
        {
            Vector3 itemCurrentLocalPostion = Component.GetComponent<RectTransform>().InverseTransformVector(ConvertLocalPosToWorldPos(item));
            Vector3 itemTargetLocalPos = Component.GetComponent<RectTransform>().InverseTransformVector(ConvertLocalPosToWorldPos(viewport));

            Vector3 diff = itemTargetLocalPos - itemCurrentLocalPostion;
            diff.z = 0.0f;

            var newNormalizedPosition = new Vector2(
                diff.x / (content.GetComponent<RectTransform>().rect.width - viewport.rect.width),
                diff.y / (content.GetComponent<RectTransform>().rect.height - viewport.rect.height)
                );

            newNormalizedPosition = Component.normalizedPosition - newNormalizedPosition;

            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

            //  DOTween.To(() => scrollRect.GetComponent<ScrollRect>().normalizedPosition, x => scrollRect.GetComponent<ScrollRect>().normalizedPosition = x, newNormalizedPosition, 0.8f);
            Component.normalizedPosition = newNormalizedPosition;

        }

        /// <summary>
        /// 转化坐标
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Vector3 ConvertLocalPosToWorldPos(RectTransform target)
        {
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);

            var localPosition = target.localPosition + pivotOffset;

            return target.parent.TransformPoint(localPosition);
        }

        public Vector2 GetScrollRectValue() { return component.normalizedPosition; }


        protected virtual void ValueChangeEvent(Vector2 _value) { }
    }
}