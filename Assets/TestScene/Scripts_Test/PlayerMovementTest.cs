using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerMovementTest : MonoBehaviour
{
    public LayerMask ground;
    public float castDistance;
    public Transform GroundCheck;
    public Rigidbody2D player;
    bool isGrounded;
    public int speed2;
    public int stopForce;
    public int downForce;
    public int jumpForce;
    public Vector2 speedVector2 = new Vector2(1, 1);
    public Vector2 stopVector = new Vector2(1, 1);
    public Vector2 downVector = new Vector2(1, 1);
    public Vector2 jumpVector = new Vector2(1, 1);
    public Vector2 boxSize = new Vector2(1, 1);
    private void Start()
    {
        Debug.Log("scene started! Yay!");
    }
    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        speedVector2 = new Vector2(speed2, 1);
        stopVector = new Vector2(stopForce, 1);
        downVector = new Vector2(player.velocity.x, -downForce);
        jumpVector = new Vector2(player.velocity.x, jumpForce);

        //Removes Air Friction, Keeps Ground Friction
        if (isGrounded)
        {
            player.drag = 1.5f;
            player.angularDrag = 1f;
        }
        else
        {
            player.drag = 0;
            player.angularDrag = 0;
        }

        //Stopping When Hold "S"
        if (Input.GetKey("s"))
        {
            if (!isGrounded) { player.AddForce(downVector); }
            if (player.velocity.x > 0) { player.AddForce(stopVector * new Vector2(-1, 1)); }
            else if (player.velocity.x < 0) { player.AddForce(stopVector); }
        }
        
        //Jump Script      
        if (Input.GetKey("space"))
            { Debug.Log("yo"); }
        if (Input.GetKey("space") && isGrounded)
        {
            
            player.AddForce(jumpVector, ForceMode2D.Impulse);
           
        }
        //movement (set velocity)
        if (Input.GetKey(KeyCode.D))
        {
            player.velocity = speedVector2 * new Vector2(1, player.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.velocity = speedVector2 * new Vector2(-1 , player.velocity.y);
        }

    }
    //Checks If Grounded
    public bool IsGrounded()
    {
         return (Physics2D.BoxCast(player.transform.position, boxSize, 0, -transform.up, castDistance, ground));
    } 
}
