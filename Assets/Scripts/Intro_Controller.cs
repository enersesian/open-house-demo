using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro_Controller : MonoBehaviour
{

    public Vector3 handPosition = new Vector3(2.5f, -1.2f, 3.2f);
    public Transform cam;
    public float raycastDistance = 100.0f;

    private Vector3 offset;
    private float localStartDist;
    private float localEndDist;
    public GameObject playScreen;


    private float mousex;
    private float mousey;
    private float startTime;
    private float endTime;
    private Vector2 start;
    private Vector2 end;
    private bool prev;
    private Vector2 prevPos;
    public Text finalText;
    public Text chosen_input;
    public Text loadingText;
    public Image loadingCircle;
    private bool pressed;
    public float loadTime;
    private float timer;
    private float timeInLevel;
    private int inputCount;
    public enum Direction { None, Right, Left, Forward };
    public enum Strength { None, Strong, Weak };
    private bool screenShowing;

    public Level_Load levelLoader;
    // Use this for initialization
    void Start()
    {
        //localStartDist = 0;
        loadingCircle.fillAmount = 0;
        loadingText.text = "Enter a Command";
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (!screenShowing)
            {
                screenShowing = true;
                playScreen.SetActive(true);
            }
            else
            {
                if (timeInLevel >= 0.1f)
                {
                    levelLoader.setChange();
                }
            }
        }
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            if (screenShowing)
            {
                screenShowing = false;
                playScreen.SetActive(false);
                timeInLevel = 0;
            }
        }
        if (screenShowing)
        {
            timeInLevel += Time.deltaTime;
            return;
        }



        if (Input.GetKeyDown("o"))
        {
            EnterCommand(0);
        }
       // Vector2 pos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote);

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) && !prev)
        {

           // start = pos;
            //startTime = Time.time;
            prev = true;
        }

        if (!OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) && prev)
        {

            //end = prevPos;
            //endTime = Time.time;
            //float dist = Vector2.Distance(start, end);
            //float vel = dist / (endTime - startTime);

            prev = false;

            if (OVRInput.GetDown(OVRInput.Button.DpadRight))
            {
                EnterCommand(0);
            }
            else if (OVRInput.GetDown(OVRInput.Button.DpadLeft))
            {
                EnterCommand(1);
            }

            else if (OVRInput.GetDown(OVRInput.Button.DpadUp))
            {
                EnterCommand(2);
            }


            // inputqueue.GetComponent<Input_Queue>().Read();
        }
        

        if (pressed == true)
        {
            LoadCircle();
        }
    }

    void EnterCommand(int dir)
    {
        loadingText.text = "Locking in Command";
        inputCount++;
        if (dir == 0)
        {
            chosen_input.text = "Turn Right";
        }
        else if (dir == 1)
        {
            chosen_input.text = "Turn Left";
        }
        else if (dir == 2)
        {
            chosen_input.text = "Move Forward";
        }

        pressed = true;
    }
    void LoadCircle()
    {
        Debug.Log("HUR2");
        loadingCircle.fillAmount += Time.deltaTime / loadTime;
        timer += Time.deltaTime;
        if (loadingCircle.fillAmount == 1)
        {
            loadingText.text = "Enter a Command";
            timer = 0;
            pressed = false;
            loadingCircle.fillAmount = 0;
            finalText.text += chosen_input.text + "\n";
            chosen_input.text = "";
        }
    }
    void Clear_Command()
    {
        chosen_input.text = "";
        loadingCircle.fillAmount = 0;
        pressed = false;
        timer = 0;
        inputCount = 0;

    }
}
