using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSettings : MonoBehaviour
{
    public TMP_Text GoodJobText;
    public GameObject Panel;
    public TMP_InputField GroundSpeed;
    public TMP_InputField AirSpeed;
 
    void Start()
    {
        GoodJobText.enabled = false;
        Panel.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            Panel.SetActive(!Panel.activeSelf);
            if (Panel.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public void callGoodJob()
    {

        GoodJobText.enabled = true;
        GoodJob(2);
    }
    private IEnumerator GoodJob(int time)
    {
        
        yield return new WaitForSeconds(time);
        GoodJobText.enabled = false;
    }
}
