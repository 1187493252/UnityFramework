/*
* FileName:          DebuggerComponent
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Debugger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{

    public sealed partial class DebuggerComponent : UnityFrameworkComponent
    {
        private abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
        {
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            public virtual void Initialize(params object[] args)
            {
            }

            public virtual void Shutdown()
            {
            }

            public virtual void OnEnter()
            {
            }

            public virtual void OnLeave()
            {
            }

            public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
            }

            public void OnDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    OnDrawScrollableWindow();
                }
                GUILayout.EndScrollView();
            }

            protected abstract void OnDrawScrollableWindow();

            protected static void DrawItem(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title, GUILayout.Width(TitleWidth));
                    if (GUILayout.Button(content, "label"))
                    {
                        CopyToClipboard(content);
                    }
                }
                GUILayout.EndHorizontal();
            }

            protected static string GetByteLengthString(long byteLength)
            {
                if (byteLength < 1024L) // 2 ^ 10
                {
                    return Utility.Text.Format("{0} Bytes", byteLength.ToString());
                }

                if (byteLength < 1048576L) // 2 ^ 20
                {
                    return Utility.Text.Format("{0} KB", (byteLength / 1024f).ToString("F2"));
                }

                if (byteLength < 1073741824L) // 2 ^ 30
                {
                    return Utility.Text.Format("{0} MB", (byteLength / 1048576f).ToString("F2"));
                }

                if (byteLength < 1099511627776L) // 2 ^ 40
                {
                    return Utility.Text.Format("{0} GB", (byteLength / 1073741824f).ToString("F2"));
                }

                if (byteLength < 1125899906842624L) // 2 ^ 50
                {
                    return Utility.Text.Format("{0} TB", (byteLength / 1099511627776f).ToString("F2"));
                }

                if (byteLength < 1152921504606846976L) // 2 ^ 60
                {
                    return Utility.Text.Format("{0} PB", (byteLength / 1125899906842624f).ToString("F2"));
                }

                return Utility.Text.Format("{0} EB", (byteLength / 1152921504606846976f).ToString("F2"));
            }
        }


    }
}
