/*
* FileName:          WebRequestAgentHelperErrorEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/



namespace Framework.WebRequest
{
    /// <summary>
    /// Web 请求代理辅助器错误事件。
    /// </summary>
    public sealed class WebRequestAgentHelperErrorEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求代理辅助器错误事件的新实例。
        /// </summary>
        public WebRequestAgentHelperErrorEventArgs()
        {
            ErrorMessage = null;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建 Web 请求代理辅助器错误事件。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的 Web 请求代理辅助器错误事件。</returns>
        public static WebRequestAgentHelperErrorEventArgs Create(string errorMessage)
        {
            WebRequestAgentHelperErrorEventArgs webRequestAgentHelperErrorEventArgs = ReferencePool.Acquire<WebRequestAgentHelperErrorEventArgs>();
            webRequestAgentHelperErrorEventArgs.ErrorMessage = errorMessage;
            return webRequestAgentHelperErrorEventArgs;
        }

        /// <summary>
        /// 清理 Web 请求代理辅助器错误事件。
        /// </summary>
        public override void Clear()
        {
            ErrorMessage = null;
        }
    }
}
