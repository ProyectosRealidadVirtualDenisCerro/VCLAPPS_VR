using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionMe : MonoBehaviour
{
    public bool Me = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "M-")
        {
            Me = true;
            Debug.Log("Multímetro conectado RO");
        }
    }
}
