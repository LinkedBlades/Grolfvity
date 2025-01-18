using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int levelHits {  get; private set; }
    public int totalHits { get; private set; }

    private float timer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer = Time.realtimeSinceStartup;

    }

    public void IncrementHits()
    {
        levelHits++;
        totalHits++;
    }

    //Simply returns time for UI controller
    public float GetTime()
    {
        return timer;
    }

}
