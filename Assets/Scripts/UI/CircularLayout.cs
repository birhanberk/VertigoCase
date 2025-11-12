using UnityEngine;

namespace UI
{
    public class CircularLayout : MonoBehaviour
    {
        [SerializeField] private float radius = 100f;
        [SerializeField] private float startAngle;
        [SerializeField] private bool clockwise = true;

        private void OnValidate()
        {
            ArrangeChildren();
        }

        private void ArrangeChildren()
        {
            var count = transform.childCount;
            if (count == 0) return;

            var angleStep = 360f / count;
            var direction = clockwise ? -1f : 1f;

            for (var i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (child is RectTransform rect)
                {
                    var angle = (startAngle + i * angleStep * direction) * Mathf.Deg2Rad;
                    rect.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    rect.localRotation = Quaternion.identity;
                }
            }
        }
    }
}
