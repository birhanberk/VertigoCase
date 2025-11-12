using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Data / Item")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private string itemHeader;
        [SerializeField] private Sprite itemImage;
    }
}
