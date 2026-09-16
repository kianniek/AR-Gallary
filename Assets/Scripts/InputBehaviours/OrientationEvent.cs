using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class OrientationEvent : MonoBehaviour
    {
        [Header("Orientation Change Detection Settings")]
        public UnityEvent OnOrientationChanged;
        public float orientationThreshold = 0.1f; // Quaternion change threshold

        private Quaternion lastOrientation;

        private void OnEnable()
        {
            SensorManager.OnAttitudeData += HandleAttitudeData;
        }

        private void OnDisable()
        {
            SensorManager.OnAttitudeData -= HandleAttitudeData;
        }

        private void HandleAttitudeData(Quaternion orientation)
        {
            if (Quaternion.Angle(lastOrientation, orientation) >= orientationThreshold)
            {
                OnOrientationChanged?.Invoke();
            }
            lastOrientation = orientation;
        }
    }
}