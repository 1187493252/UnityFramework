using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework.Runtime;

public class UIHomeWindow : UIFormLogic
{
    private Text _version;

    private void Awake()
    {
        _version = this.transform.Find("version").GetComponent<Text>();


        var loginBtn = this.transform.Find("Start").GetComponent<Button>();
        loginBtn.onClick.AddListener(OnClickLoginBtn);


    }
    private void Start()
    {
        var package = YooAsset.YooAssets.GetPackage("DefaultPackage");
        _version.text = "Ver : " + package.GetPackageVersion();
    }

    private void OnClickLoginBtn()
    {
        SceneEventDefine.ChangeToBattleScene.SendEventMessage();
        ComponentEntry.UI.CloseUIForm(this.UIForm);

    }

}