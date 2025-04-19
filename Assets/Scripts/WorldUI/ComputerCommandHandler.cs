using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class ComputerCommandHandler : MonoBehaviour
{
    public ComputerScreenHandler computerScreenHandler;
    public Dictionary<string, Command> commands = new Dictionary<string, Command>();

    public TMP_Text bootText;
    public TMP_Text terminalText;
    public TMP_Text upgradeText;
    public TMP_Text shopText;
    public TMP_Text logText;
    public TMP_Text timeMachineText;
    public TMP_Text bestiaryText;

    public TMP_Text dataUploadText;

    [Header("Entry strings for the various computer menus")]
    public string shopInfo;
    public string upgradeInfo;
    public string timeMachineInfo;
    public string bestiaryInfo;

    public List<string> logEntries = new List<string>
    {
        "Remember Isaac..., try to remember. Everything is spotty, our memory is clouded. Maybe yours will come back, one can only hope in this God forsaken place. All we know is the log on this computer transcends time.. and the God Particle must be stopped for all of it to be over. We leave the rest to you."
    };


    private void Awake()
    {
        RegisterEntryCommands();
        RegisterTimeMachineCommands();
        RegisterStrings();
    }

    private void RegisterEntryCommands()
    {
        commands.Add("log", new Command(
            "Log",
            null,
            new List<TMP_Text> { logText },
            new List<TMP_Text> { terminalText, shopText, upgradeText, timeMachineText, bestiaryText, dataUploadText }
        ));
        commands.Add("terminal", new Command(
            "Terminal",
            null,
            new List<TMP_Text> { terminalText },
            new List<TMP_Text> { logText, upgradeText, timeMachineText, shopText, bestiaryText, dataUploadText }
        ));
        commands.Add("shop", new Command(
            "Shop",
            null,
            new List<TMP_Text> { shopText },
            new List<TMP_Text> { terminalText, logText, upgradeText, timeMachineText, bestiaryText, dataUploadText }
        ));
        commands.Add("upgrades", new Command(
            "Upgrades",
            null,
            new List<TMP_Text> { upgradeText },
            new List<TMP_Text> { terminalText, logText, timeMachineText, shopText, bestiaryText, dataUploadText }
        ));
        commands.Add("jumper", new Command(
            "The Jumper",
            null,
            new List<TMP_Text> { timeMachineText },
            new List<TMP_Text> { terminalText, upgradeText, shopText, logText, bestiaryText, dataUploadText }
        ));
        commands.Add("bestiary", new Command(
            "Bestiary",
            null,
            new List<TMP_Text> { bestiaryText },
            new List<TMP_Text> { terminalText, upgradeText, logText, timeMachineText, shopText, dataUploadText }
        ));

    }

    private void RegisterShopCommands()
    {

    }

    private void RegisterUpgradeCommands()
    {

    }

    private void RegisterTimeMachineCommands()
    {
        commands.Add("2000", new Command(
            "Time data upload",
            null,
            new List<TMP_Text> { dataUploadText },
            new List<TMP_Text> { terminalText, upgradeText, logText, timeMachineText, shopText, bestiaryText },
            computerScreenHandler.TimeDataSequence
        ));
    }

    private void RegisterBestiaryCommands()
    {

    }

    //Enable the text element passed in and grabs the response from the command input key entered.
    public void ExecuteCommand(string input)
    {
        input = input.Trim().ToLower();

        if (commands.TryGetValue(input, out Command command))
        {
            // Disable all elements specified in the command
            foreach (var element in command.disableElements)
            {
                element.enabled = false;
            }

            // Enable all elements specified in the command
            foreach (var element in command.enableElements)
            {
                element.enabled = true;
                if (!string.IsNullOrEmpty(command.response))
                {
                    element.text = command.response;
                }
            }

            command.commandAction?.Invoke();
            Debug.Log(command.response);
        }
        else
        {
            Debug.Log("Unknown command: " + input);
        }
    }

    private void RegisterStrings()
    {
        //string init for text elements
        shopInfo = " > Flashlight : 25 Echos\n > Smokebomb : 50 Echos\n > Noisemaker : 60 Echos";
        //will need to implement a way to update the upgrade info based on if the user has bought the upgrade already
        //For example if we buy the initial stamina upgrade it should update to the next stamina upgrade thats more expensive.
        upgradeInfo = " > Stamina : 40 Echos\n > Rewind Charge : 75 Echos\n > Rewind Duration : 100 Echos\n > Rewind Cooldown : 150 Echos";
        timeMachineInfo = " > Enter a year between 2056 - 2101 to prime The Jumper\n  <Warning : Priming the jumper will trigger anomoly B, REMAIN CAUTIOUS DURING THE PRIMING SEQUENCE USE THE CAMERA ON THE OTHER TERMINAL TO YOUR ADVANTAGE.> ";
        bestiaryInfo = " > Anomolie(s) haven't been encountered";
    }




}

public struct Command
{
    public string name;
    public string response;
    public List<TMP_Text> enableElements;
    public List<TMP_Text> disableElements;
    public Action commandAction; // Function to execute when the command is run

    public Command(string name, string response, List<TMP_Text> enableElements, List<TMP_Text> disableElements, Action commandAction = null)
    {
        this.name = name;
        this.response = response;
        this.enableElements = enableElements;
        this.disableElements = disableElements;
        this.commandAction = commandAction;
    }
}
