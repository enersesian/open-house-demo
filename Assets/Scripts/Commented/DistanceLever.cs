using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceLever : MonoBehaviour
{
    /// <summary>
    /// A reference to the  distance meter's fill gauge
    /// </summary>
    [SerializeField]
    private GameObject meter;
    /// <summary>
    /// The maximum value that the distance lever can return
    /// </summary>
    [SerializeField]
    private int maxDistance;
    /// <summary>
    /// The maximum position that the distance lever can be moved to
    /// </summary>
    [SerializeField]
    private Transform maxDisTransform;
    /// <summary>
    /// The initial distance to the max position
    /// </summary>
    private float initialDistanceToMax;
    /// <summary>
    /// A reference to the fill gauge's renderer
    /// </summary>
    private Renderer render;
    /// <summary>
    /// Variable to represent the max position in local space
    /// </summary>
    private Vector3 localMaxPosition;
    /// <summary>
    /// The starting position of the distance lever
    /// </summary>
    private Vector3 startPosition;
    /// <summary>
    /// Reference to the fill gauge
    /// </summary>
    public Transform Meter
    {
        get  { return meter.transform; }
    }

    /// <summary>
    /// The current value that the distance lever is set to
    /// </summary>
    public int Value
    {
        get
        {
            int num = (int)((Vector3.Distance(transform.localPosition, startPosition) / initialDistanceToMax) * maxDistance);

            if (transform.localPosition.z <= localMaxPosition.z)
            {
                num = maxDistance;
            }
            else if (transform.localPosition.z >= startPosition.z)
            {
                num = 0;
            }

            return num;
        }
    }

    /// <summary>
    /// The maximum value that the distance lever can return
    /// </summary>
    public float MaxDistance
    {
        get { return maxDistance; }
    }

    /// <summary>
    /// Boolean representing whether or not the distance lever is currently grabbed
    /// </summary>
    public bool ChoosingDistance { get; set;}

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        startPosition = transform.localPosition;
        initialDistanceToMax = Vector3.Distance(transform.localPosition, maxDisTransform.localPosition);
        localMaxPosition = maxDisTransform.localPosition;
        render = meter.GetComponent<Renderer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void LateUpdate()
    {
        if (transform.localPosition.z <=  localMaxPosition.z)
        {
            transform.localPosition = localMaxPosition;
        }
        else if (transform.localPosition.z >= startPosition.z)
        {
            transform.localPosition = startPosition;
        }

        float blendAmount;
        blendAmount = Vector3.Distance(transform.localPosition, startPosition) / initialDistanceToMax;
        blendAmount -= 0.5f;

        render.material.SetFloat("_BlendAmount", blendAmount);
    }
}
