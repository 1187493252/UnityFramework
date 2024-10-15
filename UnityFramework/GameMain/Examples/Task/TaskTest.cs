/*
* FileName:          TaskTest
* CompanyName:       
* Author:            
* Description:       
*/
#if VIU_STEAMVR_2_0_0_OR_NEWER

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework.Runtime;
using UnityFramework.Runtime.Task;

public class TaskTest : MonoBehaviour
{
    public TaskConfig taskConfig;
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TaskManager.Instance.CurrentTask.AchieveGoal(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TaskManager.Instance.SkipTask(TaskManager.Instance.GetTaskConfsByName("2")); ;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TaskManager.Instance.SkipTask(TaskManager.Instance.GetTaskConfsByName("0")); ;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ComponentEntry.Task.StartTask(0);

        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("0"));

        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("1"));

        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("2"));

        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("3"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("1_1"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            ComponentEntry.Task.SkipToTask(ComponentEntry.Task.GetTaskByName("1_2"));
        }
    }
}
#endif