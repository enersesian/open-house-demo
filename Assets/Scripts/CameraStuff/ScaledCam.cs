using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledCam : MonoBehaviour {


    [SerializeField]
    Transform scaledSpot;
    [SerializeField]
    Camera mainCam;
    Camera scaledCam;
    [Range(1.0f, 20000f)]
    public float  scaleFactor;

    Transform scalePos;
	// Use this for initialization
	void Start () {

        scaledCam = GetComponent<Camera>();
        scaledCam.fieldOfView = mainCam.fieldOfView;
        //scaledCam.farClipPlane = mainCam.farClipPlane;
       // transform.position = transform.position / scaleFactor;
       // scaledSpot.position = transform.position;
       // scaledCam.transform.position = mainCam.transform.position;
	}

    // Update is called once per frame
    void Update() {

        AdjustCam();
    }

    void AdjustCam()
    {
        transform.rotation = mainCam.transform.rotation;

    }


    
}
