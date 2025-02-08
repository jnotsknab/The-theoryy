using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerCommandHandler
{
    private Dictionary<string, Command> commands = new Dictionary<string, Command>();

    public List<string> logResponses = new List<string>
    {
        "Remember Isaac..., remember why you are here, how you got here, your purpose.. The God Particle must be stopped."
    };

    public ComputerCommandHandler()
    {
        RegisterCommands();
    }

    

    //For Log Populate response value in each dict with a index from a list of strings depending on progress in the game.
    private void RegisterCommands()
    {
        commands.Add("log", new Command("Log", logResponses[0]));
    }

    public void ExecuteCommand(string input, TMP_Text textElement)
    {
        input = input.Trim().ToLower();

        if (commands.TryGetValue(input, out Command command))
        {
            Debug.Log(command.response);
            textElement.text += command.response;
        }
        else
        {
            Debug.Log("Unknown command: " + input);
        }
    }


    
}

public struct Command
{
    public string name; public string response;

    public Command(string name, string response)
    {
        this.name = name;
        this.response = response;

    }
}
