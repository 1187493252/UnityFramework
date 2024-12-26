/*
* FileName:          DataManager
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections.Generic;
using Framework.Resource;

namespace Framework.Data
{
    /// <summary>
    /// 数据管理器。
    /// </summary>
    internal sealed partial class DataManager : FrameworkModule, IDataManager
    {
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal override void Shutdown()
        {
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}
