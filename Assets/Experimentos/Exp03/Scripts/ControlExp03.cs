using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControlExp03 : MonoBehaviour
{
    public Slider ReostatoSlider; // Slider con valores de 0 a 12

    public TextMeshProUGUI txtmA; // Amperímetro (Corriente RMS en mA)
    public TextMeshProUGUI txtV; // Voltímetro (Voltaje en la resistencia fija)
    public TextMeshProUGUI txtVF; // Voltaje Fuente
    public TextMeshProUGUI txtR; // Resistencia Reostato

    public float voltajeFuente = 19f;
    public float intensidadMedia = 0.017f;
    
    // Variables públicas para acceso directo desde otros scripts
    public float voltajeActual { get; private set; }
    public float intensidadActual { get; private set; } // En mA
    public float resistenciaActual { get; private set; }

    public bool conected = false;
    private float lastUpdateTime = 0f;
    private float updateInterval = 0.5f; // Actualizar valores cada 0.5 segundos

    // Variables para la estabilización
    private float tiempoInicial = 0f;
    private float tiempoEstabilizacion = 5f; // Tiempo en segundos para estabilizarse
    private bool nuevaMedicion = true;

    public ColisionHandler CMa;
    public ColisionMe CMe;
    public ColisionMRO CRoM;
    public ColisionRoG CRoG;
    public ColisionGp CGp;
    public ColisionGm CGm;
    public ColisionAp CAp;
    public ColisionAm CAm;
    public ColisionPap CPAp;
    public ColisionMR CMR;
    public ColisionRG CRG;
    public ColisionPam CPAm;

    private void Start()
    {
        ReostatoSlider.onValueChanged.AddListener(UpdateRValue);
        UpdateRValue(ReostatoSlider.value);
    }

    void UpdateRValue(float value)
    {
        if (conected)
        {
            // Reiniciamos el proceso de estabilización cuando cambia el valor del slider
            nuevaMedicion = true;
            Operacion();
        }
    }

    private void Update()
    {
        SistemConect();

        // Actualizamos los valores periódicamente para simular fluctuaciones
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            lastUpdateTime = Time.time;
            if (conected)
            {
                Operacion();
            }
        }

        if (!conected)
        {
            Operacion();
        }
    }

    private void SistemConect()
    {
        if (CMa.Ma && CMe.Me && CRoM.RoM && CRoG.RoG && CGp.Gp && CGm.Gm && CMR.MR && CRG.RG)
        {
            conected = true;
            Debug.Log("Todo conectado");
        }
        else
        {
            conected = false;
        }
    }

    private void Operacion()
    {
        if ( CAp.Ap && CAm.Am && CPAp.PAp && CPAm.PAm && conected == true) 
        {
            // Si es una nueva medición, registramos el tiempo inicial
            if (nuevaMedicion)
            {
                tiempoInicial = Time.time;
                nuevaMedicion = false;
            }

            // Calculamos el factor de estabilización (de 0 a 1)
            float tiempoTranscurrido = Time.time - tiempoInicial;
            float factorEstabilizacion = Mathf.Clamp01(tiempoTranscurrido / tiempoEstabilizacion);

            // A medida que el factor aumenta, la fluctuación disminuye
            float rangoFluctuacion = 0.05f * (1f - factorEstabilizacion);

            // Mapear el valor del slider (111-231) a voltaje (11.1V-23.1V)
            float voltajeBase = 11.1f + (ReostatoSlider.value - 111f) * (23.1f - 11.1f) / (231f - 111f);

            // Generar una fluctuación aleatoria que se reduce con el tiempo
            float fluctuacionVoltaje = Random.Range(-rangoFluctuacion, rangoFluctuacion);
            voltajeActual = voltajeBase * (1f + fluctuacionVoltaje);

            // Calcular Resistencia con una regla de 3 y añadir también fluctuación
            float resistencia = 330 - ((ReostatoSlider.value - 111) * 330 / 120);
            float fluctuacionResistencia = Random.Range(-rangoFluctuacion, rangoFluctuacion);
            resistenciaActual = resistencia * (1f + fluctuacionResistencia);

            // Calcular la intensidad con una regla de 3 y añadir también fluctuación
            float intensidadBase = (voltajeBase * 0.024f) / 23.1f;
            float fluctuacionIntensidad = Random.Range(-rangoFluctuacion, rangoFluctuacion);
            intensidadActual = intensidadBase * (1f + fluctuacionIntensidad);

            // Asegurarnos de que los valores no sean negativos
            voltajeActual = Mathf.Max(voltajeActual, 0.1f);
            intensidadActual = Mathf.Max(intensidadActual, 0.0001f);

            // Actualizar los textos de los displays
            txtV.text = voltajeActual.ToString("F1") + "V";
            txtmA.text = intensidadActual.ToString("F5") + "mA";
            txtVF.text = "12VAC";
            txtR.text = resistenciaActual.ToString("F0") + "Ω";
        }
        else
        {
            // Si no están todas las conexiones, mostrar cero o un mensaje de error
            voltajeActual = 0f;
            intensidadActual = 0f;
            resistenciaActual = 0f;
            
            txtV.text = "0.0V";
            txtmA.text = "0.0mA";
            txtVF.text = "";
            txtR.text = "";
        }
    }
}