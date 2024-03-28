using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene1 : MonoBehaviour
{
    public GameObject timerObject;
    public int delay;
    private void Start()
    {
        if (timerObject != null)
        {
            timerObject.GetComponent<SpeedrunTimer>().SetSpeedrunTime();
        }
        StartCoroutine(GoNextScene());
    }
    private IEnumerator GoNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("load next level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("load next level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
