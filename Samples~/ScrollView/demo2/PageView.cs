using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect rect;                        //滑动组件  
    private float targethorizontal = 0;             //滑动的起始坐标  
    private bool isDrag = false;                    //是否拖拽结束  
    public List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始 0-1 
    int currentPageIndex = -1;
    public delegate int PageChange(int index);
    public PageChange OnPageChanged;
    private bool stopMove = true;
    public float smooting = 1;      //滑动速度  
    public float sensitivity = 0.3f;   //灵敏度
    private float startTime;

    private float startDragHorizontal;//记录当前开始滑动的位置（0-1）


    void Start()
    {
        rect = transform.GetComponent<ScrollRect>();
        //刷新Content 因为挂有ContentSizeFitter组件
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect.content);
        //Content水平宽度的大小减去视口大小
        float horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;

        posList.Add(0);
        //求出每页的临界值（0-1）
        for (int i = 1; i < rect.content.transform.childCount - 1; i++)
        {
            posList.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
        }
        posList.Add(1);



    }

    void Update()
    {
        FlipOver();

    }
    /// <summary>
    /// 移动到第几页
    /// </summary>
    /// <param name="index"></param>
    public void pageTo(int index)
    {
        if (index >= 0 && index < posList.Count)
        {
            rect.horizontalNormalizedPosition = posList[index];
            SetPageIndex(index);
        }
        else
        {
            Debug.LogWarning("页码不存在");
        }
    }
    //同步给其他脚本当前页码
    private void SetPageIndex(int index)
    {
        if (currentPageIndex != index)
        {
            currentPageIndex = index;
            if (OnPageChanged != null)
                OnPageChanged(index);
        }


    }
    /// <summary>
    /// 滑动翻页
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        startDragHorizontal = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //结束拖拽的位置（0-1）
        float posX = rect.horizontalNormalizedPosition;
        posX += ((posX - startDragHorizontal) * sensitivity);
        posX = posX < 1 ? posX : 1;
        posX = posX > 0 ? posX : 0;
        int index = 0;
        float offset = Mathf.Abs(posList[index] - posX);
        for (int i = 1; i < posList.Count; i++)
        {
            float temp = Mathf.Abs(posList[i] - posX);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }
        SetPageIndex(index);

        targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
        isDrag = false;
        startTime = 0;
        stopMove = false;
    }
    public void NextPage(int index)
    {

        int num = Mathf.Clamp(index + 1, 0, posList.Count - 1);
        targethorizontal = posList[num];
        SetPageIndex(num);
        isDrag = false;
        startTime = 0;
        stopMove = false;


    }
    public void LastPage(int index)
    {
        int num = Mathf.Clamp(index - 1, 0, posList.Count - 1);
        targethorizontal = posList[num];
        SetPageIndex(num);

        isDrag = false;
        startTime = 0;
        stopMove = false;

    }
    //翻页
    public void FlipOver()
    {
        if (!isDrag && !stopMove)
        {
            startTime += Time.deltaTime;
            float t = startTime * smooting;
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
            if (t >= 1)
                stopMove = true;


        }

    }

}
