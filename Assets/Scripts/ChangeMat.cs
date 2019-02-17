using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour {


    Renderer rend;

    [SerializeField]
    Material mat1;

    [SerializeField]
    Material mat2;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown("i"))
        {
            rend.material = mat1;
        }
        else if (Input.GetKeyDown("o"))
        {
            rend.material = mat2;
        }
    }
}
