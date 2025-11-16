using Level;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Zone
{
    public class ZoneElement : MonoBehaviour
    {
        [SerializeField, Required] protected ZoneSo zoneSo;
        [SerializeField, Required] private TMP_Text levelText;

        private LevelListSo _levelListSo;
        private LevelManager _levelManager;
        
        [Inject]
        private void Construct(LevelListSo levelListSo, LevelManager levelManager)
        {
            _levelListSo = levelListSo;
            _levelManager = levelManager;
        }
        
        private void Start()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void SetData(int level)
        {
            levelText.text = level.ToString();
        }

        private void AddListeners()
        {
            _levelManager.onLevelStarted += OnLevelStarted;
        }

        private void RemoveListeners()
        {
            _levelManager.onLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted(LevelSo levelSo)
        {
            var hasLevel = false;
            var index = _levelListSo.LevelList.IndexOf(levelSo);
            for (var i = index; i < _levelListSo.LevelList.Count; i++)
            {
                if (levelSo != _levelListSo.LevelList[i] && _levelListSo.LevelList[i].ZoneSo == zoneSo)
                {
                    SetData(i + 1);
                    hasLevel = true;
                    break;
                }
            }
            if (!hasLevel)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
