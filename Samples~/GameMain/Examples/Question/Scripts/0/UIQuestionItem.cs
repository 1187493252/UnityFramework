/*
* FileName:          
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;

namespace PCI
{
    public class UIQuestionItem : MonoBehaviour
    {
        public Text title;
        public Text rightAnswer;

        public Text explain;

        public Transform options_Parent;
        public Transform option_clone;
        List<Toggle> toggles = new List<Toggle>();
        QuestionItem questionItem;



        private void Awake()
        {

        }
        public void RefreshUI()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
        }
        public void Create(QuestionItem item)
        {

            toggles.Clear();
            questionItem = item;
            questionItem.submitOptions = new List<int>();
            title.text = questionItem.title;
            explain.gameObject.Hide();

            rightAnswer.gameObject.Hide();


            if (!questionItem.explain.IsNullOrEmpty())
            {
                explain.text = questionItem.explain;
            }
            if (!questionItem.rightAnswer.IsNullOrEmpty())
            {
                rightAnswer.text = questionItem.rightAnswer;
            }


            for (int i = 0; i < questionItem.option.Count; i++)
            {
                int index = i;
                Transform option = Instantiate(option_clone, options_Parent);
                option.localScale = Vector3.one;

                option.name = index.ToString();
                option.GetComponentByChild<Text>("Label").text = questionItem.option[index];
                Toggle toggle = option.GetComponentInChildren<Toggle>();

                toggles.Add(toggle);

                toggle.onValueChanged.AddListener(delegate
                {
                    ToggleValueChange(toggle);
                });
            }

            gameObject.Show();
            RefreshUI();

        }
        void ToggleValueChange(Toggle toggle)
        {
            if (!questionItem.multiple)
            {
                if (toggle.isOn)
                {
                    foreach (var item in toggles)
                    {
                        if (item != toggle)
                        {
                            item.isOn = false;
                        }
                    }
                }
            }
        }
        public void Complete()
        {
            //记录答案
            questionItem.submitOptions.Clear();
            foreach (var item in toggles)
            {
                if (item.isOn)
                {
                    questionItem.submitOptions.Add(item.transform.name.ToInt());
                }
            }
            //计算分数
            if (questionItem.errorZero)
            {
                //错误直接0分,扣分制
                questionItem.score = questionItem.questionScore;
                if (questionItem.submitOptions.Count != questionItem.answer.Count)
                {
                    questionItem.score = 0;
                }
                else
                {
                    for (int i = 0; i < questionItem.submitOptions.Count; i++)
                    {
                        if (questionItem.submitOptions[i] != questionItem.answer[i])
                        {
                            questionItem.score = 0;
                            break;
                        }
                    }
                }
            }
            else
            {


                //答案个数不相等 采用普通加分制
                if (questionItem.submitOptions.Count != questionItem.answer.Count)
                {
                    for (int i = 0; i < questionItem.submitOptions.Count; i++)
                    {
                        int num = questionItem.submitOptions[i];
                        int optionScore = questionItem.optionsScore[num];
                        questionItem.score += optionScore;
                    }
                }
                else
                {
                    //个数相等 检查答案是否完全相等
                    bool isAllRight = true;
                    for (int i = 0; i < questionItem.submitOptions.Count; i++)
                    {
                        int num = questionItem.submitOptions[i];
                        int optionScore = questionItem.optionsScore[num];
                        questionItem.score += optionScore;
                        if (questionItem.submitOptions[i] != questionItem.answer[i])
                        {
                            isAllRight = false;
                        }
                    }
                    //普通情况,全对, 及阶段式 不全对 就是加对应选项的分数
                    //全对 并且是阶段式分数  分数为题目总分 (全对选项总分可能不等于题目总分)
                    if (isAllRight && questionItem.isStagewise)
                    {
                        questionItem.score = questionItem.questionScore;

                    }
                }


            }

            if (!questionItem.explain.IsNullOrEmpty())
            {
                explain.gameObject.Show();
            }
            if (!questionItem.rightAnswer.IsNullOrEmpty())
            {
                rightAnswer.gameObject.Show();
            }
            RefreshUI();
        }
    }
}
