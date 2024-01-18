using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    public String currentTime;
    public String bestTime1;
    public GameObject text;
    DateTime startTime;
    public TimeSpan timeElapsed { get; private set; }

    private void Start()
    {
        startTime = DateTime.Now;
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
        PlayerPrefs.SetString("StartTime", startTime.ToString());
        bestTime1 = currentTime;
        PlayerPrefs.SetString("bestTime1", bestTime1);
        
    }
}
