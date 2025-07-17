# UnityFramework
# 导入方法:
1.在Unity的PackageManager里通过url下载,需要导入示例Samples/GameMain,里面包含必要的插件及脚本

2.直接下载源码,根据报错导入对应的插件，例如LitJson，YooAsset等
# 说明:
本项目以GameFramework为核心,再结合本人项目经历(VR/AR)去掉/修改不适用的模块,目前有些模块功能还没实现或者临时写的单例没有遵循框架规则,后续慢慢完善

1.Resource,GF是编辑器模式直接从本地读,打包后从AB读,本框架直接改成从Resource文件夹加载,有AB需求的本框架也拓展了YooAssetHelper(在Samples~/YooAssetDemo里),也可以自行拓展Helper

2.Data,数据表目前单独从StreamingAssets加载并缓存(代码在DataHelper),不是从Resource模块加载,后续会按框架规则完善,Resource模块使用YooAssetHelper的加载完需要自己缓存数据表

3.Config,通过ConfigHelper的配置从Data内读取缓存,后续会按框架规则完善

4.Audio,通过AudioHelper一次性从Resource/StreamingAssets文件夹加载完,后续会按框架规则完善改成按需加载

5.Entity同GF,同时缓存动态与静态Entity,通过ShowSceneEntity获取

6.UI同GF,同时缓存动态与静态UIForm,通过GetSceneUIForm获取

7.Debugger同GF

8.Event同GF

9.ObjectPool同GF

10.ReferencePool同GF

11.Setting同GF

12.Task线性任务系统

13.Timer计时器

14.WebRequest用于与后台接口交互http

