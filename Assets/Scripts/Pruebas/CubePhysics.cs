using UnityEngine;

public class CubePhysics : MonoBehaviour
{
    public Rigidbody rbCuboGrande; // Rigidbody del cubo grande
    public Rigidbody rbCuboPequeno; // Rigidbody del cubo pequeño (opcional si necesitas modificarlo)
    public float fuerza = 5f; // Fuerza constante aplicada
    public float distanciaObjetivo = 1f; // Distancia máxima a recorrer
    private Vector3 posicionInicial;
    private bool moviendo = false;

    void Start()
    {
        if (rbCuboGrande == null)
        {
            Debug.LogError("No se ha asignado el Rigidbody del cubo grande.");
            return;
        }
        posicionInicial = rbCuboGrande.position; // Guarda la posición inicial
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !moviendo)
        {
            moviendo = true;
            posicionInicial = rbCuboGrande.position; // Guarda la posición inicial al comenzar el movimiento
            rbCuboGrande.velocity = Vector3.zero; // Asegurar que el cubo no tenga velocidad inicial
        }
    }

    void FixedUpdate()
    {
        if (moviendo)
        {
            float distanciaRecorrida = Vector3.Distance(rbCuboGrande.position, posicionInicial);

            if (distanciaRecorrida < distanciaObjetivo)
            {
                rbCuboGrande.velocity = new Vector3(fuerza, rbCuboGrande.velocity.y, rbCuboGrande.velocity.z);
            }
            else
            {
                moviendo = false;
                rbCuboGrande.velocity = Vector3.zero; // Detiene el movimiento
                Debug.Log("Cubo grande ha recorrido 1 metro, deteniéndose.");
            }
        }
    }
}

