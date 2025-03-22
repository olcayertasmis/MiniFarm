using UnityEngine;

namespace MiniFarm.Gameplay.Factories
{
    public class FactoryClickEvent : MonoBehaviour
    {
        public delegate void FactoryClickedHandler(AdvancedFactory advancedFactory);

        public static event FactoryClickedHandler OnFactoryClicked;

        private void OnMouseDown()
        {
            if (TryGetComponent(out AdvancedFactory advancedFactory)) OnFactoryClicked?.Invoke(advancedFactory);
        }
    }
}