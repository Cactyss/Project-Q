using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    private float MoveSpeed;
    public Transform MovePoint;
    public LayerMask StopsMovement;
    void Start()
    {
        MoveSpeed = 5f;
        MovePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, MovePoint.position) <= 0.2f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), 0.2f, StopsMovement))
                {
                    MovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                    //move horizontal animation
                }
                else
                {
                    //bump into collider animation horizontal
                }
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(0, Input.GetAxisRaw("Vertical"), 0), 0.2f, StopsMovement))
                {
                    MovePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                    //move verical animation
                }
                else
                {
                    //bump into collider animation vertical
                }
            }
        }
    }
}
