using UnityEngine;

public class CameraFollowBezier : MonoBehaviour
{
    public BezierCurve bezier;      // Référence à ton script BezierCurve
    public float duration = 5f;     // Temps total pour parcourir la courbe
    public bool loop = false;       // Revenir au début ?

    private float elapsed = 0f;

    void Update()
    {
        if (!bezier) return;

        // Avancer dans le temps
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);

        // Application d’un easing (ease-out quadratique)
        float easedT = 1 - Mathf.Pow(1 - t, 2);

        // Récupère la position du point courant
        Vector3 position = bezier.isCubic ? bezier.GetCubicPoint(easedT) : bezier.GetQuadraticPoint(easedT);

        transform.position = position;

        // (Optionnel) Orienter la caméra vers le prochain point
        float lookAhead = 0.01f;
        float nextT = Mathf.Clamp01(easedT + lookAhead);
        Vector3 nextPos = bezier.isCubic ? bezier.GetCubicPoint(nextT) : bezier.GetQuadraticPoint(nextT);
        transform.LookAt(nextPos);

        // Reboucle si activé
        if (loop && t >= 1f)
            elapsed = 0f;
    }
}
