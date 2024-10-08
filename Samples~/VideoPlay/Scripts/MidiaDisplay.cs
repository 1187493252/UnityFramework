using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace K_UnityGF
{
    /// <summary>
    /// 媒体显示
    /// </summary>
    public class MidiaDisplay : MonoBehaviour
    {
        [Header("画面画布"), SerializeField]
        public RawImage rawImage;        

        [Header("媒体播放器"), SerializeField]
        public VideoPlayer videoPlayer;    

        private void Update()
        {
            rawImage.texture = videoPlayer.texture;
        }
    }
}
