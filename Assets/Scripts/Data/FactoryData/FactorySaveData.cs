using System;
using System.Collections.Generic;

namespace MiniFarm.Data.FactoryData
{
    [Serializable]
    public class FactorySaveDataWrapper
    {
        public List<FactorySaveData> factories;
    }

    [Serializable]
    public class FactorySaveData
    {
        public string factoryID;
        public int currentProductAmount;
        public int maxCapacity;
        public float remainingTime;
        public List<int> productionQueue;
    }
}