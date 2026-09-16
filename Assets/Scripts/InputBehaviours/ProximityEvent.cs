using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class ProximityEvent : MonoBehaviour
    {
#if UNITY_ANDROID
        [Header("Proximity Detection Settings")]
        public UnityEvent OnProximityDetected;
        public float proximityThreshold = 5.0f;

        private void OnEnable()
        {
            SensorManager.OnProximitySensorData += HandleProximitySensorData;
        }

        private void OnDisable()
        {
            SensorManager.OnProximitySensorData -= HandleProximitySensorData;
        }

        private void HandleProximitySensorData(float distance)
        {
            if (distance <= proximityThreshold)
            {
                OnProximityDetected?.Invoke();
            }
        }
#endif
    }
}