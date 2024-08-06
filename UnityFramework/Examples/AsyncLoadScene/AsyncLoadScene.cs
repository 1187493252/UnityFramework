using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityFramework
{
    public class AsyncLoadScene : MonoBehaviour
    {
        public Text loadingText;
        public Image progressBar;
        private int curProgressValue = 0;
        private AsyncOperation operation;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(AsyncLoading());
        }

        IEnumerator AsyncLoading()
        {
            string loadType = PlayerPrefs.GetString("SceneSyncLoadType");
            switch (loadType)
            {
                case "Single":
                    operation = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NextSceneName"));
                    break;
                case "Additive":
                    operation = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NextSceneName"), LoadSceneMode.Additive);
                    break;
            }


            //阻止当加载完成自动切换
            operation.allowSceneActivation = false;

            yield return operation;
        }

        // Update is called once per frame
        void Update()
        {

            int progressValue = 100;

            if (curProgressValue < progressValue)
            {
                curProgressValue++;
            }

            loadingText.text = "加载中..." + curProgressValue + "%";//实时更新进度百分比的文本显示  

            progressBar.fillAmount = curProgressValue / 100f;//实时更新滑动进度图片的fillAmount值  

            if (curProgressValue == 100)
            {

                //  loadingText.text = "OK";//文本显示完成OK  

                operation.allowSceneActivation = true;//启用自动加载场景

            }
        }
    }
}