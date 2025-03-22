using MiniFarm.Gameplay.Factories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniFarm.Managers
{
    public class FactoryManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;

        [Header("Data")]
        private AdvancedFactory _currentOpenFactory;

        #region Unity Methods

        private void OnEnable()
        {
            FactoryClickEvent.OnFactoryClicked += HandleFactoryClicked;
        }

        private void OnDisable()
        {
            FactoryClickEvent.OnFactoryClicked -= HandleFactoryClicked;
        }

        private void Update()
        {
            if (_currentOpenFactory && Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == _currentOpenFactory.gameObject)
                    {
                        _currentOpenFactory.CollectProduct();
                        return;
                    }
                }

                CloseCurrentPanel();
            }
        }

        #endregion

        private void HandleFactoryClicked(AdvancedFactory factory)
        {
            if (_currentOpenFactory == factory) return;

            CloseCurrentPanel();
            _currentOpenFactory = factory;
            _currentOpenFactory.SetProductionPanelActive(true);
        }

        private void CloseCurrentPanel()
        {
            if (!_currentOpenFactory) return;

            _currentOpenFactory.SetProductionPanelActive(false);
            _currentOpenFactory = null;
        }
    }
}