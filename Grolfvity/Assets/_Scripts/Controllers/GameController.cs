using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public int levelStrokes {  get; private set; } //Unused
    public int totalStrokes { get; private set; }
    public float gameTimer { get; private set; }

    public int levelReached;
    public int currentLevel { get; private set; }
    public int levelHolesLeft {  get; private set; }
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
        End
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

            case GameState.End:
                HandleEnd();
                break;

        }

    }

    //-----------------------------------State transitions handling-----------------------------------//

    //Initiates game start sequence
    private void HandleStarting()
    {
        SceneController.Instance.StartGame();
        Time.timeScale = 0;
        levelReached = 1;
        gameTimer = 0;
        totalStrokes = 0;
        levelStrokes = 0;
        currentLevel = 1;
        UIcontroller.Instance.UpdateShotsTaken(); //Update shots back to zero after reseting game
        GetHolesInLevel();
    }

    //Sets up game for playing state
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

    //Handles pause and menus
    private void HandlePause()
    {
        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.pauseMenu);

        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.levelTimer);
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.shotsCounter);

        SoundController.Instance.BGMVolume(0.02f);
        Time.timeScale = 0;
    }

    //Loads next level - Called when finishing a not final level
    private void HandleLoadingNextLevel()
    {
        levelStrokes = 0;
        //Update beaten levels
        if(levelReached <5)
        {
            levelReached++;
        }
        currentLevel++;
        //Load next level and unload current level
        SceneController.Instance.LoadLevel(currentLevel.ToString());

        ChangeGameState(GameState.Playing);
    }

     //Unused
    private void HandleRestart()
    {
        SceneController.Instance.RestartLevel();

        ChangeGameState(GameState.Playing);
    }

    //Starts end sequence - Called when beating last level
    private void HandleEnd()
    {

        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.levelTimer);
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.shotsCounter);

        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.endScreen);
        UIcontroller.Instance.UpdateEndScore();

        Time.timeScale = 0;
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

    }

    public BallBehaviour.BallState getBallState()
    {
            return ballState;
    }

    public void RestartGame()
    {
        UIcontroller.Instance.DeactivateUI(UIcontroller.Instance.endScreen);
        UIcontroller.Instance.ActivateUI(UIcontroller.Instance.startMenu);
        ChangeGameState(GameState.Starting);
    }
    public void GetHolesInLevel()
    {
        levelHolesLeft = GameObject.FindGameObjectsWithTag("Hole").Length;
    }
    public void DecreaseHolesRemaining()
    {
        levelHolesLeft--;
    }

    public void SetCurrentLevel(int levelNum)
    {
        currentLevel = levelNum;
    }

}
