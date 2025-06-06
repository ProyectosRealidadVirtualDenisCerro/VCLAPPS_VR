using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimaReturn : MonoBehaviour
{
    public Rigidbody limaR;
    public Transform limaT;
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
            limaR.velocity = Vector3.zero;
            Debug.Log("La lima está en el suelo");
        }
    }
}
