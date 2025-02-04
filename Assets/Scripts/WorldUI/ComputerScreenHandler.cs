using TMPro;
using UnityEngine;

public class ComputerScreenHandler : MonoBehaviour
{
    public TextMeshProUGUI computerDisplay;
    public GameObject subScreenLight;
    public GameObject mainScreenLight;
    private GameObject computerHolder;
    private AudioSource computerSFXSource;
    public CameraTransitionHandler camTransitionHandler;

    public bool isTurnedOn = false;
    public KeyCode interactKey = KeyCode.E;

    private string currentCommand = "";
    private string initialText = "TallyHo20Pro - Model 7 - Unit 731\nSerial: 5AB789JB\nWelcome to Tally Ho! Enter !help to display commands.";

    public Material screenEmissiveMaterial;

    private void Start()
    {
        if (screenEmissiveMaterial == null)
        {
            Debug.LogError("ComputerScreenHandler: Screen Emissive Material is not assigned!");
        }
        screenEmissiveMaterial.DisableKeyword("_EMISSION");
        GetRefs();
        computerSFXSource.enabled = false;

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

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentCommand.Length > 0)
                    currentCommand = currentCommand.Substring(0, currentCommand.Length);

            }
            else if (c == '\n' || c == '\r')
            {
                Debug.Log("Command Entered: " + currentCommand);
                currentCommand = "";
            }
            else
            {
                currentCommand += c;
            }
        }
        computerDisplay.text = initialText + currentCommand;
    }

    public void TurnOnComputer()
    {   

        camTransitionHandler.StartSwitchToTargetCam(.6f);
        computerSFXSource.enabled = true;
        computerSFXSource.Play();

        isTurnedOn = true;
        computerDisplay.text = "";
        if (!subScreenLight.activeSelf && !mainScreenLight.activeSelf)
        {
            subScreenLight.SetActive(true);
            mainScreenLight.SetActive(true);
        }

        if (screenEmissiveMaterial != null && !screenEmissiveMaterial.IsKeywordEnabled("_EMISSION"))
        {
            screenEmissiveMaterial.EnableKeyword("_EMISSION");
        }

    }

    public void TurnOffComputer()
    {   
        computerSFXSource.enabled = false;
        camTransitionHandler.StartSwitchToPlayerCam(.6f);
        isTurnedOn = false;
        computerDisplay.text = "";
        if (subScreenLight.activeSelf && mainScreenLight.activeSelf)
        {
            subScreenLight.SetActive(false);
            mainScreenLight.SetActive(false);
        }

        if (screenEmissiveMaterial != null && screenEmissiveMaterial.IsKeywordEnabled("_EMISSION"))
        {
            screenEmissiveMaterial.DisableKeyword("_EMISSION");
        }
        
    }
}
