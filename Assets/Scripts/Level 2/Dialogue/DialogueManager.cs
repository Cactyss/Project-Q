using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{

    // variable for the load_globals.ink JSON
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    //custom rich text tags
    private const string DELETE_CUSTOM = "del";
    private const string WAIT_CUSTOM = "wait";
    private const string SPACE_CUSTOM = "space";

    //ink tags
    private const string SOUND_TAG = "sound";
    private const string NAME_TAG = "name";
    private const string IMAGE_TAG = "image";
    private const string LAYOUT_TAG = "layout";
    private const string UNSKIPPABLE = "unskippable";
    private const string TYPING_SPEED = "typingspeed";
    private const string AUTO_SKIP = "autoskip";
    //TODO: add ink tags (or tie to the portrait tag): sound effect, font, font color, font size, delay until next dialogue
    [Header("Parameters and Tags")]
    [SerializeField] private float defaultTypingSpeed = 0.04f;
    private float typingSpeed;
    private string defaultTypingSound = "default";
    private string currentTypingSound;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool canSkipTyping = true;
    private float autoSkip = 0f;
    // TODO: implement keybinds (this is the continue dialogue keybind)
    private KeyCode SubmitKeybind = KeyCode.Space;
    private static DialogueManager instance;
    private AnimationClip[] portraitClips;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private GameObject canContinueIcon;
    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    private String currentLine;
    public bool dialogueIsPlaying { get; private set; } = false;

    private DialogueVariables dialogueVariables;

    private void Awake()
    {
        transform.SetParent(null);
        if (instance != null)
        {
            Debug.LogWarning("multiple instances of dialogue manager in scene");
        }
        instance = this;
        // pass that variable to the DIalogueVariables constructor in the Awake method
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }
    private void Start()
    {
        typingSpeed = defaultTypingSpeed;
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        portraitClips = portraitAnimator.runtimeAnimatorController.animationClips;
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(SubmitKeybind) && currentStory.currentChoices.Count == 0 && canContinueToNextLine)
        {
            ContinueStory();
        }

    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        resetEncounterTags();
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //if a line is already being displayed, stop it
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            //do we make it go character by character right here?
            //makes currentStory skip lines that are blank

            currentLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(currentLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private IEnumerator DisplayLine(string line)
    {
        //if the line is blank we skip it because it's probably not supposed to be blank
        if (line.Trim() == "")
        {
            Debug.LogWarning("skipped a blank line");
            ContinueStory();
        }
        else
        {
            //hides certain UI while typing out dialogue
            canContinueIcon.SetActive(false);
            HideChoices();

            //dont allow player to skip onto the next line before they see the current line fully printed out
            bool skip = false;
            canContinueToNextLine = false;
            bool addedAngleBracket = true;
            bool isAddingRichTextTag = false;
            bool isAddingCustomText = false;
            StringBuilder customText = new StringBuilder();
            int bracketIndex = 0;
            int visibleCharacters = 0;

            //empty the displayed text
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;

            foreach (char letter in line.ToCharArray())
            {
                visibleCharacters++;
                //allows the dialogue to be skipped after 3 characters are displayed
                if (canSkipTyping && Input.GetKey(SubmitKeybind) && visibleCharacters > 5)
                {
                    skip = true;
                }
                if (letter == '<')
                {
                    bracketIndex++;
                    if (bracketIndex == 1)
                    {
                        isAddingRichTextTag = true;
                        addedAngleBracket = false;
                    }
                    if (bracketIndex == 2)
                    {
                        isAddingCustomText = true;
                        isAddingRichTextTag = false;
                    }
                    if (bracketIndex != 1 && bracketIndex != 2)
                    {
                        Debug.LogWarning("you typed three angle brackets in a row in an ink file, this is bad");
                    }
                }
                else if (isAddingCustomText)
                {
                    if (letter != '<' && letter != '>')
                    {
                        customText.Append(letter);
                    }
                    if (letter == '>')
                    {
                        // '>' means that the tag is fully typed out, so now we Handle Custom Tags
                        Debug.Log(customText.ToString());

                        string[] splitTag = customText.ToString().Split(':');
                        if (splitTag.Length != 2)
                        {
                            switch (customText.ToString())
                            {//if it's a one word tag then it goes to this switch statement
                                case DELETE_CUSTOM:
                                    Debug.Log("delete triggered");
                                    DisplayChoices();
                                    yield return new WaitForSeconds(0.5f);
                                    HideChoices();

                                    // Calculate the number of characters to delete
                                    int charactersToDelete = Mathf.Min(dialogueText.maxVisibleCharacters, dialogueText.text.Length);

                                    // Adjust maxVisibleCharacters to hide the portion that has already been revealed
                                    dialogueText.maxVisibleCharacters -= charactersToDelete;

                                    // Adjust the displayed text to remove the deleted portion
                                    dialogueText.text = dialogueText.text.Substring(charactersToDelete);

                                    break;
                                default:
                                    Debug.LogWarning("Tag wasn't one of the custom tags" + tag);
                                    break;
                            }
                            // Remove the custom tag from the dialogueText.text
                            dialogueText.text = dialogueText.text.Replace("<<" + customText.ToString() + ">", "");
                        }
                        else
                        {
                            string tagValue = splitTag[1].Trim();
                            string tagKey = splitTag[0].Trim();
                            switch (tagKey)
                            {//if it's a two word tag then it goes to this switch statement
                                case WAIT_CUSTOM:
                                    Debug.Log("wait triggered");
                                    yield return new WaitForSeconds(float.Parse(tagValue));
                                    break;
                                case SPACE_CUSTOM:
                                    Debug.Log("space triggered");
                                    int spaceCount = int.Parse(tagValue);
                                    // Calculate the number of characters that are currently visible
                                    int visibleChars = Mathf.Min(dialogueText.text.Length, dialogueText.maxVisibleCharacters);
                                    // Insert spaces at the point where the visible text ends
                                    dialogueText.text = dialogueText.text.Insert(visibleChars, new string(' ', spaceCount));
                                    // Increase maxVisibleCharacters to make the added spaces visible
                                    dialogueText.maxVisibleCharacters += spaceCount;
                                    break;
                                default:
                                    Debug.LogWarning("Tag wasn't one of the custom tags" + tag);
                                    break;
                            }
                            // Remove the custom tag from the dialogueText.text
                            dialogueText.text = dialogueText.text.Replace("<<" + customText.ToString() + ">", "");
                        }
                        customText = new StringBuilder();
                        bracketIndex = 0;
                        addedAngleBracket = true;
                        isAddingCustomText = false;
                        isAddingRichTextTag = false;
                    }
                }
                else if (isAddingRichTextTag)
                {//add the rich text tag info and stuff
                    if (!addedAngleBracket)
                    {
                        dialogueText.text += '<';
                        addedAngleBracket = true;
                    }
                    dialogueText.text += letter;
                    if (letter == '>')
                    {
                        customText = new StringBuilder();
                        bracketIndex = 0;
                        addedAngleBracket = true;
                        isAddingCustomText = false;
                        isAddingRichTextTag = false;
                    }
                }
                else
                {//add the letter and play a sound
                 //dialogueText.text += letter;
                    dialogueText.maxVisibleCharacters++;
                    //TODO: add a thing that makes it so a word jumps to the next line if it can't fit completely on the current line
                    if (!letter.ToString().Equals(" ") && !skip && currentTypingSound != "silent" && !isAddingCustomText)
                    {//TODO: if we are adding a letter and not skipping, play a sound
                        FindObjectOfType<AudioManager>().PlayTypingSound(currentTypingSound);
                    }
                    else
                    {
                        //TODO: maybe add here a thing for people who pause after completing a word?
                    }
                    //if we are skipping to the end, type all letters without delay
                    if (!skip) { yield return new WaitForSeconds(typingSpeed); }
                }
            }
            if (autoSkip > 0)
            {
                yield return new WaitForSeconds(autoSkip);
                ContinueStory();
            }
            // display choices if any
            DisplayChoices();
            canContinueIcon.SetActive(true);
            canContinueToNextLine = true;
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogWarning("can't handle any more choices, stopped displaying choices");
        }

        int index = 0;

        //enable and initilize the choi8ces up to the amount of choices for this story
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }
    private void HideChoices()
    {
        foreach (GameObject c in choices)
        {
            c.SetActive(false);
        }
    }

    public void MakeChoice(int i)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(i);
            ContinueStory();
        }
    }
    private void resetEncounterTags()
    {
        //reset typing sound
        currentTypingSound = defaultTypingSound;
        //reset dialouge  UI
        displayNameText.text = "???";
        //portraitAnimator.Play("default");
        layoutAnimator.Play("right");
        //reset image
        portraitAnimator.Play("default");

    }
    private void resetLineTags()
    {
        //default dont autoskip (0 means don't autoskip)
        autoSkip = 0f;
        //reset typing speed
        typingSpeed = defaultTypingSpeed;
        //can skip by default
        canSkipTyping = true;
    }
    private void HandleTags(List<String> tags)
    {
        resetLineTags();
        foreach (string tag in tags)
        {
            //make a list w/ the tag and tag value
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                switch (tag)
                {//if it's a one word tag then it goes to this switch statement
                    case UNSKIPPABLE:
                        canSkipTyping = false;
                        break;
                    default:
                        Debug.LogWarning("Tag failed to parse correctly" + tag);
                        break;
                }
            }
            else
            {
                string tagValue = splitTag[1].Trim();
                string tagKey = splitTag[0].Trim();

                switch (tagKey)
                {//if it's a two word tag then it goes to this switch statement
                    case NAME_TAG:
                        displayNameText.text = tagValue;
                        break;
                    case SOUND_TAG:
                        currentTypingSound = tagValue;
                        break;
                    case TYPING_SPEED:
                        typingSpeed = float.Parse(tagValue);
                        break;
                    case AUTO_SKIP:
                        autoSkip = float.Parse(tagValue);
                        break;
                    case IMAGE_TAG:
                        bool isValidPortraitName = false;
                        foreach (AnimationClip a in portraitClips)
                        {
                            if (a.name.Equals(tagValue))
                            {
                                isValidPortraitName = true;
                            }
                        }
                        if (isValidPortraitName)
                        {
                            portraitAnimator.Play(tagValue);
                        }
                        else
                        {
                            Debug.LogWarning("Portrait Tag Value: " + tagValue + " is not in the list of animation clips, playing default"); // (maybe it's named wrong in INK/portrait animation clip)
                            portraitAnimator.Play("default");
                        }
                        break;

                    case LAYOUT_TAG:
                        layoutAnimator.Play(tagValue);
                        break;
                    default:
                        Debug.LogWarning("Tag isn't one of the defined keys: " + tag);
                        break;
                }
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("tried to get variable of name: " + variableName + " but didn't find in dictionary");
        }
        return variableValue;
    }

    public void OnApplicationQuit()
    {
        SaveInk();
    }

    public void SaveInk()
    {
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
        else
        {
            Debug.LogWarning("Tried to save Ink Variables but dialogueVariables was null");
        }
    }
}
