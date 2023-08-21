using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerMovementTest : MonoBehaviour
{

    public bool bouncy;
    public LayerMask ground;
    public float castDistance;
    public Transform GroundCheck;
    public Rigidbody2D player;
    bool isGrounded;
    public int groundSpeed;
    public int AirSpeed;
    int speed2;
    public int stopForce;
    public int downForce;
    public int jumpForce;
    public float airDrag = 0.1f;
    public Vector2 speedVector2 = new Vector2(1, 1);
    public Vector2 stopVector = new Vector2(1, 1);
    public Vector2 downVector = new Vector2(1, 1);
    public Vector2 jumpVector = new Vector2(1, 1);
    public Vector2 boxSize = new Vector2(1, 1);
    private void Start()
    {
        Debug.Log("scene started! Yay!");
        bouncy = false;
        groundSpeed = 10;
        AirSpeed = 10;
        airDrag = 0.1f;
        stopForce = 30;
        downForce = 10;
        jumpForce = 10;
        airDrag = 0.1f;
    }
    void Update()
    {
        /** Does Update() mean that movement is tied to fps? **/

        //Checks the IsGrouned() method once, and sets it as a boolean
        isGrounded = IsGrounded();
        //Removes Air Friction, Keeps Ground Friction (changes values)
        if (isGrounded)
        {
            speed2 = groundSpeed;
            player.drag = 1.5f;
            player.angularDrag = 1f;
        }
        else
        {
            speed2 = AirSpeed;
            player.drag = airDrag;
            player.angularDrag = airDrag;
        }
        //sets the variables with any changed values (sets variables)
        speedVector2 = new Vector2(speed2, 1);
        stopVector = new Vector2(stopForce, 1);
        downVector = new Vector2(player.velocity.x, -downForce);
        jumpVector = new Vector2(player.velocity.x, jumpForce);

        //Stopping When Hold "S", and player not traveling up too fast to allow the jumppad to work when holding S
        if (Input.GetKey("s")  && player.velocity.y < 20)
        {
            //instant stop
            //  if (!isGrounded) { player.velocity = new Vector2(0, -15); }
            //  else { player.velocity = new Vector2(0, 0); }

            //slow stop
            //  if (!isGrounded) { player.AddForce(downVector); }
            // if (player.velocity.x > 0) { player.AddForce(stopVector * new Vector2(-1, 1)); }
            // else if (player.velocity.x < 0) { player.AddForce(stopVector); }

            // exponential stop
            if (player.velocity.x > 0.1f) { player.velocity = (new Vector2(player.velocity.x * 0.99f, player.velocity.y)); }
            else if (player.velocity.x < -0.1f) { player.velocity = (new Vector2(player.velocity.x * 0.99f, player.velocity.y)); }
            if (player.velocity.y > -3)
            {
                player.velocity = (new Vector2(player.velocity.x, -3));
            }
            if (!isGrounded && player.velocity.y > -10f)
            {
                player.velocity = (new Vector2(player.velocity.x, player.velocity.y * 1.5f));
            }
        }

        //Jump Script (has a toggle for bouncy mode)
        if (bouncy)
        {
            if (Input.GetKey("space") && isGrounded)
            {
                player.AddForce(jumpVector, ForceMode2D.Force);
            }
        }
        else
        {
            if (Input.GetKeyDown("space") && isGrounded)
            {
                player.AddForce(jumpVector, ForceMode2D.Force);
                Debug.Log("jump");
            }
        }
        //movement (set velocity)
        if (Input.GetKey(KeyCode.D))
        {
            player.velocity = speedVector2 * new Vector2(1, player.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.velocity = speedVector2 * new Vector2(-1, player.velocity.y);
        }
        // Reset if player presses r (and player isn't at the start)
        if ((Input.GetKey(KeyCode.R) && (player.transform.position.x > 1 || (player.transform.position.x < -1))))
        {
            ResetPlayer();
        }
        // Reset is player is off the map (down too far)
        if (player.transform.position.y < -10)
        {
            ResetPlayer();
        }
    }
    //Checks If Grounded
    public bool IsGrounded()
    {
         return (Physics2D.BoxCast(player.transform.position, boxSize, 0, -transform.up, castDistance, ground));
    } 
    // checks if the player is below the group, if so, they are teleported back
    public void ResetPlayer()
    {
        player.velocity = new Vector2(0, 0);
        player.transform.position = new Vector2(0, 1);
    }
}
