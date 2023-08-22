using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsDefault : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D player;
    

    // sets camera position to player position with a SmoothDamp
    void FixedUpdate()
    {
        if (target.position.y < 15.5)
        {
            
        }
        else if (target.position.y < 35.5)
        {
            
        }
        else if (target.position.y < 54)
        {
            
        }
        else
        {
            
        }
    }
}
