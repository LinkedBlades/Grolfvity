using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class aimLine : MonoBehaviour
{
    //Line renderer reference, asigned in inspector
    [SerializeField] LineRenderer lineRenderer;

    //Array to store points for line renderer
    Vector3[] positions;

    private void Start()
    {
        //Initializing positions array
        positions = new Vector3[2];
        lineRenderer = GetComponentInParent<LineRenderer>();
    }

    private void Update()
    {
        DrawAimLine();
    }

    public void OnMouseUp()
    {
        Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void UpdateLineRenderer(Vector2 start, Vector2 end)
    {
        positions[0] = start;
        positions[1] = end;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].z = 0f;
        }

        lineRenderer.positionCount = positions.Length;

    }
    private void DrawAimLine()
    {
        if (lineRenderer.positionCount > 0)
        {
            lineRenderer.SetPositions(positions);
        }
    }

    public void ClearAimLine()
    {
        lineRenderer.positionCount = 0;
    }

}
