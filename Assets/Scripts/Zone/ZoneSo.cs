using Data;
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

        public int PerLevel => perLevel;
        public WheelSo WheelSo => wheelSo;
        public ProgressSo ProgressSo => progressSo;
        public bool UseBomb => useBomb;
    }
}
