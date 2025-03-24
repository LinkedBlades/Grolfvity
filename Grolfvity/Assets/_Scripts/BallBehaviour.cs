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
    //Used to increase drag in ball if flying for too long due to orbiting a planet
    private float ballFlyingTimer;

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

    //Particle system for hotspot
    ParticleSystem hotspotIndicator;

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
        bounceCount = 0;
    }
    
    void Start()
    {
        ballSpawn = GameObject.Find("SpawnPoint").transform.position;
        this.transform.position = ballSpawn;

        hotspotIndicator = GetComponentInChildren<ParticleSystem>();

        cam = Camera.main;

        //Set ball initial state - Spawn the ball moving so 
        currentState = BallState.Moving;
        SwitchState(BallState.Stationary);
    }

    void FixedUpdate()
    {

        //Countdowns when ball stops moving
        if (rbody.velocity.magnitude <= minSpeedCheck && ballStoppedTimer > 0)
        {
            ballStoppedTimer -= Time.deltaTime;
        }

        //Check if ball has come to a stop
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
        if (newState == currentState)
        {
            return;
        }
        currentState = newState;

        //Decided to use switch instead of ifs although it saves only one check
        switch(newState)
        {
            case BallState.Stationary:
                HandleStationary();
                break;

            case BallState.Moving:
                HandleMoving();
                break;
        }

    }

    private void HandleStationary()
    {
        RenderHotspot(true);
        //Stop ball and set drag back to normal value in case of increased due to long orbiting
        rbody.velocity = Vector2.zero;
        rbody.angularVelocity = 0;
        rbody.drag = drag;
        ballFlyingTimer = 0;
        SoundController.Instance.PlaySFX(SoundController.Instance.ballReady, 0.1f);
        GameController.Instance.ballState = BallState.Stationary;
    }
    private void HandleMoving()
    {
        RenderHotspot(false);
        ballStoppedTimer = 0.5f;
        GameController.Instance.ballState = BallState.Moving;
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
        if (state)
        {
            hotspotIndicator.Play();
        }
        else
        {
            hotspotIndicator.Clear();
            hotspotIndicator.Stop();
        }
    }

    //--------------------------------------------------Events--------------------------------------------------//

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Stop ball after X bounces
        if(col.gameObject.name == "Hitbox")
        {   
            //Only stop ball when bouncing off planet surfaces
            if(col.gameObject.tag != "Obstacle")
            {
                bounceCount++;
            }

            SoundController.Instance.PlaySFX(SoundController.Instance.ballBounce, 0.8f);
            if (bounceCount == bounceCap)
            {
                rbody.velocity = Vector2.zero;
                bounceCount = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Hole")
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.ballInHole, 0.3f);
            //Respawn ball in final level
            if (GameController.Instance.levelHolesLeft > 1)
            {
                BallRespawn();
            }
        }

        if(collision.tag == "BlackHole")
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.outOfBounds, 1.0f);
            BallToPreviousPosition();
        }

        //Setting ball drag to X value ewhen entering planet field
        if(collision.tag == "PlanetField")
        {
            rbody.drag = drag;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentState == BallState.Moving)
        {
            ballFlyingTimer += Time.deltaTime;
        }
        //Increase drag 10% every frame when ball gest stuck orbiting 
        if (ballFlyingTimer > 5.0f)
        {
            rbody.drag += 0.001f;
        }
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
                aimLine.UpdateLineRenderer(aimLineIni, aimLineEnd, maxLineLenght);
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
