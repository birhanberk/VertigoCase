using System;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UI.Item;
using UnityEngine;

namespace UI.Reward
{
    public class RewardUIController : AutoButtonAssigner
    {
        [SerializeField] private RewardCardController rewardCardController;
        [SerializeField] private RewardPieceController rewardPieceController;

        public Action<ItemSo, int> onCompleted;

        private void Start()
        {
            rewardCardController.Close();
        }

        [Button]
        public async void StartSequence(ItemSo itemSo, int amount, RectTransform target)
        {
            await rewardCardController.StartSequence(itemSo, amount);
            await rewardPieceController.StartSequence(itemSo, target);
            rewardCardController.StartCloseSequence();
            onCompleted?.Invoke(itemSo, amount);
        }
    }
}