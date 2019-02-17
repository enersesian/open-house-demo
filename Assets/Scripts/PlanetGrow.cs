using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGrow : MonoBehaviour
{
    public Transform ship;
    public float factor;

    private float lastDiff;
    private float currentDiff;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentDiff = Vector3.Distance(ship.position, transform.position);
        float blah = 0.0f;

        if (currentDiff > lastDiff)
        {
            blah = -factor;
        }
        else if (currentDiff < lastDiff)
        {
            blah = factor;
        }

        transform.localScale += Vector3.one * blah;

        lastDiff = currentDiff;
    }
}
