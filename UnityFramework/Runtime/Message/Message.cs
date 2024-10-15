/*
* FileName:          Message
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityFramework
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
    public delegate void Callback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);
    public delegate void Callback<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, X arg5);
    public class Message
    {
        static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

        #region AddListener
        static public void AddListener(string eventType, Callback handler)
        {
            RegisterListener(eventType, handler);
        }

        //Single parameter
        static public void AddListener<T>(string eventType, Callback<T> handler)
        {
            RegisterListener(eventType, handler);
        }

        //Two parameters
        static public void AddListener<T, U>(string eventType, Callback<T, U> handler)
        {
            RegisterListener(eventType, handler);
        }

        //Three parameters
        static public void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler)
        {
            RegisterListener(eventType, handler);
        }

        //4 parameters
        static public void AddListener<T, U, V, W>(string eventType, Callback<T, U, V, W> handler)
        {
            RegisterListener(eventType, handler);
        }

        //5 parameters
        static public void AddListener<T, U, V, W, X>(string eventType, Callback<T, U, V, W, X> handler)
        {
            RegisterListener(eventType, handler);
        }
        #endregion

        static void RegisterListener(string eventType, Delegate handler)
        {
            if (handler == null || string.IsNullOrEmpty(eventType))
            {
                Debug.LogError($"{eventType}ע��ʧ��");
                return;
            }
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = handler;
            }
        }
        static void UnregisterListener(string eventType)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable.Remove(eventType);
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }

        static public void RemoveListener(string eventType)
        {
            UnregisterListener(eventType);
        }


        #region Broadcast
        //No parameters
        static public void Broadcast(string eventType)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback callback = d as Callback;

                if (callback != null)
                {
                    callback();
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }

        //Single parameter
        static public void Broadcast<T>(string eventType, T arg1)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback<T> callback = d as Callback<T>;

                if (callback != null)
                {
                    callback(arg1);
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }

        //Two parameters
        static public void Broadcast<T, U>(string eventType, T arg1, U arg2)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback<T, U> callback = d as Callback<T, U>;
                if (callback != null)
                {
                    callback(arg1, arg2);
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }

        //Three parameters
        static public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback<T, U, V> callback = d as Callback<T, U, V>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }


        //4 parameters
        static public void Broadcast<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback<T, U, V, W> callback = d as Callback<T, U, V, W>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4);
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }



        //5 parameters
        static public void Broadcast<T, U, V, W, X>(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5)
        {
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Callback<T, U, V, W, X> callback = d as Callback<T, U, V, W, X>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4, arg5);
                }
            }
            else
            {
                Debug.LogError($"{eventType}未注册");
            }
        }


        #endregion

    }
}



