using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.SearchService;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Rendering;
using System.Data;

public class DialogueManager : MonoBehaviour
{
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    //TODO: add ink tags (or tie to the portrait tag): sound effect, font, font color, font size, delay until next dialogue
    [Header("Text Parameters")]
    [SerializeField] private float typingSpeed = 0.04f;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool canSkipTyping = true;
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
    public bool dialogueIsPlaying { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("multiple instances of dialogue manager in scene");
        }
        instance = this;

        
    }
    private void Start()
    {
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        dialogueIsPlaying = false;
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

        resetTags();
        ContinueStory();
    }
    private void resetTags()
    {
        //reset dialouge  UI
        displayNameText.text = "???";
        //portraitAnimator.Play("default");
        layoutAnimator.Play("right");
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
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private IEnumerator DisplayLine(string line)
    {
        int letters = 0;
        //hides certian UI while typing out dialogue
        canContinueIcon.SetActive(false);
        HideChoices();
        
        //dont allow player to skip onto the next line before they see the current line fully printed out
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        //empty the displayed text
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            Debug.Log(letters);
            //allows the dialogue to be skipped after 3 characters are displayed
            if (/*canSkipTyping && */Input.GetKey(SubmitKeybind) && letters > 5)
            {
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {//add the rich text tag info and stuff
               
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {//add the letter and play a sound
                dialogueText.text += letter;
                letters += 1;
                //TODO: add sound effects for typing character by character
                //TODO: add a thing that makes it so a word jumps to the next line if it can't fit completely on the current line
                //TODO: add a thing 
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        // display choices if any
        DisplayChoices();
        canContinueIcon.SetActive(true);
        canContinueToNextLine = true;
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
        foreach(Choice choice in currentChoices)
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

    private void HandleTags(List<String> tags)
    {
        //resetTags();
        foreach (string tag in tags)
        {
            //make a list w/ the tag and tag value
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tag failed to parse correctly" + tag);
            }
            string tagValue = splitTag[1].Trim();
            string tagKey = splitTag[0].Trim();

           
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;

                case PORTRAIT_TAG:
                    bool isValidPortraitName = false;
                    for (int i = 0; i < portraitClips.Length; i++) 
                    {
                        if (portraitClips[i].name.Equals(tagValue))
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
                        portraitAnimator.Play("default");
                        Debug.LogWarning("Portrait Tag Value: " + tagValue + " is not in the list of animation clips, playing default (maybe it's named wrong in INK/portrait animation clip)");
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


    private bool isPlaying(Animator a, String stateName)
    {
        if (a.GetCurrentAnimatorStateInfo(0).IsName(stateName) && a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }
}
