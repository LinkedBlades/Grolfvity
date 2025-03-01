using System;
using System.Net.Http.Headers;
using Unity.VisualScripting;


/*using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;*/
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    Camera cam;

    //Getting rigid body
    Rigidbody2D rbody;
    aimLine aimLine;

    [Header("Ball max speed limit")]
    [SerializeField] float ballSpeedCap;

    //Drag
    [Header("Ball drag inside planet grav field")]
    [SerializeField] float drag = 0.2f;

    //Shot variables
    Vector2 mousePosEnd;
    Vector2 ballPos;

    [Header("Min speed threshold to stop ball")]
    [SerializeField] float minSpeedCheck = 0.1f;

    [Header("Experimenting with stopping ball after X time")]
    [SerializeField] float ballStoppedTimer = 1.0f;

    [Header("Main control of shot strength")]
    [SerializeField] float shotStrenghtMultiplier = 1.0f;
    float shotStrenght;

    //Aim Line variables
    Vector2 aimLineIni;
    Vector2 aimLineEnd;
    [Header("Max lenght of aim line")]
    [SerializeField] float maxLineLenght = 10.0f;

    //Checking for number of bounces
    [Header("Bounce count cap for testing")]
    [SerializeField] int bounceCap = 3;
    private int bounceCount;

    [Header("Pixel tolerance for out of bounds")]
    [SerializeField] int oobTol = 1;

    //Ball last position
    Vector2 prevPosition;
    Vector2 ballSpawn;

    //References needed to enable/disable hotspot sprite
    SpriteMask mask;
    GameObject hotspotRange;
    SpriteRenderer spriteRenderer;

    //Ball states
    public enum BallState
    {
        Stationary,
        Moving
    }
    private BallState currentState;

    void Awake()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        aimLine = this.GetComponentInChildren<aimLine>();
        mask = this.GetComponentInChildren<SpriteMask>();
        bounceCount = 0;

    }
    
    void Start()
    {
        ballSpawn = GameObject.Find("SpawnPoint").transform.position;
        this.transform.position = ballSpawn;

        hotspotRange = GameObject.Find("Range");
        spriteRenderer = hotspotRange.GetComponent<SpriteRenderer>();

        cam = Camera.main;

    }

    void FixedUpdate()
    {
        //Check if ball has topped for enough time
        if (rbody.velocity.magnitude <= minSpeedCheck && ballStoppedTimer > 0)
        {
            ballStoppedTimer -= Time.deltaTime;
        }

        //Render hotspot sprite only when able to shoot ball
        if (ballStoppedTimer <= 0)
        {
            SwitchState(BallState.Stationary);
        }

        //Stop ball from going over speed cap
        if (rbody.velocity.magnitude >= ballSpeedCap)
        {
            rbody.velocity = rbody.velocity.normalized * ballSpeedCap;
        }

        CheckBallOutOfBounds();

    }

    //--------------------------------------------------State machine---------------------------------------------------//

    private void SwitchState (BallState newState)
    {
        currentState = newState;

        //Decided to use switch instead of ifs although it saves only one check
        switch(newState)
        {
            case BallState.Stationary:
                RenderHotspot(true);
                rbody.velocity = Vector2.zero;
                rbody.angularVelocity = 0;
                SoundController.Instance.PlaySFX(SoundController.Instance.ballReady , 0.05f);
                break;

            case BallState.Moving:
                RenderHotspot(false);
                ballStoppedTimer = 0.5f;
                break;
        }

    }
    public BallState GetCurrentState()
    {
        return currentState;
    }

    //--------------------------------------------------Helper methods--------------------------------------------------//

    //Checks ball stays within camera boundaries
    private void CheckBallOutOfBounds()
    {
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);

        if (screenPos.x < -oobTol || screenPos.x > cam.pixelWidth + oobTol || 
            screenPos.y < -oobTol || screenPos.y > cam.pixelHeight + oobTol)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.outOfBounds, 1.0f);
            //Move ball back to valid position
            BallToPreviousPosition();
        }

    }

    //Stops the ball and moves it back to position before last shot
    private void BallToPreviousPosition()
    {
        transform.position = new Vector2(prevPosition.x, prevPosition.y);
        SwitchState(BallState.Stationary);
    }

    private void BallRespawn()
    {
        transform.position = new Vector2(ballSpawn.x, ballSpawn.y);
        SwitchState(BallState.Stationary);
    }

    private void RenderHotspot(bool state)
    {
        spriteRenderer.enabled = state;
        mask.enabled = state;
    }

    //--------------------------------------------------Events--------------------------------------------------//

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Stop ball after X bounces
        if(col.gameObject.name == "Hitbox")
        {
            bounceCount++;
            SoundController.Instance.PlaySFX(SoundController.Instance.ballBounce, 1.0f);
            if (bounceCount == bounceCap)
            {
                //rbody.velocity = Vector2.zero;
                bounceCount = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Temporary for Playtest1
        if(collision.name == "Hole")
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.ballInHole, 0.4f);
            BallRespawn();
        }

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

        }
    }

    private void OnMouseDrag()
    {
        //Variables for storing direction and lengtt of aim line
        Vector2 aimDirection;
        float aimMagnitude;

        //Check correct ball state
        if (currentState == BallState.Stationary)
        {
            ballPos = new Vector2(transform.position.x, transform.position.y);
            aimLineIni = ballPos;

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
                aimLine.UpdateLineRenderer(aimLineIni, aimLineEnd);
            }

            if (Vector2.Distance(aimLineIni, mousePosEnd) <= 1.0f)
            {
                aimLine.ClearAimLine();
            }
        }
    }

    private void OnMouseUp()
    {
        //Check distance before shooting and correct ball state
        if (Vector2.Distance(ballPos, mousePosEnd) > 1.0f && currentState == BallState.Stationary)
        {
            prevPosition = transform.position;

            rbody.AddForce((ballPos - mousePosEnd).normalized * shotStrenght * shotStrenghtMultiplier, ForceMode2D.Impulse);
            
            GameController.Instance.IncrementHits();
            SoundController.Instance.PlaySFX(SoundController.Instance.ballHit);
            
            aimLine.ClearAimLine();

            SwitchState(BallState.Moving);
        }
    }
}
