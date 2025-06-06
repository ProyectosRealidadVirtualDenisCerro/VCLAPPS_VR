using UnityEngine;
using UnityEngine.AI;

public class DetectarJugador : MonoBehaviour
{
    public Transform jugador;           // Referencia al jugador
    public float radioDeteccion = 10f;  // Radio para detectar al jugador
    public GameObject objetoActivar;    // Objeto que quieres activar
    
    // Referencia al Animator
    private Animator animator;
    private NavMeshAgent agente;
    private bool jugadorDetectado = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Comprobamos que tengamos un animator
        if (animator == null)
            Debug.LogWarning("No se ha encontrado un componente Animator");
            
        // Asegúrate de tener asignado el objeto a activar
        if (objetoActivar == null)
            Debug.LogWarning("No se ha asignado el objeto a activar");
            
        // Si no has asignado el jugador, intenta encontrarlo por tag
        if (jugador == null)
            jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // Si no hay jugador, no podemos continuar
        if (jugador == null)
            return;
            
        // Calcula la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        
        // Comprueba si el jugador está dentro del radio de detección
        if (distanciaAlJugador <= radioDeteccion)
        {
            if (!jugadorDetectado)
            {
                // Primera vez que detecta al jugador
                jugadorDetectado = true;
                
                // Detener el NavMeshAgent
                agente.isStopped = true;
                
                // Cambiar a la animación "wave" usando un bool en lugar de un trigger
                if (animator != null)
                {
                    // Desactivar la animación de caminar
                    animator.SetBool("IsWalking", false);
                    // Activar la animación de saludo (usando un bool en lugar de trigger)
                    animator.SetBool("IsWaving", true);
                }
                
                // Activar el objeto
                if (objetoActivar != null)
                    objetoActivar.SetActive(true);
            }
            
            // Mirar hacia el jugador (suavemente)
            Vector3 direccion = jugador.position - transform.position;
            direccion.y = 0; // Para que no mire hacia arriba o abajo
            
            if (direccion != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
            }
        }
        else if (jugadorDetectado)
        {
            // El jugador ya no está en el radio de detección
            jugadorDetectado = false;
            
            // Reanudar el NavMeshAgent
            agente.isStopped = false;
            
            // Volver a la animación de caminar
            if (animator != null)
            {
                // Desactivar la animación de saludo
                animator.SetBool("IsWaving", false);
                // Activar la animación de caminar
                animator.SetBool("IsWalking", true);
            }
            
            // Desactivar el objeto
            if (objetoActivar != null)
                objetoActivar.SetActive(false);
        }
    }
    
    // Método opcional para visualizar el radio de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}