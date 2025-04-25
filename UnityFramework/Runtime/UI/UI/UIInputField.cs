/*
* FileName:          UIInputField
* CompanyName:  
* Author:            
* Description:       
* 
*/

using UnityEngine.UI;


namespace UnityFramework.UI
{
    public class UIInputField : UI<InputField>
    {
        public InputField Component
        {
            get
            {
                if (component == null)
                {
                    component = GetComponent<InputField>();
                }
                return component;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            component.onValueChanged.AddListener(ValueChangeEvent);
            component.onEndEdit.AddListener(EndEvent);
        }

        /// <summary>
        /// 获取输入内容
        /// </summary>
        /// <returns>返回在输入框所输入的内容</returns>
        public string GetInputValue()
        {
            return component.text;
        }

        /// <summary>
        /// 输入时执行事件
        /// </summary>
        /// <param name="_content"></param>
        protected virtual void ValueChangeEvent(string _content)
        {

        }

        /// <summary>
        /// 输入完成后执行事件
        /// </summary>
        /// <param name="_content"></param>
        protected virtual void EndEvent(string _content)
        {

        }

        /// <summary>
        /// 设置显示/隐藏明文还是*号
        /// </summary>
        /// <param name="boolean"></param>
        public void SetContentTypePassword(bool boolean)
        {
            component.contentType = boolean ? InputField.ContentType.Password : InputField.ContentType.Standard;
            component.Select();
        }
    }
}