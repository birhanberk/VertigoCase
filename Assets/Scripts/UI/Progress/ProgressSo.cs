using UnityEngine;

namespace UI.Progress
{
    [CreateAssetMenu(fileName = "Progress Data", menuName = "Data / Progress")]
    public class ProgressSo : ScriptableObject
    {
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        public Color BackgroundColor => backgroundColor;
        public Color ActiveColor => activeColor;
        public Color InactiveColor => inactiveColor;
    }
}
