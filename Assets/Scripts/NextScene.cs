using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject timerObject;
    public int delay;
    private void Start()
    {
        try
        {
            timerObject.GetComponent<SpeedrunTimer>().SetSpeedrunTime();
        }
        finally
        { 
        StartCoroutine(GoNextScene());
        }
    }
    private IEnumerator GoNextScene()
    {   
        yield return new WaitForSeconds(delay);
        Debug.Log("load next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
