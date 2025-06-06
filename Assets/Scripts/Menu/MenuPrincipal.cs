using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // M�todo que se llama al presionar el bot�n Play
    public void PlayGame()
    {
        // Carga la escena del juego, aseg�rate de que la escena est� incluida en Build Settings
        SceneManager.LoadScene("MainGame");  // Cambia esto por el nombre de tu escena de juego
    }

    // M�todo que se llama al presionar el bot�n Exit
    public void ExitGame()
    {
        // Cierra la aplicaci�n
        Debug.Log("Sali� del juego!");  // Este mensaje solo se ver� en el editor
        Application.Quit();
    }

    // M�todo que se llama al presionar el bot�n Settings
    public void Settings()
    {
        // Carga la escena de settings, aseg�rate de que la escena est� incluida en Build Settings
        SceneManager.LoadScene("SettingsMenu");  // Cambia esto por el nombre de tu escena de juego
    }
}
