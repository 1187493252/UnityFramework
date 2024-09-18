/*
* FileName:          ResourceManager
* CompanyName:       
* Author:            relly
* Description:       
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Resource
{
    internal sealed partial class ResourceManager : FrameworkModule
    {
        private const int DefaultPriority = 0;

  

   
        private Dictionary<string, UnityEngine.Object> m_CachedAssets = null;
        private Dictionary<string, object> m_CachedDatas = null;




        internal override void Shutdown()
        {

        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
 
        }
    }
}
