using System.Collections.Generic;
using Data;
using UnityEngine;
using Zone;

namespace Level
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Level / Level")]
    public class LevelSo : ScriptableObject
    {
        [SerializeField] private ZoneSo zoneSo;
        [SerializeField] private List<ItemSo> items = new ();

        public ZoneSo ZoneSo => zoneSo;
    }
}
