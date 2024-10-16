/*
* FileName:          LoadConfigFailEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadConfigFailEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigFailEventArgs).GetHashCode();
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

        public LoadConfigFailEventArgs()
        {
            UserData = null;
        }
        public static LoadConfigFailEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadConfigFailEventArgs failEventArgs = ReferencePool.Acquire<LoadConfigFailEventArgs>();
            failEventArgs.UserData = userData;
            return failEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
