using UnityEngine;

public class AvatarExperimentHandler : MonoBehaviour
{
    public AvatarController avatarController; // 🔥 Asegurar que está asignado en el Inspector
    public static AvatarExperimentHandler instance; // Singleton

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void MoveAvatarToExperiment(Transform experimentPosition)
    {
        if (avatarController != null && experimentPosition != null)
        {
            avatarController.GoToExperiment(experimentPosition); // 🔥 Llamar al método correcto
        }
        else
        {
            Debug.LogError("AvatarController o experimentPosition no asignados.");
        }
    }

    public void ReturnAvatarToPatrol()
    {
        if (avatarController != null)
        {
            avatarController.LeaveExperiment();
        }
    }
}

