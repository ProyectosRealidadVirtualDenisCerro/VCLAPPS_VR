using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class ElectricTorchOnOff : MonoBehaviour
{
    private EmissionMaterialGlassTorchFadeOut _emissionMaterialFade;
    private BatteryPowerPickup _batteryPower;

    public enum LightChoose { noBattery, withBattery }
    public LightChoose modoLightChoose;

    public bool _PowerPickUp = false;
    public float intensityLight = 2.5f;
    [SerializeField] float _lightTime = 0.05f;

    private bool _flashLightOn = false;

    [Header("XR Input")]
    public InputActionReference toggleFlashlightAction; // Acción XR configurada en el Input Action Asset

    private void Awake()
    {
        _batteryPower = FindObjectOfType<BatteryPowerPickup>();
    }

    void OnEnable()
    {
        if (toggleFlashlightAction != null)
            toggleFlashlightAction.action.performed += OnTogglePerformed;
    }

    void OnDisable()
    {
        if (toggleFlashlightAction != null)
            toggleFlashlightAction.action.performed -= OnTogglePerformed;
    }

    void Start()
    {
        GameObject _scriptControllerEmissionFade = GameObject.Find("default");

        if (_scriptControllerEmissionFade != null)
        {
            _emissionMaterialFade = _scriptControllerEmissionFade.GetComponent<EmissionMaterialGlassTorchFadeOut>();
        }
        else
        {
            Debug.LogWarning("Cannot find 'EmissionMaterialGlassTorchFadeOut' script");
        }
    }

    void Update()
    {
        switch (modoLightChoose)
        {
            case LightChoose.noBattery:
                NoBatteryLight();
                break;
            case LightChoose.withBattery:
                WithBatteryLight();
                break;
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext ctx)
    {
        ToggleFlashlight();
    }

    void ToggleFlashlight()
    {
        _flashLightOn = !_flashLightOn;
    }

    void NoBatteryLight()
    {
        if (_flashLightOn)
        {
            GetComponent<Light>().intensity = intensityLight;
            _emissionMaterialFade?.OnEmission();
        }
        else
        {
            GetComponent<Light>().intensity = 0.0f;
            _emissionMaterialFade?.OffEmission();
        }
    }

    void WithBatteryLight()
    {
        if (_flashLightOn)
        {
            GetComponent<Light>().intensity = intensityLight;
            intensityLight -= Time.deltaTime * _lightTime;
            _emissionMaterialFade?.TimeEmission(_lightTime);

            if (intensityLight < 0) intensityLight = 0;

            if (_PowerPickUp)
                intensityLight = _batteryPower.PowerIntensityLight;
        }
        else
        {
            GetComponent<Light>().intensity = 0.0f;
            _emissionMaterialFade?.OffEmission();

            if (_PowerPickUp)
                intensityLight = _batteryPower.PowerIntensityLight;
        }
    }
}
