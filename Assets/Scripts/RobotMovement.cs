using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotMovement : MonoBehaviour
{
    private GameObject targetNode;
    private GameObject currentNode;
    private GameObject previousNode;

    private Vector3 targetPosition;
    private Vector3 currentTargetPosition;

    public GameObject TargetNode
    {
        set
        {
            if (targetNode != null)
            {
                targetNode.GetComponent<Image>().color = Color.white;
            }

            targetNode = value;

            SetCurrentNode();

            targetPosition = new Vector3(
                targetNode.transform.position.x, transform.position.y, targetNode.transform.position.z);

            if (transform.forward == Vector3.left || transform.forward == Vector3.right)
            {
                currentTargetPosition = new Vector3(
                    targetNode.transform.position.x, transform.position.y, transform.position.z);
            }
            else if (transform.forward == Vector3.forward || transform.forward == Vector3.back)
            {
                currentTargetPosition = new Vector3(
                    transform.position.x, transform.position.y, targetNode.transform.position.z);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Move()
    {
        while (transform.position != targetPosition)
        {
            SetCurrentNode();

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, 0.005f);

            if (transform.position == currentTargetPosition)
            {
                ChangeTarget();
            }

            yield return new WaitForSeconds(0);
        }
    }

    public void ChangeTarget()
    {
        if (transform.forward == Vector3.left || transform.forward == Vector3.right)
        {
            currentTargetPosition = new Vector3(transform.position.x, transform.position.y, targetNode.transform.position.z);
        }
        else if (transform.forward == Vector3.forward || transform.forward == Vector3.back)
        {
            currentTargetPosition = new Vector3(targetNode.transform.position.x, transform.position.y, transform.position.z);
        }
        transform.LookAt(targetPosition);
    }

    public void SetCurrentNode()
    {
        currentNode = GridGenerator.singleton.nodeAt(transform.position);
    }
}
