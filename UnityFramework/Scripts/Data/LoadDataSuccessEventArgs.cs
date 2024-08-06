/*
* FileName:          LoadDataSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadDataSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadDataSuccessEventArgs).GetHashCode();
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

        public LoadDataSuccessEventArgs()
        {
            UserData = null;
        }
        public static LoadDataSuccessEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadDataSuccessEventArgs loadDataSuccessEventArgs = ReferencePool.Acquire<LoadDataSuccessEventArgs>();
            loadDataSuccessEventArgs.UserData = userData;
            return loadDataSuccessEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
