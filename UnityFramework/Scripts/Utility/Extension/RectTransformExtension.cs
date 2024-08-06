using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework
{
    /// <summary>
    /// RectTransform扩展
    /// </summary>
    public static class RectTransformExtension
    {
        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <param name="allign"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static RectTransform SetAnchor(this RectTransform rectTransform, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            rectTransform.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign)
            {
                case AnchorPresets.TopLeft:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                case AnchorPresets.TopCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 1);
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                case AnchorPresets.TopRight:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                case AnchorPresets.MiddleLeft:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;
                case AnchorPresets.MiddleCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case AnchorPresets.MiddleRight:
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;

                case AnchorPresets.BottomLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case AnchorPresets.BottonCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
                case AnchorPresets.BottomRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;

                case AnchorPresets.HorStretchTop:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case AnchorPresets.HorStretchMiddle:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case AnchorPresets.HorStretchBottom:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;

                case AnchorPresets.VertStretchLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                case AnchorPresets.VertStretchCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                case AnchorPresets.VertStretchRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                case AnchorPresets.StretchAll:
                    rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
                    rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                    rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
                    rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;
            }

            return rectTransform;
        }

        /// <summary>
        /// 设置中心点
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <param name="preset">调整位置</param>
        public static RectTransform SetPivot(this RectTransform rectTransform, PivotPresets preset)
        {
            switch (preset)
            {
                case PivotPresets.TopLeft: rectTransform.pivot = new Vector2(0, 1); break;

                case PivotPresets.TopCenter: rectTransform.pivot = new Vector2(0.5f, 1); break;

                case PivotPresets.TopRight: rectTransform.pivot = new Vector2(1, 1); break;

                case PivotPresets.MiddleLeft: rectTransform.pivot = new Vector2(0, 0.5f); break;

                case PivotPresets.MiddleCenter: rectTransform.pivot = new Vector2(0.5f, 0.5f); break;

                case PivotPresets.MiddleRight: rectTransform.pivot = new Vector2(1, 0.5f); break;

                case PivotPresets.BottomLeft: rectTransform.pivot = new Vector2(0, 0); break;

                case PivotPresets.BottomCenter: rectTransform.pivot = new Vector2(0.5f, 0); break;

                case PivotPresets.BottomRight: rectTransform.pivot = new Vector2(1, 0); break;
            }

            return rectTransform;
        }

        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <param name="sizeDelta">大小</param>
        /// <returns></returns>
        public static RectTransform SetSizeDelta(this RectTransform rectTransform, Vector2 sizeDelta)
        {
            rectTransform.sizeDelta = sizeDelta;
            return rectTransform;
        }

        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public static RectTransform SetSizeWidth(this RectTransform rectTransform, float width)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            return rectTransform;
        }

        /// <summary>
        /// 设置高度
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static RectTransform SetSizeHeight(this RectTransform rectTransform, float height)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            return rectTransform;
        }

        /// <summary>
        /// 刷新RectTransform
        /// </summary>
        /// <param name="rectTransform">实例</param>
        /// <returns></returns>
        public static RectTransform ForceRebuildLayoutImmediate(this RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            return rectTransform;
        }
    }

    /// <summary>
    /// 锚点对齐位置
    /// </summary>
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }

    /// <summary>
    /// 中心点对齐位置
    /// </summary>
    public enum PivotPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }
}
