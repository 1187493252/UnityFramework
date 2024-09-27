#if VIU_STEAMVR_2_0_0_OR_NEWER


using HighlightingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime.Task
{
    public class ToolHighLight : MonoBehaviour
    {
        private ToolConf toolConf;
        /// <summary>
        /// 当前是否是高亮状态
        /// </summary>
        [HideInInspector]
        public bool isHighlight;

        private Highlighter highlighter;
        private ToolBasic toolBasic;
        Material localmaterial;
        private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private List<Material> dicMeshMat = new List<Material>();



        private void Awake()
        {
            toolBasic = GetComponent<ToolBasic>();
            meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>(true));
            highlighter = GetComponent<Highlighter>() ? GetComponent<Highlighter>() : gameObject.AddComponent<Highlighter>();
            highlighter.overlay = true;

            highlighter.constantColor = Color.yellow;

            Color startColor = Color.yellow;
            Color endColor = Color.yellow;
            startColor.a = 0;
            highlighter.tweenGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(new Color(startColor.r, startColor.g, startColor.b, 0f), 0f),
                    new GradientColorKey(new Color(endColor.r, endColor.g, endColor.b, 1f), 1f)
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(startColor.a, 0f),
                    new GradientAlphaKey(endColor.a, 1f),
                }
            };
            foreach (var item in meshRenderers)
            {
                dicMeshMat.Add(item.material);
            }

            if (toolBasic)
                ReadToolConf(toolBasic.toolConf);


        }
        public void ReadToolConf(ToolConf toolConf)
        {
            this.toolConf = toolConf;
            if (toolConf != null)
            {
                ///判断是否显示本体
                if (!toolConf.isShowBody)
                {
                    HideBody();
                }
                highlighter.tweenDuration = toolConf.flashFrequency;
                highlighter.tweenLoop = toolConf.loopMode;
                highlighter.tweenEasing = toolConf.easing;

            }
        }
        public void ShowBody()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].material = dicMeshMat[i];
            }
        }
        public void HideBody()
        {
            if (toolConf.lightMat)
            {
                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    meshRenderers[i].material = toolConf.lightMat;
                }
            }
        }

        public void HideMesh()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = false;
            }
        }

        public void ShowMesh()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = true;
            }
        }

        public void ShowToolLight(bool isTween = false)
        {
            if (isTween)
            {
                highlighter.tween = true;
            }
            else
            {
                highlighter.constant = true;
            }
        }
        public void SetHighLight(bool isHighLight, bool isTween = false)
        {
            if (isHighLight)
            {
                ShowToolLight(isTween);
            }
            else
            {
                HideHighLight();
            }
        }

        public void HideHighLight()
        {
            highlighter.tween = false;

            highlighter.ConstantOff();
            highlighter.constant = false;
            //关闭子物体的高亮
            Highlighter[] highlighters = transform.GetComponentsInChildren<Highlighter>();
            if (highlighters != null || highlighters.Length > 0)
            {
                foreach (var item in highlighters)
                {
                    item.tween = false;
                    item.ConstantOff();
                    item.constant = false;
                }
            }
        }

        public void AddFilterList(params Transform[] transforms)
        {
            //foreach (var item in transforms)
            //{
            //	if (item)
            //	{
            //		highlighter._filterList.Add(item);
            //	}
            //}
        }
        public void RemoveFilterList(params Transform[] transforms)
        {
            //foreach (var item in transforms)
            //{
            //	if (item && _filterList.Contains(item))
            //	{
            //		_filterList.Remove(item);
            //	}
            //}
        }

        internal void SetLightEnd()
        {
        }

        internal void HideToolLight()
        {
            HideHighLight();
        }
    }
}
#endif