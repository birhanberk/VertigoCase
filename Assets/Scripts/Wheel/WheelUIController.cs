using System;
using System.Collections.Generic;
using Item;
using Level;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Wheel
{
    public class WheelUIController : AutoRefAssigner
    {
        [SerializeField, Required] private RectTransform wheelTransform;
        [SerializeField, Required] private RectTransform indicatorTransform;

        [SerializeField, ReadOnly, AutoButton("spin")] private Button spinButton;
        [SerializeField, Required] private ItemSo bombItem;
        [SerializeField] private List<ItemSlot> itemSlots;

        [SerializeField] private WheelVisualController visualController;
        [SerializeField] private WheelSlotController slotController;
        [SerializeField] private WheelSpinController spinController;
        [SerializeField] private WheelResultResolver resultResolver;
        
        private LevelManager _levelManager;

        public Action<bool> onSpinStateChanged;
        public Action<ItemSlot> onItemSelected;

        [Inject]
        private void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
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
            spinButton.onClick.AddListener(OnSpinButton);
            spinController.onSpinCompleted += ResolveResult;
            _levelManager.onLevelStarted += OnLevelStarted;
        }

        private void RemoveListeners()
        {
            spinButton.onClick.RemoveListener(OnSpinButton);
            spinController.onSpinCompleted -= ResolveResult;
            _levelManager.onLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted(LevelSo levelSo)
        {
            slotController.InitializeSlots(levelSo, itemSlots);
            
            spinButton.interactable = true;
        }

        private void OnSpinButton()
        {
            onSpinStateChanged?.Invoke(true);
            spinButton.interactable = false;

            spinController.StartSpin();
        }

        private void ResolveResult()
        {
            var index = resultResolver.Resolve(wheelTransform, indicatorTransform, itemSlots);

            onSpinStateChanged?.Invoke(false);

            var itemSlot = itemSlots[index];
            onItemSelected?.Invoke(itemSlot);
        }
    }
}
