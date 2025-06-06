using UnityEngine;

public class RemovePhysicMaterialOnTrigger : MonoBehaviour
{
    private PhysicMaterial originalMaterial;
    
    private void Start()
    {
        // Guarda el material original
        originalMaterial = GetComponent<Collider>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Socket"))
        {
            GetComponent<Collider>().material = null; // O elimina el material
            // GetComponent<Collider>().material = newMaterial; // O usa otro material sin rebote
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Socket"))
        {
            GetComponent<Collider>().material = originalMaterial; // Restaura el material original
        }
    }
}
