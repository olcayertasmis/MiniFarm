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
        private BaseFactory _currentOpenFactory;

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
            if (Input.GetMouseButtonDown(0) && _currentOpenFactory)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == _currentOpenFactory.gameObject) return;
                }

                CloseCurrentPanel();
            }
        }

        #endregion

        private void HandleFactoryClicked(BaseFactory factory)
        {
            if (factory is null) return;

            CloseCurrentPanel();

            _currentOpenFactory = factory;
            factory.CollectProduct();

            if (factory is not AdvancedFactory advancedFactory) return;

            advancedFactory.SetProductionPanelActive(true);
        }

        private void CloseCurrentPanel()
        {
            if (!_currentOpenFactory || _currentOpenFactory is not AdvancedFactory advancedFactory) return;

            advancedFactory.SetProductionPanelActive(false);

            _currentOpenFactory = null;
        }
    }
}