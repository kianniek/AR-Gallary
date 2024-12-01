using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Vector3Event : UnityEvent<Vector3>
{
}

[Serializable]
public class QuaternionEvent : UnityEvent<Quaternion>
{
}

[Serializable]
public class FloatEvent : UnityEvent<float>
{
}

[Serializable]
public class IntEvent : UnityEvent<int>
{
}

public class SensorSubscriber : MonoBehaviour
{
    // UnityEvents for shared sensors
    public Vector3Event OnAccelerometerDataReceived;
    public Vector3Event OnGyroscopeDataReceived;
    public Vector3Event OnGravityDataReceived;
    public QuaternionEvent OnAttitudeDataReceived;
    public Vector3Event OnLinearAccelerationDataReceived;
    public Vector3Event OnMagnetometerDataReceived;

#if UNITY_ANDROID
    // UnityEvents for Android-specific sensors
    public FloatEvent OnLightSensorDataReceived;
    public FloatEvent OnPressureSensorDataReceived;
    public FloatEvent OnProximitySensorDataReceived;
    public FloatEvent OnHumiditySensorDataReceived;
    public FloatEvent OnAmbientTemperatureDataReceived;
    public IntEvent OnStepCounterDataReceived;
#endif

    private void OnEnable()
    {
        // Subscribe to SensorManager events
        SensorManager.OnAccelerometerData += HandleAccelerometerData;
        SensorManager.OnGyroscopeData += HandleGyroscopeData;
        SensorManager.OnGravityData += HandleGravityData;
        SensorManager.OnAttitudeData += HandleAttitudeData;
        SensorManager.OnLinearAccelerationData += HandleLinearAccelerationData;
        SensorManager.OnMagnetometerData += HandleMagnetometerData;

#if UNITY_ANDROID
        SensorManager.OnLightSensorData += HandleLightSensorData;
        SensorManager.OnPressureSensorData += HandlePressureSensorData;
        SensorManager.OnProximitySensorData += HandleProximitySensorData;
        SensorManager.OnHumiditySensorData += HandleHumiditySensorData;
        SensorManager.OnAmbientTemperatureData += HandleAmbientTemperatureData;
        SensorManager.OnStepCounterData += HandleStepCounterData;
#endif
    }

    private void OnDisable()
    {
        // Unsubscribe from SensorManager events
        SensorManager.OnAccelerometerData -= HandleAccelerometerData;
        SensorManager.OnGyroscopeData -= HandleGyroscopeData;
        SensorManager.OnGravityData -= HandleGravityData;
        SensorManager.OnAttitudeData -= HandleAttitudeData;
        SensorManager.OnLinearAccelerationData -= HandleLinearAccelerationData;
        SensorManager.OnMagnetometerData -= HandleMagnetometerData;

#if UNITY_ANDROID
        SensorManager.OnLightSensorData -= HandleLightSensorData;
        SensorManager.OnPressureSensorData -= HandlePressureSensorData;
        SensorManager.OnProximitySensorData -= HandleProximitySensorData;
        SensorManager.OnHumiditySensorData -= HandleHumiditySensorData;
        SensorManager.OnAmbientTemperatureData -= HandleAmbientTemperatureData;
        SensorManager.OnStepCounterData -= HandleStepCounterData;
#endif
    }

    // Handlers for shared sensors
    private void HandleAccelerometerData(Vector3 data)
    {
        OnAccelerometerDataReceived?.Invoke(data);
    }

    private void HandleGyroscopeData(Vector3 data)
    {
        OnGyroscopeDataReceived?.Invoke(data);
    }

    private void HandleGravityData(Vector3 data)
    {
        OnGravityDataReceived?.Invoke(data);
    }

    private void HandleAttitudeData(Quaternion data)
    {
        OnAttitudeDataReceived?.Invoke(data);
    }

    private void HandleLinearAccelerationData(Vector3 data)
    {
        OnLinearAccelerationDataReceived?.Invoke(data);
    }

    private void HandleMagnetometerData(Vector3 data)
    {
        OnMagnetometerDataReceived?.Invoke(data);
    }

#if UNITY_ANDROID
    // Handlers for Android-specific sensors
    private void HandleLightSensorData(float data)
    {
        OnLightSensorDataReceived?.Invoke(data);
    }

    private void HandlePressureSensorData(float data)
    {
        OnPressureSensorDataReceived?.Invoke(data);
    }

    private void HandleProximitySensorData(float data)
    {
        OnProximitySensorDataReceived?.Invoke(data);
    }

    private void HandleHumiditySensorData(float data)
    {
        OnHumiditySensorDataReceived?.Invoke(data);
    }

    private void HandleAmbientTemperatureData(float data)
    {
        OnAmbientTemperatureDataReceived?.Invoke(data);
    }

    private void HandleStepCounterData(int data)
    {
        OnStepCounterDataReceived?.Invoke(data);
    }
#endif
}