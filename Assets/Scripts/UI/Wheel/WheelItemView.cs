using Sirenix.OdinInspector;
using UI.Item;
using UnityEngine;

namespace UI.Wheel
{
    public class WheelItemView : ItemView
    {
        [SerializeField] private Transform centerTransform;
        [SerializeField, ReadOnly] private ItemSo itemSo;

        public ItemSo ItemSo => itemSo;

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
            UpdateItem(itemSo.ItemImage);
        }
        
        public void UpdateItem(ItemSo newItemSo, int newAmount)
        {
            itemSo = newItemSo;
            UpdateItem(itemSo.ItemImage, newAmount);
        }
    }
}