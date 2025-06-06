using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazaReturn : MonoBehaviour
{
    public Rigidbody mazaR;
    public Transform mazaT;
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            transform.position = startPos;
            mazaR.velocity = Vector3.zero;
            Debug.Log("La maza está en el suelo");
        }
    }
}
