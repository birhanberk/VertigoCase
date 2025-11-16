using System;
using Sirenix.OdinInspector;
using UI.AutoButton;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Entrance
{
    public class EntranceUIController : AutoButtonAssigner
    {
        [SerializeField, ReadOnly, AutoButton("start")] private Button startButton;
        
        public Action onStartPerformed;
        
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
            startButton.onClick.AddListener(OnStartButtonPerformed);
        }

        private void RemoveListeners()
        {
            startButton.onClick.RemoveListener(OnStartButtonPerformed);
        }

        private void OnStartButtonPerformed()
        {
            onStartPerformed?.Invoke();
        }
    }
}
