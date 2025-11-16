using Currency;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Item
{
    [CreateAssetMenu(fileName = "Currency Item Data", menuName = "Data / Item / Currency Item")]
    public class CurrencyItemSo : ItemSo
    {
        [SerializeField, Required] private CurrencySo currencySo;

        public CurrencySo CurrencySo => currencySo;
    }
}
