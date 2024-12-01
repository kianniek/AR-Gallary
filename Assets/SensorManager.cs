using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class SensorManager : MonoBehaviour
{
    // Singleton instance
    public static SensorManager Instance { get; private set; }

    // Define events for sensor data
    public static event Action<Vector3> OnAccelerometerData;
    public static event Action<Vector3> OnGyroscopeData;
    public static event Action<Vector3> OnGravityData;
    public static event Action<Quaternion> OnAttitudeData;
    public static event Action<Vector3> OnLinearAccelerationData;
    public static event Action<Vector3> OnMagnetometerData;

#if UNITY_ANDROID
    public static event Action<float> OnLightSensorData;
    public static event Action<float> OnPressureSensorData;
    public static event Action<float> OnProximitySensorData;
    public static event Action<float> OnHumiditySensorData;
    public static event Action<float> OnAmbientTemperatureData;
    public static event Action<int> OnStepCounterData;
#endif

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this instance persistent across scenes
    }

    void Start()
    {
        // Enable shared sensors

        foreach (var inputDevice in InputSystem.devices)
        {
            Debug.Log(inputDevice);
        }

        
        
        InputSystem.EnableDevice(Accelerometer.current);
        InputSystem.EnableDevice(Gyroscope.current);
        InputSystem.EnableDevice(GravitySensor.current);
        InputSystem.EnableDevice(AttitudeSensor.current);
        InputSystem.EnableDevice(LinearAccelerationSensor.current);
        InputSystem.EnableDevice(MagneticFieldSensor.current);

#if UNITY_ANDROID
        // Enable Android-specific sensors
        InputSystem.EnableDevice(LightSensor.current);
        InputSystem.EnableDevice(PressureSensor.current);
        InputSystem.EnableDevice(ProximitySensor.current);
        InputSystem.EnableDevice(HumiditySensor.current);
        InputSystem.EnableDevice(AmbientTemperatureSensor.current);
        InputSystem.EnableDevice(StepCounter.current);
#endif

        Debug.Log("Sensors enabled.");
    }

    void Update()
    {
        // Shared Sensors
        if (Accelerometer.current != null)
        {
            Vector3 accelerometerData = Accelerometer.current.acceleration.ReadValue();
            OnAccelerometerData?.Invoke(accelerometerData);
        }

        if (Gyroscope.current != null)
        {
            Vector3 gyroscopeData = Gyroscope.current.angularVelocity.ReadValue();
            OnGyroscopeData?.Invoke(gyroscopeData);
        }

        if (GravitySensor.current != null)
        {
            Vector3 gravityData = GravitySensor.current.gravity.ReadValue();
            OnGravityData?.Invoke(gravityData);
        }

        if (AttitudeSensor.current != null)
        {
            Quaternion attitudeData = AttitudeSensor.current.attitude.ReadValue();
            OnAttitudeData?.Invoke(attitudeData);
        }

        if (LinearAccelerationSensor.current != null)
        {
            Vector3 linearAccelerationData = LinearAccelerationSensor.current.acceleration.ReadValue();
            OnLinearAccelerationData?.Invoke(linearAccelerationData);
        }

        if (MagneticFieldSensor.current != null)
        {
            Vector3 magnetometerData = MagneticFieldSensor.current.magneticField.ReadValue();
            OnMagnetometerData?.Invoke(magnetometerData);
        }

#if UNITY_ANDROID
        // Android-Only Sensors
        if (LightSensor.current != null)
        {
            float lightSensorData = LightSensor.current.lightLevel.ReadValue();
            OnLightSensorData?.Invoke(lightSensorData);
        }

        if (PressureSensor.current != null)
        {
            float pressureSensorData = PressureSensor.current.atmosphericPressure.ReadValue();
            OnPressureSensorData?.Invoke(pressureSensorData);
        }

        if (ProximitySensor.current != null)
        {
            float proximitySensorData = ProximitySensor.current.distance.ReadValue();
            OnProximitySensorData?.Invoke(proximitySensorData);
        }

        if (HumiditySensor.current != null)
        {
            float humiditySensorData = HumiditySensor.current.relativeHumidity.ReadValue();
            OnHumiditySensorData?.Invoke(humiditySensorData);
        }

        if (AmbientTemperatureSensor.current != null)
        {
            float ambientTemperatureData = AmbientTemperatureSensor.current.ambientTemperature.ReadValue();
            OnAmbientTemperatureData?.Invoke(ambientTemperatureData);
        }

        if (StepCounter.current != null)
        {
            int stepCounterData = StepCounter.current.stepCounter.ReadValue();
            OnStepCounterData?.Invoke(stepCounterData);
        }
#endif
    }
}
