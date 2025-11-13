using System;
using System.Collections.Generic;
using Item;
using UnityEngine;

namespace Wheel
{
    [Serializable]
    public class WheelResultResolver
    {
        public int Resolve(RectTransform wheel, RectTransform indicator, List<ItemSlot> slots)
        {
            var center = wheel.position;
            var indicatorDir = (center - indicator.position).normalized;

            var best = float.NegativeInfinity;
            var bestIndex = 0;

            for (var i = 0; i < slots.Count; i++)
            {
                var slotDir = (center - slots[i].transform.position).normalized;
                var dot = Vector2.Dot(indicatorDir, slotDir);

                if (dot > best)
                {
                    best = dot;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }
    }
}