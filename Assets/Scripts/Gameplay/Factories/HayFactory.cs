using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MiniFarm.Gameplay.Factories
{
    public class HayFactory : BaseFactory
    {
        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            StartProduction().Forget();
        }

        #endregion

        #region Production Methods

        protected override async UniTask StartProduction()
        {
            CancellationTokenSource = new CancellationTokenSource();

            while (!CancellationTokenSource.IsCancellationRequested && CurrentProductAmount < CurrentMaxCapacity)
            {
                if (CurrentProductAmount < CurrentMaxCapacity)
                {
                    ProductionQueue.Enqueue(1);
                }

                if (ProductionQueue.Count > 0 && !IsProducing)
                {
                    await StartProductionBody();
                }

                await UniTask.Yield();
            }
        }

        protected override void ProcessFactorySpecificOfflineProduction(TimeSpan elapsedTime)
        {
            var productionTime = factoryData.GetProductionTime;
            var productionCount = (int)(elapsedTime.TotalSeconds / productionTime);

            CurrentProductAmount += productionCount;
            if (ProductionQueue.Count > 0) ProductionQueue.Dequeue();
            if (CurrentProductAmount > CurrentMaxCapacity)
            {
                CurrentProductAmount = CurrentMaxCapacity;
            }

            UpdateUI();
        }

        #endregion

        #region Click Handling

        private void OnMouseDown()
        {
            OnFactoryClicked();
        }

        private void OnFactoryClicked()
        {
            CollectProduct();
        }

        #endregion
    }
}