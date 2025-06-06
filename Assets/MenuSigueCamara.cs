using UnityEngine;

public class MenuFollowPosition : MonoBehaviour
{
    public Transform cameraTransform;
    public float followSpeed = 5f; // Velocidad de seguimiento

    private Vector3 offset; // Almacena la distancia inicial con la cámara

    void Start()
    {
        if (cameraTransform != null)
        {
            // Guarda la distancia inicial entre la cámara y el menú
            offset = transform.position - cameraTransform.position;
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Mantiene la misma distancia inicial con la cámara
        Vector3 targetPosition = cameraTransform.position + offset;
        
        // Mueve el menú suavemente con la cámara
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
