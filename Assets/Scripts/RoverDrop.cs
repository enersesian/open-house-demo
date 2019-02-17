using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoverDrop : MonoBehaviour
{
    public bool goalHit;
    public bool landed;
    private Text endText;
    // Use this for initialization
    void Start()
    {
        endText = GameObject.Find("EndText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        landed = true;
        if (other.gameObject.tag == "Goal")
        {
            goalHit = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (goalHit)
        {
            return;
        }
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        //endText.text = "You missed the drop zone!";
        yield return new WaitForSeconds(1.0f);
        landed = true;
        endText.text = "";
    }
}
