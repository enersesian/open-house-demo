using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDial : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's ship
    /// </summary>
    [SerializeField]
    private Transform ship;

    /// <summary>
    /// Enum for the various increments the dial can have
    /// </summary>
    public enum Increments
    {
        /// <summary>
        /// 15 degree increment
        /// </summary>
        _15 = 15,
        /// <summary>
        /// 30 degree increment
        /// </summary>
        _30 = 30,
        /// <summary>
        /// 45 degree increment
        /// </summary>
        _45 = 45
    };

    /// <summary>
    /// The current increment mode of the dial
    /// </summary>
    public Increments increment = Increments._15;

    /// <summary>
    /// List that holds the different angles that the dial can be rotated to
    /// </summary>
    private List<int> angles;

    /// <summary>
    /// Returns the rotation of the dial 
    /// </summary>
    public float RotateAmount { get; private set; }

   

    /// <summary>
    /// Boolean representing whether or not the dial currently being turned
    /// </summary>
    private bool turningDial;

    /// <summary>
    /// Boolean that represents whether or not the dial is currently being turned
    /// </summary>
    public bool TurningDial
    {
        get { return turningDial; }
        set
        {
            turningDial = value;
            if (turningDial == false)
            {
                StopTurningDial();
            }
        }
    }

    /// <summary>
    /// The starting rotation of the dial
    /// </summary>
    private Vector3 startRotation;

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Start()
    {
        startRotation = transform.localEulerAngles;

        angles = new List<int>();
        int numValue = (int)increment;
        for (int i = 0; i < ((180 / numValue) * 2); i++)
        {
            angles.Add(180 - (numValue * i));
        }
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// Stops the player from having control over the dial, and snaps the dial to the closest angle increment
    /// </summary>
    void StopTurningDial()
    {
        float min = 1000.0f;
        float minAngle = 0.0f;
        float zRot = transform.localEulerAngles.z;
        float modifiedAngle = zRot > 180.0f ? 360 - zRot : zRot;
        for (int i = 0; i < angles.Count; i++)
        {
            float distance = Mathf.Abs(angles[i] - modifiedAngle);
            if (distance < min)
            {
                min = distance;
                minAngle = angles[i];
            }
        }
        minAngle = zRot > 180.0f ? -minAngle : minAngle;
        RotateAmount = minAngle;
        transform.localEulerAngles = startRotation + minAngle * Vector3.forward;
    }
}
