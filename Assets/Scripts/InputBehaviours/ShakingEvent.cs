using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class ShakingEvent : MonoBehaviour
    {
        [Header("Shake Detection Settings")]
        public UnityEvent OnShakeDetected; // Event triggered on shake detection
        public float shakeDetectionSensitivity = 2.0f; // Sensitivity for detecting shakes
        public float shakeCooldownTime = 0.5f; // Cooldown time between shake events

        private float lastShakeTime; // Tracks the last shake detection time

        private void OnEnable()
        {
            // Subscribe to accelerometer data events
            SensorManager.OnAccelerometerData += HandleAccelerometerData;
        }

        private void OnDisable()
        {
            // Unsubscribe from accelerometer data events
            SensorManager.OnAccelerometerData -= HandleAccelerometerData;
        }

        private void HandleAccelerometerData(Vector3 acceleration)
        {
            // Check for shake condition
            if (acceleration.sqrMagnitude >= shakeDetectionSensitivity * shakeDetectionSensitivity)
            {
                if (Time.time - lastShakeTime > shakeCooldownTime)
                {
                    lastShakeTime = Time.time; // Update last shake time
                    OnShakeDetected?.Invoke(); // Trigger the Unity event
                }
            }
        }
    }
}