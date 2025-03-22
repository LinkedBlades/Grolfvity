using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public int levelStrokes {  get; private set; }
    public int totalStrokes { get; private set; }
    public float gameTimer { get; private set; }

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
        if (currentState == GameState.Playing)
        {
            gameTimer += Time.deltaTime;
        }
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

        }

    }

    //-----------------------------------State transitions handling-----------------------------------//

    private void HandleStarting()
    {
        SceneController.Instance.StartGame();
        Time.timeScale = 0;
        levelReached = 1;
        gameTimer = 0;
        totalStrokes = 0;
        levelStrokes = 0;
        ChangeGameState(GameState.Pause);
    }

    private void HandlePlaying()
    {
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.pauseMenu);
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.levelSelectMenu);

        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.levelTimer);
        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.shotsCounter);

        if(SoundController.Instance.BGMPlaying()) SoundController.Instance.PlayBGM();
        SoundController.Instance.BGMVolume(0.08f);
        Time.timeScale = 1;
    }
    private void HandlePause()
    {
        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.pauseMenu);

        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.levelTimer);
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.shotsCounter);

        SoundController.Instance.BGMVolume(0.02f);
        Time.timeScale = 0;
    }
    private void HandleLoadingNextLevel()
    {
        levelStrokes = 0;

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

    //Called from ball every time a a shot is made
    public void IncrementHits()
    {
        levelStrokes++;
        totalStrokes++;
        UIcontroller.Instance.UpdateShotsTaken();

        Debug.Log("Shots taken:" + levelStrokes);

    }

    public BallBehaviour.BallState getBallState()
    {
            return ballState;
    }

    public void RestartGame()
    {
        ChangeGameState(GameState.Starting);
    }

}
