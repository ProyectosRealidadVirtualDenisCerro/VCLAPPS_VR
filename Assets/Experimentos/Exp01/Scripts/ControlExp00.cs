using UnityEngine;
using UnityEngine.UI;

public class ControlExp00 : MonoBehaviour
{
    [Header("Rigidbody Settings")]
    public Rigidbody rbCuboGrande; 
    public Rigidbody rbCuboPequeno; 
    public float distanciaObjetivo = 1f; 

    [Header("Force Settings")]
    public Slider forceSlider; 
    public float fuerza = 5f; 

    [Header("UI Settings")]
    public Button applyForceButton; 
    public Button continueButton; 
    public Button finalizarButton;
    public Button resetearButton; 
    public GameObject panelValores; 
    public GameObject panelResultados; 
    public GameObject panelReinicio;
    public TMPro.TextMeshProUGUI[] aceleracionTexts; 
    public TMPro.TextMeshProUGUI[] friccionTexts; 
    public TMPro.TextMeshProUGUI intentosText; 

    [Header("Real-Time Distance Display")]
    public TMPro.TextMeshPro distanceDisplay; 

    [Header("Camera Control")]
    //public CameraSwitcher cameraSwitcher;
    //public int experimentPanelIndex;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicialCuboPequeno;
    private bool moviendo = false;
    private bool cuboPequenoEnMesa = false;
    private int pasoActual = 0; 
    private int intentosRestantes = 7; 
    

    private float[] aceleracionPorPaso; 
    private float[] friccionPorPaso; 

    void Start()
    {
        aceleracionPorPaso = new float[intentosRestantes];
        friccionPorPaso = new float[intentosRestantes];

        if (rbCuboGrande == null || rbCuboPequeno == null)
        {
            Debug.LogError("No se han asignado los Rigidbodies.");
            return;
        }

        posicionInicial = rbCuboGrande.position;
        rotacionInicialCuboPequeno = rbCuboPequeno.rotation;

        applyForceButton.onClick.AddListener(OnApplyForceButtonClicked);
        continueButton.onClick.AddListener(OnContinueExperiment);
        finalizarButton.onClick.AddListener(FinalizarExperimento);
        

        panelValores.SetActive(true);
        panelResultados.SetActive(false);
        UpdateIntentosText();

        continueButton.gameObject.SetActive(false);
        finalizarButton.gameObject.SetActive(false);
        resetearButton.gameObject.SetActive(false);

    }

    void FixedUpdate()
    {
        if (moviendo)
        {
            float distanciaRecorrida = Vector3.Distance(rbCuboGrande.position, posicionInicial);

            if (distanceDisplay != null)
                distanceDisplay.text = $"{distanciaRecorrida:F2} m";

            if (distanciaRecorrida < distanciaObjetivo)
            {
                rbCuboGrande.velocity = new Vector3(-fuerza, 0, 0);
            }
            else
            {
                moviendo = false;
                rbCuboGrande.velocity = Vector3.zero;
                rbCuboGrande.angularVelocity = Vector3.zero;

                float aceleracion = fuerza / rbCuboGrande.mass;
                float coeficienteFriccion = 0.3f; 
                float fuerzaNormal = rbCuboGrande.mass * Physics.gravity.magnitude;
                float friccion = coeficienteFriccion * fuerzaNormal;

                aceleracionPorPaso[pasoActual] = aceleracion;
                friccionPorPaso[pasoActual] = friccion;

                UpdatePanelData(aceleracion, friccion);

                panelResultados.SetActive(true);
                panelReinicio.SetActive(false);

                if (pasoActual < intentosRestantes - 1)
                {
                    continueButton.gameObject.SetActive(true);
                    finalizarButton.gameObject.SetActive(false);
                }
                else
                {
                    continueButton.gameObject.SetActive(false);
                    finalizarButton.gameObject.SetActive(true);
                }
            }
            Debug.Log(pasoActual);
        }

        if (cuboPequenoEnMesa)
        {
            rbCuboPequeno.velocity = Vector3.zero;
            rbCuboPequeno.angularVelocity = Vector3.zero;
        }
    }

    public void OnApplyForceButtonClicked()
    {
        fuerza = forceSlider.value;

        if (!moviendo)
        {
            moviendo = true;
            rbCuboGrande.position = posicionInicial;
            rbCuboGrande.velocity = Vector3.zero;
            panelValores.SetActive(false);
            panelReinicio.SetActive(true);
            resetearButton.gameObject.SetActive(true);
        }
    }

    public void OnContinueExperiment()
    {
        pasoActual++;
        ResetExperiment();
        continueButton.gameObject.SetActive(true);
        panelValores.SetActive(true);
        panelResultados.SetActive(false);
        panelReinicio.SetActive(false);

        
    }

    public void FinalizarExperimento()
    {
        pasoActual = 0;
        moviendo = false;

        // Resetear arrays de datos
        for (int i = 0; i < intentosRestantes; i++)
        {
            aceleracionPorPaso[i] = 0;
            friccionPorPaso[i] = 0;

            if (i < aceleracionTexts.Length)
                aceleracionTexts[i].text = "0.00 m/s²";

            if (i < friccionTexts.Length)
                friccionTexts[i].text = "0.00 N";
        }

        ResetExperiment();

        //cameraSwitcher.ReturnToPlayerControl();
        //cameraSwitcher.ActivarInstruccionesCambio();

        panelValores.SetActive(true);
        finalizarButton.gameObject.SetActive(false);
        panelResultados.SetActive(false);
        panelReinicio.SetActive(false);
        

        if (distanceDisplay != null)
            distanceDisplay.text = "0.00 m";

        //UpdateIntentosText();
    }

    private void ResetExperiment()
    {
        rbCuboGrande.position = posicionInicial;
        rbCuboGrande.velocity = Vector3.zero;
        rbCuboGrande.angularVelocity = Vector3.zero;

        rbCuboPequeno.velocity = Vector3.zero;
        rbCuboPequeno.angularVelocity = Vector3.zero;
        rbCuboPequeno.position = posicionInicial + Vector3.right * 2.2f;
        rbCuboPequeno.rotation = rotacionInicialCuboPequeno;

        cuboPequenoEnMesa = false;
        
    }

    private void UpdatePanelData(float aceleracion, float friccion)
    {
        if (pasoActual >= 0 && pasoActual < aceleracionTexts.Length && pasoActual < friccionTexts.Length)
        {
            aceleracionTexts[pasoActual].text = $"{aceleracion:F2} m/s²";
            friccionTexts[pasoActual].text = $"{friccion:F2} N";
        }
    }

    private void UpdateIntentosText()
    {
        //intentosText.text = $"Intentos restantes: {intentosRestantes - pasoActual}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table") && collision.rigidbody == rbCuboPequeno)
        {
            cuboPequenoEnMesa = true;
            rbCuboPequeno.velocity = Vector3.zero;
            rbCuboPequeno.angularVelocity = Vector3.zero;
        }
    }
}
