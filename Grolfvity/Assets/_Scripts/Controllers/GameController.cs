using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public int levelStrokes {  get; private set; }
    public int totalStrokes { get; private set; }
    public float timer { get; private set; }

    public GameState currentState { get; private set; }

    public enum GameState
    {
        Starting,
        Playing,
        Pause,
        Loading,
        Menu
    }


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
        currentState = GameState.Pause;
        ChangeGameState(GameState.Starting);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer = Time.realtimeSinceStartup;
    }


    public GameState GetCurrentState()
    {
        return currentState;
    }


    public void ChangeGameState(GameState newState)
    {
        if(newState == currentState)
        {
            return;
        }

        currentState = newState;

        switch(currentState)
        {
            case GameState.Starting:
                HandleStarting();
                break;

            case GameState.Playing:
                HandlePlaying();
                break;

            case GameState.Pause:
                HandlePause();
                break;

            case GameState.Loading:
                Debug.Log("Handling Loading");
                break;

            case GameState.Menu:
                Debug.Log("Handling Menu");
                break;
        }

    }

    private void HandleStarting()
    {
        Debug.Log("Current state: Starting");

        SceneController.Instance.StartGame();
        Time.timeScale = 0;
        ChangeGameState(GameState.Pause);
    }

    private void HandlePlaying()
    {
        Debug.Log("Current state: Playing");

        Time.timeScale = 1;
        SoundController.Instance.PlayBGM();
    }

    private void HandlePause()
    {
        Debug.Log("Current state: Pause");

        Time.timeScale = 0;
    }

    public void IncrementHits()
    {
        //Debug.Log("Hits incremented");
        levelStrokes++;
        totalStrokes++;
    }

}
