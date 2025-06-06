using UnityEngine;
using UnityEngine.UI;

public class SliderColorChanger : MonoBehaviour
{
    public Slider forceSlider;  // Referencia al slider
    public Image fillImage;     // Referencia a la imagen del Fill Area

    void Start()
    {
        if (forceSlider != null)
            forceSlider.onValueChanged.AddListener(UpdateFillColor);

        UpdateFillColor(forceSlider.value); // Aplicar color inicial
    }

    void UpdateFillColor(float value)
    {
        // Valores mínimos y máximos
        float minValue = 1f;
        float maxValue = 10f;

        // Normalizar el valor entre 0 y 1
        float normalizedValue = Mathf.InverseLerp(minValue, maxValue, value);

        // Interpolación de color:
        // Verde (1N) → Amarillo (5N) → Rojo (10N)
        Color color = Color.Lerp(Color.green, Color.yellow, normalizedValue * 2);  // Verde a Amarillo (0-0.5)
        if (normalizedValue > 0.5f) 
            color = Color.Lerp(Color.yellow, Color.red, (normalizedValue - 0.5f) * 2); // Amarillo a Rojo (0.5-1)

        // Aplicar el color
        if (fillImage != null)
            fillImage.color = color;
    }
}
