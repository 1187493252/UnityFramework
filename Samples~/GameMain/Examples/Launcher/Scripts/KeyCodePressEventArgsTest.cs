/*
* FileName:          KeyCodePressEventArgsTest
* CompanyName:       杭州中锐
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFramework;
using UnityFramework.Runtime;

public class KeyCodePressEventArgsTest : MonoBehaviour
{
    private void Start()
    {
        ComponentEntry.Coroutine.DelayExecute(1, () => UnityFramework.Runtime.ComponentEntry.Event.Subscribe(KeyCodePressEventArgs.EventId, KeyCodePress));


    }
    public void Test1(List<int> list)
    {
        for (int i = 0; i < 10; i++)
        {
            list.Add(i);
        }
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    UnityFramework.Runtime.ComponentEntry.Event.Fire(this, KeyCodePressEventArgs.Create(keyCode, ""));

                }
            }
        }
    }
    private void OnApplicationQuit()
    {
        // UnityFramework.Framework.Event.Unsubscribe(KeyCodePressEventArgs.EventId, KeyCodePress);

    }
    void KeyCodePress(object sender, Framework.BaseEventArgs e)
    {
        KeyCodePressEventArgs keyCodePressEventArgs = (KeyCodePressEventArgs)e;
        Log.Info($"{keyCodePressEventArgs.KeyCode}被按下");
        if (KeyCode.Escape == keyCodePressEventArgs.KeyCode)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();

#endif
        }
        else
        {
            //  UnityFramework.Runtime.Framework.Scene.SyncLoadScene("Demo1");
        }
    }
}



