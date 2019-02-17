using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotWave : MonoBehaviour {

    Animator ani;
    private float timer;
	// Use this for initialization
	void Start () {

        ani = transform.gameObject.GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {

       
        if(ani.GetBool("Seen"))
        {
            timer = timer + Time.deltaTime;
        }
        if(timer > 6.0f)
        {
            ani.SetBool("Seen", false);
            timer = 0f;
            ani.SetFloat("Timer", timer);
        }
	}

    private void OnBecameVisible()
    {
        
        ani.SetBool("Seen", true);
        ani.SetFloat("Timer", timer);
    }

    private void OnBecameInvisible()
    {
        
        timer = 0f;
        ani.SetBool("Seen", false);
        ani.SetFloat("Timer", timer);
    }

}
