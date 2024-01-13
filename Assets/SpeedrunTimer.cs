using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    public String currentTime;
    public String bestTime;
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
}
