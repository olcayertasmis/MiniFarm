using UnityEngine;

namespace MiniFarm.Gameplay.Factories
{
    public class FactoryClickEvent : MonoBehaviour
    {
        public delegate void FactoryClickedHandler(BaseFactory baseFactory);
        public static event FactoryClickedHandler OnFactoryClicked;

        private void OnMouseDown()
        {
            if (TryGetComponent(out BaseFactory baseFactory)) OnFactoryClicked?.Invoke(baseFactory);
        }
    }
}