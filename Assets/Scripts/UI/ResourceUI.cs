using TMPro;
using UnityEngine;

namespace MiniFarm.UI
{
    public class ResourceUI : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI resourceText;

        #endregion

        #region Public Methods

        public void UpdateResourceUI(int amount)
        {
            resourceText.text = amount.ToString();
        }

        #endregion
    }
}