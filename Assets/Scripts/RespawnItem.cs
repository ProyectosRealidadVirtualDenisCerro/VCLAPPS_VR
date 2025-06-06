using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectReturn : MonoBehaviour
{
    private Vector3 initialPosition; // Posición inicial del objeto
    private float maxDistance = 2f; // Distancia máxima permitida

    private void Start()
    {
        // Guardar la posición inicial del objeto al comenzar
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Verificar si la distancia entre la posición actual y la inicial es mayor que 2 metros
        if (Vector3.Distance(transform.position, initialPosition) > maxDistance)
        {
            // Volver a la posición inicial
            transform.position = initialPosition;
        }
    }
}
