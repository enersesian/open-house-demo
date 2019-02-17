using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetHighlight : MonoBehaviour {


    [SerializeField]
    GameObject planet;
   

    [SerializeField]
    Text planetName;
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Vector3 textOrgPos;

    [SerializeField]
    Vector3 textPos;

    private Vector3 orgScale;

    public Vector3 planetScreenBoundsmax;
    public Vector3 planentScreenBoundsmin;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        PlanetLookedAt();

        if(planetName != null)
        {
            UpdateTextPosition();
        }
	}


    void PlanetLookedAt()
    {


        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
        {
           
          
            
            HighlightPlanet(hit.transform.gameObject);
        }
    }

    void HighlightPlanet(GameObject planetToHighlight)
    {
        planet = planetToHighlight;

        
        DisplayName();
       
    }

    void DisplayName()
    {
        if(planetName != null)
        {
            // planetName
            //planetName.transform.position = textOrgPos;
            //planetName.transform.parent = planet.transform;
            //canvas.transform.position = textOrgPos;
            canvas.transform.localScale = orgScale;
            canvas.transform.gameObject.SetActive(false);
           // canvas.transform.parent = planet.transform;
        }
        planetName = planet.GetComponent<Planet>().text;

        canvas = planet.GetComponent<Planet>().canvas;
        canvas.transform.gameObject.SetActive(true);
        orgScale = canvas.transform.localScale;
        //textOrgPos = canvas.transform.position;

      
        
        //planetName.transform.LookAt(transform.position);
        
        //planetName = planet.GetComponent<Text>
    }
    
    void UpdateTextPosition()
    {


        float distance = Vector3.Distance(canvas.transform.position, transform.position);

        canvas.transform.localScale = orgScale * (distance / 7000);

        canvas.transform.LookAt(transform.position);
        canvas.transform.forward = -canvas.transform.forward;
        //canvas.transform.localScale = 


        // RaycastHit hit;

        //if (Physics.Raycast(planetName.transform.position, planetName.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Window") & ~LayerMask.GetMask("Planet")))
        //{
        //    canvas.transform.position = hit.point;


        //    planetScreenBoundsmax = Camera.main.WorldToScreenPoint(planet.GetComponent<Planet>().rend.bounds.max);


        //    Vector3 newPos = Camera.main.WorldToScreenPoint(planetName.transform.position);

        //    newPos.y = planetScreenBoundsmax.y + 5;

        //    canvas.transform.position = Camera.main.ScreenToWorldPoint(newPos);

        //    canvas.transform.parent = Camera.main.transform;
        //    //screenBoundsmin = Camera.main.WorldToScreenPoint(rend.bounds.min);


        //}
    }
}
