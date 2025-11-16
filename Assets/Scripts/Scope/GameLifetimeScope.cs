using Currency;
using Level;
using Sirenix.OdinInspector;
using UI;
using UI.Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField, Required] private LevelListSo levelListSo;
        
        [SerializeField, Required] private UIDirector uiDirector;
        [SerializeField] private CurrencyManager currencyManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(levelListSo);
            builder.RegisterComponent(uiDirector);
            builder.Register<LevelManager>(Lifetime.Singleton);
            builder.Register<InventoryManager>(Lifetime.Singleton);
            builder.RegisterInstance(currencyManager);
        }

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            currencyManager.OnStart();
        }
    }
}
