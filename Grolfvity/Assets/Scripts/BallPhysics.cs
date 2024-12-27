using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Timeline.Actions;
using UnityEditorInternal;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    //Getting rigid body
    Rigidbody2D rbody;
    [SerializeField] float magnitude;
    [SerializeField] aimLine aimLine;
    //[SerializeField] float friction;

    //Shot variables
    Vector2 mousePosEnd;
    Vector2 ballPos;
    [SerializeField] float shotStrength = 1;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;
<<<<<<< Updated upstream
    //GameObject aimLine;
    //LineRenderer currentLineRenderer;

=======
    [SerializeField] float maxLineLenght = 10;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        aimLine = GetComponentInChildren<aimLine>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

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
        //Unused variable for now
        //mousePosIni = Input.mousePosition;
        ballPos = new Vector2(this.transform.position.x, this.transform.position.y);

        aimLineIni = ballPos;

    }

    private void OnMouseDrag()
    {
<<<<<<< Updated upstream
        //Getting mouse position into world coordinates for aiming shot
        mousePosEnd = Input.mousePosition;
        mousePosEnd = Camera.main.ScreenToWorldPoint(mousePosEnd);
    
        //Updating aim line end
        aimLineEnd = (ballPos - mousePosEnd);

        //Checking for mouse distance before drawing aimline
        if (Vector2.Distance(aimLineIni, mousePosEnd) > 1.0f)
        {
            aimLine.UpdateLineRenderer(ballPos, aimLineEnd);
=======

        //Variables for storing direction and lengtt of aim line
        Vector2 aimDirection;
        float aimMagnitude;

        //Ball stopped check
        if (rbody.velocity.magnitude <= 0.1f)
        {
            //Getting mouse position into world coordinates for aiming shot
            mousePosEnd = Input.mousePosition;
            mousePosEnd = Camera.main.ScreenToWorldPoint(mousePosEnd);

            //Calculating direction and lenght of aimline based on ball and mouse position
            aimDirection = (ballPos - mousePosEnd).normalized;
            aimMagnitude = MathF.Min((ballPos - mousePosEnd).magnitude, maxLineLenght);

            //Updating aim line end
            aimLineEnd = ballPos + aimDirection * aimMagnitude;

            //Checking for mouse distance before drawing aimline
            if (Vector2.Distance(aimLineIni, mousePosEnd) > 1.0f)
            {
                aimLine.UpdateLineRenderer(ballPos, aimLineEnd);
            }

            if (Vector2.Distance(aimLineIni, mousePosEnd) <= 1.0f)
            {
                aimLine.ClearAimLine();
            }
>>>>>>> Stashed changes
        }

        if (Vector2.Distance(aimLineIni, mousePosEnd) <= 1.0f)
        {
            aimLine.ClearAimLine();
        }

    }

    private void OnMouseUp()
    {
        //Check distance before shooting
        if (Vector2.Distance(ballPos, mousePosEnd) > 1.0f)
        {
            rbody.AddForce((ballPos - mousePosEnd) * shotStrength, ForceMode2D.Impulse);
        }
        
        aimLine.ClearAimLine();
    }

}
