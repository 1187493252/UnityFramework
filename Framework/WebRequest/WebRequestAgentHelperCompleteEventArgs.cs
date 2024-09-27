/*
* FileName:          WebRequestAgentHelperCompleteEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/



namespace Framework.WebRequest
{
    public sealed class WebRequestAgentHelperCompleteEventArgs : FrameworkEventArgs
    {
        private byte[] m_WebResponseBytes;
        private string m_WebResponseText;
        /// <summary>
        /// 初始化 Web 请求代理辅助器完成事件的新实例。
        /// </summary>
        public WebRequestAgentHelperCompleteEventArgs()
        {
            m_WebResponseBytes = null;
            m_WebResponseText = null;

        }

        /// <summary>
        /// 创建 Web 请求代理辅助器完成事件。
        /// </summary>
        /// <param name="webResponseBytes">Web 响应的数据流。</param>
        /// <returns>创建的 Web 请求代理辅助器完成事件。</returns>
        public static WebRequestAgentHelperCompleteEventArgs Create(byte[] webResponseBytes, string webResponseText)
        {
            WebRequestAgentHelperCompleteEventArgs webRequestAgentHelperCompleteEventArgs = ReferencePool.Acquire<WebRequestAgentHelperCompleteEventArgs>();
            webRequestAgentHelperCompleteEventArgs.m_WebResponseBytes = webResponseBytes;
            webRequestAgentHelperCompleteEventArgs.m_WebResponseText = webResponseText;
            return webRequestAgentHelperCompleteEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求代理辅助器完成事件。
        /// </summary>
        public override void Clear()
        {
            m_WebResponseBytes = null;
            m_WebResponseText = null;
        }

        /// <summary>
        /// 获取 Web 响应的字符串。
        /// </summary>
        /// <returns></returns>
        public string GetWebResponseText()
        {
            return m_WebResponseText;
        }

        /// <summary>
        /// 获取 Web 响应的数据流。
        /// </summary>
        /// <returns>Web 响应的数据流。</returns>
        public byte[] GetWebResponseBytes()
        {
            return m_WebResponseBytes;
        }
    }
}
