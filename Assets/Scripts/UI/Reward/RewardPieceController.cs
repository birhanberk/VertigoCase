using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UI.Item;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace UI.Reward
{
    [Serializable]
    public class RewardPieceController
    {
        [SerializeField] private RectTransform pieceParent;
        [SerializeField] private ItemView piecePrefab;
        [SerializeField] private Vector2Int pieceCountRange = new(5, 8);
        [SerializeField] private Vector2 randomScaleRange = new(0.6f, 1.2f);

        [SerializeField] private float scatterDuration = 0.35f;
        [SerializeField] private float moveDuration = 0.55f;
        [SerializeField] private float popScaleMultiplier = 1.8f;
        [SerializeField] private float popDuration = 0.15f;

        private List<ItemView> _pieces = new();
        private readonly List<Tween> _activeTweens = new();

        public async Task StartSequence(ItemSo itemSo, RectTransform target)
        {
            KillTweens();

            var count = GetPieceCount();
            EnsurePieceCount(count);
            ResetPieces(itemSo);

            await ScatterAll(count);
            await MoveAllPiecesToTarget(count, target);
        }

        private int GetPieceCount()
        {
            return Random.Range(pieceCountRange.x, pieceCountRange.y);
        }

        private void EnsurePieceCount(int count)
        {
            while (_pieces.Count < count)
            {
                var newPiece = Object.Instantiate(piecePrefab, pieceParent);
                newPiece.gameObject.SetActive(false);
                _pieces.Add(newPiece);
            }
        }

        private void ResetPieces(ItemSo itemSo)
        {
            foreach (var piece in _pieces)
            {
                piece.UpdateItem(itemSo.ItemImage);
                piece.ItemTransform.anchoredPosition = Vector2.zero;
                piece.ItemTransform.localScale = Vector3.one;
                piece.gameObject.SetActive(false);
            }
        }

        private async Task ScatterAll(int count)
        {
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(ScatterPiece(_pieces[i]));

            await Task.WhenAll(tasks);
        }

        private async Task ScatterPiece(ItemView piece)
        {
            piece.gameObject.SetActive(true);

            piece.ItemTransform.localScale = Vector3.one * Random.Range(randomScaleRange.x, randomScaleRange.y);

            var scatterPos = Random.insideUnitCircle * Random.Range(100f, 220f);
            var scatterTween = piece.ItemTransform.DOAnchorPos(scatterPos, scatterDuration).SetEase(Ease.OutBack);

            _activeTweens.Add(scatterTween);

            await scatterTween.AsyncWaitForCompletion();
        }


        private async Task MoveAllPiecesToTarget(int count, RectTransform target)
        {
            var tasks = new List<Task>();

            var targetPos = pieceParent.InverseTransformPoint(target.position);
            var targetScale = target.lossyScale;

            for (var i = 0; i < count; i++)
                tasks.Add(AnimatePieceMove(_pieces[i], targetPos, targetScale));

            await Task.WhenAll(tasks);
        }

        private async Task AnimatePieceMove(ItemView piece, Vector2 targetPos, Vector3 targetScale)
        {
            var move = piece.ItemTransform.DOAnchorPos(targetPos, moveDuration).SetEase(Ease.InQuad);

            await move.AsyncWaitForCompletion();

            var scaleUp = piece.ItemTransform.DOScale(targetScale * popScaleMultiplier, popDuration).SetEase(Ease.OutBack);

            await scaleUp.AsyncWaitForCompletion();

            var scaleDown = piece.ItemTransform.DOScale(targetScale, popDuration).SetEase(Ease.InQuad);

            await scaleDown.AsyncWaitForCompletion();

            piece.gameObject.SetActive(false);
        }

        private void KillTweens()
        {
            foreach (var tween in _activeTweens)
                tween?.Kill();

            _activeTweens.Clear();
        }
    }
}
