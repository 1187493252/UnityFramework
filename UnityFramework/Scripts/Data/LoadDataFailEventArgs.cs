/*
* FileName:          LoadDataFailEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadDataFailEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadDataFailEventArgs).GetHashCode();
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

        public LoadDataFailEventArgs()
        {
            UserData = null;
        }
        public static LoadDataFailEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadDataFailEventArgs loadDataFailEventArgs = ReferencePool.Acquire<LoadDataFailEventArgs>();
            loadDataFailEventArgs.UserData = userData;
            return loadDataFailEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
