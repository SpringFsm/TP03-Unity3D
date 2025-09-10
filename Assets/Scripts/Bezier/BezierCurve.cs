using UnityEngine;

[ExecuteAlways] // <--- Important pour que ça s’exécute aussi en mode Éditeur
[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // 3 pour quadratique, 4 pour cubique
    [Range(10, 100)] public int resolution = 50;
    public bool isCubic = false;

    private LineRenderer lineRenderer;

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void OnDrawGizmos()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        DrawCurve();
    }

    void DrawCurve()
    {
        if ((!isCubic && controlPoints.Length < 3) || (isCubic && controlPoints.Length < 4))
            return;

        lineRenderer.positionCount = resolution + 1;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = isCubic ? GetCubicPoint(t) : GetQuadraticPoint(t);
            lineRenderer.SetPosition(i, point);
        }
    }

    public Vector3 GetQuadraticPoint(float t)
    {
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;

        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

    public Vector3 GetCubicPoint(float t)
    {
        Vector3 p0 = controlPoints[0].position;
        Vector3 p1 = controlPoints[1].position;
        Vector3 p2 = controlPoints[2].position;
        Vector3 p3 = controlPoints[3].position;

        return Mathf.Pow(1 - t, 3) * p0 +
               3 * Mathf.Pow(1 - t, 2) * t * p1 +
               3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
               Mathf.Pow(t, 3) * p3;
    }
}
