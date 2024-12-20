/*
* FileName:          TaskStatus
* CompanyName:       
* Author:            relly
* Description:       任务状态
*/


namespace Framework
{
    /// <summary>
    /// 任务状态。
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 未开始。
        /// </summary>
        Todo = 0,

        /// <summary>
        /// 执行中。
        /// </summary>
        Doing,

        /// <summary>
        /// 完成。
        /// </summary>
        Done
    }
}
