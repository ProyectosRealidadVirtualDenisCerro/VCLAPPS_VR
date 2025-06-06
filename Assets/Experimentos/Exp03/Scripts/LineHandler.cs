using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    public Transform cable1; 
    public Transform cable2;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Ajustar los puntos de la cuerda
        lineRenderer.SetPosition(0, cable1.position);
        lineRenderer.SetPosition(1, cable2.position);
        
    }
}
