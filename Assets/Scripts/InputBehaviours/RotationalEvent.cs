using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class RotationalEvent : MonoBehaviour
    {
        [Header("Rotation Detection Settings")]
        public UnityEvent OnRotationDetected;
        public float rotationThreshold = 1.0f; // Threshold for angular velocity

        private void OnEnable()
        {
            SensorManager.OnGyroscopeData += HandleGyroscopeData;
        }

        private void OnDisable()
        {
            SensorManager.OnGyroscopeData -= HandleGyroscopeData;
        }

        private void HandleGyroscopeData(Vector3 angularVelocity)
        {
            if (angularVelocity.sqrMagnitude >= rotationThreshold * rotationThreshold)
            {
                OnRotationDetected?.Invoke();
            }
        }
    }
}