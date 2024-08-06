/*
* FileName:          UnityInitialzeOnLoad.AutoRefresh
* CompanyName:       
* Author:            relly
* Description:       Unity在进入播放模式时刷新(自动刷新关闭)
* 
*/

using UnityEditor;
namespace UnityFramework.Editor
{
    public partial class UnityInitialzeOnLoad
    {

        [InitializeOnLoad]
        public class EditorNotification : AssetPostprocessor
        {
            private static bool isPlaymode;
            private static bool isFocused;
            static EditorNotification()
            {
                //  EditorApplication.update -= Update;
                // EditorApplication.update += Update;
                EditorApplication.playModeStateChanged -= PlaymodeStateChanged;
                EditorApplication.playModeStateChanged += PlaymodeStateChanged;
            }

            private static void Update()
            {
                if (isFocused == UnityEditorInternal.InternalEditorUtility.isApplicationActive)
                {
                    return;
                }
                isFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnEditorFocus(isFocused);


            }

            private static void OnEditorFocus(bool focus)
            {
                if (focus)
                {
                    if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        Refresh();
                    }
                }
            }

            private static void PlaymodeStateChanged(PlayModeStateChange playModeStateChange)
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode && !isPlaymode)
                {
                    isPlaymode = true;
                    Refresh();
                }
                else if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    isPlaymode = false;
                }
            }

            private static void Refresh()
            {

                AssetDatabase.Refresh();
                EditorUtility.RequestScriptReload();

            }
        }
    }
}

