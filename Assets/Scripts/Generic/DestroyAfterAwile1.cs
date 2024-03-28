using System.Collections;
using UnityEngine;

public class DestroyAfterAwile1 : MonoBehaviour

/* Unmerged change from project 'Assembly-CSharp.Player'
Before:
{
    
    // Start is called before the first frame update
After:
{

    // Start is called before the first frame update
*/
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
