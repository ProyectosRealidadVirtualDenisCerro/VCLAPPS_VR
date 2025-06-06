using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionHandler : MonoBehaviour
{
    public float fr = 0;
    

    private void Start()
    {
       
        
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Espejo")
        {
            fr = 0.1f;
            
            
        }

        if(collision.gameObject.tag == "Lima")
        {
            fr = fr - 0.1f;
            
            
        }
    }
    */

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Espejo")
        {
            fr = 0;
        }

        else if (collision.gameObject.tag == "Madera")
        {
            fr = 0.1f;
        }

        else if (collision.gameObject.tag == "Rugoso")
        {
            fr = 0.2f;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Espejo")
        {
            fr = 0;
        }

        if (collision.gameObject.tag == "Madera")
        {
            fr = 0;
        }

        if (collision.gameObject.tag == "Rugoso")
        {
            fr = 0;
        }
    }
}
