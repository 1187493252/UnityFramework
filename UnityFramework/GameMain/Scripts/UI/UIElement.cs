/*
* FileName:          UIElement
* CompanyName:  
* Author:            
* Description:       UI元素:button,text,image等
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.UI
{
    public class UIElement
    {

        protected GameObject parentElement;
        private List<GameObject> elements = new List<GameObject>();

        public UIElement(GameObject _target)
        {
            parentElement = _target;
            elements.Clear();
            foreach (Transform child in _target.transform)
            {
                Add(child.gameObject);
            }
        }



        public virtual void Add(GameObject _childElement)
        {
            if (!elements.Contains(_childElement))
            {
                elements.Add(_childElement);
            }
            else
            {
                Debug.LogError(string.Format("{0} 元素已经存在于列表中,不能重复添加", _childElement.name));
            }
        }

        public virtual void Remove(GameObject _childElement)
        {
            if (elements.Contains(_childElement))
            {
                elements.Remove(_childElement);
            }
            else
            {
                Debug.LogError(string.Format("{0} 元素在列表中不存在", _childElement.name));
            }
        }

        public virtual void Show(int _elementIndex)
        {
            if (_elementIndex < elements.Count)
            {
                elements[_elementIndex].SetActive(true);
            }
            else
            {
                Debug.LogError("元素超索引");
            }
        }

        public virtual void Hide(int _elementIndex)
        {
            if (_elementIndex < elements.Count)
            {
                elements[_elementIndex].SetActive(false);
            }
            else
            {
                Debug.LogError("元素超索引");
            }
        }
        /// <summary>
        /// 显示 唯一指定元素 其他元素隐藏
        /// </summary>
        /// <param name="_elementIndex"></param>
        public virtual void OnlyShow(int _elementIndex)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (i.Equals(_elementIndex))
                {
                    elements[i].SetActive(true);
                }
                else
                {
                    elements[i].SetActive(false);
                }
            }
        }

        public virtual void BatchShow(int[] _indexArray)
        {
            foreach (int index in _indexArray)
            {
                if (index < elements.Count)
                {
                    elements[index].SetActive(true);
                }
                else
                {
                    Debug.LogError("元素超索引");
                }
            }
        }

        public virtual void BatchHide(int[] _indexArray)
        {
            foreach (int index in _indexArray)
            {
                if (index < elements.Count)
                {
                    elements[index].SetActive(false);
                }
                else
                {
                    Debug.LogError("元素超索引");
                }
            }
        }
        /// <summary>
        /// 查找子对象
        /// </summary>
        /// <param name="_targetName">对象名称</param>
        /// <returns></returns>
        public GameObject FindChild(string _targetName)
        {
            return parentElement.transform.FindChildByName(_targetName).gameObject;
        }
        /// <summary>
        /// 从子对象身上获取某个组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="_targetName">对象名称</param>
        /// <returns></returns>
        public T GetComponentByChildName<T>(string _targetName) where T : Component
        {
            return parentElement.transform.GetComponentByChild<T>(_targetName);
        }
    }
}