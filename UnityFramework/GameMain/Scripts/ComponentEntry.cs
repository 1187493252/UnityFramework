using Framework.Event;
using System;
using UnityEngine;



namespace UnityFramework.Runtime
{
    public class ComponentEntry : MonoBehaviour
    {

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


        void Init()
        {
            IsInit = true;
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




            Event.Subscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);

            Data.Init();

        }



        private void LoadDataSuccess(object sender, GameEventArgs e)
        {
            Event.Unsubscribe(LoadDataSuccessEventArgs.EventId, LoadDataSuccess);
            Audio.Init();
            Entity.Init();
            UI.Init();
            Config.Init();
            Task.Init();
            GameObject.Init();
            OnInitFinish?.Invoke();
            OnInitFinish = null;
        }


    }
}