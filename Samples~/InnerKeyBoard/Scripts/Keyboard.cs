using hyjiacan.py4n;
using input.core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InnerKeyboard
{

    //键盘参数类
    public class KeyboardParam
    {
        public string InputStr;
        public string OutputStr;

        public KeyboardParam(string InStr, string OutStr = "")
        {
            InputStr = InStr;
            OutputStr = OutStr;
        }

        public void Clear()
        {
            InputStr = "";
            OutputStr = "";
        }
    }

    //委托事件类
    public class EventCommon
    {

        public delegate void CallBack<T>(T para);

        public delegate void NorEvent();
    }

    public class Keyboard : MonoBehaviour
    {
        private RectTransform KeyboardWindow;
        private GameObject ComBtnPref;
        private Transform Line0, Line1, Line2, Line3;
        private Button BackSpaceBtn, ShiftBtn, SpaceBtn, CancelBtn, EnterBtn, LangugeBtn, ClearBtn;
        private Image ShiftBG;

        //-------------
        private RectTransform kb_bg;
        private Transform output_parent;
        private Transform output;
        /// <summary>
        /// 键盘拼音输出栏
        /// </summary>
        private Text output_text;
        private Image langugeBG;
        private Text languge_text;

        private string[][] Line0_KeyValue = {
            new string[]{"`","1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "="},
            new string[]{"·", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" },
            new string[]{"false", "false","false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false" }

        };

        private string[][] Line1_KeyValue = {
            new string[]{"q","w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\"},
            new string[]{"Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|" },
             new string[]{"true", "true", "true", "true", "true", "true", "true", "true", "true", "true", "true", "false", "false" }
        };

        private string[][] Line2_KeyValue = {
            new string[]{"a","s", "d", "f", "g", "h", "j", "k", "l", ";", "'"},
            new string[]{"A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\""},
             new string[]{ "true", "true", "true", "true", "true", "true", "true", "true", "true", "false", "false" }
        };

        private string[][] Line3_KeyValue = {
            new string[]{"z","x", "c", "v", "b", "n", "m", ",", ".", "/"},
            new string[]{"Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"},
             new string[]{ "true", "true", "true", "true", "true", "true", "true", "false", "false", "false" }
        };

        /// <summary>
        /// 当前输入的内容
        /// </summary>
        private string NewEditeString = "";

        [HideInInspector]
        public bool isShift = false;
        private bool isShiftLock = false;
        private float ShiftTime = 0f;
        private Color32 LockColor = new Color32(127, 171, 179, 255);
        private Color32 NormalColor = new Color(128, 128, 128, 255);

        [HideInInspector]
        public bool isChinese = false;


        public event EventCommon.NorEvent OnShiftOn = null;
        public event EventCommon.NorEvent OnShiftOff = null;

        private KeyboardParam KeyboardPara = null;//键盘参数
        engine engine1;


        private EventCommon.CallBack<KeyboardParam> call = null; //回调函数

        private static Keyboard instance = null;

        public static Keyboard Instance
        {
            get { return instance; }
        }
        //-----------
        /// <summary>
        /// 当前的text文本
        /// </summary>
        Text current_text;
        int subnum = 20;//截取固定个数汉子

        [Header("动态创建:需要使用不同的预制")]
        public bool DynamicCreate;
        //Awake
        private void Awake()
        {
            instance = this;
            KeyboardPara = new KeyboardParam("");
            engine1 = new engine();
            KeyboardWindow = this.transform.GetComponent<RectTransform>();
            ComBtnPref = Resources.Load<GameObject>("KeyItem");
            Line0 = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine0");
            Line1 = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine1");
            Line2 = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine2");
            Line3 = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine3/ComBtnLine3");

            BackSpaceBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine3/BackSpaceBtn").GetComponent<Button>();
            ShiftBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine3/ShiftBtn").GetComponent<Button>();
            SpaceBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine4/SpaceBtn").GetComponent<Button>();
            CancelBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine4/CancelBtn").GetComponent<Button>();
            EnterBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine4/EnterBtn").GetComponent<Button>();
            LangugeBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine4/LangugeBtn").GetComponent<Button>();
            ClearBtn = KeyboardWindow.Find("KB_BG/KeyBtns/ComBtnLine4/ClearBtn").GetComponent<Button>();

            ShiftBG = ShiftBtn.GetComponent<Image>();
            langugeBG = LangugeBtn.GetComponent<Image>();
            languge_text = LangugeBtn.GetComponentInChildren<Text>();
            //------------
            kb_bg = KeyboardWindow.Find("KB_BG").GetComponent<RectTransform>();
            output_parent = kb_bg.Find("Output/Scroll View/Viewport/Content");
            output = output_parent.Find("Output");
            output_text = output.GetComponentInChildren<Text>();
        }

        void Start()
        {
            if (DynamicCreate)
            {
                InitComBtn();
            }
            HideKeyboard();
            BackSpaceBtn.onClick.AddListener(ClickBackSpace);
            ShiftBtn.onClick.AddListener(ClickShift);
            SpaceBtn.onClick.AddListener(ClickSpace);
            CancelBtn.onClick.AddListener(ClickCancel);
            EnterBtn.onClick.AddListener(ClickEnter);
            LangugeBtn.onClick.AddListener(ClickLanguage);
            ClearBtn.onClick.AddListener(ClickClear);

        }


        //初始化按钮
        private void InitComBtn()
        {
            InstantLineComBtns(Line0, Line0_KeyValue);
            InstantLineComBtns(Line1, Line1_KeyValue);
            InstantLineComBtns(Line2, Line2_KeyValue);
            InstantLineComBtns(Line3, Line3_KeyValue);
            LayoutRebuilder.ForceRebuildLayoutImmediate(kb_bg);
        }

        //按行实例化按钮
        private void InstantLineComBtns(Transform LineTran, string[][] KeyValues)
        {
            for (int i = 0; i < KeyValues[0].Length; i++)
            {
                GameObject TempObj = GameObject.Instantiate<GameObject>(ComBtnPref);
                TempObj.transform.SetParent(LineTran);
                TempObj.transform.localScale = Vector3.one;
                ComBtn comBtnCtrl = TempObj.GetComponent<ComBtn>();
                TempObj.name = KeyValues[0][i];
                if (comBtnCtrl != null)
                {
                    bool boolean = bool.Parse(KeyValues[2][i]);
                    comBtnCtrl.SetKeyValue(KeyValues[0][i], KeyValues[1][i], boolean);
                    OnShiftOn += comBtnCtrl.OnShiftOn;
                    OnShiftOff += comBtnCtrl.OnShiftOff;
                }
            }
        }

        /// <summary>
        /// 输入内容追加
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="isLetters">是否是字母</param>
        public void AddComBtnString(string str, bool isLetters)
        {


            if (isChinese)
            {
                AddComBtnStringCN(str, isLetters);
            }
            else
            {
                AddComBtnStringEn(str, isLetters);
            }
        }

        void AddComBtnStringEn(string str, bool isLetters)
        {

            NewEditeString += str;
            if (KeyboardPara != null)
            {
                KeyboardPara.OutputStr = NewEditeString;
            }
            current_text.text = KeyboardPara.OutputStr;
            call?.Invoke(KeyboardPara);
            if (isShift && !isShiftLock)
            {
                isShift = false;
                ShiftBG.color = NormalColor;
                OnShiftOff();
            }
        }

        void AddComBtnStringCN(string str, bool isLetters)
        {
            if (isLetters)
            {
                output_text.text += str;
                Search(output_text.text);
            }
            else
            {
                output_text.text += str;
                NewEditeString += output_text.text;
                if (KeyboardPara != null)
                {
                    KeyboardPara.OutputStr = NewEditeString;
                }
                current_text.text = KeyboardPara.OutputStr;
                call?.Invoke(KeyboardPara);
                ClearOuput();
            }

        }

        void Search(string str)
        {
            ClearOuput(false);

            string tmp;
            engine1.search(str, out tmp);
            if (string.IsNullOrEmpty(tmp))
            {
                return;
            }
            string[] hanzi = tmp.Split(' ');
            List<string> content = hanzi.ToList();
            //截取固定个数汉子
            if (hanzi.Length > subnum)
            {
                content = content.Take(subnum).ToList();
            }
            foreach (var item in content)
            {
                Transform tran = Instantiate(output, output_parent);
                tran.GetComponentInChildren<Text>().text = item;
                Button button = tran.GetComponent<Button>();
                button.onClick.AddListener(delegate
                {
                    NewEditeString += item;
                    if (KeyboardPara != null)
                    {
                        KeyboardPara.OutputStr = NewEditeString;
                    }
                    current_text.text = KeyboardPara.OutputStr;
                    call?.Invoke(KeyboardPara);
                    ClearOuput();
                });
            }
        }


        // 唤起键盘
        public void ShowKeyboard(Text para, EventCommon.CallBack<KeyboardParam> call)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(kb_bg);
            KeyboardPara.InputStr = para.text;
            current_text = para;
            NewEditeString = KeyboardPara.InputStr;

            KeyboardWindow.localScale = Vector3.one;
            if (call != null)
                this.call = call;
        }

        //隐藏键盘
        public void HideKeyboard()
        {
            ClearOuput();
            current_text = null;
            KeyboardPara.Clear();
            KeyboardWindow.localScale = Vector3.zero;
        }


        //语言点击事件
        private void ClickLanguage()
        {
            if (isChinese)
            {
                isChinese = false;
                langugeBG.color = NormalColor;
                languge_text.text = "英文";
                ShiftBtn.interactable = true;
            }
            else
            {
                isChinese = true;
                langugeBG.color = LockColor;
                languge_text.text = "中文";

                ShiftBtn.interactable = false;
                isShift = false;
                isShiftLock = false;
                ShiftBG.color = NormalColor;
                OnShiftOff();
            }
        }

        //取消点击事件
        private void ClickCancel()
        {
            NewEditeString = "";
            if (KeyboardPara != null)
                KeyboardPara.OutputStr = KeyboardPara.InputStr;

            current_text.text = KeyboardPara.OutputStr;

            call?.Invoke(KeyboardPara);
            HideKeyboard();
        }

        //确认点击事件
        private void ClickEnter()
        {
            if (KeyboardPara != null)
                KeyboardPara.OutputStr = NewEditeString;
            current_text.text = KeyboardPara.OutputStr;

            call?.Invoke(KeyboardPara);
            HideKeyboard();
        }

        //shift点击事件
        private void ClickShift()
        {
            if (Time.time - ShiftTime <= 0.5f)
            {
                ShiftTime = Time.time;
                isShift = true;
                isShiftLock = true;
                ShiftBG.color = LockColor;
                OnShiftOn();
            }
            else
            {
                if (isShift)
                {
                    ShiftTime = Time.time;
                    isShift = false;
                    isShiftLock = false;
                    ShiftBG.color = NormalColor;
                    OnShiftOff();
                }
                else
                {
                    ShiftTime = Time.time;
                    isShift = true;
                    isShiftLock = false;
                    OnShiftOn();
                }
            }
        }

        //空格点击事件
        private void ClickSpace()
        {
            NewEditeString += " ";
            if (KeyboardPara != null)
                KeyboardPara.OutputStr = NewEditeString;
            current_text.text = KeyboardPara.OutputStr;

            call?.Invoke(KeyboardPara);
        }

        //清空点击事件
        private void ClickClear()
        {

            ClearOuput();
            NewEditeString = "";
            if (KeyboardPara != null)
                KeyboardPara.OutputStr = NewEditeString;
            current_text.text = KeyboardPara.OutputStr;

            call?.Invoke(KeyboardPara);
        }

        //回退点击事件
        private void ClickBackSpace()
        {
            if (!isChinese)
            {
                if (!string.IsNullOrEmpty(NewEditeString))
                {
                    NewEditeString = NewEditeString.Substring(0, NewEditeString.Length - 1);

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(output_text.text))
                {
                    output_text.text = output_text.text.Substring(0, output_text.text.Length - 1);
                    Search(output_text.text);

                }
                else
                {
                    if (!string.IsNullOrEmpty(NewEditeString))
                    {
                        NewEditeString = NewEditeString.Substring(0, NewEditeString.Length - 1);
                    }
                }
            }
            if (KeyboardPara != null)
                KeyboardPara.OutputStr = NewEditeString;
            current_text.text = KeyboardPara.OutputStr;
            call?.Invoke(KeyboardPara);

        }

        /// <summary>
        /// 清空输出栏中文字
        /// </summary>
        /// <param name="clearoutputText">是否清除输出栏拼音</param>
        private void ClearOuput(bool clearoutputText = true)
        {
            if (clearoutputText)
            {
                output_text.text = "";
            }
            foreach (Transform item in output_parent)
            {
                if (item != output)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
