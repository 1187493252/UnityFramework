/*
* FileName:          TaskTargetConfig
* CompanyName:       
* Author:            
* Description:       任务目标，不分先后顺序,须有判断成功完成目标或者错误完成目标的功能
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityFramework.Runtime
{

    public class TaskTargetConfig : ScriptableObject
    {

        [Header("任务目标描述")]
        public string TargetName;
        [Header("目标模式")]
        public TaskMode TargetMode = TaskMode.Default;

        /// <summary>
        /// 对应的任务
        /// </summary>
        protected TaskConfig targetTask;

        /// <summary>
        /// 任务目标优先级
        /// </summary>
        public int priority;

        /// <summary>
        /// 任务成功/失败
        /// </summary>
        protected bool isSuccess = false;



        private bool hadDone = false;
        /// <summary>
        /// 是否完成,不考虑Success
        /// </summary>
        public bool HadDone
        {
            get => hadDone; set => hadDone = value;
        }



        /// <summary>
        /// 是否自动播放音频
        /// </summary>
        public bool isAutoPlayStartAudio = true;

        /// <summary>
        /// 是否重新播放音频
        /// </summary>
        public bool isReplayStartAudio = true;

        /// <summary>
        /// 提示培训模式任务目标开始语音
        /// </summary>
        [Tooltip("提示培训模式任务目标开始语音")]
        public List<string> StudyStartAudioKeys;
        public List<string> StudyEndAudioKeys;
        public List<string> StudySuccessAudioKeys;
        public List<string> StudyErrorAudioKeys;

        /// <summary>
        /// 提示考试模式任务目标开始语音
        /// </summary>
        [Tooltip("提示考试模式任务目标开始语音")]
        public List<string> ExamStartAudioKeys;
        public List<string> ExamEndAudioKeys;
        public List<string> ExamSuccessAudioKeys;
        public List<string> ExamErrorAudioKeys;

        /// <summary>
        /// 是否设置倒计时
        /// </summary>
        [Header("是否设置倒计时")]
        public bool isSetCountDown = false;
        /// <summary>
        /// 倒计时时间
        /// </summary>
        [Header("倒计时时间")]
        public int countDownTime = 60;
        /// <summary>
        /// 倒计时结束提示
        /// </summary>
        public string TimeOutHint;
        protected Timer countDownTimer;

        public Action<bool> OnNotifyResult;

        /// <summary>
        /// 是否最后的任务目标，如果被先执行，则此任务失败
        /// </summary>
        public bool isFinal = false;


        private Timer timer;

        /// <summary>
        /// 任务初始化
        /// </summary>
        public virtual void OnTaskInit(TaskConfig taskConfig)
        {
            isSuccess = false;
            HadDone = false;
            targetTask = taskConfig;
            countDownTimer = ComponentEntry.Timer.DestroyTimer(countDownTimer);
            timer = ComponentEntry.Timer.DestroyTimer(timer);

        }

        /// <summary>
        /// 任务开始
        /// </summary>
        public virtual void OnTaskStart(TaskConfig taskConfig)
        {
            OnTargetStart();
        }

        /// <summary>
        /// 任务持续中
        /// </summary>
        public virtual void OnTaskDoing(TaskConfig taskConfig)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                NotifyResult(true);

            }
        }


        /// <summary>
        /// 任务结束 不论任务的结果正确/出错/跳过
        /// </summary>
        public virtual void OnTaskEnd(TaskConfig taskConfig)
        {
            HadDone = true;
            PlayEndAudio();
            countDownTimer = ComponentEntry.Timer.DestroyTimer(countDownTimer);
            timer = ComponentEntry.Timer.DestroyTimer(timer);
        }

        /// <summary>
        /// 强制完成任务
        /// 检测并记录目标是否完成
        /// </summary>
        public virtual void ForceCompleteTask(TaskConfig taskConf)
        {
            HadDone = true;
            isSuccess = false;
        }

        public virtual void OnTargetStart()
        {
            isSuccess = false;
            HadDone = false;
            if (TargetMode != ComponentEntry.Task.TaskMode && TargetMode != TaskMode.Default)
            {
                HadDone = true;
                isSuccess = true;
                targetTask.NotifyResult(true);
            }
            AutoPlayStartAudio();
        }



        /// <summary>
        /// 强制提示,任务未完成等
        /// </summary>
        public virtual void ForceTip()
        {

        }

        public void AutoPlayStartAudio()
        {
            if (isAutoPlayStartAudio)
            {
                PlayStartAudio();
            }
        }


        public void PlayStartAudio()
        {

            if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
            {
                if (TargetMode != TaskMode.Study)
                {
                    PlayAudios(ExamStartAudioKeys);
                }
            }
            else if (ComponentEntry.Task.TaskMode == TaskMode.Study)
            {
                if (TargetMode != TaskMode.Exam)
                {
                    PlayAudios(StudyStartAudioKeys);
                }
            }
        }

        public void PlayEndAudio()
        {

            if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
            {
                if (TargetMode != TaskMode.Study)
                {
                    PlayAudios(ExamEndAudioKeys);
                }
            }
            else if (ComponentEntry.Task.TaskMode == TaskMode.Study)
            {
                if (TargetMode != TaskMode.Exam)
                {
                    PlayAudios(StudyEndAudioKeys);
                }
            }
        }

        /// <summary>
        /// 任务开始语音播放完毕
        /// </summary>
        /// <param name="taskConf"></param>
        public virtual void OnTaskPlayStartAudioComplete(TaskConfig taskConf)
        {
            if (isSetCountDown)
            {
                if (!HadDone && targetTask.TaskState.Equals(TaskState.Doing))
                {
                    Debug.Log("任务开始计时: " + TargetName);
                    countDownTimer = ComponentEntry.Timer.CreateTimer(OnTaskTimeOut, countDownTime, 1);
                }
            }
        }

        /// <summary>
        /// 任务超时
        /// </summary>
        protected virtual void OnTaskTimeOut()
        {
            countDownTimer = ComponentEntry.Timer.DestroyTimer(countDownTimer);
            PlayAudios(TimeOutHint);
        }



        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后将goal设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void SetTargetState(bool issuccess, bool hasDone = true)
        {
            this.HadDone = hasDone;
            this.isSuccess = issuccess;
        }

        public bool CheckResult()
        {
            return isSuccess;
        }

        /// <summary>
        /// 发送结果
        /// </summary>
        /// <param name="success">是否成功完成任务目标</param>
        public virtual void NotifyResult(bool success)
        {
            if (!targetTask)
            {
                return;
            }
            if (isSuccess)
            {
                return;
            }
            Log.Info("{0}|任务目标:{1}", this.name, success ? "成功" : "失败");

            HadDone = true;
            isSuccess = success;
            _OnNotifyResult(isSuccess);


        }

        void _OnNotifyResult(bool success)
        {

            float _delayTime = 0;

            if (!success)
            {
                if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
                {
                    PlayAudios(ExamErrorAudioKeys);
                    _delayTime = GetAudioLength(ExamErrorAudioKeys);

                }
                else if (ComponentEntry.Task.TaskMode == TaskMode.Study)
                {
                    PlayAudios(StudyErrorAudioKeys);
                    _delayTime = GetAudioLength(StudyErrorAudioKeys);
                }
            }
            else
            {
                if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
                {
                    PlayAudios(ExamSuccessAudioKeys);
                    _delayTime = GetAudioLength(ExamSuccessAudioKeys);

                }
                else if (ComponentEntry.Task.TaskMode == TaskMode.Study)
                {
                    PlayAudios(StudySuccessAudioKeys);
                    _delayTime = GetAudioLength(StudySuccessAudioKeys);
                }
            }

            OnNotifyResult?.Invoke(success);

            ComponentEntry.Timer.CreateTimer(() =>
                  {
                      targetTask.NotifyResult(success);

                  }, _delayTime);


        }
        protected bool BeforeCheck()
        {
            //已完成不再检测
            if (isSuccess)
                return false;

            if (ComponentEntry.Task.CurrentTask != targetTask)
            {
                return false;
            }

            if (ComponentEntry.Task.CurrentTask == targetTask &&
                    (targetTask.TaskState == TaskState.End))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 跳步 往前跳
        /// </summary>
        public virtual void JumpForward(TaskConfig taskConfig)
        {

        }

        /// <summary>
        /// 跳步 往后跳
        /// </summary>
        public virtual void JumpBack(TaskConfig taskConfig)
        {

        }



        public float GetAudioLength(string key)
        {
            return ComponentEntry.Audio.GetAudioClipLength(key);
        }
        public float GetAudioLength(List<string> audiokeys)
        {
            return ComponentEntry.Audio.GetAudioClipLength(audiokeys);
        }
        public void PlayAudios(string audiokeys, Action endAction = null)
        {
            ComponentEntry.Audio.PlayAudio(audiokeys, endAction: endAction);
        }
        public void PlayAudios(List<string> audiokeys, Action endAction = null)
        {
            ComponentEntry.Audio.PlayAudio(audiokeys, endAction: endAction);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskTargetConfig", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskTargetConfig>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskTargetConfig).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskTargetConfig));
                        index++;
                    } while (obj1);
                    AssetDatabase.CreateAsset(asset, confName);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;
                }
            }
        }

#endif


    }
}