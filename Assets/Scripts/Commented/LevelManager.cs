using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Variable to control how fast the screen fades to black after a level is completed
    /// </summary>
    [SerializeField]
    private float fadeSpeed;
    /// <summary>
    /// The time to wait after each screen fade increment (used for fade coroutine)
    /// </summary>
    [SerializeField]
    private float waitTime;
    /// <summary>
    /// Reference to the image that will fade to black and be overlayed on the screen
    /// </summary>
    [SerializeField]
    private Image screenOverlay;
    /// <summary>
    /// Boolean representing whether or not the screen fade has begun
    /// </summary>
    private bool begin;
    /// <summary>
    /// The LevelManager singleton instance
    /// </summary>
    public static LevelManager instance;
    /// <summary>
    /// A boolean representing whether or not the current level is completed
    /// </summary>
    public bool LevelCompleted
    {
        get;
        set;
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        OVRManager.display.displayFrequency = 72.0f;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (LevelCompleted && !begin)
        {
            begin = true;
            StartCoroutine(EndLevel());
        }
    }

    /// <summary>
    /// Coroutine that causes the screen to fade to black
    /// </summary>
    /// <returns> Returns an IEnumerator object </returns>
    IEnumerator EndLevel()
    {
        Color startingColor = screenOverlay.color;
        float time = 0.0f;
        while (screenOverlay.color.a < 1.0f)
        {
            time += fadeSpeed;
            screenOverlay.color = Color.Lerp(startingColor, Color.black, time);
            yield return new WaitForSeconds(waitTime);
        }

        int index = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(index > 3 ? 0 : index);
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
