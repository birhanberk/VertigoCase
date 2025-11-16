using System;
using Currency;
using Level;
using Sirenix.OdinInspector;
using UI.Entrance;
using UI.Fail;
using UI.GameEnd;
using UI.Inventory;
using UI.Item;
using UI.Progress;
using UI.Reward;
using UI.Wheel;
using UnityEngine;
using VContainer;

namespace UI
{
    public class UIDirector : MonoBehaviour
    {
        [SerializeField, Required] private EntranceUIController entranceUIController;
        [SerializeField, Required] private WheelUIController wheelUIController;
        [SerializeField, Required] private ProgressUIController progressUIController;
        [SerializeField, Required] private InventoryUIController inventoryUIController;
        [SerializeField, Required] private RewardUIController rewardUIController;
        [SerializeField, Required] private FailUIController failUIController;
        [SerializeField, Required] private GameEndUIController gameEndUIController;
        
        [SerializeField, Required] private ItemSo bombItem;

        private LevelManager _levelManager;
        private InventoryManager _inventoryManager;
        private CurrencyManager _currencyManager;

        public Action<bool> onCanLeaveChanged;

        [Inject]
        private void Construct(LevelManager levelManager, InventoryManager inventoryManager, CurrencyManager currencyManager)
        {
            _levelManager = levelManager;
            _inventoryManager = inventoryManager;
            _currencyManager = currencyManager;
        }

        private void Start()
        {
            _levelManager.onLevelStarted += OnLevelStarted;

            OnEntrance();
        }

        private void OnDestroy()
        {
            _levelManager.onLevelStarted -= OnLevelStarted;
        }

        private void OnEntrance()
        {
            entranceUIController.onStartPerformed += OnStartPerformed;
            entranceUIController.gameObject.SetActive(true);
            gameEndUIController.gameObject.SetActive(false);
        }

        private void OnLevelStarted(LevelSo levelSo)
        {
            wheelUIController.onSpinStateChanged += OnWheelSpinStateChanged;
            SetCanLeave(levelSo.ZoneSo.CanLeave);
        }

        private void OnWheelSpinStateChanged(bool state)
        {
            wheelUIController.onSpinStateChanged -= OnWheelSpinStateChanged;
            if (state)
            {
                SetCanLeave(false);
            }
        }

        private void SetCanLeave(bool value)
        {
            onCanLeaveChanged?.Invoke(value);
        }

        private void OnStartPerformed()
        {
            entranceUIController.onStartPerformed -= OnStartPerformed;
            entranceUIController.gameObject.SetActive(false);

            StartGame();
        }

        private void StartGame()
        {
            _levelManager.LoadLevel(0);
            wheelUIController.onItemSelected += OnWheelItemSelected;
            inventoryUIController.onLeavePerformed += OnInventoryLeavePerformed;
        }

        private void OnWheelItemSelected(WheelItemView wheelItemView)
        {
            var itemSo = wheelItemView.ItemSo;
            if (itemSo == bombItem)
            {
                ShowFailPanel();
            }
            else
            {
                ShowRewardPanel(wheelItemView);
            }
        }

        private void ShowFailPanel()
        {
            wheelUIController.onItemSelected -= OnWheelItemSelected;

            failUIController.onGiveUpPerformed += OnGiveUpPerformed;
            failUIController.onRevivePerformed += OnRevivePerformed;
            failUIController.gameObject.SetActive(true);
        }

        private void OnGiveUpPerformed()
        {
            CloseFailPanel();
            OnLeave();
        }

        private void OnRevivePerformed()
        {
            CloseFailPanel();
            wheelUIController.ReviveLevel();
            wheelUIController.onItemSelected += OnWheelItemSelected;
        }

        private void CloseFailPanel()
        {
            failUIController.onGiveUpPerformed -= OnGiveUpPerformed;
            failUIController.onRevivePerformed -= OnRevivePerformed;
            failUIController.gameObject.SetActive(false);
        }

        private void ShowRewardPanel(WheelItemView wheelItemView)
        {
            rewardUIController.onCompleted += OnRewardSequenceCompleted;
            var targetTransform = inventoryUIController.GetItemTransform(wheelItemView.ItemSo);
            rewardUIController.StartSequence(wheelItemView.ItemSo, wheelItemView.CurrentAmount, targetTransform);
        }

        private void OnRewardSequenceCompleted(ItemSo itemSo, int amount)
        {
            rewardUIController.onCompleted -= OnRewardSequenceCompleted;

            inventoryUIController.onItemAddCompleted += OnItemAddedInventory;
            _inventoryManager.AddItem(itemSo, amount);
        }

        private void OnItemAddedInventory()
        {
            inventoryUIController.onItemAddCompleted -= OnItemAddedInventory;
            if (_levelManager.IsLastLevel())
            {
                gameEndUIController.gameObject.SetActive(true);
                gameEndUIController.onRestartPerformed += OnGameEndRestart;
            }
            else
            {
                _levelManager.NextLevel();
            }
        }

        private void OnGameEndRestart()
        {
            gameEndUIController.onRestartPerformed -= OnGameEndRestart;
            OnLeave();
        }

        private void OnInventoryLeavePerformed()
        {
            OnLeave();
        }

        private void OnLeave()
        {
            inventoryUIController.onLeavePerformed -= OnInventoryLeavePerformed;
            wheelUIController.onItemSelected -= OnWheelItemSelected;
            
            EarnCurrencies();
            _inventoryManager.RemoveAll();
            OnEntrance();
        }

        private void EarnCurrencies()
        {
            foreach (var inventoryPair in _inventoryManager.Inventory)
            {
                if (inventoryPair.Key is CurrencyItemSo currencyItem)
                {
                    _currencyManager.AddCurrency(currencyItem.CurrencySo, inventoryPair.Value);
                }
            }
        }
    }
}