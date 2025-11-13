using System;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Entrance
{
    public class EntranceUIController : AutoRefAssigner
    {
        [SerializeField, ReadOnly, AutoButton("start")] private Button startButton;
        
        public Action onStartPerformed;
        
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
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
