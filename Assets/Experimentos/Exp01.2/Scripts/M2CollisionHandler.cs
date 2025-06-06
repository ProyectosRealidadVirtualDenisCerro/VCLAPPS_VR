using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2CollisionHandler : MonoBehaviour
{
    public float peso = 0.1f;
    public AudioSource metal;

    private void Start()
    {
        peso = 0.1f;
        metal = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Peso")
        {
            peso++;
            Debug.Log(peso);
            Debug.Log("El peso está tocando");
            metal.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Peso")
        {
            peso--;
            Debug.Log(peso);
        }
    }
}
