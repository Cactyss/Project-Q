using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVisualTemp : MonoBehaviour
{
    private SpriteRenderer eyeL;
    private SpriteRenderer eyeR;
    private SpriteRenderer hair;
    private SpriteRenderer skin;
    void Start()
    {
        foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            switch (sprite.name)
                {
                case "eyeL": 
                    eyeL = sprite;
                    break;
                case "eyeR":
                    eyeR = sprite;
                    break;
                case "hair":
                    hair = sprite;
                    break;
                case "skin":
                    skin = sprite;
                    break;
                default: Debug.Log("huh?");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string hairColor = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("hair_color")).value;
        string eyeColor = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("eye_color")).value;
        string skinColor = ((Ink.Runtime.StringValue)DialogueManager.GetInstance().GetVariableState("skin_color")).value;


        switch (hairColor)
        {
            case "":
                break;
            case "brown":
                Debug.Log("brown");
                //hair.color = new Color(150, 75, 1, 255);
                //hair.color = new Color(100f, 50f, 150f, 1f);
                hair.color = Color.black;
                break;
            case "black":
                hair.color = Color.black;
                break;
            case "red":
                hair.color = Color.red;
                break;
            case "demon":
                hair.color = Color.red;
                break;
            default:
                break;
        }

        switch (eyeColor)
        {
            case "":
                break;
            case "blue":
                eyeL.color = Color.blue;
                eyeR.color = Color.blue;
                break;
            case "brown":
                eyeL.color = Color.black;
                eyeR.color = Color.black;
                break;
            case "green":
                eyeL.color = Color.green;
                eyeR.color = Color.green;
                break;
            case "demon":
                eyeL.color = Color.black;
                eyeR.color = Color.black;
                break;
            default:
                break;
        }

        switch (skinColor)
        {
            case "":
                break;
            case "light":
                skin.color = Color.white;
                break;
            case "dark":
                skin.color = Color.black;
                break;
            case "cyborg":
                skin.color = Color.gray;
                break;
            case "demon":
                skin.color = Color.red;
                break;
            default:
                break;
        }
    }
}
