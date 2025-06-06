using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ControlExp01 : MonoBehaviour
{
    public Rigidbody m1; // Bloque sobre la mesa
    public Rigidbody m2; // Bloque colgante
    public Transform pulley; // Polea
    public Transform startPositionM1; // Posición inicial de m1
    public Transform startPositionM2; // Posición inicial de m2

    //public Slider sliderM1;
    //public Slider sliderM2;
    //public Slider sliderFriccion;

    public TextMeshProUGUI txtM1;
    public TextMeshProUGUI txtM2;
    public TextMeshProUGUI txtFriccion;
    
    public GameObject panelValores;  
    public GameObject panelDatos;    

    public TextMeshProUGUI[] m1Texts;
    public TextMeshProUGUI[] m2Texts;
    public TextMeshProUGUI[] tTexts;
    public TextMeshProUGUI[] muTexts;

    public Button iniciarButton;
    public Button siguienteButton;
    public Button finalizarButton;

    private float masa1;
    private float masa2;
    private float friccion;
    private int intento = 0;
    private bool enEjecucion = false;
    private float tiempoFinal;
    private float distancia = 2f; // Distancia m1 - polea

    private const float g = 9.81f; // Gravedad

    private Vector3 startPosM1;
    private Vector3 startPosM2;

    public bool colisionDetectada = false; // Detecta cuando m1 toca la polea

    private M1CollisionHandler peso1;
    private M2CollisionHandler peso2;
    private FrictionHandler friction;


    void Start()
    {
        peso1 = FindObjectOfType<M1CollisionHandler>();
        peso2 = FindObjectOfType<M2CollisionHandler>();
        friction = FindObjectOfType<FrictionHandler>();

        iniciarButton.onClick.AddListener(IniciarExperimento);
        siguienteButton.onClick.AddListener(SiguienteIntento);
        finalizarButton.onClick.AddListener(FinalizarExperimento);

        // 🔴 Suscribir eventos de cambio en los sliders
        //sliderM1.onValueChanged.AddListener(UpdateM1Value);
        //sliderM2.onValueChanged.AddListener(UpdateM2Value);
        //sliderFriccion.onValueChanged.AddListener(UpdateFriccionValue);

        // 🔴 Inicializar los valores en pantalla
        //UpdateM1Value(peso1.peso);
        //UpdateM2Value(sliderM2.value);
        //UpdateFriccionValue(sliderFriccion.value);

        panelValores.SetActive(false);
        panelDatos.SetActive(false);
        finalizarButton.gameObject.SetActive(false);

        // 🔴 GUARDAR la posición inicial en una variable Vector3
        startPosM1 = m1.transform.position;
        startPosM2 = m2.transform.position;

        Debug.Log($"✅ Posición inicial guardada de m1: {startPosM1}");
        Debug.Log($"✅ Posición inicial guardada de m2: {startPosM2}");

        // 🔴 Asegurar que m1 está en su posición inicial y sin fuerzas
        m1.transform.position = startPosM1;
        m1.velocity = Vector3.zero;
        m1.angularVelocity = Vector3.zero;
        m1.isKinematic = false;

        //txtM1.text = peso1.peso.ToString("F0") + "KG";
    }
    void Update()
    {
        txtM1.text = peso1.peso.ToString("F1") + "KG"; // Muestra con 2 decimales
        txtM2.text = peso2.peso.ToString("F1") + "KG";
        txtFriccion.text = friction.fr.ToString("F1") + "μk";
    }


    void UpdateFriccionValue(float value)
    {
        
    }
    public void IniciarExperimento()
{
    if (intento >= 4 || enEjecucion) return;

    Debug.Log("🔄 Iniciando experimento...");

    // 🔴 Asegurar que `enEjecucion` está en true para ejecutar `MoverCubos()`
    enEjecucion = true;

    // 🔴 Reactivar físicas en `m1` y `m2`
    m1.isKinematic = false;
    m2.isKinematic = false;

    // 🔴 Apagar la gravedad de `m2` hasta que comience el movimiento
    m2.useGravity = false;

    // Obtener valores de sliders
    masa1 = peso1.peso;
    masa2 = peso2.peso;
    friccion = friction.fr;

    // Calcular el tiempo de caída
    tiempoFinal = CalcularTiempoCaida();

    // Configurar masas
    m1.mass = masa1;
    m2.mass = masa2;

    // 🔴 Mostrar en consola los valores
    Debug.Log($"⚙️ m1: {masa1} kg, m2: {masa2} kg, fricción: {friccion}, tiempo calculado: {tiempoFinal}");

    // 🔴 Verificar que `tiempoFinal` es válido antes de iniciar
    if (tiempoFinal <= 0)
    {
        Debug.LogError("❌ Error: Tiempo de caída inválido.");
        enEjecucion = false;
        return;
    }

    // 🔴 Iniciar la simulación de movimiento
    panelValores.SetActive(false);
    StartCoroutine(MoverCubos());
}


    float CalcularTiempoCaida()
    {
        float fuerzaGravedadM2 = masa2 * g;
        float fuerzaFriccion = friccion * masa1 * g;

        // Asegurar que la fuerza neta no es negativa
        float fuerzaNeta = Mathf.Max(fuerzaGravedadM2 - fuerzaFriccion, 0.01f);

        // Calcular aceleración usando la Segunda Ley de Newton
        float aceleracion = fuerzaNeta / (masa1 + masa2);

        // Evitar divisiones por cero
        if (aceleracion <= 0)
            return float.MaxValue; // Un valor grande para indicar que no se mueve

        // Calcular el tiempo usando la ecuación del movimiento uniformemente acelerado
        return Mathf.Sqrt((2 * distancia) / aceleracion);
    }

    IEnumerator MoverCubos()
{
    float tiempo = 0;
    Vector3 inicioM1 = m1.position;
    Vector3 destinoM1 = new Vector3(pulley.position.x - (m1.transform.localScale.x / 2), m1.position.y, m1.position.z);
    
    Vector3 inicioM2 = m2.position;
    float desplazamiento = destinoM1.x - inicioM1.x;
    Vector3 destinoM2 = new Vector3(m2.position.x, inicioM2.y - desplazamiento, m2.position.z);

    Debug.Log("▶️ Iniciando movimiento de cubos...");

    while (tiempo < tiempoFinal)
    {
        if (!enEjecucion)
        {
            Debug.LogWarning("⚠️ Movimiento detenido.");
            yield break;
        }

        tiempo += Time.deltaTime;
        float t = Mathf.Clamp01(tiempo / tiempoFinal);

        m1.position = Vector3.Lerp(inicioM1, destinoM1, t);
        m2.position = Vector3.Lerp(inicioM2, destinoM2, t);

        yield return null;
    }

    Debug.Log("✅ Movimiento completado. Deteniendo cubos.");
    StartCoroutine(DetenerMovimiento());
}





    public IEnumerator DetenerMovimiento()
{
    if (!enEjecucion) yield break; // Evita llamadas múltiples

    enEjecucion = false;

    // 🔴 Detener ambos cubos inmediatamente sin modificar posiciones
    m1.velocity = Vector3.zero;
    m1.angularVelocity = Vector3.zero;
    m2.velocity = Vector3.zero;
    m2.angularVelocity = Vector3.zero;

    // 🔴 Apagar la gravedad de `m2` para que no siga cayendo
    m2.useGravity = false;

    // 🔴 Desactivar la física de ambos cubos para evitar movimientos inesperados
    m1.isKinematic = true;
    m2.isKinematic = true;

    Debug.Log("✅ Ambos cubos detenidos en su posición actual.");

    MostrarResultados();
}






    void MostrarResultados()
    {
        Debug.Log("MostrarResultados");
        m1Texts[intento].text = $"{masa1:F2}";
        m2Texts[intento].text = $"{masa2:F2}";
        tTexts[intento].text = $"{tiempoFinal:F3}";
        muTexts[intento].text = $"{friccion:F1}";

        intento++;

        panelDatos.SetActive(true);

        if (intento >= 4)
        {
            siguienteButton.gameObject.SetActive(false);
            finalizarButton.gameObject.SetActive(true);
        }
    }

   IEnumerator ReiniciarPosiciones()
{
    Debug.Log("🔄 Reiniciando posiciones...");

    enEjecucion = false;

    m1.isKinematic = true;
    m2.isKinematic = true;

    yield return new WaitForSeconds(0.1f);

    m1.transform.position = startPosM1;
    m2.transform.position = startPosM2;

    Debug.Log($"✅ Posición reiniciada de m1: {m1.transform.position}");
    Debug.Log($"✅ Posición reiniciada de m2: {m2.transform.position}");

    yield return new WaitForSeconds(0.1f);

    m1.isKinematic = false;
    m2.isKinematic = false;

    m2.useGravity = false;

    m1.velocity = Vector3.zero;
    m1.angularVelocity = Vector3.zero;
    m2.velocity = Vector3.zero;
    m2.angularVelocity = Vector3.zero;

    Debug.Log("✅ Rigidbody reactivado y posiciones corregidas.");

    panelDatos.SetActive(false);
    panelValores.SetActive(true);
}




    void SiguienteIntento()
{
    if (intento >= 4) return;

    StartCoroutine(ReiniciarPosiciones());
}


    void FinalizarExperimento()
{
    Debug.Log("🔴 Finalizando experimento y saliendo...");

    // 🔴 Detener la simulación
    enEjecucion = false;

    // 🔴 Reiniciar intento
    intento = 0;

    // 🔴 Resetear la posición de los cubos
    StartCoroutine(ReiniciarPosiciones());

    // 🔴 Desactivar la gravedad de m2
    m2.useGravity = false;

    // 🔴 Volver la cámara al jugador (si usa CameraSwitcher)
    /*if (FindObjectOfType<CameraSwitcher>() != null)
    {
        FindObjectOfType<CameraSwitcher>().ReturnToPlayerControl();
    }
    */

    // 🔴 Ocultar panel de datos
    panelDatos.SetActive(false);
    finalizarButton.gameObject.SetActive(false);

    // 🔴 Volver a activar el botón "Siguiente"
    siguienteButton.gameObject.SetActive(true);
    siguienteButton.interactable = true;

    // 🔴 Restablecer los valores de los paneles con "-----"
    foreach (var txt in m1Texts) txt.text = "-----";
    foreach (var txt in m2Texts) txt.text = "-----";
    foreach (var txt in tTexts) txt.text = "-----";
    foreach (var txt in muTexts) txt.text = "-----";

    // 🔴 También limpiamos los valores de los sliders y textos de los valores en tiempo real
    //sliderM1.value = sliderM1.minValue;
    //sliderM2.value = sliderM2.minValue;
    //sliderFriccion.value = sliderFriccion.minValue;

    //UpdateM1Value(peso1.peso);
    //UpdateM2Value(sliderM2.value);
    //UpdateFriccionValue(sliderFriccion.value);

    Debug.Log("✅ Experimento finalizado y listo para reiniciarse.");
}

    






}
