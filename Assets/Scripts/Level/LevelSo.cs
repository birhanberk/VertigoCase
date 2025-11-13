using System.Collections.Generic;
using Item;
using UnityEngine;
using Zone;

namespace Level
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Level / Level")]
    public class LevelSo : ScriptableObject
    {
        [SerializeField] private ZoneSo zoneSo;
        [SerializeField] private List<ItemSo> items = new ();

        public ZoneSo ZoneSo { get => zoneSo; set  => zoneSo = value; }
        public List<ItemSo> Items { get => items; set  => items = value; }
    }
}
