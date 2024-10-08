/*
* FileName:          LoopScrollView 
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

public class LoopScrollView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool SlideSwitch = true;//滑动开关
    public float SlideOffset = 270;//滑动触发值

    [SerializeField] private Sprite[] roomTextures; //所有卡片
    [SerializeField] private GameObject itemPrefab; //列表项预制体
    [SerializeField] private Transform itemParent; //列表项的父级，将卡片生成到该物体下
    [SerializeField] private Button prevButton; //上一个按钮
    [SerializeField] private Button nextButton; //下一个按钮
    [SerializeField] private float offsetX = 400f; //卡片之间的间距


    //生成的卡片列表
    private readonly List<LoopScrollViewItem> itemList = new List<LoopScrollViewItem>();
    //字典用于存储各位置对应的卡片层级和坐标
    private readonly Dictionary<int, LoopScrollViewData> map = new Dictionary<int, LoopScrollViewData>();
    Vector3 last;//记录鼠标位置

    Vector3 offset;//鼠标滑动差值
    private void Start()
    {
        for (int i = 0; i < roomTextures.Length; i++)
        {
            var tex = roomTextures[i];
            var instance = Instantiate(itemPrefab);
            instance.SetActive(true);
            instance.transform.SetParent(itemParent, false);
            instance.GetComponent<Image>().sprite = tex;
            instance.name = i.ToString();

            //坐标位置
            (instance.transform as RectTransform).anchoredPosition3D = Vector3.right * offsetX
                * (i <= roomTextures.Length / 2 ? i : (i - roomTextures.Length));
            //层级关系 0最底层 itemParent.childCount - 1最上层 
            instance.transform.SetSiblingIndex(i <= roomTextures.Length / 2 ? 0 : itemParent.childCount - 2);


            //大小
            instance.transform.localScale = (i == 0 ? 1.2f : 1f) * Vector3.one;

            var item = instance.AddComponent<LoopScrollViewItem>();
            item.Index = i;
            itemList.Add(item);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            map.Add(i, new LoopScrollViewData(item.transform.GetSiblingIndex(), (item.transform as RectTransform).anchoredPosition3D));
        }

        //添加按钮点击事件
        nextButton.onClick.AddListener(OnNextButtonClick);
        prevButton.onClick.AddListener(OnPrevButtonClick);
    }

    //下一个按钮点击事件
    private void OnNextButtonClick()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            bool last = item.Index == itemList.Count / 2;//是不是最右侧
            int index = item.Index + 1;
            index = index >= itemList.Count ? 0 : index;
            item.Index = index;
            item.Move(map[index], last);
        }
    }
    //上一个按钮点击事件
    private void OnPrevButtonClick()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            bool last = item.Index == (itemList.Count / 2) + 1;//是不是最左侧

            int index = item.Index - 1;
            index = index < 0 ? itemList.Count - 1 : index;
            item.Index = index;

            item.Move(map[index], last);
        }
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
                    OnNextButtonClick();

                }
                else
                {
                    OnPrevButtonClick();

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
public class LoopScrollViewData
{
    public int SiblingIndex { get; private set; }

    public Vector3 AnchorPosition3D { get; private set; }

    public LoopScrollViewData(int siblingIndex, Vector3 anchorPosition3D)
    {
        SiblingIndex = siblingIndex;
        AnchorPosition3D = anchorPosition3D;
    }
}