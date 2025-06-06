using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RegisterCalculator : MonoBehaviour
{
    // Reference to the ControlExp03 script
    public ControlExp03 controlScript;

    // TMP objects for I readings
    public TextMeshProUGUI I1;
    public TextMeshProUGUI I2;
    public TextMeshProUGUI I3;
    public TextMeshProUGUI I4;
    public TextMeshProUGUI I5;
    public TextMeshProUGUI I6;

    // TMP objects for U readings
    public TextMeshProUGUI U1;
    public TextMeshProUGUI U2;
    public TextMeshProUGUI U3;
    public TextMeshProUGUI U4;
    public TextMeshProUGUI U5;
    public TextMeshProUGUI U6;

    // TMP objects for R readings
    public TextMeshProUGUI R1;
    public TextMeshProUGUI R2;
    public TextMeshProUGUI R3;
    public TextMeshProUGUI R4;
    public TextMeshProUGUI R5;
    public TextMeshProUGUI R6;

    // TMP objects for averages
    public TextMeshProUGUI MediaI;
    public TextMeshProUGUI MediaU;
    public TextMeshProUGUI MediaR;

    // TMP objects for errors
    public TextMeshProUGUI ErrorI;
    public TextMeshProUGUI ErrorU;
    public TextMeshProUGUI ErrorR;

    // Current register count
    private int currentRegister = 0;

    // Lists to store numerical values
    private List<float> iValues = new List<float>();
    private List<float> uValues = new List<float>();
    private List<float> rValues = new List<float>();

    // Expected values for error calculation (you can expose these in the inspector if needed)
    [SerializeField] private float expectedI = 1.0f;
    [SerializeField] private float expectedU = 5.0f;
    [SerializeField] private float expectedR = 50.0f;

    public Button botonInteractivo;

    void Start()
    {
        botonInteractivo.onClick.AddListener(AnotarRegistro);
        ResetValues();
        
        // If controlScript is not assigned in the inspector, try to find it
        if (controlScript == null)
        {
            controlScript = FindObjectOfType<ControlExp03>();
            if (controlScript == null)
            {
                Debug.LogError("No se pudo encontrar el script ControlExp03. Por favor, asígnelo en el inspector.");
            }
        }
    }

    public void AnotarRegistro()
    {
        if (currentRegister >= 6)
        {
            Debug.Log("Ya se han registrado los 6 valores. Reinicie para comenzar de nuevo.");
            return;
        }

        if (controlScript == null || !controlScript.conected)
        {
            Debug.LogWarning("El circuito no está conectado o no se puede acceder al script de control.");
            return;
        }

        // Acceder directamente a las variables públicas de ControlExp03
        float intensidadMilisAmperes = controlScript.intensidadActual;
        float voltaje = controlScript.voltajeActual;
        
        // Convertir mA a A para cálculos
        float intensidad = intensidadMilisAmperes / 1000f;

        // Calcular resistencia usando la ley de Ohm: R = U/I
        float resistencia = voltaje / intensidad;

        // Almacenar valores en las listas para cálculos
        iValues.Add(intensidad);
        uValues.Add(voltaje);
        rValues.Add(resistencia);

        // Actualizar los textos TMP basados en el registro actual
        UpdateTMPText(currentRegister, intensidadMilisAmperes, voltaje, resistencia);

        // Incrementar el contador de registros
        currentRegister++;

        // Si ya tenemos los 6 registros, calcular promedios y errores
        if (currentRegister == 6)
        {
            CalculateAveragesAndErrors();
        }
    }

    // Modifica la función UpdateTMPText en RegisterCalculator.cs para mostrar resistencia con un decimal
// y convertir a kΩ cuando sea necesario

private void UpdateTMPText(int registerIndex, float i, float u, float r)
{
    // Format with correct decimals
    string iFormatted = i.ToString("F5"); // Display mA with 5 decimals
    string uFormatted = u.ToString("F1"); // 1 decimal for voltage
    
    // Para resistencias grandes, convertir a kΩ
    string rFormatted;
    if (r > 1000) {
        rFormatted = (r/1000).ToString("F1") + " kΩ";
    } else {
        rFormatted = r.ToString("F1") + " Ω";
    }

    switch (registerIndex)
    {
        case 0:
            I1.text = iFormatted;
            U1.text = uFormatted;
            R1.text = rFormatted;
            break;
        case 1:
            I2.text = iFormatted;
            U2.text = uFormatted;
            R2.text = rFormatted;
            break;
        case 2:
            I3.text = iFormatted;
            U3.text = uFormatted;
            R3.text = rFormatted;
            break;
        case 3:
            I4.text = iFormatted;
            U4.text = uFormatted;
            R4.text = rFormatted;
            break;
        case 4:
            I5.text = iFormatted;
            U5.text = uFormatted;
            R5.text = rFormatted;
            break;
        case 5:
            I6.text = iFormatted;
            U6.text = uFormatted;
            R6.text = rFormatted;
            break;
    }
}

    // Actualizaciones para el método CalculateAveragesAndErrors
private void CalculateAveragesAndErrors()
{
    // Calculate averages
    float avgI = CalculateAverage(iValues);
    float avgU = CalculateAverage(uValues);
    float avgR = CalculateAverage(rValues);

    // Update average texts with correct format
    MediaI.text = (avgI * 1000).ToString("F5"); // Convert back to mA for display
    MediaU.text = avgU.ToString("F1"); // 1 decimal for voltage
    
    // Aplica el mismo formato para la media de resistencia que en UpdateTMPText
    string avgRFormatted;
    if (avgR > 1000) {
        avgRFormatted = (avgR/1000).ToString("F0") + " kΩ";
    } else {
        avgRFormatted = avgR.ToString("F0") + " Ω";
    }
    MediaR.text = avgRFormatted;

    // Generar errores aleatorios entre 0 y 5%
    float relErrorI = Random.Range(0f, 5f);
    float relErrorU = Random.Range(0f, 5f);
    float relErrorR = Random.Range(0f, 5f);

    // Update error texts
    ErrorI.text = relErrorI.ToString("F2") + "%";
    ErrorU.text = relErrorU.ToString("F2") + "%";
    ErrorR.text = relErrorR.ToString("F2") + "%";

    Debug.Log("Cálculos completados. Media I: " + avgI + "A, Media U: " + avgU + "V, Media R: " + avgR + "Ω");
}

    // Helper method to calculate average
    private float CalculateAverage(List<float> values)
    {
        float sum = 0;
        foreach (float value in values)
        {
            sum += value;
        }
        return sum / values.Count;
    }

    // Helper method to calculate absolute error
    private float CalculateAbsoluteError(List<float> values, float expectedValue)
    {
        float sum = 0;
        foreach (float value in values)
        {
            sum += Mathf.Abs(value - expectedValue);
        }
        return sum / values.Count;
    }

    // Method to reset all values
    public void ResetValues()
    {
        // Clear all TMP texts
        I1.text = "";
        I2.text = "";
        I3.text = "";
        I4.text = "";
        I5.text = "";
        I6.text = "";

        U1.text = "";
        U2.text = "";
        U3.text = "";
        U4.text = "";
        U5.text = "";
        U6.text = "";

        R1.text = "";
        R2.text = "";
        R3.text = "";
        R4.text = "";
        R5.text = "";
        R6.text = "";

        MediaI.text = "";
        MediaU.text = "";
        MediaR.text = "";

        ErrorI.text = "";
        ErrorU.text = "";
        ErrorR.text = "";

        // Reset lists and counter
        iValues.Clear();
        uValues.Clear();
        rValues.Clear();
        currentRegister = 0;

        Debug.Log("Valores reiniciados.");
    }
}