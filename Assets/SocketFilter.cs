using UnityEngine;


public class TagBasedSocketFilter : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
{
    [SerializeField] private string[] allowedTags; // Lista de tags permitidos

    public override bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable)
    {
        Debug.Log($"PruebaSocket");
        // Si el objeto tiene un tag permitido, deja que el socket lo acepte
        foreach (string tag in allowedTags)
        {
            if (interactable.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}
