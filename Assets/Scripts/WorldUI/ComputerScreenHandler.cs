using System.Collections;
using TMPro;
using UnityEngine;

public class ComputerScreenHandler : MonoBehaviour
{   
    private ComputerCommandHandler commandHandler = new ComputerCommandHandler();

    [Header("Display Stuff")]
    public TextMeshProUGUI computerDisplay;
    public GameObject subScreenLight;
    public GameObject mainScreenLight;
    private GameObject computerHolder;

    [Header("Audio Stuff")]
    private AudioSource computerSFXSource;
    public AudioClip powerOffClip;
    public AudioClip powerOnClip;
    public CameraTransitionHandler camTransitionHandler;

    public bool isTurnedOn = false;

    [Header("Cursor/Carat Stuff")]
    [SerializeField] private bool showCursor = true;
    [SerializeField] public float cursorBlinkRate;
    [SerializeField] private Coroutine cursorRoutine;

    //public KeyCode interactKey = KeyCode.E;

    [Header("Boot Stuff")]
    public TMP_Text screenText;
    public TMP_Text terminalText;
    public Material screenMat;
    public GameObject computerLight;

    private string currentCommand = "";
    private string initialText = " > Log\n\n > Market\n\n > Upgrades\n\n > Bestiary\n\n > The Jumper\n\n Command: ";

    private string[] bootMessages = new string[]
    {
        "JCC, An Era Corp Ally.\nCopyright (C) XXXX - XXXX, Era Corp Inc.\nEphem 500 CPU at 1500MHz, 2 Processor(s)\nMemory Test : 0x5AB78980",
        "  Detecting Flash ROM    ... CreonOS 2",
        "  Detecting Flash Extenstion    ... Generic microSD",
        "  Detecting Storage    ... Generic HardDrive",
        " Cache Memory : 10485K\n Memory Installed : 256MB, ROM\n Storage Avaliable : 336MB, DISC\n Display Type   : True CRT - LCD Hybrid\n Serial Port(s) : F2P A2DP\n Parallel Port(s) : 278\n Time Warp Module(s) : Yes"
    };


    

    private void Start()
    {
        if (screenMat == null)
        {
            Debug.LogError("ComputerScreenHandler: Screen Emissive Material is not assigned!");
        }
        screenMat.SetFloat("_Intensity", 0f);
        GetRefs();
        computerSFXSource.enabled = false;
        DisableTerminalScreen();

    }

    private void GetRefs()
    {
        computerHolder = GameObject.FindGameObjectWithTag("Computer");
        computerSFXSource = computerHolder.GetComponent<AudioSource>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTurnedOn)
            return;

        if (terminalText.enabled)
        {
            UserInput();
        }

        //TestTerminal();
        
    }

    private IEnumerator BootUpSequence()
    {   
        
        
        foreach (string message in bootMessages)
        {
            // Clear the screen before displaying the next message
            screenText.text = "";

            // Show the typing animation for the message
            yield return StartCoroutine(TypeText(message));

            // Pause for a short time before transitioning to the next message
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
        yield return new WaitForSeconds(0.5f);
        DisableBootScreen();

        //Testing for now later just call another coroutine here that activates the terminal.
        EnableTerminalScreen();
        

    }

    private IEnumerator TypeText(string message)
    {
        foreach (char letter in message)
        {
            screenText.text = screenText.text.TrimEnd('_') + letter + "_"; // Keep cursor at the end
            yield return new WaitForSeconds(0.022f);
        }
    }



    public void TurnOnComputer()
    {
        computerLight.SetActive(true);
        terminalText.enabled = false;
        EnableBootScreen();
        StartCoroutine(BootUpSequence());
        screenMat.SetFloat("_Intensity", 0.5f);
        camTransitionHandler.StartSwitchToTargetCam(.6f);
        computerSFXSource.enabled = true;
        computerSFXSource.clip = powerOnClip;
        computerSFXSource.Play();

        isTurnedOn = true;
        computerDisplay.text = "";
        if (!subScreenLight.activeSelf && !mainScreenLight.activeSelf)
        {
            subScreenLight.SetActive(true);
            mainScreenLight.SetActive(true);
        }
        



    }

    public void TurnOffComputer()
    {   
        computerLight.SetActive(false);
        DisableBootScreen();
        DisableTerminalScreen();
        screenMat.SetFloat("_Intensity", 0f);
        camTransitionHandler.StartSwitchToPlayerCam(.6f);

        computerSFXSource.clip = powerOffClip;
        computerSFXSource.Play();

        isTurnedOn = false;
        computerDisplay.text = "";
        if (subScreenLight.activeSelf && mainScreenLight.activeSelf)
        {
            subScreenLight.SetActive(false);
            mainScreenLight.SetActive(false);
        }


        
    }

    private void DisableBootScreen()
    {
        screenText.enabled = false;

    }

    private void EnableBootScreen()
    {
        screenText.enabled = true;

    }

    private void EnableTerminalScreen()
    {   
        if (cursorRoutine == null) cursorRoutine = StartCoroutine(CursorBlink());
        terminalText.enabled = true;

    }

    private void DisableTerminalScreen()
    {   
        if (cursorRoutine != null)
        {
            StopCoroutine(cursorRoutine);
            cursorRoutine = null;
        }
        terminalText.enabled = false;

    }

    //Instead of creating many tmp elements lets instead clear the screen when a command is entered and then repopulate the screen with the correct text
    private void UserInput()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentCommand.Length > 0)
                    currentCommand = currentCommand.Substring(0, currentCommand.Length - 1);

            }
            else if (c == '\n' || c == '\r')
            {   
                Debug.Log("Command Entered: " + currentCommand);
                commandHandler.ExecuteCommand(currentCommand, terminalText);
                currentCommand = "";
            }
            else
            {
                currentCommand += c;
            }
        }
        string cursor = showCursor ? "_" : " ";
        terminalText.text = initialText + currentCommand + cursor;
    }

    private IEnumerator CursorBlink()
    {
        while (true)
        {
            showCursor = !showCursor;
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }
}
