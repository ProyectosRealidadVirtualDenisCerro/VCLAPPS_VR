using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DropZoneRestriction : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Color originalColor;
    private bool canDrop = false;
    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;
    private Rigidbody objectRigidbody;
    private TriggerVelocidad triggerVelocidad;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        objectRenderer = GetComponent<Renderer>();
        objectRigidbody = GetComponent<Rigidbody>();
        triggerVelocidad = FindObjectOfType<TriggerVelocidad>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color; // Guarda el color original
        }
    }

    private void Start()
    {
        // Guarda la posición inicial del objeto como la primera posición válida
        lastValidPosition = transform.position;
        lastValidRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zona")) // Asegúrate de etiquetar la zona con la etiqueta "Zona"
        {
            canDrop = true;
            if (triggerVelocidad != null)
            {
                // Usa el color del paso actual en TriggerVelocidad
                Color zoneColor = triggerVelocidad.colores[triggerVelocidad.currentStep % triggerVelocidad.colores.Length];
                SetObjectColor(zoneColor);
            }
            else
            {
                SetObjectColor(Color.green); // Fallback color si no se encuentra TriggerVelocidad
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zona"))
        {
            canDrop = false;
            SetObjectColor(originalColor); // Restaura el color original
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Guarda la última posición y rotación válidas cuando el usuario agarra el objeto
        lastValidPosition = transform.position;
        lastValidRotation = transform.rotation;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!canDrop)
        {
            // Si el objeto se suelta fuera del área permitida
            objectRigidbody.isKinematic = true; // Desactiva la física (sin inercia)
            transform.position = lastValidPosition; // Regresa a la posición válida
            transform.rotation = lastValidRotation;
        }
        else
        {
            objectRigidbody.isKinematic = false; // Vuelve a habilitar la física
        }

        // Restaura el color original al soltar el objeto
        SetObjectColor(originalColor);
    }

    private void SetObjectColor(Color color)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = color;
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }
}