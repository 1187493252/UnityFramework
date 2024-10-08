/*
* FileName:          LoopScrollViewItem
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoopScrollViewItem : MonoBehaviour
{
    private RectTransform rectTransform;
    private int index; //用于记录当前所在位置
    private Vector3 cacheScale; //开始移动时的大小
    private Vector3 cacheAnchorPosition3d; //开始移动时的坐标
    private Vector3 targetAnchorPostion3D; //目标坐标
    private int targetSiblingIndex; //目标层级
    private bool isMoving; //是否正在移动标识
    private float timer; //计时
    private bool last; //是否为最右侧的那张卡片

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime * 2f;
            timer = Mathf.Clamp01(timer);
            if (timer >= .2f)
            {
                transform.SetSiblingIndex(targetSiblingIndex);
            }
            rectTransform.anchoredPosition3D = Vector3.Lerp(cacheAnchorPosition3d, targetAnchorPostion3D, last ? 1f : timer);
            transform.localScale = Vector3.Lerp(cacheScale, (index == 0 ? 1.3f : 1f) * Vector3.one, last ? 1f : timer);
            if (timer == 1f)
            {
                isMoving = false;
            }
        }
    }

    public void Move(LoopScrollViewData data, bool last)
    {
        timer = 0f;
        targetAnchorPostion3D = data.AnchorPosition3D;
        targetSiblingIndex = data.SiblingIndex;
        cacheAnchorPosition3d = rectTransform.anchoredPosition3D;
        cacheScale = transform.localScale;
        isMoving = true;
        this.last = last;
    }
}
