using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAm : MonoBehaviour
{
    public bool Am = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "A-")
        {
            Am = true;
            Debug.Log("Amperímetro- conectado");
        }
    }
}
