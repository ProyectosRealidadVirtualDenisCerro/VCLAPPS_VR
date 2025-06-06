using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionRoG : MonoBehaviour
{
    public bool RoG = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RoG")
        {
            RoG = true;
            Debug.Log("ReostatoG");
        }
    }
}
