using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetForces : MonoBehaviour
{
    //Variables for controlling gravity field
    [Header("Replaces G*m1*m2")]
    [SerializeField] float pullMagnitude;

    //Ball reference for when coming field
    GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Event functions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Debug.Log("Collided with planet");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("On trigger stay2d call" + collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("Pull magnitude " + pullMagnitude);

            GravityPull(collision.attachedRigidbody, collision.transform);

            //float distanceModifier = Vector3.Distance(this.transform.position, collision.transform.position);
            //Vector2 pullDirection = Vector3.Normalize(this.transform.position - collision.transform.position);
            //Vector2 gravPull = (pullMagnitude * pullDirection) / (Mathf.Pow(distanceModifier, 2 ) * distanceDamping);

            //Debug.DrawRay(collision.transform.position, gravPull, Color.yellow);

            //collision.attachedRigidbody.AddForce(gravPull, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void GravityPull(Rigidbody2D ballRb, Transform ballTransform)
    {
        //Distance between objects transforms
        Vector2 difference = this.transform.position - ballTransform.position;
        float distanceModifier = difference.magnitude;

        //float distanceModifier = Vector3.Distance(this.transform.position, ballTransform.position);

        //F = G * (m1*m2) / r^2 
        float forceMagnitude = pullMagnitude / (Mathf.Pow(distanceModifier, 2));

        Vector2 pullDirection = difference.normalized;
        //Vector2 pullDirection = Vector3.Normalize(this.transform.position - ballTransform.position);
        Vector2 gravVector = forceMagnitude * pullDirection;

        Debug.DrawRay(ballTransform.position, gravVector, Color.yellow);

        ballRb.AddForce(gravVector);
    }
}
