using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MiniFarm.Data.FactoryData;
using MiniFarm.Managers;
using MiniFarm.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MiniFarm.Gameplay.Factories
{
    public class AdvancedFactory : BaseFactory
    {
        #region Variables

        [Header("Produce UI References")]
        [SerializeField] private GameObject productionButtonPanel;
        [SerializeField] private ProduceButton produceButton;
        [SerializeField] private Button cancelButton;

        [Header("References")]
        private AdvancedUISliderController _advancedUISliderController;
        [Inject] private FactoryManager _factoryManager;

        [Header("Data")]
        private AdvancedFactoryDataSO _advancedFactoryData;

        #endregion

        #region Helpers

        public void SetProductionPanelActive(bool active) => productionButtonPanel.SetActive(active);

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            if (uiSliderController is AdvancedUISliderController advancedUISliderController) _advancedUISliderController = advancedUISliderController;
            if (factoryData is AdvancedFactoryDataSO advancedFactoryData) _advancedFactoryData = advancedFactoryData;
        }

        protected override void Start()
        {
            base.Start();

            var requiredResourceData = _advancedFactoryData.GetRequiredResourceData[0];
            produceButton.SetProduceButton(requiredResourceData.GetRequiredResourceData.GetResourceIcon(), requiredResourceData.RequiredAmount, OnProduceButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);

            if (productionButtonPanel.activeSelf) SetButtonPanelActiveness(false);

            UpdateUI();

            if (ProductionQueue.Count > 0 && !IsProducing) StartProduction().Forget();
        }

        #endregion

        #region Production Methods

        protected override async UniTask StartProduction()
        {
            CancellationTokenSource = new CancellationTokenSource();

            while (ProductionQueue.Count > 0 && CurrentProductAmount < CurrentMaxCapacity)
            {
                if (CancellationTokenSource.IsCancellationRequested) break;

                if (!IsProducing) await StartProductionBody();
            }
        }

        protected override void ProcessFactorySpecificOfflineProduction(TimeSpan elapsedTime)
        {
            var productionTime = factoryData.GetProductionTime;
            var productionCount = (int)(elapsedTime.TotalSeconds / productionTime);

            for (int i = 0; i < productionCount; i++)
            {
                if (CurrentProductAmount >= CurrentMaxCapacity) break;

                CurrentProductAmount++;
                if (ProductionQueue.Count > 0) ProductionQueue.Dequeue();
            }

            UpdateUI();
        }

        private bool AddProductionOrder()
        {
            if (CurrentProductAmount < CurrentMaxCapacity && HasRequiredResources())
            {
                ConsumeRequiredResources();
                ProductionQueue.Enqueue(1);
                return true;
            }

            return false;
        }

        private void CancelProductionOrder()
        {
            if (ProductionQueue.Count > 0)
            {
                ProductionQueue.Dequeue();
                GiveBackRequiredResources();
                if (ProductionQueue.Count <= 0) remainingTime = factoryData.GetProductionTime;
            }
        }

        #endregion

        #region Button Methods

        private void SetButtonPanelActiveness(bool active) => productionButtonPanel.SetActive(active);

        private void OnProduceButtonClicked()
        {
            if (!AddProductionOrder()) return;

            if (!IsProducing) StartProduction().Forget();

            UpdateUI();
        }

        private void UpdateProduceButtonState()
        {
            produceButton.SetInteractable(CurrentProductAmount < CurrentMaxCapacity && HasRequiredResources());
        }

        private void OnCancelButtonClicked()
        {
            CancelProductionOrder();
            UpdateUI();
        }

        private void UpdateCancelButtonState() => cancelButton.interactable = ProductionQueue.Count > 0;

        #endregion

        #region Resource Methods

        private bool HasRequiredResources()
        {
            foreach (var requiredResource in _advancedFactoryData.GetRequiredResourceData)
            {
                if (!ResourceManager.HasEnoughResource(requiredResource.GetRequiredResourceData.GetResourceType(), requiredResource.RequiredAmount))
                {
                    return false;
                }
            }

            return true;
        }

        private void ConsumeRequiredResources()
        {
            foreach (var requiredResource in _advancedFactoryData.GetRequiredResourceData)
            {
                ResourceManager.UpdateResource(requiredResource.GetRequiredResourceData.GetResourceType(), -requiredResource.RequiredAmount);
            }
        }

        private void GiveBackRequiredResources()
        {
            foreach (var requiredResource in _advancedFactoryData.GetRequiredResourceData)
            {
                ResourceManager.UpdateResource(requiredResource.GetRequiredResourceData.GetResourceType(), requiredResource.RequiredAmount);
            }
        }

        #endregion

        #region UI Methods

        public override void UpdateUI()
        {
            base.UpdateUI();
            _advancedUISliderController.UpdateProductionQueue(ProductionQueue.Count, CurrentMaxCapacity);
            UpdateProduceButtonState();
            UpdateCancelButtonState();
        }

        #endregion
    }
}