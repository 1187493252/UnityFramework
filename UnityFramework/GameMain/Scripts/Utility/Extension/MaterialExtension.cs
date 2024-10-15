using UnityEngine;

namespace UnityFramework
{
    /// <summary>
    /// Material 拓展
    /// </summary>
    public static class MaterialExtension
    {
        /// <summary>
        /// 渲染模式
        /// </summary>
        public enum RenderingMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent,
        }

        /// <summary>
        /// 设置渲染模式
        /// </summary>
        /// <param name="_material"></param>
        /// <param name="_renderingMode">渲染模式类型</param>
        public static void SetRendingMode(Material _material, RenderingMode _renderingMode)
        {
            switch (_renderingMode)
            {
                case RenderingMode.Opaque:
                    _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    _material.SetInt("_ZWrite", 1);
                    _material.DisableKeyword("_ALPHATEST_ON");
                    _material.DisableKeyword("_ALPHABLEND_ON");
                    _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _material.renderQueue = -1;
                    break;
                case RenderingMode.Cutout:
                    _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    _material.SetInt("_ZWrite", 1);
                    _material.EnableKeyword("_ALPHATEST_ON");
                    _material.DisableKeyword("_ALPHABLEND_ON");
                    _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _material.renderQueue = 2450;
                    break;
                case RenderingMode.Fade:
                    _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _material.SetInt("_ZWrite", 0);
                    _material.DisableKeyword("_ALPHATEST_ON");
                    _material.EnableKeyword("_ALPHABLEND_ON");
                    _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _material.renderQueue = 3000;
                    break;
                case RenderingMode.Transparent:
                    _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _material.SetInt("_ZWrite", 0);
                    _material.DisableKeyword("_ALPHATEST_ON");
                    _material.DisableKeyword("_ALPHABLEND_ON");
                    _material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    _material.renderQueue = 3000;
                    break;
            }
        }
    }
}
