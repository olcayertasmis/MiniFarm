using System;
using UnityEngine;
using Zenject;

namespace MiniFarm.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        private SaveManager _saveManager;

        #endregion

        #region Constructors

        [Inject]
        public void Construct(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        #endregion

        #region Unity Methods

        private async void Awake()
        {
            try
            {
                await _saveManager.LoadGame();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async void OnApplicationQuit()
        {
            try
            {
                await _saveManager.SaveGame();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #endregion
    }
}