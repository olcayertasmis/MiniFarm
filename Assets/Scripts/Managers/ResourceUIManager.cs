using System;
using MiniFarm.Gameplay.Resources;
using MiniFarm.UI;
using UnityEngine;
using Zenject;

namespace MiniFarm.Managers
{
    public class ResourceUIManager : MonoBehaviour
    {
        #region Variables

        [Header("Resources UI")]
        [SerializeField] private ResourceUI hayUI;
        [SerializeField] private ResourceUI flourUI;
        [SerializeField] private ResourceUI breadUI;

        [Header("References")]
        private ResourceManager _resourceManager;

        #endregion

        [Inject]
        public void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        #region Unity Methods

        private void Start()
        {
            _resourceManager.LoadResources();
        }

        private void OnEnable()
        {
            _resourceManager.OnHayResourceUpdated += hayUI.UpdateResourceUI;
            _resourceManager.OnFlourResourceUpdated += flourUI.UpdateResourceUI;
            _resourceManager.OnBreadResourceUpdated += breadUI.UpdateResourceUI;
        }

        private void OnDisable()
        {
            _resourceManager.OnHayResourceUpdated -= hayUI.UpdateResourceUI;
            _resourceManager.OnFlourResourceUpdated -= flourUI.UpdateResourceUI;
            _resourceManager.OnBreadResourceUpdated -= breadUI.UpdateResourceUI;
        }

        #endregion
    }
}