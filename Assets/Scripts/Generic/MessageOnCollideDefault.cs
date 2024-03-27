using UnityEngine;

public class MessageOnCollideDefault : MonoBehaviour
{
    //place this script on a gameobject that you want to display a message when the player touches it
    public GameObject TheMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            TheMessage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            TheMessage.SetActive(false);
        }
    }
}
