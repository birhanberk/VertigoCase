using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Fail
{
    [Serializable]
    public class VignetteController
    {
        [SerializeField, Required] private Image vignetteImage;
        
        [SerializeField] private float maxAlpha = 0.6f;
        [SerializeField] private float lowAlpha = 0.2f;
        [SerializeField] private float pulseInDuration = 0.3f;
        [SerializeField] private float pulseOutDuration = 0.5f;
        
        [SerializeField] private float fadeOutDuration = 0.3f;
        
        private Tween _vignetteTween;

        [Button]
        public void PlayVignette()
        {
            _vignetteTween?.Kill();

            var c = vignetteImage.color;
            c.a = lowAlpha;
            vignetteImage.color = c;

            _vignetteTween = DOTween.Sequence()
                .Append(vignetteImage.DOFade(maxAlpha, pulseInDuration).SetEase(Ease.OutQuad))
                .Append(vignetteImage.DOFade(lowAlpha, pulseOutDuration).SetEase(Ease.InQuad))
                .SetLoops(-1, LoopType.Restart);
        }
        
        [Button]
        public void StopVignette()
        {
            _vignetteTween?.Kill();

            _vignetteTween = vignetteImage
                .DOFade(0f, fadeOutDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _vignetteTween = null;
                });
        }
    }
}
