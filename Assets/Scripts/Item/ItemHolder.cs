using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Item
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField, Required] private Image image;
        [SerializeField, Required] private TMP_Text text;
        [SerializeField] private float setDuration = 0.4f;

        public Image Image => image;

        private Tween _countTween;
        private int _currentAmount;

        public async Task UpdateItem(int targetAmount)
        {
            _countTween?.Kill();

            float value = _currentAmount;

            _countTween = DOTween.To(() => value, x =>
                {
                    value = x;
                    _currentAmount = Mathf.FloorToInt(value);
                    text.text = _currentAmount.ToString();
                }, targetAmount, setDuration).SetEase(Ease.OutQuad);

            await _countTween.AsyncWaitForCompletion();

            _currentAmount = targetAmount;
            text.text = _currentAmount.ToString();
        }

        public void UpdateItem(Sprite sprite, int targetAmount)
        {
            image.sprite = sprite;
            text.text = targetAmount.ToString();
            _currentAmount = targetAmount;
        }
    }
}
