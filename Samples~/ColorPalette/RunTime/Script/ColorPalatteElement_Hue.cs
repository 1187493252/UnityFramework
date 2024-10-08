using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace K_UnityGF.ColorPalette
{
    /// <summary>
    /// 调色板元素-Hue-色调
    /// </summary>
    public class ColorPalatteElement_Hue : ColorPalatteElementBase
    {
        private Slider syncSlider;                               //同步Slider
        private InputField syncInputField;                       //同步InputFiled
        private RawImage syncRawImage;                           //同步RawImage

        protected override void Awake()
        {
            base.Awake();

            syncSlider = transform.parent.Find("Hue2").GetComponentInChildren<Slider>();
            syncInputField = transform.parent.Find("Hue2").GetComponentInChildren<InputField>();
            syncRawImage = transform.parent.Find("Hue2").GetComponentInChildren<RawImage>();
        }

        protected override void Start()
        {
            base.Start();

            syncRawImage.texture = texture2D;

            syncSlider.onValueChanged.AddListener((float value) =>
            {
                m_Slider.value = value;
                syncSlider.value = value;
                ColorPaletteManager.color_Hue2 = Color.HSVToRGB(value / 359, 1, 1);
                syncInputField.text = value.ToString();
                UpdateSendColorDelegate?.Invoke(ColorPaletteManager.color_Hue2);
            });

            syncInputField.onValueChanged.AddListener(InputFieldOnValueChangeEvent);
        }

        protected override Color[] CalcArrayColor()
        {
            int addValue = (height - 1) / 3;
            for (int i = 0; i < width; i++)
            {
                arrayColor[i, 0] = Color.red;
                arrayColor[i, addValue] = Color.green;
                arrayColor[i, addValue + addValue] = Color.blue;
                arrayColor[i, height - 1] = Color.red;
            }
            Color value = (Color.green - Color.red) / addValue;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < addValue; j++)
                {
                    arrayColor[i, j] = Color.red + value * j;
                }
            }
            value = (Color.blue - Color.green) / addValue;
            for (int i = 0; i < width; i++)
            {
                for (int j = addValue; j < addValue * 2; j++)
                {
                    arrayColor[i, j] = Color.green + value * (j - addValue);
                }
            }

            value = (Color.red - Color.blue) / ((height - 1) - addValue - addValue);
            for (int i = 0; i < width; i++)
            {
                for (int j = addValue * 2; j < height - 1; j++)
                {
                    arrayColor[i, j] = Color.blue + value * (j - addValue * 2);
                }
            }

            List<Color> listColor = new List<Color>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    listColor.Add(arrayColor[j, i]);
                }
            }

            return listColor.ToArray();
        }

        public override void SliderOnValueChangeEvent(float value)
        {
            m_Slider.value = value;
            syncSlider.value = value;
            ColorPaletteManager.color_Hue1 = Color.HSVToRGB(value / 359, 1, 1);
            m_InputField.text = value.ToString();
            UpdateSendColorDelegate?.Invoke(ColorPaletteManager.color_Hue1);
        }

        public override void InputFieldOnValueChangeEvent(string value)
        {
            if (value == "")
            {
                value = "0";

            }
            float sliderValue = float.Parse(value);
            if (sliderValue > 359)
            {
                sliderValue = 359;
                m_InputField.text = sliderValue.ToString();
            }
            m_Slider.value = sliderValue;
            syncSlider.value = sliderValue;
        }
    }
}
