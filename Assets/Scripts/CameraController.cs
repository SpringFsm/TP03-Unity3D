using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;        // Cible à suivre (le joueur)
    public Vector3 offset = new Vector3(0, 2, 3);
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    public float minPitch = 5f;   // limite basse pour pas que la caméra rentre dans le sol
    public float maxPitch = 50f;    // limite haute

    private float distance;
    private float yaw = 0f;
    private float pitch = 20f;

    public Transform cam;
    public float camCollisionDistance = 3f;

    void Start()
    {
        distance = offset.magnitude;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        HandleZoom();
        HandleRotation();

        // Calcul position finale
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back * distance;

        transform.position = player.position + direction;
        transform.LookAt(player.position);
        
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, camCollisionDistance))
        {
            transform.position = transform.position + transform.forward * (hit.distance + 1f);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // clic droit = caméra + joueur
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // fait tourner le joueur avec la caméra
            player.rotation = Quaternion.Euler(0, yaw, 0);
        }
        else if (Input.GetMouseButton(0)) // clic gauche = caméra seule
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(cam.position, cam.forward * camCollisionDistance, Color.red);
    }
}