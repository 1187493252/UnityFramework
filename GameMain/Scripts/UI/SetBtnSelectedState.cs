/*
* FileName:          SetBtnSelectedState
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class SetBtnSelectedState : MonoBehaviour
{
    [Header("父节点,不可空")]
    public Transform root;
    [Header("选中背景,可为空")]
    public GameObject bg;
    [Header("可为空,默认为按钮Image")]
    public Image image;

    [Header("普通状态图片,可为空,默认为image图片")]
    public Sprite normal;

    [Header("选中状态图片,可为空")]
    public Sprite selected;
    [Header("文字提示,可为空")]
    public Text text;
    [Header("普通状态文字颜色")]
    public Color normalColor = Color.black;
    [Header("选中状态文字颜色")]
    public Color selectedColor = Color.white;

    Button button;


    public bool IsStartSelected;
    public UnityEvent SelectedEvent;
    public UnityEvent UnSelectedEvent;


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
        button.onClick.AddListener(delegate
        {
            SetOthersNormalState(true);
            SetSelectedState(true);
        });

        if (IsStartSelected)
        {
            SetSelectedState();
        }
        else
        {
            SetNormalState();
        }
    }
    private void Start()
    {




    }
    public void SetNormalState(bool executeEvent = false)
    {
        if (bg)
        {
            bg.SetActive(false);
        }
        image.sprite = normal;

        if (text)
        {
            text.color = normalColor;
        }
        if (executeEvent)
        {
            UnSelectedEvent.Invoke();
        }
    }
    public void SetSelectedState(bool executeEvent = false)
    {


        if (bg)
        {
            bg.SetActive(true);
        }

        if (selected)
        {
            image.sprite = selected;
        }
        if (text)
        {
            text.color = selectedColor;
        }
        if (executeEvent)
        {
            SelectedEvent.Invoke();
        }

    }
    public void SetOthersNormalState(bool executeEvent = false)
    {
        if (root == null)
        {
            return;
        }
        SetBtnSelectedState[] tmp = root.GetComponentsInChildren<SetBtnSelectedState>();
        foreach (var item in tmp)
        {
            if (item != this)
            {
                item.SetNormalState(executeEvent);
            }
        }
    }
}
