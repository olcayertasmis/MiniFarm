using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm.UI
{
    public class ProduceButton : MonoBehaviour
    {
        [SerializeField] private Button produceButton;
        [SerializeField] private Image requiredResourceImage;
        [SerializeField] private TextMeshProUGUI requiredResourceAmountText;

        public void SetProduceButton(Sprite requiredResourceIcon, int requiredResourceAmount, Action onClickAction)
        {
            requiredResourceImage.sprite = requiredResourceIcon;
            requiredResourceAmountText.text = "x" + requiredResourceAmount;
            produceButton.onClick.AddListener(() => onClickAction?.Invoke());
        }

        public void SetInteractable(bool isInteractable) => produceButton.interactable = isInteractable;
    }
}