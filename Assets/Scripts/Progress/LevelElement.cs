using Level;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Progress
{
    public class LevelElement : MonoBehaviour
    {
        [SerializeField, Required] private RectTransform rectTransform;
        [SerializeField, Required] private TMP_Text text;
        [SerializeField] private float speed;

        public RectTransform RectTransform => rectTransform;

        public virtual void SetElement(LevelSo levelSo, int level)
        {
            text.text = level.ToString();
            text.color = levelSo.ZoneSo.ProgressSo.InactiveColor;
        }
    }
}
