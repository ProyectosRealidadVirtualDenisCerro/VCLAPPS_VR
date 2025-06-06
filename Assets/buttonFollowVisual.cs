using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FollowOnHover : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool isFollowing = false;
    private bool isFrozen = false;
    private Transform pokeAttachTransform;
    public Transform visualTarget;
    
    // Dirección del movimiento (eje Y negativo = hacia abajo)
    public Vector3 localAxis = new Vector3(0, -1, 0);
    
    private Vector3 initialLocalPos;
    public float resetSpeed = 5f;
    
    // Limitar cuánto puede moverse hacia abajo
    public float maxPushDistance = 0.05f;

    private void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        initialLocalPos = visualTarget.localPosition;
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor interactor)
        {
            pokeAttachTransform = interactor.attachTransform;
            isFollowing = true;
            isFrozen = false;
        }
    }

    public void Reset(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)
        {
            isFollowing = false;
            isFrozen = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)
        {
            isFollowing = false;
            isFrozen = true;
        }
    }

    private void Update()
    {
        if (isFrozen)
            return;

        if (isFollowing && pokeAttachTransform != null)
        {
            // Calcular la distancia entre el interactor y el objeto en el espacio local
            Vector3 localInteractorPos = visualTarget.parent.InverseTransformPoint(pokeAttachTransform.position);
            Vector3 localButtonPos = visualTarget.localPosition;
            
            // Calcular la proyección solo en el eje Y
            float yOffset = localInteractorPos.y - initialLocalPos.y;
            
            // Limitar el movimiento solo hacia abajo (valores negativos)
            yOffset = Mathf.Min(0, yOffset);
            
            // Limitar la distancia máxima que puede moverse
            yOffset = Mathf.Max(-maxPushDistance, yOffset);
            
            // Aplicar el offset solo al eje Y
            Vector3 newPosition = initialLocalPos;
            newPosition.y += yOffset;
            
            // Actualizar la posición del botón
            visualTarget.localPosition = newPosition;
        }
        else
        {
            // Restaurar suavemente a la posición original cuando no se está siguiendo
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }
}