using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    // Método que se llama al seleccionar un idioma
    public void SetLanguage(string language)
    {
        Debug.Log("Setting language to: " + language);
        PlayerPrefs.SetString("selectedLanguage", language);
        PlayerPrefs.Save();

        if (LocalizationManager.instance != null)
        {
            Debug.Log("Loading localized text for language: " + language);
            LocalizationManager.instance.LoadLocalizedText(language);
        }
        else
        {
            Debug.LogError("LocalizationManager instance is null. Make sure it is present in the scene.");
        }

        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        // Obtener el idioma guardado o establecer "Ingles" como predeterminado
        string savedLanguage = PlayerPrefs.GetString("selectedLanguage", "Ingles");

        if (LocalizationManager.instance != null)
        {
            LocalizationManager.instance.LoadLocalizedText(savedLanguage);
        }
        else
        {
            Debug.LogError("LocalizationManager instance is null. Ensure that the LocalizationManager is present in the scene.");
        }
    }


}

