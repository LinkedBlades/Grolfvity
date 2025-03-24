using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitingobstacles : MonoBehaviour
{
    [Header("Planet to orbit around")]
    [SerializeField] GameObject planet;

    //Variables for physics
    [Header("Speed of rotation positive=clockwise, negative=anticlockwise")]
    [SerializeField] float speed = 1.0f;

    //Distance to the planet
    private float radius;
    //Angle of rotatoin 
    private float angle;

    private Rigidbody2D rbody;

    private void Start()
    {
        rbody = GetComponentInChildren<Rigidbody2D>();

        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rbody.gravityScale = 0;

        radius = Vector2.Distance(transform.position, planet.transform.position);
    }

    private void FixedUpdate()
    {

        //Angle to move in orbit path
        angle += speed * Time.deltaTime;

        //Calculate new position in orbit
        Vector3 nextPosition = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);

        Vector2 newPosition = planet.transform.position + nextPosition;

        rbody.MovePosition(newPosition);

    }
}
