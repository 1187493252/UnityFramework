/*
* FileName:          UIWindowMove
* CompanyName:       
* Author:            
* Description:       要设置锚点靠左/右
*/

using DG.Tweening;
using UnityEngine;

public class UIWindowMove : MonoBehaviour
{
    public RectTransform MoveTF;
    float origiX;
    public float offsetX = 0;

    public float duration = 0.5f;
    [Range(-1, 1)]
    public int direction = 1;
    void Awake()
    {

        origiX = MoveTF.rect.width;
    }

    public void MoveHide()
    {
        float targetX = direction == 1 ? Screen.width + origiX / 2 + offsetX : -origiX / 2 + offsetX;
        MoveTF.DOMoveX(targetX, duration);
    }

    public void MoveShow()
    {
        float targetX = direction == 1 ? Screen.width - origiX / 2 + offsetX : origiX / 2 + offsetX;
        MoveTF.DOMoveX(targetX, duration);

    }
}
