using TMPro;
using UnityEngine;

namespace MiniFarm.UI
{
    public class AdvancedUISliderController : UISliderController
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI productionQueueText;

        #endregion
        
        #region UI Methods

        public void UpdateProductionQueue(int currentQueue, int maxCapacity)
        {
            productionQueueText.text = $"{currentQueue}/{maxCapacity}";
        }

        #endregion
    }
}