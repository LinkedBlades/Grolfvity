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
    [SerializeField] float magnitude;
    //[SerializeField] float friction;

    //Shot variables
    Vector2 mousePosIni;
    Vector2 mousePosEnd;
    Vector2 ballPos;
    Vector2 startVertex;
    [SerializeField] Material mat;
    [SerializeField] float shotStrength = 1;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;
    GameObject aimLine;
    //LineRenderer currentLineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Debug.DrawRay(this.transform.position, rbody.velocity, Color.red);

        if (mousePosEnd != null && ballPos != null)
        {
            Debug.DrawLine(ballPos, -mousePosEnd);

        }
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
        startVertex = ballPos;

        Debug.Log("Click on hotspot");

    }

    private void OnMouseDrag()
    {
        //Getting mouse position into world coordinates for aiming shot
        mousePosEnd = Input.mousePosition;
        mousePosEnd = Camera.main.ScreenToWorldPoint(mousePosEnd);

        //Updating aim line end
        aimLineEnd = ballPos - mousePosEnd;
        drawAimLIne();

    }

    private void OnMouseUp()
    {
        //ballPos = Camera.main.ScreenToWorldPoint(ballPos);
        rbody.AddForce((ballPos - mousePosEnd) * shotStrength, ForceMode2D.Impulse);
    }

    private void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(startVertex);
        GL.Vertex(new Vector3(mousePosEnd.x, mousePosEnd.y, 0));
        GL.End();

        GL.PopMatrix();
    }

    private void drawAimLIne()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(startVertex);
        GL.Vertex(new Vector3(mousePosEnd.x, mousePosEnd.y, 0));
        GL.End();

        GL.PopMatrix();
    }


}
