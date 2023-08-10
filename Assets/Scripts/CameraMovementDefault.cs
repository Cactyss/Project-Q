using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementTest : MonoBehaviour
{
    public Transform MyCamera;
    public Transform target;
    public Rigidbody2D player;
    public float smoothSpeed = 0.125f;
    public Vector3 velocity = Vector3.zero;

    // sets camera position to player position with a SmoothDamp
    void FixedUpdate()
    {
        MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3 (0, -target.position.y + 5, -10), ref velocity, smoothSpeed);
    }


 


   
}
