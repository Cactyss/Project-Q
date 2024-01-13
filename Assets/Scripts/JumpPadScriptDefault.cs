using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadScriptDefault : MonoBehaviour
{
    bool Collided;
    public float JumpPadForce;
    Vector2 jumpVector;
    public Rigidbody2D playerBody;
    private void Start()
    {
        Collided = false;
        jumpVector = new Vector2(0, JumpPadForce);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collided = true;
        }
    }
    private void LateUpdate()
    {//does JumpPad after all other movement scripts
        if (Collided)
        {
            Debug.Log("jumpad");
            playerBody.AddForce(jumpVector, ForceMode2D.Force);
            playerBody.velocity = jumpVector;
        }
        Collided = false;
    }
}
