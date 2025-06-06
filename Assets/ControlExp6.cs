using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;

public class ControlExp6 : MonoBehaviour
{
    [Header("Sockets")]
    [Tooltip("Socket para LED")]
    public GameObject socketLED;

    [Tooltip("Socket para Tapa")]
    public GameObject socketTapa;

    [Tooltip("Sockets de cables")]
    public GameObject[] socketsCable; // ARRAY DE SOCKETS DE CABLES

    [Header("Linternas")]
    public GameObject linternaNormal;
    public GameObject linternaUV;
    public float distanciaDeteccion = 10f;
    public float radioDeteccion = 0.5f; // Radio para la detección esférica
    public string tagObjetivo = "Detectable";

    [Header("UI")]
    public TextMeshProUGUI textoPSU;
    public TextMeshProUGUI textoMultimetro;

    // Referencias a los objetos en los sockets
    private GameObject ledInstalado;
    private GameObject tapaInstalada;
    private GameObject linternaActiva;

    // Propiedades de los objetos
    private bool esLedNormal = false;
    private bool esLedUV = false;
    private bool esTapaTranslucida = false;
    private bool esTapaOpaca = false;

    // Estado de detección
    [HideInInspector] public bool estaApuntando = false;
    [HideInInspector] public GameObject objetoDetectado = null;

    void Start()
    {
    }

    void Update()
    {
        ActualizarEstadoSockets();
        ActualizarEstadoLinternas();
        DetectarConLinterna();

        if (TodosLosSocketsCableOcupados()) // SOLO SI TODOS LOS SOCKETS DE CABLE ESTÁN OCUPADOS
        {
            textoPSU.text = "12V";
            ActualizarLecturaMultimetro();
        }
        else
        {
            textoMultimetro.text = "00.00";
            textoPSU.text = "";
        }
    }

    void ActualizarEstadoSockets()
    {
        var socketLEDInteractor = socketLED.GetComponent<XRSocketInteractor>();
        if (socketLEDInteractor != null && socketLEDInteractor.hasSelection)
        {
            ledInstalado = socketLEDInteractor.firstInteractableSelected.transform.gameObject;
            esLedNormal = ledInstalado.CompareTag("LEDNormal");
            esLedUV = ledInstalado.CompareTag("LEDUV");
        }
        else
        {
            ledInstalado = null;
            esLedNormal = false;
            esLedUV = false;
        }

        var socketTapaInteractor = socketTapa.GetComponent<XRSocketInteractor>();
        if (socketTapaInteractor != null && socketTapaInteractor.hasSelection)
        {
            tapaInstalada = socketTapaInteractor.firstInteractableSelected.transform.gameObject;
            esTapaTranslucida = tapaInstalada.CompareTag("TapaTranslucida");
            esTapaOpaca = tapaInstalada.CompareTag("TapaOpaca");
        }
        else
        {
            tapaInstalada = null;
            esTapaTranslucida = false;
            esTapaOpaca = false;
        }
    }

    void ActualizarEstadoLinternas()
    {
        Light luzNormal = linternaNormal.GetComponentInChildren<Light>();
        Light luzUV = linternaUV.GetComponentInChildren<Light>();

        if (luzNormal != null && luzNormal.enabled)
            linternaActiva = linternaNormal;
        else if (luzUV != null && luzUV.enabled)
            linternaActiva = linternaUV;
        else
            linternaActiva = null;
    }

    void DetectarConLinterna()
    {
        estaApuntando = false;
        objetoDetectado = null;

        if (linternaActiva != null)
        {
            Light luz = linternaActiva.GetComponentInChildren<Light>();
            if (luz != null && luz.enabled)
            {
                // Punto de origen del rayo
                Vector3 origen = linternaActiva.transform.position;
                // Dirección del rayo
                Vector3 direccion = linternaActiva.transform.forward;
            

                // Primero intentamos con un raycast normal
                RaycastHit hit;
                if (Physics.Raycast(origen, direccion, out hit, distanciaDeteccion))
                {
                    if (EsObjetivoValido(hit.collider.gameObject))
                    {
                        estaApuntando = true;
                        objetoDetectado = hit.collider.gameObject;
                        Debug.Log("Raycast directo detectó: " + objetoDetectado.name);
                        return;
                    }
                }

                // Si el raycast directo falla, buscamos objetos en un radio alrededor del punto final del rayo
                Vector3 puntoFinal = origen + direccion * distanciaDeteccion;
                Collider[] colisiones = Physics.OverlapSphere(puntoFinal, radioDeteccion);
                
                // También podemos verificar puntos a lo largo del rayo
                for (float distancia = 0; distancia <= distanciaDeteccion; distancia += distanciaDeteccion/5)
                {
                    Vector3 puntoPrueba = origen + direccion * distancia;
                    Collider[] colisionesPunto = Physics.OverlapSphere(puntoPrueba, radioDeteccion);
                    
                    foreach (Collider col in colisionesPunto)
                    {
                        if (EsObjetivoValido(col.gameObject))
                        {
                            estaApuntando = true;
                            objetoDetectado = col.gameObject;
                            Debug.Log("Detección esférica en punto intermedio detectó: " + objetoDetectado.name);
                            return;
                        }
                    }
                }
                
                // Si no encontramos nada en los puntos intermedios, verificamos el punto final
                foreach (Collider col in colisiones)
                {
                    if (EsObjetivoValido(col.gameObject))
                    {
                        estaApuntando = true;
                        objetoDetectado = col.gameObject;
                        Debug.Log("Detección esférica en punto final detectó: " + objetoDetectado.name);
                        return;
                    }
                }
            }
        }
    }

    bool EsObjetivoValido(GameObject obj)
    {
        return obj.CompareTag("LEDNormal") || 
               obj.CompareTag("LEDUV") ||
               obj.CompareTag("TapaTranslucida") || 
               obj.CompareTag("TapaOpaca") ||
               obj.CompareTag(tagObjetivo);
    }

    bool TodosLosSocketsCableOcupados()
    {
        foreach (GameObject socket in socketsCable)
        {
            var interactor = socket.GetComponent<XRSocketInteractor>();
            if (interactor == null || !interactor.hasSelection)
                return false;
        }
        return true;
    }

    void ActualizarLecturaMultimetro()
    {
        float lectura = CalcularLectura();

        if (lectura < 1.0f)
            textoMultimetro.text = "00." + (int)(lectura * 10);
        else if (lectura < 10.0f)
            textoMultimetro.text = "0" + lectura.ToString("F1");
        else if (lectura < 100.0f)
            textoMultimetro.text = lectura.ToString("F1");
        else
            textoMultimetro.text = lectura.ToString("F0");

        Debug.Log($"Lectura: {lectura} - LED Normal: {esLedNormal}, LED UV: {esLedUV}, Tapa Trans: {esTapaTranslucida}, Tapa Opaca: {esTapaOpaca}, Linterna: {(linternaActiva == linternaNormal ? "Normal" : (linternaActiva == linternaUV ? "UV" : "Ninguna"))}");
    }

    float CalcularLectura()
    {
        if (ledInstalado == null)
            return 0.0f;

        if (esTapaOpaca && (esLedNormal || esLedUV))
            return 0.1f;

        bool apuntandoALed = objetoDetectado != null && (objetoDetectado.CompareTag("LEDNormal") || objetoDetectado.CompareTag("LEDUV"));
        bool apuntandoATapa = objetoDetectado != null && (objetoDetectado.CompareTag("TapaTranslucida") || objetoDetectado.CompareTag("TapaOpaca"));
        bool linternaActivaYApuntando = (linternaActiva != null) && (apuntandoALed || apuntandoATapa);

        string configuracion = "";

        if (esLedNormal)
            configuracion += "LED_NORMAL_";
        else if (esLedUV)
            configuracion += "LED_UV_";

        if (esTapaTranslucida)
            configuracion += "TAPA_TRANS_";
        else if (tapaInstalada == null)
            configuracion += "SIN_TAPA_";

        if (linternaActivaYApuntando)
        {
            if (linternaActiva == linternaNormal)
                configuracion += "LINTERNA_NORMAL";
            else if (linternaActiva == linternaUV)
                configuracion += "LINTERNA_UV";

            if (apuntandoALed)
                configuracion += "_APUNTA_LED";
            else if (apuntandoATapa)
                configuracion += "_APUNTA_TAPA";
        }
        else
        {
            configuracion += "SIN_LINTERNA";
        }

        Debug.Log("Configuración actual: " + configuracion);

        switch (configuracion)
        {
            case "LED_NORMAL_SIN_TAPA_SIN_LINTERNA": return 300.0f;
            case "LED_NORMAL_TAPA_TRANS_SIN_LINTERNA": return 100.0f;
            case "LED_NORMAL_SIN_TAPA_LINTERNA_NORMAL_APUNTA_LED": return 3000.0f;
            case "LED_NORMAL_TAPA_TRANS_LINTERNA_NORMAL_APUNTA_TAPA": return 1000.0f;
            case "LED_UV_SIN_TAPA_SIN_LINTERNA": return 100.0f;
            case "LED_UV_TAPA_TRANS_SIN_LINTERNA": return 40.0f;
            case "LED_UV_SIN_TAPA_LINTERNA_NORMAL_APUNTA_LED": return 200.0f;
            case "LED_UV_TAPA_TRANS_LINTERNA_NORMAL_APUNTA_TAPA": return 70.0f;
            case "LED_UV_SIN_TAPA_LINTERNA_UV_APUNTA_LED": return 3000.0f;
            case "LED_UV_TAPA_TRANS_LINTERNA_UV_APUNTA_TAPA": return 1000.0f;
            // Nuevos casos para cubrir la combinación LED normal con luz UV
            case "LED_NORMAL_SIN_TAPA_LINTERNA_UV_APUNTA_LED": return 500.0f; // Valor para LED normal con luz UV
            case "LED_NORMAL_TAPA_TRANS_LINTERNA_UV_APUNTA_TAPA": return 200.0f; // Valor para LED normal con luz UV y tapa translúcida
            default: return 0.0f;
        }
    }
}