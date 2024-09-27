/*
* FileName:          UIComponent.UIGroup
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public sealed partial class UIComponent : UnityFrameworkComponent
    {
        [Serializable]
        private sealed class UIGroup
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private int m_Depth = 0;
            [SerializeField]
            private RenderMode renderMode = RenderMode.ScreenSpaceOverlay;
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public int Depth
            {
                get
                {
                    return m_Depth;
                }
            }
            public RenderMode RenderMode
            {
                get
                {
                    return renderMode;
                }
            }
        }
    }
}
