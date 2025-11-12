using Level;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private LevelSettingsSo levelSettingsSo;
        
        [SerializeField] private GameManager gameManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(levelSettingsSo);
            builder.RegisterComponent(gameManager);
            builder.Register<LevelManager>(Lifetime.Singleton);
        }

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }
    }
}
