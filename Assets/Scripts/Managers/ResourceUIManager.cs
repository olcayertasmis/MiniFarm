using System.Collections.Generic;
using MiniFarm.UI;
using UnityEngine;
using Zenject;

namespace MiniFarm.Managers
{
    public class ResourceUIManager : MonoBehaviour
    {
        #region Variables

        [Header("Resources UI")]
        [SerializeField] private List<ResourceUI> resourceUIList = new();
        /*[SerializeField] private ResourceUI hayUI;
        [SerializeField] private ResourceUI flourUI;
        [SerializeField] private ResourceUI breadUI;*/

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
            foreach (var resourceUI in resourceUIList)
            {
                _resourceManager.OnResourceUpdated += resourceUI.HandleResourceUpdated;
            }
            /*_resourceManager.OnResourceUpdated += hayUI.HandleResourceUpdated;
            _resourceManager.OnFlourResourceUpdated += flourUI.HandleResourceUpdated;
            _resourceManager.OnBreadResourceUpdated += breadUI.HandleResourceUpdated;*/
        }

        private void OnDisable()
        {
            foreach (var resourceUI in resourceUIList)
            {
                _resourceManager.OnResourceUpdated -= resourceUI.HandleResourceUpdated;
            }
            /*_resourceManager.OnHayResourceUpdated -= hayUI.UpdateResourceUI;
            _resourceManager.OnFlourResourceUpdated -= flourUI.UpdateResourceUI;
            _resourceManager.OnBreadResourceUpdated -= breadUI.UpdateResourceUI;*/
        }

        #endregion
    }
}