using UnityEngine;

namespace Currency
{
    [CreateAssetMenu(fileName = "Currency Data", menuName = "Data / Currency")]
    public class CurrencySo : ScriptableObject
    {
        [SerializeField] private string currencyHeader;
        [SerializeField] private Sprite currencyImage;

        public string CurrencyHeader => currencyHeader;
        public Sprite CurrencyImage => currencyImage;
    }
}
