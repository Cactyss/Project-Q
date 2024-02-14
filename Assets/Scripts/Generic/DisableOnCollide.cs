using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollide : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            this.gameObject.SetActive(false);
            Debug.Log("yo");
        }
    }
}
