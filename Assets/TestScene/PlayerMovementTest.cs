using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public LayerMask ground;
    public Transform GroundCheck;
    public Rigidbody2D player;
    bool isGrounded;
    public int speed;
    public int speed2;
    public int speed3;
    public int speed4;
    public Vector2 speedVector = new Vector2(1, 1);
    public Vector2 speedVector2 = new Vector2(1, 1);
    public Vector2 speedVector3 = new Vector2(1, 1);
    public Vector2 speedVector4 = new Vector2(1, 1);
    private void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        speedVector4 = new Vector2(speed4, 1);
        speedVector3 = new Vector2(speed3, 1);
        speedVector2 = new Vector2(speed2, 1);
        speedVector = new Vector2(speed, 1);
        //Removes Air Friction, Keeps Ground Friction
        if (isGrounded)
        {
            player.drag = 1;
            player.angularDrag = 0.15f;
        }
        else
        {
            player.drag = 0;
            player.angularDrag = 0;
        }
        //movement option1 (addforce)
        if (Input.GetKey(KeyCode.D))
        {
            player.AddForce(speedVector);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.AddForce(-speedVector);
        }

        //movement option2 (set velocity)
        if (Input.GetKey(KeyCode.E))
        {
            player.velocity = speedVector2 * new Vector2(1, player.velocity.y);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            player.velocity = speedVector2 * new Vector2(-1 , player.velocity.y);
        }

    }
    //Checks If Grounded
    public bool IsGrounded()
    {
        return (Physics.CheckSphere(GroundCheck.position, .1f, ground));
    } 
}
