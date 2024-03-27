using UnityEngine;

public class FollowSomething : MonoBehaviour
{
    Vector3 velocity;
    float smoothSpeed;
    float x;
    float y;
    Vector3 offset;
    public GameObject This;
    public GameObject Something;
    public GameObject Camera;
    void Start()
    {
        velocity = Vector3.zero;
        smoothSpeed = 0.15f;
        x = -10;
        y = 0;
        offset = new Vector3(x, y, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Monster.transform.position = Something.transform.position + offset;
        This.transform.position = Vector3.SmoothDamp(This.transform.position, Something.transform.position + offset, ref velocity, smoothSpeed);
    }
}
