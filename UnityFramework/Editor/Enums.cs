












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





