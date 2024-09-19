/*
* FileName:          BaseEventArgs
* CompanyName:       
* Author:            relly
* Description:       
* 
*/



namespace Framework
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 获取类型编号
        /// </summary>
        public abstract int Id
        {
            get;
        }
    }
}