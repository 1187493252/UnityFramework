
/// <summary>
/// 文件类型
/// </summary>
public enum FileType
{
    txt,
    json,
    xml,
    doc,
    docx,
    xls,
    xlsx,
    mp3,
    wav,
    mp4,
    jpg,
    png,
    html,
    all
}

public enum GoalReslut
{
    Init = -1,
    Error = 0,
    Sucess = 1
}
/// <summary>
/// TaskState
/// 任务状态
/// </summary>
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
public enum TaskType
{
    All = 0,
    OnlyStudy = 1,
    OnlyExam = 2
}

public enum WhichHand
{
    RightHand,
    LeftHand,
    Both
}
public enum GrabButton
{
    Trigger = 0,
    Grip = 1
}
public enum GrabType
{
    HoldDown = 0,
    Toggle = 1
}
public enum PluginOption
{
    VRTK,
    ViveInputUtility,
    Oculus
#if VIU_STEAMVR_2_0_0_OR_NEWER
            ,
    SteamVR
#endif
}

/// <summary>
/// 数据加载模式
/// </summary>
public enum ConfigLoadMode
{
    /// <summary>
    /// 从数据缓存中读取
    /// </summary>
    DataCache,
    /// <summary>
    /// 从单独路径读取
    /// </summary>
    Path
}
/// <summary>
/// 资源加载模式
/// </summary>
public enum ResourceLoadMode
{
    Resource,
    StreamingAssets,
    WebRequest//服务器
}
/// <summary>
/// 模式
/// </summary>
public enum GameMode
{
    /// <summary>
    /// 学习模式
    /// </summary>
    Study,
    /// <summary>
    /// 实训模式
    /// </summary>
    Training,
    /// <summary>
    /// 考核模式
    /// </summary>
    Check
}

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    /// <summary>
    /// 游戏未开始
    /// </summary>
    NotStarted,

    /// <summary>
    /// 游戏开始
    /// </summary>
    GameStart,
    /// <summary>
    /// 游戏进行中
    /// </summary>
    GamePlaying,
    /// <summary>
    /// 游戏暂停
    /// </summary>
    GamePause,
    /// <summary>
    /// 游戏结束
    /// </summary>
    GameOver
}

/// <summary>
/// 播放状态:动画,音频
/// </summary>
public enum PlayState
{
    /// <summary>
    /// 播放
    /// </summary>
    Play,

    /// <summary>
    /// 暂停
    /// </summary>
    Pause,
    /// <summary>
    /// 停止
    /// </summary>
    Stop
}

/// <summary>
/// 渲染模式
/// </summary>
public enum RenderingMode
{
    Opaque,
    Cutout,
    Fade,
    Transparent,
}
/// <summary>
/// 动画类型
/// </summary>
public enum AnimType
{
    Animation,
    Animator
}



