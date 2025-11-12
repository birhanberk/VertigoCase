using System;
using System.Collections.Generic;
using DG.Tweening;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Progress
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
        [SerializeField, ReadOnly] private int _currentLevel = 1;

        private Queue<LevelSo> levelSoQueue = new ();
        private Sequence _shiftSequence;

        private LevelSettingsSo _levelSettingsSo;
        
        [Inject]
        private void Construct(LevelSettingsSo levelSettingsSo)
        {
            _levelSettingsSo = levelSettingsSo;
        }

        public virtual void Initialize()
        {
            for (var i = 0; i < _levelSettingsSo.LevelList.Count; i++)
            {
                var levelSo = _levelSettingsSo.LevelList[i];
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

        private void CreateElement(LevelSo levelSo, int index)
        {
            var levelElement = Object.Instantiate(elementPrefab, elementParent);
            levelElement.SetElement(levelSo, index + 1);
            levelElements.Add(levelElement);

            var anchoredPos = levelElement.RectTransform.anchoredPosition;
            anchoredPos.x = index * levelElement.RectTransform.rect.width;
            levelElement.RectTransform.anchoredPosition = anchoredPos;
        }

        private float StartOffset()
        {
            return rectTransform.rect.width / 2;
        }
        
        public void Shift()
        {
            _shiftSequence?.Complete(true);
            _shiftSequence?.Kill(true);

            var elementWidth = levelElements[0].RectTransform.rect.width;
            _currentLevel++;

            _shiftSequence = DOTween.Sequence();

            foreach (var element in levelElements)
            {
                var targetX = element.RectTransform.anchoredPosition.x - elementWidth;
                _shiftSequence.Join(
                    element.RectTransform.DOAnchorPosX(targetX, moveDuration).SetEase(Ease.OutQuad)
                );
            }

            if (_currentLevel > elementCount / 2 && levelSoQueue.Count > 0)
            {
                var firstElement = levelElements[0];
                _shiftSequence.OnComplete(() =>
                {
                    levelElements.RemoveAt(0);

                    var newPos = firstElement.RectTransform.anchoredPosition;
                    newPos.x = levelElements[^1].RectTransform.anchoredPosition.x + elementWidth;
                    firstElement.RectTransform.anchoredPosition = newPos;

                    firstElement.SetElement(levelSoQueue.Dequeue(), _currentLevel + elementCount / 2);
                    levelElements.Add(firstElement);
                });
            }

            _shiftSequence.Play();
        }
    }
}
