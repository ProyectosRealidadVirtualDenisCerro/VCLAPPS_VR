using UnityEngine;
using UnityEngine.XR;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;          // El componente AudioSource
    public AudioClip footstepSound;          // El sonido de los pasos
    public float stepThreshold = 0.5f;       // Velocidad mínima para considerar que el jugador está caminando
    private Vector3 lastPosition;            // Última posición conocida del jugador
    private float stepInterval = 0.5f;       // Tiempo mínimo entre sonidos de pasos
    private float lastStepTime = 0f;         // Tiempo del último paso

    private XRNode inputSource = XRNode.LeftHand; // Asumimos que el jugador camina con ambos pies
    private InputDevice device;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        lastPosition = transform.position; // Guarda la posición inicial
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    void Update()
    {
        // Detecta el movimiento del jugador
        Vector3 currentPosition = transform.position;
        float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime; // Calcula la velocidad

        if (speed > stepThreshold && Time.time - lastStepTime > stepInterval)
        {
            // Reproduce el sonido de los pasos si el jugador se mueve suficientemente rápido
            audioSource.PlayOneShot(footstepSound);
            lastStepTime = Time.time; // Actualiza el tiempo del último paso
        }

        lastPosition = currentPosition; // Guarda la nueva posición
    }
}
