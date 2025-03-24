using System.Collections.Generic;
using MiniFarm.Gameplay.Factories;
using UnityEngine;
using Zenject;

namespace MiniFarm.Managers
{
    public class FactorySpawnManager : MonoBehaviour
    {
        #region Variables

        [Header("Spawn Settings")]
        [SerializeField] private List<FactorySpawnPoint> spawnPoints = new();
        [SerializeField] private List<GameObject> baseFactoryPrefabs = new();

        private readonly List<BaseFactory> _spawnedFactories = new();

        [Inject] private DiContainer _container;

        #endregion

        #region Helpers

        public List<BaseFactory> GetSpawnedFactories() => _spawnedFactories;

        #endregion

        #region Unity Methods

        private void Start()
        {
            SpawnFactories();
        }

        #endregion

        #region Spawn Methods

        private void SpawnFactories()
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (i < baseFactoryPrefabs.Count && spawnPoints[i].IsAvailable)
                {
                    var factoryPrefab = baseFactoryPrefabs[i];
                    var spawnedFactory = _container.InstantiatePrefab(factoryPrefab, spawnPoints[i].transform.position, Quaternion.identity, spawnPoints[i].transform);

                    spawnPoints[i].AssignFactory(spawnedFactory);

                    var baseFactory = spawnedFactory.GetComponent<BaseFactory>();
                    _spawnedFactories.Add(baseFactory);
                }
            }
        }

        #endregion

        #region Public Methods

        public void RespawnFactories()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.ClearFactory();
            }

            _spawnedFactories.Clear();
            SpawnFactories();
        }

        #endregion
    }
}