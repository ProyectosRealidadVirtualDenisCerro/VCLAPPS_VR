using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;  // Clave para el texto localizado

    private Text uiTextComponent;  // Componente de UI Text
    private TextMeshProUGUI textMeshProUGUIComponent;  // Componente de UI TextMeshPro
    private TextMeshPro textMeshProComponent;  // Componente de TextMeshPro para textos 3D

    void Start()
    {
        // Verifica si hay un componente de Unity UI Text
        uiTextComponent = GetComponent<Text>();
        if (uiTextComponent != null)
        {
            UpdateUIText();
            return;
        }

        // Verifica si hay un componente TextMeshProUGUI
        textMeshProUGUIComponent = GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUIComponent != null)
        {
            UpdateTextMeshProUGUIText();
            return;
        }

        // Verifica si hay un componente TextMeshPro para textos 3D
        textMeshProComponent = GetComponent<TextMeshPro>();
        if (textMeshProComponent != null)
        {
            UpdateTextMeshProText();
            return;
        }

        // Si no encuentra ninguno, lanza un error
        Debug.LogError("No Text, TextMeshProUGUI, or TextMeshPro component found on the GameObject: " + gameObject.name);
    }

    // M�todo para actualizar el texto de UI Text
    private void UpdateUIText()
    {
        if (LocalizationManager.instance != null && LocalizationManager.instance.IsReady())
        {
            uiTextComponent.text = LocalizationManager.instance.GetLocalizedText(key);
        }
        else
        {
            Debug.LogError("LocalizationManager no est� listo o es nulo");
        }
    }

    // M�todo para actualizar el texto de TextMeshProUGUI
    private void UpdateTextMeshProUGUIText()
    {
        if (LocalizationManager.instance != null && LocalizationManager.instance.IsReady())
        {
            textMeshProUGUIComponent.text = LocalizationManager.instance.GetLocalizedText(key);
        }
        else
        {
            Debug.LogError("LocalizationManager no est� listo o es nulo");
        }
    }

    // M�todo para actualizar el texto de TextMeshPro (Textos 3D)
    private void UpdateTextMeshProText()
    {
        if (LocalizationManager.instance != null && LocalizationManager.instance.IsReady())
        {
            textMeshProComponent.text = LocalizationManager.instance.GetLocalizedText(key);
        }
        else
        {
            Debug.LogError("LocalizationManager no est� listo o es nulo");
        }
    }
}




