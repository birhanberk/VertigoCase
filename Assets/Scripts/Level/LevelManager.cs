using System;
using VContainer;

namespace Level
{
    public class LevelManager
    {
        private LevelSettingsSo _levelSettingsSo;
        private int _currentLevelIndex;

        public Action<LevelSo> onLevelStarted;

        [Inject]
        private void Construct(LevelSettingsSo levelSettingsSo)
        {
            _levelSettingsSo = levelSettingsSo;
        }

        public void NextLevel()
        {
            _currentLevelIndex++;
            LoadLevel(_currentLevelIndex);
        }
        
        public void LoadLevel(int levelIndex)
        {
            if (levelIndex < _levelSettingsSo.LevelList.Count)
            {
                onLevelStarted?.Invoke(_levelSettingsSo.LevelList[levelIndex]);
            }
        }

        public LevelSo GetCurrentLevelSo()
        {
            return _levelSettingsSo.LevelList[_currentLevelIndex];
        }
    }
}
