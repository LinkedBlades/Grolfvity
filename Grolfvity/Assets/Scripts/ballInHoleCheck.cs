using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ballInHoleCheck : MonoBehaviour
{
    Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.ballInHole, 0.1f);
        }
    }

}
