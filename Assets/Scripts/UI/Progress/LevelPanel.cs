using System;
using System.Collections.Generic;
using DG.Tweening;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace UI.Progress
{
    [Serializable]
    public class LevelPanel
    {
        [SerializeField, Required] private RectTransform rectTransform;
        [SerializeField, Required] private RectTransform elementParent;
        [SerializeField, Required] private LevelElement elementPrefab;
        [SerializeField] private int elementCount = 14;
        [SerializeField] private float moveDuration = 0.35f;
        [SerializeField, ReadOnly] private List<LevelElement> levelElements;
        [SerializeField, ReadOnly] private int currentLevel = 1;

        private Queue<LevelSo> levelSoQueue = new ();
        private Sequence _shiftSequence;

        private LevelListSo _levelListSo;
        
        [Inject]
        private void Construct(LevelListSo levelListSo)
        {
            _levelListSo = levelListSo;
        }

        public virtual void Initialize()
        {
            Reset();
            for (var i = 0; i < _levelListSo.LevelList.Count; i++)
            {
                var levelSo = _levelListSo.LevelList[i];
                if (i > elementCount - 1)
                {
                    levelSoQueue.Enqueue(levelSo);
                }
                else
                {
                    CreateElement(levelSo, i);
                }
            }
        }

        private void Reset()
        {
            levelSoQueue.Clear();
            currentLevel = 1;
            _shiftSequence?.Kill();
            foreach (var element in levelElements)
            {
                Object.Destroy(element.gameObject);
            }
            levelElements.Clear();
        }

        private void CreateElement(LevelSo levelSo, int index)
        {
            var levelElement = Object.Instantiate(elementPrefab, elementParent);
            levelElement.SetElement(levelSo, index + 1);
            levelElements.Add(levelElement);

            var anchoredPos = levelElement.RectTransform.anchoredPosition;
            anchoredPos.x = index * levelElement.RectTransform.rect.width;
            levelElement.RectTransform.anchoredPosition = anchoredPos;
        }
        
        public void Shift()
        {
            _shiftSequence?.Complete(true);
            _shiftSequence?.Kill(true);

            var elementWidth = levelElements[0].RectTransform.rect.width;
            currentLevel++;

            _shiftSequence = DOTween.Sequence();

            foreach (var element in levelElements)
            {
                var targetX = element.RectTransform.anchoredPosition.x - elementWidth;
                _shiftSequence.Join(
                    element.RectTransform.DOAnchorPosX(targetX, moveDuration).SetEase(Ease.OutQuad)
                );
            }

            if (currentLevel > elementCount / 2 && levelSoQueue.Count > 0)
            {
                var firstElement = levelElements[0];
                _shiftSequence.OnComplete(() =>
                {
                    levelElements.RemoveAt(0);

                    var newPos = firstElement.RectTransform.anchoredPosition;
                    newPos.x = levelElements[^1].RectTransform.anchoredPosition.x + elementWidth;
                    firstElement.RectTransform.anchoredPosition = newPos;

                    firstElement.SetElement(levelSoQueue.Dequeue(), currentLevel + elementCount / 2);
                    levelElements.Add(firstElement);
                });
            }

            _shiftSequence.Play();
        }
    }
}
