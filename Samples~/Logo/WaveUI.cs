using DG.Tweening;
using UnityEngine;

namespace K_UnityGF.UIForm
{
    /// <summary>
    /// UI-飘动
    /// </summary>
    public class WaveUI : MonoBehaviour
    {
        private RectTransform dragTarget;               //被拖拽的物体
        private RectTransform dragRangeRect;            //拖拽限制范围Rect
        private Vector2 min;                            //屏幕最小坐标
        private Vector2 max;                            //屏幕最大坐标

        public bool isWave;

        [Header("飘动时间间隔")]
        public float waveInterval = 10;

        private void Awake()
        {
            dragTarget = GetComponent<RectTransform>();
            dragRangeRect = transform.parent.GetComponent<RectTransform>();
            min = dragRangeRect.rect.min - dragTarget.rect.min;
            max = dragRangeRect.rect.max - dragTarget.rect.max;
        }

        private void OnEnable()
        {
            transform.DOKill();
            if (isWave)
            {
                StartWave();
            }
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        /// <summary>
        /// 开始飘动
        /// </summary>
        private void StartWave()
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            transform.DOLocalMove(new Vector2(x, y), waveInterval).SetEase(DG.Tweening.Ease.Linear).OnComplete(() => StartWave());
        }
    }
}
