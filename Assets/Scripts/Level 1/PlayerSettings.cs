using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerSettings : MonoBehaviour
{
    public GameObject otherCanvasOne;
    public GameObject otherCanvasTwo;
    public GameObject FirstPanel;
    public GameObject SecondPanel;
    public GameObject ThirdPanel;
    public GameObject SettingsPanel;
    public GameObject GoodJobText;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        //enables or disables time and the settings UI based on if they are enabled or disabled already
        if (Input.GetKeyDown("escape"))
        {
            SettingsPanel.SetActive(!SettingsPanel.activeSelf);
           // otherCanvasOne.SetActive(!SettingsPanel.activeSelf);
            otherCanvasTwo.SetActive(!SettingsPanel.activeSelf);

            if (SettingsPanel.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (Input.GetKey("1"))
        {
            SetFirstPanel();
        }
        if (Input.GetKey("2"))
        {
            SetSecondPanel();
        }
        if (Input.GetKey("3"))
        {
            SetThirdPanel();
        }
    }
    //sets ground and airspeed to what is in the input fields after they are changed
    public void SetFirstPanel()
    {
        SecondPanel.SetActive(false);
        ThirdPanel.SetActive(false);
        FirstPanel.SetActive(true);
    }
    public void SetSecondPanel()
    {
        SecondPanel.SetActive(true);
        ThirdPanel.SetActive(false);
        FirstPanel.SetActive(false);
    }
    public void SetThirdPanel()
    {
        SecondPanel.SetActive(false);
        ThirdPanel.SetActive(true);
        FirstPanel.SetActive(false);
    }
    
    public void callGoodJob()
    {//called by changing the input field ground/air speeds
        GoodJobText.SetActive(true);
        StartCoroutine(GoodJob()); 
    }
    private IEnumerator GoodJob()
    {
        //waits for a few seconds before turning off the good job text
        yield return new WaitForSecondsRealtime(3);
        GoodJobText.SetActive(false);
    }
}
