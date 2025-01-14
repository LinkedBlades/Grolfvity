using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetForces : MonoBehaviour
{
    //Variables for controlling gravity field
    [SerializeField] float pullMagnitude = 1;
    [SerializeField] float radius;

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
        //if(collision.gameObject.tag == "Ball")
        //{
        //    Debug.Log("Ball entered field");
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("On trigger stay2d call");
        if(collision.gameObject.tag == "Ball")
        {

            float distanceModifier = Vector3.Distance(this.transform.position, collision.transform.position);
            Vector2 pullDirection = Vector3.Normalize(this.transform.position - collision.transform.position);
            Vector2 gravPull = (pullMagnitude * pullDirection) / (Mathf.Pow(distanceModifier, 2 ) * 2);

            Debug.DrawRay(collision.transform.position, gravPull, Color.yellow);
            //Debug.Log("grav pull" + gravPull);


            collision.attachedRigidbody.AddForce(gravPull, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Ball")
        //{
        //    Debug.Log("Ball left field");
        //}
    }

}
