using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRend : MonoBehaviour {


    LineRenderer lRend;

    [SerializeField]
    GameObject goal;
    Vector3 goalPos;
	// Use this for initialization
	void Start () {
        lRend = GetComponent<LineRenderer>();

        goalPos = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {

      

        if(goal.GetComponent<MeshRenderer>().enabled == true)
        {
            //Debug.Log("true");
            // lRend.gameObject.SetActive(true);
            lRend.enabled = true;
            lRend.SetPosition(0, transform.position);
            lRend.SetPosition(1, goalPos);
        }
        else
        {
            lRend.enabled = false;
        }
       




    }
}
