using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViews : MonoBehaviour
{
    public GameObject OptionPrefab;//预制体
    public Transform OptionGroup;//父物体

    [SerializeField]
    private PageView pageView;
    int num;
    public Button Lastpage, Nextpage;
    public Sprite[] Sprites;

    private void Awake()
    {
        for (int i = 0; i < Sprites.Length; i++)
        {
            GameObject clone = Instantiate(OptionPrefab, Vector3.zero, Quaternion.identity, OptionGroup);
            clone.name = i + "";
            Button btn = clone.GetComponent<Button>();
            //替换图片
            if (Sprites[i] != null)
            {
                btn.image.sprite = Sprites[i];
            }
            btn.onClick.AddListener(delegate
            {
                Debug.Log($"点击了{clone.name}");
            });
        }
    }
    // Use this for initialization
    void Start()
    {
        pageView.OnPageChanged = pageChanged;
        pageView.pageTo(0);
        Lastpage.onClick.AddListener(LastPage);
        Nextpage.onClick.AddListener(NextPage);

    }

    int pageChanged(int index)
    {
        num = index;
        return index;
    }



    public void NextPage()
    {
        pageView.NextPage(num);
    }
    public void LastPage()
    {
        pageView.LastPage(num);
    }

    void Destroy()
    {
        pageView.OnPageChanged = null;
    }

}
