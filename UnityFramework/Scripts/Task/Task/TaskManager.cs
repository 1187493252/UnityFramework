#if VIU_STEAMVR_2_0_0_OR_NEWER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework.Runtime.Task
{
    public class TaskManager : MonoSingleton<TaskManager>
    {

        public List<TaskConf> passTasks;

        /// <summary>
        /// 第一个教学任务
        /// </summary>
        public TaskConf firstTask;
        /// <summary>
        /// 最后一个结算任务
        /// </summary>
        public TaskConf finalTask;
        /// <summary>
        /// 所有的任务
        /// </summary>
        public List<TaskConf> allTasks;
        /// <summary>
        /// 大任务节点
        /// </summary>
        public List<TaskConf> bigTasks;
        /// <summary>
        /// 任务模式 
        /// </summary>
        [SerializeField]
        private TaskMode taskMode = TaskMode.Study;




        /// <summary>
        /// 任务记录
        /// </summary>
        public Dictionary<TaskMode, List<TaskConf>> dicRecordTasks;
        public Dictionary<string, TaskConf> titleTaskMap;
        public event Action<TaskConf> OnTaskChange;
        private Coroutine coroutineDoing;
        private Coroutine coroutineDelayTask;

        public List<TaskConf> examingList = new List<TaskConf>();
        public List<TaskConf> trainintList = new List<TaskConf>();
        public List<string> errorBigTasks = new List<string>();
        /// <summary>
        /// 开始考试模式的时间
        /// </summary>
        public DateTime startExcameTime;


        /// <summary>
        /// 任务模式
        /// </summary>
        public TaskMode TaskMode
        {
            get
            {
                return taskMode;
            }
            set
            {
                taskMode = value;
            }
        }

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

        /// <summary>
        /// 当前任务
        /// </summary>
        private TaskConf mCurrentTask;
        public TaskConf CurrentTask
        {
            get
            {
                return mCurrentTask;
            }
            set
            {
                mCurrentTask = value;
            }
        }


        private void InitTasks()
        {

            TaskConf nextTask = firstTask;
            while (nextTask != null)
            {
                nextTask.TaskState = TaskState.UnInit;
                nextTask.TaskState = TaskState.Init;
                nextTask = GetNextTaskLittle(nextTask);

                //if (firstTask.index < 13 && nextTask != null)
                //{
                //    if (nextTask.index == 35|| nextTask.index == 39 || nextTask.index == 41)
                //        nextTask = GetNextTaskLittle(nextTask);
                //    if (nextTask !=null && nextTask.index == 42)
                //        nextTask = GetNextTaskLittle(nextTask);
                //}
            }
            errorBigTasks.Clear();
            //CarModelCtl.inst.SetInstal(true);
            //ToolsModelCtl.inst.SetInstal(true);
            //PlaceItemsCtl.inst.SetInstal(true);
            Debug.Log("——————————————————————————AllTaskInit--Over——————————————————————————");
        }

        void Awake()
        {

            if (allTasks == null)
                allTasks = new List<TaskConf>();
            if (bigTasks == null)
                bigTasks = new List<TaskConf>();
            if (dicRecordTasks == null)
                dicRecordTasks = new Dictionary<TaskMode, List<TaskConf>>();

            ReadTasks(firstTask);
            GetCanSkipTasks();

            startExcameTime = DateTime.Now;
        }

        /// <summary>
        /// 任务的目标自定义反射类列表
        /// </summary>
        private List<Type> customLogicList;



        private void OnVRHandActive()
        {
            if (VRHandHelper.Instance)
            {
                VRHandHelper.Instance.OnVRHandActive -= OnVRHandActive;
            }

        }

        public void StartTask()
        {
            InitTasks();
            CurrentTask = firstTask;
            ExecuteTask(CurrentTask);
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        private void ExecuteTask(TaskConf taskConf, bool isJump = false)
        {
            if (CurrentTask == null)
                return;

            TaskConf tempTask = CurrentTask;
            //首先判断该任务是否跳步，如果跳步，做跳步处理      
            int compareResult = CompareTaskPriority(taskConf, tempTask);
            if (taskConf != firstTask && TaskMode == TaskMode.Exam && compareResult <= 0)
                return;


            //保存已通过的任务 方便调试
            passTasks.Add(CurrentTask);
            CurrentTask = taskConf;

            //追加特殊的跳步判断和数据处理 
            DetectSpecialSkipTask(compareResult);

            //CurrentTask.taskState = TaskState.Init;

            //向前执行跳步
            if (compareResult < 0)
            {
                //重新设置两个任务之间的状态      
                TaskConf last = tempTask;
                while (last && last != taskConf)
                {
                    last.isJumpInit = true;
                    last.TaskState = TaskState.Init;
                    last.isJumpInit = false;

                    //往回跳的状态记录恢复处理 
                    //问题太多难以解决 现改为固定重置为跳过状态

                    ////如果任务已经完成 就不再管跳过状态了
                    if (isJump && !last.IsPass)
                        last.IsSkip = true;
                    else
                        last.IsSkip = false;

                    last = GetLastNode(last);
                }
                CurrentTask.TaskState = TaskState.Start;
            }
            //向后执行跳步
            else if (compareResult > 0)
            {
                //跳步到这个任务之后的任务
                TaskConf next = tempTask;
                while (next && next != taskConf)
                {

                    next.isJumpEnd = isJump;
                    //如果任务已经完成 就不再管跳过状态了
                    if (isJump && !next.IsPass)
                        next.IsSkip = true;
                    else
                        next.IsSkip = false;

                    if (TaskMode == TaskMode.Study)
                    {
                        if (next.taskType == TaskType.OnlyStudy)
                        {
                            next.TaskState = TaskState.End;
                            next = GetNextNode(next);
                        }
                        else if (next.taskType == TaskType.OnlyExam)
                        {
                            next.SetTaskSuccess(true, false);//非当前模式的任务直接标记为已完成
                            next = GetNextNode(next);
                        }
                        else
                        {

                            next.TaskState = TaskState.End;
                            next = GetNextNode(next);
                        }
                    }
                    else if (TaskMode == TaskMode.Exam)
                    {
                        if (next.taskType == TaskType.OnlyStudy)
                        {
                            next.SetTaskSuccess(true, false);//非当前模式的任务直接标记为已完成
                            next = GetNextNode(next);
                        }
                        else if (next.taskType == TaskType.OnlyExam)
                        {
                            next.TaskState = TaskState.End;
                            next = GetNextNode(next);
                        }
                        else
                        {
                            if (next.taskType == TaskType.OnlyExam)
                            {
                                next = GetNextNode(next);
                            }
                            else
                            {

                                next.TaskState = TaskState.End;
                                next = GetNextNode(next);
                            }
                        }
                    }
                    else
                    {
                        next.TaskState = TaskState.End;
                        next = GetNextNode(next);
                    }
                    next.isJumpEnd = false;

                    //Debug.Log(next.index);
                }
                CurrentTask.TaskState = TaskState.Start;
            }
            else
            {
                //重新执行这个任务
                CurrentTask.TaskState = TaskState.Start;
            }





            OnTaskChange?.Invoke(CurrentTask);
        }

        private IEnumerator ExecuteTaskDelay(TaskConf taskConf, bool isJump = false)
        {
            yield return new WaitForSeconds(taskConf.delayTimeToStart);
            ExecuteTask(taskConf, isJump);
        }

        private void ResetTasks()
        {
            allTasks.ForEach(t =>
            {
                t.InitTask();
                t.TaskState = TaskState.UnInit;
            });
        }

        private void ReadTasks(TaskConf first)
        {
            if (!firstTask)
                return;

            allTasks.Clear();
            bigTasks.Clear();

            TaskConf nextTask = first;
            int index = 0;

            while (nextTask)
            {
                if (nextTask)
                {

                    nextTask.traingScore = 0;
                    nextTask.examScore = 0;


                }



                TaskConf repetitionTask = allTasks.Find(t => t.name == nextTask.name);
                if (repetitionTask != null)
                    Debug.LogWarning("注意:存在相同名字的任务:" + repetitionTask.name);

                nextTask.RecoverTaskStatus();
                allTasks.Add(nextTask);
                nextTask.index = index++;
                nextTask = (TaskConf)nextTask.NextNode;
            }

            TaskConf bigNextTask = first;
            while (bigNextTask)
            {
                bigTasks.Add(bigNextTask);
                bigNextTask = (TaskConf)bigNextTask.NextBigNode;
            }
            //Debug.Log("taskNum:" + taskNum + "  scoreListNum:" + scoreList.Count+"   TaskName:"+ taskName);
            //读完任务要重设状态
            ResetTasks();
        }












        public void StartDoingCoroutine(TaskConf taskConf)
        {

            coroutineDoing = StartCoroutine(SetDoingState(taskConf));
        }

        public void StopDoingCoroutine()
        {
            if (coroutineDoing != null)
            {
                StopCoroutine(coroutineDoing);
                coroutineDoing = null;
            }
        }


        private IEnumerator SetDoingState(TaskConf taskConf)
        {
            yield return new WaitForSeconds(taskConf.delayTimeStartToDoing);
            while (true)
            {
                taskConf.TaskState = TaskState.Doing;
                yield return null;
            }
        }

        /// <summary>
        /// 下个任务，包括小任务
        /// </summary>
        public void ExecuteNextTaskLittle(bool isJump = false)
        {
            if (coroutineDelayTask != null)
            {
                StopCoroutine(coroutineDelayTask);
                coroutineDelayTask = null;
            }

            if (CurrentTask)
            {
                TaskConf nextTaskLittle = GetNextTaskLittle(CurrentTask);

                if (nextTaskLittle != null)
                    coroutineDelayTask = StartCoroutine(ExecuteTaskDelay(nextTaskLittle, isJump));
                else
                    OnCompletAllTask();
            }
        }


        public void ExecuteNextTaskBig(bool isJump = false)
        {
            if (coroutineDelayTask != null)
            {
                StopCoroutine(coroutineDelayTask);
                coroutineDelayTask = null;
            }

            if (CurrentTask)
            {
                TaskConf nextTaskBig = GetNextTaskBig(CurrentTask);
                if (nextTaskBig != null)
                    coroutineDelayTask = StartCoroutine(ExecuteTaskDelay(nextTaskBig, isJump));
                else
                {
                    coroutineDelayTask = StartCoroutine(ExecuteTaskDelay(allTasks[allTasks.Count - 1], isJump));
                    OnCompletAllTask();
                }

            }
        }

        /// <summary>
        /// 跳步到大任务
        /// </summary>
        /// <param name="taskConf"></param>
        public void SkipTask(TaskConf taskConf)
        {
            if (coroutineDelayTask != null)
            {
                StopCoroutine(coroutineDelayTask);
                coroutineDelayTask = null;
            }
            coroutineDelayTask = StartCoroutine(ExecuteTaskDelay(taskConf, true));
        }

        private bool CheckCanSkip(TaskConf taskConf)
        {
            if (TaskMode == TaskMode.Exam)
            {
                if (taskConf.isCanRecord && taskConf.taskType != TaskType.OnlyStudy)
                    return true;
            }
            else
            {
                if (taskConf.isCanRecord && taskConf.taskType != TaskType.OnlyExam)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取任务的下一个节点，不考虑模式，只是下一个节点
        /// </summary>
        /// <param name="taskConf"></param>
        public TaskConf GetNextNode(TaskConf taskConf)
        {
            if (taskConf.index < allTasks.Count - 1)
            {
                return allTasks[taskConf.index + 1];
            }
            else
                return null;
        }

        public TaskConf GetNextBigNode(TaskConf taskConf)
        {
            return (TaskConf)taskConf.NextBigNode;
        }
        /// <summary>
        /// 获取任务的上一个节点，不考虑模式，只是上一个节点
        /// </summary>
        /// <param name="taskConf"></param>
        public TaskConf GetLastNode(TaskConf taskConf)
        {
            if (taskConf.index > 0)
                return allTasks[taskConf.index - 1];
            else
                return null;
        }


        /// <summary>
        /// 下一个小任务:考虑任务模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConf GetNextTaskLittle(TaskConf taskConf)
        {
            TaskConf next = GetNextNode(taskConf);
            if (TaskMode == TaskMode.Exam)
            {
                while (next != null)
                {
                    if (next.taskType == TaskType.OnlyStudy)
                        next = GetNextNode(next);
                    else
                        break;
                }
                return next;
            }
            else if (TaskMode == TaskMode.Study)
            {
                while (next != null)
                {
                    if (next.taskType == TaskType.OnlyExam)
                        next = GetNextNode(next);
                    else
                        break;
                }
                return next;
            }
            else
                return next;
        }


        /// <summary>
        /// 下一个小任务:考虑任务模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConf GetLastTaskLittle(TaskConf taskConf)
        {
            TaskConf last = GetLastNode(taskConf);
            if (TaskMode == TaskMode.Exam)
            {
                while (last != null)
                {
                    if (last.taskType == TaskType.OnlyStudy)
                        last = GetLastNode(last);
                    else
                        break;
                }
                return last;
            }
            else if (TaskMode == TaskMode.Study)
            {
                while (last != null)
                {
                    if (last.taskType == TaskType.OnlyExam)
                        last = GetLastNode(last);
                    else
                        break;
                }
                return last;
            }
            else
                return last;
        }

        /// <summary>
        /// 下一个大任务:考虑任务模式
        /// </summary>
        /// <param name="taskConf"></param>
        /// <returns></returns>
        public TaskConf GetNextTaskBig(TaskConf taskConf)
        {
            TaskConf next = GetNextBigNode(taskConf);
            if (TaskMode == TaskMode.Exam)
            {
                while (next != null)
                {
                    if (next.taskType == TaskType.OnlyStudy || !next.isCanRecord)
                        next = GetNextBigNode(next);
                    else
                        break;
                }
                return next;
            }
            else
            {
                while (next != null)
                {
                    if (next.taskType == TaskType.OnlyExam || !next.isCanRecord)
                        next = GetNextBigNode(next);
                    else
                        break;
                }
                return next;
            }
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public void RestartTask()
        {
            TaskManager.Instance.TaskMode = TaskMode.Default;
            //   InitTasks();
            StartTask();
        }

        /// <summary>
        /// 获取工具在当前任务的状态信息
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public ToolTaskInfo GetToolInitInfo(ToolConf tool)
        {
            //先读前面的任务配置 找之前任务最后工具状态
            TaskConf last = CurrentTask;

            ToolTaskInfo toolTaskInfo = new ToolTaskInfo();
            while (last)
            {
                foreach (ToolTaskConf toolTask in last.TaskTools)
                {
                    if (toolTask.toolConf == tool && toolTask != CurrentTask)
                    {
                        toolTaskInfo.isSetCanHover = toolTask.isSetEndCanHover;
                        toolTaskInfo.isCanHover = toolTask.isEndCanHover;
                        toolTaskInfo.isSetCanCatch = toolTask.isSetEndCanCatch;
                        toolTaskInfo.isCanCatch = toolTask.isEndCanCatch;
                        toolTaskInfo.isSetKinematic = toolTask.isSetEndKinematic;
                        toolTaskInfo.isKinematic = toolTask.isEndKinematic;
                        toolTaskInfo.isSetHide = toolTask.isSetEndHide;
                        toolTaskInfo.isHide = toolTask.isEndHide;
                        toolTaskInfo.isSetHighlight = toolTask.isSetEndHighlight;
                        toolTaskInfo.isHighlight = toolTask.isEndHighlight;
                        toolTaskInfo.isSetScaleSize = toolTask.isSetEndScaleSize;
                        toolTaskInfo.scaleSize = toolTask.endScaleSize;
                        toolTaskInfo.isSetPose = toolTask.isSetEndPose;
                        toolTaskInfo.isSetParent = toolTask.isSetParent;
                        toolTaskInfo.setParentTool = toolTask.parentTool;
                        toolTaskInfo.position = toolTask.EndPosition;
                        toolTaskInfo.angle = toolTask.EndAngle;
                        toolTaskInfo.indexAoCao = toolTask.aocaoStartIndex;
                        toolTaskInfo.isSetColliderStart = toolTask.isSetStartCollider;
                        toolTaskInfo.isColliderStart = toolTask.isStartCollider;
                        //print("toolTask--" + toolTask);
                        //print("toolTaskInfo.indexAoCao--" + toolTaskInfo.indexAoCao);
                        return toolTaskInfo;
                    }
                }
                last = GetLastNode(last);
            }


            toolTaskInfo.isSetCanHover = tool.isSetInitCanHover;
            toolTaskInfo.isCanHover = tool.isInitCanHover;
            toolTaskInfo.isSetCanCatch = tool.isSetInitCanCatch;
            toolTaskInfo.isCanCatch = tool.isInitCanCatch;
            toolTaskInfo.isSetKinematic = tool.isSetInitKinematic;
            toolTaskInfo.isKinematic = tool.isInitKinematic;
            toolTaskInfo.isSetHide = tool.isSetInitHide;
            toolTaskInfo.isHide = tool.isInitHide;
            toolTaskInfo.isSetHighlight = tool.isSetInitHighlight;
            toolTaskInfo.isHighlight = tool.isInitHighlight;
            toolTaskInfo.isSetScaleSize = tool.isSetInitScaleSize;
            toolTaskInfo.scaleSize = tool.InitScaleSize;
            toolTaskInfo.isSetPose = tool.isSetInitPose;
            toolTaskInfo.position = tool.InitPosition;
            toolTaskInfo.isSetParent = false;
            toolTaskInfo.angle = tool.InitAngle;
            toolTaskInfo.isSetColliderStart = tool.isSetColliderStart;
            toolTaskInfo.isColliderStart = tool.isCollider;
            return toolTaskInfo;
        }


        /// <summary>
        /// 完成所有任务
        /// </summary>
        private void OnCompletAllTask()
        {



        }


        void OnDestroy()
        {
            ResetTasks();
        }

        /// <summary>
        /// 比较任务优先级，返回结果小于零 大于零 等于零
        /// </summary>
        /// <param name="taskConf0"></param>
        /// <param name="taskConf1"></param>
        /// <returns></returns>
        private int CompareTaskPriority(TaskConf taskConf0, TaskConf taskConf1)
        {
            return taskConf0.index - taskConf1.index;
        }

        private void Start()
        {
            StartTask();
        }


        /// <summary>
        /// 获取可以跳转的任务
        /// </summary>
        /// <returns></returns>
        private void GetCanSkipTasks()
        {
            List<TaskConf> canSkipExaminationTasks = new List<TaskConf>();
            List<TaskConf> canSkipTrainingTasks = new List<TaskConf>();

            examingList.Clear();
            trainintList.Clear();

            allTasks.ForEach(t =>
            {
                if (t.isCanRecord && t.taskType != TaskType.OnlyStudy)
                    canSkipExaminationTasks.Add(t);

                if (t.isCanRecord && t.taskType != TaskType.OnlyExam)
                    canSkipTrainingTasks.Add(t);
            });


            examingList = GetTaskConfsByModel(TaskMode.Exam);

            trainintList = GetTaskConfsByModel(TaskMode.Study);


            dicRecordTasks.Add(TaskMode.Exam, canSkipExaminationTasks);
            dicRecordTasks.Add(TaskMode.Study, canSkipTrainingTasks);
        }

        /// <summary>
        /// 完成所有任务，获取任务完成情况
        /// </summary>
        /// <returns></returns>
        public List<TaskConf> GetTaskRecords()
        {
            List<TaskConf> taskRecords = new List<TaskConf>();
            if (TaskMode == TaskMode.Exam)
                return dicRecordTasks[TaskMode.Exam];
            else
                return dicRecordTasks[TaskMode.Study];
        }







        /// <summary>
        /// 获取当前任务后面所有的子任务配置
        /// </summary>
        /// <param name="compareResult"></param>
        public List<TaskConf> GetBigTaskAllChild(TaskConf bigTask)
        {
            List<TaskConf> taskConfs = new List<TaskConf>();
            if (bigTask == null) return taskConfs;

            Debug.Log("读取任务组:当前任务" + bigTask.name + " index:" + bigTask.index);
            taskConfs.Add(bigTask);
            TaskConf nextTask = GetNextTaskLittle(CurrentTask);
            while (nextTask != null && nextTask.Parent != null)
            {
                Debug.Log("读取任务组:当前任务后续子任务" + nextTask.name + " index:" + nextTask.index);
                taskConfs.Add(nextTask);
                nextTask = GetNextTaskLittle(nextTask);
            }
            return taskConfs;
        }

        /// <summary>
        /// 如果步数差异为负数(往回跳),0(不变),>1(向后跳大任务)  
        /// 判定为特殊跳步(非正常向后运行流程)
        /// 对后续的任务数据进行标记处理
        /// </summary>
        /// <param name="compareResult"></param>
        public void DetectSpecialSkipTask(int compareResult)
        {
            if (compareResult < 0 || compareResult > 1)
            {
                //讲当前任务所属的大任务后续步骤全部判定为特殊跳步 对任务数据进行标记
                CurrentTask.isSkipJumpStart = true;
                Debug.Log("特殊跳步" + CurrentTask.name + " index:" + CurrentTask.index);
                TaskConf nextTask = GetNextTaskLittle(CurrentTask);
                while (nextTask != null && nextTask.Parent != null)
                {
                    Debug.Log("特殊跳步" + nextTask.name + " index:" + nextTask.index);
                    nextTask.isSkipJumpStart = true;
                    nextTask = GetNextTaskLittle(nextTask);
                }
            }
        }

        /// <summary>
        /// 根据游戏模式 获取所有任务 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<TaskConf> GetAllTaskConfsByMode(TaskMode mode)
        {
            List<TaskConf> resual = new List<TaskConf>();
            for (int i = 0; i < allTasks.Count; i++)
            {

                switch (mode)
                {
                    case TaskMode.Study:
                        if (allTasks[i].taskType == TaskType.All || allTasks[i].taskType == TaskType.OnlyStudy)
                            resual.Add(allTasks[i]);
                        break;
                    case TaskMode.Exam:
                        if (allTasks[i].taskType == TaskType.All || allTasks[i].taskType == TaskType.OnlyExam)
                            resual.Add(allTasks[i]);
                        break;
                }
            }

            return resual;
        }



        /// <summary>
        /// 获取当前模块下所有大任务配置 
        /// 目前取法是获取所有可跳并且配置有错误反馈任务
        /// </summary>
        /// <param name="IncludingNoScore">是否要包括没有分数和错误反馈的任务</param>
        /// <returns></returns>
        public List<TaskConf> GetAllBigTaskConfs(bool includingNoScore = false)
        {
            List<TaskConf> allBigTaskConfs = new List<TaskConf>();

            List<TaskConf> taskList = dicRecordTasks[TaskMode];
            if (taskList != null && taskList.Count > 0)
            {
                for (int i = 0; i < taskList.Count; i++)
                {
                    TaskConf conf = taskList[i];
                    if (includingNoScore)
                    {
                        allBigTaskConfs.Add(conf);

                    }

                }
            }
            return allBigTaskConfs;
        }

        /// <summary>
        /// 获取当前模块下所有任务的分数
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public float GetAllTaskScore()
        {
            float score = 0;
            List<TaskConf> taskList = GetAllBigTaskConfs();
            List<BigTaskStatus> taskStatusList = new List<BigTaskStatus>();
            for (int i = 0; i < taskList.Count; i++)
            {
                TaskConf conf = taskList[i];
                BigTaskStatus item = CheckTaskGroupStatus(conf, TaskMode);
                taskStatusList.Add(item);
            }
            score = GetAllTaskScore(taskStatusList);
            return score;
        }

        /// <summary>
        /// 获取当前模块下所有任务的分数
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public float GetAllTaskScore(List<BigTaskStatus> taskList)
        {
            float score = 0;

            for (int i = 0; i < taskList.Count; i++)
            {
                BigTaskStatus item = taskList[i];

                if (item.score > 0)
                    score += item.score;
            }
            return score;
        }



        /// <summary>
        /// 根据任务名字(任务配置文件的taskName) 获取所有任务 
        /// </summary>
        /// <param name="mode">taskName</param>
        /// <returns></returns>
        public TaskConf GetTaskConfsByName(string taskName)
        {
            TaskConf task = allTasks.Find(t => t.taskName == taskName);
            return task;
        }


        /// <summary>
        /// 根据课程模式 获取所有任务 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<TaskConf> GetTaskConfsByModel(TaskMode model)
        {
            List<TaskConf> tasks = new List<TaskConf>();
            switch (model)
            {
                case TaskMode.Study:
                    tasks = allTasks.FindAll(t => t.taskType == TaskType.All || t.taskType == TaskType.OnlyStudy);
                    break;
                case TaskMode.Exam:
                    tasks = allTasks.FindAll(t => t.taskType == TaskType.All || t.taskType == TaskType.OnlyExam);
                    break;
                default:
                    break;
            }
            return tasks;
        }



        /// <summary>
        /// 获取当前任务的状态信息 (大任务下所有子任务是否全部成功完成)
        /// 任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckTaskSuccess(TaskConf task)
        {
            if (task == null)
            {
                Debug.LogError("CheckTaskStatus task为空");
                return false;
            }
            if (task.TaskGoals == null || task.TaskGoals.Count == 0)
                return true;

            bool nowTaskItemSuccess = true; //当前任务组中的子任务是否成功

            for (int j = 0; j < task.TaskGoals.Count; j++)
            {
                //Debug.Log("taskname--" + task + "  " + "goalName--" + task.taskGoals[j] + "  " + "IsTheGoalDone========" + task.taskGoals[j].IsTheGoalDone);

                if (!task.TaskGoals[j].CheckAchieveGoal())
                {
                    nowTaskItemSuccess = false;//任何一个目标失败 都标记这个任务存在错误
                    break;
                }
            }

            return nowTaskItemSuccess;
        }




        public bool CheckTaskGroupSuccess(TaskConf task)
        {
            bool isSuccess = true;
            TaskGroupSuccess(task, ref isSuccess);
            return isSuccess;
        }

        /// <summary>
        /// 根据task是否包含child子任务 判断目标任务或其下所有子任务的完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        void TaskGroupSuccess(TaskConf task, ref bool isSuccess)
        {
            if (task == null)
            {
                Debug.LogError("CheckTaskStatus task为空");
                return;
            }

            if (isSuccess == false)
                return;

            if (task.Child == null)
            {
                isSuccess = CheckTaskSuccess(task);
            }
            else
            {
                List<TaskConf> tasks = GetChildTask(task);
                for (int i = 0; i < tasks.Count; i++)
                {
                    TaskGroupSuccess(tasks[i], ref isSuccess);
                }
            }
        }


        /// <summary>
        /// 当前任务组是否已经执行过
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool CheckTaskHasExecute(TaskConf task)
        {
            if (task == null)
            {
                Debug.LogError("CheckTaskStatus task为空");
                return false;
            }

            bool nowTaskDone = false; //当前任务组中的子任务是否成功
            TaskHasExecute(task, ref nowTaskDone);
            return nowTaskDone;
        }

        bool CheckTaskExecute(TaskConf task)
        {
            bool nowTaskDone = false; //当前任务组中的子任务是否执行过

            for (int j = 0; j < task.TaskGoals.Count; j++)
            {
                if (task.TaskGoals[j].notCalculate)
                    continue;

                if (task.TaskGoals[j].HasDone)
                {
                    nowTaskDone = true;
                    break;
                }
            }
            return nowTaskDone;
        }


        void TaskHasExecute(TaskConf task, ref bool isHasdone)
        {
            if (task == null)
            {
                return;
            }

            if (isHasdone == true)
                return;

            if (task.Child == null)
            {
                isHasdone = CheckTaskExecute(task);
            }
            else
            {
                List<TaskConf> tasks = GetChildTask(task);
                for (int i = 0; i < tasks.Count; i++)
                {
                    TaskHasExecute(tasks[i], ref isHasdone);
                }
            }
        }





        /// <summary>
        /// 获取当前大任务的状态信息 (大任务下所有子任务是否全部成功完成)
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public BigTaskStatus CheckTaskGroupStatus(TaskConf task, TaskMode type)
        {
            List<TaskConf> taskGroup = GetChildTask(task, true);
            BigTaskStatus status = new BigTaskStatus();

            for (int i = 0; i < taskGroup.Count; i++)
            {
                if (taskGroup[i].taskType == TaskType.All || ((int)taskGroup[i].taskType - 1) == (int)type)
                {
                    bool nowTaskItemSuccess = CheckTaskGroupSuccess(taskGroup[i]); //当前任务组中的子任务是否成功
                    if (status.isSuccess && nowTaskItemSuccess == false) //任何一个子任务失败 都标记这个大任务存在错误
                    {
                        status.isSuccess = false;
                    }

                }
            }
            return status;
        }





        /// <summary>
        /// 获取指定任务所属组中所有任务(其下的第一个子任务,第一个子任务的所有同级任务)
        /// </summary>
        /// <param name="task"></param>
        /// <param name="hasSelf">是否要包含任务本身</param>
        /// <returns></returns>
        public List<TaskConf> GetChildTask(TaskConf task, bool hasSelf = false)
        {
            List<TaskConf> taskGroup = new List<TaskConf>();
            if (hasSelf)
                taskGroup.Add(task);
            TaskConf conf;
            if (task.Child != null)
            {
                conf = task.Child as TaskConf;
                taskGroup.Add(conf);
                //Debug.Log("");
                while (conf.Next != null)
                {
                    conf = conf.Next as TaskConf;
                    taskGroup.Add(conf);
                }
            }

            return taskGroup;
        }


        public void RecoverAllTaskStatus()
        {
            List<TaskConf> tasks = new List<TaskConf>();
            switch (TaskManager.Instance.TaskMode)
            {
                case TaskMode.Study:
                    tasks = trainintList;
                    break;
                case TaskMode.Exam:
                    tasks = examingList;
                    break;

                default:
                    tasks = trainintList;
                    break;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                TaskConf t = tasks[i];
                t.RecoverTaskStatus();
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                SkipTask(GetNextTaskLittle(CurrentTask));
        }
    }


    public class BigTaskStatus
    {
        /// <summary>
        /// 大任务组是否全部成功完成任务
        /// </summary>
        public bool isSuccess = true;
        /// <summary>
        /// 大任务组是否有跳过步骤 
        /// 跳过状态和错误严重等级状态需要共存
        /// </summary>
        public bool isSkip = false;
        /// <summary>
        /// 大任务组获得的总分数
        /// </summary>
        public float score = 0;

        /// <summary>
        /// 当前大任务下所有任务配置 
        /// </summary>
        public List<TaskConf> task = new List<TaskConf>();

    }


}

#endif