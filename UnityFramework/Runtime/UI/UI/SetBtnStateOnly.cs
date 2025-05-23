﻿/*
* FileName:          SetBtnStateOnly
* CompanyName:       
* Author:            relly
* Description:       按钮的选中状态设置,多个按钮只能有一个选中
*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SetBtnStateOnly : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("父节点,不可空")]
    public Transform root;
    [Header("普通状态背景,可为空")]
    public GameObject normalbg;
    [Header("选中状态背景,可为空")]
    public GameObject selectedbg;
    [Header("可为空,默认为按钮Image")]
    public Image image;

    [Header("普通状态图片,可为空,默认为image图片")]
    public Sprite normal;

    [Header("选中状态图片,可为空")]
    public Sprite selected;
    [Header("文字提示,可为空")]
    public Text text;
    [Header("普通状态文字")]
    public string normalContent;
    [Header("选中状态文字")]
    public string selectedContent;
    [Header("普通状态文字颜色")]
    public Color normalColor = Color.black;
    [Header("选中状态文字颜色")]
    public Color selectedColor = Color.white;

    Button button;
    public bool IsOpenHover;


    public bool IsStartSelected;
    public bool IsStartSelectedEvent;

    public UnityEvent SelectedEvent;
    public UnityEvent UnSelectedEvent;

    bool isSelected;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        if (normal == null)
        {
            normal = image.sprite;
        }
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(delegate {
            SetOthersNormalState(true);
            SetSelectedState(true);
        });

        if (IsStartSelected)
        {
            SetSelectedState(IsStartSelectedEvent);
        }
        else
        {
            SetNormalState();
        }
    }
    private void Start()
    {




    }
    public void SetNormalState(bool executeEvent = false, bool setIsSelectedValue = true)
    {
        if (normalbg)
        {
            normalbg.SetActive(true);
        }
        if (selectedbg)
        {
            selectedbg.SetActive(false);
        }
        image.sprite = normal;

        if (text)
        {
            text.color = normalColor;
            if (normalContent != "")
            {
                text.text = normalContent;
            }
        }
        if (executeEvent)
        {
            UnSelectedEvent.Invoke();
        }
        if (setIsSelectedValue)
        {
            isSelected = false;
        }

    }
    public void SetSelectedState(bool executeEvent = false, bool setIsSelectedValue = true)
    {
        if (normalbg)
        {
            normalbg.SetActive(false);
        }

        if (selectedbg)
        {
            selectedbg.SetActive(true);
        }

        if (selected)
        {
            image.sprite = selected;
        }
        if (text)
        {
            text.color = selectedColor;
            if (selectedContent != "")
            {
                text.text = selectedContent;
            }
        }
        if (executeEvent)
        {
            SelectedEvent.Invoke();
        }
        if (setIsSelectedValue)
        {
            isSelected = true;
        }

    }
    public void SetOthersNormalState(bool executeEvent = false)
    {
        if (root == null)
        {
            return;
        }
        SetBtnStateOnly[] tmp = root.GetComponentsInChildren<SetBtnStateOnly>();
        foreach (var item in tmp)
        {
            if (item != this)
            {
                item.SetNormalState(executeEvent);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsOpenHover)
        {
            if (!isSelected)
            {
                SetSelectedState(false, false);

            }
            //  SetOthersNormalState();
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsOpenHover)
        {
            if (!isSelected)
            {
                SetNormalState(false, false);


            }
        }
    }
}
