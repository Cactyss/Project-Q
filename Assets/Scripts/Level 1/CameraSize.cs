using UnityEngine;

public class CameraSize : MonoBehaviour
{
    GameObject cam;
    Camera Mycamera;
    // Use this for initialization
    void Start()
    {
        Mycamera = cam.GetComponent<Camera>();
        Mycamera.orthographicSize = 8.534866f; // Size u want to start with
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) // Change From Q to anyother key you want
        {
            Mycamera.orthographicSize = Mycamera.orthographicSize + 1 * Time.deltaTime;
            if (Mycamera.orthographicSize > 8)
            {
                Mycamera.orthographicSize = 8; // Max size
            }
        }


        if (Input.GetKey(KeyCode.E)) // Also you can change E to anything
        {
            Mycamera.orthographicSize = Mycamera.orthographicSize - 1 * Time.deltaTime;
            if (Mycamera.orthographicSize < 6)
            {
                Mycamera.orthographicSize = 6; // Min size 
            }
        }
    }
}



