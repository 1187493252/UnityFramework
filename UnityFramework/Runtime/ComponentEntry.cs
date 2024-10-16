using Framework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;



namespace UnityFramework.Runtime
{
    public class ComponentEntry : MonoBehaviour
    {
        //  List<string> listTag = new List<string>() { "Data", "Config", "Audio", "Entity", "UIForm" };
        List<string> listTag = new List<string>() { "Data", "Config" };

        Dictionary<string, bool> dicTag = new Dictionary<string, bool>();
        static bool IsInit;
        public static EntityComponent Entity
        {
            get;
            private set;
        }
        public static UIComponent UI
        {
            get;
            private set;
        }
        public static AudioComponent Audio
        {
            get;
            private set;
        }
        public static TimerComponent Timer
        {
            get;
            private set;
        }
        public static DataComponent Data
        {
            get;
            private set;
        }

        public static EventComponent Event
        {
            get;
            private set;
        }
        public static WebRequestComponent WebRequest
        {
            get;
            private set;
        }
        public static SceneComponent Scene
        {
            get;
            private set;
        }
        public static AnimComponent Anim
        {
            get;
            private set;
        }
        public static TaskComponent Task
        {
            get;
            private set;
        }
        public static ExternalCallComponent ExternalCall
        {
            get;
            private set;
        }

        public static ResourceComponent Resource
        {
            get;
            private set;
        }
        public static SettingComponent Setting
        {
            get;
            private set;
        }
        public static ConfigComponent Config
        {
            get;
            private set;
        }
        public static GameObjectComponent GameObject
        {
            get;
            private set;
        }
        public static MonoBehaviourComponent MonoBehaviour
        {
            get;
            private set;
        }
        public static CoroutineComponent Coroutine
        {
            get;
            private set;
        }
        public static DebuggerComponent Debugger
        {
            get;
            private set;
        }

        public static event Action OnInitFinish;
        public bool IsDontDestroyOnLoad = true;


        private void Start()
        {
            if (!IsInit)
            {
                Init();
            }
            else
            {
                Destroy(gameObject);
            }

        }
        private void Update()
        {
            //if (!IsInit)
            //{
            //    foreach (var item in dicTag)
            //    {
            //        if (item.Value == false)
            //        {
            //            return;
            //        }
            //    }
            //    IsInit = true;
            //    OnInitFinish?.Invoke();
            //    OnInitFinish = null;
            //}
        }

        void Init()
        {
            Entity = UnityFrameworkEntry.GetComponent<EntityComponent>();
            UI = UnityFrameworkEntry.GetComponent<UIComponent>();
            Audio = UnityFrameworkEntry.GetComponent<AudioComponent>();
            Timer = UnityFrameworkEntry.GetComponent<TimerComponent>();
            Data = UnityFrameworkEntry.GetComponent<DataComponent>();
            Event = UnityFrameworkEntry.GetComponent<EventComponent>();
            WebRequest = UnityFrameworkEntry.GetComponent<WebRequestComponent>();
            Scene = UnityFrameworkEntry.GetComponent<SceneComponent>();
            Anim = UnityFrameworkEntry.GetComponent<AnimComponent>();
            Task = UnityFrameworkEntry.GetComponent<TaskComponent>();
            ExternalCall = UnityFrameworkEntry.GetComponent<ExternalCallComponent>();
            Resource = UnityFrameworkEntry.GetComponent<ResourceComponent>();
            Setting = UnityFrameworkEntry.GetComponent<SettingComponent>();
            Config = UnityFrameworkEntry.GetComponent<ConfigComponent>();
            GameObject = UnityFrameworkEntry.GetComponent<GameObjectComponent>();
            MonoBehaviour = UnityFrameworkEntry.GetComponent<MonoBehaviourComponent>();
            Coroutine = UnityFrameworkEntry.GetComponent<CoroutineComponent>();
            Debugger = UnityFrameworkEntry.GetComponent<DebuggerComponent>();

            if (IsDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            foreach (var item in listTag)
            {
                dicTag.Add(item, false);
            }


            Event.Subscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);
            Event.Subscribe(LoadConfigSuccessEventArgs.EventId, LoadConfigSuccess);
            // Event.Subscribe(LoadAudioInfoSuccessEventArgs.EventId, LoadAudioInfoSuccess);
            // Event.Subscribe(LoadUIFormInfoSuccessEventArgs.EventId, LoadUIFormInfoSuccess);
            // Event.Subscribe(LoadEntityInfoSuccessEventArgs.EventId, LoadEntityInfoSuccess);



            Data.Init();

        }

        private void LoadEntityInfoSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadEntityInfoSuccessEventArgs.EventId, LoadEntityInfoSuccess);
            dicTag["Entity"] = true;
        }

        private void LoadUIFormInfoSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadUIFormInfoSuccessEventArgs.EventId, LoadUIFormInfoSuccess);
            dicTag["UIForm"] = true;
        }

        private void LoadAudioInfoSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadAudioInfoSuccessEventArgs.EventId, LoadAudioInfoSuccess);
            dicTag["Audio"] = true;
        }

        private void LoadConfigSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, LoadConfigSuccess);
            dicTag["Config"] = true;
        }

        private void LoadDataSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);
            dicTag["Data"] = true;
            Audio.Init();
            Entity.Init();
            UI.Init();
            Config.Init();
            Task.Init();
            GameObject.Init();
            IsInit = true;
            OnInitFinish?.Invoke();
            OnInitFinish = null;
        }


    }
}