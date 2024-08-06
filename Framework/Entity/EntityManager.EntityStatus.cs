/*
* FileName:          EntityManager.EntityStatus
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework.ObjectPool;
using Framework.Resource;
using System;
using System.Collections.Generic;

namespace Framework.Entity
{
    internal sealed partial class EntityManager : FrameworkModule, IEntityManager
    {
        /// <summary>
        /// 实体状态。
        /// </summary>
        private enum EntityStatus : byte
        {
            Unknown = 0,
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled
        }

    }
}
