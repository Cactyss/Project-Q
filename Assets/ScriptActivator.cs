using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptActivator : MonoBehaviour
{
    public GameObject obby;
    public GameObject player;
    void Start()
    {
        obby.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == player.tag)
            {
            obby.SetActive (true);
            }
    }
}
