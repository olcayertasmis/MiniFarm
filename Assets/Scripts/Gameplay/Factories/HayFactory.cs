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

            while (!CancellationTokenSource.IsCancellationRequested)
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