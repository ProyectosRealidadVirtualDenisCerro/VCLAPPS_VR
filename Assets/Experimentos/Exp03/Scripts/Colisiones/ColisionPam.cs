using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionPam : MonoBehaviour
{
    public bool PAm = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "PA-")
        {
            PAm = true;
            Debug.Log("Pinza- conectado");
        }
    }
}
