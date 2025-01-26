using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Set background image and scrolling speeds")]
    [SerializeField] private RawImage img;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, img.uvRect.size);
    }
}
