using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Item
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField, Required] private Image image;
        [SerializeField, Required] private TMP_Text text;
        [SerializeField] private Transform centerTransform;
        [SerializeField, ReadOnly] private ItemSo itemSo;
        [SerializeField, ReadOnly] private int amount;

        public ItemSo ItemSo => itemSo;
        public int Amount => amount;

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (!centerTransform) return;

            var dir = centerTransform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }

        public void UpdateItem(ItemSo newItemSo)
        {
            itemSo = newItemSo;
            image.sprite = itemSo.ItemImage;
            text.gameObject.SetActive(false);
        }
        
        public void UpdateItem(ItemSo newItemSo, int newAmount)
        {
            amount = newAmount;
            itemSo = newItemSo;
            image.sprite = itemSo.ItemImage;
            text.text = amount.ToString();
            text.gameObject.SetActive(true);
        }
    }
}