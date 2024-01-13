using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyAfterAwile : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Destroy()); 
  
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(41);
      
        Destroy(this.gameObject);
    }
}
