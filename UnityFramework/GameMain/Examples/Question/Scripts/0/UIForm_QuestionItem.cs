/*
* FileName:          
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;
using UnityFramework.Runtime;
using UnityFramework.Runtime.UI;

namespace PCI
{
    public class UIForm_QuestionItem : UIFormBase
    {
        [Header("是否有答案解析")]
        public bool IsHasExplain;




        public GameObject questionObj;

        public GameObject completeObj;
        public GameObject closeObj;
        public CanvasGroup canvasGroup;
        public Transform questionItem_Parent;
        public Transform questionItem_clone;
        public Button completeBtn;
        public Button closeBtn;

        public List<int> questionItemIds = new List<int>();
        List<QuestionItem> questionItemList = new List<QuestionItem>();
        List<UIQuestionItem> UIQuestionItemList = new List<UIQuestionItem>();



        List<QuestionItem> QuestionItemDataList = new List<QuestionItem>();
        Dictionary<int, QuestionItem> QuestionItemDataDic = new Dictionary<int, QuestionItem>();

        private void Start()
        {
            ComponentEntry.OnInitFinish += delegate
            {
                QuestionItemDataList = ComponentEntry.Data.GetDataByTableName<List<QuestionItem>>("QuestionItem");
                foreach (var item in QuestionItemDataList)
                {
                    if (item != null)
                    {
                        QuestionItemDataDic.Add(item.id, item);
                    }
                }
                closeObj.Hide();
                questionItemList = GetQuestionItem(questionItemIds);
                Create();

            };
        }

        private void OnEnable()
        {





        }

        public void Create()
        {
            questionObj.Show();
            completeObj.Show();

            completeBtn.onClick.AddListener(ClickComplete);
            closeBtn.onClick.AddListener(ClickClose);


            closeObj.Hide();
            UIQuestionItemList.Clear();
            foreach (var item in questionItemList)
            {
                Transform clone = Instantiate(questionItem_clone, questionItem_Parent);
                clone.localScale = Vector3.one;
                clone.name = item.id + "";
                UIQuestionItem uIQuestionItem = clone.GetComponent<UIQuestionItem>();
                UIQuestionItemList.Add(uIQuestionItem);
                uIQuestionItem.Create(item);
            }
            canvasGroup.interactable = true;

            RefreshUI();
        }
        private void OnDisable()
        {
            foreach (Transform item in questionItem_Parent)
            {
                Destroy(item.gameObject);
            }
        }

        public void ClickComplete()
        {
            foreach (var item in UIQuestionItemList)
            {
                item.Complete();
            }
            int score = 0;
            foreach (var item in questionItemList)
            {
                score += item.score;
            }
            Debug.Log($"任务id:{Id} 得分:{score}");

            RefreshUI();
            completeBtn.onClick.RemoveAllListeners();

            if (IsHasExplain)
            {
                completeObj.Hide();
                closeObj.Show();
                canvasGroup.interactable = false;
            }
            else
            {
                ComponentEntry.Task.CurrentTask.NotifyResult(true, true);
                Hide();
            }




        }
        public void ClickClose()
        {
            closeBtn.onClick.RemoveAllListeners();


            completeObj.Show();
            closeObj.Hide();
            Hide();
        }
        public void RefreshUI()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)questionItem_Parent);
        }

        public QuestionItem GetQuestionItem(int num)
        {
            return QuestionItemDataDic[num];
        }
        public List<QuestionItem> GetQuestionItem(List<int> num)
        {
            List<QuestionItem> list = new List<QuestionItem>();
            foreach (var item in num)
            {
                QuestionItem tmp = GetQuestionItem(item);
                if (tmp != null)
                {
                    list.Add(tmp);
                }
            }
            return list;
        }
    }
}
