using UnityEngine;

namespace MiniFarm.Gameplay.Factories
{
    public class FactorySpawnPoint : MonoBehaviour
    {
        #region Variables

        private GameObject _spawnedFactory;

        #endregion

        #region Properties

        public bool IsAvailable => !_spawnedFactory;

        #endregion

        #region Public Methods

        public void AssignFactory(GameObject factory)
        {
            if (!IsAvailable) return;

            _spawnedFactory = factory;
        }

        public void ClearFactory()
        {
            if (IsAvailable) return;

            Destroy(_spawnedFactory);
            _spawnedFactory = null;
        }

        #endregion
    }
}