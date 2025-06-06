using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DropReturnToInitialPosition : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody objectRigidbody;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Guarda la posición y rotación inicial del objeto
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Guarda la posición y rotación cuando el objeto es agarrado
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // Regresa el objeto a la posición inicial al soltarlo
        objectRigidbody.isKinematic = true; // Desactiva la física
        transform.position = initialPosition; // Regresa a la posición inicial
        transform.rotation = initialRotation; // Regresa a la rotación inicial
        objectRigidbody.isKinematic = false; // Reactiva la física
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
