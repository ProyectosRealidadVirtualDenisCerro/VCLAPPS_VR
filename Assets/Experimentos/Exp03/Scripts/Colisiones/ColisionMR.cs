using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionMR : MonoBehaviour
{
    public bool MR = false;
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "RM")
        {
            MR = true;
            Debug.Log("Multi Res");
        }
    }
}
