/*
* FileName:          IResourceManager
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections.Generic;


namespace Framework.Resource
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath
        {
            get;
        }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath
        {
            get;
        }

        /// <summary>
        /// 获取资源模式。
        /// </summary>
        ResourceMode ResourceMode
        {
            get;
        }

        /// <summary>
        /// 获取资源数量。
        /// </summary>
        int AssetCount
        {
            get;
        }

        /// <summary>
        /// 获取资源数量。
        /// </summary>
        int ResourceCount
        {
            get;
        }


        /// <summary>
        /// 资源应用成功事件。
        /// </summary>
        event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;

        /// <summary>
        /// 资源应用失败事件。
        /// </summary>
        event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;

        /// <summary>
        /// 资源更新开始事件。
        /// </summary>
        event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;

        /// <summary>
        /// 资源更新改变事件。
        /// </summary>
        event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;

        /// <summary>
        /// 资源更新成功事件。
        /// </summary>
        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;

        /// <summary>
        /// 资源更新失败事件。
        /// </summary>
        event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;

        /// <summary>
        /// 资源更新全部完成事件。
        /// </summary>
        event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;










        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 设置资源模式。
        /// </summary>
        /// <param name="resourceMode">资源模式。</param>
        void SetResourceMode(ResourceMode resourceMode);


        /// <summary>
        /// 设置资源辅助器。
        /// </summary>
        /// <param name="resourceHelper">资源辅助器。</param>
        void SetResourceHelper(IResourceHelper resourceHelper);

        /// <summary>
        /// 增加加载资源代理辅助器。
        /// </summary>
        /// <param name="loadResourceAgentHelper">要增加的加载资源代理辅助器。</param>
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper);

        /// <summary>
        /// 使用单机模式并初始化资源。
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时的回调函数。</param>
        void InitResources(Action initResourcesCompleteCallback);



        /// <summary>
        /// 停止更新资源。
        /// </summary>
        void StopUpdateResources();






        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="assetName">要检查资源的名称。</param>
        /// <returns>检查资源是否存在的结果。</returns>
        HasAssetResult HasAsset(string assetName);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData);

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData);

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks);

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <returns>二进制资源的实际路径。</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（而非文件系统）中的情况。若二进制资源存储在文件系统中时，返回值将始终为空。</remarks>
        string GetBinaryPath(string binaryAssetName);

        /// <summary>
        /// 获取二进制资源的实际路径。
        /// </summary>
        /// <param name="binaryAssetName">要获取实际路径的二进制资源的名称。</param>
        /// <param name="storageInReadOnly">二进制资源是否存储在只读区中。</param>
        /// <param name="storageInFileSystem">二进制资源是否存储在文件系统中。</param>
        /// <param name="relativePath">二进制资源或存储二进制资源的文件系统，相对于只读区或者读写区的相对路径。</param>
        /// <param name="fileName">若二进制资源存储在文件系统中，则指示二进制资源在文件系统中的名称，否则此参数返回空。</param>
        /// <returns>是否获取二进制资源的实际路径成功。</returns>
        bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName);

        /// <summary>
        /// 获取二进制资源的长度。
        /// </summary>
        /// <param name="binaryAssetName">要获取长度的二进制资源的名称。</param>
        /// <returns>二进制资源的长度。</returns>
        int GetBinaryLength(string binaryAssetName);

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks);

        /// <summary>
        /// 异步加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData);

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <returns>存储加载二进制资源的二进制流。</returns>
        byte[] LoadBinaryFromFileSystem(string binaryAssetName);

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源的二进制流的起始位置。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex);

        /// <summary>
        /// 从文件系统中加载二进制资源。
        /// </summary>
        /// <param name="binaryAssetName">要加载二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源的二进制流的起始位置。</param>
        /// <param name="length">存储加载二进制资源的二进制流的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>存储加载二进制资源片段内容的二进制流。</returns>
        byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>存储加载二进制资源片段内容的二进制流。</returns>
        byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源片段内容的二进制流的起始位置。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段。
        /// </summary>
        /// <param name="binaryAssetName">要加载片段的二进制资源的名称。</param>
        /// <param name="offset">要加载片段的偏移。</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流。</param>
        /// <param name="startIndex">存储加载二进制资源片段内容的二进制流的起始位置。</param>
        /// <param name="length">要加载片段的长度。</param>
        /// <returns>实际加载了多少字节。</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex, int length);
    }
}