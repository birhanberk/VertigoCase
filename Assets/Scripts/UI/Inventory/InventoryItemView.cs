using System.Threading.Tasks;
using DG.Tweening;
using UI.Item;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryItemView : ItemView
    {
        [SerializeField] private float setDuration = 0.4f;

        private Tween _countTween;

        public async Task UpdateAmountIncrease(int targetAmount)
        {
            _countTween?.Kill();

            float value = CurrentAmount;
            _countTween = DOTween.To(() => value, x =>
                {
                    value = x;
                    UpdateAmount(Mathf.FloorToInt(value));
                }, targetAmount, setDuration).SetEase(Ease.OutQuad);

            await _countTween.AsyncWaitForCompletion();

            UpdateAmount(targetAmount);
        }
    }
}
