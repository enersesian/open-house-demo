using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    public bool connected;
    private Transform debris;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && debris != null)
        {
            debris.GetComponent<Rigidbody>().isKinematic = false;
            debris.parent = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        connected = true;
        debris = collision.transform;
        collision.transform.parent = transform;
        collision.transform.GetComponent<Rigidbody>().isKinematic = true;
        collision.transform.position += Vector3.down * 0.1f;
    }

    private void OnCollisionExit(Collision collision)
    {
        connected = false;
    }
}
