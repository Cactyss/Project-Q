using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using System.Security.Cryptography.X509Certificates;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    private Story globalVariablesStory;
    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        // create the story
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        // if we have saved data, load it on startup
        if (true)//if saveload system has saved data to load on startup
        {
            string jsonState = null; //TODO: set jsonState to the data
            //TODO: save the INK variables data to data system
            // globalVariablesStory.state.LoadJson(jsonState); //TODO: remove the comment after jsonState is implemented
        }

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }
    public void StartListening(Story story)
    {
        //important that VariablesToStory is called before the listener is assigned!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }
    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
        else
        {
            Debug.LogWarning("idk how but DialogueVariables tried to load a variable we don't have defined already: " + name + " set to: " + value);
        }

    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
    
    public void SaveVariables()
    {
        if (globalVariablesStory != null)
        {
            //load the current state ofa ll our variables to the globals stroy
            // VariablesToStory(globalVariablesStory);
            //TODO: here we save the globalVariablesStory.state.ToJson() to our save system
        }
    }
}
