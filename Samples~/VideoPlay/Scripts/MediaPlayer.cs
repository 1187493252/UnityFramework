using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace K_UnityGF
{
    /// <summary>
    /// 媒体播放器
    /// </summary>
    public class MediaPlayer : MonoBehaviour
    {
        [Header("视频播放器")]
        public VideoPlayer videoPlayer;

        public Overlay overlay;

        [Header("按钮-屏幕触碰")]
        public Button button_ScreenTouch;

        [Header("按钮-播放状态")]
        public Button button_PlayState;

        [Header("按钮-静音")]
        public Button button_Mute;

        [Header("滑动条-音量")]
        public Slider slider_Volume;

        [Header("文本框-时间")]
        public Text text_TimeDuration;

        [Header("滑动条-播放进度")]
        public Slider slider_PlayProgress;

        [Header("视频路径")]
        public string videoPath;

        [Header("游戏运行后播放")]
        public bool playOnAwake;

        private void Start()
        {
            button_ScreenTouch.onClick.AddListener(SetPlayState);
            button_PlayState.onClick.AddListener(SetPlayState);
            button_Mute.onClick.AddListener(SetMuteState);
            slider_Volume.onValueChanged.AddListener(SetVolume);

            EventTrigger eventTrigger = slider_PlayProgress.GetComponent<EventTrigger>();

            eventTrigger.AddListener(EventTriggerType.Drag, ProgressDrag);
            eventTrigger.AddListener(EventTriggerType.PointerDown, ProgressDrag);
            eventTrigger.AddListener(EventTriggerType.PointerUp, ProgressEndDrag);

            if (playOnAwake) { VideoPlay(videoPath); }
        }

        private void Update()
        {
            //在播放状态时 实时更新视频信息
            if (videoPlayer.isPlaying)
            {
                UpdateVideoInfo();
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="videoPath">视频路径</param>
        /// <param name="loop">是否循环</param>
        public void VideoPlay(string videoPath, bool loop = false)
        {
            videoPlayer.url = Application.streamingAssetsPath + "/Videos/" + videoPath;
            videoPlayer.isLooping = loop;

            button_PlayState.image.material.DisableKeyword("UI_PLAY");
            button_PlayState.image.material.EnableKeyword("UI_PAUSE");
            button_Mute.image.material.SetFloat("_Mute", 0);
            button_Mute.image.material.SetFloat("_Volume", 1f);

            videoPlayer.SetDirectAudioMute(0, false);
            videoPlayer.Play();
        }

        /// <summary>
        /// 进度条拖拽
        /// </summary>
        private void ProgressDrag()
        {
            button_PlayState.image.material.DisableKeyword("UI_PAUSE");
            button_PlayState.image.material.EnableKeyword("UI_PLAY");

            videoPlayer.Pause();

            UpdateProgress(slider_PlayProgress.value);
        }

        /// <summary>
        /// 进度条结束拖拽
        /// </summary>
        private void ProgressEndDrag()
        {
            button_PlayState.image.material.DisableKeyword("UI_PLAY");
            button_PlayState.image.material.EnableKeyword("UI_PAUSE");

            videoPlayer.Play();
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgress(float value)
        {
            float videoLength = videoPlayer.frameCount / videoPlayer.frameRate;
            videoPlayer.frame = (long)(videoLength * value * videoPlayer.frameRate);
            float currenLength = (videoPlayer.frame / videoPlayer.frameRate) + 1;
            string totleTimeStr = GetTimeString(videoLength);
            string currentTimeStr = GetTimeString(currenLength);
            text_TimeDuration.text = string.Format("{0} / {1}", currentTimeStr, totleTimeStr);
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        private void UpdateVideoInfo()
        {
            float videoLength = videoPlayer.frameCount / videoPlayer.frameRate;
            float currenLength = (videoPlayer.frame / videoPlayer.frameRate) + 1;
            string totleTimeStr = GetTimeString(videoLength);
            string currentTimeStr = GetTimeString(currenLength);

            float progress = currenLength / videoLength;
            slider_PlayProgress.value = progress;

            text_TimeDuration.text = string.Format("{0} / {1}", currentTimeStr, totleTimeStr);
        }

        /// <summary>
        /// 设置播放状态
        /// </summary>
        public void SetPlayState() { if (videoPlayer.isPlaying) { Pause(); } else { Play(); } }

        /// <summary>
        /// 设置静音状态
        /// </summary>
        public void SetMuteState() { if (videoPlayer.GetDirectAudioMute(0)) { UnMute(); } else { Mute(); } }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="volumeValue">音量值</param>
        public void SetVolume(float volumeValue)
        {
            videoPlayer.SetDirectAudioVolume(0, volumeValue);
            button_Mute.image.material.SetFloat("_Volume", volumeValue);

            if (volumeValue>0)
            {
                videoPlayer.SetDirectAudioMute(0, false);
                button_Mute.image.material.SetFloat("_Mute", 0);
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void Play()
        {
            UnMute();
            overlay.TriggerFeedback(Overlay.Feedback.Play);

            button_PlayState.image.material.DisableKeyword("UI_PLAY");
            button_PlayState.image.material.EnableKeyword("UI_PAUSE");

            videoPlayer.Play();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void Pause()
        {
            overlay.TriggerFeedback(Overlay.Feedback.Pause);

            button_PlayState.image.material.DisableKeyword("UI_PAUSE");
            button_PlayState.image.material.EnableKeyword("UI_PLAY");

            videoPlayer.Pause();
        }

        /// <summary>
        /// 静音
        /// </summary>
        private void Mute()
        {
            videoPlayer.SetDirectAudioMute(0, true);

            overlay.TriggerFeedback(Overlay.Feedback.VolumeMute);
            button_Mute.image.material.SetFloat("_Mute", 1f);
            button_Mute.image.material.SetFloat("_Volume", 1f);
        }

        /// <summary>
        /// 非静音
        /// </summary>
        private void UnMute()
        {
            videoPlayer.SetDirectAudioMute(0, false);

            overlay.TriggerFeedback(Overlay.Feedback.VolumeUp);
            button_Mute.image.material.SetFloat("_Mute", 0);
            button_Mute.image.material.SetFloat("_Volume", 1f);
        }

        private string GetTimeString(double timeSeconds, bool showMilliseconds = false)
        {
            float totalSeconds = (float)timeSeconds;
            int hours = Mathf.FloorToInt(totalSeconds / (60f * 60f));
            float usedSeconds = hours * 60f * 60f;

            int minutes = Mathf.FloorToInt((totalSeconds - usedSeconds) / 60f);
            usedSeconds += minutes * 60f;

            int seconds = Mathf.FloorToInt(totalSeconds - usedSeconds);

            string result;
            if (hours <= 0)
            {
                if (showMilliseconds)
                {
                    int milliSeconds = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
                    result = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
                }
                else
                {
                    result = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            }
            else
            {
                if (showMilliseconds)
                {
                    int milliSeconds = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
                    result = string.Format("{2}:{0:00}:{1:00}:{3:000}", minutes, seconds, hours, milliSeconds);
                }
                else
                {
                    result = string.Format("{2}:{0:00}:{1:00}", minutes, seconds, hours);
                }
            }

            return result;
        }
    }
}
