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
    Camera cam;

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
    [SerializeField] float shotStrenghtMultiplier = 1.0f;
    float shotStrenght;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;
    [SerializeField] float maxLineLenght = 10.0f;

    //Checking for number of bounces
    [Header("Bounce count cap for testing")]
    [SerializeField] int bounceCap = 3;
    private int bounceCount;

    [Header("Pixel tolerance for out of bounds")]
    [SerializeField] int oobTol = 1;

    //Ball last position
    Vector2 prevPosition;


    void Awake()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        aimLine = this.GetComponentInChildren<aimLine>();
        cam = Camera.main;

        bounceCount = 0;

    }

    void FixedUpdate()
    {
        if (rbody.velocity.magnitude <= minSpeedCheck && ballStoppedTimer > 0)
        {
            ballStoppedTimer -= Time.deltaTime;
            //Stop rendering hotspot sprite
        }

        if (ballStoppedTimer < 0)
        {
            Debug.Log("You can shoot now");
            //Render hotspot sprite TO DO
        }

        //Stop ball from going over speed cap
        if (rbody.velocity.magnitude >= ballSpeedCap)
        {
            rbody.velocity = rbody.velocity.normalized * ballSpeedCap;
        }

        CheckBallOutOfBounds();

    }

    //--------------------------------------------------Helper methods--------------------------------------------------//

    //Checks ball stays within camera boundaries
    private void CheckBallOutOfBounds()
    {
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);

        if (screenPos.x < -oobTol || screenPos.x > cam.pixelWidth + oobTol || 
            screenPos.y < -oobTol || screenPos.y > cam.pixelHeight + oobTol)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.outOfBounds, 0.5f);
            //Move ball back to valid position
            BallToPreviousPosition();
        }

    }

    //Stops the ball and moves it back to position before last shot
    private void BallToPreviousPosition()
    {
        transform.position = new Vector2(prevPosition.x, prevPosition.y);
        rbody.velocity = Vector2.zero;

        ballStoppedTimer = 0;
    }

    //--------------------------------------------------Events--------------------------------------------------//

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Stop ball after 3 bounces
        if(col.gameObject.name == "Hitbox")
        {
            bounceCount++;
            SoundController.Instance.PlaySFX(SoundController.Instance.ballBounce, 0.5f);
            if(bounceCount == bounceCap)
            {
                rbody.velocity = Vector2.zero;
                bounceCount = 0;
            }
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
            shotStrenght = aimMagnitude;
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
            prevPosition = transform.position;
            rbody.AddForce((ballPos - mousePosEnd).normalized * shotStrenght * shotStrenghtMultiplier, ForceMode2D.Impulse);
            ballStoppedTimer = 1.0f;

            //Increment hit count in game controller
            GameController.Instance.IncrementHits();
            SoundController.Instance.PlaySFX(SoundController.Instance.ballHit);
            //Clear vertices to stop drawing line
            aimLine.ClearAimLine();
        }
    }
}
