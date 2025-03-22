using Cysharp.Threading.Tasks;
using MiniFarm.Interfaces;
using UnityEngine;

namespace MiniFarm.Gameplay.Factories
{
    public class DefaultProductionStrategy : IProductionStrategy
    {
        public async UniTask ProduceAsync(BaseFactory factory)
        {
            while (factory.remainingTime > 0)
            {
                factory.remainingTime -= Time.deltaTime;
                factory.UpdateUI();
                await UniTask.Yield();
            }

            factory.CurrentProductAmount++;
            factory.ProductionQueue.Dequeue();
        }
    }
}