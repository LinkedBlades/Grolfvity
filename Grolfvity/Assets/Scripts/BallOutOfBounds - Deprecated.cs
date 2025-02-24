using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOutOfBounds : MonoBehaviour
{

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
           CheckBallOutOfBounds();
    }

    private void CheckBallOutOfBounds()
    {
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);

        if(screenPos.x < 0 || screenPos.x > cam.pixelWidth || screenPos.y < 0 || screenPos.y > cam.pixelHeight)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.outOfBounds, 0.1f);
        }

    }

}
