using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public int levelStrokes {  get; private set; }
    public int totalStrokes { get; private set; }
    public float timer { get; private set; }
    public string currentLevelSuffix { get; private set; }

    public GameState currentState { get; private set; }
    //For passing ball state to planets
    public BallBehaviour.BallState ballState { get;  set; }

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
        currentLevelSuffix = FindAnyObjectByType<NextLevel>().nextLevelNumber;
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
                HandleLoadingNextLevel();
                break;

            case GameState.Menu:
                break;
        }

    }

    //-----------------------------------State transitions handling-----------------------------------//

    private void HandleStarting()
    {
        SceneController.Instance.StartGame();
        Time.timeScale = 0;
        ChangeGameState(GameState.Pause);
    }

    private void HandlePlaying()
    {
        //Update current level suffix
        //currentLevelSuffix = FindAnyObjectByType<NextLevel>().currentLevelNumber;
        Debug.Log("CURRENT LEVEL SUFFIX " + currentLevelSuffix);
         
        Time.timeScale = 1;
        SoundController.Instance.PlayBGM();
    }
    private void HandlePause()
    {
        Time.timeScale = 0;
    }
    private void HandleLoadingNextLevel()
    {
        string nextLevelSuffix = FindAnyObjectByType<NextLevel>().nextLevelNumber;

        Debug.Log("NEXT LEVEL SUFFIX " + nextLevelSuffix);
        //Load level complete screen, Load next level, Unload current Level
        
        //UI Controller load menu screen

        //Try to unload current level
        if (SceneController.Instance.UnloadLevel(currentLevelSuffix))
        {
            SceneController.Instance.LoadLevel(nextLevelSuffix);
            currentLevelSuffix = nextLevelSuffix;
            ChangeGameState(GameState.Playing);
        }

    }
    ////-----------------------------------Extra functions----------------------------------- ////

    public void IncrementHits()
    {
        //Debug.Log("Hits incremented");
        levelStrokes++;
        totalStrokes++;
    }

    public BallBehaviour.BallState getBallState()
    {
            return ballState;
    }

}
