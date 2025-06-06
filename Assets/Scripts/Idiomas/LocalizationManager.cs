using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    private Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private bool isReady = false;

    void Awake()
    {
        // Configurar el Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir el objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);  // Si ya hay una instancia, destruir el nuevo objeto
        }
    }

    // Método para cargar el archivo de localización desde Resources
    public void LoadLocalizedText(string fileName)
    {
        string filePath = "Languages/" + fileName;  // Ajusta la ruta para que busque dentro de Resources/Languages
        TextAsset file = Resources.Load<TextAsset>(filePath);

        if (file != null)
        {
            string dataAsJson = file.text;  // Accede al contenido del archivo
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            localizedText.Clear();
            foreach (var item in loadedData.items)
            {
                localizedText.Add(item.key, item.value);
            }

            isReady = true;
            Debug.Log("Localización cargada: " + fileName);
        }
        else
        {
            Debug.LogError("No se encontró el archivo de localización en Resources/Languages/" + fileName);
        }
    }

    public string GetLocalizedText(string key)
    {
        if (localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        else
        {
            return "Texto no encontrado";
        }
    }

    public bool IsReady()
    {
        return isReady;
    }
}

[System.Serializable]
public class LocalizationData
{
    public LocalizationItem[] items;  // Un array de items que contendrá las traducciones
}

[System.Serializable]
public class LocalizationItem
{
    public string key;  // La clave de la traducción
    public string value;  // El valor de la traducción (texto traducido)
}
