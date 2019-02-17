using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRController : MonoBehaviour
{
    [SerializeField]
    private float raycastLength;
    [SerializeField]
    private Transform executeCompileButton;
    [SerializeField]
    private Transform deleteButton;
    [SerializeField]
    private Transform clearButton;
    [SerializeField]
    private Transform endpointMarker;
    [SerializeField]
    private Transform helpButton;

    /// <summary>
    /// Holds the previous rotation of the hand while turning the dial
    /// </summary>
    private float previousRotation;

    /// <summary>
    /// Reference to the dial
    /// </summary>
    private Transform dial;

    /// <summary>
    /// Reference to the thrust lever
    /// </summary>
    private Transform distanceLever;

    /// <summary>
    /// References to all children mesh renderers
    /// </summary>
    private MeshRenderer[] meshRenderers;

    /// <summary>
    /// Reference to the line renderer
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// The speed that the distance lever moves at when grabbed
    /// </summary>
    [SerializeField]
    private float distanceLeverSpeed;

    /// <summary>
    /// Reference to the main camera
    /// </summary>
    [SerializeField]
    private Camera theCamera;

    /// <summary>
    /// The initial scale of the laser end point
    /// </summary>
    private Vector3 initialScale;
    /// <summary>
    /// Reference to the connected audio source
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// The difference between the current rotation and the previous rotation
    /// </summary>
    private float RotationDiff
    {
        get
        {
            if (ConsoleManager.instance.TurningDial)
            {
                //return transform.localEulerAngles.z - previousRotation;
                return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).z - previousRotation;
            }
            else
            {
                return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x - previousRotation;
            }
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialScale = endpointMarker.localScale;
        lineRenderer = GetComponent<LineRenderer>();
        distanceLever = ConsoleManager.instance.DistanceLever;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        dial = ConsoleManager.instance.Dial;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastLength))
        {
            float distanceToHit = Vector3.Distance(hit.point, theCamera.transform.position);
            endpointMarker.localScale = initialScale * distanceToHit;
            endpointMarker.position = hit.point - transform.forward * 0.005f;
            endpointMarker.forward = (theCamera.transform.position - endpointMarker.position).normalized;
            lineRenderer.SetPosition(1, Vector3.forward * Vector3.Distance(hit.point, transform.position) * 0.75f);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (hit.transform.tag == "IntroStartButton")
                {
                    audioSource.Play();
                    LevelManager.instance.LevelCompleted = true;
                }
                else if (hit.transform.tag == "IntroQuitButton")
                {
                    audioSource.Play();
                    LevelManager.instance.RestartLevel();
                }
            }
        }
        else
        {
            endpointMarker.localScale = Vector3.zero;
            lineRenderer.SetPosition(1, Vector3.forward * raycastLength * 0.10f);
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandleKeyboardInput();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            HandleVRInput();
        }
    }

    /// <summary>
    /// Handles all keyboard input when in the editor
    /// </summary>
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            deleteButton.GetComponent<Animator>().SetTrigger("Pressed");
            ConsoleManager.instance.DeleteSelectedCommand();
            ConsoleManager.instance.AdvanceTutorial(deleteButton.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            clearButton.GetComponent<Animator>().SetTrigger("Pressed");
            ConsoleManager.instance.ClearCommands();
            ConsoleManager.instance.AdvanceTutorial(clearButton.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            executeCompileButton.GetComponent<Animator>().SetTrigger("Pressed");
            ConsoleManager.instance.Simulate();
            ConsoleManager.instance.AdvanceTutorial(executeCompileButton.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            helpButton.GetComponent<Animator>().SetTrigger("Pressed");
            ConsoleManager.instance.RequestHelp();
            ConsoleManager.instance.AdvanceTutorial(helpButton.gameObject);
        }

        if (Input.GetKey(KeyCode.W) && dial != null)
        {
            ConsoleManager.instance.TurningDial = true;
            dial.Rotate(dial.forward, 5, Space.World);
        }
        else if (Input.GetKey(KeyCode.S) && dial != null)
        {
            ConsoleManager.instance.TurningDial = true;

            dial.Rotate(dial.forward, -5, Space.World); ;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ConsoleManager.instance.ScrollCommands(2.0f);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ConsoleManager.instance.ScrollCommands(-2.0f);
        }

        if (Input.GetKey(KeyCode.T))
        {
            ConsoleManager.instance.ChoosingDistance = true;
            distanceLever.position += distanceLever.forward * distanceLeverSpeed;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            ConsoleManager.instance.ChoosingDistance = true;
            distanceLever.position -= distanceLever.forward * distanceLeverSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ConsoleManager.instance.TurningDial)
            {
                ConsoleManager.instance.AdvanceTutorial(dial.gameObject);
                ConsoleManager.instance.TurningDial = false;

            }

            if (ConsoleManager.instance.ChoosingDistance)
            {
                ConsoleManager.instance.AdvanceTutorial(distanceLever.gameObject);
                ConsoleManager.instance.ChoosingDistance = false;
            }
        }
    }

    /// <summary>
    /// Handles all VR input when built on Android
    /// </summary>
    void HandleVRInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.DpadUp))
        {
            ConsoleManager.instance.ScrollCommands(-0.5f);
        }

        if (OVRInput.GetDown(OVRInput.Button.DpadDown))
        {
            ConsoleManager.instance.ScrollCommands(0.5f);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (ConsoleManager.instance.TurningDial)
            {
                ConsoleManager.instance.TurningDial = false;
                ConsoleManager.instance.AdvanceTutorial(dial.gameObject);
                dial = null;
            }
            else if (ConsoleManager.instance.ScrollingCommands)
            {
                ConsoleManager.instance.ScrollingCommands = false;
            }
            else if (ConsoleManager.instance.ChoosingDistance)
            {
                ConsoleManager.instance.ChoosingDistance = false;
                ConsoleManager.instance.AdvanceTutorial(distanceLever.gameObject);
                distanceLever = null;
            }
            else
            {
                Raycast();
            }
        }
        else if (ConsoleManager.instance.TurningDial)
        {
            if (dial != null)
            {
                float distance = Vector3.Distance(transform.position, dial.position);
                Vector3 point = dial.InverseTransformPoint(transform.position + transform.forward * distance);
                point.z = 0.0f;
                point = dial.TransformPoint(point);
                float angle = Vector3.SignedAngle(dial.right, (point - dial.position).normalized, dial.forward);

                dial.Rotate(dial.forward, angle * 0.75f, Space.World);
            }
        }
        else if (ConsoleManager.instance.ChoosingDistance)
        {
            if (distanceLever != null)
            {
                distanceLever.localPosition += Vector3.forward * RotationDiff * 0.30f;
                previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x;
            }
        }
        else if (ConsoleManager.instance.ScrollingCommands)
        {
            ConsoleManager.instance.ScrollCommands(RotationDiff);
        }

        if (ConsoleManager.instance.ChoosingDistance || ConsoleManager.instance.TurningDial)
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = false;
            }
            lineRenderer.enabled = false;
            endpointMarker.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.enabled = true;
            }
            lineRenderer.enabled = true;
            endpointMarker.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    /// <summary>
    /// Performs a raycast from the controller's position towards the direction it is facing
    /// </summary>
    void Raycast()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("ConsoleControl")))
        {
            switch (hit.collider.transform.tag)
            {
                case "Dial":
                    ConsoleManager.instance.TurningDial = true;
                    previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).z;
                    dial = hit.collider.transform;
                    break;

                case "DeleteButton":
                    hit.collider.GetComponent<Animator>().SetTrigger("Pressed");
                    ConsoleManager.instance.AdvanceTutorial(hit.collider.gameObject);
                    ConsoleManager.instance.DeleteSelectedCommand();
                    break;

                case "ClearButton":
                    hit.collider.GetComponent<Animator>().SetTrigger("Pressed");
                    ConsoleManager.instance.AdvanceTutorial(hit.collider.gameObject);
                    ConsoleManager.instance.ClearCommands();
                    break;

                case "ExecuteCompileButton":
                    hit.collider.GetComponent<Animator>().SetTrigger("Pressed");
                    ConsoleManager.instance.AdvanceTutorial(hit.collider.gameObject);
                    ConsoleManager.instance.Simulate();
                    break;

                case "DistanceLever":
                    ConsoleManager.instance.ChoosingDistance = true;
                    previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).x;
                    distanceLever = hit.collider.transform;
                    break;
                case "HelpButton":
                    hit.collider.GetComponent<Animator>().SetTrigger("Pressed");
                    ConsoleManager.instance.AdvanceTutorial(hit.collider.gameObject);
                    ConsoleManager.instance.RequestHelp();
                    break;

                default:
                    break;
            }

        }
    }
}
