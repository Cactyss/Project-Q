using UnityEngine;

public class ScriptDeactivator1 : MonoBehaviour
{
    public GameObject obby; //thing to deactiavte
    public GameObject player; //thing to hit to deactivate
    void Start()
    {
        // obby.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            obby.SetActive(false);
        }
    }
}
