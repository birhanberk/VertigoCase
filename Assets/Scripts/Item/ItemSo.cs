using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Data / Item")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private string itemHeader;
        [SerializeField] private Sprite itemImage;

        public string ItemHeader { get => itemHeader; set  => itemHeader = value; }
        public Sprite ItemImage { get => itemImage; set  => itemImage = value; }
    }
}
