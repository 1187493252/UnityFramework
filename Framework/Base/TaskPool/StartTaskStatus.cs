/*
* FileName:          StartTaskStatus
* CompanyName:       
* Author:            relly
* Description:       开始处理任务的状态
*/


namespace Framework
{
    /// <summary>
    /// 开始处理任务的状态。
    /// </summary>
    public enum StartTaskStatus : byte
    {
        /// <summary>
        /// 可以立刻处理完成此任务。
        /// </summary>
        Done = 0,

        /// <summary>
        /// 可以继续处理此任务。
        /// </summary>
        CanResume,

        /// <summary>
        /// 不能继续处理此任务，需等待其它任务执行完成。
        /// </summary>
        HasToWait,

        /// <summary>
        /// 不能继续处理此任务，出现未知错误。
        /// </summary>
        UnknownError
    }
}
