using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipFollowMouse : MonoBehaviour
{
    public Text contentText;
    public bool isShow;
    RectTransform rectTransform;
    public Vector2 offset;//偏移
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (isShow)
        {
            SetLocalPosition(new Vector2(Input.mousePosition.x - Screen.width / 2 + offset.x, Input.mousePosition.y - Screen.height / 2 + rectTransform.rect.height / 2 + offset.y));
        }
        else
        {
            Hide();
        }
    }


    public void Show(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            contentText.text = name;
            isShow = true;
            gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        isShow = false;
        contentText.text = "";

        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector2 position)
    {
        transform.localPosition = position;
    }
}
