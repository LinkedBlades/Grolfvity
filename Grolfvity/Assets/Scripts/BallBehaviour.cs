using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Timeline.Actions;
using UnityEditorInternal;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    //Getting rigid body
    Rigidbody2D rbody;
    aimLine aimLine;

    [SerializeField] float ballSpeedCap;

    //Drag
    [SerializeField] float drag = 0.2f;

    //Shot variables
    Vector2 mousePosEnd;
    Vector2 ballPos;
    [SerializeField] float minSpeedCheck = 0.1f;
    [SerializeField] float ballStoppedTimer = 1.0f;
    [SerializeField] float shotStrength = 1.0f;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;
    [SerializeField] float maxLineLenght = 10.0f;

    //Checking for number of bounces
    [Header("Boucne count cap for testing")]
    [SerializeField]private int bounceCount = 0;


    //Temporary debug variables
    [SerializeField] float ySpeed;
    [SerializeField] float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        aimLine = this.GetComponentInChildren<aimLine>();

        rbody.velocity = new Vector2(xSpeed, ySpeed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("Mouse button clicked");
        //}

        if (rbody.velocity.magnitude <= minSpeedCheck && ballStoppedTimer > 0)
        {
            ballStoppedTimer -= Time.deltaTime;
        }

        if (ballStoppedTimer < 0)
        {
            Debug.Log("You can shoot now");
        }

        if (rbody.velocity.magnitude >= ballSpeedCap)
        {
            rbody.velocity = rbody.velocity.normalized * ballSpeedCap;
        }

    }


    //Events

    private void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.name == "Hitbox")
        {
            bounceCount++;
            SoundController.Instance.PlaySFX(SoundController.Instance.ballBounce, 0.5f);
            Debug.Log(bounceCount);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Setting ball drag to X value ewhen entering planet field
        rbody.drag = drag;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Setting ball drag to zero when leaving a planet
        rbody.drag = 0f;
    }


    //Hitting ball 

    private void OnMouseDown()
    {
        //Ball stopped check
        if (ballStoppedTimer <= 0)
        {
            ballPos = new Vector2(transform.position.x, transform.position.y);

            aimLineIni = ballPos;
        }
    }

    private void OnMouseDrag()
    {

        //Variables for storing direction and lengtt of aim line
        Vector2 aimDirection;
        float aimMagnitude;

        //Ball stopped check
        if (ballStoppedTimer <= 0)
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
        }
    }

    private void OnMouseUp()
    {
        //Check distance before shooting
        if (Vector2.Distance(ballPos, mousePosEnd) > 1.0f && ballStoppedTimer <= 0)
        {
            rbody.AddForce((ballPos - mousePosEnd) * shotStrength, ForceMode2D.Impulse);
            ballStoppedTimer = 1.0f;

            //Increment hit count in game controller
            GameController.Instance.IncrementHits();
            SoundController.Instance.PlaySFX(SoundController.Instance.ballHit);
            //Clear vertices to stop drawing line
            aimLine.ClearAimLine();

        }

    }

}
