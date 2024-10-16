/*
* FileName:          LoadEntityInfoSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadEntityInfoSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadEntityInfoSuccessEventArgs).GetHashCode();
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

        public LoadEntityInfoSuccessEventArgs()
        {
            UserData = null;
        }
        public static LoadEntityInfoSuccessEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadEntityInfoSuccessEventArgs successEventArgs = ReferencePool.Acquire<LoadEntityInfoSuccessEventArgs>();
            successEventArgs.UserData = userData;
            return successEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
