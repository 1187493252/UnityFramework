/*
* FileName:          LoadConfigSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadConfigSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadConfigSuccessEventArgs).GetHashCode();
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

        public LoadConfigSuccessEventArgs()
        {
            UserData = null;
        }
        public static LoadConfigSuccessEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadConfigSuccessEventArgs successEventArgs = ReferencePool.Acquire<LoadConfigSuccessEventArgs>();
            successEventArgs.UserData = userData;
            return successEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
