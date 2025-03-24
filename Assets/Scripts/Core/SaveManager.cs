using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MiniFarm.Data.FactoryData;
using MiniFarm.Managers;
using MiniFarm.Utilities;
using UnityEngine;
using Zenject;

namespace MiniFarm.Core
{
    public class SaveManager
    {
        #region Variables

        [Header("References")]
        private ResourceManager _resourceManager;
        private FactorySpawnManager _factorySpawnManager;

        [Header("Data")]
        private readonly Dictionary<string, FactorySaveData> _factorySaveData = new();

        #endregion

        [Inject]
        public void Construct(ResourceManager resourceManager, FactorySpawnManager factorySpawnManager)
        {
            _resourceManager = resourceManager;
            _factorySpawnManager = factorySpawnManager;
        }

        #region Public Methods

        public async Task SaveGame()
        {
            _resourceManager.SaveResources();

            foreach (var factory in _factorySpawnManager.GetSpawnedFactories())
            {
                factory.SaveFactoryState();
            }

            await UniTask.Delay(100);
        }

        public async Task LoadGame()
        {
            _resourceManager.LoadResources();
            await UniTask.Delay(100);
        }

        public TimeSpan GetElapsedTimeSinceLastSave()
        {
            if (!PlayerPrefs.HasKey(Constants.LastSaveTimeKey)) return TimeSpan.Zero;

            string lastSaveTimeString = PlayerPrefs.GetString(Constants.LastSaveTimeKey);
            DateTime lastSaveTime = DateTime.Parse(lastSaveTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return DateTime.UtcNow - lastSaveTime;
        }

        #endregion

        #region Factory Save-Load Methods

        public void SaveFactory(string factoryID, int productAmount, int maxCapacity, float remainingTime, Queue<int> productionQueue)
        {
            if (!_factorySaveData.ContainsKey(factoryID))
            {
                _factorySaveData[factoryID] = new FactorySaveData { factoryID = factoryID };
            }

            _factorySaveData[factoryID].currentProductAmount = productAmount;
            _factorySaveData[factoryID].maxCapacity = maxCapacity;
            _factorySaveData[factoryID].remainingTime = remainingTime;
            _factorySaveData[factoryID].productionQueue = new List<int>(productionQueue);

            Debug.Log($"Saving Factory: {factoryID} | Amount: {productAmount} | Max: {maxCapacity} | Time: {remainingTime} | Queue: {string.Join(", ", productionQueue)}");

            FactorySaveDataWrapper wrapper = new FactorySaveDataWrapper { factories = new List<FactorySaveData>(_factorySaveData.Values) };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(Constants.FactorySaveDataKey, json);
            PlayerPrefs.SetString("LastSaveTime", DateTime.UtcNow.ToString("o"));
            PlayerPrefs.Save();
        }

        public List<FactorySaveData> LoadFactories()
        {
            if (!PlayerPrefs.HasKey(Constants.FactorySaveDataKey)) return new List<FactorySaveData>();

            string json = PlayerPrefs.GetString(Constants.FactorySaveDataKey);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Factory Save JSON is empty!");
                return new List<FactorySaveData>();
            }

            // **Wrapper Kullanarak Deserialize**
            FactorySaveDataWrapper wrapper = JsonUtility.FromJson<FactorySaveDataWrapper>(json);
            if (wrapper == null || wrapper.factories == null)
            {
                Debug.LogError("Factory Save JSON is corrupted or invalid!");
                return new List<FactorySaveData>();
            }

            var elapsedTime = GetElapsedTimeSinceLastSave();

            foreach (var factory in wrapper.factories)
            {
                Debug.Log($"Elapsed Time in seconds: {elapsedTime.TotalSeconds}, Updated Remaining Time: {factory.remainingTime}");
                factory.remainingTime -= Mathf.Max(0, (float)elapsedTime.TotalSeconds);

                /*if (factory.remainingTime <= 0)
                {
                    factory.currentProductAmount += 1;
                    factory.remainingTime = 0;
                }*/
            }

            return wrapper.factories;
        }

        #endregion
    }
}