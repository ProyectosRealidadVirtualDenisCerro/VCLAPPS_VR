using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AvatarController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isExperimentActive = false;

    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    void Update()
    {
        if (!isExperimentActive && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        // Desactivar caminar
        animator.SetBool("isWalking", false);
        // Asegurarse de que esté en estado Idle
        animator.SetBool("isRunning", false);
        
        yield return new WaitForSeconds(3f);
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0 || isExperimentActive)
            return;

        agent.speed = 2f;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        
        // Asegurarse de que todas las demás animaciones estén desactivadas
        animator.SetBool("isRunning", false);
        // Activar animación de caminar
        animator.SetBool("isWalking", true);
        
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    public void GoToExperiment(Transform experimentPosition)
    {
        if (experimentPosition != null)
        {
            Debug.Log("Moviendo avatar a: " + experimentPosition.position);

            StopAllCoroutines();
            isExperimentActive = true;
            agent.speed = 5f;
            agent.SetDestination(experimentPosition.position);
            
            // Desactivar caminar y activar correr
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            
            StartCoroutine(CheckArrival(experimentPosition));
        }
        else
        {
            Debug.LogError("❌ ERROR: La posición del experimento es NULL.");
        }
    }

    private IEnumerator CheckArrival(Transform target)
    {
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        // Detener al agente
        agent.isStopped = true;
        
        // Desactivar la animación de correr
        animator.SetBool("isRunning", false);
        
        // Activar la animación de saludo (Wave) usando el trigger existente
        animator.SetTrigger("wave");
        
        // Esperar a que termine la animación de saludo
        yield return new WaitForSeconds(2f);
        
        // No necesitamos desactivar el trigger, pero aseguramos que estamos en estado idle
        // El avatar permanecerá en Idle ya que no hay otros parámetros activos
    }

    public void LeaveExperiment()
    {
        isExperimentActive = false;
        agent.isStopped = false;
        
        // No necesitamos desactivar "isWaving" porque es un trigger
        // Solo aseguramos que estamos listos para movernos
        
        MoveToNextWaypoint();
    }
}