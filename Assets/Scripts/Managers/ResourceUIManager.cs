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
            foreach (var resourceUI in resourceUIList)
            {
                _resourceManager.UpdateResource(resourceUI.GetResourceType(), 0);
            }
        }

        private void OnEnable()
        {
            foreach (var resourceUI in resourceUIList)
            {
                _resourceManager.OnResourceUpdated += resourceUI.HandleResourceUpdated;
            }
        }

        private void OnDisable()
        {
            foreach (var resourceUI in resourceUIList)
            {
                _resourceManager.OnResourceUpdated -= resourceUI.HandleResourceUpdated;
            }
        }

        #endregion
    }
}