using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBridge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            FindObjectOfType<PlayMusic>().PlayBridge();
        }
    }
}
