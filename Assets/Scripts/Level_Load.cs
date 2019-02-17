using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Load : MonoBehaviour {

   
    private float timer;
    public int i;
    private AsyncOperation asyncLoad;
    private bool changelevel;
    // Use this for initialization
    void Start () {

        //loadingCircle.fillAmount = 0;
        //transform.gameObject.SetActive(false);
        i = SceneManager.GetActiveScene().buildIndex + 1;
        //Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        StartCoroutine(LoadSceneAsy());
       
    }
	
	// Update is called once per frame
	void Update () {
            
    }

    public void setChange()
    {
        asyncLoad.allowSceneActivation = true;
        
    }

    IEnumerator LoadSceneAsy()
    {
        asyncLoad = SceneManager.LoadSceneAsync(i);

        asyncLoad.allowSceneActivation = false;

        while(asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
    }
}
