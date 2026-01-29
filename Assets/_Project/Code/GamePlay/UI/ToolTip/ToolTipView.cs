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
        [Header("Base")]
        [SerializeField] private RectTransform _root;

        [Header("ToolTip")]
        [SerializeField] private RectTransform _massageBackground;
        [SerializeField] private TextMeshProUGUI _massage;
        
        [SerializeField] private float _horizontalPadding = 24f;
        [SerializeField] private float _verticalPadding = 12f;

        private bool _isHidden;
        private Tween _previewTween;

        private void Start()
        {
            _root.localScale = Vector3.zero;
            _root.SetActive(true);
            _isHidden = true;
        }

        public void ShowFor(InteractableInfo info)
        {
            _massage.text = info.Name;

            var textWidth = _massage.GetPreferredValues(info.Name).x + _horizontalPadding;
            var textHeight = _massage.GetPreferredValues(info.Name, textWidth, 0).y + _verticalPadding;
            
            _massageBackground.sizeDelta = new Vector2(textWidth, textHeight);

            _root.localScale = Vector3.zero;
            _root.SetActive(true);
            
            _previewTween?.Kill();
            _isHidden = false;
            
            _previewTween = _root.DOScale(Constants.Scaled, Constants.BaseAnimationDuration)
                .SetEase(Ease.Linear)
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