using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFramework.Runtime;

namespace UnityFramework.Runtime
{
    public delegate void ArgsAction(params object[] args);
    public class Timer
    {
        Action timerHandler;           //无参的委托
        ArgsAction timerArgsHandler;   //带参数的委托
        /// <summary>
        /// 标志
        /// </summary>
        public string Flag;
        /// <summary>
        /// 用户设定的时长
        /// </summary>
        public float Frequency;
        /// <summary>
        /// timer事件的预设执行时间
        /// </summary>
        /// <value></value>
        public float PresetTime
        {
            get
            {
                return LastTickTime + Frequency;
            }
        }
        /// <summary>
        /// 计时开始到现在已经过去的时间
        /// </summary>
        public float TimePassed;
        /// <summary>
        /// timer标记的时间,开始计时的时间
        /// </summary>
        public float LastTickTime;
        /// <summary>
        /// 当前时间与timer事件预设执行时间的时间差:PresetTime-CurrentTime
        /// </summary>
        public float TimeOffset;
        /// <summary>
        /// 重复次数-1为无限循环
        /// </summary>
        int repeats;
        /// <summary>
        /// 重复次数-1为无限循环
        /// </summary>
        public int Repeats
        {
            get
            {
                if (repeats < -1)
                {
                    repeats = -1;
                }
                return repeats;
            }
        }
        /// <summary>
        /// 当前剩下重复次数,repeats=-1时无效
        /// </summary>
        public int NowRepeats;
        /// <summary>
        /// 是否显示Log
        /// </summary>
        bool isShowLog;

        /// <summary>
        /// 是否忽略Timescale
        /// </summary>
        bool ignorTimescale;
        /// <summary>
        /// timerArgsHandler的传参
        /// </summary>
        global::System.Object[] args;
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsFinish = false;
        /// <summary>
        /// 是否暂停
        /// </summary>
        bool isPause = false;
        /// <summary>
        /// 是否暂停
        /// </summary>
        /// <value></value>
        public bool IsPause
        {
            get { return isPause; }
            set
            {
                isPause = value;
                if (isPause)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }

        }
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <value></value>
        public float CurrentTime
        {
            get
            {
                return ignorTimescale ? Time.realtimeSinceStartup : Time.time;
            }
        }


        /// <summary>
        /// 计时中每帧执行的事件
        /// </summary>
        event global::System.Action onTiming;
        /// <summary>
        /// 销毁时事件
        /// </summary>
        event global::System.Action onDestroy;


        /// <summary>
        /// 创建一个timer计时器
        /// </summary>
        /// <param name="Handler">无参委托</param>
        /// <param name="ArgsHandler">不限参数类型及个数委托</param>
        /// <param name="onTiming">计时中每帧执行事件</param>
        /// <param name="onDestroy">销毁事件</param>
        /// <param name="flag">timer标志</param>
        /// <param name="frequency">用户设定时间</param>
        /// <param name="repeats">重复次数-1为无限循环</param>
        /// <param name="ignorTimescale">是否忽略Timescale</param>
        /// <param name="isShowLog">是否显示log</param>
        /// <param name="Args">ArgsHandler的传参</param>
        public Timer(global::System.Action Handler, ArgsAction ArgsHandler, global::System.Action onTiming, global::System.Action onDestroy, string flag, float frequency, int repeats, bool ignorTimescale, bool isShowLog, global::System.Object[] Args)
        {
            this.timerHandler = Handler;
            this.timerArgsHandler = ArgsHandler;
            this.onTiming = onTiming;
            this.onDestroy = onDestroy;
            this.Flag = flag;
            this.Frequency = frequency;
            this.repeats = repeats == 0 ? 1 : repeats;
            this.NowRepeats = this.repeats;
            this.ignorTimescale = ignorTimescale;
            this.isShowLog = isShowLog;
            this.LastTickTime = CurrentTime;
            this.args = Args;
            IsFinish = false;
            isPause = false;
        }

        /// <summary>
        /// 暂停timer
        /// </summary>
        private void Pause()
        {
            if (!IsFinish)
            {
                isPause = true;
                if (isShowLog)
                {
                    Log.Info($"{Flag} :Timer暂停");
                }
            }
        }

        /// <summary>
        /// 恢复计时
        /// </summary>
        private void Resume()
        {
            if (!IsFinish)
            {
                LastTickTime = CurrentTime + TimeOffset - Frequency;
                isPause = false;
                if (isShowLog)
                {
                    Log.Info($"{Flag} :Timer恢复");
                }
            }
        }

        public void Start()
        {
            if (isShowLog)
            {
                Log.Info($"{Flag} :计时开始");
            }
        }
        /// <summary>
        /// 执行回调
        /// </summary>
        public void Notify()
        {
            if (isShowLog)
            {
                Log.Info($"{Flag} :Timer事件执行");
            }
            if (timerHandler != null)
            {
                timerHandler?.Invoke();
            }
            if (timerArgsHandler != null)
            {
                timerArgsHandler?.Invoke(args);
            }

        }
        float num = 0;
        /// <summary>
        /// 计时中每帧执行
        /// </summary>
        public void OnTiming()
        {
#if UNITY_EDITOR
            num += Time.deltaTime;
            if (num >= 1)
            {
                num = 0;
                if (isShowLog)
                {
                    Log.Debug($"{Flag} |计时 {TimePassed}秒");
                }
            }
#endif


            if (onTiming != null)
            {
                onTiming?.Invoke();

            }
        }

        /// <summary>
        /// 清理计时器，初始化参数  同时清理事件
        /// </summary>
        public void CleanUp()
        {
            timerHandler = null;
            timerArgsHandler = null;
            repeats = 1;
            Frequency = 0;
            onTiming = null;
            IsFinish = true;
            isPause = false;
            if (onDestroy != null)
            {
                onDestroy?.Invoke();
            }
            onDestroy = null;
            if (isShowLog)
            {
                Log.Info($"{Flag} :Timer删除");
            }
        }

    }
    /// <summary>
    /// 计时器
    /// 添加一个计时事件
    /// 删除一个计时事件
    /// 更新计时事件
    /// </summary>
    [DisallowMultipleComponent]
    public class TimerComponent : UnityFrameworkComponent
    {
        private List<Timer> _Timers = new List<Timer>();//时间管理器
        Timer currentTimer;

        /// <summary>
        /// 生成一个timer计时器
        /// </summary>
        /// <param name="callBack">无参数回调</param>
        /// <param name="onTiming">计时中每帧执行事件</param>
        /// <param name="onDestroy">计时器销毁事件</param>
        /// <param name="flag">计时器标志</param>
        /// <param name="time">用户设定的时长</param>
        /// <param name="repeats">重复次数-1为无限循环</param>
        /// <param name="ignorTimescale">是否忽略Timescale</param>
        /// <param name="isShowLog">是否显示log信息</param>
        /// <returns></returns>
        public Timer CreateTimer(global::System.Action callBack, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats = 1, bool ignorTimescale = true, bool isShowLog = false)
        {
            return Create(callBack, null, onTiming, onDestroy, flag, time, repeats, ignorTimescale, isShowLog);
        }
        public Timer CreateTimer(global::System.Action callBack, float time, int repeats = 1)
        {
            return Create(callBack, null, null, null, "", time, repeats, true, false);
        }

        public Timer CreateTimer(global::System.Action callBack, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats, bool isShowLog)
        {
            return Create(callBack, null, onTiming, onDestroy, flag, time, repeats, true, isShowLog);
        }
        public Timer CreateTimer(global::System.Action callBack, string flag, float time, int repeats = 1, bool ignorTimescale = true, bool isShowLog = false)
        {
            return Create(callBack, null, null, null, flag, time, repeats, ignorTimescale, isShowLog);
        }
        public Timer CreateTimer(global::System.Action callBack, string flag, float time, int repeats, bool isShowLog)
        {
            return Create(callBack, null, null, null, flag, time, repeats, true, isShowLog);
        }
        public Timer CreateTimer(global::System.Action callBack, global::System.Action onTiming, string flag, float time, int repeats = 1, bool ignorTimescale = true, bool isShowLog = false)
        {
            return Create(callBack, null, onTiming, null, flag, time, repeats, ignorTimescale, isShowLog);
        }
        public Timer CreateTimer(global::System.Action callBack, global::System.Action onTiming, string flag, float time, int repeats, bool isShowLog)
        {
            return Create(callBack, null, onTiming, null, flag, time, repeats, true, isShowLog);
        }

        //----------------------------

        public Timer CreateTimerWithArgs(ArgsAction callBack, string flag, float time, int repeats, params global::System.Object[] args)
        {
            return Create(null, callBack, null, null, flag, time, repeats, true, false, args);
        }
        public Timer CreateTimerWithArgs(ArgsAction callBack, string flag, float time, int repeats, bool ignorTimescale, params global::System.Object[] args)
        {
            return Create(null, callBack, null, null, flag, time, repeats, ignorTimescale, false, args);
        }
        public Timer CreateTimerWithArgs(ArgsAction callBack, string flag, float time, int repeats, bool ignorTimescale, bool isShowLog, params global::System.Object[] args)
        {
            return Create(null, callBack, null, null, flag, time, repeats, ignorTimescale, isShowLog, args);
        }

        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, string flag, float time, int repeats, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, null, flag, time, repeats, true, false, args);
        }
        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, string flag, float time, int repeats, bool ignorTimescale, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, null, flag, time, repeats, ignorTimescale, false, args);
        }
        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, string flag, float time, int repeats, bool ignorTimescale, bool isShowLog, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, null, flag, time, repeats, ignorTimescale, isShowLog, args);
        }

        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, onDestroy, flag, time, repeats, true, false, args);
        }
        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats, bool ignorTimescale, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, onDestroy, flag, time, repeats, ignorTimescale, false, args);
        }
        /// <summary>
        /// 生成一个timer计时器
        /// </summary>
        /// <param name="callBackArgs">不限参数类型及个数回调</param>
        /// <param name="onTiming">计时中每帧执行事件</param>
        /// <param name="onDestroy">计时器销毁事件</param>
        /// <param name="flag">计时器标志</param>
        /// <param name="time">用户设定的时长</param>
        /// <param name="repeats">重复次数-1为无限循环</param>
        /// <param name="ignorTimescale">是否忽略Timescale</param>
        /// <param name="isShowLog">是否显示log信息</param>
        /// <param name="args">callBackArgs传递的参数</param>
        /// <returns></returns>
        public Timer CreateTimerWithArgs(ArgsAction callBack, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats, bool ignorTimescale, bool isShowLog, params global::System.Object[] args)
        {
            return Create(null, callBack, onTiming, onDestroy, flag, time, repeats, ignorTimescale, isShowLog, args);
        }



        /// <summary>
        /// 生成一个timer计时器
        /// </summary>
        /// <param name="callBack">无参数回调</param>
        /// <param name="callBackArgs">不限参数类型及个数回调</param>
        /// <param name="onTiming">计时中每帧执行事件</param>
        /// <param name="onDestroy">计时器销毁事件</param>
        /// <param name="flag">计时器标志</param>
        /// <param name="time">用户设定的时长</param>
        /// <param name="repeats">重复次数-1为无限循环</param>
        /// <param name="ignorTimescale">是否忽略Timescale</param>
        /// <param name="isShowLog">是否显示log信息</param>
        /// <param name="args">callBackArgs传递的参数</param>
        /// <returns></returns>
        private Timer Create(global::System.Action callBack, ArgsAction callBackArgs, global::System.Action onTiming, global::System.Action onDestroy, string flag, float time, int repeats, bool ignorTimescale, bool isShowLog, params global::System.Object[] args)
        {
            Timer timer = new Timer(callBack, callBackArgs, onTiming, onDestroy, flag, time, repeats, ignorTimescale, isShowLog, args);
            _Timers.Add(timer);
            timer.Start();
            return timer;
        }
        /// <summary>
        /// 恢复计时
        /// </summary>
        /// <param name="flag">计时器标志</param>
        /// <returns></returns>
        public Timer Resume(string flag)
        {
            Timer timer = _Timers.Find((x) => { return x.Flag == flag; });
            if (timer != null)
            {
                timer.IsPause = false;
            }
            return timer;
        }
        /// <summary>
        /// 恢复计时
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public Timer Resume(Timer timer)
        {
            if (timer != null)
            {
                timer.IsPause = false;
            }
            return timer;
        }
        /// <summary>
        /// 暂停一个计时器
        /// </summary>
        /// <param name="flag">计时器标志</param>
        /// <returns></returns>
        public Timer Pause(string flag)
        {
            Timer timer = _Timers.Find((x) => { return x.Flag == flag; });
            if (timer != null)
            {
                timer.IsPause = true;
            }
            return timer;
        }
        /// <summary>
        /// 暂停一个计时器
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public Timer Pause(Timer timer)
        {
            if (timer != null)
            {
                timer.IsPause = true;
            }
            return timer;
        }
        /// <summary>
        /// 删除一个计时器
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public Timer DestroyTimer(Timer timer)
        {
            if (timer != null)
            {
                _Timers.Remove(timer);
                timer.CleanUp();
                timer = null;
            }
            return timer;
        }
        /// <summary>
        /// 删除一个计时器
        /// </summary>
        /// <param name="flag">计时器标志</param>
        /// <returns></returns>
        public Timer DestroyTimer(string flag)
        {
            Timer timer = _Timers.Find((x) => { return x.Flag == flag; });
            if (timer != null)
            {
                _Timers.Remove(timer);
                timer.CleanUp();
                timer = null;
            }
            return timer;
        }
        /// <summary>
        /// 删除所有计时器
        /// </summary>
        public void ClearAll()
        {
            if (_Timers != null)
            {
                for (int i = 0; i < _Timers.Count; i++)
                {
                    _Timers[i].CleanUp();
                }
                _Timers.Clear();
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        void RunningTimer()
        {
            if (_Timers != null && _Timers.Count != 0)
            {
                for (int i = _Timers.Count - 1; i >= 0; i--)
                {
                    if (i > _Timers.Count - 1)
                    {
                        return;
                    }
                    Timer timer = _Timers[i];
                    if (!timer.IsFinish && !timer.IsPause)
                    {
                        if (timer.NowRepeats <= 0 & timer.Repeats != -1)
                        {
                            //计时完成，可以删除了
                            DestroyTimer(timer);
                            return;
                        }
                        timer.TimePassed = timer.CurrentTime - timer.LastTickTime;


                        timer.TimeOffset = timer.LastTickTime + timer.Frequency - timer.CurrentTime;
                        timer.OnTiming();


                        //标记的时间LastTickTime+时间间隔Frequency=计时器执行事件的时间
                        //如果curTime <计时器执行事件的时间 就返回
                        //或者当前时间curTime-标记的时间LastTickTime> 时间间隔Frequency就执行事件
                        if (timer.PresetTime > timer.CurrentTime)
                        {
                            continue;
                        }
                        timer.LastTickTime = timer.CurrentTime;
                        timer.NowRepeats--;
                        timer.Notify();
                    }
                }
            }
        }
        protected override void Awake()
        {
            base.Awake();
            Application.runInBackground = true;
        }
        void Update()
        {
            RunningTimer();
        }
        void OnDestroy()
        {
            ClearAll();
        }
    }

}