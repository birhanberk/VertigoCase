using Sirenix.OdinInspector;
using UI.Item;
using UnityEngine;
using VContainer;

namespace Currency
{
    public class CurrencyItem : ItemView
    {
        [SerializeField, Required] private CurrencySo currencySo;
        
        private CurrencyManager _currencyManager;

        [Inject]
        private void Construct(CurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        private void Start()
        {
            SetData();
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void SetData()
        {
            UpdateItem(currencySo.CurrencyImage, _currencyManager.Currencies[currencySo]);
        }

        private void AddListeners()
        {
            _currencyManager.onCurrencyChanged += OnCurrencyChanged;
        }

        private void RemoveListeners()
        {
            _currencyManager.onCurrencyChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(CurrencySo changedCurrencySo, int amount)
        {
            if (currencySo == changedCurrencySo)
            {
                UpdateAmount(amount);
            }
        }
    }
}
