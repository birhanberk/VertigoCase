using System;
using System.Collections.Generic;
using Item;

namespace Inventory
{
    [Serializable]
    public class InventoryManager
    {
        private Dictionary<ItemSo, int> inventory = new ();

        public Dictionary<ItemSo, int> Inventory => inventory;

        public Action<ItemSo, int> onItemChanged;
        public Action onReset;

        public void AddItem(ItemSo itemSo, int amount)
        {
            if (!inventory.TryAdd(itemSo, amount))
            {
                inventory[itemSo] += amount;
            }
            onItemChanged?.Invoke(itemSo, inventory[itemSo]);
        }

        public void RemoveAll()
        {
            inventory.Clear();
            onReset?.Invoke();
        }
    }
}
