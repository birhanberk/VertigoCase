using System;
using System.Collections.Generic;
using Level;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Random = UnityEngine.Random;

namespace UI.Wheel
{
    public class WheelUIController : AutoButtonAssigner
    {
        [SerializeField, Required] private RectTransform wheelTransform;
        [SerializeField, Required] private RectTransform indicatorTransform;

        [SerializeField, ReadOnly, AutoButton("spin")] private Button spinButton;
        [SerializeField] private List<WheelItemView> itemSlots;

        [SerializeField] private WheelVisualController visualController;
        [SerializeField] private WheelSlotController slotController;
        [SerializeField] private WheelSpinController spinController;
        
        private int _selectedItemIndex;
        
        private LevelListSo _levelListSo;
        private LevelManager _levelManager;

        public Action<bool> onSpinStateChanged;
        public Action<WheelItemView> onItemSelected;

        [Inject]
        private void Construct(IObjectResolver objectResolver, LevelListSo levelListSo, LevelManager levelManager)
        {
            objectResolver.Inject(visualController);
            _levelListSo = levelListSo;
            _levelManager = levelManager;
        }

        private void Start()
        {
            visualController.OnStart();
            AddListeners();
        }

        private void OnDestroy()
        {
            visualController.OnDestroy();
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

        public void ReviveLevel()
        {
            spinController.Reset();
            spinButton.interactable = true;
        }

        private void OnLevelStarted(LevelSo levelSo)
        {
            slotController.InitializeSlots(levelSo, itemSlots, _levelListSo.GetLevelIndex(levelSo));
            ReviveLevel();
        }

        private void OnSpinButton()
        {
            onSpinStateChanged?.Invoke(true);
            spinButton.interactable = false;

            _selectedItemIndex = Random.Range(0, itemSlots.Count);
            spinController.StartSpin(_selectedItemIndex);
        }

        private void ResolveResult()
        {
            onSpinStateChanged?.Invoke(false);

            var itemSlot = itemSlots[_selectedItemIndex];
            onItemSelected?.Invoke(itemSlot);
        }
    }
}