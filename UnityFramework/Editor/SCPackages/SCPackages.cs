#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;

namespace UnityFramework.Editor.Package
{
    public partial class UnityEditorTools
    {
        [MenuItem("Assets/UnityFramework/Add SCPackages ", false, 0)]
        private static void AddHybridCLRPackage()
        {
            var addRequest = Client.Add($"https://github.com/1187493252/SCPackages.git");
            if (addRequest.Status == StatusCode.Failure)
            {
                Client.Add($"https://gitee.com/NanGongMing/SCPackages.git");
            }
        }

    }
}
#endif