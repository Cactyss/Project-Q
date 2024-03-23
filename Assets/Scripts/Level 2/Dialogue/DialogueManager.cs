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

public class DialogueManager : MonoBehaviour
{
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private KeyCode SubmitKeybind;
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
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
        SubmitKeybind = KeyCode.Space;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
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

        if (Input.GetKeyDown(SubmitKeybind))
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
        //reset dialouge  UI
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //do we make it go character by character right here?
            dialogueText.text = currentStory.Continue();
            // display choices if any
            DisplayChoices();

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

    public void MakeChoice(int i)
    {
        currentStory.ChooseChoiceIndex(i);
        ContinueStory();
    }

    private void HandleTags(List<String> tags)
    {
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
                    portraitAnimator.Play(tagValue);
                    // if the previous Play() call failed it won't be playing, so play the default portrait
                    if (!isPlaying(portraitAnimator, tagValue))
                    {
                        portraitAnimator.Play("default");
                    }
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    Debug.Log(tagKey + tagValue);
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
