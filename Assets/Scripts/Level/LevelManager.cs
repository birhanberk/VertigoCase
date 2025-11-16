using System;
using VContainer;

namespace Level
{
    public class LevelManager
    {
        private LevelListSo _levelListSo;
        private int _currentLevelIndex;

        public Action<LevelSo> onLevelStarted;

        [Inject]
        private void Construct(LevelListSo levelListSo)
        {
            _levelListSo = levelListSo;
        }

        public void NextLevel()
        {
            LoadLevel(_currentLevelIndex + 1);
        }
        
        public void LoadLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            if (levelIndex < _levelListSo.LevelList.Count)
            {
                onLevelStarted?.Invoke(_levelListSo.LevelList[levelIndex]);
            }
        }

        public LevelSo GetStartLevelSo()
        {
            return _levelListSo.LevelList[0];
        }

        public bool IsLastLevel()
        {
            return _currentLevelIndex == _levelListSo.LevelList.Count - 1;
        }
    }
}
