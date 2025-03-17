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

    public int levelReached;
    public string currentLevelSuffix { get; private set; }
    public string nextLevelSuffix { get; private set; }

    public GameState currentState { get; private set; }
    //For passing ball state to planets
    public BallBehaviour.BallState ballState { get;  set; }

    public enum GameState
    {
        Starting,
        Playing,
        Pause,
        Loading,
        Restart,
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
                HandleLoadingNextLevel();
                break;

            case GameState.Restart:
                HandleRestart();
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
        levelReached = 1;
        ChangeGameState(GameState.Pause);
    }

    private void HandlePlaying()
    {
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.pauseMenu);
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.levelSelectMenu);
        if(SoundController.Instance.BGMPlaying()) SoundController.Instance.PlayBGM();
        SoundController.Instance.BGMVolume(0.08f);
        Time.timeScale = 1;
    }
    private void HandlePause()
    {
        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.pauseMenu);
        SoundController.Instance.BGMVolume(0.02f);
        Time.timeScale = 0;
    }
    private void HandleLoadingNextLevel()
    {
        //Load level complete screen, Load next level, Unload current Level

        //UI Controller load menu screen

        //Unload pre level
        SceneController.Instance.UnloadCurrentLevel();
        //Update beaten levels
        levelReached++;
        //Set current level
        SceneController.Instance.currLevel++;
        //Load next level
        SceneController.Instance.LoadLevel(SceneController.Instance.currLevel.ToString());

        ChangeGameState(GameState.Playing);

    }

    private void HandleRestart()
    {
        SceneController.Instance.RestartLevel();

        ChangeGameState(GameState.Playing);
    }
    ////-----------------------------------Extra functions----------------------------------- ////

    public void PauseGame()
    {
        ChangeGameState(GameState.Pause);
    }

    public void UnPause()
    {
        ChangeGameState(GameState.Playing);
    }

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
