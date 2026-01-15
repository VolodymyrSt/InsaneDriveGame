using System;
using _Project.Code.GamePlay.Interaction;
using _Project.Code.Util;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Code.GamePlay.UI.ToolTip
{
    public class ToolTipView : MonoBehaviour
    {
        [SerializeField] protected RectTransform _root;
        [SerializeField] protected TextMeshProUGUI _massage;

        private bool _isHidden;
        private Tween _previewTween;

        private void Start()
        {
            _root.localScale = Vector3.zero;
            _root.SetActive(true);
            _isHidden = true;
        }

        public void Preview(InteractableType type)
        {
            _massage.text = type.ToString();
            _root.localScale = Vector3.zero;
            _root.SetActive(true);
            
            _previewTween?.Kill();
            _isHidden = false;
            
            _previewTween = _root.DOScale(Constants.Scaled, Constants.BaseAnimationDuration)
                .SetEase(Ease.Linear)
                .SetDelay(0.2f)
                .Play();
        }

        public void Hide()
        {
            if (_isHidden) return;
            
            _previewTween?.Kill();
            
            _previewTween = _root.DOScale(Constants.Unscaled, Constants.BaseAnimationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _root.SetActive(false);
                    _isHidden = true;
                })
                .Play();
        }
    }
}