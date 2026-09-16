using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInputTester : MonoBehaviour
{
    private Vector3 initialTouchPosition;
    private Vector3 currentTouchPosition;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private Camera mainCamera;

    [SerializeField] private InputActionReference touchPositionAction;
    [SerializeField] private InputActionReference touchPressAction;
    [SerializeField] private InputActionReference pinchGapDeltaAction; // Pinch gap delta for zoom
    [SerializeField] private InputActionReference twistDeltaRotationAction;
    [SerializeField] private GameObject X;
    [SerializeField] private GameObject Y;
    [SerializeField] private GameObject Z;
    [SerializeField] private Renderer cubeRenderer;

    [Space] [Header("Which sensors to use")] [SerializeField]
    private bool useAccelerometer = true;

    [SerializeField] private bool useGyroscope = true;
    [SerializeField] private bool useLinearAcceleration = true;
    [SerializeField] private bool useLightSensor = true;
    [SerializeField] private bool useHumiditySensor = true;
    [SerializeField] private bool useAmbientTemperature = true;
    [SerializeField] private bool useStepCounter = true;

    private float initialPinchGapDelta;
    private float initialTwistDeltaRotation;
    [Space] [SerializeField] private GameObject dropDownMenu;
    private TMP_Dropdown dropdown;

    private Color initialColor;

    private Vector3 initialX;
    private Vector3 initialY;
    private Vector3 initialZ;

    private Vector2 initialPinchDistance;
    private float initialRotationAngle;

    private void OnEnable()
    {
        // Subscribe to sensor events from SensorManager
        SensorManager.OnAccelerometerData += HandleAccelerometerData;
        SensorManager.OnGyroscopeData += HandleGyroscopeData;
        SensorManager.OnLinearAccelerationData += HandleLinearAccelerationData;

        touchPositionAction.action.performed += ctx => HandleTouch(ctx.ReadValue<Vector2>());
        touchPressAction.action.canceled += ctx => HandleRelease(ctx.ReadValue<Vector2>());
        touchPressAction.action.performed += ctx => HandlePress(ctx.ReadValue<Vector2>());
        touchPressAction.action.canceled += ctx => HandleRelease(ctx.ReadValue<Vector2>());

        // Subscribe to pinch and rotate gestures
        pinchGapDeltaAction.action.performed += ctx => HandlePinchZoom(ctx.ReadValue<float>());
        twistDeltaRotationAction.action.performed += ctx => HandleRotation(ctx.ReadValue<float>());

#if UNITY_ANDROID
        SensorManager.OnLightSensorData += HandleLightSensorData;
        SensorManager.OnHumiditySensorData += HandleHumiditySensorData;
        SensorManager.OnAmbientTemperatureData += HandleAmbientTemperatureData;
        SensorManager.OnStepCounterData += HandleStepCounterData;
#endif
    }

    private void OnDisable()
    {
        // Unsubscribe from sensor events
        SensorManager.OnAccelerometerData -= HandleAccelerometerData;
        SensorManager.OnGyroscopeData -= HandleGyroscopeData;
        SensorManager.OnLinearAccelerationData -= HandleLinearAccelerationData;

        touchPositionAction.action.performed -= ctx => HandleTouch(ctx.ReadValue<Vector2>());
        touchPressAction.action.canceled -= ctx => HandleRelease(ctx.ReadValue<Vector2>());
        touchPressAction.action.performed -= ctx => HandlePress(ctx.ReadValue<Vector2>());
        touchPressAction.action.canceled -= ctx => HandleRelease(ctx.ReadValue<Vector2>());

        pinchGapDeltaAction.action.performed -= ctx => HandlePinchZoom(ctx.ReadValue<float>());
        twistDeltaRotationAction.action.performed -= ctx => HandleRotation(ctx.ReadValue<float>());

#if UNITY_ANDROID
        SensorManager.OnLightSensorData -= HandleLightSensorData;
        SensorManager.OnHumiditySensorData -= HandleHumiditySensorData;
        SensorManager.OnAmbientTemperatureData -= HandleAmbientTemperatureData;
        SensorManager.OnStepCounterData -= HandleStepCounterData;
#endif
    }

    private void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Store initial positions of X, Y, and Z
        initialX = X.transform.localPosition;
        initialY = Y.transform.localPosition;
        initialZ = Z.transform.localPosition;

        // Store initial color of the cube
        initialColor = cubeRenderer.material.color;

        // Store initial position of the gameObject
        initialPosition = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
        initialScale = gameObject.transform.localScale;

        // Initialize dropdown menu
        var options = new[]
        {
            "Accelerometer",
            "Gyroscope",
            "Linear Acceleration",
            "Light Sensor",
            "Ambient Temperature",
            "Step Counter"
        };

        // instantiate dropdown menu
        var canvas = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0];

        var dropDownMenuGameObject = Instantiate(dropDownMenu, canvas.transform);
        dropdown = dropDownMenuGameObject.GetComponent<TMP_Dropdown>();

        // Add options to dropdown menu
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(options));

        // Add listener to dropdown menu
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(dropdown); });

        OnDropdownValueChanged(dropdown);
    }

    private void OnDropdownValueChanged(TMP_Dropdown dropdown)
    {
        // Get the selected option
        var selectedOption = dropdown.options[dropdown.value].text;

        TurnOffAllSensors();
        ResetCube();

        // Enable or disable the selected sensor
        switch (selectedOption)
        {
            case "Accelerometer":
                useAccelerometer = true;
                break;
            case "Gyroscope":
                useGyroscope = true;
                break;
            case "Linear Acceleration":
                useLinearAcceleration = true;
                break;
            case "Light Sensor":
                useLightSensor = true;
                break;
            case "Humidity Sensor":
                useHumiditySensor = true;
                break;
            case "Ambient Temperature":
                useAmbientTemperature = true;
                break;
            case "Step Counter":
                useStepCounter = true;
                break;
        }
    }

    private void TurnOffAllSensors()
    {
        useAccelerometer = false;
        useGyroscope = false;
        useLinearAcceleration = false;
        useLightSensor = false;
        useHumiditySensor = false;
        useAmbientTemperature = false;
        useStepCounter = false;
    }

    private void ResetCube()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
        gameObject.transform.localScale = initialScale;
    }

    private void HandleTouch(Vector2 data)
    {
        if (initialTouchPosition == Vector3.zero)
        {
            initialTouchPosition = data;
        }

        currentTouchPosition = data;

        // Handle dragging with matrix multiplication
        if (currentTouchPosition != initialTouchPosition)
        {
            Vector3 direction = currentTouchPosition - initialTouchPosition;

            // Define the transformation matrix
            Matrix4x4 transformationMatrix = new Matrix4x4(
                new Vector4(1, 0, 0, 0), // Row 1
                new Vector4(0, 0, 1, 0), // Row 2
                new Vector4(0, 1, 0, 0), // Row 3
                new Vector4(0, 0, 0, 1) // Row 4 (homogeneous coordinate)
            );

            // Matrix multiplication
            var result = transformationMatrix * direction;

            // Convert the result to a Vector3
            direction = new Vector3(result.x, result.y, result.z);

            // Apply transformation to the object position
            gameObject.transform.position += direction * 0.001f;
            initialTouchPosition = currentTouchPosition;
        }
    }

    private void HandlePress(Vector2 data)
    {
        // Move the x, y, and z objects to the object center
        X.transform.localPosition = initialX - X.transform.right;
        Y.transform.localPosition = initialY + Y.transform.up;
        Z.transform.localPosition = initialZ + Z.transform.forward;
    }

    private void HandleRelease(Vector2 data)
    {
        ResetCube();

        // Reset the x, y, and z objects to their initial positions
        X.transform.localPosition = initialX;
        Y.transform.localPosition = initialY;
        Z.transform.localPosition = initialZ;
    }

    private void HandlePinchZoom(float pinchGapDelta)
    {
        Debug.Log("Pinch Gap Delta: " + pinchGapDelta);
        if (initialPinchGapDelta == 0f)
        {
            initialPinchGapDelta = pinchGapDelta; // Store the initial pinch gap for reference
        }

        // Calculate the zoom scale factor based on the pinch gap change
        float pinchFactor = pinchGapDelta - initialPinchGapDelta;

        // Apply scaling to the object
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + (pinchFactor*0.01f),
            gameObject.transform.localScale.y + (pinchFactor*0.01f),
            gameObject.transform.localScale.z + (pinchFactor*0.01f)); // Adjust the multiplier for more or less zoom 

        // Update the initial pinch gap for next frame comparison
        initialPinchGapDelta = pinchGapDelta;
    }

    private void HandleRotation(float twistDeltaRotation)
    {
        Debug.Log("Twist Delta Rotation: " + twistDeltaRotation);
        if (initialTwistDeltaRotation == 0f)
        {
            initialTwistDeltaRotation = twistDeltaRotation; // Store the initial twist delta for reference
        }

        // Rotate the object around the Y-axis
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,
            gameObject.transform.rotation.eulerAngles.y - twistDeltaRotation,
            0)); // Adjust the multiplier for more or less rotation

        // Update the initial twist delta for next frame comparison
        initialTwistDeltaRotation = twistDeltaRotation;
    }

    // Handle Accelerometer Data
    private void HandleAccelerometerData(Vector3 data)
    {
        if (!useAccelerometer) return;
        Z.transform.localPosition = new Vector3(initialZ.x, initialZ.y, data.z + initialZ.z);
    }

    // Handle Gyroscope Data
    private void HandleGyroscopeData(Vector3 data)
    {
        if (!useGyroscope) return;
        gameObject.transform.Rotate(data.x * Mathf.PI / 2, data.y * Mathf.PI / 2, data.z * Mathf.PI / 2);
    }

    // Handle Linear Acceleration Data
    private void HandleLinearAccelerationData(Vector3 data)
    {
        if (!useLinearAcceleration) return;
        X.transform.localPosition = new Vector3(initialX.x + data.x, initialX.y, initialX.z);
    }

#if UNITY_ANDROID
    // Handle Android Specific Sensors
    private void HandleLightSensorData(float data)
    {
        if (!useLightSensor) return;

        // Change the scale of the gameObject based on light intensity
        gameObject.transform.localScale = Vector3.one * (data * 0.05f);
    }

    private void HandleHumiditySensorData(float data)
    {
        if (!useHumiditySensor) return;
        // Example: Change gameObject's color based on humidity level
        //cubeRenderer.material.color = new Color(data / 100, cubeRenderer.material.color.g, cubeRenderer.material.color.b);
    }

    private void HandleAmbientTemperatureData(float data)
    {
        if (!useAmbientTemperature) return;
        // Scale the gameObject based on ambient temperature
        cubeRenderer.material.color = initialColor * (1 + data * 0.02f);
    }

    private void HandleStepCounterData(int data)
    {
        if (!useStepCounter) return;

        // Example: Move gameObject based on step count
        gameObject.transform.Translate(data * 0.05f, 0, 0);
    }
#endif
}