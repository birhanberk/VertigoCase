using System.Collections.Generic;
using Item;
using Progress;
using UnityEngine;
using Wheel;

namespace Zone
{
    [CreateAssetMenu(fileName = "Zone Data", menuName = "Data / Zone")]
    public class ZoneSo : ScriptableObject
    {
        [SerializeField] private int perLevel;
        [SerializeField] private WheelSo wheelSo;
        [SerializeField] private ProgressSo progressSo;
        [SerializeField] private bool useBomb;
        [SerializeField] private bool canLeave;
        [SerializeField] private List<ItemSo> items;

        public int PerLevel => perLevel;
        public WheelSo WheelSo => wheelSo;
        public ProgressSo ProgressSo => progressSo;
        public bool UseBomb => useBomb;
        public bool CanLeave => canLeave;
        public List<ItemSo> Items { get => items; set => items = value; }
    }
}
