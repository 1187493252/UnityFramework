/*
* FileName:          TaskComponent
* CompanyName:       
* Author:            
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public enum TaskState
    {
        UnInit = 0,
        Init = 1,
        Start = 2,
        Doing = 3,
        End = 4
    }
    public enum TaskMode
    {
        Default = 0,
        Study = 1,
        Exam = 2
    }

    [DisallowMultipleComponent]
    public class TaskComponent : UnityFrameworkComponent
    {
        /// <summary>
        /// 任务模式
        /// </summary>
        public TaskMode TaskMode = TaskMode.Study;

        [Header("每个模块的第一个任务")]
        public List<TaskConfig> AllModuleFirstTask = new List<TaskConfig>();
        /// <summary>
        /// 所有任务 key 任务模块
        /// </summary>
        private Dictionary<int, List<TaskConfig>> allTasks = new Dictionary<int, List<TaskConfig>>();

        /// <summary>
        /// 当前模块所有的任务
        /// </summary>
        [SerializeField]
        private List<TaskConfig> currentModuleAllTasks = new List<TaskConfig>();
        /// <summary>
        /// 当前模块大任务节点
        /// </summary>
        [SerializeField]
        private List<TaskConfig> currentModuleAllBigTasks = new List<TaskConfig>();

        /// <summary>
        /// 当前模块错误的大任务节点
        /// </summary>
        [SerializeField]
        private List<TaskConfig> currentModuleErrorBigTasks = new List<TaskConfig>();

        [SerializeField]
        private List<TaskConfig> currentModuleStudyTaskList = new List<TaskConfig>();
        [SerializeField]
        private List<TaskConfig> currentModuleExamTaskList = new List<TaskConfig>();


        /// <summary>
        /// 当前模块任务记录,学习/考核的任务
        /// </summary>
        private Dictionary<TaskMode, List<TaskConfig>> currentModuleRecordTasks = new Dictionary<TaskMode, List<TaskConfig>>();



        /// <summary>
        /// 当前任务
        /// </summary>
        [HideInInspector]
        public TaskConfig CurrentTask;

        /// <summary>
        /// 是否为考试模式
        /// </summary>
        /// <returns></returns>
        public bool IsExam
        {
            get
            {
                return TaskMode == TaskMode.Exam;
            }
        }

        bool isCurrentModuleTaskFinish;
        public bool IsCurrentModuleTaskFinish
        {
            get
            {
                return isCurrentModuleTaskFinish;
            }
        }
        /// <summary>
        /// 任务开始的时间
        /// </summary>
        public DateTime TaskStartTime;
        public DateTime TaskEndTime;


        public event Action<TaskConfig> OnTaskChange;

        private Coroutine coroutineTaskStart;

        private Coroutine coroutineTaskDoing;





        /// <summary>
        /// 读取任务
        /// </summary>
        /// <param name="list"></param>
        private void ReadAllTask(List<TaskConfig> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }



            allTasks.Clear();

            foreach (var item in list)
            {
                TaskConfig taskConf = item;
                int index = 0;
                TaskConfig nextTask = taskConf;
                List<TaskConfig> tmp = new List<TaskConfig>();
                tmp.Clear();
                while (nextTask)
                {
                    nextTask.Priority = index++;
                    tmp.Add(nextTask);
                    nextTask = nextTask.NextNode;
                }
                allTasks.Add(taskConf.ModuleID, tmp);
            }
            ResetAllTasks();
        }





        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="index">AllModuleFirstTask的下标</param>
        public void StartTask(int index)
        {
            if (index >= 0 && index < AllModuleFirstTask.Count)
            {
                StartTask(AllModuleFirstTask[index]);
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="taskConf">AllModuleFirstTask的元素</param>
        public void StartTask(TaskConfig taskConf)
        {
            if (!taskConf)
            {
                return;
            }

            currentModuleAllTasks = allTasks[taskConf.ModuleID];
            if (currentModuleAllTasks == null || currentModuleAllTasks.Count == 0)
            {
                return;
            }
            //----------------------

            currentModuleAllBigTasks.Clear();
            currentModuleErrorBigTasks.Clear();
            currentModuleRecordTasks.Clear();
            currentModuleStudyTaskList.Clear();
            currentModuleExamTaskList.Clear();
            CurrentTask = currentModuleAllTasks[0];

            TaskConfig nextTask = currentModuleAllTasks[0];
            List<TaskConfig> canSkipExamTasks = new List<TaskConfig>();
            List<TaskConfig> canSkipStudyTasks = new List<TaskConfig>();
            while (nextTask)
            {
                nextTask.TaskState = TaskState.UnInit;

                nextTask.TaskState = TaskState.Init;

                if (nextTask.TaskMode == TaskMode.Default)
                {
                    currentModuleStudyTaskList.Add(nextTask);
                    currentModuleExamTaskList.Add(nextTask);
                    if (nextTask.isCanRecord)
                    {
                        canSkipStudyTasks.Add(nextTask);
                        canSkipExamTasks.Add(nextTask);
                    }
                }
                else if (nextTask.TaskMode == TaskMode.Study)
                {
                    currentModuleStudyTaskList.Add(nextTask);
                    if (nextTask.isCanRecord)
                    {
                        canSkipStudyTasks.Add(nextTask);
                    }
                }
                else
                {
                    currentModuleExamTaskList.Add(nextTask);
                    if (nextTask.isCanRecord)
                    {
                        canSkipExamTasks.Add(nextTask);
                    }
                }
                nextTask = nextTask.NextNode;
            }
            currentModuleRecordTasks.Add(TaskMode.Exam, canSkipExamTasks);
            currentModuleRecordTasks.Add(TaskMode.Study, canSkipStudyTasks);

            TaskConfig bigNextTask = currentModuleAllTasks[0];
            while (bigNextTask)
            {
                currentModuleAllBigTasks.Add(bigNextTask);
                bigNextTask = GetNextBigTask(bigNextTask);
            }
            //-----------------
            ResetCurrentModuleTasks();
            TaskStartTime = DateTime.Now;
            isCurrentModuleTaskFinish = false;
            if (coroutineTaskStart != null)
            {
                StopCoroutine(coroutineTaskStart);
                coroutineTaskStart = null;
            }
            coroutineTaskStart = StartCoroutine(ExecuteTaskDelay(CurrentTask, false));
        }



        public void ResetCurrentModuleTasks()
        {
            foreach (var item in currentModuleAllTasks)
            {
                item.InitTask();
                item.TaskState = TaskState.UnInit;
            }

        }
        public void ResetAllTasks()
        {
            foreach (var item in allTasks)
            {
                item.Value.ForEach(t =>
                {
                    t.InitTask();
                    t.TaskState = TaskState.UnInit;
                });
            }

        }

        /// <summary>
        /// 跳过当前任务
        /// </summary>
        /// <param name="taskConf"></param>
        public void SkipCurrentTask()
        {
            CurrentTask.SkipTask();
        }

        /// <summary>
        /// 跳步到某任务
        /// </summary>
        /// <param name="taskConf"></param>
        public void SkipToTask(TaskConfig taskConf)
        {
            if (!taskConf)
            {
                return;
            }
            if (coroutineTaskStart != null)
            {
                StopCoroutine(coroutineTaskStart);
                coroutineTaskStart = null;
            }
            coroutineTaskStart = StartCoroutine(ExecuteTaskDelay(taskConf, true));
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public void RestartCurrentModuleTask()
        {
            StartTask(CurrentTask);
        }

        private IEnumerator ExecuteTaskDelay(TaskConfig taskConf, bool isJump = false)
        {
            yield return new WaitForSeconds(taskConf.DelayTimeToStart);
            ExecuteTask(taskConf, isJump);
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        private void ExecuteTask(TaskConfig targetTask, bool isJump = false)
        {
            if (CurrentTask == null)
                return;

            TaskConfig tempTask = CurrentTask;
            //首先判断该任务是否跳步，如果跳步，做跳步处理      
            int compareResult = CompareTaskPriority(targetTask, tempTask);

            CurrentTask = targetTask;


            //向前执行跳步
            if (compareResult < 0)
            {
                //重新设置两个任务之间所有任务的状态      
                TaskConfig last = tempTask;
                while (last && last != targetTask)
                {
                    //这里可以做一些往前跳步的处理操作,目前只调了init
                    //一般Init都是赋值,固不调用
                    //   last.TaskState = TaskState.Init;
                    last.JumpForward();


                    if (isJump && !last.IsPass)
                    {
                        last.IsSkip = true;
                    }
                    else
                    {
                        last.IsSkip = false;
                    }
                    last = GetPreviousTaskWithTaskMode(last);
                }
                //当前任务处理

                CurrentTask.TaskState = TaskState.Start;

            }
            //向后执行跳步
            else if (compareResult > 0)
            {
                //跳步到这个任务之后的任务
                TaskConfig next = tempTask;
                while (next && next != targetTask)
                {
                    if (isJump && !next.IsPass)
                    {
                        next.IsSkip = true;
                    }
                    else
                    {
                        next.IsSkip = false;
                    }
                    //跳步处理
                    //一般End都有Hide操作,JumpBack可以做一些特殊操作
                    next.JumpBack();
                    next.TaskState = TaskState.End;

                    next = GetNextTaskWithTaskMode(next);//不同模式不做处理
                    //区分任务模式,不同模式不做处理
                    //if (next.TaskMode == TaskMode || next.TaskMode == TaskMode.Default)
                    //{
                    //    //模式一样|Default匹配任何模式
                    //    next.TaskState = TaskState.End;
                    //    next = GetNextTask(next);
                    //}
                    //else
                    //{
                    //    //非当前模式的任务直接标记为已完成
                    //    next.SetTaskState(true, false);
                    //    next = GetNextTask(next);
                    //}
                }
                //当前任务处理

                CurrentTask.TaskState = TaskState.Start;
            }
            else
            {
                //重新执行这个任务

                CurrentTask.TaskState = TaskState.Start;
            }
            OnTaskChange?.Invoke(CurrentTask);
        }




        public void StartTaskDoingCoroutine(TaskConfig taskConf)
        {

            coroutineTaskDoing = StartCoroutine(TaskDoingCoroutine(taskConf));
        }

        public void StopTaskDoingCoroutine()
        {
            if (coroutineTaskDoing != null)
            {
                StopCoroutine(coroutineTaskDoing);
                coroutineTaskDoing = null;
            }
        }

        private IEnumerator TaskDoingCoroutine(TaskConfig taskConf)
        {
            yield return new WaitForSeconds(taskConf.DelayTimeToDoing);
            while (true)
            {
                taskConf.TaskState = TaskState.Doing;
                yield return null;
            }
        }

        /// <summary>
        /// 执行下个小任务
        /// </summary>
        /// <param name="isJump"></param>
        public void ExecuteNextTask(bool isJump = false)
        {
            if (coroutineTaskStart != null)
            {
                StopCoroutine(coroutineTaskStart);
                coroutineTaskStart = null;
            }

            if (CurrentTask)
            {
                TaskConfig nextTaskLittle = GetNextTaskWithTaskMode(CurrentTask);

                if (nextTaskLittle != null)
                {
                    coroutineTaskStart = StartCoroutine(ExecuteTaskDelay(nextTaskLittle, isJump));
                }
                else
                {

                    OnCompletAllTask();
                }
            }
        }

        /// <summary>
        /// 执行下一个大任务
        /// </summary>
        /// <param name="isJump"></param>
        public void ExecuteNextBigTask(bool isJump = false)
        {
            if (coroutineTaskStart != null)
            {
                StopCoroutine(coroutineTaskStart);
                coroutineTaskStart = null;
            }

            if (CurrentTask)
            {
                TaskConfig nextTaskBig = GetNextBigTaskWithTaskMode(CurrentTask);
                if (nextTaskBig != null)
                {
                    coroutineTaskStart = StartCoroutine(ExecuteTaskDelay(nextTaskBig, isJump));
                }
                else
                {
                    coroutineTaskStart = StartCoroutine(ExecuteTaskDelay(currentModuleAllTasks[currentModuleAllTasks.Count - 1], isJump));

                    OnCompletAllTask();
                }

            }
        }




        /// <summary>
        /// 比较任务优先级，返回结果小于零 大于零 等于零
        /// </summary>
        /// <param name="taskConf0"></param>
        /// <param name="taskConf1"></param>
        /// <returns></returns>
        private int CompareTaskPriority(TaskConfig taskConf0, TaskConfig taskConf1)
        {
            return taskConf0.Priority - taskConf1.Priority;
        }




        /// <summary>
        /// 下一个任务,不考虑模式
        /// </summary>
        /// <param name="taskConf"></param>
        public TaskConfig GetNextTask(TaskConfig taskConf)
        {
            if (taskConf.Priority < currentModuleAllTasks.Count - 1)
            {
                return currentModuleAllTasks[taskConf.Priority + 1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 下一个大任务,不考虑模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConfig GetNextBigTask(TaskConfig taskConf)
        {
            return taskConf.NextBigNode;
        }

        /// <summary>
        /// 上一个任务,不考虑模式
        /// </summary>
        /// <param name="taskConfig"></param>
        public TaskConfig GetPreviousTask(TaskConfig taskConfig)
        {
            if (!taskConfig)
            {
                return null;

            }
            if (taskConfig.Priority > 0)
            {
                return currentModuleAllTasks[taskConfig.Priority - 1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 下一个任务,考虑任务模式
        /// </summary>
        /// <param name="taskConfig"></param>
        public TaskConfig GetNextTaskWithTaskMode(TaskConfig taskConfig)
        {
            TaskConfig next = GetNextTask(taskConfig);
            while (next != null)
            {
                if (next.TaskMode == TaskMode || next.TaskMode == TaskMode.Default)
                {
                    break;
                }
                else
                {
                    next = GetNextTask(next);
                }
            }
            return next;
        }

        /// <summary>
        /// 下一个大任务:考虑任务模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConfig GetNextBigTaskWithTaskMode(TaskConfig taskConf)
        {
            TaskConfig next = GetNextBigTask(taskConf);
            while (next != null)
            {

                if ((next.TaskMode == TaskMode || next.TaskMode == TaskMode.Default) && next.isCanRecord)
                {
                    break;
                }
                else
                {
                    next = GetNextTask(next);
                }
            }
            return next;
        }

        /// <summary>
        /// 上一个大任务,不考虑模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConfig GetPreviousBigTask(TaskConfig taskConf)
        {
            return taskConf.PreviousBigNode;
        }

        /// <summary>
        /// 上一个任务,考虑模式
        /// </summary>
        /// <param name="taskConfig"></param>
        public TaskConfig GetPreviousTaskWithTaskMode(TaskConfig taskConfig)
        {
            TaskConfig last = GetPreviousTask(taskConfig);
            while (last != null)
            {

                if (last.TaskMode == TaskMode || last.TaskMode == TaskMode.Default)
                {
                    break;
                }
                else
                {
                    last = GetPreviousTask(last);
                }
            }
            return last;
        }

        /// <summary>
        /// 上一个大任务,考虑模式
        /// </summary>
        /// <param name="taskConfig"></param>
        public TaskConfig GetPreviousBigTaskWithTaskMode(TaskConfig taskConfig)
        {
            TaskConfig last = GetPreviousBigTask(taskConfig);
            while (last != null)
            {

                if (last.TaskMode == TaskMode || last.TaskMode == TaskMode.Default)
                {
                    break;
                }
                else
                {
                    last = GetPreviousBigTask(last);
                }
            }
            return last;
        }

        public TaskConfig GetCurrentBigTask(TaskConfig taskConfig)
        {
            if (taskConfig.isCanRecord)
            {
                return taskConfig;
            }
            TaskConfig task = GetPreviousTaskWithTaskMode(taskConfig);

            while (task != null)
            {
                if (task.isCanRecord)
                {
                    break;
                }
                else
                {
                    task = GetPreviousTaskWithTaskMode(task);
                }
            }
            return task;

        }

        /// <summary>
        /// 完成所有任务
        /// </summary>
        private void OnCompletAllTask()
        {
            CurrentTask = null;
            TaskEndTime = DateTime.Now;
            isCurrentModuleTaskFinish = true;

            //------------
            TimeSpan timeSpan = TaskEndTime - TaskStartTime;
            Debug.Log($"当前模块任务全部完成,用时:{timeSpan.TotalSeconds}秒");
        }

        public void StopCurrentModuleTask()
        {
            CurrentTask = null;
            ResetCurrentModuleTasks();
        }

        public List<TaskConfig> GetChildTask(TaskConfig task, bool hasSelf = false)
        {
            List<TaskConfig> taskGroup = new List<TaskConfig>();
            if (hasSelf)
                taskGroup.Add(task);
            TaskConfig conf;
            if (task.Child != null)
            {
                conf = task.Child;
                taskGroup.Add(conf);
                //Debug.Log("");
                while (conf.Next != null)
                {
                    conf = conf.Next;
                    taskGroup.Add(conf);
                }
            }

            return taskGroup;
        }

        public TaskConfig GetTaskByName(string taskName)
        {
            TaskConfig task = currentModuleAllTasks.Find(t => t.TaskName == taskName);
            return task;
        }
        public TaskConfig GetTaskById(int taskid)
        {
            TaskConfig task = currentModuleAllTasks.Find(t => t.TaskID == taskid);
            return task;
        }

        public List<TaskConfig> GetCurrentModuleAllBigTask()
        {
            return currentModuleAllBigTasks;
        }

        public List<TaskConfig> GetCurrentModuleStudyTask()
        {
            return currentModuleStudyTaskList;
        }
        public List<TaskConfig> GetCurrentModuleExamTask()
        {
            return currentModuleExamTaskList;
        }

        public List<TaskConfig> GetCurrentModuleRecordTask(TaskMode taskMode)
        {
            return currentModuleRecordTasks[taskMode];
        }

        public void ForceCompleteTask()
        {
            CurrentTask.ForceCompleteTask();
        }

        //-------------------------------------------------

        public void Init()
        {
            ReadAllTask(AllModuleFirstTask);
        }
        private void OnDestroy()
        {
            ResetAllTasks();
        }
    }
}
