using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    GameObject cam;
    Camera camera;
    // Use this for initialization
	void Start()
    {
        camera = cam.GetComponent<Camera>();
        camera.orthographicSize = 8.534866f; // Size u want to start with
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) // Change From Q to anyother key you want
        {
            camera.orthographicSize = camera.orthographicSize + 1 * Time.deltaTime;
            if (camera.orthographicSize > 8)
            {
                camera.orthographicSize = 8; // Max size
            }
        }


        if (Input.GetKey(KeyCode.E)) // Also you can change E to anything
        {
            camera.orthographicSize = camera.orthographicSize - 1 * Time.deltaTime;
            if (camera.orthographicSize < 6)
            {
                camera.orthographicSize = 6; // Min size 
            }
        }
    } 
}



