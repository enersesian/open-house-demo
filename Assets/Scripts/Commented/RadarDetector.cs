using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadarDetector : MonoBehaviour
{
    /// <summary>
    /// Reference to the spinning radar scanner
    /// </summary>
    [SerializeField]
    private Transform radarScanner;
    /// <summary>
    /// Cutoff threshold for whether or not objects are within range
    /// </summary>
    [Range(-1.0f, 1.0f)]
    public float threshold;
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("RadarIcon") && collider.tag != "SimShip")
        {
            Vector3 dirToTarget = collider.transform.position - transform.position;
            dirToTarget.y = 0.0f;
            dirToTarget.Normalize();
            radarScanner.InverseTransformDirection(dirToTarget);

            Vector3 scannerUp = -radarScanner.up;
            scannerUp.y = 0.0f;
            scannerUp.Normalize();
            
            float dot = Vector3.Dot(dirToTarget, scannerUp);
            MeshRenderer meshRenderer = collider.GetComponent<MeshRenderer>();
            Color color = meshRenderer.material.color;
            if (dot >= threshold)
            {
                color.a = 1.0f;
                meshRenderer.material.color = color;
            }
            else
            {
                color.a = 0.5f;
                meshRenderer.material.color = color;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("RadarIcon"))
        {
            collider.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }
}
