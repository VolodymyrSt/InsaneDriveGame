using _Project.Code.Util;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Code.Core.Services.SceneLoadService
{
    public class LoadingCurtain : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup _curtain;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _progressText;

        public void Init()
        {
            _progressSlider.maxValue = 100f;
            _progressSlider.value = 0;
            _progressText.text = string.Empty;
            _curtain.alpha = 0f;
        }

        public void Appear()
        {
            gameObject.SetActive(true);
            _curtain.DOFade(1f, Constants.LoadingCurtainDuration)
                .SetEase(Ease.Linear)
                .Play();
        }

        public void Fade()
        {
            _curtain.DOFade(1f, Constants.LoadingCurtainDuration)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void UpdateProgress(float progress)
        {
            _progressSlider.value = progress;
            _progressText.text = $"{progress:0}%";
        }
    }
}
