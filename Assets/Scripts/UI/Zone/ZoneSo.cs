using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.Item;
using UI.Progress;
using UI.Wheel;
using UnityEngine;

namespace UI.Zone
{
    [CreateAssetMenu(fileName = "Zone Data", menuName = "Data / Zone / Zone")]
    public class ZoneSo : ScriptableObject
    {
        [SerializeField] private int perLevel;
        [SerializeField] private WheelSo wheelSo;
        [SerializeField] private ProgressSo progressSo;
        [SerializeField] private bool useBomb;
        [SerializeField] private bool canLeave;
        [SerializeField, InlineEditor] private List<ItemSo> items;

        public int PerLevel => perLevel;
        public WheelSo WheelSo => wheelSo;
        public ProgressSo ProgressSo => progressSo;
        public bool UseBomb => useBomb;
        public bool CanLeave => canLeave;
        public List<ItemSo> Items => items;
    }
}
