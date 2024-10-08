using System;
using UnityEngine;
using UnityEngine.UI;

namespace K_UnityGF.ColorPalette
{
    public delegate void UpdateSendColorDelegate(Color color);  //委托-发送颜色 更新颜色
    public delegate void UpdateColor();                         //更新颜色

    /// <summary>
    /// 调色板元素基类
    /// </summary>
    public class ColorPalatteElementBase : MonoBehaviour
    {
        public UpdateSendColorDelegate UpdateSendColorDelegate;
        public UpdateColor UpdateColor;

        protected ColorPaletteManager colorPaletteManager;      //调色板管理器

        protected Slider m_Slider;                              //滑动条
        protected RawImage m_RawImage;                          //RawImage
        protected InputField m_InputField;                      //输入框
        protected Texture2D texture2D;                          //texture2D
        protected Color[,] arrayColor;                          //颜色多维数组

        protected int width = 255;                              //宽度
        protected int height = 255;                             //高度

        protected virtual void Awake()
        {
            colorPaletteManager = GetComponentInParent<ColorPaletteManager>();

            m_Slider = GetComponentInChildren<Slider>();
            m_RawImage = GetComponentInChildren<RawImage>();
            m_InputField = GetComponentInChildren<InputField>();
        }

        protected virtual void Start()
        {
            arrayColor = new Color[width, height];
            texture2D = new Texture2D(width, height, TextureFormat.RGBA32, true);
            texture2D.SetPixels(CalcArrayColor());
            texture2D.Apply();
            m_RawImage.texture = texture2D;
            arrayColor = colorPaletteManager.ArrayColor;

            m_Slider.onValueChanged.AddListener(SliderOnValueChangeEvent);
            m_InputField.onValueChanged.AddListener(InputFieldOnValueChangeEvent);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="_width">宽</param>
        /// <param name="_height">高</param>
        public void SetParameters(int _width,int _height) { width = _width;height = _height; }

        /// <summary>
        /// 计算颜色列表
        /// </summary>
        /// <returns></returns>
        protected virtual Color[] CalcArrayColor() { return null; }

        /// <summary>
        /// 滑动条事件
        /// </summary>
        /// <param name="value">值</param>
        public virtual void SliderOnValueChangeEvent(float value) { }

        /// <summary>
        /// 输入框事件
        /// </summary>
        /// <param name="value">值</param>
        public virtual void InputFieldOnValueChangeEvent(string value) { }
    }
}
