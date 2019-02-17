using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Level_Load>().setChange();
        //StartCoroutine(ChangeLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<Level_Load>().setChange();
    }
}

