/*
* FileName:          TaskConfig
* CompanyName:       
* Author:            
* Description:       
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
    public class TaskConfig : ScriptableObject
    {

        [Header("任务名称")]
        public string TaskName;

        [Header("任务id")]
        public int TaskID;

        [Header("模块id")]
        public int ModuleID;

        /// <summary>
        /// 优先级,自动赋值
        /// </summary>
        public int Priority;


        /// <summary>
        /// Study学习 Exam考核 Default
        /// </summary>
        [Header("任务模式")]
        public TaskMode TaskMode = TaskMode.Default;






        /// <summary>
        /// 是否已经遍历过此节点
        /// </summary>
        public bool isRead = false;

        /// <summary>
        /// 任务是否可以记录
        /// </summary>
        public bool isCanRecord = true;

        /// <summary>
        /// 父任务
        /// </summary>
        [HideInInspector]
        public TaskConfig Parent;

        [Header("子任务")]
        public TaskConfig Child;

        /// <summary>
        /// 上一个任务
        /// </summary>
        [HideInInspector]
        public TaskConfig Previous;
        /// <summary>
        /// 下一个任务
        /// </summary>
        [Header("下个任务")]
        public TaskConfig Next;

        /// <summary>
        /// 下一个任务,当无下一个任务时，代表所有任务已经完成。
        /// </summary>
        public TaskConfig NextNode
        {
            get
            {
                TaskConfig next = null;
                if (Child && !isRead)
                {
                    isRead = true;
                    next = Child;
                    next.Parent = this;
                }
                else if (Next)
                {
                    next = Next;
                    next.Previous = this;
                    next.Parent = this.Parent;
                }
                else if (Parent)
                {
                    return Parent.NextNode;
                }
                return next;
            }
        }


        /// <summary>
        /// 下一个大任务节点
        /// </summary>
        public TaskConfig NextBigNode
        {
            get
            {
                TaskConfig next = null;
                if (Parent)
                {
                    return Parent.NextBigNode;
                }
                else if (Next)
                {
                    return Next;
                }
                return next;
            }
        }

        /// <summary>
        /// 上一个大任务节点
        /// </summary>
        public TaskConfig PreviousBigNode
        {
            get
            {
                TaskConfig previous = null;
                if (Parent)
                {
                    return Parent.PreviousBigNode;
                }
                else if (Previous)
                {
                    return Previous;
                }
                return previous;
            }
        }






        /// <summary>
        /// 任务是否完成
        /// </summary>
        private bool isPass = false;

        public bool IsPass
        {
            get
            {
                bool pass = true;
                CheckTaskPass(this, ref pass);
                return pass;
            }
        }
        /// <summary>
        /// 表示任务被跳过
        /// </summary>
        private bool isSkip = false;
        /// <summary>
        /// 表示任务被跳过
        /// </summary>
        public bool IsSkip
        {
            get
            {
                bool skip = true;
                CheckTaskSkip(this, ref skip);
                return skip;
            }
            set
            {
                isSkip = value;
            }
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        private TaskState taskState = TaskState.UnInit;

        public TaskState TaskState
        {
            get
            {
                return taskState;
            }
            set
            {
                TaskState oldState = taskState;
                taskState = value;
                if (taskState != TaskState.Doing)
                {
                    ComponentEntry.Task.StopTaskDoingCoroutine();
                }
                switch (taskState)
                {
                    case TaskState.Init:
                        if (oldState != TaskState.Init)
                        {
                            _OnTaskInit(this);
                            Log.Info($"任务初始化:模块id:{this.ModuleID}|任务id:{this.TaskID}|任务名称:{this.TaskName}");
                        }
                        break;
                    case TaskState.Start:
                        if (oldState != TaskState.Start)
                        {
                            _OnTaskStart(this);
                            Log.Info($"任务开始:模块id:{this.ModuleID}|任务id:{this.TaskID}|任务名称:{this.TaskName}");
                        }
                        break;
                    case TaskState.Doing:
                        _OnTaskDoing(this);
                        break;
                    case TaskState.End:
                        if (oldState != TaskState.End)
                        {
                            _OnTaskEnd(this);
                            Log.Info($"任务结束:模块id:{this.ModuleID}|任务id:{this.TaskID}|任务名称:{this.TaskName}");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 任务目标,所有任务目标完成即认为完成任务
        /// </summary>
        [Header("任务目标")]
        public List<TaskTargetConfig> TaskTargetList = new List<TaskTargetConfig>();

        /// <summary>
        /// 任务工具
        /// </summary>
        public List<TaskToolConfig> TaskTools = new List<TaskToolConfig>();


        /// <summary>
        /// 任务未开始事件
        /// </summary>
        public event Action<TaskConfig> OnTaskInit;
        /// <summary>
        /// 任务开始时事件
        /// </summary>
        public event Action<TaskConfig> OnTaskStart;
        /// <summary>
        /// 任务执行中事件
        /// </summary>
        public event Action<TaskConfig> OnTaskDoing;
        /// <summary>
        /// 任务结束时事件
        /// </summary>
        public event Action<TaskConfig> OnTaskEnd;

        public event Action<TaskConfig> OnUnDone;

        /// <summary>
        /// 延迟执行TaskStart
        /// </summary>
        public float DelayTimeToStart;
        /// <summary>
        /// 延迟执行TaskDoing
        /// </summary>
        public float DelayTimeToDoing;

        /// <summary>
        /// 目标失败后是否自动跳入下一大任务
        /// </summary>
        public bool isJumpBigOnError = false;

        /// <summary>
        /// 任务启动时语音提示
        /// </summary>
        [Header("训练任务开始语音")]
        public List<string> startStudyAudioKeys = new List<string>();
        [Header("考核模式任务开始语音")]
        public List<string> startExamAudioKeys = new List<string>();

        private List<string> realStartAudioeys = new List<string>();


        /// <summary>
        /// 是否播放任务失败语音提示
        /// </summary>
        public bool isPlayAudioOnError = false;
        /// <summary>
        /// 任务失败语音提示
        /// </summary>
        [Header("任务失败语音,会等播放完成")]
        public List<string> errorAudioKeys = new List<string>();

        public bool isPlayAudioOnSuccess = false;

        /// <summary>
        /// 任务成功语音提示
        /// </summary>
        [Header("任务成功语音,会等播放完成")]
        public List<string> successAudioKeys = new List<string>();

        /// <summary>
        /// 任务是否只是播放语音提示
        /// 只播语音，播完进入下一任务，不需要目标
        /// </summary>
        [Header("只播语音，播完进入下一任务")]
        public bool isJustPlayAudio = false;

        public float reDoTime = 10;
        private bool isFirstReDo = true;



        private Timer timer;


        /// <summary>
        /// 初始化此任务，将此任务设置为未读取状态
        /// 因脚本配置文件会缓存属性，因此在读取任务之前需要先将任务设置为未读取状态
        /// </summary>
        public void InitTask()
        {
            isRead = false;
        }

        /// <summary>
        /// 任务初始化
        /// </summary>
        private void _OnTaskInit(TaskConfig taskConfig)
        {
            isPass = false;
            IsSkip = false;
            isFirstReDo = true;

            SortGoals();
            TaskTools.ForEach(t => t.OnTaskInit(taskConfig));
            TaskTargetList.ForEach(t => t.OnTaskInit(taskConfig));
            OnTaskInit?.Invoke(taskConfig);

        }

        /// <summary>
        /// 任务开始
        /// </summary>
        private void _OnTaskStart(TaskConfig taskConfig)
        {
            timer = ComponentEntry.Timer.DestroyTimer(timer);
            ComponentEntry.Task.StartTaskDoingCoroutine(taskConfig);

            // 播放开始语音
            // 考试模式和训练模式开始语音不一样
            if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
            {
                realStartAudioeys = startExamAudioKeys;
            }
            else
            {
                realStartAudioeys = startStudyAudioKeys;
            }
            float _dynShowTime = 0;
            PlayAudios(realStartAudioeys);
            _dynShowTime = GetAudioLength(realStartAudioeys);
            timer = ComponentEntry.Timer.CreateTimer(delegate
            {
                OnTaskPlayStartAudioComplete();
            }, _dynShowTime);


            TaskTools.ForEach(t => t.OnTaskStart(taskConfig));
            TaskTargetList.ForEach(t => t.OnTaskStart(taskConfig));
            OnTaskStart?.Invoke(taskConfig);


            if (TaskTargetList == null || TaskTargetList.Count == 0)
            {
                if (isJustPlayAudio)
                {
                    //等播放音频完
                    ComponentEntry.Timer.CreateTimer(delegate
                    {
                        NotifyResult(true);
                    }, _dynShowTime);
                }
                else
                {
                    NotifyResult(true);
                }
            }


        }

        /// <summary>
        /// 任务持续中
        /// </summary>
        private void _OnTaskDoing(TaskConfig taskConfig)
        {
            TaskTools.ForEach(t => t.OnTaskDoing(taskConfig));
            TaskTargetList.ForEach(t => t.OnTaskDoing(taskConfig));
            OnTaskDoing?.Invoke(taskConfig);
        }


        /// <summary>
        /// 任务结束 不论任务的结果正确/出错/跳过
        /// </summary>
        private void _OnTaskEnd(TaskConfig taskConfig)
        {
            TaskTools.ForEach(t => t.OnTaskEnd(taskConfig));
            TaskTargetList.ForEach(t => t.OnTaskEnd(taskConfig));
            OnTaskEnd?.Invoke(taskConfig);
        }

        /// <summary>
        /// 开始语音播放完毕
        /// </summary>
        private void OnTaskPlayStartAudioComplete()
        {
            foreach (var item in TaskTargetList)
            {
                if (item.TargetMode == TaskMode.Default || item.TargetMode == ComponentEntry.Task.TaskMode)
                {
                    item.OnTaskPlayStartAudioComplete(this);
                }
            }
        }

        /// <summary>
        ///  任务跳过
        /// </summary>
        public void SkipTask()
        {
            IsSkip = true;

            NotifyResult(false, true);
        }


        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void SetTaskState(bool isSuccess, bool hasDone = true)
        {
            for (int j = 0; j < TaskTargetList.Count; j++)
            {
                TaskTargetList[j].SetTargetState(isSuccess, hasDone);
            }
            isPass = isSuccess;
            IsSkip = false;
        }


        void CheckTaskPass(TaskConfig t, ref bool isPass)
        {
            if (isPass == false)
                return;

            if (t.Child == null)
            {
                isPass = this.isPass;
            }
            else
            {

                List<TaskConfig> tasks = ComponentEntry.Task.GetChildTask(t);
                for (int i = 0; i < tasks.Count; i++)
                {
                    try
                    {
                        CheckTaskPass(tasks[i], ref isPass);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(this.name + "    " + tasks[i].name + "\n" + e);
                    }
                }
            }

        }


        void CheckTaskSkip(TaskConfig t, ref bool skip)
        {
            if (skip == false)
                return;

            if (t.Child == null)
            {
                skip = this.isSkip;
            }
            else
            {
                List<TaskConfig> tasks = ComponentEntry.Task.GetChildTask(t);
                for (int i = 0; i < tasks.Count; i++)
                {
                    CheckTaskSkip(tasks[i], ref isSkip);
                }
            }

        }

        /// <summary>
        /// 当一个任务目标完成时，通知任务系统
        /// 如果此目标的结果是对的，则遍历任务是否存在尚未执行的任务目标
        /// 如果全部目标都已完成，则此任务结束。如果存在未操作的任务目标，则返回，继续完成目标
        /// 如果此目标错误，则记录目标
        /// </summary>
        /// <param name="success"></param>
        /// <param name="isForceJump"></param>
        public void NotifyResult(bool success, bool isForceJump = false)
        {
            timer = ComponentEntry.Timer.DestroyTimer(timer);

            ///如果先做了最后做的任务
            bool isDoLast = false;
            bool isAllHasDone = true;

            foreach (var result in TaskTargetList)
            {
                if (result.isFinal)
                {
                    if (result.HadDone)
                    {
                        isDoLast = true;
                    }
                }
                else
                {
                    if (!result.HadDone)
                    {
                        isAllHasDone = false;
                        break;
                    }
                }
            }

            bool isReDoAll = false;
            ///如果做了最后一步，但是其他的没做完
            if (isDoLast && !isAllHasDone)
            {
                success = false;
                isForceJump = true;
                ///如果是考试模式，播放对应的错误动效，然后结束任务
                if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
                {
                }
                ///如果是培训模式，播放对应的错误动效，然后引导错误流程
                else
                {
                    isReDoAll = true;
                }
            }

            foreach (var result in TaskTargetList)
            {
                ///存在没有完成的任务
                if (!result.HadDone)
                {
                    result.OnTargetStart();
                    ///如果是非强制跳跃，代表继续执行此任务
                    if (!isForceJump)
                    {
                        return;
                    }
                    success = false;
                }
                else if (!result.CheckResult())
                {
                    success = false;
                }
            }

            //播放成功/失败语音
            float dynShowTime = 0;
            if (!success && isPlayAudioOnError && errorAudioKeys != null && errorAudioKeys.Count > 0)
            {
                if (ComponentEntry.Task.TaskMode != TaskMode.Exam)
                {
                    PlayAudios(errorAudioKeys);
                    dynShowTime = GetAudioLength(errorAudioKeys);
                }
            }
            if (success && isPlayAudioOnSuccess && successAudioKeys != null && successAudioKeys.Count > 0)
            {
                if (ComponentEntry.Task.TaskMode != TaskMode.Exam)
                {
                    PlayAudios(successAudioKeys);
                    dynShowTime = GetAudioLength(successAudioKeys);
                }
            }

            if (isReDoAll)
            {
                float wtime = isFirstReDo ? reDoTime : 0;
                ComponentEntry.Timer.CreateTimer(delegate
                {
                    isFirstReDo = false;
                    OnUnDone?.Invoke(this);
                    foreach (var result in TaskTargetList)
                    {
                        if (!result.HadDone)
                        {
                            result.ForceTip();
                            return;
                        }
                    }
                }, wtime);
            }

            if (dynShowTime == 0)
            {
                DoNextTask(success, isForceJump);
            }
            else
            {
                timer = ComponentEntry.Timer.CreateTimer(() =>
                {
                    DoNextTask(success, isForceJump);
                }, dynShowTime);
            }
        }

        /// <summary>
        /// 培训模式下，将会依次执行每一个步骤
        /// 考试模式下，如果做错，跳过此任务，执行下一个任务
        /// </summary>
        /// <param name="isAllComplete"></param>
        /// <param name="isJump"></param>
        public void DoNextTask(bool isAllComplete, bool isJump = false)
        {
            timer = ComponentEntry.Timer.DestroyTimer(timer);
            ComponentEntry.Task.CurrentTask.TaskState = TaskState.End;
            if (ComponentEntry.Task.TaskMode == TaskMode.Exam)
            {
                if (!isAllComplete && isJumpBigOnError)
                {
                    ComponentEntry.Task.ExecuteNextBigTask(isJump);
                }
                else
                {
                    ComponentEntry.Task.ExecuteNextTask(isJump);
                }
            }
            else
            {
                ComponentEntry.Task.ExecuteNextTask(isJump);
            }
        }

        /// <summary>
        /// 强制完成任务
        /// 检测并记录目标是否完成
        /// </summary>
        public virtual void ForceCompleteTask()
        {
            timer = ComponentEntry.Timer.DestroyTimer(timer);

            if (TaskTargetList == null || TaskTargetList.Count < 1)
            {
                NotifyResult(true);
            }

            foreach (TaskTargetConfig item in TaskTargetList)
            {
                item.ForceCompleteTask(this);
            }
        }

        private void SortGoals()
        {
            //Debug.Log(TaskManager.Instance.CurrentTask + "-----" + "SortGoals");
            TaskTargetList.Sort((x, y) =>
            {
                if (x.priority > y.priority)
                    return 1;
                else if (x.priority < y.priority)
                    return -1;
                else
                    return 0;
            });
        }

        /// <summary>
        /// 跳步 往前跳
        /// </summary>
        public void JumpForward()
        {
            foreach (var item in TaskTargetList)
            {
                item.JumpForward(this);
            }
        }

        /// <summary>
        /// 跳步 往后跳
        /// </summary>
        public void JumpBack()
        {
            foreach (var item in TaskTargetList)
            {
                item.JumpBack(this);
            }
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
        public void StopAllAudio()
        {
            ComponentEntry.Audio.StopAllAudio();
        }


#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskConfig", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskConfig>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskConfig).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskConfig));
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