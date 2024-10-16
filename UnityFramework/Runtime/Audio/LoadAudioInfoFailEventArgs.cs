/*
* FileName:          LoadAudioInfoFailEventArgs
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Event;

namespace UnityFramework.Runtime
{
    public class LoadAudioInfoFailEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadAudioInfoFailEventArgs).GetHashCode();
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

        public LoadAudioInfoFailEventArgs()
        {
            UserData = null;
        }
        public static LoadAudioInfoFailEventArgs Create(object userData = null)
        {
            // 使用引用池技术，避免频繁内存分配
            LoadAudioInfoFailEventArgs failEventArgs = ReferencePool.Acquire<LoadAudioInfoFailEventArgs>();
            failEventArgs.UserData = userData;
            return failEventArgs;
        }
        public override void Clear()
        {
            UserData = null;
        }
    }
}
