using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesoReturn : MonoBehaviour
{
    public Rigidbody pesoR;
    public Transform pesoT;
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
            pesoR.velocity = Vector3.zero;
            Debug.Log("La lima está en el suelo");
        }
    }
}
