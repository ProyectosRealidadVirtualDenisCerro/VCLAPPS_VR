using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Método que se llama al presionar el botón Play
    public void PlayGame()
    {
        // Carga la escena del juego, asegúrate de que la escena esté incluida en Build Settings
        SceneManager.LoadScene("MainGame");  // Cambia esto por el nombre de tu escena de juego
    }

    // Método que se llama al presionar el botón Exit
    public void ExitGame()
    {
        // Cierra la aplicación
        Debug.Log("Salió del juego!");  // Este mensaje solo se verá en el editor
        Application.Quit();
    }

    // Método que se llama al presionar el botón Settings
    public void Settings()
    {
        // Carga la escena de settings, asegúrate de que la escena esté incluida en Build Settings
        SceneManager.LoadScene("SettingsMenu");  // Cambia esto por el nombre de tu escena de juego
    }
}
