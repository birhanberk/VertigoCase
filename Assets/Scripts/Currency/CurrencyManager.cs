using System;
using System.Collections.Generic;
using UnityEngine;

namespace Currency
{
    [Serializable]
    public class CurrencyManager
    {
        [SerializeField] private CurrencySo cashSo;
        [SerializeField] private CurrencySo itemSo;
        [SerializeField] private int demoValue = 1000;
        
        private Dictionary<CurrencySo, int> _currencies = new ();

        public Dictionary<CurrencySo, int> Currencies => _currencies;

        public Action<CurrencySo, int> onCurrencyChanged;
        public Action onReset;

        public void OnStart()
        {
            Load();
        }

        private void Load()
        {
            if (cashSo)
            {
                _currencies.Add(cashSo, demoValue);
            }
            if (itemSo)
            {
                _currencies.Add(itemSo, demoValue);
            }
        }

        public void AddCurrency(CurrencySo currencySo, int amount)
        {
            if (!_currencies.TryAdd(currencySo, amount))
            {
                _currencies[currencySo] += amount;
            }
            onCurrencyChanged?.Invoke(currencySo, _currencies[currencySo]);
        }

        public bool CanConsumeCurrency(CurrencySo currencySo, int amount)
        {
            if (_currencies.TryGetValue(currencySo, out var currency))
            {
                return currency >= amount;
            }
            return false;
        }

        public void ConsumeCurrency(CurrencySo currencySo, int amount)
        {
            if (CanConsumeCurrency(currencySo, amount))
            {
                _currencies[currencySo] -= amount;
                onCurrencyChanged?.Invoke(currencySo, _currencies[currencySo]);
            }
        }

        public void RemoveAll()
        {
            _currencies.Clear();
            onReset?.Invoke();
        }
    }
}
