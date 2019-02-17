using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScanner : MonoBehaviour
{
    /// <summary>
    /// The speed that the scanner rotates at
    /// </summary>
    [SerializeField]
    private float rotationSpeed;

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed);
    }
}
