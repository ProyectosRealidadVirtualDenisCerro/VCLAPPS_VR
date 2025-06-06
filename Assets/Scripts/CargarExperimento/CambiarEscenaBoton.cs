using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
