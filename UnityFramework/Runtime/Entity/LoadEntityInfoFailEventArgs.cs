/*
* FileName:          LoadEntityInfoFailEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadEntityInfoFailEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadEntityInfoFailEventArgs).GetHashCode();
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

        public LoadEntityInfoFailEventArgs()
        {
            UserData = null;
        }
        public static LoadEntityInfoFailEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadEntityInfoFailEventArgs failEventArgs = ReferencePool.Acquire<LoadEntityInfoFailEventArgs>();
            failEventArgs.UserData = userData;
            return failEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
