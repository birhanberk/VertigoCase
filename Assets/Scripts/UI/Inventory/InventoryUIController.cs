using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UI.Item;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Inventory
{
    public class InventoryUIController : AutoButtonAssigner
    {
        [SerializeField, ReadOnly, AutoButton("leave")] private Button leaveButton;
        [SerializeField, Required] private InventoryItemView inventoryItemViewPrefab;
        [SerializeField, Required] private Transform itemViewParent;
        
        private readonly Dictionary<ItemSo, InventoryItemView> _items = new ();
        
        private InventoryManager _inventoryManager;
        private UIDirector _uiDirector;

        public Action onItemAddCompleted;
        public Action onLeavePerformed;
        
        [Inject]
        private void Construct(InventoryManager inventoryManager, UIDirector uiDirector)
        {
            _inventoryManager = inventoryManager;
            _uiDirector = uiDirector;
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
            _uiDirector.onCanLeaveChanged += OnCanLeaveChanged;
            _inventoryManager.onItemChanged += OnItemChanged;
            _inventoryManager.onReset += OnReset;
            leaveButton.onClick.AddListener(OnLeaveButtonPerformed);
        }

        private void RemoveListeners()
        {
            _uiDirector.onCanLeaveChanged  -= OnCanLeaveChanged;
            _inventoryManager.onItemChanged -= OnItemChanged;
            _inventoryManager.onReset -= OnReset;
            leaveButton.onClick.RemoveListener(OnLeaveButtonPerformed);
        }

        private void OnCanLeaveChanged(bool value)
        {
            leaveButton.interactable = value;
        }

        private void OnLeaveButtonPerformed()
        {
            onLeavePerformed?.Invoke();
        }

        private async void OnItemChanged(ItemSo itemSo, int amount)
        {
            if (!_items.ContainsKey(itemSo))
            {
                CreateItemView(itemSo);
            }
            await _items[itemSo].UpdateAmountIncrease(amount);
            onItemAddCompleted?.Invoke();
        }

        private void CreateItemView(ItemSo itemSo)
        {
            var itemHolder = Instantiate(inventoryItemViewPrefab, itemViewParent);
            itemHolder.UpdateItem(itemSo.ItemImage, 0);
            _items.Add(itemSo, itemHolder);
            if (itemSo is CurrencyItemSo)
            {
                itemHolder.transform.SetAsFirstSibling();
            }
        }

        private void OnReset()
        {
            foreach (var itemHolder in _items.Values)
            {
                Destroy(itemHolder.gameObject);
            }
            _items.Clear();
        }

        public RectTransform GetItemTransform(ItemSo itemSo)
        {
            if (!_items.ContainsKey(itemSo))
            {
                CreateItemView(itemSo);
            }
            return _items[itemSo].ItemTransform;
        }
    }
}
