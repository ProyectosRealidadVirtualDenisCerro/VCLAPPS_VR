using UnityEngine;

public class LightObjectCollision : MonoBehaviour
{
    private Rigidbody rbCuboPequeno; // Rigidbody del cubo pequeño
    private bool cuboPequenoEnMesa = false; // Indica si el bloque pequeño está sobre la mesa

    void Start()
    {
        rbCuboPequeno = GetComponent<Rigidbody>();
        if (rbCuboPequeno == null)
        {
            Debug.LogError("No se ha asignado el Rigidbody del cubo pequeño.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colisión detectada con: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Table"))
        {
            cuboPequenoEnMesa = true;

            // Detener completamente el cubo pequeño
            rbCuboPequeno.velocity = Vector3.zero;
            rbCuboPequeno.angularVelocity = Vector3.zero;

            Debug.Log("Cubo pequeño ha tocado la mesa y se ha detenido.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            cuboPequenoEnMesa = false;
        }
    }
}
