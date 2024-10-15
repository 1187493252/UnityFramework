/*
* FileName:          KeyCodePressEventArgs
* CompanyName:       杭州中锐
* Author:            relly
* Description:       
* 
*/

using Framework;
using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodePressEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(KeyCodePressEventArgs).GetHashCode();

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    public object UserData
    {
        get;
        private set;
    }
    public KeyCode KeyCode
    {
        get;
        private set;
    }
    public KeyCodePressEventArgs()
    {
        KeyCode = KeyCode.None;
        UserData = null;
    }
    public static KeyCodePressEventArgs Create(KeyCode keyCode, object userData = null)
    {
        // 使用引用池技术，避免频繁内存分配
        KeyCodePressEventArgs keyCodePressEventArgs = ReferencePool.Acquire<KeyCodePressEventArgs>();
        keyCodePressEventArgs.KeyCode = keyCode;
        keyCodePressEventArgs.UserData = userData;
        return keyCodePressEventArgs;
    }
    public override void Clear()
    {
        KeyCode = KeyCode.None;
        UserData = null;
    }
}



