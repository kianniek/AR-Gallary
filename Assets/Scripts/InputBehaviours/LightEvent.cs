using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class LightEvent : MonoBehaviour
    {
#if UNITY_ANDROID
        [Header("Light Level Change Detection Settings")]
        public UnityEvent OnLightLevelChanged;
        public float lightLevelThreshold = 10.0f;

        private void OnEnable()
        {
            SensorManager.OnLightSensorData += HandleLightSensorData;
        }

        private void OnDisable()
        {
            SensorManager.OnLightSensorData -= HandleLightSensorData;
        }

        private void HandleLightSensorData(float lightLevel)
        {
            if (lightLevel >= lightLevelThreshold)
            {
                OnLightLevelChanged?.Invoke();
            }
        }
#endif
    }
}