using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetForces : MonoBehaviour
{
    //Variables for controlling gravity field
    [Header("Replaces G*m1*m2")]
    [SerializeField] float pullMagnitude;
    [Header("Factor 1/distCap for reduction in pull strenght in field edge")]
    [SerializeField] float distCap;

    //Variables used for remapping force magnitude
    private float planetRadius;
    private float fieldRadius;
    private float oldRangeMin;
    private float oldRangeMax;
    private float newRangeMin;
    private float newRangeMax;
    private float eventHorizonRadius;


    // Start is called before the first frame update
    void Start()
    {
        //Getting planet and field radii for force calculation later
        CircleCollider2D[] allColliders = transform.parent.GetComponentsInChildren<CircleCollider2D>();
        for(int i = 0; i < allColliders.Length; i++)
        {
            if(allColliders[i].gameObject.name == "Hitbox")
            {
                planetRadius = allColliders[i].radius;
            }
            else if (allColliders[i].gameObject.name == "Field")
            {
                fieldRadius = allColliders[i].radius;
            }
            else
            {
                eventHorizonRadius = allColliders[i].radius;
            }
        }
        
        if(this.gameObject.name == "EventHorizon")
        {
            Debug.Assert(planetRadius > 0);
            Debug.Assert(fieldRadius > 0);
            Debug.Assert(eventHorizonRadius > 0);
        }

        //Case diferentiation for using same script on BlackHoles
        if(this.gameObject.name == "Field")
        {
            oldRangeMax = Mathf.Pow(fieldRadius, 2);

        }

        if(this.gameObject.name == "EventHorizon")
        {
            oldRangeMax = Mathf.Pow(eventHorizonRadius, 2);
        }

        oldRangeMin = Mathf.Pow(planetRadius, 2);
        newRangeMin = oldRangeMin;
        newRangeMax = distCap * oldRangeMin;

    }

    //Event functions
    //Pull ball when on field range
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            //Only applying force when ball is moving
            if( GameController.Instance.getBallState() == BallBehaviour.BallState.Moving)
            {
                Vector2 gravVector = GravityPull(collision.transform);
                collision.attachedRigidbody.AddForce(gravVector);

            }
        }
    }

    private Vector2 GravityPull(Transform ballTransform)
    {
        //Distance between objects transforms
        Vector2 difference = this.transform.position - ballTransform.position;
        float distToBall = difference.magnitude;

        float distFactor; // How much to scale down the pull foce based on distance
        
        if(distCap == 0)
        {
            //F = G * (m1*m2) / r^2
            distFactor =  Mathf.Pow(distToBall, 2);
        }
        else
        {
            //Remapping force scaling based on distance
            float temporary = Mathf.InverseLerp(oldRangeMax, oldRangeMin, Mathf.Pow(distToBall, 2));
            distFactor = Mathf.Lerp(newRangeMax, newRangeMin, temporary);
        }

        float forceMagnitude = pullMagnitude / distFactor;

        Vector2 pullDirection = difference.normalized;
        Vector2 gravVector = forceMagnitude * pullDirection;

        Debug.DrawRay(ballTransform.position, gravVector, Color.yellow);

        return gravVector;
    }

    //Not used
    private Vector2 GravityPull2(Transform ballTransform)
    {
        //Distance between objects transforms
        Vector2 difference = this.transform.position - ballTransform.position;
        float distToBall = difference.magnitude;

        //Testing using r instead of r^2
        //F = G * (m1*m2) / r 

        float forceMagnitude = pullMagnitude / distToBall;

        Vector2 pullDirection = difference.normalized;
        Vector2 gravVector = forceMagnitude * pullDirection;

        Debug.DrawRay(ballTransform.position, gravVector, Color.yellow);

        return gravVector;
    }


}
