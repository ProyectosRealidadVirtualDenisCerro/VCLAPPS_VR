using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPap : MonoBehaviour
{
    public bool PAp = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PA+")
        {
            PAp = true;
            Debug.Log("Pinza+ conectado");
        }
    }
}
