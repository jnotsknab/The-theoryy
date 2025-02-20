using TMPro;
using UnityEngine;
using System.Collections;

public class ComputerScreenHandler : MonoBehaviour
{
    [SerializeField] ComputerCommandHandler commandHandler;
    [SerializeField] AudioHandler audioHandler = new AudioHandler();

    [Header("Display Stuff")]
    public TextMeshProUGUI computerDisplay;
    private GameObject computerHolder;
    //public GameObject turnOffUI;
    public GameObject hudViewPanel;
    public GameObject securityFeed;

    [Header("Audio Stuff")]
    private AudioSource computerSFXSource;
    public AudioClip powerOffClip;
    public AudioClip powerOnClip;
    public AudioClip keyboardClip;
    public CameraTransitionHandler camTransitionHandler;

    public bool isTurnedOn = false;
    private bool canInteract = true;

    [Header("Cursor/Carat Stuff")]
    [SerializeField] private bool showCursor = true;
    [SerializeField] public float cursorBlinkRate;
    [SerializeField] private Coroutine cursorRoutine;


    public Material screenMat;
    public GameObject computerLight;

    private string currentCommand = "";
    private string initialText = " > Log\n\n > Shop\n\n > Upgrades\n\n > Bestiary\n\n > The Jumper\n\n Command: ";

    private string[] bootMessages = new string[]
    {
        "JCC, An Era Corp Ally.\nCopyright (C) XXXX - XXXX, Era Corp Inc.\nEphem 500 CPU at 1500MHz, 2 Processor(s)\nMemory Test : 0x5AB78980",
        "  Detecting Flash ROM    ... CreonOS 2\n  Detecting Flash Extenstion    ... Generic microSD\n  Detecting Storage    ... Generic HardDrive",
        " Cache Memory : 10485K\n Memory Installed : 256MB, ROM\n Storage Avaliable : 336MB, DISC\n Display Type   : True CRT - LCD Hybrid\n Serial Port(s) : F2P A2DP\n Parallel Port(s) : 278\n Time Warp Module(s) : Yes"
    };


    private void Awake()
    {
        if (screenMat != null)
        {
            screenMat.SetFloat("_Intensity", 0f);
        }
        else
        {
            Debug.LogError("Material for computer screen is null or has not been assigned.");
        }

        GetRefs();
        DisableComputerAudio();

        //Disable all screens except for bootup screen
        DisableAllScreens();
        EnableScreen(commandHandler.bootText);


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

        if (commandHandler.terminalText.enabled)
        {
            MainTerminalInput();
        }
        else if (commandHandler.logText.enabled)
        {
            LogInput();
        }
        else if (commandHandler.shopText.enabled)
        {
            ShopInput();
        }
        else if (commandHandler.upgradeText.enabled)
        {
            UpgradesInput();
        }
        else if (commandHandler.timeMachineText.enabled)
        {
            TimeMachineInput();
        }
        else if (commandHandler.bestiaryText.enabled)
        {
            BestiaryInput();
        }

        //TestTerminal();

    }

    private IEnumerator BootUpSequence()
    {
        canInteract = false;
        foreach (string message in bootMessages)
        {
            // Clear the screen before displaying the next message
            commandHandler.bootText.text = "";

            // Show the typing animation for the message
            yield return StartCoroutine(TypeText(message));

            // Pause for a short time before transitioning to the next message
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
        DisableScreen(commandHandler.bootText);

        //Testing for now later just call another coroutine here that activates the terminal.
        //Current bug, if player turns computer off before this routine finishes the terminal text will be activated even if the computer is off.
        EnableScreen(commandHandler.terminalText);



    }

    private IEnumerator TypeText(string message)
    {
        foreach (char letter in message)
        {
            commandHandler.bootText.text = commandHandler.bootText.text.TrimEnd('_') + letter + "_"; // Keep cursor at the end
            yield return new WaitForSeconds(0.022f);
        }
    }



    public void TurnOnComputer()
    {
        if (canInteract && !isTurnedOn)
        {   
            hudViewPanel.SetActive(false);
            //turnOffUI.SetActive(true);
            computerLight.SetActive(true);
            commandHandler.terminalText.enabled = false;
            EnableScreen(commandHandler.bootText);
            StartCoroutine(BootUpSequence());
            screenMat.SetFloat("_Intensity", 0.5f);
            camTransitionHandler.StartSwitchToTargetCam(.6f);
            computerSFXSource.enabled = true;
            computerSFXSource.clip = powerOnClip;
            computerSFXSource.Play();
            securityFeed.SetActive(true);
            isTurnedOn = true;
            computerDisplay.text = "";
        }

    }

    public void TurnOffComputer()
    {
        if (canInteract && isTurnedOn)
        {
            hudViewPanel.SetActive(true);
            //turnOffUI.SetActive(false);
            computerLight.SetActive(false);

            screenMat.SetFloat("_Intensity", 0f);
            camTransitionHandler.StartSwitchToPlayerCam(.6f);

            computerSFXSource.clip = powerOffClip;
            computerSFXSource.Play();
            securityFeed.SetActive(false);
            isTurnedOn = false;
            computerDisplay.text = "";

            //Ensures we dont cut off the turn off audio before it finishes.
            Invoke(nameof(DisableComputerAudio), computerSFXSource.clip.length);
            DisableAllScreens();
        }
    }


    private IEnumerator CursorBlink()
    {
        while (true)
        {
            showCursor = !showCursor;
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }

    private void EnableScreen(TMP_Text screen, bool blinkingCursor = true)
    {
        if (blinkingCursor && cursorRoutine == null) cursorRoutine = StartCoroutine(CursorBlink());

        if (screen != null) screen.enabled = true;

    }

    private void DisableScreen(TMP_Text screen)
    {
        if (cursorRoutine != null)
        {
            StopCoroutine(cursorRoutine);
            cursorRoutine = null;
        }
        if (screen != null) screen.enabled = false;
    }

    private void DisableAllScreens()
    {
        commandHandler.bootText.enabled = false;
        commandHandler.terminalText.enabled = false;
        commandHandler.shopText.enabled = false;
        commandHandler.logText.enabled = false;
        commandHandler.upgradeText.enabled = false;
        commandHandler.timeMachineText.enabled = false;
        commandHandler.bestiaryText.enabled = false;
    }

    //Only needed for disable as we use invoke and we have to pass a function to it.
    private void DisableComputerAudio()
    {
        computerSFXSource.enabled = false;
    }

    //Handles all command input for the entry terminal after the boot sequence.
    private void MainTerminalInput()
    {
        //Set the audioclip for the source to play keyboard sounds
        if (computerSFXSource.clip != keyboardClip) computerSFXSource.clip = keyboardClip;
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        commandHandler.terminalText.text = initialText + currentCommand + cursor;
    }

    private void LogInput()
    {
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        //Later on create a set of enums somewhere to determine if player is in early, mid, late game, etc and adjust reponse based on result.
        commandHandler.logText.text = commandHandler.logEntries[0] + $"\n\n Command <Terminal> to return: {currentCommand}" + cursor;
    }

    private void ShopInput()
    {
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        commandHandler.shopText.text = commandHandler.shopInfo + $"\n\n Command <ItemName> or <Terminal>: {currentCommand}" + cursor;
    }

    private void UpgradesInput()
    {
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        commandHandler.upgradeText.text = commandHandler.upgradeInfo + $"\n\n Command <UpgradeName> or <Terminal>: {currentCommand}" + cursor;
    }

    private void TimeMachineInput()
    {
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        commandHandler.timeMachineText.text = commandHandler.timeMachineInfo + $"\n\n Command <Year>: {currentCommand}" + cursor;
    }

    private void BestiaryInput()
    {
        ExecInput();
        string cursor = showCursor ? "_" : " ";
        commandHandler.bestiaryText.text = commandHandler.bestiaryInfo + $"\n\n Command <AnomolyName> or <Terminal>: {currentCommand}" + cursor;
    }
    private void ExecInput()
    {
        foreach (char c in Input.inputString)
        {
            audioHandler.PlaySource(computerSFXSource, true, true, 0.9f, 1.1f, 0.25f, 0.4f);
            if (c == '\b')
            {
                if (currentCommand.Length > 0)
                    currentCommand = currentCommand.Substring(0, currentCommand.Length - 1);

            }
            else if (c == '\n' || c == '\r')
            {
                Debug.Log("Command Entered: " + currentCommand);
                commandHandler.ExecuteCommand(currentCommand);
                currentCommand = "";
            }
            else
            {
                currentCommand += c;
            }
        }
    }
}
