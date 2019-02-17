using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;
    private float yaw;
    private float pitch;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * sensitivity;
        float yRot = Input.GetAxis("Mouse Y") * sensitivity;
        yaw += xRot;
        pitch += yRot;

        transform.localEulerAngles = new Vector3(-pitch, yaw, 0.0f);
    }
}
