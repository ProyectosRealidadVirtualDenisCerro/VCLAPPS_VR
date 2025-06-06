using UnityEngine;

public class PulleyRope : MonoBehaviour
{
    public Transform blockOnTable; // Bloque sobre la mesa
    public Transform pulley;       // Polea (punto de giro)
    public Transform hangingBlock; // Bloque colgante
    public Transform diagonal;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4; // La cuerda tiene cuatro segmentos
    }

    void Update()
    {
        // Ajustar los puntos de la cuerda
        lineRenderer.SetPosition(0, blockOnTable.position); 
        lineRenderer.SetPosition(1, pulley.position); 
        lineRenderer.SetPosition(2, diagonal.position); 
        lineRenderer.SetPosition(3, hangingBlock.position); 

    }
}
