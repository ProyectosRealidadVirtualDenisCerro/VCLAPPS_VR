using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionGm : MonoBehaviour
{
    public bool Gm = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "G-")
        {
            Gm = true;
            Debug.Log("Generador- conectado");
        }
    }
}
