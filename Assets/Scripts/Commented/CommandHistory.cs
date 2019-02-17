using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandHistory : MonoBehaviour
{
    /// <summary>
    /// The starting position of the command history list
    /// </summary>
    public Transform startPos;

    /// <summary>
    /// The ending position of the command history list
    /// </summary>
    public Transform endPos;

    /// <summary>
    /// The beginning position of the visible region
    /// </summary>
    public Transform startOfVisibleRegion;

    /// <summary>
    /// The ending position of the visible region
    /// </summary>
    public Transform endOfVisibleRegion;

    /// <summary>
    /// Prefab for command game object
    /// </summary>
    public Transform commandText;

    /// <summary>
    /// The index of the currently selected command
    /// </summary>
    private int selectedIndex;

    /// <summary>
    /// The command that was previously selected
    /// </summary>
    private Transform previousCommand;

    /// <summary>
    /// Boolean representing whether or not the commands are currently being scrolled through
    /// </summary>
    private bool scrolling;

    /// <summary>
    /// The command that is currently selected
    /// </summary>
    public Transform SelectedCommand
    {
        get
        {
            if (transform.childCount == 0)
            {
                return null;
            }

            int index = SelectedIndex;
            index = Mathf.Clamp(SelectedIndex, 0, transform.childCount - 1);
            return transform.GetChild(index);
        }
    }

    /// <summary>
    /// A list to hold all of the commands
    /// </summary>
    private List<Transform> commandList;

    /// <summary>
    /// The height of the command text prefab
    /// </summary>
    private float childHeight;

    /// <summary>
    /// The initial local position of the command history object
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// Boolean representing whether or not the command history list is at the start position
    /// </summary>
    public bool AtBeginning
    {
        get { return transform.position.y <= startPos.position.y; }
    }

    /// <summary>
    /// Boolean representing whether or not the command history list is at the end position
    /// </summary>
    public bool AtEnd
    {
        get { return transform.position.y >= endPos.position.y; }
    }

    /// <summary>
    /// The index of the currently selected command
    /// </summary>
    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }

        set
        {
            if (transform.childCount == 0)
            {
                return;
            }
            previousCommand = SelectedCommand;
            previousCommand.GetComponent<TextMeshProUGUI>().color = Color.white;

            selectedIndex = Mathf.Clamp(value, 0, transform.childCount - 1);
            SelectedCommand.GetComponent<TextMeshProUGUI>().color = Color.green;
            StartCoroutine(Scroll());
        }
    }

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Start()
    {
        originalPosition = transform.localPosition;
        commandList = new List<Transform>();
    }

    /// <summary>
    /// Adds a new command to the list of commands
    /// </summary>
    /// <param name="commandType"> The command's type </param>
    /// <param name="amount"> The amount the command should be applied </param>
    public void AddCommand(ConsoleManager.CommandType commandType, float amount)
    {
        Transform newCommand = null;
        if (transform.childCount > 0)
        {
            Transform lastChild = transform.GetChild(transform.childCount - 1);

            if (childHeight == 0)
            {
                childHeight = lastChild.GetComponent<RectTransform>().rect.height;
            }
            newCommand = Instantiate(commandText, transform);
            newCommand.localPosition = lastChild.localPosition + Vector3.up * -(childHeight * lastChild.localScale.y);
            newCommand.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            newCommand = Instantiate(commandText, transform);
            SelectedCommand.GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        newCommand.GetComponent<TextMeshProUGUI>().text = commandType.ToString() + " " + amount;

        commandList.Add(newCommand);
        SelectedIndex = commandList.Count - 1;
    }

    /// <summary>
    /// Deletes an element from the list of commands
    /// </summary>
    /// <param name="index">The index of the element to delete</param>
    public void DeleteElement(int index)
    {
        if (commandList.Count == 0)
        {
            return;
        }

        for (int i = index + 1; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition += Vector3.up * (childHeight * child.localScale.y);
        }

        GameObject coml = commandList[index].gameObject;
        coml.transform.SetParent(null);
        commandList.RemoveAt(index);
        Destroy(coml);

        SelectedIndex = index >= commandList.Count ? commandList.Count - 1 : index;
    }

    /// <summary>
    /// Deletes the currently selected element
    /// </summary>
    public void DeleteSelectedElement()
    {
        DeleteElement(SelectedIndex);
    }

    /// <summary>
    /// Deletes all of the commands in the list
    /// </summary>
    public void ClearCommands()
    {
        if (commandList.Count == 0)
        {
            return;
        }

        transform.DetachChildren();

        for (int i = 0; i < commandList.Count; i++)
        {
            Destroy(commandList[i].gameObject);
        }

        commandList.Clear();
        transform.localPosition = originalPosition;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
    }

    /// <summary>
    /// Scrolls the history list to make certain commands visible 
    /// </summary>
    /// <returns>Returns IEnumerator object</returns>
    IEnumerator Scroll()
    {
        yield return new WaitUntil(() => scrolling == false);

        Vector3 targetPos = Vector3.zero;

        Vector3 localPosStart = startOfVisibleRegion.InverseTransformPoint(SelectedCommand.position);
        Vector3 localPosEnd = endOfVisibleRegion.InverseTransformPoint(SelectedCommand.position);

        if (localPosStart.y > 0)
        {
            scrolling = true;
            targetPos = transform.localPosition - Vector3.up * Vector3.Distance(SelectedCommand.position, startOfVisibleRegion.position - Vector3.up * 0.2f);
        }
        else if (localPosEnd.y < 0)
        {
            scrolling = true;
            targetPos = transform.localPosition + Vector3.up * Vector3.Distance(SelectedCommand.position, endOfVisibleRegion.position + Vector3.up * 0.2f);
        }

        while (scrolling)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 0.075f);

            if (Vector3.Distance(transform.localPosition, targetPos) <= 0.02f)
            {
                scrolling = false;
                transform.localPosition = targetPos;
            }

            yield return new WaitForSeconds(0.0f);
        }
    }
}
