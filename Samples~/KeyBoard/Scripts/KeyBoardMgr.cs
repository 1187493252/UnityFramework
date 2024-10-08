using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZRKFramework
{
    /// <summary>
    /// 虚拟键盘管理器
    /// </summary>
    public class KeyBoardMgr : MonoBehaviour
    {
        public delegate void OnKeyPadEvent();               //委托 按键事件
        public event OnKeyPadEvent OnSigned;                //事件-有符号
        public event OnKeyPadEvent OnUnsigned;              //事件-无符号
        public event OnKeyPadEvent OnUpper;                 //事件-大写
        public event OnKeyPadEvent OnLower;                 //事件-小写

        private bool signed;                                //是否有符号
        private bool upper;                                 //是否大写

        [Header("输入内容 文本框"),SerializeField]
        private Text m_Text_InputContent;

        private string inputContent = "";                   //输入内容

        [Header("字符串字符数量限制 默认30位"), SerializeField]
        private int characterLimit = 30;        

        /// <summary>
        /// 输入内容属性 对于外部访问 只能读不能写
        /// </summary>
        public string InputContent { get => inputContent; }
        
        /// <summary>
        /// 字符串字符数量 属性
        /// </summary>
        public int CharacterLimit { get => characterLimit; set => characterLimit = value; }

        private void OnEnable()
        {
            signed = false;
            upper = false;

            OnUnsigned?.Invoke();
        }

        /// <summary>
        /// 清空输入内容
        /// </summary>
        public void ClearInputContent() { inputContent = ""; m_Text_InputContent.text = inputContent; }

        /// <summary>
        /// 更新输入内容
        /// </summary>
        /// <param name="keyStr"></param>
        public void UpdateInputContent(string keyStr)
        {
            //限制输入字符数量为30个 超出字符 则丢前插后
            if (inputContent.Length <= CharacterLimit)
            {
                inputContent += keyStr;
                inputContent = inputContent.RegulationLimit(StringRegulation.NoRegulation);
                m_Text_InputContent.text = inputContent;
            }
            else
            {
                inputContent = (inputContent + keyStr).Substring(1, inputContent.Length - 1);
                inputContent = inputContent.RegulationLimit(StringRegulation.NoRegulation);
                m_Text_InputContent.text = inputContent;
            }
        }

        /// <summary>
        /// 事件-退格
        /// </summary>
        public void Event_BackSpace()
        {
            if (inputContent.Length >= 1)
            {
                inputContent = inputContent.Substring(0, inputContent.Length - 1);
                m_Text_InputContent.text = inputContent;
            }
        }

        /// <summary>
        /// 事件-符号
        /// </summary>
        public void Event_Symbol()
        {
            if (!signed) { OnSigned?.Invoke(); }
            else { OnUnsigned?.Invoke(); }

            signed = !signed;
            upper = false;
        }

        /// <summary>
        /// 事件-大小写
        /// </summary>
        public void Event_Case()
        {
            OnUnsigned?.Invoke();

            if (!upper) { OnUpper?.Invoke(); }
            else { OnLower?.Invoke(); }

            upper = !upper;
            signed = false;
        }

        /// <summary>
        /// 事件-输入完毕
        /// </summary>
        /// <param name="callBack">输入完毕后 回调</param>
        public void Event_Done(Action callBack = null)
        {
            gameObject.SetActive(false);
            callBack?.Invoke();
        }
    }
}
