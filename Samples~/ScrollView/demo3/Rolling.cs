/*
* FileName:          Rolling
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Rolling : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool SlideSwitch = true;//滑动开关
    public float SlideOffset = 270;//滑动触发值
    public bool ScaleSwitch = true;//缩放开关
    public bool AlphaSwitch = true;//透明度开关
    public bool ShowFirstBorder = true;//最前面的选项是否显示边框
    public GameObject OptionPrefab;//预制体
    public Transform OptionGroup;//父物体
    Transform[] options;//选项
    public int OptionNum = 6;//选项总数
    float halfOptionNum;//总数一半
    public float Radius = 500;//旋转半径
    public float Speed = 4;//旋转速度
    public float OffsetY = 130;//y偏移  
    [Range(0, 1)]
    public float MiniAlpha;//最小透明度
    [Range(1, 5)]
    public float MaxScale;//最大缩放,最前面的选项
    [Range(0, 1)]
    public float MinScale;//最小缩放
    [Range(0, 5)]
    public float SmoothTime;//缩放平滑时间

    Vector3 last;//记录鼠标位置

    Vector3 offset;//鼠标滑动差值

    //-----------
    Coroutine currentCoroutine;
    public Button LeftBtn, RightBtn;
    public Sprite[] Sprites;
    GameObject[] borders;
    private void Awake()
    {


        for (int i = 0; i < OptionNum; i++)
        {
            GameObject clone = Instantiate(OptionPrefab, Vector3.zero, Quaternion.identity, OptionGroup);
            clone.name = i + "";
            Button btn = clone.GetComponent<Button>();
            //替换图片
            try
            {
                if (Sprites != null && Sprites[i] != null)
                {
                    btn.image.sprite = Sprites[i];
                }
            }
            catch (Exception)
            {


            }

            btn.onClick.AddListener(delegate
            {
                ////绑定选项点击事件
                //if (clone.transform.GetSiblingIndex() != OptionNum - 1)
                //{
                //    return;
                //}

                Debug.Log($"点击了{clone.name}");
            });
        }
        halfOptionNum = OptionNum / 2;
        options = new Transform[OptionNum];
        borders = new GameObject[OptionNum];

        for (int i = 0; i < OptionNum; i++)
        {
            options[i] = OptionGroup.GetChild(i); ;
            borders[i] = options[i].Find("border").gameObject;
            SetBorder(i, false);
        }
        InitPos();
        InitSibling();
        SetFirstBorder(true);
        SetRaycasts();
        if (AlphaSwitch)
        {
            SetAlpha();
        }
        if (ScaleSwitch)
        {
            SetScale(0.05f);
        }

    }
    private void Start()
    {
        LeftBtn.onClick.AddListener(ClickLeft);
        RightBtn.onClick.AddListener(ClickRight);
    }

    void SetBorder(int index, bool isShow)
    {
        if (borders[index] != null)
        {
            borders[index].SetActive(isShow);
        }
    }
    /// <summary>
    /// 设置边框
    /// </summary>
    /// <param name="isShow"></param>
    void SetBorder(bool isShow)
    {
        foreach (var item in borders)
        {
            if (item != null)
            {
                item.SetActive(isShow);
            }
        }

    }
    void SetRaycasts()
    {
        //不用Interactable是因为颜色会变灰 加上alpha设置会看不清
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetSiblingIndex() == options.Length - 1)
            {
                options[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                options[i].GetComponent<CanvasGroup>().blocksRaycasts = false;

            }
        }
    }
    void SetFirstBorder(bool isShow)
    {
        if (ShowFirstBorder)
        {
            SetBorder(GetFirst(), isShow);
        }
    }
    /// <summary>
    /// 根据Z设置透明度
    /// </summary>
    void SetAlpha()
    {

        //最前面选项的z
        float startz = -Radius;
        foreach (var item in options)
        {
            float alpha = 1 - Mathf.Abs(item.localPosition.z - startz) / (2 * Radius) * (1 - MiniAlpha);
            CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
            canvasGroup.alpha = alpha;
        }
    }
    void ResetAlpha()
    {
        foreach (var item in options)
        {
            CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
        }
    }

    int GetFirst()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetSiblingIndex() == options.Length - 1)
            {
                return i;
            }
        }
        return -1;
    }
    void ResetScale()
    {
        foreach (var item in options)
        {
            item.localScale = Vector3.one;
        }
    }
    /// <summary>
    /// 设置缩放,越靠前越大越靠后越小
    /// </summary>
    /// <param name="smoothTime"></param>
    void SetScale(float smoothTime = 0.1f)
    {

        //最前面选项的z
        float startz = -Radius;

        foreach (var item in options)
        {
            if (item.GetSiblingIndex() == options.Length - 1)
            {
                StartCoroutine(ChangeScale(item, MaxScale, smoothTime));
            }
            else
            {
                float scale = 1 - Mathf.Abs(item.localPosition.z - startz) / (2 * Radius) * (1 - MinScale);
                item.localScale = Vector3.one * scale;
                StartCoroutine(ChangeScale(item, scale, smoothTime));
            }
        }
    }
    /// <summary>
    /// 初始化顺序
    /// </summary>
    void InitSibling()
    {
        for (int i = 0; i < OptionNum; i++)
        {
            //未过半
            if (i <= halfOptionNum)
            {
                if (OptionNum % 2 == 0)
                {
                    options[i].SetSiblingIndex((int)halfOptionNum - i);
                }
                else
                {
                    options[i].SetSiblingIndex((OptionNum - 1) / 2 - i);
                }
            }
            else
            {
                options[i].SetSiblingIndex(options[OptionNum - i].GetSiblingIndex());

            }
        }

    }
    /// <summary>
    /// 初始化位置
    /// </summary>
    void InitPos()
    {
        for (int i = 0; i < OptionNum; i++)
        {
            float angle = (360.0f / OptionNum) * i * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * Radius;
            float z = -Mathf.Cos(angle) * Radius;
            float y = 0;
            if (i != 0)
            {
                if (i > halfOptionNum)
                {
                    y = (OptionNum - i) * OffsetY;
                }
                else
                {
                    y = i * OffsetY;
                }
            }
            options[i].localPosition = new Vector3(x, y, z);
        }

    }
    IEnumerator MoveLeft()
    {

        if (currentCoroutine != null)
        {
            yield return currentCoroutine;
        }
        Vector3 pos;
        int index;
        Dictionary<Transform, Vector3> optionsPosDic1 = new Dictionary<Transform, Vector3>();
        Dictionary<Transform, int> optionsIndexDic1 = new Dictionary<Transform, int>();
        foreach (var item in options)
        {
            //记录当前的位置
            optionsPosDic1.Add(item, item.localPosition);

            optionsIndexDic1.Add(item, item.GetSiblingIndex());
        }
        for (int i = OptionNum - 1; i >= 0; i--)
        {
            if (i == 0)
            {

                pos = optionsPosDic1[options[OptionNum - 1]];
                index = optionsIndexDic1[options[OptionNum - 1]];
            }
            else
            {
                pos = optionsPosDic1[options[i - 1]];
                index = optionsIndexDic1[options[i - 1]];

            }
            options[i].SetSiblingIndex(index);
            currentCoroutine = StartCoroutine(MoveToTarget(options[i], pos));
        }
        yield return null;


    }
    IEnumerator MoveRight()
    {
        if (currentCoroutine != null)
        {
            yield return currentCoroutine;
        }
        Vector3 pos;
        int index;

        Dictionary<Transform, Vector3> optionsPosDic1 = new Dictionary<Transform, Vector3>();
        Dictionary<Transform, int> optionsIndexDic1 = new Dictionary<Transform, int>();
        foreach (var item in options)
        {
            //记录当前的位置

            optionsPosDic1.Add(item, item.localPosition);

            optionsIndexDic1.Add(item, item.GetSiblingIndex());
        }
        for (int i = 0; i < OptionNum; i++)
        {
            if (i == OptionNum - 1)
            {
                pos = optionsPosDic1[options[0]];
                index = optionsIndexDic1[options[0]];
            }
            else
            {
                pos = optionsPosDic1[options[i + 1]];
                index = optionsIndexDic1[options[i + 1]];
            }
            options[i].SetSiblingIndex(index);
            currentCoroutine = StartCoroutine(MoveToTarget(options[i], pos));
        }
        yield return null;
        //---


    }
    IEnumerator MoveToTarget(Transform transform, Vector3 target)
    {
        if (!ScaleSwitch)
        {
            ResetScale();
        }
        if (!AlphaSwitch)
        {
            ResetAlpha();
        }
        SetBorder(false);

        float temp = (transform.localPosition - target).magnitude * Speed;
        while (transform.localPosition != target)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * temp);

            yield return null;
            if (AlphaSwitch)
            {
                SetAlpha();
            }
            if (ScaleSwitch)
            {
                SetScale(SmoothTime);//移动过程中缩放
            }
        }


        //if (ScaleSwitch)
        //{
        //    SetScale(SmoothTime);//移动后缩放
        //}

        SetFirstBorder(true);
        SetRaycasts();
        yield return null;
        //防止连续点击按钮出现问题
        LeftBtn.interactable = true;
        RightBtn.interactable = true;
    }
    IEnumerator ChangeScale(Transform transform, float target, float smoothTime)
    {
        float temp = 0;
        while (Mathf.Abs(transform.localScale.x - target) > 0.001)
        {
            float s = Mathf.SmoothDamp(transform.localScale.x, target, ref temp, smoothTime);
            transform.localScale = Vector3.one * s;
            yield return null;
        }
    }

    public void ClickLeft()
    {
        LeftBtn.interactable = false;

        StartCoroutine(MoveLeft());
    }
    public void ClickRight()
    {
        RightBtn.interactable = false;

        StartCoroutine(MoveRight());

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlideSwitch)
        {
            offset = Input.mousePosition - last;
            if (Mathf.Abs(offset.x) > SlideOffset)
            {
                if (offset.x > 0)
                {
                    StartCoroutine(MoveRight());
                }
                else
                {
                    StartCoroutine(MoveLeft());

                }
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlideSwitch)
        {
            last = Input.mousePosition;
        }

    }
}
