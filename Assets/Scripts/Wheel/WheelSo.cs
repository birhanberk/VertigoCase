using UnityEngine;

namespace Wheel
{
    [CreateAssetMenu(fileName = "Wheel Data", menuName = "Data / Wheel")]
    public class WheelSo : ScriptableObject
    {
        [SerializeField] private string wheelHeader;
        [SerializeField] private Sprite wheelBaseImage;
        [SerializeField] private Sprite wheelIndicatorImage;

        public string WheelHeader => wheelHeader;
        public Sprite WheelBaseImage => wheelBaseImage;
        public Sprite WheelIndicatorImage => wheelIndicatorImage;
    }
}
