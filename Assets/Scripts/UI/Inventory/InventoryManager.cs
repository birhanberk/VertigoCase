using System;
using System.Collections.Generic;
using UI.Item;

namespace UI.Inventory
{
    [Serializable]
    public class InventoryManager
    {
        private Dictionary<ItemSo, int> _inventory = new ();
        
        public Dictionary<ItemSo, int> Inventory => _inventory;

        public Action<ItemSo, int> onItemChanged;
        public Action onReset;

        public void AddItem(ItemSo itemSo, int amount)
        {
            if (!_inventory.TryAdd(itemSo, amount))
            {
                _inventory[itemSo] += amount;
            }
            onItemChanged?.Invoke(itemSo, _inventory[itemSo]);
        }

        public void RemoveAll()
        {
            _inventory.Clear();
            onReset?.Invoke();
        }
    }
}
