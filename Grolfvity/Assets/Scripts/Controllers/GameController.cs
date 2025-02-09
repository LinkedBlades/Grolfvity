using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public int levelStrokes {  get; private set; }
    public int totalStrokes { get; private set; }
    public float timer { get; private set; }

    //Instantiate singleton
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer = Time.realtimeSinceStartup;

    }

    private void StartGame()
    {
        SoundController.Instance.PlayBGM();
    }

    public void IncrementHits()
    {
        //Debug.Log("Hits incremented");
        levelStrokes++;
        totalStrokes++;
    }

}
