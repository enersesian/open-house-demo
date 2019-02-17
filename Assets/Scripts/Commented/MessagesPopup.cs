using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MessagesPopup : MonoBehaviour
{
    /// <summary>
    /// Max size of the HUD
    /// </summary>
    [SerializeField]
    private float maxSize;
    /// <summary>
    /// How much the scale increases/decreases by
    /// </summary>
    [SerializeField]
    private float increment;
    /// <summary>
    /// How long the window stays open
    /// </summary>
    [SerializeField]
    private float openDuration;
    /// <summary>
    /// The text that is displayed to the player
    /// </summary>
    [SerializeField]
    private Text textBox;
    /// <summary>
    /// Boolean to checl if the player is on the first level
    /// </summary>
    [SerializeField]
    private bool firstLevel;
    /// <summary>
    /// Last message to display to the player
    /// </summary>
    [SerializeField]
    private string finalLevelMessage;
    /// <summary>
    /// Reference to the simulation ship
    /// </summary>
    [SerializeField]
    private Ship simShip;
    /// <summary>
    /// Enum for the state of the window
    /// </summary>
    private enum State { Opened, Opening, Closed, Closing }
    /// <summary>
    /// current state of window
    /// </summary>
    private State currentState;
    /// <summary>
    /// Reference of the rect transform of the HUD
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// current message index
    /// </summary>
    private int currentMessage;
    /// <summary>
    /// the scale needed to be achieved
    /// </summary>
    private Vector3 targetScale;
    /// <summary>
    /// Reference to the rneder of the current button
    /// </summary>
    private Renderer currentButton;
    /// <summary>
    /// bool to now if the system should move on
    /// </summary>
    private bool next;
  
    /// <summary>
    /// Get/Set for a bool to decide if the tutorial should be shown
    /// </summary>
    public bool DisplayingTutorial
    {
        get;
        private set;
    }
    /// <summary>
    /// Opening message of the level to show the player
    /// </summary>
    public string openingMessage;
    /// <summary>
    /// Rotaiton shown to the player when they ask for help
    /// </summary>
    public string helpRotation;

    /// <summary>
    /// Distance shown to the player when they ask for help
    /// </summary>
    public string helpDistance;
    /// <summary>
    /// Error rotation shown to the player when they fail
    /// </summary>
    public string errorRot;
    /// <summary>
    /// Error distance shown to the player when they fail
    /// </summary>
    public string errorDis;

    /// <summary>
    /// Boolean to being the level
    /// </summary>
    private bool levelBegin;

    /// <summary>
    /// Array holding the text to show the player throughout the tutorial
    /// </summary>
    private string[] tutorialText =
        {
        "This dial allows you to enter a *rotation* command. Aim at the dial and press the trigger to grab it. Press the trigger again to release.",

        "This lever allows you to enter in a distance command. Aim at the lever and press the trigger to grab it. Press the trigger again to release.",

        "This is the execute button. After entering your commands, press this button to run a simulation of the result. The simulation will appear on the radar.",

        "This is the delete button. Pressing this button will delete the currently selected command in the list. Select a command by swiping on the controller's touchpad.",

        "This is the clear button. Pressing this button will delete all commands in the list.",

        "This is the help button. This button will allow you to receive helpful hints"
    };

    // Use this for initialization
    void Start()
    {
        levelBegin = true;
        currentState = State.Closed;
        rectTransform = GetComponent<RectTransform>();
        Open();

        if (firstLevel)
        {
            StartCoroutine(Tutorial());
        }
    }


    /// <summary>
    /// moves the tutorial onto the next step
    /// </summary>
    /// <param name="control"></param>
    public void AdvanceTutorial(GameObject control)
    {
        next = control.GetComponent<Renderer>() == currentButton ? true : false;
    }

    /// <summary>
    /// Used to explain to the player what each button, the dial and the level does during the tutorial and will simulate the ship moving in the radar
    /// </summary>
    /// <returns></returns>
    IEnumerator Tutorial()
    {
        DisplayingTutorial = true;
        Renderer executeCompileButton = GameObject.FindGameObjectWithTag("ExecuteCompileButton").GetComponent<Renderer>();
        Renderer deleteButton = GameObject.FindGameObjectWithTag("DeleteButton").GetComponent<Renderer>();
        Renderer clearButton = GameObject.FindGameObjectWithTag("ClearButton").GetComponent<Renderer>();
        Renderer helpButton = GameObject.FindGameObjectWithTag("HelpButton").GetComponent<Renderer>();
        Renderer distanceLever = GameObject.FindGameObjectWithTag("DistanceLever").GetComponent<Renderer>();
        Renderer dial = GameObject.FindGameObjectWithTag("Dial").GetComponent<Renderer>();
        currentButton = dial;

        Renderer[] buttons = { dial, distanceLever, executeCompileButton, deleteButton, clearButton,
                                helpButton, distanceLever};

        StartCoroutine(ButtonFlash());

        for (int i = 0; i < tutorialText.Length; i++)
        {
            textBox.text = tutorialText[i];
            yield return new WaitUntil(() => next == true);
            next = false;
            simShip.transform.position = simShip.OriginalPosition;
            simShip.transform.rotation = simShip.OriginalRotation;
            simShip.gameObject.GetComponent<TrailRenderer>().time = 0;
            currentButton = i == tutorialText.Length - 1 ? null : buttons[i + 1];
        }

        DisplayingTutorial = false;
    }

    /// <summary>
    /// Changes the buttons, the lever and the dials color to make it flash
    /// </summary>
    /// <returns></returns>
    IEnumerator ButtonFlash()
    {
        Renderer button = currentButton;
        Color[] startColors = new Color[button.materials.Length];

        for (int i = 0; i < startColors.Length; i++)
        {
            startColors[i] = button.materials[i].color;
        }

        float time = 0.0f;
        while (currentButton == button)
        {

            for(int i = 0; i < button.materials.Length; i++)
            {
                button.materials[i].SetColor("_EmissionColor", Color.Lerp(Color.black, startColors[i], Mathf.PingPong(time, 1.0f)));
                
            }

            time += 0.04f;
            yield return new WaitForSeconds(0.0f);
        }

        for (int i = 0; i < button.materials.Length; i++)
        {
            button.materials[i].SetColor("_EmissionColor", Color.black);
        }
        
        if (DisplayingTutorial)
        {
            StartCoroutine(ButtonFlash());
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Opens up the HUD
    /// </summary>
    public void Open()
    {
        if (currentState == State.Closed)
        {
            currentState = State.Opening;
            targetScale = new Vector3(maxSize, rectTransform.localScale.y, rectTransform.localScale.z);
            StartCoroutine(ContinueOpening());
        }
    }
    /// <summary>
    /// Closes the HUD
    /// </summary>
    public void Close()
    {
        if (currentState == State.Opened)
        {
            currentState = State.Closing;
            targetScale = new Vector3(0.0f, rectTransform.localScale.y, rectTransform.localScale.z);
            StartCoroutine(ContinueClosing());
        }
    }

    /// <summary>
    /// Contines to open the HUD if the target scale is not reached
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinueOpening()
    {
        while (rectTransform.localScale != targetScale)
        {
            rectTransform.localScale = Vector3.MoveTowards(rectTransform.localScale, targetScale, increment);
            yield return new WaitForSeconds(0.001f);
        }

        currentState = State.Opened;

        yield return new WaitUntil(() => !DisplayingTutorial);

        if (levelBegin)
        {
            MessageToDisplay(openingMessage);
            levelBegin = false;
        }

        StartCoroutine(WaitToClose());
    }
    /// <summary>
    /// Cloes the HUD after the right amount of time
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(openDuration);
        Close();
    }
    /// <summary>
    /// Continues to close the HUD if it has not been cloes all the way
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinueClosing()
    {
        while (rectTransform.localScale != targetScale)
        {
            rectTransform.localScale = Vector3.MoveTowards(rectTransform.localScale, targetScale, increment);
            yield return new WaitForSeconds(0.001f);
        }

        currentState = State.Closed;
    }

    /// <summary>
    /// Displays the correct message to the player
    /// </summary>
    /// <param name="message"> The text that will be displayed </param>
    public void MessageToDisplay(string message)
    {
      
        textBox.text = message;
    }
}
