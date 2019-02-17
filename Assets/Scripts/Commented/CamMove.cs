using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {

    public float timer;
    public GameObject holder;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        transform.position += transform.forward * 2 * Time.deltaTime;

        if(timer > 5)
        {
           holder.transform.parent = transform;
           holder.transform.localPosition = Vector3.zero;
        }
	}
}
