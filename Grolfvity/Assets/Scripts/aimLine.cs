using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEditorInternal;
using UnityEngine;

public class aimLine : MonoBehaviour
{

    //Variables for drawing aim line
    private Vector2 startVertex;
    private Vector2 mousePosEnd;
    [SerializeField] Material mat;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

    }

    private void OnMouseDown()
    {
        startVertex = Input.mousePosition;
        startVertex = Camera.main.ScreenToWorldPoint(startVertex);
    }

    private void OnMouseDrag()
    {
        mousePosEnd = Input.mousePosition;
        mousePosEnd = Camera.main.ScreenToWorldPoint(mousePosEnd);
        drawAimLine(startVertex);

    }

    public void drawAimLine(Vector2 startVertex)
    {
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
