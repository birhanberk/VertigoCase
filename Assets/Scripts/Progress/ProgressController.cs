using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Progress
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField, Required] private RectTransform rectTransform;
        [SerializeField] private LevelPanel levelPanel;
        [SerializeField] private LevelPanel currentLevelPanel;
        [SerializeField, ReadOnly] private LevelSo currentLevelSo;

        private LevelManager _levelManager;
        
        [Inject]
        private void Construct(IObjectResolver objectResolver, LevelManager levelManager)
        {
            objectResolver.Inject(levelPanel);
            objectResolver.Inject(currentLevelPanel);
            _levelManager = levelManager;
        }

        private void Start()
        {
            AddListeners();
            SetupUI();
        }

        private void OnDestroy()
        {
            RemoveListeners();
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
            if (currentLevelSo != levelSo)
            {
                currentLevelSo = levelSo;
                levelPanel.Shift();
                currentLevelPanel.Shift();
            }
        }

        private void SetupUI()
        {
            currentLevelSo = _levelManager.GetCurrentLevelSo();
            levelPanel.Initialize();
            currentLevelPanel.Initialize();
        }
    }
}
