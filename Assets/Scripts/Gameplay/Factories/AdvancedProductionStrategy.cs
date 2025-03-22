using Cysharp.Threading.Tasks;
using MiniFarm.Interfaces;

namespace MiniFarm.Gameplay.Factories
{
    public class AdvancedProductionStrategy : IProductionStrategy
    {
        public async UniTask ProduceAsync(BaseFactory factory)
        {
            await UniTask.Yield();
        }
    }
}