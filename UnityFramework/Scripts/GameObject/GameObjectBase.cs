/*
* FileName:          GameObjectBase
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Runtime
{
    public class GameObjectBase : MonoBase
    {
        [SerializeField]
        private int id;
        public bool InitShow = true;
        public int Id
        {
            get
            {
                return id;
            }
        }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Start()
        {
            SetVisible(InitShow);
        }

        protected virtual void OnDestroy()
        {
            ComponentEntry.GameObject.Remove(Id);
        }

        public virtual void Init()
        {
            RegisterEntity();
        }

        private void RegisterEntity()
        {
            if (Id != 0)
            {
                if (ComponentEntry.GameObject)
                {
                    ComponentEntry.GameObject.Add(Id, this.gameObject);
                }
                else
                {
                    ComponentEntry.OnInitFinish += delegate
                    {
                        ComponentEntry.GameObject.Add(Id, this.gameObject);
                    };
                }

            }
        }
    }
}
