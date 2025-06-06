using UnityEngine;
using UnityEngine.Events;

public class XRPhysicalButton : MonoBehaviour
{
    public Transform restingPosition; // Posición inicial del botón
    public float activationThreshold = 0.02f; // Distancia mínima para activarlo
    public UnityEvent onPressed; // Evento que se ejecuta al presionar

    private bool isPressed = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(initialPosition, transform.position);

        if (!isPressed && distance >= activationThreshold)
        {
            isPressed = true;
            onPressed.Invoke();
        }
        else if (isPressed && distance < activationThreshold * 0.5f)
        {
            isPressed = false;
        }
    }
}
