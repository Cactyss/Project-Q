using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyAfterAwile : MonoBehaviour
{
    float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = 10f;
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Debug.Log("should be destroyed");

        Destroy(this);
    }
}
