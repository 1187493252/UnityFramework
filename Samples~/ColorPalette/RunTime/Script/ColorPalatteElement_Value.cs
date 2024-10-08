using UnityEngine;
using System.Collections.Generic;

namespace K_UnityGF.ColorPalette
{
    /// <summary>
    /// 调色板元素-Value-明度
    /// </summary>
    public class ColorPalatteElement_Value : ColorPalatteElementBase
    {
        private ColorPanelCircle colorPanelCircle;          //颜色面板Circle

        protected override void Awake()
        {
            base.Awake();

            colorPanelCircle = FindObjectOfType<ColorPanelCircle>();
        }

        protected override void Start()
        {
            base.Start();

            colorPanelCircle.Event_UpdatePosition += UpdateCirclePoint;
        }

        private void Update()
        {
            texture2D.SetPixels(CalcArrayColor());
            texture2D.Apply();
            m_RawImage.texture = texture2D;
        }

        public override void SliderOnValueChangeEvent(float value)
        {
            arrayColor = colorPaletteManager.ArrayColor;

            float f = value;
            if (f == 0)
            {
                f = 1;
            }

            ColorPaletteManager.color_Value = Color.HSVToRGB(1, 1, f / 255);

            colorPanelCircle.Rect.anchoredPosition = new Vector2(colorPanelCircle.Rect.anchoredPosition.x, f);

            m_InputField.text = value.ToString();
            UpdateColor?.Invoke();
        }

        public override void InputFieldOnValueChangeEvent(string value)
        {
            if (value == "")
            {
                value = "0";
            }
            float sliderValue = float.Parse(value);
            if (sliderValue > 255)
            {
                sliderValue = 255;
                m_InputField.text = sliderValue.ToString();
            }
            m_Slider.value = sliderValue;
        }

        protected override Color[] CalcArrayColor()
        {
            Color[,] colr = new Color[255, 255];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    colr[i, j] = arrayColor[255 - 1, i];
                }
            }
            List<Color> listColor = new List<Color>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    listColor.Add(colr[j, i]);
                }
            }

            return listColor.ToArray();
        }

        /// <summary>
        /// 更新圆圈位置
        /// </summary>
        /// <param name="position"></param>
        private void UpdateCirclePoint(Vector2 position) { m_Slider.value = position.y; }
    }
}
