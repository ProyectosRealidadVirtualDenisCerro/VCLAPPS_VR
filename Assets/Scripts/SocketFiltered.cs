using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TaggedSocketInteractable : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
{
    [Tooltip("Tags permitidos para este socket")]
    public string[] allowedTags;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactable = args.interactableObject;
        string tagDelObjeto = interactable.transform.tag;

        // Verifica si el tag del objeto está en la lista de tags permitidos
        bool tagValido = false;
        foreach (string tagPermitido in allowedTags)
        {
            if (tagDelObjeto == tagPermitido)
            {
                tagValido = true;
                break;
            }
        }

        if (tagValido)
        {
            base.OnSelectEntered(args);
            Debug.Log("Objeto aceptado en el socket: " + interactable.transform.name);
        }
        else
        {
            Debug.Log("Objeto rechazado por tag: " + interactable.transform.name);
            interactionManager.SelectExit(this, interactable);
        }
    }
}
