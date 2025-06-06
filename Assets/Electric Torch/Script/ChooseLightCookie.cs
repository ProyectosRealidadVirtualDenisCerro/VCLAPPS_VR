using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class ChooseLightCookieXR : MonoBehaviour
{
    [Header("Light Cookie Settings")]
    public Texture selectedCookie; // Solo una cookie fija
    private Light _thisLight;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;

    void Awake()
    {
        _thisLight = GetComponent<Light>();
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (selectedCookie != null)
            _thisLight.cookie = selectedCookie;

        _thisLight.enabled = false; // Empieza apagada
    }

    void OnEnable()
    {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        _grabInteractable.selectEntered.RemoveListener(OnGrab);
        _grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _thisLight.enabled = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        _thisLight.enabled = false;
    }
}
