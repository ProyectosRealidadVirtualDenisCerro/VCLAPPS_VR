using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceSliderController : MonoBehaviour
{
    public Slider forceSlider;         // Referencia al slider
    public TextMeshProUGUI forceValueText;  // Referencia al texto del valor
    public float currentForce { get; private set; } // Fuerza seleccionada

    void Start()
    {
        if (forceSlider != null)
        {
            forceSlider.onValueChanged.AddListener(UpdateForceValue);
            UpdateForceValue(forceSlider.value); // Inicializar con el valor actual
        }
    }

    private void UpdateForceValue(float value)
    {
        currentForce = value;
        if (forceValueText != null)
        {
            forceValueText.text = $"{currentForce:F2} N";
        }
    }
}
