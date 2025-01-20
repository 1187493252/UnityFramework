/*
* FileName:          ResetAudioAgentEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace Framework.Audio
{
    /// <summary>
    /// 重置声音代理事件。
    /// </summary>
    public sealed class ResetAudioAgentEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化重置声音代理事件的新实例。
        /// </summary>
        public ResetAudioAgentEventArgs()
        {
        }

        /// <summary>
        /// 创建重置声音代理事件。
        /// </summary>
        /// <returns>创建的重置声音代理事件。</returns>
        public static ResetAudioAgentEventArgs Create()
        {
            ResetAudioAgentEventArgs resetSoundAgentEventArgs = ReferencePool.Acquire<ResetAudioAgentEventArgs>();
            return resetSoundAgentEventArgs;
        }

        /// <summary>
        /// 清理重置声音代理事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}
