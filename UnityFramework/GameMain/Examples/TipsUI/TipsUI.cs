using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;

public class TipsUI : MonoBehaviour
{
    public Text Text;
    public Button Button;
    public Text BtnText;
    public void ShowTips(string content, Action action = null)
    {
        this.gameObject.SetActive(true);
        Text.text = content;
        Button.onClick.AddListener(delegate
        {
            action?.Invoke();
            Button.onClick.RemoveAllListeners();
            this.gameObject.SetActive(false);
        });
    }

    public void ShowTips(string content, string btnText, Action action = null)
    {
        this.gameObject.SetActive(true);
        Text.text = content;
        BtnText.text = btnText;
        Button.onClick.AddListener(delegate
        {
            action?.Invoke();
            Button.onClick.RemoveAllListeners();
            this.gameObject.SetActive(false);
        });
    }

    public void ShowTips(string content, string btnText, string audioIndex, Action action = null)
    {
        this.gameObject.SetActive(true);
        Text.text = content;
        BtnText.text = btnText;




        Button.onClick.AddListener(delegate
        {
            this.gameObject.SetActive(false);
            Button.onClick.RemoveAllListeners();

            action?.Invoke();
        });
    }
    private void OnDisable()
    {
        Button.onClick.RemoveAllListeners();
    }
}
