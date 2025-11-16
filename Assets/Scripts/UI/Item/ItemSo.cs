using Sirenix.OdinInspector;
using UI.Reward;
using UnityEngine;

namespace UI.Item
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Data / Item / Item")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private string itemHeader;
        [SerializeField, Required, PreviewField] private Sprite itemImage;
        [SerializeField] protected RewardSo rewardSo;
        [SerializeField] protected int baseAmount;

        public string ItemHeader { get => itemHeader; set  => itemHeader = value; }
        public Sprite ItemImage { get => itemImage; set  => itemImage = value; }
        public RewardSo RewardSo => rewardSo;

        public virtual int GetAmount(int levelIndex)
        {
            return baseAmount * CalculateMultiplier(levelIndex);
        }

        protected int CalculateMultiplier(int levelIndex)
        {
            var level = levelIndex + 1;
            
            var levelInterval = Mathf.FloorToInt(level / 5f);
            int levelMultiplier;
            if (level % 5 != 0)
            {
                levelMultiplier = levelInterval + 1;
            }
            else
            {
                levelMultiplier = 5 * levelInterval;
            }
            return levelMultiplier;
        }
    }
}
