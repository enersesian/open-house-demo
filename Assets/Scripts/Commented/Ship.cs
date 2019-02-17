using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    /// <summary>
    /// Reference to the goal object
    /// </summary>
    [SerializeField]
    private Transform goal;
    /// <summary>
    /// Refernce to the mars object
    /// </summary>
    [SerializeField]
    private Transform mars;
    /// <summary>
    /// The required distance away from the goal in order to complete the level
    /// </summary>
    [SerializeField]
    private float completionDistance;
    /// <summary>
    /// Boolean representing whether or not this ship is a part of the simulation
    /// </summary>
    [SerializeField]
    private bool simulated;
    /// <summary>
    /// Boolean representing whether or not this ship is the rover
    /// </summary>
    [SerializeField]
    private bool rover;
    /// <summary>
    /// Variable to set how fast the ship rotates
    /// </summary>
    [Range(0.1f, 300.0f)]
    public float rotationSpeed;
    /// <summary>
    /// Variable to set how fast the ship translates
    /// </summary>
    [Range(0.1f, 10000.0f)]
    public float translationSpeed;

    /// <summary>
    /// Boolean that represents whether or not the ship is currently rotating
    /// </summary>
    private bool rotating;

    /// <summary>
    /// Boolean that represents whether or not the ship is currently translating
    /// </summary>
    private bool translating;

    /// <summary>
    /// The rotation of the ship before the dial's rotation is applied
    /// </summary>
    private Quaternion previousRot;

    /// <summary>
    /// The rotation that the ship should have after applying the dial's rotation
    /// </summary>
    private Quaternion targetRot;

    /// <summary>
    /// The position that the ship was at before it started translation
    /// </summary>
    private Vector3 previousPosition;

    /// <summary>
    /// The position that the ship is translating to 
    /// </summary>
    private Vector3 targetPosition;

    /// <summary>
    /// The amount of the translation that the ship has currently completed 
    /// </summary>
    private float journeyLength;

    /// <summary>
    /// A reference to the attached rigid body
    /// </summary>
    private Rigidbody rigidBody;

    /// <summary>
    /// The starting rotation of the dial
    /// </summary>
    public Quaternion OriginalRotation
    {
        get;
        private set;
    }

    /// <summary>
    /// The starting position of the dial
    /// </summary>
    public Vector3 OriginalPosition
    {
        get;
        private set;
    }

    /// <summary>
    /// The required distance away from the goal in order to complete the level
    /// </summary>
    public float CompletionDistance
    {
        get { return completionDistance; }
    }

    /// <summary>
    /// Used as a general time variable
    /// </summary>
    private float time;

    /// <summary>
    /// Used to determine if the correct angle was used
    /// </summary>
    private bool correctAngle;

    public bool CorrectAngle
    {
        get
        {
            return correctAngle;
        }
    }


    /// <summary>
    /// Used to hold distance between targetPosition and Goal's position
    /// </summary>
    [SerializeField]
    private float disBetween;

    /// <summary>
    /// Used to determine if the correct distance was used
    /// </summary>
    private bool correctDistance;

    /// <summary>
    /// Reference to the goal's collider
    /// </summary>
    private Collider goalCollider;

    /// <summary>
    /// Boolean representing whether or not the ship is done rotating or translating
    /// </summary>
    public bool Done
    {
        get;
        private set;

    }

    /// <summary>
    /// Reference to goal object
    /// </summary>
    public Transform Goal
    {
        get
        {
            return goal;
        }
    }

    /// <summary>
    /// Keeps track of the distance to the goal
    /// </summary>
    public float DistanceToGoal
    {
        get { return Vector3.Distance(goal.position, transform.position); }
    }

    void Start()
    {
        goalCollider = goal.GetComponent<Collider>();
        if (simulated)
        {
            GetComponent<TrailRenderer>().emitting = true;
        }
        Done = true;
        rigidBody = GetComponent<Rigidbody>();
        OriginalRotation = transform.rotation;
        OriginalPosition = transform.position;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        if (DistanceToGoal <= completionDistance)
        {
            if (simulated)
            {
                if (!Done)
                {
                    ConsoleManager.instance.SimulationSuccesful();
                    Done = true;
                }
            }
            else
            {
                if (rover)
                {
                    ConsoleManager.instance.ShowFinalMessage();
                }
                else
                {
                    LevelManager.instance.LevelCompleted = true;
                }
            }
        }

     

        if (Input.GetKeyDown(KeyCode.L))
        {
            Land();
        }
    }

    private void FixedUpdate()
    {
        if (rotating)
        {
            ContinueRotatingShip();
            return;
        }

        if (translating)
        {
            ContinueTranslatingShip();
            return;
        }
    }

    /// <summary>
    /// Begins the rotation of the ship
    /// </summary>
    /// <param name="rotateAmount">The amount the ship will be rotated by</param>
    public void BeginRotatingShip(float rotateAmount)
    {
        if (rotating || translating)
        {
            return;
        }

        Vector3 tarDir = goal.transform.position - transform.position;
    
        Vector3 crossPro = Vector3.Cross(transform.forward, tarDir);

        float rlResult = Vector3.Dot(crossPro, transform.up);
        rlResult = (Mathf.Round(rlResult * 100)) / 100;
      
        if (rotateAmount <= rlResult + 5 && rotateAmount >= rlResult - 5)
        {
            
            correctAngle = true;
        }
        else
        {
            correctAngle = false;
        }

        Done = false;
        rotating = true;
        previousRot = transform.rotation;
        targetRot = previousRot * Quaternion.AngleAxis(rotateAmount, transform.up);
    }

    /// <summary>
    /// Continues the rotation of the ship until it reaches its target rotation
    /// </summary>
    private void ContinueRotatingShip()
    {
        if (Done)
        {
            time = 0.0f;
            rotating = false;
            return;
        }

        time += Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Slerp(previousRot, targetRot, time);

        if (time >= 1.0f)
        {
            Done = true;
            time = 0.0f;
            rotating = false;
        }
    }

    /// <summary>
    /// Begins the translation of the ship
    /// </summary>
    /// <param name="direction"> The direction to translate </param>
    /// <param name="distance"> The distance to translate </param>
    public void BeginTranslatingShip(Vector3 direction, float distance)
    {
        if (translating || rotating)
        {
            return;
        }

        Done = false;
        time = Time.time;
        targetPosition = transform.position + direction * distance;

        correctDistance = !(Vector3.Distance(targetPosition, goal.transform.position) > disBetween);

        journeyLength = Vector3.Distance(transform.position, targetPosition);
        translating = true;
        previousPosition = transform.position;
    }

    /// <summary>
    /// Continues translating the ship
    /// </summary>
    public void ContinueTranslatingShip()
    {
        if (Done)
        {
            time = 0.0f;
            translating = false;
            return;
        }

        float distanceCovered = (Time.time - time) * translationSpeed;
        float journeyCompletion = distanceCovered / journeyLength;
        Vector3 vec = transform.forward * translationSpeed;
        rigidBody.MovePosition(Vector3.Lerp(previousPosition, targetPosition, journeyCompletion));

        if (journeyCompletion >= 1.0f)
        {
            Done = true;
            translating = false;
            time = 0.0f;
        }
    }

    /// <summary>
    /// Makes the ship land on the ground below it
    /// </summary>
    private void Land()
    {
        float distanceToLand;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            distanceToLand = Vector3.Distance(transform.position, hit.transform.position);
            BeginTranslatingShip(Vector3.down, distanceToLand);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WarpHole"))
        {
            WarpHole warpHole = other.GetComponent<WarpHole>();
            transform.position = warpHole.ExitOffset;
            transform.forward = warpHole.ExitDirection;
            ConsoleManager.instance.ClearCommands();
            Done = true;

            if (mars != null)
            {
                mars.gameObject.layer = LayerMask.NameToLayer("Scaled");
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Blockade"))
        {
            if (simulated)
            {
                ConsoleManager.instance.SimulationFailed();
                Done = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Done = true;
        }
    }
}
