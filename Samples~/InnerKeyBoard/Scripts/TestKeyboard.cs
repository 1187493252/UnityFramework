using InnerKeyboard;
using UnityEngine;
using UnityEngine.UI;

public class TestKeyboard : MonoBehaviour
{

    private Text EditText;
    private Button TextBtn;



    private Text NowEditText;
    private void Awake()
    {
        EditText = transform.Find("Image/Text").GetComponent<Text>();
        TextBtn = EditText.GetComponent<Button>();


    }

    // Start is called before the first frame update
    void Start()
    {
        TextBtn.onClick.AddListener(() => ClickText(EditText));
    }
    void ClickText(Text sender)
    {
        NowEditText = sender;
        if (Keyboard.Instance)
            Keyboard.Instance.ShowKeyboard(sender, EditCallBack);
    }

    void EditCallBack(KeyboardParam kbpara)
    {
        Debug.Log(kbpara.OutputStr);
    }
}