using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public Transform claw;
    public float speed;
    public float distance;
    private Vector3 clawStart;
    private Vector3 target;
    private bool grabbing;
    private Vector3 vel;
    private enum State { Retracted, MovingDown, MovingUp, Extended };
    private State currentState;

    // Use this for initialization
    void Start()
    {
        currentState = State.Retracted;
        clawStart = claw.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == State.Retracted)
            {
                currentState = State.MovingDown;
                target = claw.position + Vector3.down * distance;
                StartCoroutine(Grab());
            }
            else if (currentState == State.Extended)
            {
                currentState = State.MovingUp;
                target = clawStart;
                StartCoroutine(Grab());
            }
        }
    }

    IEnumerator Grab()
    {
        while (claw.position != target && !claw.GetComponent<Claw>().connected)
        {
            claw.position = Vector3.MoveTowards(claw.position, target, speed);
            yield return new WaitForSeconds(0.01f);
        }

        if (currentState == State.MovingDown)
        {
            claw.GetComponent<Claw>().connected = false;
            currentState = State.Extended;
        }
        else if (currentState == State.MovingUp)
        {
            currentState = State.Retracted;
        }
    }
}
