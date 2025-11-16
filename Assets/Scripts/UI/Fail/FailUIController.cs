using System;
using Currency;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Fail
{
    public class FailUIController : AutoButtonAssigner
    {
        [SerializeField, Required] private CurrencySo currencySo;
        [SerializeField, Required] private int amount;
        [SerializeField, Required] private VignetteController vignetteController;
        [SerializeField, ReadOnly, AutoButton("give")] private Button giveUpButton;
        [SerializeField, ReadOnly, AutoButton("revive")] private Button reviveButton;
        
        private CurrencyManager _currencyManager;
        
        public Action onGiveUpPerformed;
        public Action onRevivePerformed;

        [Inject]
        private void Construct(CurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }
        
        private void Start()
        {
            vignetteController.PlayVignette();
            AddListeners();
            CheckCanRevive();
        }

        private void OnDestroy()
        {
            vignetteController.StopVignette();
            RemoveListeners();
        }

        private void AddListeners()
        {
            giveUpButton.onClick.AddListener(OnGiveUpButtonPerformed);
            reviveButton.onClick.AddListener(OnReviveButtonPerformed);
        }

        private void RemoveListeners()
        {
            giveUpButton.onClick.RemoveListener(OnGiveUpButtonPerformed);
            reviveButton.onClick.RemoveListener(OnReviveButtonPerformed);
        }

        private void OnGiveUpButtonPerformed()
        {
            onGiveUpPerformed?.Invoke();
        }

        private void CheckCanRevive()
        {
            reviveButton.interactable = _currencyManager.CanConsumeCurrency(currencySo, amount);
        }

        private void OnReviveButtonPerformed()
        {
            _currencyManager.ConsumeCurrency(currencySo, amount);
            onRevivePerformed?.Invoke();
        }
    }
}
