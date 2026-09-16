using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class MotionEvent : MonoBehaviour
    {
        [Header("Linear Motion Detection Settings")]
        public UnityEvent OnMotionDetected;
        public float motionSensitivity = 1.0f; // Threshold for linear acceleration

        private void OnEnable()
        {
            SensorManager.OnLinearAccelerationData += HandleLinearAccelerationData;
        }

        private void OnDisable()
        {
            SensorManager.OnLinearAccelerationData -= HandleLinearAccelerationData;
        }

        private void HandleLinearAccelerationData(Vector3 linearAcceleration)
        {
            if (linearAcceleration.sqrMagnitude >= motionSensitivity * motionSensitivity)
            {
                OnMotionDetected?.Invoke();
            }
        }
    }
}