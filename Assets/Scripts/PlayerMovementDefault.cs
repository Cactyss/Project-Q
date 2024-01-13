using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PlayerMovementTest : MonoBehaviour
{
    GameObject[] blue;
    GameObject[] click;
    private bool shouldStop;
    public float maxspeed;
    public bool gravityYes;
    public float gravity;
    private Vector2 spawnPos; 
    public bool bouncy;
    public LayerMask ground;
    private float castDistance;
    public Rigidbody2D player;
    private bool isGrounded;
    public float groundSpeed;
    public float AirSpeed;
    int satiated;
    float speed2;
    public int stopForce;
    public int downForce;
    public int jumpForce;
    public float airDrag;
    public float airAngleDrag;
    public float groundDrag;
    public float groundAngleDrag;
    public Vector2 speedVector2;
    public Vector2 stopVector;
    public Vector2 downVector;
    public Vector2 jumpVector;
    public Vector2 boxSize;
    float velocityTemp;
    public float boostValue;
    private void Start()
    {
        blue = GameObject.FindGameObjectsWithTag("Blue");
        boostValue = 15;
        speedVector2 = new Vector2(1, 1);
        stopVector = new Vector2(1, 1);
        downVector = new Vector2(1, 1); 
        jumpVector = new Vector2(1, 1);
        boxSize = new Vector2(1, 1);
        castDistance = 1;
        spawnPos = player.position;
        bouncy = false;
        //groundSpeed = 8.5;
        //AirSpeed = 8.5;
        //airDrag = 0.1f;
        //airAngleDrag = 0f;
        //groundDrag = 1.5f;
        //GroundAngleDrag = 0f;
        stopForce = 30;
        downForce = 10;
        jumpForce = 10;
        gravity = 1.05f;
        maxspeed = 10;
        groundSpeed = 10;
        AirSpeed = 10;
        airDrag = 0.1f;
        airAngleDrag = 0;
        groundDrag = 1.5f;
        groundAngleDrag = 0;
        satiated = 0;
        shouldStop = false;
    }
    private void FixedUpdate()
    {
        playerPhysicsGravity();
    }
    void Update()
    {
        /** Does Update() mean that movement is tied to fps? **/

        ChangeVariables();

        playerMoveJump();

        playerMoveStop();

        if (!shouldStop) { playerMoveLeftRight(); }

        playerMoveBoost();

        checkReset();
    }
    /** MAIN METHODS **/

    public void playerMoveLeftRight()
    {
        //movement (set velocity)
        if (Input.GetKey(KeyCode.D))
        {
            if (player.velocity.x < maxspeed && player.velocity.x > -maxspeed)
            {
                player.velocity = speedVector2 * new Vector2(1, player.velocity.y);
            }
            else if (player.velocity.x > maxspeed || player.velocity.x < -maxspeed)
            {
                SeteVelocityTemp();
                player.velocity = new Vector2(velocityTemp, player.velocity.y);
            }
            /**
            else if (player.velocity.x < maxspeed)
            {//speed up to maxspeed at a decelerating rate
              player.velocity = player.velocity + (new Vector2(((maxspeed - player.velocity.x)) * Time.deltaTime, 0));
            } */
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (player.velocity.x < maxspeed && player.velocity.x > -maxspeed)
            {
                player.velocity = speedVector2 * new Vector2(-1, player.velocity.y);
            }
            else if (player.velocity.x > maxspeed || player.velocity.x < -maxspeed)
            {
                SeteVelocityTemp();
                player.velocity = new Vector2(velocityTemp * -1, player.velocity.y);
            }
            
            /**
            else if (player.velocity.x > -maxspeed)
            {//speed up to maxspeed at a decelerating rate
              player.velocity = player.velocity + (new Vector2((-(player.velocity.x + maxspeed)) * Time.deltaTime, 0));
            }*/

        }
    }
    //Checks for stop input
    public void playerMoveStop()
    {
        
        //Stopping When Hold "S", and player not traveling up too fast to allow the jumppad to work when holding S
        if (Input.GetKey("s") && player.velocity.y < 20)
        {
            shouldStop = true;
            //instant stop
             // { player.velocity = new Vector2(0, player.velocity.y); }

            // exponential stop
            if (player.velocity.x > 0.05f) 
            { //stop all x movement exponentially
                //player.velocity = (new Vector2(player.velocity.x * 0.99f, player.velocity.y));
                player.velocity = player.velocity + (new Vector2(-40 * Time.deltaTime, 0));
            }
            if (player.velocity.x < -0.05f)
            {
                player.velocity = player.velocity + (new Vector2(40 * Time.deltaTime, 0));
            }

            //stop y movement
            /** if (player.velocity.y > -3)
             { 
                 player.velocity = (new Vector2(player.velocity.x, -3));
             }
             if (!isGrounded && player.velocity.y > -10f)
             {
                 player.velocity = (new Vector2(player.velocity.x, player.velocity.y * 1.5f *  Time.deltaTime));
             } **/
        }
        else
        {
            shouldStop = false;
        }
    }
    public void playerMoveBoost()
    {
        if(Input.GetKeyDown(KeyCode.J) && satiated >= 25 && isGrounded)
        {
            boostValue = player.velocity.x * 3f;
            if (player.velocity.x > 0)
            {
                boostValue = Mathf.Abs(boostValue);
            }
            else
            {
                boostValue = Mathf.Abs(boostValue) * -1;
            }
            //BOOST with J
            player.velocity = new Vector2(player.velocity.x + boostValue , player.velocity.y * 1.3f);
            //player.velocity = new Vector2(player.velocity.x + player.velocity.x * boostValue, player.velocity.y);
            satiated = satiated - 25;
            Debug.Log("J");
            Debug.Log(satiated);
        }
    }
    public void playerMoveJump()
    {
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
    }
    //changes player gravity/physics based on which level of atmosphere they are in (for now just makes them go down fast on ground level, once they are already going down a certian speed)
    public void playerPhysicsGravity()
    {
        //ground level
        if (player.position.y < 15.5)
        {
            gravityYes = true;
        }
        //sky 
        else if (player.position.y < 35.5)
        {
            gravityYes = false;
        }
        //pretty high yo!
        else if (player.position.y < 54)
        {
            gravityYes = false;
        }
        //space
        else
        {
            gravityYes = false;
        }

        if (player.velocity.y < -0.5 && player.velocity.y > -18 && gravityYes)
        {
           // player.velocity = player.velocity + new Vector2(0, player.velocity.y * gravity * Time.deltaTime);
            player.AddForce(new Vector2(0, gravity * Time.deltaTime));
        }
    }
    public void ChangeVariables()
    {
        //Checks the IsGrouned() method once, and sets it as a boolean
        isGrounded = IsGrounded();
        //Removes Air Friction, Keeps Ground Friction (changes values)
        if (isGrounded)
        {
            speed2 = groundSpeed;
            player.drag = groundDrag;
            player.angularDrag = groundAngleDrag;
        }
        else
        {
            speed2 = AirSpeed;
            player.drag = airDrag;
            player.angularDrag = airAngleDrag;
        }
        //sets the variables with any changed values (sets variables)
        speedVector2 = new Vector2(speed2, 1);
        stopVector = new Vector2(stopForce, 1);
        downVector = new Vector2(player.velocity.x, -downForce * Time.deltaTime);
        jumpVector = new Vector2(player.velocity.x, jumpForce * Time.deltaTime);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Blue" && Input.GetKey(KeyCode.K) && satiated < 50)
        {
           
            satiated = satiated + 25;
            Debug.Log(satiated);
            //Destroy(collision.gameObject); Disable or destroy?
            collision.gameObject.SetActive(false);
        }
    }
    public void checkReset()
    {
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


    /** OTHER METHODS **/

    //Checks If Grounded
    public bool IsGrounded()
    {
        return (Physics2D.BoxCast(player.transform.position, boxSize, 0, -transform.up, castDistance, ground));
    }
    // checks if the player is below the group, if so, they are teleported back
    public void ResetPlayer()
    {
        player.velocity = new Vector2(0, 0);
        player.transform.position = spawnPos;
        satiated = 0;
        foreach (GameObject b in blue)
        {
            b.SetActive(true);
        }
        click = GameObject.FindGameObjectsWithTag("click mechanic");
        foreach (GameObject c in click)
        {
            Destroy(c.gameObject);
        }


    }
    public void SeteVelocityTemp()
    {
        velocityTemp = Mathf.Abs(player.velocity.x);
    }
    //sets a new spawn point if the player triggers a checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            SetNewCheckpoint();
            Debug.Log("checkpoint");
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "death")
        {
            ResetPlayer();
        }
    }
    public void SetNewCheckpoint()
    {
        spawnPos = player.transform.position;
        
    }
}
