using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCheck : MonoBehaviour {

    Renderer rend;
    //Renderer rend1;
     
    //Collider collid;
    public Vector3 screenBoundsmax;
    public Vector3 screenBoundsmin;

    
   // Vector3 winBoundsmax;
   // Vector3 winBoundsmin;

    //Vector3 screenSpacePos;

    public GameObject screen;

    public GameObject Info;

    public GameObject InfoScreen;
    public bool test;

    
	// Use this for initialization
	void Start () {
        Info.transform.gameObject.SetActive(false);
        rend = transform.GetComponent<Renderer>();
 
	}
	
	// Update is called once per frame
	void Update () {

      
        screenBoundsmax = Camera.main.WorldToScreenPoint(rend.bounds.max);
        screenBoundsmin = Camera.main.WorldToScreenPoint(rend.bounds.min);

        if(Input.GetKeyDown("o"))
        {
            Debug.Log(Mathf.Abs(screenBoundsmax.x - screenBoundsmin.x));
            Debug.Log(Screen.height);
            Debug.Log(Screen.width);
        }

        CheckSize();
    }


    private void OnBecameInvisible()
    {
       if(Info.transform.gameObject == null && transform.gameObject == null && screen.gameObject == null && InfoScreen.gameObject == null)
        {
           // Debug.Log("Hello");
           // Debug.Log(Info.transform);
           //Info.transform.gameObject.SetActive(false);
        }
        
    }

    private void OnBecameVisible()
    {
        CheckSize();
        Info.transform.position = transform.position;

        float dis = Vector3.Distance(Info.transform.position, screen.transform.position);

        Info.transform.LookAt(Camera.main.transform);

        RaycastHit hit;

        if (Physics.Raycast(Info.transform.position, Info.transform.forward, out hit, dis, LayerMask.GetMask("Window") & ~LayerMask.GetMask("Planet")))
        {
            Info.transform.position = hit.point;
        }
    }

    private void CheckSize()
    {
        if (Mathf.Abs(screenBoundsmax.x - screenBoundsmin.x) > 150 || Mathf.Abs(screenBoundsmax.x - screenBoundsmin.x) < 20)
        {
            Info.transform.gameObject.SetActive(false);
     
        }
        else
        {
            Info.transform.gameObject.SetActive(true);
    
        }
    }
}
