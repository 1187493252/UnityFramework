/*
* FileName:          UIForm_Tip
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIForm_Tip : MonoBehaviour
{
    public Text textContent;
    public Button btn_Confirm;
    public Button btn_Cancel;

    private void Awake()
    {
        Hide();
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Show(string content = "", Action confirmAction = null, Action cancelAction = null)
    {
        textContent.gameObject.SetActive(content != "" ? true : false);
        btn_Confirm.gameObject.SetActive(confirmAction != null ? true : false);
        btn_Cancel.gameObject.SetActive(cancelAction != null ? true : false);
        textContent.text = content;
        btn_Confirm.onClick.RemoveAllListeners();
        btn_Cancel.onClick.RemoveAllListeners();
        if (confirmAction != null)
        {
            btn_Confirm.onClick.AddListener(() =>
            {
                confirmAction();
                Hide();
            });
        }
        if (cancelAction != null)
        {
            btn_Cancel.onClick.AddListener(() =>
            {
                cancelAction();
                Hide();
            });
        }
        gameObject.SetActive(true);

    }
    public void Hide()
    {
        gameObject.SetActive(false);

    }
}
