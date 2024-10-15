// 此脚本为自动生成,请勿修改

using System.Collections.Generic;

public class QuestionItem
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;
    /// <summary>
    /// 题目
    /// </summary>
    public string title;
    /// <summary>
    /// 选项
    /// </summary>
    public List<string> option;

    /// <summary>
    /// 答案
    /// </summary>
    public List<int> answer;
    /// <summary>
    /// 正确答案提示
    /// </summary>
    public string rightAnswer;

    /// <summary>
    /// 提交的选项
    /// </summary>
    public List<int> submitOptions;

    /// <summary>
    /// 题目分数
    /// </summary>
    public int questionScore;

    /// <summary>
    /// 阶梯式分数
    /// </summary>
    public bool isStagewise;
    /// <summary>
    /// 实际得分
    /// </summary>
    public int score;
    /// <summary>
    /// 
    /// </summary>
    public bool errorZero;
    /// <summary>
    /// 是否多选
    /// </summary>
    public bool multiple;
    /// <summary>
    /// 题目解析
    /// </summary>
    public string explain;
    /// <summary>
    /// 各选项的分
    /// </summary>
    public List<int> optionsScore;
}
