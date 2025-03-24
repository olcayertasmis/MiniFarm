using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MiniFarm.Core;
using MiniFarm.Data.FactoryData;
using MiniFarm.Interfaces;
using MiniFarm.Managers;
using MiniFarm.UI;
using MiniFarm.Utilities;
using UnityEngine;
using Zenject;

namespace MiniFarm.Gameplay.Factories
{
    public abstract class BaseFactory : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] protected FactoryDataSO factoryData;
        [SerializeField] protected UISliderController uiSliderController;
        protected ResourceManager ResourceManager;
        private SaveManager _saveManager;

        [Header("Data")]
        private int _currentMaxCapacity;
        private int _currentProductAmount;
        [HideInInspector] public float remainingTime;

        [Header("Controls")]
        protected bool IsProducing;
        public readonly Queue<int> ProductionQueue = new();
        protected CancellationTokenSource CancellationTokenSource;

        [Header("Strategy")]
        [Inject] private IProductionStrategy _productionStrategy;

        #region Helpers

        public FactoryDataSO GetFactoryData => factoryData;

        #endregion

        public int CurrentProductAmount
        {
            get => _currentProductAmount;
            set
            {
                if (value < 0) _currentProductAmount = 0;
                else if (value > _currentMaxCapacity) _currentProductAmount = _currentMaxCapacity;
                else _currentProductAmount = value;
            }
        }
        protected int CurrentMaxCapacity
        {
            get => _currentMaxCapacity;
            private set
            {
                if (value < 0) _currentMaxCapacity = 0;
                else if (value > factoryData.GetDefaultCapacity) _currentMaxCapacity = factoryData.GetDefaultCapacity;
                else _currentMaxCapacity = value;
            }
        }

        #endregion

        #region Initialization

        [Inject]
        public void Construct(ResourceManager resourceManager, SaveManager saveManager)
        {
            ResourceManager = resourceManager;
            _saveManager = saveManager;
        }

        protected virtual void Awake()
        {
            CurrentMaxCapacity = factoryData.GetDefaultCapacity;
            remainingTime = factoryData.GetProductionTime;
        }

        protected virtual void Start()
        {
            LoadFactoryState();
            ProcessOfflineProduction();
            uiSliderController.SetSlider(factoryData.GetProductIcon, _currentProductAmount, GetRemainingTime(), factoryData.GetProductionTime, CurrentMaxCapacity);
        }

        #endregion

        #region Production Methods

        protected abstract UniTask StartProduction();

        protected virtual async Task StartProductionBody()
        {
            IsProducing = true;

            if (_productionStrategy != null)
            {
                await _productionStrategy.ProduceAsync(this);
            }

            IsProducing = false;
            Debug.Log($"Current {factoryData.GetProductType} Amount: {CurrentProductAmount}");

            //SaveFactoryState();
            UpdateUI();

            remainingTime = factoryData.GetProductionTime;
        }

        private void ProcessOfflineProduction()
        {
            TimeSpan elapsedTime = _saveManager.GetElapsedTimeSinceLastSave();
            if (elapsedTime <= TimeSpan.Zero) return;

            ProcessFactorySpecificOfflineProduction(elapsedTime);
        }

        protected abstract void ProcessFactorySpecificOfflineProduction(TimeSpan elapsedTime);

        public void StopProduction()
        {
            CancellationTokenSource?.Cancel();
            remainingTime = factoryData.GetProductionTime;
        }

        #endregion

        #region Collection Methods

        public virtual void CollectProduct()
        {
            if (CurrentProductAmount <= 0) return;

            ResourceManager.UpdateResource(factoryData.GetProductType, CurrentProductAmount);
            CurrentProductAmount = 0;
            if (!IsProducing) StartProduction().Forget();
            UpdateUI();
        }

        #endregion

        #region UI Methods

        public virtual void UpdateUI()
        {
            uiSliderController.UpdateSlider(CurrentProductAmount, GetRemainingTime());
        }

        private float GetRemainingTime()
        {
            if (ProductionQueue.Count <= 0) return factoryData.GetProductionTime;

            return remainingTime;
        }

        #endregion

        #region Save-Load Methods

        public void SaveFactoryState()
        {
            _saveManager.SaveFactory(
                factoryData.GetFactoryName,
                CurrentProductAmount,
                CurrentMaxCapacity,
                remainingTime,
                new Queue<int>(ProductionQueue)
            );
        }

        private void LoadFactoryState()
        {
            Debug.Log($"Loading Factory: {factoryData.GetFactoryName}");
            Debug.Log($"Saved JSON on Load: {PlayerPrefs.GetString(Constants.FactorySaveDataKey, "NO DATA")}");

            var savedFactories = _saveManager.LoadFactories();
            var myFactorySaveData = savedFactories.Find(f => f.factoryID == factoryData.GetFactoryName);

            if (myFactorySaveData != null)
            {
                CurrentProductAmount = myFactorySaveData.currentProductAmount;
                remainingTime = myFactorySaveData.remainingTime;
                CurrentMaxCapacity = myFactorySaveData.maxCapacity;
                if (myFactorySaveData.productionQueue is { Count: > 0 })
                {
                    foreach (var item in myFactorySaveData.productionQueue)
                    {
                        ProductionQueue.Enqueue(item);
                    }
                }
            }
        }

        #endregion
    }
}