/*
* FileName:          LoadUIFormInfoFailEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadUIFormInfoFailEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadUIFormInfoFailEventArgs).GetHashCode();
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

        public LoadUIFormInfoFailEventArgs()
        {
            UserData = null;
        }
        public static LoadUIFormInfoFailEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadUIFormInfoFailEventArgs failEventArgs = ReferencePool.Acquire<LoadUIFormInfoFailEventArgs>();
            failEventArgs.UserData = userData;
            return failEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
