/*
* FileName:          GameObjectComponent
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{
    [DisallowMultipleComponent]
    public class GameObjectComponent : UnityFrameworkComponent
    {
        Dictionary<int, GameObject> gameObjectDic = new Dictionary<int, GameObject>();
        protected override void Awake()
        {
            base.Awake();
        }
        public void Init()
        {
            ClearAll();
        }

        public void Add(int id, GameObject gameObject)
        {
            gameObjectDic.AddOrReplace(id, gameObject);
        }

        public void Remove(int id)
        {
            if (gameObjectDic.ContainsKey(id))
            {
                gameObjectDic.Remove(id);
            }
        }

        public void Remove(GameObject gameObject)
        {
            foreach (var item in gameObjectDic)
            {
                if (item.Value == gameObject)
                {
                    gameObjectDic.Remove(item.Key);
                    break;
                }
            }
        }

        public void ClearAll()
        {
            gameObjectDic.Clear();
        }

        public GameObject GetGameObject(int id)
        {
            return gameObjectDic.GetValue(id);
        }

        public T GetGameObject<T>(int id) where T : Component
        {
            GameObject gameObject = GetGameObject(id);
            if (gameObject)
            {
                T t = gameObject.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
                else
                {
                    Log.Error($"GameObject[{id}]不存在组件|{t.name}");
                    return null;
                }
            }
            else
            {
                Log.Error($"GameObject不存在|id:{id}");
                return null;
            }
        }


        public void ShowGameObject(params int[] id)
        {
            foreach (var item in id)
            {
                if (gameObjectDic.ContainsKey(item))
                {
                    gameObjectDic[item].Show();
                }
            }
        }

        public void HideGameObject(params int[] id)
        {
            foreach (var item in id)
            {
                if (gameObjectDic.ContainsKey(item))
                {
                    gameObjectDic[item].Hide();
                }
            }
        }

    }
}
