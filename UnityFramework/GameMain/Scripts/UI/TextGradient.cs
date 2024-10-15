using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class TextGradient : BaseMeshEffect
{
    [SerializeField] Color32 topColor = Color.white;
    [SerializeField] Color32 bottomColor = Color.black;


    public override void ModifyMesh(VertexHelper vh)
    {
        if (vh.currentVertCount <= 0) return;

        //取得模型
        float bottomY = -1;
        float topY = -1;

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);

            if (bottomY == -1)
                bottomY = v.position.y;
            if (topY == -1)
                topY = v.position.y;

            if (v.position.y > topY)
                topY = v.position.y;
            else if (v.position.y < bottomY)
                bottomY = v.position.y;
        }

        //混色
        UIVertex vx = new UIVertex();
        vh.PopulateUIVertex(ref vx, 0);

        Color32 topColor = new Color32((byte)((this.topColor.r + vx.color.r) / 2), (byte)((this.topColor.g + vx.color.g) / 2), (byte)((this.topColor.b + vx.color.b) / 2), 255);
        Color32 bottomColor = new Color32((byte)((this.bottomColor.r + vx.color.r) / 2), (byte)((this.bottomColor.g + vx.color.g) / 2), (byte)((this.bottomColor.b + vx.color.b) / 2), 255);

        //上色
        float uiElementHeight = topY - bottomY;
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);

            v.color = Color32.Lerp(bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);
            vh.SetUIVertex(v, i);
        }
    }

}
