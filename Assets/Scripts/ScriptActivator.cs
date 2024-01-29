using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptActivator : MonoBehaviour
{
    public bool OnSpawn;
    public bool OnCollide;
    public GameObject obby; //thing to actiavte
    public GameObject player; //thing to hit to activate
    void Start()
    {

        obby.SetActive(OnSpawn);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
            {
            obby.SetActive (OnCollide);
            }
    }
}
