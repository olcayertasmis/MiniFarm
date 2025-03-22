using MiniFarm.Data.EnumData;
using UnityEngine;

namespace MiniFarm.Data.FactoryData
{
    public class FactoryDataSO : ScriptableObject
    {
        #region Data

        [Header("Default Factory Data")]
        [SerializeField] private string factoryName;
        [SerializeField] private int defaultCapacity;
        [SerializeField] private float productionTime;
        [SerializeField] private Sprite productIcon;
        [SerializeField] private ResourceType productType;

        #endregion

        #region Helpers

        public string GetFactoryName => factoryName;
        public int GetDefaultCapacity => defaultCapacity;
        public float GetProductionTime => productionTime;
        public Sprite GetProductIcon => productIcon;
        public ResourceType GetProductType => productType;

        #endregion
    }
}