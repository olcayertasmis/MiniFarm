using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm.UI
{
    public class UISliderController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI currentAmountText;
        [SerializeField] private TextMeshProUGUI remainingTimeText;
        [SerializeField] private Image productIcon;
        [SerializeField] private Slider progressSlider;

        private int _maxCapacity;

        #endregion

        #region UI Methods

        public void SetSlider(Sprite icon, int currentAmount, float remainingTime, float maxTime, int maxCapacity)
        {
            productIcon.sprite = icon;
            progressSlider.maxValue = maxTime;
            progressSlider.value = remainingTime;
            _maxCapacity = maxCapacity;

            UpdateSlider(currentAmount, remainingTime);
        }

        public void UpdateSlider(int currentAmount, float remainingTime)
        {
            currentAmountText.text = currentAmount.ToString();

            if (currentAmount >= _maxCapacity) remainingTimeText.text = "FULL";
            else
            {
                int remainingTimeInt = Mathf.CeilToInt(remainingTime);
                remainingTimeText.text = $"{remainingTimeInt}s";
            }

            progressSlider.value = progressSlider.maxValue - remainingTime;
        }

        #endregion
    }
}