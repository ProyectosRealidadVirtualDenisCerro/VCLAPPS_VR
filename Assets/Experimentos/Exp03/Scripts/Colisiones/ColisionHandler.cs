using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionHandler : MonoBehaviour
{
    public bool Ma;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "M+")
        {
            Ma = true;
            Debug.Log("Multímetro conectado");
        }
    }
}
