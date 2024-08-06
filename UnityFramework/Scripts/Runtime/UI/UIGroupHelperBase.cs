/*
* FileName:          UIGroupHelperBase
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework.UI;
using UnityEngine;

namespace UnityFramework.Runtime
{
    /// <summary>
    /// 界面组辅助器基类。
    /// </summary>
    public abstract class UIGroupHelperBase : MonoBehaviour, IUIGroupHelper
    {
        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public abstract void SetDepth(int depth);

        /// <summary>
        /// 设置Canvas RenderMode 0/1/2
        /// </summary>
        /// <param name="mode"></param>
        public abstract void SetRenderMode(int mode);
    }
}
