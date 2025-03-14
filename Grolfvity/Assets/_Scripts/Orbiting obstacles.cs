using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitingobstacles : MonoBehaviour
{
    [Header("Planet to orbit around")]
    [SerializeField] GameObject planet;

    [Header("Speed in radial/s")]
    [SerializeField] float speed = 1.0f;

    [Header("Rotation direction 1/-1 Anti/clockwise")]
    [SerializeField] Vector3 direction = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(planet.transform.position, direction, speed * Time.deltaTime);
    }
}
