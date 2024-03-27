using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{
    public String currentTime;
    public String bestTime1;
    public String bestTime2;
    public GameObject text;
    DateTime startTime;
    public TimeSpan timeElapsed { get; private set; }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            startTime = DateTime.Now;
            PlayerPrefs.SetString("StartTime", startTime.ToString());
        }
        else
        {
            startTime = System.DateTime.Parse(PlayerPrefs.GetString("startTime"));
        }
    }
    private void Update()
    {
        this.timeElapsed = DateTime.Now - startTime;
        currentTime = timeElapsed.ToString();
        //output to text
        text.GetComponent<TextMeshProUGUI>().SetText(timeElapsed.ToString());
    }
    public void SetSpeedrunTime()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            bestTime1 = currentTime;
            PlayerPrefs.SetString("bestTime1", bestTime1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            bestTime2 = currentTime;
            PlayerPrefs.SetString("bestTime2", bestTime2);
        }
    }
}
