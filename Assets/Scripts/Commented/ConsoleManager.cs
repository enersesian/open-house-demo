using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConsoleManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance of the ConsoleManager
    /// </summary>
    public static ConsoleManager instance;

    /// <summary>
    /// The ship that is being controlled
    /// </summary>
    [SerializeField]
    private Ship ship;

    [SerializeField]
    private Ship simShip;
    /// <summary>
    /// The dial that controls the ship's rotation
    /// </summary>
    [SerializeField]
    private RotationDial dial;

    /// <summary>
    /// The lever that controls how far the ship will move
    /// </summary>
    [SerializeField]
    private DistanceLever distanceLever;

    /// <summary>
    /// Reference to the commands history
    /// </summary>
    [SerializeField]
    private CommandHistory commandHistory;

    /// <summary>
    /// The grid part of the radar screen
    /// </summary>
    [SerializeField]
    private Image grid;

    /// <summary>
    /// The spinning part of the radar screen
    /// </summary>
    [SerializeField]
    private Image spinner;

    /// <summary>
    /// Reference to the HUD that pops up in front of the player, used to display messages
    /// </summary>
    [SerializeField]
    private MessagesPopup messageBoard;

    /// <summary>
    /// Bool checking to see if you are in the intro scene
    /// </summary>
    [SerializeField]
    private bool introScene;


    /// <summary>
    /// Int used to keep track of command postion
    /// </summary>
    private int commandIndex;

    /// <summary>
    /// float to keep track of time
    /// </summary>
    public float timer;

    /// <summary>
    /// Get/Set for the Dial transform
    /// </summary>
    public Transform Dial
    {
        get { return dial.transform; }
    }

    /// <summary>
    /// Get/Set for the ship's transform
    /// </summary>
    public Transform Ship
    {
        get { return ship.transform; }
    }

    /// <summary>
    /// Get/Set for the Lever transform
    /// </summary>
    public Transform DistanceLever
    {
        get { return distanceLever.transform; }
    }

    /// <summary>
    /// Get/Set bool to see if the player is scrolling through commands
    /// </summary>
    public bool ScrollingCommands
    {
        get;
        set;
    }

    /// <summary>
    /// Used to set what type of movement a command is
    /// </summary>
    public enum CommandType
    {
        Rotate, Translate
    }

    /// <summary>
    /// Struct used to reprsent a command a player has given to the "ship"
    /// </summary>
    public struct Command
    {
        public CommandType commandType;
        public float amount;

        public Command(CommandType cType, float value)
        {
            commandType = cType;
            amount = value;
        }
    }
    /// <summary>
    /// Used to keep track of the state of the player's inputs
    /// </summary>
    private enum State { Default, Executing, Simulating, CheckForError, CompilationError, CompilationSuccess }
    /// <summary>
    /// The current state of the player's input
    /// </summary>
    private State currentState;
    /// <summary>
    /// Reference to the execute/compile button in the scene
    /// </summary>
    private GameObject executeCompileButton;
    /// <summary>
    /// Reference to the mesh render of the execute/compile button
    /// </summary>
    private MeshRenderer ecButtonRenderer;
    /// <summary>
    /// Refrence to the audio source in the scene for the background music
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Reference tot he ships transform
    /// </summary>
    [SerializeField]
    Transform shipTransform;
    
    /// <summary>
    /// The ships rotation
    /// </summary>
    Vector3 shipRotation;
    /// <summary>
    /// Boolean represent whether or not the dial is being turned
    /// </summary>
    public bool TurningDial
    {
        get { return dial.TurningDial; }
        set
        {
            dial.TurningDial = value;
            if (dial.TurningDial == false)
            {
                AddCommand(CommandType.Rotate, dial.RotateAmount);
            }
        }
    }
    /// <summary>
    /// Boolena represent wheter or not the lever is being moved
    /// </summary>
    public bool ChoosingDistance
    {
        get { return distanceLever.ChoosingDistance; }
        set
        {
            distanceLever.ChoosingDistance = value;
            if (distanceLever.ChoosingDistance == false)
            {
                AddCommand(CommandType.Translate, distanceLever.MaxDistance);
            }
        }
    }
    
    /// <summary>
    /// List of the commands the palyer ahs entered
    /// </summary>
    private List<Command> commandList;

    /// <summary>
    /// Used for initialization
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            commandList = new List<Command>();
            currentState = State.Default;
            executeCompileButton = GameObject.FindGameObjectWithTag("ExecuteCompileButton");
            ecButtonRenderer = executeCompileButton.GetComponent<MeshRenderer>();
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (introScene)
        {
            return;
        }

        if (currentState == State.CheckForError)
        {
            SuccessCheck();
        }

        if (currentState == State.CompilationError)
        {
            timer = timer + Time.deltaTime;

            if (timer >= 3.0f)
            {
                currentState = State.Default;
                timer = 0.0f;
            }

        }

    }

    /// <summary>
    /// Displays the next part of the tutorial
    /// </summary>
    /// <param name="control">Reference to the control that was pressed</param>
    public void AdvanceTutorial(GameObject control)
    {
        if (!messageBoard.DisplayingTutorial)
        {
            return;
        }

        messageBoard.AdvanceTutorial(control);
    }

    /// <summary>
    /// Sets the state to successful if the simualtion was good
    /// </summary>
    public void SimulationSuccesful()
    {
        if (introScene)
        {
            return;
        }

        if (messageBoard.DisplayingTutorial)
        {

        }

        currentState = State.CompilationSuccess;
        StartCoroutine(ButtonFlash());
    }

    /// <summary>
    /// Flashes the execute/complie button
    /// </summary>
    /// <returns></returns>
    IEnumerator ButtonFlash()
    {
        float time = 0.0f;
        while (currentState == State.CompilationSuccess)
        {
            ecButtonRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.green, Mathf.PingPong(time, 1.0f)));
            time += 0.075f;
            yield return new WaitForSeconds(0.0f);
        }
    }
    /// <summary>
    /// Sets the state to error if the simualtion was bad
    /// </summary>
    public void SimulationFailed()
    {
        if (introScene)
        {
            return;
        }

        currentState = State.CompilationError;
        commandIndex = 0;
        StartCoroutine(GridColorChange());
    }
    /// <summary>
    /// Checks to see if the simulation was a success and sets the state accordingly
    /// </summary>
    void SuccessCheck()
    {
        if (introScene)
        {
            return;
        }

        if (currentState == State.CompilationSuccess)
        {

            return;
        }

        if (Vector3.Distance(simShip.transform.position, simShip.Goal.transform.position) > simShip.CompletionDistance)
        {
            currentState = State.CompilationError;
            commandIndex = 0;
            StartCoroutine(GridColorChange());
        }
        else
        {
            currentState = State.CompilationSuccess;
        }
    }

    /// <summary>
    /// Changes the color of the grid if the simulation failed and resets the simualtion ship
    /// </summary>
    /// <returns></returns>
    IEnumerator GridColorChange()
    {
        while (currentState == State.CompilationError)
        {
            if (grid.color.g == 1)
            {
                Color col = new Color();
                col = Color.red;
                col.a = 0.5f;
                grid.color = col;
                spinner.color = col;

            }
            else if (grid.color.r == 1)
            {
                Color col = new Color();
                col = Color.green;
                col.a = .25f;
                grid.color = col;
                spinner.color = col;
            }

            yield return new WaitForSeconds(0.5f);
        }

        simShip.transform.position = simShip.OriginalPosition;
        simShip.transform.rotation = simShip.OriginalRotation;
        simShip.gameObject.GetComponent<TrailRenderer>().time = 0;
    }
    /// <summary>
    /// Adds a command to the commandList
    /// </summary>
    /// <param name="commandType">The type of movement the command will do</param>
    /// <param name="amount">The amount of which the ship will move</param>
    private void AddCommand(CommandType commandType, float amount)
    {
        if (introScene)
        {
            return;
        }

        if (currentState == State.CompilationSuccess)
        {
            return;
        }
        commandList.Add(new Command(commandType, amount));
        commandHistory.AddCommand(commandType, amount);
        KeeptrackForHelp();
    }
    /// <summary>
    /// Deletes the selected command from the command list
    /// </summary>
    public void DeleteSelectedCommand()
    {
        audioSource.Play();

        if (introScene)
        {
            return;
        }

        if (currentState == State.CompilationSuccess)
        {
            return;
        }
        if (commandList.Count == 0)
        {
            return;
        }
        commandList.RemoveAt(commandHistory.SelectedIndex);
        commandHistory.DeleteSelectedElement();
        KeeptrackForHelp();
    }
    /// <summary>
    /// Clears the commands from the list
    /// </summary>
    public void ClearCommands()
    {
        audioSource.Play();

        if (introScene)
        {
            return;
        }

        if (currentState == State.CompilationSuccess)
        {
            return;
        }
        if (commandList.Count == 0)
        {
            return;
        }

        commandHistory.ClearCommands();
        commandList.Clear();
    }
    /// <summary>
    /// Displays the final message to the player
    /// </summary>
    public void ShowFinalMessage()
    {
        if (introScene)
        {
            return;
        }

        messageBoard.MessageToDisplay("Great job, now head back to home base.");
        messageBoard.Open();
        StartCoroutine(WaitToEnd());
    }
    /// <summary>
    /// Waits 5 seconds to complete the level
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(5.0f);
        LevelManager.instance.LevelCompleted = true;
    }

    /// <summary>
    /// Displays a helpful hint to the player on the HUD
    /// </summary>
    public void RequestHelp()
    {
        audioSource.Play();

        if (introScene)
        {
            return;
        }

        if (messageBoard.DisplayingTutorial)
        {
            return;
        }

        Vector3 tarDir = ship.Goal.transform.position - shipTransform.position;
        Vector3 crossPro = Vector3.Cross(shipTransform.transform.forward, tarDir);
        float rlResult = Vector3.Dot(crossPro, shipTransform.transform.up);
        rlResult = (Mathf.Round(rlResult * 100)) / 100;
        float angle = Vector3.Angle(shipTransform.transform.forward, tarDir);

        float distance = Vector3.Distance(ship.Goal.transform.position, shipTransform.position);

        float rand = Random.Range(0.0f, 1.0f);
        if (rand > 0.5)
        {
            messageBoard.MessageToDisplay(messageBoard.helpRotation + (int)angle + " degrees");
        }
        else
        {
            messageBoard.MessageToDisplay(messageBoard.helpDistance + (int)distance + " units");
        }
        messageBoard.Open();
    }

    /// <summary>
    /// Used to simulate the ship as the player enters commands to keep track of it so it can give help when requested
    /// </summary>
    public void KeeptrackForHelp()
    {
        if (introScene)
        {
            return;
        }

        if (messageBoard.DisplayingTutorial)
        {
            return;
        }

        shipTransform.position = simShip.transform.position;
        shipTransform.eulerAngles = simShip.transform.eulerAngles;

        if (commandList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < commandList.Count; i++)
        {
            if (commandList[i].commandType == CommandType.Translate)
            {
                simShip.transform.eulerAngles = shipTransform.eulerAngles;
                shipTransform.position = shipTransform.position + simShip.transform.forward * commandList[i].amount;
                simShip.transform.eulerAngles = ship.transform.eulerAngles;
            }
            else if (commandList[i].commandType == CommandType.Rotate)
            {
                shipRotation.y += commandList[i].amount;
                shipTransform.eulerAngles = shipRotation;
            }
        }

    }
    /// <summary>
    /// Displays how much the player was off by in terms of angle and distance when a simulation fails
    /// Not currently being used
    /// </summary>
    public void ErrorMessage()
    {
        shipTransform.position = simShip.transform.position;
        shipTransform.eulerAngles = simShip.transform.eulerAngles;

        Vector3 tarDir = ship.Goal.transform.position - shipTransform.position;
        Vector3 crossPro = Vector3.Cross(shipTransform.transform.forward, tarDir);
        float rlResult = Vector3.Dot(crossPro, shipTransform.transform.up);
        rlResult = (Mathf.Round(rlResult * 100)) / 100;
        float angle = Vector3.Angle(shipTransform.transform.forward, tarDir);

        float distance = Vector3.Distance(ship.Goal.transform.position, shipTransform.position);

        messageBoard.MessageToDisplay(messageBoard.helpRotation + " " + angle + "\n" + messageBoard.helpDistance + " " + distance);
    }

    /// <summary>
    /// Begins rotating the ship if the dial has stopped being turned
    /// </summary>
    /// <param name="currentShip">The ship that needs to be rotated</param>
    /// <param name="amount">The amount the ship will rotate</param>
    public void RotateShip(Ship currentShip, float amount)
    {
        if (introScene)
        {
            return;
        }

        if (!dial.TurningDial)
        {
            currentShip.BeginRotatingShip(amount);
        }
    }

    /// <summary>
    /// Scrolls through the command history by a specific distance
    /// </summary>
    /// <param name="distance">How far to scroll the commands</param>
    public void ScrollCommands(float distance)
    {
        if (introScene)
        {
            return;
        }

        commandHistory.SelectedIndex += (int)Mathf.Sign(distance);
    }

    /// <summary>
    /// Used to translate the ship by giving the BegineTranslatiingShip method the appropriate parameters
    /// </summary>
    /// <param name="currentShip">The current ship that will be translated</param>
    /// <param name="amount">The amount the ship will be translated</param>
    public void TranslateShip(Ship currentShip, float amount)
    {
        if (introScene)
        {
            return;
        }

        simShip.gameObject.GetComponent<TrailRenderer>().time = Mathf.Infinity;
        currentShip.BeginTranslatingShip(currentShip.transform.forward, amount);
    }

    /// <summary>
    /// Executes the command list by starting the coroutine InterpretCommands
    /// </summary>
    public void ExecuteCommands()
    {
        audioSource.Play();

        if (introScene)
        {
            return;
        }

        if (messageBoard.DisplayingTutorial)
        {
            return;
        }
        currentState = State.Executing;
        commandHistory.SelectedIndex = 0;
        StartCoroutine(InterpretCommands());
    }
    /// <summary>
    /// Used to simulate the current entered commands to see if the player makes it
    /// </summary>
    public void Simulate()
    {
        audioSource.Play();

        if (introScene)
        {
            return;
        }

        if (currentState != State.Default && currentState != State.CompilationSuccess)
        {
            return;
        }
        commandIndex = 0;

        simShip.transform.position = simShip.OriginalPosition;
        simShip.transform.rotation = simShip.OriginalRotation;
        simShip.gameObject.GetComponent<TrailRenderer>().time = 0;

        Debug.Log(simShip.gameObject.GetComponent<TrailRenderer>().time);

        if (currentState == State.CompilationSuccess)
        {
            simShip.gameObject.GetComponent<TrailRenderer>().enabled = false;
            ExecuteCommands();
            return;
        }

        currentState = State.Simulating;
  
        StartCoroutine(InterpretCommands());
    }
    /// <summary>
    /// Stops the simulation
    /// </summary>
    public void StopSimulating()
    {
        if (introScene)
        {
            return;
        }

        currentState = State.Default;
    }

    /// <summary>
    /// Goes through the commands in the command list one by one and moves the ship according to the command type and value
    /// </summary>
    /// <returns></returns>
    IEnumerator InterpretCommands()
    {
        Ship currentShip = currentState == State.Simulating ? simShip : ship;

        yield return new WaitForSeconds(0.2f);

        while (commandIndex < commandList.Count && (currentState == State.Simulating || currentState == State.Executing))
        {
            yield return new WaitUntil(() => currentShip.Done == true);

            Command command = commandList[commandIndex];

            if (currentState == State.Executing)
            {
                commandList.RemoveAt(0);
                commandHistory.DeleteElement(0);
            }
            else if (currentState == State.Simulating)
            {
                commandIndex++;
            }

            switch (command.commandType)
            {
                case CommandType.Translate:
                    TranslateShip(currentShip, command.amount);
                    break;

                case CommandType.Rotate:
                    RotateShip(currentShip, command.amount);
                    break;
            }
        }

        yield return new WaitUntil(() => currentShip.Done == true);

        if (currentState == State.Simulating)
        {
            if (messageBoard.DisplayingTutorial)
            {
                currentState = State.Default;
                simShip.transform.position = simShip.OriginalPosition;
                simShip.transform.rotation = simShip.OriginalRotation;
                simShip.gameObject.GetComponent<TrailRenderer>().time = 0;
            }
            else
            {
                currentState = State.CheckForError;
            }
        }
        else if (currentState == State.Executing)
        {
            currentState = State.Default;
            simShip.transform.position = simShip.OriginalPosition;
            simShip.transform.rotation = simShip.OriginalRotation;
            simShip.gameObject.GetComponent<TrailRenderer>().time = 0;
        }
    }
}
