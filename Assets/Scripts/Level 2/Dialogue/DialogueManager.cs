using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //custom rich text tags
    private const string DELETE_CUSTOM = "del";

    //ink tags
    private const string NAME_TAG = "name";
    private const string IMAGE_TAG = "image";
    private const string LAYOUT_TAG = "layout";
    private const string UNSKIPPABLE = "unskippable";
    private const string TYPING_SPEED = "typingspeed";
    private const string AUTO_SKIP = "autoskip";
    //TODO: add ink tags (or tie to the portrait tag): sound effect, font, font color, font size, delay until next dialogue
    [Header("Text Parameters")]
    [SerializeField] private float defaultTypingSpeed = 0.04f;
    private float typingSpeed;

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
        typingSpeed = defaultTypingSpeed;
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

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private IEnumerator DisplayLine(string line)
    {
        //if the line is blank we skip it because it's prob not supposed to be blank
        if (line.Trim() == "")
        {
            Debug.LogWarning("skipped a blank line");
            ContinueStory();
        }
        else
        {
            int characters = 0;
            int letters = 0;
            //hides certian UI while typing out dialogue
            canContinueIcon.SetActive(false);
            HideChoices();

            //dont allow player to skip onto the next line before they see the current line fully printed out

            canContinueToNextLine = false;
            bool addedAngleBracket = false;
            bool isAddingRichTextTag = false;
            bool isAddingCustomText = false;
            StringBuilder customText = new StringBuilder();
            //empty the displayed text
            dialogueText.text = "";
            foreach (char letter in line.ToCharArray())
            {
                characters++;
                //allows the dialogue to be skipped after 3 characters are displayed
                if (canSkipTyping && Input.GetKey(SubmitKeybind) && letters > 5)
                {
                    //dialogueText.text = line;
                    typingSpeed = 0f;
                }
                if ((letter == '<' || isAddingCustomText))
                {
                    if (isAddingRichTextTag)
                    {
                        isAddingCustomText = true;
                        isAddingRichTextTag = false;
                    }
                    else
                    {
                        isAddingRichTextTag = true;
                    }
                    if (isAddingCustomText)
                    {
                        if (letter != '<' && letter != '>')
                        {
                            customText.Append(letter);
                        }
                        if (letter == '>')
                        {
                            Debug.Log(customText.ToString());
                            isAddingCustomText = false;
                            //Handle Custom Tags
                            switch (customText.ToString())
                            {
                                case DELETE_CUSTOM:
                                    DisplayChoices();
                                    yield return new WaitForSeconds(0.5f);
                                    HideChoices();
                                    dialogueText.text = "";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else if (isAddingRichTextTag)
                {//add the rich text tag info and stuff
                    if (!addedAngleBracket) { dialogueText.text += '<'; addedAngleBracket = true; }
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
                    //TODO: add a thing that makes it so a word jumps to the next line if it can't fit completely on the current line
                    if (!letter.ToString().Equals(" "))
                    {

                        //TODO: add sound effects for typing character by character
                    }
                    else
                    {
                        //TODO: maybe add here a thing for people who pause after completing a word?
                    }
                    yield return new WaitForSeconds(typingSpeed);
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
        //reset dialouge  UI
        displayNameText.text = "???";
        //portraitAnimator.Play("default");
        layoutAnimator.Play("right");
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
    private bool isPlaying(Animator a, String stateName)
    {
        if (a.GetCurrentAnimatorStateInfo(0).IsName(stateName) && a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }
}
