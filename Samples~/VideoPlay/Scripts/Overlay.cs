using UnityEngine;
using UnityEngine.UI;

namespace K_UnityGF
{
    public class Overlay : MonoBehaviour
    {
		public enum Feedback
		{
			Play,
			Pause,
			VolumeUp,
			VolumeDown,
			VolumeMute,
		}

		[SerializeField] Image _feedbackImage = null;
		[SerializeField] CanvasGroup _feedbackCanvas = null;
		[SerializeField] float _startScale = 0.25f;
		[SerializeField] float _endScale = 1.0f;
		[SerializeField] float _animationSpeed = 1.5f;

		private Material _feedbackMaterial;
		private float _feedbackTimer;

		private readonly string _propMute = "_Mute";
		private readonly string _propVolume = "_Volume";

		public Material FeedbackMaterial { get => _feedbackMaterial; }

		private void Start()
		{
			_feedbackMaterial = new Material(_feedbackImage.material);
			_feedbackImage.material = _feedbackMaterial;
			_feedbackCanvas.alpha = 0f;
			_feedbackTimer = 1f;
		}

		private void OnDestroy()
		{
			if (_feedbackMaterial)
			{
                Destroy(_feedbackMaterial);
				_feedbackMaterial = null;
			}
		}

		private const string KeywordPlay = "UI_PLAY";
		private const string KeywordPause = "UI_PAUSE";
		private const string KeywordVolume = "UI_VOLUME";
		private const string KeywordVolumeUp = "UI_VOLUMEUP";
		private const string KeywordVolumeDown = "UI_VOLUMEDOWN";
		private const string KeywordVolumeMute = "UI_VOLUMEMUTE";

		public void TriggerFeedback(Feedback feedback)
		{
			_feedbackMaterial.DisableKeyword(KeywordPlay);
			_feedbackMaterial.DisableKeyword(KeywordPause);
			_feedbackMaterial.DisableKeyword(KeywordVolume);
			_feedbackMaterial.DisableKeyword(KeywordVolumeUp);
			_feedbackMaterial.DisableKeyword(KeywordVolumeDown);
			_feedbackMaterial.DisableKeyword(KeywordVolumeMute);

			string keyword = null;
			switch (feedback)
			{
				case Feedback.Play:
					keyword = KeywordPlay;
					break;
				case Feedback.Pause:
					keyword = KeywordPause;
					break;
				case Feedback.VolumeUp:
					keyword = KeywordVolume;
					_feedbackMaterial.SetFloat(_propMute, 0f);
					_feedbackMaterial.SetFloat(_propVolume, 1f);
					break;
				case Feedback.VolumeDown:
					keyword = KeywordVolume;
					_feedbackMaterial.SetFloat(_propMute, 0f);
					_feedbackMaterial.SetFloat(_propVolume, 0.5f);
					break;
				case Feedback.VolumeMute:
					keyword = KeywordVolume;
					_feedbackMaterial.SetFloat(_propVolume, 1f);
					_feedbackMaterial.SetFloat(_propMute, 1f);
					break;
			}

			if (!string.IsNullOrEmpty(keyword))
			{
				_feedbackMaterial.EnableKeyword(keyword);
			}

			_feedbackCanvas.alpha = 1f;
			_feedbackCanvas.transform.localScale = new Vector3(_startScale, _startScale, _startScale);
			_feedbackTimer = 0f;
			Update();
		}

		private void Update()
		{
			float t2 = Mathf.Clamp01(_feedbackTimer);
			float t = Mathf.Clamp01((_feedbackTimer - 0.5f) * 2f);
			_feedbackCanvas.alpha = Mathf.Lerp(1f, 0f, PowerEaseOut(t, 1f));
			if (_feedbackCanvas.alpha > 0f)
			{
				float scale = Mathf.Lerp(_startScale, _endScale, PowerEaseOut(t2, 2f));
				_feedbackCanvas.transform.localScale = new Vector3(scale, scale, scale);
			}

			_feedbackTimer += Time.deltaTime * _animationSpeed;
		}

		private static float PowerEaseOut(float t, float power) { return 1f - Mathf.Abs(Mathf.Pow(t - 1f, power)); }
	}
}
