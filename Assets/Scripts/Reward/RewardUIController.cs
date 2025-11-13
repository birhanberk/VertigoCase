using System;
using Item;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Reward
{
    public class RewardUIController : MonoBehaviour
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