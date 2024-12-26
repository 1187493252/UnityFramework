using UnityEngine;
using UnityEngine.UI;

namespace InnerKeyboard
{
    public class ComBtn : MonoBehaviour
    {
        [SerializeReference]
        [Header("是否是字母")]
        private bool isLetters;
        [Header("小写内容")]
        public string NorText;
        [Header("大写内容")]
        public string SfText;

        private Text Text, ShiftText;
        private Button Btn;
        private string Key, ShiftKey;

        //切换参数
        private Vector3 UpPos = new Vector3(-12, 24, 0);
        private Vector3 UpScale = new Vector3(0.5f, 0.5f, 1);

        public Color32 NorColor, SfColor;

        private void Awake()
        {
            Btn = this.transform.GetComponent<Button>();
            Text = transform.Find("Text").GetComponent<Text>();
            ShiftText = transform.Find("ShiftText").GetComponent<Text>();
        }

        void Start()
        {
            Btn.onClick.AddListener(KeyClick);
            if (!Keyboard.Instance.DynamicCreate)
            {
                SetKeyValue(NorText, SfText);
                Keyboard.Instance.OnShiftOn += OnShiftOn;
                Keyboard.Instance.OnShiftOff += OnShiftOff;
            }

        }

        //设置键值，普通和大写（Shift）值
        public void SetKeyValue(string Key, string ShiftKey)
        {
            this.Key = Key;
            this.ShiftKey = ShiftKey;
            Text.text = this.Key;
            ShiftText.text = this.ShiftKey;
        }

        //设置键值，普通和大写（Shift）值
        public void SetKeyValue(string Key, string ShiftKey, bool isletters)
        {
            this.Key = Key;
            this.ShiftKey = ShiftKey;
            Text.text = this.Key;
            ShiftText.text = this.ShiftKey;
            isLetters = isletters;
        }

        //切换至shift状态
        public void OnShiftOn()
        {
            Text.transform.localPosition = UpPos;
            Text.color = NorColor;
            Text.transform.localScale = UpScale;

            ShiftText.transform.localPosition = Vector3.zero;
            ShiftText.color = SfColor;
            ShiftText.transform.localScale = Vector3.one;
        }

        //关闭shift状态
        public void OnShiftOff()
        {

            ShiftText.transform.localPosition = UpPos;
            ShiftText.color = NorColor;
            ShiftText.transform.localScale = UpScale;

            Text.transform.localPosition = Vector3.zero;
            Text.color = SfColor;
            Text.transform.localScale = Vector3.one;
        }

        //按键事件
        public void KeyClick()
        {

            if (Keyboard.Instance != null)
            {
                if (!Keyboard.Instance.isChinese)
                {
                    if (Keyboard.Instance.isShift)
                        Keyboard.Instance.AddComBtnString(ShiftKey, isLetters);
                    else
                        Keyboard.Instance.AddComBtnString(Key, isLetters);
                }
                else
                {
                    Keyboard.Instance.AddComBtnString(Key, isLetters);
                }
            }
        }
    }
}