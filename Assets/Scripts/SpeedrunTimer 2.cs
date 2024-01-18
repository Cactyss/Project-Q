using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer2 : MonoBehaviour
{

    public String currentTime;
    public String bestTime2;
    public GameObject text;
    DateTime startTime;
    public TimeSpan timeElapsed { get; private set; }

    private void Start()
    {
        startTime = System.DateTime.Parse(PlayerPrefs.GetString("startTime"));

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
        bestTime2 = currentTime;
        PlayerPrefs.SetString("bestTime2", bestTime2);
        
    }
}
