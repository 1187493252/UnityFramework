/*
* FileName:          LoadAudioInfoSuccessEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadAudioInfoSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadAudioInfoSuccessEventArgs).GetHashCode();
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

        public LoadAudioInfoSuccessEventArgs()
        {
            UserData = null;
        }
        public static LoadAudioInfoSuccessEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadAudioInfoSuccessEventArgs successEventArgs = ReferencePool.Acquire<LoadAudioInfoSuccessEventArgs>();
            successEventArgs.UserData = userData;
            return successEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
