using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Item;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Reward
{
    [Serializable]
    public class RewardPieceController
    {
        [SerializeField] private RectTransform pieceParent;
        [SerializeField] private Image piecePrefab;
        [SerializeField] private Vector2Int pieceCountRange = new(5, 8);
        [SerializeField] private Vector2 randomScaleRange = new(0.6f, 1.2f);

        [SerializeField] private float scatterDuration = 0.35f;
        [SerializeField] private float moveDuration = 0.55f;
        [SerializeField] private float popScaleMultiplier = 1.8f;
        [SerializeField] private float popDuration = 0.15f;

        private List<Image> pieces = new();
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
            while (pieces.Count < count)
            {
                var newPiece = Object.Instantiate(piecePrefab, pieceParent);
                newPiece.gameObject.SetActive(false);
                pieces.Add(newPiece);
            }
        }

        private void ResetPieces(ItemSo itemSo)
        {
            foreach (var p in pieces)
            {
                p.sprite = itemSo.ItemImage;
                p.color = Color.white;
                p.rectTransform.anchoredPosition = Vector2.zero;
                p.rectTransform.localScale = Vector3.one;
                p.gameObject.SetActive(false);
            }
        }

        private async Task ScatterAll(int count)
        {
            var tasks = new List<Task>();

            for (var i = 0; i < count; i++)
                tasks.Add(ScatterPiece(pieces[i]));

            await Task.WhenAll(tasks);
        }

        private async Task ScatterPiece(Image piece)
        {
            piece.gameObject.SetActive(true);

            var rect = piece.rectTransform;
            rect.localScale = Vector3.one * Random.Range(randomScaleRange.x, randomScaleRange.y);

            var scatterPos = Random.insideUnitCircle * Random.Range(100f, 220f);
            var scatterTween = rect.DOAnchorPos(scatterPos, scatterDuration).SetEase(Ease.OutBack);

            _activeTweens.Add(scatterTween);

            await scatterTween.AsyncWaitForCompletion();
        }


        private async Task MoveAllPiecesToTarget(int count, RectTransform target)
        {
            var tasks = new List<Task>();

            var targetPos = pieceParent.InverseTransformPoint(target.position);
            var targetScale = target.lossyScale;

            for (var i = 0; i < count; i++)
                tasks.Add(AnimatePieceMove(pieces[i], targetPos, targetScale));

            await Task.WhenAll(tasks);
        }

        private async Task AnimatePieceMove(Image piece, Vector2 targetPos, Vector3 targetScale)
        {
            var rect = piece.rectTransform;

            var move = rect.DOAnchorPos(targetPos, moveDuration).SetEase(Ease.InQuad);

            await move.AsyncWaitForCompletion();

            var scaleUp = rect.DOScale(targetScale * popScaleMultiplier, popDuration).SetEase(Ease.OutBack);

            await scaleUp.AsyncWaitForCompletion();

            var scaleDown = rect.DOScale(targetScale, popDuration).SetEase(Ease.InQuad);

            await scaleDown.AsyncWaitForCompletion();

            piece.gameObject.SetActive(false);
        }

        private void KillTweens()
        {
            foreach (var t in _activeTweens)
                t?.Kill();

            _activeTweens.Clear();
        }
    }
}
