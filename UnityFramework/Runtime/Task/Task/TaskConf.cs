#if VIU_STEAMVR_2_0_0_OR_NEWER

using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



namespace UnityFramework.Runtime.Task
{

    public class TaskConf : LinkConf
    {

        /// <summary>
        /// 任务的名称
        /// </summary>
        public string taskName;

        /// <summary>
        /// 此节点的下标
        /// </summary>        
        public int index;


        /// <summary>
        /// 任务是否可以跳步或者记录
        /// </summary>
        public bool isCanRecord;

        /// <summary>
        /// 任务是否完成
        /// </summary>
        private bool isComplete = false;


        public float traingScore;
        public float examScore;


        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType taskType;

        /// <summary>
        /// 任务目标，拆分任务，所有任务目标完成即认为完成任务
        /// </summary>
        public List<TaskGoalConf> TaskGoals = new List<TaskGoalConf>();
        public List<ToolTaskConf> TaskTools = new List<ToolTaskConf>();
        /// <summary>
        /// 任务未开始状态
        /// </summary>
        public event Action<TaskConf> OnTaskInit;
        /// <summary>
        /// 任务开始时事件
        /// </summary>
        public event Action<TaskConf> OnTaskStart;
        /// <summary>
        /// 任务执行中事件
        /// </summary>
        public event Action<TaskConf> OnTaskDoing;
        /// <summary>
        /// 任务结束时事件
        /// </summary>
        public event Action<TaskConf> OnTaskEnd;

        public float delayTimeStartToDoing;
        public float delayTimeToStart;
        public event Action OnAchiveUnDone;
        public bool isJumpBigOnGoalError = true;


        public float reDoTime = 10;
        public float errorSound = -1;

        [Header("是否重复播放语音")]
        public bool isRepeatPlay = true;
        public float repeatTime = 30;
        public Timer timerOfRepeatPlay;

        /// <summary>
        /// 当前任务执行时  是否是通过跳步方式开始 此数据由TaskManager进行任务跳转时进行赋值
        /// </summary>
        [HideInInspector]
        public bool isSkipJumpStart = false;

        /// <summary>
        /// 临时添加 判断任务是否为跳过 
        /// taskStatus有问题 菜单大范围跳步时 中间的任务不会被记录
        /// </summary>
        [HideInInspector]
        private bool _isSkip = false;
        public bool IsSkip
        {
            get
            {
                bool skip = true;
                CheckTaskSkip(this, ref skip);
                return skip;
            }
            set => _isSkip = value;
        }
        //任务状态
        private TaskState taskState = TaskState.UnInit;


        private Timer dynTimer;



        /// <summary>
        /// 当任务往回跳转时 当前和目标(包括之间的任务)的isJumpInit会置为true
        /// 此开关只在任务状态变taskState变为Init时被赋值一次 
        /// 执行完任务的OnTaskInit后重新被置为false
        /// </summary>
        [HideInInspector]
        public bool isJumpInit = false;

        /// <summary>
        /// 标记任务是否跳过结束 用来给任务做判断
        /// 此开关只在任务状态taskState变为End时被赋值一次 
        /// 执行完相关逻辑后重新被置为false
        /// </summary>
        [HideInInspector]
        public bool isJumpEnd = false;
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
                    if (TaskManager.Instance)
                        TaskManager.Instance.StopDoingCoroutine();
                switch (taskState)
                {
                    case TaskState.Init:
                        if (oldState != TaskState.Init)
                        {
                            _OnTaskInit(this);
                            //Debug.Log("_OnTaskInit------------------------------"+this);
                        }
                        break;
                    case TaskState.Start:
                        if (oldState != TaskState.Start)
                        {
                            Debug.Log("_OnTaskStart------------------------------" + this);
                            _OnTaskStart(this);
                        }
                        break;
                    case TaskState.Doing:
                        _OnTaskDoing(this);
                        break;
                    case TaskState.End:
                        if (oldState != TaskState.End)
                        {
                            _OnTaskEnd(this);
                            Debug.Log("_OnTaskEnd------------------------------" + this);
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 记录已完成状态  
        /// 修正 这个状态用来记录任务是否完成过一次
        /// 之后基于这个开关 只要为true 任务init和end时都强制将状态置为true
        /// </summary>
        public bool IsPass
        {
            get
            {
                bool pass = true;
                CheckTaskPass(this, ref pass);
                return pass;
            }
        }
        void CheckTaskSkip(TaskConf t, ref bool isSkip)
        {
            if (isSkip == false)
                return;

            if (t.Child == null)
            {
                isSkip = this._isSkip;
            }
            else
            {
                List<TaskConf> tasks = TaskManager.Instance.GetChildTask(t);
                for (int i = 0; i < tasks.Count; i++)
                {
                    CheckTaskSkip(tasks[i], ref isSkip);
                }
            }
        }
        public void AchieveGoal(bool success, bool isForceJump = false, bool isForceJumpLittleInExcam = false)
        {

            dynTimer = ComponentEntry.Timer.DestroyTimer(dynTimer);



            ///如果先做了最后做的任务
            bool isDoLast = false;
            bool isAllHasDone = true;
            foreach (var result in TaskGoals)
            {
                if (result.isFinalGoal)
                {
                    if (result.HasDone)
                    {
                        isDoLast = true;
                    }
                }
                else
                {
                    if (!result.HasDone)
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
                if (TaskManager.Instance.TaskMode == TaskMode.Exam)
                {
                }
                ///如果是培训模式，播放对应的错误动效，然后引导错误流程
                else
                {
                    isReDoAll = true;
                }
            }

            //Debug.Log("任务数量:" + taskGoals.Count);
            foreach (var result in TaskGoals)
            {
                ///存在没有完成的任务
                if (!result.HasDone)
                {
                    Debug.Log("没有完成的任务:" + result.name);

                    ///如果是非强制跳跃，代表继续执行此任务
                    if (!isForceJump)
                    {
                        return;
                    }
                    success = false;
                }
                else if (!result.CheckAchieveGoal())
                {
                    success = false;
                }
            }
            float dynShowTime = 0;

            if (isReDoAll)
            {
                ComponentEntry.Timer.CreateTimer(() =>
                {
                    OnAchiveUnDone?.Invoke();
                    foreach (var result in TaskGoals)
                    {
                        if (!result.HasDone)
                        {
                            result.ForceTip();
                            return;
                        }
                    }
                }, 0);
                return;
            }

            dynTimer = ComponentEntry.Timer.CreateTimer(() =>
            {
                global::System.Diagnostics.StackTrace st = new global::System.Diagnostics.StackTrace();
                Debug.LogFormat("{0}完成任务:{1}", success ? "成功" : "失败", this.name + "\n" + st);
                if (isForceJumpLittleInExcam)
                {
                    DoNextTask(true, isForceJump);
                }
                else
                {
                    DoNextTask(success, isForceJump);
                }
                //Debug.LogError("开始下一个任务      "+this.taskName);

            }, "", dynShowTime);
        }



        /// <summary>
        /// 培训模式下，将会依次执行每一个步骤
        /// 考试模式下，如果做错，跳过此任务，执行下一个任务
        /// </summary>
        /// <param name="isAllComplete"></param>
        public void DoNextTask(bool isAllComplete, bool isJump = false)
        {
            //Debug.Log(TaskManager.Instance.CurrentTask + "-----" + "DoNextTask");
            dynTimer = ComponentEntry.Timer.DestroyTimer(dynTimer);

            isJumpEnd = isJump;//跳过状态赋值
            TaskManager.Instance.CurrentTask.taskState = TaskState.End;
            isJumpEnd = false; //恢复结束状态
            TaskManager.Instance.ExecuteNextTaskLittle();

            if (TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                if (!isAllComplete && isJumpBigOnGoalError)
                {
                    TaskManager.Instance.ExecuteNextTaskBig(isJump);
                }
                else
                {
                    TaskManager.Instance.ExecuteNextTaskLittle(isJump);
                }
            }
            else
            {
                TaskManager.Instance.ExecuteNextTaskLittle(isJump);
            }

        }


        /// <summary>
        /// 跳过任务
        /// </summary>
        public void SkipTask()
        {
            Debug.Log(TaskManager.Instance.CurrentTask + "-----" + "SkipTask");

            ToCompleteTask(true);
        }
        /// <summary>
        /// 考试模式下提交结果
        /// </summary>
        public void ToCompleteTask(bool isJump = false)
        {
            //Debug.Log(TaskManager.Instance.CurrentTask + "-----" + "ToCompleteTask");
            if (isJump)
            {
                TaskManager.Instance.CurrentTask.AchieveGoal(false, true);
                return;
            }
            if (TaskManager.Instance.TaskMode == TaskMode.Exam)
            {

                TaskManager.Instance.CurrentTask.AchieveGoal(false, true);


            }
            else
            {
                TaskManager.Instance.CurrentTask.AchieveGoal(false, true);
            }
        }


        private void PlayAudios(List<string> audios)
        {


        }



        private void SortGoals()
        {
            //Debug.Log(TaskManager.Instance.CurrentTask + "-----" + "SortGoals");
            TaskGoals.Sort((x, y) =>
            {
                if (x.Priority > y.Priority)
                    return 1;
                else if (x.Priority < y.Priority)
                    return -1;
                else
                    return 0;
            });
        }

        private void DoTaskRight()
        {
            //所有任务目标已达成,正确执行任务
            Debug.LogFormat("成功完成任务:{0} Index:{1}", this.taskName, this.index);
            this.isComplete = true;
            TaskManager.Instance.CurrentTask.taskState = TaskState.End;
            TaskManager.Instance.ExecuteNextTaskLittle();
        }

        private void DoTaskError()
        {
            //考试模式直接执行下一个任务，培训模式则不处理
            if (TaskManager.Instance.TaskMode == TaskMode.Exam)
            {
                Debug.LogFormat("失败完成任务:{0} Index:{1}", this.taskName, this.index);
                this.isComplete = false;
                TaskManager.Instance.CurrentTask.taskState = TaskState.End;
                TaskManager.Instance.ExecuteNextTaskBig();
            }
        }

        //事件源，供内部调用
        private void _OnTaskInit(TaskConf taskConf)
        {

            if (!isJumpInit)
                isComplete = false;


            IsSkip = false;
            SortGoals();



            try
            {
                TaskTools.ForEach(t =>
                {
                    Debug.Log("处理taskTools OnTaskInit----" + t.name);
                    t.OnTaskInit(this);
                });
                TaskGoals.ForEach(t =>
                {
                    Debug.Log("处理taskGoals OnTaskInit----" + t.name);
                    t.OnTaskInit(this);
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Error Task=" + this.name + "\n" + e);
            }


            if (OnTaskInit != null)
            {
                OnTaskInit(taskConf);
            }



            //如果任务已完成 就自动将任务的goal状态全部置为成功;
            if (isComplete)
                SetTaskSuccess(true);
        }


        private void _OnTaskStart(TaskConf taskConf)
        {
            Debug.LogFormat("任务:{0} OnTaskStart Index:{1} 时间：{2}", this.taskName, this.index, Time.time);

            foreach (var result in TaskGoals)
            {
                if (!result.HasDone)
                {
                    result.OnGoalStart();
                    break;
                }
            }

            TaskManager.Instance.StartDoingCoroutine(this);


            TaskTools.ForEach(t =>
            {
                Debug.Log("处理taskTools OnTaskStart----" + t.name);
                t.OnTaskStart(this);
            });
            TaskGoals.ForEach(t =>
            {
                Debug.Log("处理taskGoals OnTaskStart----" + t.name);
                t.OnTaskStart(this);
            });
            //(t =>  { Debug.Log(t.name);t.OnTaskStart(this); });
            //执行完taskTools的OnTaskStart后将此任务的特殊跳步标记关闭
            isSkipJumpStart = false;

            //通知工具系统
            if (OnTaskStart != null)
            {
                OnTaskStart(taskConf);
            }
            if (TaskGoals == null || TaskGoals.Count == 0)
            {
                AchieveGoal(true);
            }
        }



        private Timer waitTimer;
        private void _OnTaskDoing(TaskConf taskConf)
        {
            //Debug.LogFormat("任务:{0} OnTaskDoing Index:{1} 时间：{2}", this.taskName, this.index, Time.time);
            TaskGoals.ForEach(t => t.OnTaskDoing(this));
            TaskTools.ForEach(t => t.OnTaskDoing(this));


            if (OnTaskDoing != null)
            {
                OnTaskDoing(taskConf);
            }
        }


        private void _OnTaskEnd(TaskConf taskConf)
        {
            Debug.LogFormat("任务:{0} OnTaskEnd Index:{1} 时间：{2}", this.taskName, this.index, Time.time);



            TaskTools.ForEach(t =>
            {
                Debug.Log("处理taskTools OnTaskEnd----" + t.name);
                t.OnTaskEnd(this);
            });
            TaskGoals.ForEach(t =>
            {
                Debug.Log("处理taskGoals OnTaskEnd----" + t.name);
                t.OnTaskEnd(this);
            });


            if (OnTaskEnd != null)
            {
                OnTaskEnd(taskConf);
            }
            if (isRepeatPlay)
                ComponentEntry.Timer.DestroyTimer(timerOfRepeatPlay);
            if (waitTimer != null)
            {
                ComponentEntry.Timer.DestroyTimer(waitTimer);
            }




            if (!isComplete)
            {
                isComplete = TaskManager.Instance.CheckTaskGroupSuccess(this);
            }
            //如果任务已完成 就自动将任务的goal状态全部置为成功;
            if (isComplete)
            {
                SetTaskSuccess(true);
            }


        }





        void CheckTaskPass(TaskConf t, ref bool isPass)
        {
            if (isPass == false)
                return;

            if (t.Child == null)
            {
                isPass = this.isComplete;
            }
            else
            {

                List<TaskConf> tasks = TaskManager.Instance.GetChildTask(t);
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


        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后将goal设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void SetTaskSuccess(bool isSuccess, bool hasDone = true)
        {
            for (int j = 0; j < TaskGoals.Count; j++)
            {
                TaskGoals[j].SetGoalSuccess(isSuccess, hasDone);
            }
            isComplete = isSuccess;
            IsSkip = false;
        }

        /// <summary>
        /// 提供进度恢复时强制恢复任务状态接口 
        /// 执行后将goal设置为已执行和对应成功状态
        /// </summary>
        /// <param name="isSuccess">是否成功完成</param>
        public void RecoverTaskStatus()
        {
            for (int j = 0; j < TaskGoals.Count; j++)
            {
                TaskGoals[j].RecoverGoalStatus();
                IsSkip = false;
                isComplete = false;
            }
        }


#if UNITY_EDITOR
        [MenuItem("Assets/UnityFramework/Task/TaskConf", false, 0)]
        static void CreateDynamicConf()
        {
            UnityEngine.Object obj = Selection.activeObject;
            if (obj)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                ScriptableObject asset = CreateInstance<TaskConf>();
                if (asset)
                {
                    int index = 0;
                    string confName = "";
                    UnityEngine.Object obj1 = null;
                    do
                    {
                        confName = path + "/" + typeof(TaskConf).Name + "_" + index + ".asset";
                        obj1 = UnityEditor.AssetDatabase.LoadAssetAtPath(confName, typeof(TaskConf));
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