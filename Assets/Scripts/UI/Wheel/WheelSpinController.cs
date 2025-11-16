using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Wheel
{
    [Serializable]
    public class WheelSpinController
    {
        [SerializeField] private RectTransform wheel;
        [SerializeField] private RectTransform indicator;

        [SerializeField] private float tickAngle = 30f;
        [SerializeField] private float tickDuration = 0.2f;

        [SerializeField] private float spinSpeedDegPerSec = 720f;
        [SerializeField] private Vector2Int spinRoundsRange= new(3, 8);
        
        private float _lastTickAngle;
        private const float tickPerSegment = 45f;

        public event Action onSpinCompleted;

        public void Reset()
        {
            wheel.localEulerAngles = Vector3.zero;
        }
        
        public void StartSpin(int index)
        {
            var rounds = Random.Range(spinRoundsRange.x, spinRoundsRange.y);

            var endAngle = -tickPerSegment * index;
            var totalRotation = rounds * 360f + endAngle;
            var duration = totalRotation / spinSpeedDegPerSec;

            _lastTickAngle = Mathf.Repeat(-wheel.localEulerAngles.z, 360f);

            wheel
                .DOLocalRotate(new Vector3(0f, 0f, -totalRotation), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart)
                .OnUpdate(HandleTick)
                .OnComplete(() => onSpinCompleted?.Invoke());
        }

        private void HandleTick()
        {
            var angle = Mathf.Repeat(-wheel.localEulerAngles.z, 360f);
            if (Mathf.Abs(angle - _lastTickAngle) >= tickPerSegment)
            {
                _lastTickAngle = angle;
                PlayIndicatorTick();
            }
        }

        private void PlayIndicatorTick()
        {
            indicator?.DOKill();

            indicator
                .DOLocalRotate(new Vector3(0, 0, tickAngle), tickDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    indicator
                        .DOLocalRotate(Vector3.zero, tickDuration)
                        .SetEase(Ease.InCubic);
                });
        }
    }
}