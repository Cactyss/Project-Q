using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCamera : MonoBehaviour
{
    public GameObject cam;
    Camera camerab;
    public GameObject player;
    public Transform MyCamera;
    public Transform target;
    private float smoothSpeed;
    private Vector3 velocity;
    public GameObject GameManager;
    private void Start()
    {
        velocity = Vector3.zero;
        smoothSpeed = 1f;
        camerab = cam.GetComponent<Camera>();
        camerab.orthographicSize = 8.534866f; // Size u want to start with
    }
    void Update()
    {
        if (Input.GetKey("r"))
        {
            camerab.orthographicSize = 8.534866f;
            this.gameObject.SetActive(false);
            GameManager.GetComponent<CameraMovementTest>().enabled = true;
        }
        if (this.gameObject.activeInHierarchy == true)
        {
            GameManager.GetComponent<CameraMovementTest>().enabled = false;

        }
    }
    void FixedUpdate()
    {
        MyCamera.transform.position = Vector3.SmoothDamp(MyCamera.transform.position, target.position + new Vector3(0, 10, -10), ref velocity, smoothSpeed);
        
        if (camerab.orthographicSize < 45)
        {
            camerab.orthographicSize = camerab.orthographicSize + 10 * Time.deltaTime;
        }
        // Camera.main.lensShift;
        // Camera.main.
    }



}
