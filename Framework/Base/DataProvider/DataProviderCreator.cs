/*
* FileName:          DataProviderCreator
* CompanyName:       
* Author:            relly
* Description:       
*/


using Framework.Resource;

namespace Framework
{
    /// <summary>
    /// 数据提供者创建器。
    /// </summary>
    public static class DataProviderCreator
    {
        /// <summary>
        /// 获取缓冲二进制流的大小。
        /// </summary>
        /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
        /// <returns>缓冲二进制流的大小。</returns>
        public static int GetCachedBytesSize<T>()
        {
            return DataProvider<T>.CachedBytesSize;
        }

        /// <summary>
        /// 确保二进制流缓存分配足够大小的内存并缓存。
        /// </summary>
        /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
        /// <param name="ensureSize">要确保二进制流缓存分配内存的大小。</param>
        public static void EnsureCachedBytesSize<T>(int ensureSize)
        {
            DataProvider<T>.EnsureCachedBytesSize(ensureSize);
        }

        /// <summary>
        /// 释放缓存的二进制流。
        /// </summary>
        /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
        public static void FreeCachedBytes<T>()
        {
            DataProvider<T>.FreeCachedBytes();
        }

        /// <summary>
        /// 创建数据提供者。
        /// </summary>
        /// <typeparam name="T">数据提供者的持有者的类型。</typeparam>
        /// <param name="owner">数据提供者的持有者。</param>
        /// <param name="resourceManager">资源管理器。</param>
        /// <param name="dataProviderHelper">数据提供者辅助器。</param>
        /// <returns>创建的数据提供者。</returns>
        public static IDataProvider<T> Create<T>(T owner, IResourceManager resourceManager, IDataProviderHelper<T> dataProviderHelper)
        {
            if (owner == null)
            {
                throw new FrameworkException("Owner is invalid.");
            }

            if (resourceManager == null)
            {
                throw new FrameworkException("Resource manager is invalid.");
            }

            if (dataProviderHelper == null)
            {
                throw new FrameworkException("Data provider helper is invalid.");
            }

            DataProvider<T> dataProvider = new DataProvider<T>(owner);
            dataProvider.SetResourceManager(resourceManager);
            dataProvider.SetDataProviderHelper(dataProviderHelper);
            return dataProvider;
        }
    }
}
