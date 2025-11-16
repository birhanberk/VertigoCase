using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UI.Item;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Reward
{
    [Serializable]
    public class RewardCardController
    {
        [SerializeField, Required] private RectTransform cardTransform;
        [SerializeField, Required] private Image cardImage;
        [SerializeField, Required] private TMP_Text headerText;
        [SerializeField, Required] private Image itemImage;
        [SerializeField, Required] private TMP_Text amountText;
        [SerializeField, Required] private Image shineImage;
        
        [SerializeField] private float scaleDuration = 1f;
        [SerializeField] private float effectRotateDuration = 5f;
        [SerializeField] private float afterScaleDelay = 0.5f;
        
        private Tween _shineTween;
        private Tween _scaleTween;
        
        public async Task StartSequence(ItemSo itemSo, int amount)
        {
            UpdateItem(itemSo, amount);
            KillTweens();
            ResetState();

            PlayShineRotation();
            await PlayScaleThenDelayAsync();
        }

        public void StartCloseSequence()
        {
            KillTweens();

            _scaleTween = cardTransform
                .DOScale(0f, 0.35f)
                .SetEase(Ease.InBack);
        }

        public void Close()
        {
            cardTransform.localScale = Vector3.zero;
        }
        
        public void UpdateItem(ItemSo itemSo, int amount)
        {
            headerText.text = itemSo.ItemHeader;
            headerText.color = itemSo.RewardSo.HeaderColor;
            cardImage.color = itemSo.RewardSo.CardColor;
            itemImage.sprite = itemSo.ItemImage;
            amountText.text = "x" + amount;
            shineImage.color = itemSo.RewardSo.EffectColor;
        }
        
        private void KillTweens()
        {
            _shineTween?.Kill();
            _scaleTween?.Kill();
        }

        private void ResetState()
        {
            cardTransform.anchoredPosition = Vector2.zero;
            cardTransform.localScale = Vector3.zero;
            shineImage.rectTransform.localEulerAngles = Vector3.zero;
        }

        private async Task PlayScaleThenDelayAsync()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(cardTransform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack));
            sequence.AppendInterval(afterScaleDelay);

            await sequence.AsyncWaitForCompletion();
        }

        private void PlayShineRotation()
        {
            _shineTween = shineImage.rectTransform
                .DOLocalRotate(new Vector3(0, 0, 360f), effectRotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}
