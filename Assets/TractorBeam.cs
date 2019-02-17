using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collid)
    {
        if(collid.gameObject.tag == "Sample")
        {
            CollectSample(collid.gameObject);
        }
    }

    void CollectSample(GameObject sample)
    {
        Debug.Log("Sample Collected");
        Destroy(sample);
    }
}
