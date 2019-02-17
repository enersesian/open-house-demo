using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeObjCheck : MonoBehaviour {


    public GameObject cube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        Debug.Log(transform.InverseTransformPoint(transform.GetComponent<Renderer>().bounds.max));
        Debug.Log(transform.InverseTransformPoint(transform.GetComponent<Renderer>().bounds.min));
        
        Debug.Log(transform.InverseTransformPoint(cube.transform.GetComponent<Renderer>().bounds.max));
        Debug.Log(transform.InverseTransformPoint(cube.transform.GetComponent<Renderer>().bounds.min));
    }
}
