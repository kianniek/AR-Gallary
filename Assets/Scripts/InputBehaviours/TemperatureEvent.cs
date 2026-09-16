using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class TemperatureEvent : MonoBehaviour
    {
        #if UNITY_ANDROID
        [Header("Temperature Change Detection Settings")]
        public UnityEvent OnTemperatureChanged;
        public float temperatureThreshold = 30.0f;

        private void OnEnable()
        {
            SensorManager.OnAmbientTemperatureData += HandleTemperatureData;
        }

        private void OnDisable()
        {
            SensorManager.OnAmbientTemperatureData -= HandleTemperatureData;
        }

        private void HandleTemperatureData(float temperature)
        {
            if (temperature >= temperatureThreshold)
            {
                OnTemperatureChanged?.Invoke();
            }
        }
        #endif
    }
}