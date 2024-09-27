/*
* FileName:          AudioManager
* CompanyName:  
* Author:            relly
* Description:       Audio管理脚本,主要功能:PlayAudio,StopAudio,GetAudioSource,GetAudioClip等
*                    
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public partial class AudioComponent : UnityFrameworkComponent
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
                actionDic.Add(audioSource.clip.length, delegate
                {
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
                actionDic.Add(audioSource.clip.length, delegate
                {
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
            ClearAll();
        }
        private void OnApplicationQuit()
        {
            ClearAll();
        }

    }

}

