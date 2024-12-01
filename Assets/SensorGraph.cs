using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerationGraphUI : MonoBehaviour
{
    [Header("Graph Settings")]
    public int maxPoints = 100; // Maximum number of points on the graph
    public float graphScale = 1f; // Scale factor for graph height

    [Header("Graph Line Colors")]
    public Color xAxisColor = Color.red;   // Color for the X-axis
    public Color yAxisColor = Color.green; // Color for the Y-axis
    public Color zAxisColor = Color.blue;  // Color for the Z-axis

    private RectTransform graphRect;
    private LineRenderer xLineRenderer;
    private LineRenderer yLineRenderer;
    private LineRenderer zLineRenderer;

    private Queue<Vector3> accelerationData = new Queue<Vector3>();

    private void Start()
    {
        // Initialize graph RectTransform
        graphRect = GetComponent<RectTransform>();
        if (graphRect == null)
        {
            Debug.LogError("This script must be attached to a UI GameObject with a RectTransform.");
            return;
        }

        // Create LineRenderers
        xLineRenderer = CreateLineRenderer("X-Axis Line", xAxisColor);
        yLineRenderer = CreateLineRenderer("Y-Axis Line", yAxisColor);
        zLineRenderer = CreateLineRenderer("Z-Axis Line", zAxisColor);

        // Ensure SensorManager is active
        if (SensorManager.Instance == null)
        {
            Debug.LogError("SensorManager is not active in the scene.");
            return;
        }

        // Subscribe to acceleration data updates
        SensorManager.OnAccelerometerData += UpdateAccelerationData;
    }

    private void OnDestroy()
    {
        // Unsubscribe from SensorManager events
        if (SensorManager.Instance != null)
        {
            SensorManager.OnAccelerometerData -= UpdateAccelerationData;
        }
    }

    private LineRenderer CreateLineRenderer(string name, Color color)
    {
        // Create a new GameObject for the line
        GameObject lineObject = new GameObject(name);
        lineObject.transform.SetParent(transform, false);

        // Add LineRenderer and configure it
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 2f; // Line width
        lineRenderer.endWidth = 2f;
        lineRenderer.positionCount = 0;

        return lineRenderer;
    }

    private void UpdateAccelerationData(Vector3 data)
    {
        // Add new data to the queue
        accelerationData.Enqueue(data);

        // Ensure queue size does not exceed maxPoints
        if (accelerationData.Count > maxPoints)
        {
            accelerationData.Dequeue();
        }

        // Update the graph
        UpdateGraph(xLineRenderer, axisIndex: 0); // X-axis
        UpdateGraph(yLineRenderer, axisIndex: 1); // Y-axis
        UpdateGraph(zLineRenderer, axisIndex: 2); // Z-axis
    }

    private void UpdateGraph(LineRenderer lineRenderer, int axisIndex)
    {
        Vector3[] dataArray = accelerationData.ToArray();
        lineRenderer.positionCount = dataArray.Length;

        float width = graphRect.rect.width;
        float height = graphRect.rect.height;
        float xSpacing = width / maxPoints; // Space between points

        for (int i = 0; i < dataArray.Length; i++)
        {
            float value = axisIndex switch
            {
                0 => dataArray[i].x, // X-axis
                1 => dataArray[i].y, // Y-axis
                2 => dataArray[i].z, // Z-axis
                _ => 0f
            };

            // Map value to graph height
            float normalizedValue = Mathf.Clamp(value * graphScale, -height / 2, height / 2);

            // Set position in graph space
            lineRenderer.SetPosition(i, new Vector3(i * xSpacing, normalizedValue, 0));
        }
    }
}
