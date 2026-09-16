using UnityEngine;
using UnityEngine.Events;

namespace InputBehaviours
{
    public class GravityEvent : MonoBehaviour
    {
        [Header("Gravity Change Detection Settings")]
        public UnityEvent OnGravityChanged;
        public float gravityChangeThreshold = 0.1f; // Threshold for gravity change detection

        private Vector3 lastGravity;

        private void OnEnable()
        {
            SensorManager.OnGravityData += HandleGravityData;
        }

        private void OnDisable()
        {
            SensorManager.OnGravityData -= HandleGravityData;
        }

        private void HandleGravityData(Vector3 gravity)
        {
            if ((gravity - lastGravity).sqrMagnitude >= gravityChangeThreshold * gravityChangeThreshold)
            {
                OnGravityChanged?.Invoke();
            }
            lastGravity = gravity;
        }
    }
}