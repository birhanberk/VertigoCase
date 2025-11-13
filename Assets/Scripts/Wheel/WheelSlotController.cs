using System;
using System.Collections.Generic;
using System.Linq;
using Item;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wheel
{
    [Serializable]
    public class WheelSlotController
    {
        [SerializeField] private ItemSo bombItem;

        public void InitializeSlots(LevelSo levelSo, List<ItemSlot> slots)
        {
            var bombIndex = GetBombIndex(levelSo);

            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                if (i == bombIndex)
                {
                    slot.UpdateItem(bombItem);
                    continue;
                }

                var item = levelSo.Items[i];

                if (!item)
                    item = GetRandomItem(levelSo);

                if (item == bombItem)
                    slot.UpdateItem(bombItem);
                else
                    slot.UpdateItem(item, 100);
            }
        }

        private int GetBombIndex(LevelSo levelSo)
        {
            if (levelSo.Items.Any(x => x == bombItem)) return -1;
            if (!levelSo.ZoneSo.UseBomb) return -1;

            var empty = levelSo.Items.Select((x, i) => new { x, i })
                .Where(x => !x.x)
                .Select(x => x.i)
                .ToList();

            return empty[Random.Range(0, empty.Count)];
        }

        private ItemSo GetRandomItem(LevelSo levelSo)
        {
            var items = levelSo.ZoneSo.Items;
            return items[Random.Range(0, items.Count)];
        }
    }
}