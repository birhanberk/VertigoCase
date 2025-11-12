using UnityEngine;

namespace Progress
{
    [CreateAssetMenu(fileName = "Progress Data", menuName = "Data / Progress")]
    public class ProgressSo : ScriptableObject
    {
        [SerializeField] private Sprite backgroundImage;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        public Sprite BackgroundImage => backgroundImage;
        public Color ActiveColor => activeColor;
        public Color InactiveColor => inactiveColor;
    }
}
