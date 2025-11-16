using System;
using Level;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Wheel
{
    [Serializable]
    public class WheelVisualController
    {
        [SerializeField, Required] private TMP_Text headerText;
        [SerializeField, Required] private Image wheelBase;
        [SerializeField, Required] private Image wheelIndicator;

        private LevelManager _levelManager;
        
        [Inject]
        private void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void OnStart()
        {
            AddListeners();
        }

        public void OnDestroy()
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
            var wheelSo = levelSo.ZoneSo.WheelSo;
            
            headerText.text = wheelSo.WheelHeader;
            headerText.color = wheelSo.WheelHeaderColor;
            wheelBase.sprite = wheelSo.WheelBaseImage;
            wheelIndicator.sprite = wheelSo.WheelIndicatorImage;
        }
    }
}
