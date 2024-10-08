using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace K_UnityGF.ColorPalette
{
    /// <summary>
    /// 调色板管理器
    /// </summary>
    public class ColorPaletteManager : MonoBehaviour
    {
        public static ColorPaletteManager Instance;

        private const int width = 256;                        
        private const int height = 256;

        private Color[,] arrayColor;
        internal Color[,] ArrayColor { get => arrayColor; }

        private Color color_Result = Color.red;                         //颜色结果

        public static Color32 color_Hue1 = Color.white;                 //颜色-色调
        public static Color32 color_Hue2 = Color.red;                   //颜色-色调
        public static Color32 color_Saturation = Color.white;           //颜色-饱和度
        public static Color32 color_Value = Color.red;                  //颜色-明度

        private ColorPalatteElement_Hue element_Hue;                    //元素-色调
        private ColorPalatteElement_Saturation element_Saturation;      //元素-饱和度
        private ColorPalatteElement_Value element_Value;                //元素-明度
        private ColorPalatteElement_Alpha element_Alpha;                //元素-透明度

        [Header("RawImage-实际颜色")]
        public RawImage rawImage_ActualColor;

        [Header("RawImage-调色板")]
        public RawImage rawImage_ColorPalette;
        private Texture2D tex_ColorPalette;                             //Texture2D-调色板

        [Header("(色调)Hue 父节点"), Space(10)]
        public GameObject hueParent;

        [Header("(饱和度)Saturation 父节点")]
        public GameObject saturationParent;

        [Header("(明度)Value 父节点")]
        public GameObject valueParent;

        [Header("(透明度)Alpha 父节点")]
        public GameObject alphaParent;

        [Header("颜色16进制输出框"),Space(10)]
        public InputField inputField_16Hex;

        [Header("游戏运行时显示"), SerializeField]
        private bool activeRuning;

        private Transform colorTarget;                                  //颜色对象

        private void Awake() { Instance = this; }

        private void Start()
        {
            arrayColor = new Color[width, height];
            rawImage_ActualColor.color = color_Hue1;
            tex_ColorPalette = new Texture2D(width, height, TextureFormat.RGBA32, true);
            tex_ColorPalette.SetPixels(CalcArrayColor(Color.red));
            tex_ColorPalette.Apply();
            rawImage_ColorPalette.texture = tex_ColorPalette;

            //Hue
            element_Hue = hueParent.AddComponent<ColorPalatteElement_Hue>();
            element_Hue.SetParameters(20, 270);

            //Saturation
            element_Saturation = saturationParent.AddComponent<ColorPalatteElement_Saturation>();

            //Value
            element_Value = valueParent.AddComponent<ColorPalatteElement_Value>();

            //A
            element_Alpha = alphaParent.AddComponent<ColorPalatteElement_Alpha>();

            //添加事件 
            element_Hue.UpdateSendColorDelegate += UpdateColor;
            element_Saturation.UpdateColor += UpdateColor;
            element_Value.UpdateColor += UpdateColor;
            element_Alpha.UpdateColor += UpdateColor;

            if (!activeRuning) { gameObject.SetActive(false); }
        }

        /// <summary>
        /// 设置颜色和对象
        /// </summary>
        /// <param name="_color"></param>
        public void SetColorAndTarget(Color _color,Transform _target)
        {
            colorTarget = _target;
            Color.RGBToHSV(_color, out float hue, out float saturation, out float value);

            element_Hue.SliderOnValueChangeEvent(hue * 360);
            element_Saturation.SliderOnValueChangeEvent(saturation * 255);
            element_Value.SliderOnValueChangeEvent(value * 255);
            element_Alpha.SliderOnValueChangeEvent(_color.a * 255);
        }

        /// <summary>
        /// 更新颜色
        /// </summary>
        /// <param name="endColor"></param>
        private void UpdateColor(Color endColor)
        {
            UpdateColor();
            tex_ColorPalette.SetPixels(CalcArrayColor(color_Hue1));
            tex_ColorPalette.Apply();
            rawImage_ColorPalette.texture = tex_ColorPalette;
        }

        /// <summary>
        /// 更新颜色
        /// </summary>
        private void UpdateColor()
        {
            float hue, saturation, value;       //HSV 值

            Color.RGBToHSV(color_Hue1, out hue, out _, out _);
            Color.RGBToHSV(color_Saturation, out _, out saturation, out _);
            Color.RGBToHSV(color_Value, out _, out _, out value);

            rawImage_ActualColor.color = Color.HSVToRGB(hue, saturation, value);
            rawImage_ActualColor.color = new Color(rawImage_ActualColor.color.r, rawImage_ActualColor.color.g, rawImage_ActualColor.color.b, element_Alpha.Color_a);
            color_Result = rawImage_ActualColor.color;

            inputField_16Hex.text = ColorUtility.ToHtmlStringRGB(color_Result);

            if (colorTarget == null) return;
            colorTarget.gameObject.SendMessage("UpdateColor", color_Result);
        }

        /// <summary>
        /// 计算颜色
        /// </summary>
        /// <param name="endColor">目标颜色</param>
        /// <returns></returns>
        private Color[] CalcArrayColor(Color endColor)
        {
            Color value = (endColor - Color.white) / (width - 1);
            for (int i = 0; i < width; i++)
            {
                ArrayColor[i, width - 1] = Color.white + value * i;
            }

            for (int i = 0; i < width; i++)
            {
                value = (ArrayColor[i, width - 1] - Color.black) / (width - 1);
                for (int j = 0; j < width; j++)
                {
                    ArrayColor[i, j] = Color.black + value * j;
                }
            }
            List<Color> listColor = new List<Color>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    listColor.Add(ArrayColor[j, i]);
                }
            }

            return listColor.ToArray();
        }

        /// <summary>
        /// 获取颜色结果
        /// </summary>
        /// <returns></returns>
        public Color GetResultColor() { return color_Result; }

        /// <summary>
        /// 获取颜色结果16进制
        /// </summary>
        /// <returns></returns>
        public string GetResultColor16Hex() { return ColorUtility.ToHtmlStringRGB(color_Result);}

        /// <summary>
        /// 关闭调色板
        /// </summary>
        public void CloseColorPalatte() { colorTarget = null; gameObject.SetActive(false); }

        /// <summary>
        /// 显示
        /// </summary>
        public void Display() { gameObject.SetActive(true); }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hidden() { gameObject.SetActive(false); }
    }
}
