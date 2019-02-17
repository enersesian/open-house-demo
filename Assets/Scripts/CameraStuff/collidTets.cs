using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collidTets : MonoBehaviour {


    public GameObject cube;
    public GameObject cube2;

    public float aMaxX;
    public float aMaxY;
    public float aMaxZ;
    public float aMinX;
    public float aMinY;
    public float aMinZ;

    public float bMaxX;
    public float bMaxY;
    public float bMaxZ;
    public float bMinX;
    public float bMinY;
    public float bMinZ;

    public bool test;
    public bool moving;
    // Use this for initialization
    void Start () {



        aMaxX = cube.GetComponent<Collider>().bounds.max.x;
        aMaxY = cube.GetComponent<Collider>().bounds.max.y;
        aMaxZ = cube.GetComponent<Collider>().bounds.max.z;

        aMinX = cube.GetComponent<Collider>().bounds.min.x;
        aMinY = cube.GetComponent<Collider>().bounds.min.y;
        aMinZ = cube.GetComponent<Collider>().bounds.min.z;


        bMaxX = cube2.GetComponent<Collider>().bounds.max.x;
        bMaxY = cube2.GetComponent<Collider>().bounds.max.y;
        bMaxZ = cube2.GetComponent<Collider>().bounds.max.z;

        bMinX = cube2.GetComponent<Collider>().bounds.min.x;
        bMinY = cube2.GetComponent<Collider>().bounds.min.y;
        bMinZ = cube2.GetComponent<Collider>().bounds.min.z;

    }
	
	// Update is called once per frames
	void Update () {


        if(Input.GetKey("d"))
        {
            moving = true;

            cube.transform.position = cube.transform.position + cube.transform.right * Time.deltaTime;
        }
        else
        {
            moving = false;
           // Camera.main.WorldToScreenPoint
        }

        if(moving == true)
        {
            BoundsCheck();
        }
        //aMaxX = cube.GetComponent<Collider>().bounds.max.x;
        //aMaxY = cube.GetComponent<Collider>().bounds.max.y;
        //aMaxZ = cube.GetComponent<Collider>().bounds.max.z;

        //aMinX = cube.GetComponent<Collider>().bounds.min.x;
        //aMinY = cube.GetComponent<Collider>().bounds.min.y;
        //aMinZ = cube.GetComponent<Collider>().bounds.min.z;


        //bMaxX = cube2.GetComponent<Collider>().bounds.max.x;
        //bMaxY = cube2.GetComponent<Collider>().bounds.max.y;
        //bMaxZ = cube2.GetComponent<Collider>().bounds.max.z;

        //bMinX = cube2.GetComponent<Collider>().bounds.min.x;
        //bMinY = cube2.GetComponent<Collider>().bounds.min.y;
        //bMinZ = cube2.GetComponent<Collider>().bounds.min.z;
        

        

    }

    void BoundsCheck()
    {
        aMaxX = cube.GetComponent<Collider>().bounds.max.x;
        aMaxY = cube.GetComponent<Collider>().bounds.max.y;
        aMaxZ = cube.GetComponent<Collider>().bounds.max.z;

        aMinX = cube.GetComponent<Collider>().bounds.min.x;
        aMinY = cube.GetComponent<Collider>().bounds.min.y;
        aMinZ = cube.GetComponent<Collider>().bounds.min.z;


        bMaxX = cube2.GetComponent<Collider>().bounds.max.x;
        bMaxY = cube2.GetComponent<Collider>().bounds.max.y;
        bMaxZ = cube2.GetComponent<Collider>().bounds.max.z;

        bMinX = cube2.GetComponent<Collider>().bounds.min.x;
        bMinY = cube2.GetComponent<Collider>().bounds.min.y;
        bMinZ = cube2.GetComponent<Collider>().bounds.min.z;

        if (aMaxX > bMaxX || aMinX < bMinX)
        {
            Debug.Log("Here");
            test = false;
            return;
        }


        if (aMaxY > bMaxY || aMinY < bMinY)
        {
            Debug.Log("Here2");
            test = false;
            return;

        }

        if (aMaxZ > bMaxZ || aMinZ < bMinZ)
        {
            Debug.Log("Here3");
            test = false;
            return;
        }

        else

        {
            Debug.Log("Nope");
            test = true;
        }
    }
}
