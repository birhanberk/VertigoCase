using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    private LevelManager _levelManager;
    
    [Inject]
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _levelManager.LoadLevel(0);
    }

    [Button]
    private void NextLevel()
    {
        _levelManager.NextLevel();
    }
}
