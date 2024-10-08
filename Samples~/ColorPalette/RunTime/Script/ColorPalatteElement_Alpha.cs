using UnityEngine;
using System.Collections.Generic;

namespace K_UnityGF.ColorPalette
{
    /// <summary>
    /// 调色板元素-Alpha-透明度
    /// </summary>
    public class ColorPalatteElement_Alpha : ColorPalatteElementBase
    {
        private float color_a = 1;              //颜色透明度

        public float Color_a { get => color_a; }

        public override void SliderOnValueChangeEvent(float value)
        {
            m_InputField.text = value.ToString();

            color_a = value / 255;
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
            Color value = (Color.black - Color.white) / (width - 1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    colr[j, i] = Color.white + value * i;
                }
            }

            List<Color> listColor = new List<Color>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    listColor.Add(colr[i, j]);
                }
            }

            return listColor.ToArray();
        }
    }
}