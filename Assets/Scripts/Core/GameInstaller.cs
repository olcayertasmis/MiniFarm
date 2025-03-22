using MiniFarm.Gameplay.Factories;
using MiniFarm.Interfaces;
using MiniFarm.Managers;
using Zenject;

namespace MiniFarm.Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SaveManager>().AsSingle();

            Container.Bind<ResourceManager>().AsSingle();

            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<FactoryManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IProductionStrategy>().To<DefaultProductionStrategy>().WhenInjectedInto<BaseFactory>();
        }
    }
}