/*
* FileName:          LoadUIFormInfoSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadUIFormInfoSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadUIFormInfoSuccessEventArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public object UserData
        {
            get;
            private set;
        }

        public LoadUIFormInfoSuccessEventArgs()
        {
            UserData = null;
        }
        public static LoadUIFormInfoSuccessEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadUIFormInfoSuccessEventArgs successEventArgs = ReferencePool.Acquire<LoadUIFormInfoSuccessEventArgs>();
            successEventArgs.UserData = userData;
            return successEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
