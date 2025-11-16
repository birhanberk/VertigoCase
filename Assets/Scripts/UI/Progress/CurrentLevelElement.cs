using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Progress
{
    public class CurrentLevelElement : LevelElement
    {
        [SerializeField, Required] private Image background;
        
        public override void SetElement(LevelSo levelSo, int level)
        {
            base.SetElement(levelSo, level);
            text.color = levelSo.ZoneSo.ProgressSo.ActiveColor;
            background.color = levelSo.ZoneSo.ProgressSo.BackgroundColor;
        }
    }
}
