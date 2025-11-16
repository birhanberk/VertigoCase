using Level;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UnityEngine;
using VContainer;

namespace UI.Progress
{
    public class ProgressUIController : AutoButtonAssigner
    {
        [SerializeField, Required] private RectTransform rectTransform;
        [SerializeField] private LevelPanel levelPanel;
        [SerializeField] private LevelPanel currentLevelPanel;
        [SerializeField, ReadOnly] private LevelSo startLevelSo;

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
            startLevelSo = _levelManager.GetStartLevelSo();
            AddListeners();
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
            if (startLevelSo == levelSo)
            {
                SetupUI();
            }
            else
            {
                levelPanel.Shift();
                currentLevelPanel.Shift();
            }
        }

        private void SetupUI()
        {
            levelPanel.Initialize();
            currentLevelPanel.Initialize();
        }
    }
}
