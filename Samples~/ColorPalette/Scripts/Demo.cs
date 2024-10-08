using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace K_UnityGF.ColorPalette
{
    public class Demo : MonoBehaviour
    {
        private Button m_Button;

        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }

        private void Start()
        {
            m_Button.onClick.AddListener(() => 
            {
                ColorPaletteManager.Instance.SetColorAndTarget(m_Button.image.color,transform);
                ColorPaletteManager.Instance.Display();
            });
        }

        public void UpdateColor(Color _color)
        {
            m_Button.image.color = _color;
        }
    }
}