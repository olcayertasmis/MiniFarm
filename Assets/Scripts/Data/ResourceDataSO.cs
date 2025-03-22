using MiniFarm.Data.EnumData;
using UnityEngine;

namespace MiniFarm.Data
{
    [CreateAssetMenu(fileName = "ResourceDataSO", menuName = "Data/ResourceData")]
    public class ResourceDataSO : ScriptableObject
    {
        #region Data

        [Header("Data")]
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private Sprite resourceIcon;

        #endregion

        #region Helpers

        public ResourceType GetResourceType() => resourceType;
        public Sprite GetResourceIcon() => resourceIcon;

        #endregion
    }
}