using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int delay;
    private void Start()
    {
        StartCoroutine(GoNextScene());
    }
    private IEnumerator GoNextScene()
    {
        
        yield return new WaitForSeconds(delay);
        Debug.Log("load next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
