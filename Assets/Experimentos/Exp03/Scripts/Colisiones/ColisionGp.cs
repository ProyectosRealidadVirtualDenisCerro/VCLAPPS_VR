using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionGp : MonoBehaviour
{
    public bool Gp = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "G+")
        {
            Gp = true;
            Debug.Log("Generador +");
        }
    }
}
