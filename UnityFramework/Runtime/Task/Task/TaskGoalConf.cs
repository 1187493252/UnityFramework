#if VIU_STEAMVR_2_0_0_OR_NEWER

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;



namespace UnityFramework.Runtime.Task
{

    //任务目标，不分先后顺序,须有判断成功完成目标或者错误完成目标的功能
    public class TaskGoalConf : ScriptableObject
    {
        /// <summary>
        /// 任务目标描述
        /// </summary>
        public string GoalName;
        public TaskType GoalMode;
        /// <summary>
        /// 任务目标优先级
        /// </summary>
        public int Priority;
        protected TaskConf targetTask;
        protected bool isAchieveGoal = false;
        public bool isAutoPlayStartAudio = true;

        [Header("是否重复播放考试或学习的开始语音")]
        public bool isRepeatPlay = false;
        public float RepeatTime = 30;
        public int RepeatNum = 3;

        Timer timerOfRepeatPlay;

        //培训模式语音 
        /// <summary>
        /// 提示培训模式任务目标开始语音
        /// </summary>
        [Tooltip("提示培训模式任务目标开始语音")]
        public List<string> goalTrainingStartAudioKeys;
        public List<string> goalTrainingErrorAudioKeys;
        /// <summary>
        /// 提示考试模式任务目标开始语音
        /// </summary>
        [Tooltip("提示考试模式任务目标开始语音")]
        public List<string> goalExaminationStartAudioKeys;
        public List<string> goalExaminationErrorAudioKeys;



        public Action<bool> OnAchieveGoal;
        public float playDeltTime;
        /// <summary>
        /// 是否最后的任务目标，如果被先执行，则此任务失败
        /// </summary>
        public bool isFinalGoal = false;





        [Header("考核限时")]
        public bool examinaLimit = false;
        [Tooltip("当前Goal超时时是否跳过此任务")]
        public bool goalFailureJumpTask = false;
        public float taskTime = 120;
        /// <summary>
        /// 考核计时器
        /// </summary>
        protected Timer examTimer;

        //强制提醒,比如任务没做
        public virtual void ForceTip()
        {

        }
        [Tooltip("是否为不计算类型Goal 不计算的默认全部自动完成 不算入已执行状态")]
        public bool notCalculate = false;

        private bool hasDone = false;

        public bool HasDone { get => notCalculate ? true : hasDone; set => hasDone = value; }


        /// <summary>
        /// 任务目标初始化
        /// </summary>
        /// <param name="taskConf"></param>
        public virtual void OnTaskInit(TaskConf taskConf)
        {

            isAchieveGoal = false;
            HasDone = false;

            targetTask = taskConf;


            timerOfRepeatPlay = ComponentEntry.Timer.DestroyTimer(timerOfRepeatPlay);


        }

        protected void PlayAudios(List<string> audios)
        {

        }
        public void PlayAudios(List<string> audios, Action playEnd = null, float delt = 0, Action<float> playBackTime = null)
        {

        }
        public void PlayAudios(string audios, Action playEnd = null, float delt = 0, Action<float> playBackTime = null)
        {

        }
        public void StopAllAudios()
        {


        }
        public void PlayStartAudio()
        {
            List<string> audioKeys = new List<string>();
            if (TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                if (GoalMode != TaskType.OnlyStudy)
                    audioKeys = goalExaminationStartAudioKeys;
            }
            else if (TaskManager.Instance.TaskMode == TaskMode.Study)
            {
                if (GoalMode != TaskType.OnlyExam)
                    audioKeys = goalTrainingStartAudioKeys;
            }




            PlayAudios(audioKeys);
            //创建循环语音
            timerOfRepeatPlay = ComponentEntry.Timer.DestroyTimer(timerOfRepeatPlay);

            if (isRepeatPlay && timerOfRepeatPlay == null)
            {
                if (TaskManager.Instance.TaskMode == TaskMode.Exam)
                    timerOfRepeatPlay = ComponentEntry.Timer.CreateTimer(() => { PlayAudios(audioKeys); }, "", RepeatTime, RepeatNum);
                else
                    timerOfRepeatPlay = ComponentEntry.Timer.CreateTimer(() => { PlayAudios(audioKeys); }, "", RepeatTime, -1);
            }


        }



        /// <summary>
        /// 任务开始
        /// </summary>
        /// <param name="taskConf"></param>
        public virtual void OnTaskStart(TaskConf taskConf)
        {
            HasDone = false;
            isAchieveGoal = false;
            if (examinaLimit && TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                examTimer = ComponentEntry.Timer.CreateTimer(delegate
                {
                    if (!HasDone)
                    {
                        //完成任务不能放在语音播放的回调里 会被后面的顶掉
                        //    PlayAudios("LT_01_08");
                        ComponentEntry.Timer.CreateTimer(() =>
                        {
                            if (!HasDone)
                            {
                                if (goalFailureJumpTask) //当前goal标记为超时整个任务失败
                                    TaskManager.Instance.CurrentTask.AchieveGoal(false, true);
                                else
                                    AchieveGoal(false);
                            }
                        }, 2f);

                    }
                }, taskTime);
            }




            if (TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                if (GoalMode == TaskType.OnlyStudy)
                {
                    HasDone = true;
                    isAchieveGoal = true;
                    targetTask.AchieveGoal(true);
                }
            }
            else
            {
                if (GoalMode == TaskType.OnlyExam)
                {
                    HasDone = true;
                    isAchieveGoal = true;
                    targetTask.AchieveGoal(true);
                }
            }


        }


        /// <summary>
        /// 任务中一直需要监测的事件：比如手柄
        /// </summary>
        /// <param name="targetTask"></param>
        public virtual void OnTaskDoing(TaskConf taskConf)
        {

        }

        /// <summary>
        /// !!!注意 子类一定要最后使用base.OnTaskEnd()
        /// 一定要放在自己业务逻辑后面 不然部分数据会提前销毁 造成逻辑丢失
        /// 任务结束时需要的处理
        /// </summary>
        /// <param name="taskConf"></param>
        public virtual void OnTaskEnd(TaskConf taskConf)
        {
            if (examinaLimit && TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                if (examTimer != null)
                {
                    examTimer = ComponentEntry.Timer.DestroyTimer(examTimer);
                }
            }
            HasDone = true;
            timerOfRepeatPlay = ComponentEntry.Timer.DestroyTimer(timerOfRepeatPlay);
        }

        /// <summary>
        /// 完成任务目标发送
        /// </summary>
        /// <param name="success">是否成功完成任务目标</param>
        public virtual void AchieveGoal(bool success, bool isJumpNext = true)
        {

            if (!targetTask)
                return;

            global::System.Diagnostics.StackTrace st = new global::System.Diagnostics.StackTrace();
            Debug.LogFormat("{0}完成任务目标:{1}", success ? "成功" : "失败", this.name + "\n" + st);

            //Debug.Log("isThisGoalDone-------" + isTheGoalDone);
            HasDone = true;
            isAchieveGoal = success;
            OnAchieveGoal_(success);
            //Debug.LogFormat("HasDone"+ HasDone+ "         isJumpNext:"+ isJumpNext);
            if (isJumpNext)
            {
                targetTask.AchieveGoal(success);
            }
        }

        public void OnGoalStart()
        {
            if (isAutoPlayStartAudio)
                PlayStartAudio();

        }

        /// <summary>
        /// 目标成功触发事件，一般时提示信息
        /// </summary>
        protected virtual void OnAchieveGoal_(bool success)
        {


            //语音
            if (!success)
            {
                if (TaskManager.Instance.TaskMode == TaskMode.Exam)
                {
                    PlayAudios(goalExaminationErrorAudioKeys);

                }
                else if (TaskManager.Instance.TaskMode == TaskMode.Study)
                {
                    PlayAudios(goalTrainingErrorAudioKeys);
                }

            }



            if (OnAchieveGoal != null)
                OnAchieveGoal(success);
        }


        public bool CheckAchieveGoal()
        {
            return notCalculate ? true : isAchieveGoal;
        }

        protected bool BeforeCheck()
        {
            //已完成不再检测
            if (isAchieveGoal)
                return false;

            if (targetTask.TaskState == TaskState.End)
                return false;

            return true;
        }

        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后将goal设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void SetGoalSuccess(bool isSuccess, bool hasDone = true)
        {
            this.HasDone = hasDone;
            isAchieveGoal = isSuccess;

        }

        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后将goal设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void RecoverGoalStatus()
        {
            HasDone = false;
            isAchieveGoal = false;
        }


#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskGoal/TaskGoalConf", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskGoalConf>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskGoalConf).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskGoalConf));
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



#endif