using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeTextBasedOnInputType : MonoBehaviour
{
    [Header("Input System")]
    public InputActionAsset inputActions; // Drag your InputActionAsset here in the Inspector

    [Header("UI")]
    public TextMeshProUGUI textDisplay; // Reference to a TextMeshProUGUI element in your UI

    // Strings for each sensor type
    private string _accelerometerString = "";
    private string _gyroscopeString = "";
    private string _gravityString = "";
    private string _attitudeString = "";
    private string _linearAccelerationString = "";
    private string _magnetometerString = "";

    private string _accelerometerMinMaxString = "";
    private string _gyroscopeMinMaxString = "";
    private string _gravityMinMaxString = "";
    private string _linearAccelerationMinMaxString = "";
    private string _magnetometerMinMaxString = "";

#if UNITY_ANDROID
    private string _lightSensorString = "";
    private string _pressureSensorString = "";
    private string _proximitySensorString = "";
    private string _humiditySensorString = "";
    private string _ambientTemperatureString = "";
    private string _stepCounterString = "";
#endif

    private string _inputActionString = ""; // String for input action data

    // Min/Max data storage
    private Vector3 _accelerometerMin = Vector3.positiveInfinity;
    private Vector3 _accelerometerMax = Vector3.negativeInfinity;

    private Vector3 _gyroscopeMin = Vector3.positiveInfinity;
    private Vector3 _gyroscopeMax = Vector3.negativeInfinity;

    private Vector3 _gravityMin = Vector3.positiveInfinity;
    private Vector3 _gravityMax = Vector3.negativeInfinity;

    private Vector3 _linearAccelerationMin = Vector3.positiveInfinity;
    private Vector3 _linearAccelerationMax = Vector3.negativeInfinity;

    private Vector3 _magnetometerMin = Vector3.positiveInfinity;
    private Vector3 _magnetometerMax = Vector3.negativeInfinity;

#if UNITY_ANDROID
    private float _lightSensorMin = Mathf.Infinity;
    private float _lightSensorMax = -Mathf.Infinity;

    private float _pressureSensorMin = Mathf.Infinity;
    private float _pressureSensorMax = -Mathf.Infinity;

    private float _proximitySensorMin = Mathf.Infinity;
    private float _proximitySensorMax = -Mathf.Infinity;

    private float _humiditySensorMin = Mathf.Infinity;
    private float _humiditySensorMax = -Mathf.Infinity;

    private float _ambientTemperatureMin = Mathf.Infinity;
    private float _ambientTemperatureMax = -Mathf.Infinity;

    private int _stepCounterMin = int.MaxValue;
    private int _stepCounterMax = int.MinValue;
#endif

    private void OnEnable()
    {
        if (inputActions == null)
        {
            Debug.LogError("Please assign an InputActionAsset.");
            return;
        }

        // Subscribe to input actions
        foreach (var actionMap in inputActions.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                action.performed += OnInputActionPerformed;
                action.Enable();
            }
        }

        // Subscribe to sensor events
        SensorManager.OnAccelerometerData += UpdateAccelerometerData;
        SensorManager.OnGyroscopeData += UpdateGyroscopeData;
        SensorManager.OnGravityData += UpdateGravityData;
        SensorManager.OnAttitudeData += UpdateAttitudeData;
        SensorManager.OnLinearAccelerationData += UpdateLinearAccelerationData;
        SensorManager.OnMagnetometerData += UpdateMagnetometerData;

#if UNITY_ANDROID
        SensorManager.OnLightSensorData += UpdateLightSensorData;
        SensorManager.OnPressureSensorData += UpdatePressureSensorData;
        SensorManager.OnProximitySensorData += UpdateProximitySensorData;
        SensorManager.OnHumiditySensorData += UpdateHumiditySensorData;
        SensorManager.OnAmbientTemperatureData += UpdateAmbientTemperatureData;
        SensorManager.OnStepCounterData += UpdateStepCounterData;
#endif
    }

    

    private void OnDisable()
    {
        if (inputActions != null)
        {
            foreach (var actionMap in inputActions.actionMaps)
            {
                foreach (var action in actionMap.actions)
                {
                    action.performed -= OnInputActionPerformed;
                    action.Disable();
                }
            }
        }

        // Unsubscribe from sensor events
        SensorManager.OnAccelerometerData -= UpdateAccelerometerData;
        SensorManager.OnGyroscopeData -= UpdateGyroscopeData;
        SensorManager.OnGravityData -= UpdateGravityData;
        SensorManager.OnAttitudeData -= UpdateAttitudeData;
        SensorManager.OnLinearAccelerationData -= UpdateLinearAccelerationData;
        SensorManager.OnMagnetometerData -= UpdateMagnetometerData;

#if UNITY_ANDROID
        SensorManager.OnLightSensorData -= UpdateLightSensorData;
        SensorManager.OnPressureSensorData -= UpdatePressureSensorData;
        SensorManager.OnProximitySensorData -= UpdateProximitySensorData;
        SensorManager.OnHumiditySensorData -= UpdateHumiditySensorData;
        SensorManager.OnAmbientTemperatureData -= UpdateAmbientTemperatureData;
        SensorManager.OnStepCounterData -= UpdateStepCounterData;
#endif
    }

    private void Update()
    {
        // Merge all sensor data and input action data into a single string
        string mergedData = $"{_accelerometerString}\n" +
                            $"{_gyroscopeString}\n" +
                            $"{_gravityString}\n" +
                            $"{_attitudeString}\n" +
                            $"{_linearAccelerationString}\n" +
                            $"{_magnetometerString}\n" +
                            $"{_accelerometerMinMaxString}\n" +
                            $"{_gyroscopeMinMaxString}\n" +
                            $"{_gravityMinMaxString}\n" +
                            $"{_linearAccelerationMinMaxString}\n" +
                            $"{_magnetometerMinMaxString}\n";

#if UNITY_ANDROID
        mergedData += $"{_lightSensorString}\n" +
                      $"{_pressureSensorString}\n" +
                      $"{_proximitySensorString}\n" +
                      $"{_humiditySensorString}\n" +
                      $"{_ambientTemperatureString}\n" +
                      $"{_stepCounterString}\n" +
                      $"Light Sensor Min/Max: {_lightSensorMin} / {_lightSensorMax}\n" +
                      $"Pressure Sensor Min/Max: {_pressureSensorMin} / {_pressureSensorMax}\n" +
                      $"Proximity Sensor Min/Max: {_proximitySensorMin} / {_proximitySensorMax}\n" +
                      $"Humidity Sensor Min/Max: {_humiditySensorMin} / {_humiditySensorMax}\n" +
                      $"Ambient Temperature Min/Max: {_ambientTemperatureMin} / {_ambientTemperatureMax}\n" +
                      $"Step Counter Min/Max: {_stepCounterMin} / {_stepCounterMax}\n";
#endif

        mergedData += $"{_inputActionString}";

        if (textDisplay != null)
        {
            textDisplay.text = mergedData;
        }
    }

    private void OnInputActionPerformed(InputAction.CallbackContext context)
    {
        string actionName = context.action.name;
        string inputValue = GetInputValue(context);

        _inputActionString = $"Action: {actionName}, Value: {inputValue}";
    }

    private string GetInputValue(InputAction.CallbackContext context)
    {
        if (context.valueType == typeof(Vector2))
            return context.ReadValue<Vector2>().ToString();
        if (context.valueType == typeof(float))
            return context.ReadValue<float>().ToString();
        if (context.valueType == typeof(int))
            return context.ReadValue<int>().ToString();
        if (context.valueType == typeof(Vector3))
            return context.ReadValue<Vector3>().ToString();

        return "Unknown Value";
    }

    #region Sensor Data Handlers

    private void UpdateAccelerometerData(Vector3 data)
    {
        Debug.Log(data.magnitude);
        _accelerometerString = $"Accelerometer: {data}";
        UpdateMinMax(ref _accelerometerMin, ref _accelerometerMax, data);
        _accelerometerMinMaxString = $"Accel Min/Max: {_accelerometerMin} / {_accelerometerMax}";
    }

    private void UpdateGyroscopeData(Vector3 data)
    {
        _gyroscopeString = $"Gyroscope: {data}";
        UpdateMinMax(ref _gyroscopeMin, ref _gyroscopeMax, data);
        _gyroscopeMinMaxString = $"Gyro Min/Max: {_gyroscopeMin} / {_gyroscopeMax}";
    }

    private void UpdateGravityData(Vector3 data)
    {
        _gravityString = $"Gravity: {data}";
        UpdateMinMax(ref _gravityMin, ref _gravityMax, data);
        _gravityMinMaxString = $"Gravity Min/Max: {_gravityMin} / {_gravityMax}";
    }

    private void UpdateLinearAccelerationData(Vector3 data)
    {
        _linearAccelerationString = $"Linear Acceleration: {data}";
        UpdateMinMax(ref _linearAccelerationMin, ref _linearAccelerationMax, data);
        _linearAccelerationMinMaxString = $"Linear Accel Min/Max: {_linearAccelerationMin} / {_linearAccelerationMax}";
    }

    private void UpdateMagnetometerData(Vector3 data)
    {
        _magnetometerString = $"Magnetometer: {data}";
        UpdateMinMax(ref _magnetometerMin, ref _magnetometerMax, data);
        _magnetometerMinMaxString = $"Magnetometer Min/Max: {_magnetometerMin} / {_magnetometerMax}";
    }
    
    private void UpdateAttitudeData(Quaternion data)
    {
        _attitudeString = $"Altitude: {data}";
    }

    private void UpdateMinMax(ref Vector3 min, ref Vector3 max, Vector3 newData)
    {
        min = Vector3.Min(min, newData);
        max = Vector3.Max(max, newData);
    }

#if UNITY_ANDROID
    private void UpdateLightSensorData(float data)
    {
        _lightSensorString = $"Light Sensor: {data}";
        _lightSensorMin = Mathf.Min(_lightSensorMin, data);
        _lightSensorMax = Mathf.Max(_lightSensorMax, data);
    }

    private void UpdatePressureSensorData(float data)
    {
        _pressureSensorString = $"Pressure: {data}";
        _pressureSensorMin = Mathf.Min(_pressureSensorMin, data);
        _pressureSensorMax = Mathf.Max(_pressureSensorMax, data);
    }

    private void UpdateProximitySensorData(float data)
    {
        _proximitySensorString = $"Proximity: {data}";
        _proximitySensorMin = Mathf.Min(_proximitySensorMin, data);
        _proximitySensorMax = Mathf.Max(_proximitySensorMax, data);
    }

    private void UpdateHumiditySensorData(float data)
    {
        _humiditySensorString = $"Humidity: {data}";
        _humiditySensorMin = Mathf.Min(_humiditySensorMin, data);
        _humiditySensorMax = Mathf.Max(_humiditySensorMax, data);
    }

    private void UpdateAmbientTemperatureData(float data)
    {
        _ambientTemperatureString = $"Ambient Temperature: {data}";
        _ambientTemperatureMin = Mathf.Min(_ambientTemperatureMin, data);
        _ambientTemperatureMax = Mathf.Max(_ambientTemperatureMax, data);
    }

    private void UpdateStepCounterData(int data)
    {
        _stepCounterString = $"Step Counter: {data}";
        _stepCounterMin = Mathf.Min(_stepCounterMin, data);
        _stepCounterMax = Mathf.Max(_stepCounterMax, data);
    }
#endif

    #endregion
}