/*
* FileName:          AudioManager
* CompanyName:  
* Author:            relly
* Description:       Audio管理脚本,主要功能:PlayAudio,StopAudio,GetAudioSource,GetAudioClip等
*                    
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Audio;
using Framework.Resource;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UnityFramework.Runtime
{

    public class AudioInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id;
        /// <summary>
        /// 名称
        /// </summary>
        public string AudioName;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextContent;
        /// <summary>
        /// 音频
        /// </summary>
        public AudioClip Clip;
        /// <summary>
        /// 音频绑定的事件缓存
        /// </summary>
        public Dictionary<float, Action> AudioActionDic;
        /// <summary>
        /// 播放时保存的计时事件
        /// </summary>
        public List<Timer> TimerList;
    }

    [DisallowMultipleComponent]
    public sealed partial class AudioComponent : UnityFrameworkComponent
    {
        /// <summary>
        /// 当前音量,初始化每个播放器的声音
        /// </summary>
        private float mVolume;
        /// <summary>
        /// 当前最大音量
        /// </summary>
        private float mVolumeMax = 1.0f;
        /// <summary>
        /// 初始化BGM播放器音量
        /// </summary>
        private float mVolumeBGM;
        /// <summary>
        /// BGM播放器最大音量
        /// </summary>
        private float mVolumeBGMMax = 0.8f;
        /// <summary>
        /// 只播放BGM的AudioSource
        /// </summary>
        private AudioSource mBGMAudioSource;
        /// <summary>
        /// 默认AudioSource
        /// </summary>
        private AudioSource defaultAudioSource;

        /// <summary>
        /// 所有AudioSource缓存
        /// </summary>
        private Dictionary<string, AudioSource> mAudioSourceDic = new Dictionary<string, AudioSource>();

        //------------------
        private const int DefaultPriority = 0;

        private IAudioManager m_AudioManager = null;
        private EventComponent m_EventComponent = null;
        [SerializeField]
        private bool m_EnablePlayAudioUpdateEvent = false;

        [SerializeField]
        private bool m_EnablePlayAudioDependencyAssetEvent = false;
        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_AudioHelperTypeName = "UnityFramework.Runtime.DefaultAudioHelper";

        [SerializeField]
        private AudioHelperBase m_CustomAudioHelper = null;

        [SerializeField]
        private string m_AudioGroupHelperTypeName = "UnityFramework.Runtime.DefaultAudioGroupHelper";

        [SerializeField]
        private AudioGroupHelperBase m_CustomAudioGroupHelper = null;

        [SerializeField]
        private string m_AudioAgentHelperTypeName = "UnityFramework.Runtime.DefaultAudioAgentHelper";

        [SerializeField]
        private AudioAgentHelperBase m_CustomAudioAgentHelper = null;

        [SerializeField]
        private AudioGroup[] m_AudioGroups = null;

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int AudioGroupCount
        {
            get
            {
                return m_AudioManager.AudioGroupCount;
            }
        }


        //-------------------------
        public int AudioInfoCout
        {
            get
            {
                return AudioInfoDicById.Count;
            }
        }
        /// <summary>
        /// 语音配置信息缓存
        /// </summary>
        private Dictionary<int, AudioInfo> AudioInfoDicById = new Dictionary<int, AudioInfo>();


        public float Volume
        {
            get
            {
                return mVolume;
            }
            set
            {
                if (value > mVolumeMax)
                {
                    value = mVolumeMax;
                }
                mVolume = value;
            }
        }
        public float VolumeBGM
        {
            get
            {
                return mVolumeBGM;
            }
            set
            {
                if (value > mVolumeBGMMax)
                {
                    value = mVolumeBGMMax;
                }
                mVolumeBGM = value;
            }
        }
        public AudioSource DefaultAudioSource
        {
            get
            {
                if (defaultAudioSource == null)
                {
                    defaultAudioSource = GetDefaultAudioSource();
                }
                return defaultAudioSource;
            }
        }
        public AudioSource BGMAudioSource
        {
            get
            {
                if (mBGMAudioSource == null)
                {
                    mBGMAudioSource = GetBGMAudioSource();
                }
                return mBGMAudioSource;
            }
        }
        AudioHelper helper;
        protected override void Awake()
        {
            base.Awake();

            m_AudioManager = FrameworkEntry.GetModule<IAudioManager>();
            if (m_AudioManager == null)
            {
                Log.Fatal("Audio manager is invalid.");
                return;
            }

            m_AudioManager.PlayAudioSuccess += OnPlayAudioSuccess;
            m_AudioManager.PlayAudioFailure += OnPlayAudioFailure;

            if (m_EnablePlayAudioUpdateEvent)
            {
                m_AudioManager.PlayAudioUpdate += OnPlayAudioUpdate;
            }

            if (m_EnablePlayAudioDependencyAssetEvent)
            {
                m_AudioManager.PlayAudioDependencyAsset += OnPlayAudioDependencyAsset;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

        }
        private void Start()
        {
            BaseComponent baseComponent = UnityFrameworkEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = UnityFrameworkEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_AudioManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_AudioManager.SetResourceManager(FrameworkEntry.GetModule<IResourceManager>());
            }

            AudioHelperBase audioHelper = Helper.CreateHelper(m_AudioHelperTypeName, m_CustomAudioHelper);
            if (audioHelper == null)
            {
                Log.Error("Can not create audio helper.");
                return;
            }

            audioHelper.name = "Audio Helper";
            Transform transform = audioHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_AudioManager.SetAudioHelper(audioHelper);

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("Sound Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_AudioGroups.Length; i++)
            {
                if (!AddAudioGroup(m_AudioGroups[i].Name, m_AudioGroups[i].AvoidBeingReplacedBySamePriority, m_AudioGroups[i].Mute, m_AudioGroups[i].Volume, m_AudioGroups[i].AgentHelperCount))
                {
                    Log.Warning("Add audio group '{0}' failure.", m_AudioGroups[i].Name);
                    continue;
                }
            }
        }



        public void Init()
        {
            ClearAll();
            Volume = mVolumeMax;
            VolumeBGM = mVolumeBGMMax;
            helper = GetComponent<AudioHelper>();
            if (helper)
            {
                helper.Init();
            }
        }
        public AudioSource PlayAudioBGM(string _filename)
        {
            AudioClip clip = GetAudioClip(_filename);
            PlayAudio(clip, BGMAudioSource, 0, null);
            return BGMAudioSource;
        }
        public AudioSource PlayAudioBGM(int id)
        {
            AudioClip clip = GetAudioClip(id);
            PlayAudio(clip, BGMAudioSource, 0, null);
            return BGMAudioSource;
        }
        public AudioSource PlayAudio(string _fileName)
        {
            AudioClip clip = GetAudioClip(_fileName);

            PlayAudio(clip, DefaultAudioSource, 0, null);
            return DefaultAudioSource;
        }
        public AudioSource PlayAudio(int id)
        {
            AudioClip clip = GetAudioClip(id);

            PlayAudio(clip, DefaultAudioSource, 0, null);
            return DefaultAudioSource;
        }
        public void PlayAudio(List<string> _fileNames, AudioSource audioSource = null, float _delayPlay = 0, Action endAction = null)
        {
            if (_fileNames == null || _fileNames.Count < 1)
            {
                return;
            }
            if (audioSource == null)
            {
                audioSource = DefaultAudioSource;
            }
            AudioClip _clip = GetAudioClip(_fileNames.First());
            if (_clip == null)
            {
                return;
            }
            audioSource.clip = _clip;
            Dictionary<float, Action> actionDic = new Dictionary<float, Action> { };

            if (_fileNames.Count == 1)
            {
                actionDic.Add(audioSource.clip.length, endAction);
            }
            else
            {
                actionDic.Add(audioSource.clip.length, delegate {
                    PlayAudio(_fileNames, audioSource, _delayPlay, endAction);
                });
            }
            _fileNames.RemoveAt(0);
            PlayAudio(_clip, audioSource, _delayPlay, actionDic);
        }

        public void PlayAudio(List<int> _fileId, AudioSource audioSource = null, float _delayPlay = 0, Action endAction = null)
        {
            if (_fileId == null || _fileId.Count < 1)
            {
                return;
            }
            if (audioSource == null)
            {
                audioSource = DefaultAudioSource;
            }
            AudioClip _clip = GetAudioClip(_fileId.First());
            if (_clip == null)
            {
                return;
            }
            audioSource.clip = _clip;
            Dictionary<float, Action> actionDic = new Dictionary<float, Action> { };

            if (_fileId.Count == 1)
            {
                actionDic.Add(audioSource.clip.length, endAction);
            }
            else
            {
                actionDic.Add(audioSource.clip.length, delegate {
                    PlayAudio(_fileId, audioSource, _delayPlay, endAction);
                });
            }
            _fileId.RemoveAt(0);
            PlayAudio(_clip, audioSource, _delayPlay, actionDic);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="_fileName">声音文件名字</param>
        /// <param name="delayPlay">延迟几秒播放</param>
        /// <param name="delayCallBack">延迟几秒回调</param>
        /// <param name="action">回调事件</param>
        public AudioSource PlayAudio(string _fileName, float delayPlay = 0, float delayCallBack = 0, global::System.Action action = null)
        {
            AudioClip clip = GetAudioClip(_fileName);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {delayCallBack,action}
            };
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
            return DefaultAudioSource;
        }
        public AudioSource PlayAudio(int id, float delayPlay = 0, float delayCallBack = 0, global::System.Action action = null)
        {
            AudioClip clip = GetAudioClip(id);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {delayCallBack,action}
            };
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
            return DefaultAudioSource;
        }


        public void PlayAudio(string _fileName, AudioSource audioSource, float delayPlay = 0, float delayCallBack = 0, global::System.Action action = null)
        {
            AudioClip clip = GetAudioClip(_fileName);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {delayCallBack,action}
            };
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }


        public void PlayAudio(int id, AudioSource audioSource, float delayPlay = 0, float delayCallBack = 0, global::System.Action action = null)
        {
            AudioClip clip = GetAudioClip(id);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {delayCallBack,action}
            };
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="_fileName">音频名称</param>
        /// <param name="delayPlay">延迟播放</param>
        /// <param name="endAction">播放完事件</param>
        /// <returns></returns>
        public AudioSource PlayAudio(string _fileName, float delayPlay = 0, global::System.Action endAction = null)
        {
            AudioClip clip = GetAudioClip(_fileName);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {clip.length,endAction}
            };
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
            return DefaultAudioSource;
        }

        public AudioSource PlayAudio(int id, float delayPlay = 0, global::System.Action endAction = null)
        {
            AudioClip clip = GetAudioClip(id);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {clip.length,endAction}
            };
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
            return DefaultAudioSource;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="_filename">名称</param>
        /// <param name="audioSource">audioSource</param>
        /// <param name="delayPlay">延迟几秒播放</param>
        /// <param name="endAction">播放完事件</param>
        public void PlayAudio(string _filename, AudioSource audioSource, float delayPlay = 0, global::System.Action endAction = null)
        {
            AudioClip clip = GetAudioClip(_filename);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {clip.length,endAction}
            };
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }

        public void PlayAudio(int id, AudioSource audioSource, float delayPlay = 0, global::System.Action endAction = null)
        {
            AudioClip clip = GetAudioClip(id);
            Dictionary<float, Action> actionDic = new Dictionary<float, Action>
            {
                {clip.length,endAction}
            };
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }

        public void PlayAudio(int id, AudioSource audioSource, float delayPlay = 0, Dictionary<float, Action> actionDic = null)
        {
            AudioClip clip = GetAudioClip(id);
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }
        public void PlayAudio(string _filename, AudioSource audioSource, float delayPlay = 0, Dictionary<float, Action> actionDic = null)
        {
            AudioClip clip = GetAudioClip(_filename);
            PlayAudio(clip, audioSource, delayPlay, actionDic);
        }

        public void PlayAudio(int id, float delayPlay = 0, Dictionary<float, Action> actionDic = null)
        {
            AudioClip clip = GetAudioClip(id);
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
        }
        public void PlayAudio(string _filename, float delayPlay = 0, Dictionary<float, Action> actionDic = null)
        {
            AudioClip clip = GetAudioClip(_filename);
            PlayAudio(clip, DefaultAudioSource, delayPlay, actionDic);
        }

        public void PlayAudioByDefault(int id, float delayPlay = 0, global::System.Action endAction = null)
        {
            StartCoroutine(PlayAudioByDefaultAudioSource(id, delayPlay, endAction));
        }

        IEnumerator PlayAudioByDefaultAudioSource(int id, float delayPlay = 0, global::System.Action endAction = null)
        {
            AudioClip clip = GetAudioClip(id);
            if (clip == null)
            {
                AudioInfo audioInfo = GetAudioInfoById(id);
                string audiopath = Application.streamingAssetsPath + audioInfo.Path + ".wav";

                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audiopath, AudioType.WAV);
                yield return request.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
#else
			                if (request.isHttpError || request.isNetworkError)
#endif
                {
                    Debug.LogError($"load audio {id} fail : {request.error}");
                }
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(request);
                    audioInfo.Clip = clip;
                }
            }
            if (clip)
            {
                Dictionary<float, Action> actionDic = new Dictionary<float, Action>
                {
                  {clip.length,endAction}
                };
                PlayAudio(clip, DefaultAudioSource, 0, actionDic);
            }

            yield return null;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="audioSource">播放器</param>
        /// <param name="delayPlay">延迟播放</param>
        /// <param name="actionDic">计时事件</param>
        public void PlayAudio(AudioClip clip, AudioSource audioSource, float delayPlay, Dictionary<float, Action> actionDic)
        {
            if (clip == null)
            {
                Log.Error($"播放失败 AudioClip {clip} 为空");
                return;
            }
            if (audioSource == null)
            {
                Log.Error($"播放失败 AudioSource {audioSource} 为空");
                return;
            }
            audioSource.enabled = true;
            audioSource.clip = clip;

            //  audioSource.loop = audioSource == BGMAudioSource ? true : false;

            audioSource.PlayDelayed(delayPlay);

            if (actionDic == null || actionDic.Count == 0)
            {
                return;
            }
            AudioInfo audioInfo = GetAudioInfo(clip);
            audioInfo.AudioActionDic.Clear();
            audioInfo.TimerList.Clear();
            audioInfo.AudioActionDic.AddRange(actionDic, false);
            foreach (var item in audioInfo.AudioActionDic)
            {
                Timer timer = ComponentEntry.Timer.CreateTimer(item.Value, item.Key.ToString(), item.Key + delayPlay);
                audioInfo.TimerList.Add(timer);
            }
        }



        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="audioSourceName">播放器标识</param>
        public AudioSource Pause(string audioSourceName)
        {
            AudioSource audioSource = GetAudioSource(audioSourceName);
            audioSource.Pause();
            if (audioSource.clip == null)
            {
                return audioSource;
            }
            //需要暂停计时器
            AudioInfo audioInfo = GetAudioInfo(audioSource.clip);
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = true;
                }
            }
            return audioSource;
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="id">clip的id</param>
        public AudioSource Pause(int id)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfoById(id);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == audioInfo.Clip)
                {
                    audioSource = item;
                    audioSource.Pause();
                    break;
                }
            }
            //需要暂停计时器
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = true;
                }
            }
            return audioSource;
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <returns></returns>
        public AudioSource Pause(AudioClip clip)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfo(clip);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == clip)
                {
                    audioSource = item;
                    audioSource.Pause();
                    break;
                }
            }
            //需要暂停计时器
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = true;
                }
            }
            return audioSource;
        }

        /// <summary>
        /// 恢复播放
        /// </summary>
        /// <param name="audioSourceName">播放器标识</param>
        /// <returns></returns>
        public AudioSource UnPause(string audioSourceName)
        {
            AudioSource audioSource = GetAudioSource(audioSourceName);
            audioSource.UnPause();
            if (audioSource.clip == null)
            {
                return audioSource;
            }
            //需要恢复计时器
            AudioInfo audioInfo = GetAudioInfo(audioSource.clip);
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = false;
                }
            }
            return audioSource;
        }
        /// <summary>
        /// 恢复播放
        /// </summary>
        /// <param name="id">clip的id</param>
        public AudioSource UnPause(int id)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfoById(id);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == audioInfo.Clip)
                {
                    audioSource = item;
                    audioSource.UnPause();
                    break;
                }
            }
            //需要恢复计时器
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = false;
                }
            }
            return audioSource;
        }
        /// <summary>
        /// 恢复播放
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <returns></returns>
        public AudioSource UnPause(AudioClip clip)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfo(clip);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == clip)
                {
                    audioSource = item;
                    audioSource.UnPause();
                    break;
                }
            }
            //需要恢复计时器
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.IsPause = false;
                }
            }
            return audioSource;
        }


        public void StopDefaultAudioSource()
        {
            defaultAudioSource.Stop();
            if (defaultAudioSource.clip == null)
            {
                return;
            }
            AudioInfo audioInfo = GetAudioInfo(defaultAudioSource.clip);
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item1 in audioInfo.TimerList)
                {
                    item1.CleanUp();
                }
                audioInfo.TimerList.Clear();
            }
        }
        public void StopBGMAudioSource()
        {
            BGMAudioSource.Stop();
            if (BGMAudioSource.clip == null)
            {
                return;
            }
            AudioInfo audioInfo = GetAudioInfo(BGMAudioSource.clip);
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item1 in audioInfo.TimerList)
                {
                    item1.CleanUp();
                }
                audioInfo.TimerList.Clear();
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="audioSourceName">audioSourceName名字</param>
        public AudioSource StopAudio(string audioSourceName)
        {
            AudioSource audioSource = GetAudioSource(audioSourceName);
            audioSource.Stop();
            if (audioSource.clip == null)
            {
                return audioSource;
            }

            AudioInfo audioInfo = GetAudioInfo(audioSource.clip);
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.CleanUp();
                }
                audioInfo.TimerList.Clear();
            }
            return audioSource;
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="id">音频id</param>
        public AudioSource StopAudio(int id)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfoById(id);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == audioInfo.Clip)
                {
                    audioSource = item;
                    audioSource.Stop();
                    break;
                }
            }
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.CleanUp();
                }
                audioInfo.TimerList.Clear();
            }
            return audioSource;
        }
        public AudioSource StopAudio(AudioClip clip)
        {
            AudioSource audioSource = null;
            AudioInfo audioInfo = GetAudioInfo(clip);

            foreach (var item in mAudioSourceDic.Values)
            {
                if (item.clip != null && item.clip == clip)
                {
                    audioSource = item;
                    audioSource.Stop();

                    break;
                }
            }
            if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
            {
                foreach (var item in audioInfo.TimerList)
                {
                    item.CleanUp();
                }
                audioInfo.TimerList.Clear();
            }
            return audioSource;
        }


        /// <summary>
        /// 停止所有播放器,移除所有绑定回调
        /// </summary>
        /// <param name="isAll">是否包含default,BGM播放器</param>
        public void StopAllAudio(bool isAll = true)
        {
            foreach (var item in mAudioSourceDic.Values)
            {
                if (!isAll)
                {
                    if (item == BGMAudioSource || item == defaultAudioSource)
                    {
                        continue;
                    }
                }
                item.Stop();
                if (item.clip == null)
                {
                    continue;
                }
                AudioInfo audioInfo = GetAudioInfo(item.clip);
                if (audioInfo.TimerList != null && audioInfo.TimerList.Count > 0)
                {
                    foreach (var item1 in audioInfo.TimerList)
                    {
                        item1.CleanUp();
                    }
                    audioInfo.TimerList.Clear();
                }
            }
        }
        /// <summary>
        /// 获取指定AudioClip长度
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_fileName"></param>
        /// <returns></returns>
        public float GetAudioClipLength(string _fileName)
        {
            AudioClip _clip = GetAudioClip(_fileName);
            return _clip == null ? 0 : _clip.length;
        }

        public float GetAudioClipLength(int id)
        {
            AudioClip _clip = GetAudioClip(id);
            return _clip == null ? 0 : _clip.length;
        }

        public float GetAudioClipLength(List<string> _fileNames, float _delay = 0)
        {
            float _totalDelay = _delay;
            foreach (string _fileName in _fileNames)
            {
                _totalDelay += GetAudioClipLength(_fileName) + _delay;
            }
            return _totalDelay;
        }

        public float GetAudioClipLength(List<int> _fileId, float _delay = 0)
        {
            float _totalDelay = _delay;
            foreach (int id in _fileId)
            {
                _totalDelay += GetAudioClipLength(id) + _delay;
            }
            return _totalDelay;
        }

        /// <summary>
        /// 获取默认AudioSource
        /// </summary>
        /// <returns></returns>
        AudioSource GetDefaultAudioSource()
        {
            AudioSource audioSource = GetAudioSource("DefaultAudioSource");
            if (audioSource == null)
            {
                GameObject go = new GameObject("DefaultAudioSource");
                audioSource = go.AddComponent<AudioSource>();
                audioSource.volume = Volume;
                go.transform.SetParent(transform);
                AddAudioSourse("DefaultAudioSource", audioSource);
            }
            return audioSource;
        }
        /// <summary>
        /// 获取BGMAudioSource
        /// </summary>
        /// <returns></returns>
        AudioSource GetBGMAudioSource()
        {
            AudioSource audioSource = GetAudioSource("BGMAudioSource");
            if (audioSource == null)
            {
                GameObject go = new GameObject("BGMAudioSource");
                audioSource = go.AddComponent<AudioSource>();
                audioSource.volume = VolumeBGM;
                audioSource.loop = true;
                go.transform.SetParent(transform);
                AddAudioSourse("BGMAudioSource", audioSource);
            }
            return audioSource;
        }

        /// <summary>
        /// 获取指定AudioSource
        /// </summary>
        /// <param name="outName"></param>
        /// <returns></returns>
        public AudioSource GetAudioSource(string outName)
        {
            return mAudioSourceDic.GetValue(outName, null);
        }

        /// <summary>
        /// 创建一个AudioSource
        /// </summary>
        /// <param name="outName"></param>
        public void CreateAudioSource(string outName, Transform parent = null)
        {
            GameObject go = new GameObject(outName);
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.volume = mVolume;
            if (parent == null)
            {
                parent = transform;
            }
            go.transform.SetParent(parent);
            AddAudioSourse(outName, audioSource);
        }
        /// <summary>
        /// 将指定AudioSource移除缓存
        /// </summary>
        /// <param name="outName"></param>
        /// <returns>被移除的AudioSource</returns>
        public AudioSource RemoveAudioSource(string outName)
        {
            AudioSource audioSource;
            if (mAudioSourceDic.TryGetValue(outName, out audioSource))
            {
                mAudioSourceDic.Remove(outName);
            }
            return audioSource;
        }
        /// <summary>
        /// 将指定AudioSource移除缓存
        /// </summary>
        /// <param name="audioSource"></param>
        public void RemoveAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                return;
            }
            string key = "";
            if (mAudioSourceDic.TryGetKey(audioSource, ref key))
            {
                mAudioSourceDic.Remove(key);
            }
        }
        /// <summary>
        /// 将指定AudioSource加入缓存
        /// </summary>
        /// <param name="outName"></param>
        /// <param name="audioSource"></param>
        public void AddAudioSourse(string outName, AudioSource audioSource)
        {
            if (audioSource == null || outName == "")
            {
                Log.Error($"缓存失败AudioSource标识或者AudioSource为空");
                return;
            }
            mAudioSourceDic.TryAdd(outName, audioSource);
        }

        /// <summary>
        /// 获取AudioClip
        /// </summary>
        /// <param name="_fileName">文件名</param>
        /// <returns></returns>
        public AudioClip GetAudioClip(string _fileName)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                Debug.LogError($"GetAudioClip: 音频名不可为空");
                return null;
            }

            AudioClip clip = null;
            clip = AudioInfoDicById.FirstOrDefault(a => a.Value.Clip.name.Equals(_fileName)).Value.Clip;
            if (!clip)
            {
                Log.Warning($"GetAudioClip:音频 {_fileName} does not exist");
            }
            return clip;
        }

        /// <summary>
        /// 获取AudioClip
        /// </summary>
        /// <param name="_fileName">文件名</param>
        /// <returns></returns>
        public AudioClip GetAudioClip(int id)
        {

            AudioClip clip = null;
            if (AudioInfoDicById.ContainsKey(id))
            {
                clip = AudioInfoDicById[id].Clip;
            }
            else
            {
                Log.Error($"GetAudioClip:音频 Id {id} does not exist");
            }
            return clip;
        }
        public void AddAudioInfo(AudioInfo audioInfo)
        {

            AudioInfoDicById.TryAdd(audioInfo.Id, audioInfo);
        }

        public AudioInfo GetAudioInfoById(int id)
        {
            return AudioInfoDicById.GetValue(id, null);
        }
        public AudioInfo GetAudioInfo(AudioClip clip)
        {
            AudioInfo audioInfo = null;

            foreach (var item in AudioInfoDicById)
            {
                if (item.Value.Clip == clip)
                {
                    audioInfo = item.Value;
                    break;
                }
            }
            return audioInfo;
        }
        public AudioInfo RemoveAudioInfo(string audioName)
        {
            AudioInfo audioInfo = null;
            audioInfo = AudioInfoDicById.FirstOrDefault(a => a.Value.AudioName.Equals(audioName)).Value;
            if (audioInfo != null)
            {
                AudioInfoDicById.Remove(audioInfo.Id);
            }
            return audioInfo;
        }
        public AudioInfo RemoveAudioInfo(int id)
        {
            AudioInfo audioInfo = null;
            if (AudioInfoDicById.ContainsKey(id))
            {
                audioInfo = AudioInfoDicById[id];
                AudioInfoDicById.Remove(id);
            }
            return audioInfo;
        }
        public void RemoveAudioInfo(AudioInfo audioInfo)
        {
            if (AudioInfoDicById.ContainsKey(audioInfo.Id))
            {
                AudioInfoDicById.Remove(audioInfo.Id);
            }
        }
        public string GetAudioTextContent(string audioName)
        {
            AudioInfo audioInfo;

            audioInfo = AudioInfoDicById.FirstOrDefault(a => a.Value.AudioName.Equals(audioName)).Value;
            if (audioInfo != null)
            {
                return audioInfo.TextContent;
            }

            return "";
        }
        public string GetAudioTextContent(int id)
        {
            if (!AudioInfoDicById.ContainsKey(id))
            {
                return null;
            }
            AudioInfo audioInfo;
            audioInfo = AudioInfoDicById[id];
            return audioInfo.TextContent;
        }
        public void GetAudioTextContent(string audioName, ref string textContent)
        {
            AudioInfo audioInfo;

            audioInfo = AudioInfoDicById.FirstOrDefault(a => a.Value.AudioName.Equals(audioName)).Value;
            if (audioInfo != null)
            {
                textContent = audioInfo.TextContent;
            }
        }

        public void ClearAll()
        {
            mAudioSourceDic.Clear();

            AudioInfoDicById.Clear();
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            ClearAll();
        }
        private void OnApplicationQuit()
        {
            ClearAll();
        }

        //------------------


        /// <summary>
        /// 是否存在指定声音组。
        /// </summary>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasAudioGroup(string audioGroupName)
        {
            return m_AudioManager.HasAudioGroup(audioGroupName);
        }

        /// <summary>
        /// 获取指定声音组。
        /// </summary>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public IAudioGroup GetAudioGroup(string audioGroupName)
        {
            return m_AudioManager.GetAudioGroup(audioGroupName);
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <returns>所有声音组。</returns>
        public IAudioGroup[] GetAllAudioGroups()
        {
            return m_AudioManager.GetAllAudioGroups();
        }

        /// <summary>
        /// 获取所有声音组。
        /// </summary>
        /// <param name="results">所有声音组。</param>
        public void GetAllAudioGroups(List<IAudioGroup> results)
        {
            m_AudioManager.GetAllAudioGroups(results);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="audioAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddAudioGroup(string audioGroupName, int audioAgentHelperCount)
        {
            return AddAudioGroup(audioGroupName, false, false, 1f, audioAgentHelperCount);
        }

        /// <summary>
        /// 增加声音组。
        /// </summary>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="audioGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="audioGroupMute">声音组是否静音。</param>
        /// <param name="audioGroupVolume">声音组音量。</param>
        /// <param name="audioAgentHelperCount">声音代理辅助器数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddAudioGroup(string audioGroupName, bool audioGroupAvoidBeingReplacedBySamePriority, bool audioGroupMute, float audioGroupVolume, int audioAgentHelperCount)
        {
            if (m_AudioManager.HasAudioGroup(audioGroupName))
            {
                return false;
            }

            AudioGroupHelperBase audioGroupHelper = Helper.CreateHelper(m_AudioGroupHelperTypeName, m_CustomAudioGroupHelper, AudioGroupCount);
            if (audioGroupHelper == null)
            {
                Log.Error("Can not create audio group helper.");
                return false;
            }

            audioGroupHelper.name = Framework.Utility.Text.Format("Audio Group - {0}", audioGroupName);
            Transform transform = audioGroupHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;



            if (!m_AudioManager.AddAudioGroup(audioGroupName, audioGroupAvoidBeingReplacedBySamePriority, audioGroupMute, audioGroupVolume, audioGroupHelper))
            {
                return false;
            }

            for (int i = 0; i < audioAgentHelperCount; i++)
            {
                if (!AddAudioAgentHelper(audioGroupName, audioGroupHelper, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingAudioSerialIds()
        {
            return m_AudioManager.GetAllLoadingAudioSerialIds();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号。</param>
        public void GetAllLoadingAudioSerialIds(List<int> results)
        {
            m_AudioManager.GetAllLoadingAudioSerialIds(results);
        }

        /// <summary>
        /// 是否正在加载声音。
        /// </summary>
        /// <param name="serialId">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingAudio(int serialId)
        {
            return m_AudioManager.IsLoadingAudio(serialId);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName)
        {
            return PlayAudio(audioAssetName, audioGroupName, DefaultPriority, null, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority)
        {
            return PlayAudio(audioAssetName, audioGroupName, priority, null, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, PlayAudioParams playAudioParams)
        {
            return PlayAudio(audioAssetName, audioGroupName, DefaultPriority, playAudioParams, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, Entity bindingEntity)
        {
            return PlayAudio(audioAssetName, audioGroupName, DefaultPriority, null, bindingEntity, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, Vector3 worldPosition)
        {
            return PlayAudio(audioAssetName, audioGroupName, DefaultPriority, null, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, object userData)
        {
            return PlayAudio(audioAssetName, audioGroupName, DefaultPriority, null, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams)
        {
            return PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, null, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams, object userData)
        {
            return PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, null, userData);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams, Entity bindingEntity)
        {
            return PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, bindingEntity, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="bindingEntity">声音绑定的实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams, Entity bindingEntity, object userData)
        {
            return m_AudioManager.PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, PlayAudioInfo.Create(bindingEntity, Vector3.zero, userData));
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams, Vector3 worldPosition)
        {
            return PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, worldPosition, null);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="audioAssetName">声音资源名称。</param>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="priority">加载声音资源的优先级。</param>
        /// <param name="playAudioParams">播放声音参数。</param>
        /// <param name="worldPosition">声音所在的世界坐标。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlayAudio(string audioAssetName, string audioGroupName, int priority, PlayAudioParams playAudioParams, Vector3 worldPosition, object userData)
        {
            return m_AudioManager.PlayAudio(audioAssetName, audioGroupName, priority, playAudioParams, PlayAudioInfo.Create(null, worldPosition, userData));
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        //public bool StopAudio(int serialId)
        //{
        //    return m_AudioManager.StopAudio(serialId);
        //}

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        /// <param name="serialId">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopAudio(int serialId, float fadeOutSeconds)
        {
            return m_AudioManager.StopAudio(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        public void StopAllLoadedAudios()
        {
            m_AudioManager.StopAllLoadedAudios();
        }

        /// <summary>
        /// 停止所有已加载的声音。
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllLoadedAudios(float fadeOutSeconds)
        {
            m_AudioManager.StopAllLoadedAudios(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音。
        /// </summary>
        public void StopAllLoadingAudios()
        {
            m_AudioManager.StopAllLoadingAudios();
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        public void PauseAudio(int serialId)
        {
            m_AudioManager.PauseAudio(serialId);
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        /// <param name="serialId">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseAudio(int serialId, float fadeOutSeconds)
        {
            m_AudioManager.PauseAudio(serialId, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        public void ResumeAudio(int serialId)
        {
            m_AudioManager.ResumeAudio(serialId);
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        /// <param name="serialId">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public void ResumeAudio(int serialId, float fadeInSeconds)
        {
            m_AudioManager.ResumeAudio(serialId, fadeInSeconds);
        }

        /// <summary>
        /// 增加声音代理辅助器。
        /// </summary>
        /// <param name="audioGroupName">声音组名称。</param>
        /// <param name="audioGroupHelper">声音组辅助器。</param>
        /// <param name="index">声音代理辅助器索引。</param>
        /// <returns>是否增加声音代理辅助器成功。</returns>
        private bool AddAudioAgentHelper(string audioGroupName, AudioGroupHelperBase audioGroupHelper, int index)
        {
            AudioAgentHelperBase audioAgentHelper = Helper.CreateHelper(m_AudioAgentHelperTypeName, m_CustomAudioAgentHelper, index);
            if (audioAgentHelper == null)
            {
                Log.Error("Can not create audio agent helper.");
                return false;
            }

            audioAgentHelper.name = Framework.Utility.Text.Format("Audio Agent Helper - {0} - {1}", audioGroupName, index.ToString());
            Transform transform = audioAgentHelper.transform;
            transform.SetParent(audioGroupHelper.transform);
            transform.localScale = Vector3.one;

            m_AudioManager.AddAudioAgentHelper(audioGroupName, audioAgentHelper);

            return true;
        }


        private void OnPlayAudioSuccess(object sender, Framework.Audio.PlayAudioSuccessEventArgs e)
        {
            PlayAudioInfo playAudioInfo = (PlayAudioInfo)e.UserData;
            if (playAudioInfo != null)
            {
                AudioAgentHelperBase audioAgentHelper = (AudioAgentHelperBase)e.AudioAgent.Helper;
                if (playAudioInfo.BindingEntity != null)
                {
                    audioAgentHelper.SetBindingEntity(playAudioInfo.BindingEntity);
                }
                else
                {
                    audioAgentHelper.SetWorldPosition(playAudioInfo.WorldPosition);
                }
            }

            m_EventComponent.Fire(this, PlayAudioSuccessEventArgs.Create(e));
        }

        private void OnPlayAudioFailure(object sender, Framework.Audio.PlayAudioFailureEventArgs e)
        {
            string logMessage = Framework.Utility.Text.Format("Play audio failure, asset name '{0}', audio group name '{1}', error code '{2}', error message '{3}'.", e.AudioAssetName, e.AudioGroupName, e.ErrorCode.ToString(), e.ErrorMessage);
            if (e.ErrorCode == PlayAudioErrorCode.IgnoredDueToLowPriority)
            {
                Log.Info(logMessage);
            }
            else
            {
                Log.Warning(logMessage);
            }

            m_EventComponent.Fire(this, PlayAudioFailureEventArgs.Create(e));
        }

        private void OnPlayAudioUpdate(object sender, Framework.Audio.PlayAudioUpdateEventArgs e)
        {
            m_EventComponent.Fire(this, PlayAudioUpdateEventArgs.Create(e));
        }

        private void OnPlayAudioDependencyAsset(object sender, Framework.Audio.PlayAudioDependencyAssetEventArgs e)
        {
            m_EventComponent.Fire(this, PlayAudioDependencyAssetEventArgs.Create(e));
        }





        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        private void OnSceneUnloaded(Scene scene)
        {
        }
    }

}

