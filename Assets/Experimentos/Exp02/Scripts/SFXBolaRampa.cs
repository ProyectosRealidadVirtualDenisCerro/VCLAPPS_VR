using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource; // El componente AudioSource
    public Rigidbody ballRigidbody; // El Rigidbody de la bola
    public AudioClip rampSound; // Sonido cuando toca la rampa
    public AudioClip floorSound; // Sonido cuando toca el suelo
    public float minPitch = 0.5f; // Pitch mínimo (velocidad baja)
    public float maxPitch = 2.0f; // Pitch máximo (velocidad alta)
    private bool isOnRamp = false; // Variable para saber si la bola está en la rampa

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (ballRigidbody == null)
            ballRigidbody = GetComponent<Rigidbody>();

        audioSource.loop = false; // No necesitamos que se repita, ya que cada sonido se reproduce una vez al tocar
    }

    void Update()
    {
        if (isOnRamp)
        {
            // Obtener la velocidad de la bola (magnitud de la velocidad)
            float speed = ballRigidbody.velocity.magnitude;

            // Ajustar el pitch del sonido basado en la velocidad
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speed / 10f); // Ajusta el divisor según la escala de velocidad que desees
        }
    }

    // Detectar cuando la bola toca la rampa o el suelo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp")) // Asegúrate de que la rampa tenga la etiqueta "Ramp"
        {
            isOnRamp = true;
            audioSource.clip = rampSound; // Cambiar al sonido de la rampa
            audioSource.Play(); // Comienza a sonar cuando toca la rampa
        }
        else if (collision.gameObject.CompareTag("Floor")) // Si toca el suelo
        {
            audioSource.clip = floorSound; // Cambiar al sonido del suelo
            audioSource.Play(); // Comienza a sonar cuando toca el suelo
        }
    }

    // Detectar cuando la bola deja de estar en contacto con la rampa o el suelo
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp") || collision.gameObject.CompareTag("Floor"))
        {
            audioSource.Stop(); // Detener el sonido cuando ya no está en contacto
        }
    }
}
