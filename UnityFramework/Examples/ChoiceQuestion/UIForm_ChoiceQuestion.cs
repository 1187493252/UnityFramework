using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIForm_ChoiceQuestion : MonoBehaviour
{

    public PanelExam mPanelExam;
    public GameObject mDescRoot;
    public Button mConfirmBtn;

    internal int AllNum = 0;
    internal int errornum = 0;
    public Text mInfoText;

    public string url;
    public static string id;

    public event Action OnSetInfoText;
    protected void Awake()
    {

        mConfirmBtn.onClick.AddListener(delegate
        {
            this.OnConfirmBtnClicked();
        });

    }
    public void LoadQuestion()
    {
        LoadQuestion(url + id, LoadQuestionSuccess);

    }


    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
    public void ShowPanel()
    {
        gameObject.SetActive(true);
        mPanelExam.ShowPanel();

        mDescRoot.SetActive(false);
    }
    public void SetInfoText(string _msg, Action action = null)
    {
        mInfoText.text = _msg;
        mPanelExam.HidePanel();
        mDescRoot.SetActive(true);

        mConfirmBtn.onClick.AddListener(delegate
        {
            action?.Invoke();
            mConfirmBtn.onClick.RemoveAllListeners();
            this.gameObject.SetActive(false);
            OnSetInfoText?.Invoke();
        });

    }

    public void OnConfirmBtnClicked()
    {
        HidePanel();
    }

    public void LoadQuestion(string url, Action<string> _successCallBack = null
        , Action _failCallBack = null)
    {
        //	WebRequestHelper.RequestPost(url, "post", InfoBase.tokenName, InfoBase.tokenValue, receiveContent => { _successCallBack(receiveContent); }, _failCallBack);
    }
    public void LoadQuestionSuccess(string content)
    {
        JsonData jsondata = JsonMapper.ToObject(content);
        List<QuestionData> QuestionDatalist = JsonMapper.ToObject<List<QuestionData>>(jsondata["data"].ToJson());

        Debug.LogError($"选择题加载成功,题数:{QuestionDatalist.Count}");

        mPanelExam.InitUI(QuestionDatalist);
        ShowPanel();

    }
}
public class QuestionData
{
    public int id;
    public string questionContent;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;
    public string[] optionList;
    public string[] answerList;

}
public static class InfoBase
{
    /// <summary>
    /// 登录接口
    /// </summary>
    public const string loginInterface = "http://121.43.188.245:9999/user/login";
    /// <summary>
    /// 修改密码接口
    /// </summary>
    public const string changePasswordInterface = "http://121.43.188.245:9999/user/studentUpdatePass";
    /// <summary>
    /// 选择题接口
    /// </summary>
    public const string ChoiceQuestionInterface = "http://121.43.188.245:9999/user/login";
    /// <summary>
    /// 考核结果接口
    /// </summary>
    public const string CheckRejiesultInterface = "http://121.43.188.245:9999/user/login";
    public static string tokenName = "";

    public static string tokenValue = "";
    public static int studentId = 0;


    public static string satokenValue = "";

}