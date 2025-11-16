using Sirenix.OdinInspector;
using TMPro;
using UI.Format;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField, Required] private RectTransform itemTransform;
        [SerializeField, Required] protected Image itemImage;
        [SerializeField, Optional] protected TMP_Text amountText;
        [SerializeField, ShowIf("@amountText != null")] protected NumberFormatType formatType;
        
        private int _currentAmount;

        public RectTransform ItemTransform => itemTransform;
        public int CurrentAmount => _currentAmount;

        public void UpdateItem(Sprite sprite)
        {
            itemImage.sprite = sprite;
            if (amountText)
            {
                amountText.gameObject.SetActive(false);
            }
        }
        
        public void UpdateItem(Sprite sprite, int targetAmount)
        {
            itemImage.sprite = sprite;
            if (amountText)
            {
                amountText.gameObject.SetActive(true);
            }
            UpdateAmount(targetAmount);
        }
        
        protected void UpdateAmount(int amount)
        {
            _currentAmount = amount;
            if (amountText)
            {
                amountText.text = NumberFormatter.Format(_currentAmount, formatType);
            }
        }
    }
}
