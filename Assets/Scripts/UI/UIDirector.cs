using Entrance;
using Fail;
using Inventory;
using Item;
using Level;
using Progress;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Wheel;

namespace UI
{
    public class UIDirector : MonoBehaviour
    {
        [SerializeField, Required] private EntranceUIController entranceUIController;
        [SerializeField, Required] private WheelUIController wheelUIController;
        [SerializeField, Required] private ProgressController progressController;
        [SerializeField, Required] private InventoryUIController inventoryUIController;
        [SerializeField, Required] private RewardUIController rewardUIController;
        [SerializeField, Required] private FailUIController failUIController;
        
        [SerializeField, Required] private ItemSo bombItem;
        
        private LevelManager _levelManager;
        private InventoryManager _inventoryManager;

        public WheelUIController WheelUIController => wheelUIController;

        [Inject]
        private void Construct(LevelManager levelManager, InventoryManager inventoryManager)
        {
            _levelManager = levelManager;
            _inventoryManager = inventoryManager;
        }

        private void Start()
        {
            OnEntrance();
        }

        private void RemoveListeners()
        {
            wheelUIController.onItemSelected -= OnWheelItemSelected;
        }

        private void OnEntrance()
        {
            entranceUIController.onStartPerformed += OnStartPerformed;
            entranceUIController.gameObject.SetActive(true);
        }

        private void OnStartPerformed()
        {
            entranceUIController.onStartPerformed -= OnStartPerformed;
            entranceUIController.gameObject.SetActive(false);
            
            wheelUIController.onItemSelected += OnWheelItemSelected;
        }

        private void OnWheelItemSelected(ItemSlot itemSlot)
        {
            var itemSo = itemSlot.ItemSo;
            if (itemSo == bombItem)
            {
                ShowFailPanel();
            }
            else
            {
                ShowRewardPanel(itemSlot);
            }
        }

        private void ShowFailPanel()
        {
            failUIController.gameObject.SetActive(true);
        }

        private void ShowRewardPanel(ItemSlot itemSlot)
        {
            rewardUIController.onCompleted += OnRewardSequenceCompleted;
            var targetTransform = inventoryUIController.GetItemTransform(itemSlot.ItemSo);
            rewardUIController.StartSequence(itemSlot.ItemSo, itemSlot.Amount, targetTransform);
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
            _levelManager.NextLevel();
        }
    }
}
