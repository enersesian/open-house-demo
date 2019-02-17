using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpHole : MonoBehaviour
{
    [SerializeField]
    private Transform exit;
    [SerializeField]
    private float offset;

    public Vector3 ExitOffset
    {
        get
        {
            float z = exit.GetComponent<Collider>().bounds.extents.z;
            return exit.position + exit.forward * (z + offset);
        }
    }

    public Vector3 ExitDirection
    {
        get { return exit.forward; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
