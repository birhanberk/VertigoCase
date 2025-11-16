using System;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameEnd
{
    public class GameEndUIController : AutoButtonAssigner
    {
        [SerializeField, ReadOnly, AutoButton("restart")] private Button restartButton;
        
        public Action onRestartPerformed;

        private void Start()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            restartButton.onClick.AddListener(OnRestartButtonPerformed);
        }

        private void RemoveListeners()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonPerformed);
        }

        private void OnRestartButtonPerformed()
        {
            onRestartPerformed?.Invoke();
        }
    }
}
