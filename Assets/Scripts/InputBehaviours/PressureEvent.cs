using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class PressureEvent : MonoBehaviour
    {
#if UNITY_ANDROID
        [Header("Pressure Change Detection Settings")]
        public UnityEvent OnPressureChanged;
        public float pressureThreshold = 1.0f;

        private void OnEnable()
        {
            SensorManager.OnPressureSensorData += HandlePressureSensorData;
        }

        private void OnDisable()
        {
            SensorManager.OnPressureSensorData -= HandlePressureSensorData;
        }

        private void HandlePressureSensorData(float pressure)
        {
            if (pressure >= pressureThreshold)
            {
                OnPressureChanged?.Invoke();
            }
        }
#endif
    }
}