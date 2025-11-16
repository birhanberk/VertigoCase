using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Level List", menuName = "Data / Level / List")]
    public class LevelListSo : ScriptableObject
    {
        [SerializeField] private List<LevelSo> levelList;

        public List<LevelSo> LevelList => levelList;

        public int GetLevelIndex(LevelSo levelSo)
        {
            return levelList.IndexOf(levelSo);
        }
    }
}
