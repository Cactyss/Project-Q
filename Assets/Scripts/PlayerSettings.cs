using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerSettings : MonoBehaviour
{
    public GameObject GoodJobText;
    public GameObject SettingsPanel;
    public TextMeshProUGUI GroundSpeed;
    public TMP_Text AirSpeed;
    public GameObject player;

     
    void Start()
    {
        
        GoodJobText.SetActive(false);
        SettingsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //enables or disables time and the settings UI based on if they are enabled or disabled already
        if (Input.GetKeyDown("escape"))
        {
            SettingsPanel.SetActive(!SettingsPanel.activeSelf);
            if (SettingsPanel.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    //sets ground and airspeed to what is in the input fields after they are changed
    public void UpdateGroundSpeed()
    {
        player.GetComponent<PlayerMovementTest>().groundSpeed = int.Parse(GroundSpeed.text.Substring(0, GroundSpeed.text.Length - 1));
    }
    public void UpdateAirSpeed()
    {
          player.GetComponent<PlayerMovementTest>().AirSpeed = int.Parse(AirSpeed.text.Substring(0, AirSpeed.text.Length - 1));
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
