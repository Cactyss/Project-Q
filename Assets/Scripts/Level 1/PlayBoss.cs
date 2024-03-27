using UnityEngine;

public class PlayBoss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            FindObjectOfType<PlayMusic>().PlayBoss();
        }
    }
}
