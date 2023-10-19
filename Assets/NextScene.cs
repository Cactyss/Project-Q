using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    int yo;
    public void GoNextScene()
    {
        yo = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(yo);
    }
}
