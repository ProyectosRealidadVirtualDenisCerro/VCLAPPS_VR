using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionRG : MonoBehaviour
{
    public bool RG = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RG")
        {
            RG = true;
            Debug.Log("ResGenerador");
        }
    }
}
