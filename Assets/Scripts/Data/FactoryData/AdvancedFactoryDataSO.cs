using System;
using UnityEngine;

namespace MiniFarm.Data.FactoryData
{
    public class AdvancedFactoryDataSO : FactoryDataSO
    {
        #region Data

        [Header("Advanced Factory Data")]
        [SerializeField] private RequiredResource[] requiredResourceData;

        #endregion

        #region Helpers

        public RequiredResource[] GetRequiredResourceData => requiredResourceData;

        #endregion
    }

    [Serializable]
    public class RequiredResource
    {
        #region Data

        [Header("Required Resource Data")]
        [SerializeField] private ResourceDataSO getRequiredResourceData;
        [SerializeField] private int requiredAmount;

        #endregion

        #region Helpers

        public ResourceDataSO GetRequiredResourceData => getRequiredResourceData;
        public int RequiredAmount => requiredAmount;

        #endregion
    }
}