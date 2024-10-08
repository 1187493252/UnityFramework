using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZRKFramework
{
    /// <summary>
    /// ������̹�����
    /// </summary>
    public class KeyBoardMgr : MonoBehaviour
    {
        public delegate void OnKeyPadEvent();               //ί�� �����¼�
        public event OnKeyPadEvent OnSigned;                //�¼�-�з���
        public event OnKeyPadEvent OnUnsigned;              //�¼�-�޷���
        public event OnKeyPadEvent OnUpper;                 //�¼�-��д
        public event OnKeyPadEvent OnLower;                 //�¼�-Сд

        private bool signed;                                //�Ƿ��з���
        private bool upper;                                 //�Ƿ��д

        [Header("�������� �ı���"),SerializeField]
        private Text m_Text_InputContent;

        private string inputContent = "";                   //��������

        [Header("�ַ����ַ��������� Ĭ��30λ"), SerializeField]
        private int characterLimit = 30;        

        /// <summary>
        /// ������������ �����ⲿ���� ֻ�ܶ�����д
        /// </summary>
        public string InputContent { get => inputContent; }
        
        /// <summary>
        /// �ַ����ַ����� ����
        /// </summary>
        public int CharacterLimit { get => characterLimit; set => characterLimit = value; }

        private void OnEnable()
        {
            signed = false;
            upper = false;

            OnUnsigned?.Invoke();
        }

        /// <summary>
        /// �����������
        /// </summary>
        public void ClearInputContent() { inputContent = ""; m_Text_InputContent.text = inputContent; }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="keyStr"></param>
        public void UpdateInputContent(string keyStr)
        {
            //���������ַ�����Ϊ30�� �����ַ� ��ǰ���
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
        /// �¼�-�˸�
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
        /// �¼�-����
        /// </summary>
        public void Event_Symbol()
        {
            if (!signed) { OnSigned?.Invoke(); }
            else { OnUnsigned?.Invoke(); }

            signed = !signed;
            upper = false;
        }

        /// <summary>
        /// �¼�-��Сд
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
        /// �¼�-�������
        /// </summary>
        /// <param name="callBack">������Ϻ� �ص�</param>
        public void Event_Done(Action callBack = null)
        {
            gameObject.SetActive(false);
            callBack?.Invoke();
        }
    }
}
