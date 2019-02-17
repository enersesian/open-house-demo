using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour {


    public GameObject otherObj;
    public float rlResult;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 tarDir = otherObj.transform.position - transform.position;
        //Debug.Log(Vector3.Angle(tarDir, transform.forward));

        Vector3  crossPro = Vector3.Cross(transform.forward, tarDir);

        rlResult = Vector3.Dot(crossPro, transform.up);


      

	}
}
