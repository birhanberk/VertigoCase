using UnityEngine;

namespace UI.Reward
{
    [CreateAssetMenu(fileName = "Reward Data", menuName = "Data / Reward")]
    public class RewardSo : ScriptableObject
    {
        [SerializeField] private Color headerColor;
        [SerializeField] private Color cardColor;
        [SerializeField] private Color effectColor;

        public Color HeaderColor => headerColor;
        public Color CardColor => cardColor;
        public Color EffectColor => effectColor;
        
    }
}
