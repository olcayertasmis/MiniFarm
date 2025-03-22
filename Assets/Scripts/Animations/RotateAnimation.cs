using DG.Tweening;
using UnityEngine;

namespace MiniFarm.Animations
{
    public class RotateAnimation : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector3 rotationAxis;

        private void Start()
        {
            StartRotation();
        }

        private void StartRotation()
        {
            transform.DORotate(rotationAxis * 360, rotationSpeed, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        private void OnDestroy() => transform.DOKill();
    }
}