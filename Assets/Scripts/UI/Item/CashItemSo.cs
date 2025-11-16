using UnityEngine;

namespace UI.Item
{
    [CreateAssetMenu(fileName = "Cash Item Data", menuName = "Data / Item / Cash Item")]
    public class CashItemSo : CurrencyItemSo
    {
        [SerializeField] private int minAmount;
        
        public override int GetAmount(int levelIndex)
        {
            return Random.Range(minAmount, baseAmount) * CalculateMultiplier(levelIndex);
        }
    }
}
