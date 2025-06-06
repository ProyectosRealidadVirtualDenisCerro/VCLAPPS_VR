using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GuideHandler : MonoBehaviour
{
    public Transform cable1;
    public Transform cable2;
    public Transform origen;
    private LineRenderer lineRenderer;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false; // Ocultar la línea al inicio

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        lineRenderer.enabled = true; // Mostrar la línea al agarrar
        lineRenderer.SetPosition(0, cable1.position);
        lineRenderer.SetPosition(1, cable2.position);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        lineRenderer.enabled = false; // Ocultar la línea al soltar
    }

    void Update()
    {
        if (lineRenderer.enabled)
        {
            // Actualizar posiciones si la línea está activa
            lineRenderer.SetPosition(0, cable1.position);
            lineRenderer.SetPosition(1, cable2.position);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
