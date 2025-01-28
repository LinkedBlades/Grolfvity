using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballInHoleCheck : MonoBehaviour
{

    GameObject ball;


    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && collision.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.ballInHole);
        }
    }

}
