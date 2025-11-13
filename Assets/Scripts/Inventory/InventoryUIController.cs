using System;
using System.Collections.Generic;
using Item;
using Level;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Zone;

namespace Inventory
{
    public class InventoryUIController : AutoRefAssigner
    {
        [SerializeField, ReadOnly, AutoButton("leave")] private Button leaveButton;
        [SerializeField, Required] private ItemHolder itemHolderPrefab;
        [SerializeField, Required] private Transform itemHolderParent;
        
        private readonly Dictionary<ItemSo, ItemHolder> items = new ();
        private ZoneSo _zoneSo;
        
        private LevelManager _levelManager;
        private InventoryManager _inventoryManager;
        private UIDirector uiDirector;

        public Action onItemAddCompleted;
        
        [Inject]
        private void Construct(LevelManager levelManager, InventoryManager inventoryManager, UIDirector uiDirector)
        {
            _levelManager = levelManager;
            _inventoryManager = inventoryManager;
            this.uiDirector = uiDirector;
        }

        private void Start()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _levelManager.onLevelStarted += OnLevelStarted;
            uiDirector.WheelUIController.onSpinStateChanged += OnSpinStateChanged;
            _inventoryManager.onItemChanged += OnItemChanged;
            _inventoryManager.onReset += OnReset;
            leaveButton.onClick.AddListener(OnLeaveButtonPerformed);
        }

        private void RemoveListeners()
        {
            _levelManager.onLevelStarted -= OnLevelStarted;
            uiDirector.WheelUIController.onSpinStateChanged  -= OnSpinStateChanged;
            _inventoryManager.onItemChanged -= OnItemChanged;
            _inventoryManager.onReset -= OnReset;
            leaveButton.onClick.RemoveListener(OnLeaveButtonPerformed);
        }

        private void OnLevelStarted(LevelSo levelSo)
        {
            _zoneSo = levelSo.ZoneSo;
            leaveButton.interactable = _zoneSo.CanLeave;
        }

        private void OnSpinStateChanged(bool value)
        {
            if (_zoneSo.CanLeave)
            {
                leaveButton.interactable = !value;
            }
        }

        private void OnLeaveButtonPerformed()
        {
            
        }

        private async void OnItemChanged(ItemSo itemSo, int amount)
        {
            if (!items.ContainsKey(itemSo))
            {
                CreateItemHolder(itemSo);
            }
            await items[itemSo].UpdateItem(amount);
            onItemAddCompleted.Invoke();
        }

        private void CreateItemHolder(ItemSo itemSo)
        {
            var itemHolder = Instantiate(itemHolderPrefab, itemHolderParent);
            itemHolder.UpdateItem(itemSo.ItemImage, 0);
            items.Add(itemSo, itemHolder);
        }

        private void OnReset()
        {
            foreach (var itemHolder in items.Values)
            {
                Destroy(itemHolder.gameObject);
            }
            items.Clear();
        }

        public RectTransform GetItemTransform(ItemSo itemSo)
        {
            if (!items.ContainsKey(itemSo))
            {
                CreateItemHolder(itemSo);
            }
            return items[itemSo].Image.rectTransform;
        }
    }
}
