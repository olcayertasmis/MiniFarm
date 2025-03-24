using MiniFarm.Data.EnumData;
using TMPro;
using UnityEngine;

namespace MiniFarm.UI
{
    public class ResourceUI : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI resourceText;
        [SerializeField] private ResourceType resourceType;

        #endregion

        #region Helpers

        public ResourceType GetResourceType() => resourceType;

        #endregion

        #region Public Methods

        public void HandleResourceUpdated(ResourceType updatedResourceType, int amount)
        {
            if (updatedResourceType == resourceType)
            {
                UpdateResourceUI(amount);
            }
        }

        #endregion

        #region Private Methods

        private void UpdateResourceUI(int amount)
        {
            resourceText.text = amount.ToString();
        }

        #endregion
    }
}