using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseGenMechanic : MonoBehaviour
{
    public Camera MainCam;
    public GameObject GenObject;
    Vector3 mousePosition;
    // Update is called once per frame
    void Update()
    {
        Vector3 point = new Vector3();
        // declare a new variable to be my spawn Point

        Vector2 mousePos = new Vector2();
        // declaring the variable for the mouse click

        mousePos = Input.mousePosition;
        // set the mousePos variable to the position of the mouse click (screen space)

        point = MainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, MainCam.nearClipPlane));
        // set my spawn point variable by converting mousePos from screen space into world space

        
        // spawns the object
        if (Input.GetMouseButtonDown(0)) 
        {
            //Instantiate(GenObject, mousePosition + new Vector3(0,0,-10) ,new quaternion (0,0,0, 0));
            Instantiate(GenObject);
            GenObject.transform.position = point;
        }
    }
}
