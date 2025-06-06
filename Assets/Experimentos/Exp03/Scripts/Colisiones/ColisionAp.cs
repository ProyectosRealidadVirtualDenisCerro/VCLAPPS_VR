using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAp : MonoBehaviour
{
    public bool Ap = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "A+")
        {
            Ap = true;
            Debug.Log("Amperímetro+ conectado");
        }
    }
}
