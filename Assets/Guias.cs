using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Script extremadamente simple para mostrar una línea cuando se agarra un objeto y ocultarla al soltarlo.
/// </summary>
public class Guias : MonoBehaviour
{
    public GameObject objetoAgarrable; // El objeto que se puede agarrar
    public Transform puntoDestino;     // Hacia dónde va la línea

    public LineRenderer linea;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        Debug.Log("Iniciando Guias");

        // Configurar LineRenderer
        linea = GetComponent<LineRenderer>();
        if (linea == null)
        {
            linea = gameObject.AddComponent<LineRenderer>();
            Debug.Log("LineRenderer añadido");
        }

        // Configuración básica del LineRenderer
        linea.positionCount = 2;
        linea.startWidth = 0.01f;
        linea.endWidth = 0.01f;
        linea.material = new Material(Shader.Find("Sprites/Default"));
        linea.startColor = Color.yellow;
        linea.endColor = Color.yellow;
        linea.enabled = false;  // Ocultar la línea al inicio
        Debug.Log("LineRenderer configurado: " + linea);
    }

    void Start()
    {
        if (objetoAgarrable == null)
        {
            Debug.LogError("No se ha asignado un objeto agarrable");
            return;
        }

        if (puntoDestino == null)
        {
            Debug.LogError("No se ha asignado un punto destino");
            return;
        }

        if (grabInteractable == null)
        {
            Debug.LogError("El objeto agarrable no tiene un componente XRGrabInteractable");
            return;
        }

        // Registrar eventos para detectar cuando se agarra el objeto
        grabInteractable.selectEntered.AddListener(MostrarLinea);
        grabInteractable.selectExited.AddListener(OcultarLinea);

        Debug.Log("Eventos registrados");
    }

    // Este método se ejecuta cuando el objeto es agarrado
    private void MostrarLinea(SelectEnterEventArgs args)
    {
        Debug.Log("Objeto agarrado - Mostrando línea");
        linea.enabled = true;
    }

    // Este método se ejecuta cuando el objeto es soltado
    private void OcultarLinea(SelectExitEventArgs args)
    {
        Debug.Log("Objeto soltado - Ocultando línea");
        linea.enabled = false;
    }

    void Update()
    {
        // Si la línea está activa, actualizar sus posiciones
        if (linea != null && linea.enabled && objetoAgarrable != null && puntoDestino != null)
        {
            linea.SetPosition(0, objetoAgarrable.transform.position);
            linea.SetPosition(1, puntoDestino.position);
        }
    }

    void OnDestroy()
    {
        // Limpiar los eventos al destruir el objeto
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(MostrarLinea);
            grabInteractable.selectExited.RemoveListener(OcultarLinea);
        }
    }
}
