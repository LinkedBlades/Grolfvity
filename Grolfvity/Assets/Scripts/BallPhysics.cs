using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEditorInternal;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    //Getting rigid body
    Rigidbody2D rbody;
    aimLine aimLine;

    //Shot variables
    Vector2 mousePosIni;
    Vector2 mousePosEnd;
    Vector2 ballPos;
    [SerializeField] Material mat;
    [SerializeField] float shotStrength = 1;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        aimLine = GetComponentInChildren<aimLine>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (mousePosEnd != null && ballPos != null)
        {
            Debug.DrawLine(ballPos, -mousePosEnd, Color.blue);

        }
    }


    //Events

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Gravity Field")
        {
            //Temporary while prototyping, eventually will just be set in the editor
            rbody.gravityScale = 0;
        }
    }

    private void OnMouseDown()
    {
        //Ball stopped check
        if (rbody.velocity.magnitude <= 0.1f)
        {
            ballPos = new Vector2(this.transform.position.x, this.transform.position.y);

            aimLineIni = ballPos;
        }

    }

    private void OnMouseDrag()
    {
        //Ball stopped check
        if (rbody.velocity.magnitude <= 0.1f)
        {
            //Getting mouse position into world coordinates for aiming shot
            mousePosEnd = Input.mousePosition;
            mousePosEnd = Camera.main.ScreenToWorldPoint(mousePosEnd);

            //Updating aim line end
            aimLineEnd = (ballPos - mousePosEnd);

            //Checking for mouse distance before drawing aimline
            if (Vector2.Distance(aimLineIni, mousePosEnd) > 1.0f)
            {
                aimLine.UpdateLineRenderer(ballPos, aimLineEnd);
            }

            if (Vector2.Distance(aimLineIni, mousePosEnd) <= 1.0f)
            {
                aimLine.ClearAimLine();
            }
        }
    }

    private void OnMouseUp()
    {
        //Ball stopped check
        if (rbody.velocity.magnitude <= 0.1f)
        {
            //Check distance before shooting
            if (Vector2.Distance(ballPos, mousePosEnd) > 1.0f)
            {
                rbody.AddForce((ballPos - mousePosEnd) * shotStrength, ForceMode2D.Impulse);
            }

            aimLine.ClearAimLine();
        }
    }

}
