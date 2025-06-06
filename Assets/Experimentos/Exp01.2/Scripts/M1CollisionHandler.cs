using UnityEngine;

public class M1CollisionHandler : MonoBehaviour
{
    private ControlExp01 controlExp; // Referencia al script principal
    public float peso = 0;
    public AudioSource metal;
    void Start()
    {
        // Buscar el script ControlExp01 en la escena
        controlExp = FindObjectOfType<ControlExp01>();
        peso = 0.1f;
        metal = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
{
        if (collision.gameObject.CompareTag("Polea"))
        {
            Debug.Log("🛑 m1 ha chocado con la polea. Deteniendo movimiento.");
        
            if (controlExp != null)
            {
                controlExp.colisionDetectada = true; // 🔴 Se activa la bandera para detener MoverCubos()
                StartCoroutine(controlExp.DetenerMovimiento()); // 🔴 Llamamos a la función como corrutina
            }
            else
            {
                Debug.LogError("❌ No se encontró ControlExp01 en la escena.");
            }
        }

        if (collision.gameObject.tag == "Peso")
        {
            peso++;
            Debug.Log(peso);
            Debug.Log("El peso está tocando");
            metal.Play();
        }

        

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Peso")
        {
            peso--;
            Debug.Log(peso);
        }
    }

}


