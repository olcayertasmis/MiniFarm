using System;
using MiniFarm.Data.EnumData;
using UnityEngine;

namespace MiniFarm.Gameplay.Resources
{
    public class ResourceManager
    {
        #region Variables

        public Action<int> OnHayResourceUpdated;
        public Action<int> OnFlourResourceUpdated;
        public Action<int> OnBreadResourceUpdated;

        private int _hayResourceAmount;
        private int _flourResourceAmount;
        private int _breadResourceAmount;

        #endregion

        #region Public Methods

        public void UpdateResource(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Hay:
                    UpdateHay(amount);
                    break;
                case ResourceType.Flour:
                    UpdateFlour(amount);
                    break;
                case ResourceType.Bread:
                    UpdateBread(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }

        public bool HasEnoughResource(ResourceType resourceType, int amount)
        {
            return resourceType switch
            {
                ResourceType.Hay => _hayResourceAmount >= amount,
                ResourceType.Flour => _flourResourceAmount >= amount,
                ResourceType.Bread => _breadResourceAmount >= amount,
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };
        }
        
        public void SaveResources()
        {
            PlayerPrefs.SetInt("HayResource", _hayResourceAmount);
            PlayerPrefs.SetInt("FlourResource", _flourResourceAmount);
            PlayerPrefs.SetInt("BreadResource", _breadResourceAmount);
            PlayerPrefs.Save();
        }

        public void LoadResources()
        {
            _hayResourceAmount = PlayerPrefs.GetInt("HayResource", 0);
            _flourResourceAmount = PlayerPrefs.GetInt("FlourResource", 0);
            _breadResourceAmount = PlayerPrefs.GetInt("BreadResource", 0);

            OnHayResourceUpdated?.Invoke(_hayResourceAmount);
            OnFlourResourceUpdated?.Invoke(_flourResourceAmount);
            OnBreadResourceUpdated?.Invoke(_breadResourceAmount);
        }

        #endregion

        #region Private Methods

        private void UpdateHay(int amount)
        {
            if (_hayResourceAmount + amount < 0) _hayResourceAmount = 0;
            else _hayResourceAmount += amount;
            OnHayResourceUpdated?.Invoke(_hayResourceAmount);
        }

        private void UpdateFlour(int amount)
        {
            if (_flourResourceAmount + amount < 0) _flourResourceAmount = 0;
            else _flourResourceAmount += amount;
            OnFlourResourceUpdated?.Invoke(_flourResourceAmount);
        }

        private void UpdateBread(int amount)
        {
            if (_breadResourceAmount + amount < 0) _breadResourceAmount = 0;
            else _breadResourceAmount += amount;
            OnBreadResourceUpdated?.Invoke(_breadResourceAmount);
        }

        #endregion
    }
}