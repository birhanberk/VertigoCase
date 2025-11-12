using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Progress
{
    public class CurrentLevelElement : LevelElement
    {
        [SerializeField, Required] private Image background;
        
        public override void SetElement(LevelSo levelSo, int level)
        {
            base.SetElement(levelSo, level);
            background.sprite = levelSo.ZoneSo.ProgressSo.BackgroundImage;
        }
    }
}
