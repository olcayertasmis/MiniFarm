using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MiniFarm.Core;
using MiniFarm.Data.FactoryData;
using MiniFarm.Gameplay.Resources;
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
        protected float RemainingTime;

        [Header("Controls")]
        protected bool IsProducing;
        protected readonly Queue<int> ProductionQueue = new();
        protected CancellationTokenSource CancellationTokenSource;

        #region Helpers

        public FactoryDataSO GetFactoryData => factoryData;

        #endregion

        protected int CurrentProductAmount
        {
            get => _currentProductAmount;
            private set
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
            RemainingTime = factoryData.GetProductionTime;
        }

        protected virtual void Start()
        {
            LoadFactoryState();
            uiSliderController.SetSlider(factoryData.GetProductIcon, _currentProductAmount, GetRemainingTime(), factoryData.GetProductionTime, CurrentMaxCapacity);
        }

        #endregion

        #region Production Methods

        protected abstract UniTask StartProduction();

        protected virtual async Task StartProductionBody()
        {
            IsProducing = true;

            while (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
                UpdateUI();
                await UniTask.Yield();
            }

            CurrentProductAmount++;
            ProductionQueue.Dequeue();
            IsProducing = false;
            Debug.Log($"Current {factoryData.GetProductType} Amount: {CurrentProductAmount}");

            //SaveFactoryState();
            UpdateUI();

            RemainingTime = factoryData.GetProductionTime;
        }

        public void StopProduction()
        {
            CancellationTokenSource?.Cancel();
            RemainingTime = factoryData.GetProductionTime;
        }

        #endregion

        #region Collection Methods

        public virtual void CollectProduct()
        {
            if (CurrentProductAmount <= 0) return;

            ResourceManager.UpdateResource(factoryData.GetProductType, CurrentProductAmount);
            CurrentProductAmount = 0;
            UpdateUI();
        }

        #endregion

        #region UI Methods

        protected virtual void UpdateUI()
        {
            uiSliderController.UpdateSlider(CurrentProductAmount, GetRemainingTime());
        }

        private float GetRemainingTime()
        {
            if (ProductionQueue.Count <= 0) return factoryData.GetProductionTime;

            return RemainingTime;
        }

        #endregion

        #region Save-Load Methods

        private void SaveFactoryState()
        {
            _saveManager.SaveFactory(
                factoryData.GetFactoryName,
                CurrentProductAmount,
                CurrentMaxCapacity,
                RemainingTime
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
                RemainingTime = myFactorySaveData.remainingTime;
                CurrentMaxCapacity = myFactorySaveData.maxCapacity;
            }
        }

        #endregion

        private void OnApplicationQuit()
        {
            Debug.Log("Application is quitting, saving factories...");
            SaveFactoryState();
        }
    }
}