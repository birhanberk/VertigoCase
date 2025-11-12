using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Level Settings", menuName = "Data / Level / Settings")]
    public class LevelSettingsSo : ScriptableObject
    {
        [SerializeField] private List<LevelSo> levelList;

        public List<LevelSo> LevelList => levelList;
    }
}
