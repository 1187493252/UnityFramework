/*
* FileName:          LoadDataCallbacks
* CompanyName:  
* Author:            relly
* Description:       
* 
*/



namespace Framework.Resource
{
    /// <summary>
    /// 加载数据成功回调函数。
    /// </summary>
    /// <param name="dataName">要加载的数据名称。</param>
    /// <param name="data">已加载的数据。</param>
    /// <param name="duration">加载持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadDataSuccessCallback(string dataName, object data, float duration, object userData);

    /// <summary>
    /// 加载数据失败回调函数。
    /// </summary>
    /// <param name="dataName">要加载的数据名称。</param>
    /// <param name="status">加载数据状态。</param>
    /// <param name="errorMessage">错误信息。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadDataFailureCallback(string dataName, LoadResourceStatus status, string errorMessage, object userData);

    /// <summary>
    /// 加载数据更新回调函数。
    /// </summary>
    /// <param name="dataName">要加载的数据名称。</param>
    /// <param name="progress">加载数据进度。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadDataUpdateCallback(string dataName, float progress, object userData);



    /// <summary>
    /// 加载数据回调函数集。
    /// </summary>
    public sealed class LoadDataCallbacks
    {
        private readonly LoadDataSuccessCallback m_LoadDataSuccessCallback;
        private readonly LoadDataFailureCallback m_LoadDataFailureCallback;
        private readonly LoadDataUpdateCallback m_LoadDataUpdateCallback;


        /// <summary>
        /// 初始化加载数据回调函数集的新实例。
        /// </summary>
        /// <param name="loadDataSuccessCallback">加载数据成功回调函数。</param>
        public LoadDataCallbacks(LoadDataSuccessCallback loadDataSuccessCallback)
            : this(loadDataSuccessCallback, null, null)
        {
        }

        /// <summary>
        /// 初始化加载数据回调函数集的新实例。
        /// </summary>
        /// <param name="loadDataSuccessCallback">加载数据成功回调函数。</param>
        /// <param name="loadDataFailureCallback">加载数据失败回调函数。</param>
        public LoadDataCallbacks(LoadDataSuccessCallback loadDataSuccessCallback, LoadDataFailureCallback loadDataFailureCallback)
            : this(loadDataSuccessCallback, loadDataFailureCallback, null)
        {
        }

        /// <summary>
        /// 初始化加载数据回调函数集的新实例。
        /// </summary>
        /// <param name="loadDataSuccessCallback">加载数据成功回调函数。</param>
        /// <param name="loadDataUpdateCallback">加载数据更新回调函数。</param>
        public LoadDataCallbacks(LoadDataSuccessCallback loadDataSuccessCallback, LoadDataUpdateCallback loadDataUpdateCallback)
            : this(loadDataSuccessCallback, null, loadDataUpdateCallback)
        {
        }





        /// <summary>
        /// 初始化加载数据回调函数集的新实例。
        /// </summary>
        /// <param name="loadDataSuccessCallback">加载数据成功回调函数。</param>
        /// <param name="loadDataFailureCallback">加载数据失败回调函数。</param>
        /// <param name="loadDataUpdateCallback">加载数据更新回调函数。</param>
        /// <param name="loadDataDependencyDataCallback">加载数据时加载依赖数据回调函数。</param>
        public LoadDataCallbacks(LoadDataSuccessCallback loadDataSuccessCallback, LoadDataFailureCallback loadDataFailureCallback, LoadDataUpdateCallback loadDataUpdateCallback)
        {
            if (loadDataSuccessCallback == null)
            {
                throw new FrameworkException("Load Data success callback is invalid.");
            }

            m_LoadDataSuccessCallback = loadDataSuccessCallback;
            m_LoadDataFailureCallback = loadDataFailureCallback;
            m_LoadDataUpdateCallback = loadDataUpdateCallback;

        }

        /// <summary>
        /// 获取加载数据成功回调函数。
        /// </summary>
        public LoadDataSuccessCallback LoadDataSuccessCallback
        {
            get
            {
                return m_LoadDataSuccessCallback;
            }
        }

        /// <summary>
        /// 获取加载数据失败回调函数。
        /// </summary>
        public LoadDataFailureCallback LoadDataFailureCallback
        {
            get
            {
                return m_LoadDataFailureCallback;
            }
        }

        /// <summary>
        /// 获取加载数据更新回调函数。
        /// </summary>
        public LoadDataUpdateCallback LoadDataUpdateCallback
        {
            get
            {
                return m_LoadDataUpdateCallback;
            }
        }


    }
}