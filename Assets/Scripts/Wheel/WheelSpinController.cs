using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wheel
{
    [Serializable]
    public class WheelSpinController
    {
        [SerializeField] private RectTransform wheel;
        [SerializeField] private RectTransform indicator;

        [Title("Tick Settings")]
        [SerializeField] private float tickAngle = 30f;
        [SerializeField] private float tickDuration = 0.2f;

        [Title("Spin Settings")]
        [SerializeField] private Vector2 spinDurationRange = new(3.5f, 6f);
        [SerializeField] private Vector2 spinRoundsRange= new(3, 8);
        
        private const int _segmentCount = 8;
        private float _lastTickAngle;
        private float _tickPerSegment;

        public event Action onSpinCompleted;

        public void StartSpin()
        {
            var duration = Random.Range(spinDurationRange.x, spinDurationRange.y);
            var rounds = Random.Range(Mathf.RoundToInt(spinRoundsRange.x), Mathf.RoundToInt(spinRoundsRange.y));
            var endAngle = Random.Range(0, 360f);

            var totalRotation = rounds * 360f + endAngle;

            _tickPerSegment = 360f / _segmentCount;
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
            if (Mathf.Abs(angle - _lastTickAngle) >= _tickPerSegment)
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
