using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionMRO : MonoBehaviour
{
    public bool RoM = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RoM")
        {
            RoM = true;
            Debug.Log("MReostato");
        }
    }
}
