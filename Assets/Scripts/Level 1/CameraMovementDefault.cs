using UnityEngine;

public class CameraMovementTest : MonoBehaviour
{
    public Transform MyCamera;
    public Transform target;
    public Rigidbody2D player;
    private float smoothSpeed;
    private Vector3 velocity;

    private void Start()
    {
        velocity = Vector3.zero;
        smoothSpeed = 0.125f;
    }
    // sets camera position to player position with a SmoothDamp
    void FixedUpdate()
    {
        if (target.position.y < 15.5)
        {
            MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3(0, -target.position.y + 5, -10), ref velocity, smoothSpeed);
        }
        //  else if (target.position.y < 35.5)
        //  {
        //      MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3(0, -target.position.y + 24.5f, -10), ref velocity, smoothSpeed);
        //  }
        //  else if (target.position.y < 54)
        //  {
        //      MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3(0, -target.position.y + 43.5f, -10), ref velocity, smoothSpeed);
        //   }
        else
        {
            MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3(0, 3, -10), ref velocity, smoothSpeed);
        }
    }






}
