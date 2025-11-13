using Inventory;
using Level;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField, Required] private LevelSettingsSo levelSettingsSo;
        
        [SerializeField, Required] private UIDirector uiDirector;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(levelSettingsSo);
            builder.RegisterComponent(uiDirector);
            builder.Register<LevelManager>(Lifetime.Singleton);
            builder.Register<InventoryManager>(Lifetime.Singleton);
        }

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }
    }
}
