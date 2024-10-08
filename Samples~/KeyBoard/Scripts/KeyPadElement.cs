using UnityEngine;
using UnityEngine.UI;

namespace ZRKFramework
{
    /// <summary>
    /// ¼üÅÌÔªËØ
    /// </summary>
    public class KeyPadElement : MonoBehaviour
    {
        [Header("ÊÇ·ñ·ûºÅÇÐ»»"), SerializeField]
        private bool symbolSwitching = true;

        [Header("ÊÇ·ñÎª°¢À­²®Êý×Ö"), SerializeField]
        private bool number;

        [Header("Ìæ´ú·ûºÅ"),SerializeField]
        private string symbolStr;

        private string originKeyContent;    //Ô´ ¼üÄÚÈÝ

        private Text m_Text_Key;            //¼ü ÎÄ±¾¿ò
        private Button m_Button_Key;        //¼ü °´Å¥

        private KeyBoardMgr keyBoardMgr;    //ÐéÄâ¼üÅÌ¹ÜÀíÆ÷

        private void Awake()
        {
            m_Text_Key = GetComponentInChildren<Text>();
            m_Button_Key = GetComponent<Button>();
            keyBoardMgr = GetComponentInParent<KeyBoardMgr>();

            originKeyContent = m_Text_Key.text;
        }

        private void OnEnable()
        {
            m_Text_Key.text = originKeyContent;
        }

        private void Start()
        {
            m_Button_Key.onClick.AddListener(() =>
            {
                keyBoardMgr.UpdateInputContent(m_Text_Key.text);
            });

            keyBoardMgr.OnSigned += KeyBoardMgr_OnSigned;
            keyBoardMgr.OnUnsigned += KeyBoardMgr_OnUnsigned;
            keyBoardMgr.OnUpper += KeyBoardMgr_OnUpper;
            keyBoardMgr.OnLower += KeyBoardMgr_OnLower;
        }

        private void OnDestroy()
        {
            keyBoardMgr.OnSigned -= KeyBoardMgr_OnSigned;
            keyBoardMgr.OnUnsigned -= KeyBoardMgr_OnUnsigned;
            keyBoardMgr.OnUpper -= KeyBoardMgr_OnUpper;
            keyBoardMgr.OnLower -= KeyBoardMgr_OnLower;
        }

        private void KeyBoardMgr_OnSigned()
        {
            if (!symbolSwitching) return;
            m_Text_Key.text = symbolStr;
        }

        private void KeyBoardMgr_OnUnsigned()
        {
            if (!symbolSwitching) return;
            m_Text_Key.text = originKeyContent;
        }

        private void KeyBoardMgr_OnUpper()
        {
            char letter = char.Parse(originKeyContent);
            int letterNumber = letter;
            if ((65 <= letterNumber && letterNumber <= 90) || (97 <= letterNumber && letterNumber <= 122))
            {
                m_Text_Key.text = originKeyContent.ToUpper();
            }
        }

        private void KeyBoardMgr_OnLower()
        {
            char letter = char.Parse(originKeyContent);
            int letterNumber = letter;
            if ((65 <= letterNumber && letterNumber <= 90) || (97 <= letterNumber && letterNumber <= 122))
            {
                m_Text_Key.text = originKeyContent.ToLower();
            }
        }
    }
}
