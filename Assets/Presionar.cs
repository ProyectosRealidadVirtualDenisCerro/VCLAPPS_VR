using UnityEngine;
using UnityEngine.Events;

public class BotonPresionable : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    [SerializeField] private Transform mano;
    [SerializeField] private float velocidadSeguimiento = 10f;
    [SerializeField] private float distanciaDeteccion = 0.1f;
    
    [Header("Configuración de Presión")]
    [SerializeField] private float profundidadMaxima = 0.05f;
    [SerializeField] private float umbralActivacion = 0.03f; // Cuánto debe hundirse para activarse
    
    [Header("Eventos")]
    public UnityEvent alHacerClick;
    
    private Vector3 posicionOriginal;
    private bool estaPresionado = false;
    private bool ejecutoEvento = false;

    void Start()
    {
        posicionOriginal = transform.position;
        
        if (mano == null)
            Debug.LogWarning("Asigna la referencia de la mano en el inspector");
    }

    void Update()
    {
        if (mano == null) return;

        // Comprobar si la mano está sobre el botón (XZ)
        float distanciaXZ = Mathf.Sqrt(
            Mathf.Pow(transform.position.x - mano.position.x, 2) + 
            Mathf.Pow(transform.position.z - mano.position.z, 2)
        );
        
        if (distanciaXZ < distanciaDeteccion)
        {
            // CORRECCIÓN: Solo permitir movimiento hacia abajo
            // La mano solo puede presionar si está por debajo de la posición original del botón
            if (mano.position.y <= posicionOriginal.y)
            {
                // Limitar la posición Y entre la original y la profundidad máxima
                float posicionYObjetivo = Mathf.Max(
                    posicionOriginal.y - profundidadMaxima,
                    mano.position.y
                );
                
                // Calcular la nueva posición del botón
                Vector3 nuevaPos = transform.position;
                nuevaPos.y = Mathf.Lerp(
                    transform.position.y,
                    posicionYObjetivo,
                    Time.deltaTime * velocidadSeguimiento
                );
                
                // Asegurar que nunca suba por encima de la posición original
                nuevaPos.y = Mathf.Min(nuevaPos.y, posicionOriginal.y);
                
                // Aplicar la nueva posición
                transform.position = nuevaPos;
                
                // Calcular cuánto se ha hundido el botón
                float profundidadActual = posicionOriginal.y - transform.position.y;
                
                // Verificar si ha alcanzado el umbral de activación
                if (profundidadActual >= umbralActivacion)
                {
                    estaPresionado = true;
                    
                    // Ejecutar el evento solo una vez por presión
                    if (!ejecutoEvento)
                    {
                        alHacerClick.Invoke();
                        ejecutoEvento = true;
                        Debug.Log("Botón activado a profundidad: " + profundidadActual);
                    }
                }
                else
                {
                    estaPresionado = false;
                    ejecutoEvento = false;
                }
            }
        }
        else
        {
            // Si la mano no está encima, volver suavemente a la posición original
            transform.position = Vector3.Lerp(
                transform.position,
                posicionOriginal,
                Time.deltaTime * velocidadSeguimiento
            );
            
            estaPresionado = false;
            ejecutoEvento = false;
        }
    }
    
    public void AsignarMano(Transform nuevaMano)
    {
        mano = nuevaMano;
    }
    
    // Método opcional para visualizar la zona de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 posicion = Application.isPlaying ? posicionOriginal : transform.position;
        Gizmos.DrawWireSphere(posicion, distanciaDeteccion);
        
        Gizmos.color = Color.red;
        Vector3 posicionMinima = posicion;
        posicionMinima.y -= profundidadMaxima;
        Gizmos.DrawLine(posicion, posicionMinima);
        
        Gizmos.color = Color.green;
        Vector3 posicionActivacion = posicion;
        posicionActivacion.y -= umbralActivacion;
        Gizmos.DrawLine(
            new Vector3(posicion.x - 0.05f, posicionActivacion.y, posicion.z),
            new Vector3(posicion.x + 0.05f, posicionActivacion.y, posicion.z)
        );
    }
}