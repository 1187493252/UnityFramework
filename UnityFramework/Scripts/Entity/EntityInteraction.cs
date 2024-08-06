/*
* FileName:          EntityInteraction
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
    [RequireComponent(typeof(EntityProxy))]
    public abstract class EntityInteraction : MonoBehaviour
    {
        EntityProxy entityProxy;
        public EntityProxy EntityProxy
        {
            get
            {
                if (entityProxy == null)
                {
                    entityProxy = GetComponent<EntityProxy>();

                }
                return entityProxy;
            }
        }
        protected virtual void Awake()
        {
            entityProxy = GetComponent<EntityProxy>();
        }
    }
}
