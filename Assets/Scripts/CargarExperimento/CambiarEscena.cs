using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneChange : MonoBehaviour
{
    // Campo público para seleccionar la escena a cargar en el Inspector
    public string sceneToLoad;

    // Este método se llama cuando otro objeto entra en el Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al trigger tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Carga la escena seleccionada en el campo 'sceneToLoad'
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
