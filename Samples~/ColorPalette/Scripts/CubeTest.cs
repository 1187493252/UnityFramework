using UnityEngine;

namespace K_UnityGF.ColorPalette
{
    public class CubeTest : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private Material m_Material;

        private void Awake()
        {
            m_Material = GetComponent<MeshRenderer>().material;
        }

        private void OnMouseDown()
        {
            ColorPaletteManager.Instance.SetColorAndTarget(m_Material.color, transform);
            ColorPaletteManager.Instance.gameObject.SetActive(true);
        }

        public void UpdateColor(Color _color)
        {
            m_Material.color = _color;
        }
    }
}