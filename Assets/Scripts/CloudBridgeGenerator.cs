using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CloudBridgeGenerator : MonoBehaviour
{
    public GameObject parent;
    public GameObject platform;
    public float waitTime;
    public float offset;
    public Vector2 NewVelocity;
    public Vector3 NewTransform;
    float realWaitTime;
    // Start is called before the first frame update
    void Start()
    {
        realWaitTime = waitTime - offset;
        StartCoroutine(BridgeGen());

    }
    private IEnumerator BridgeGen()
    {
        yield return new WaitForSeconds(offset);
        GameObject newPlatform = Instantiate(platform, NewTransform, new Quaternion(0,0,0,0));
        newPlatform.transform.parent = parent.transform;
        newPlatform.GetComponent<Rigidbody2D>().velocity = NewVelocity;
        yield return new WaitForSeconds(realWaitTime);
        //Debug.Log(realWaitTime + offset);
        StartCoroutine(BridgeGen());

    }
}
 