using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Timeline.Actions;
using UnityEditorInternal;
using UnityEngine;

public class aimLine : MonoBehaviour
{
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] LineRenderer lineRenderer;

    Vector2 startMousePos;
    Vector2 currentMousePos;
    Vector3[] positions;

    private void Start()
    {
        positions = new Vector3[2];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            positions[0] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0))
        {
            positions[1] = -Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //Debug.Log("mouseDown ")

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
