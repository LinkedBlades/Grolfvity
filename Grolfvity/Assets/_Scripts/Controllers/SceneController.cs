using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    public string currentLevelName { get; private set; }

    //This is how levels are named in the ./Scenes folder
    //We just add the level number after this prefix
    private const string LevelScenePrefix = "Playtest3_Level";

    public int currLevel;

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
        //Set resolution and current level
        Screen.SetResolution(1920, 1080, true);
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        //Unload levels loaded in the editor
        UnloadLevels();
        currLevel = 1;
        //Load game first level
        if (!SceneManager.GetSceneByName("Playtest3_Level1").isLoaded)
        {
            SceneManager.LoadSceneAsync("Playtest3_Level1", LoadSceneMode.Additive);
        }

    }

    private void UnloadLevels()
    {
        //Loop through all scenes and unload every level scene
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "PersistentElements")
            {
                if(scene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
    }

    public void LoadLevel(string levelSuffix)
    {
        int levelNum = int.Parse(levelSuffix);
        string nextSceneName = LevelScenePrefix + levelSuffix;
        if (nextSceneName != "")
        {
            if (levelNum <= GameController.Instance.levelReached)
            {
                UnloadCurrentLevel();
                //Update current level to new level number
                currLevel = levelNum;
                GameController.Instance.SetCurrentLevel(currLevel);
                //Load new level
                SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
                //UnPause game if called from level select button
                GameController.Instance.ChangeGameState(GameController.GameState.Playing);
            }
            else
            {
                //In case user selects level that has not been reached pop up error menu
                SoundController.Instance.PlaySFX(SoundController.Instance.levelSelectError);
                UIcontroller.Instance.ActivateUI(UIcontroller.Instance.levelSelectError);
            }

        }

    }

    public bool UnloadLevel(string levelSuffix)
    {
        try
        {
            SceneManager.UnloadSceneAsync(LevelScenePrefix + levelSuffix);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public bool UnloadCurrentLevel()
    {
        try
        {
            SceneManager.UnloadSceneAsync(LevelScenePrefix + currLevel);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public void RestartLevel()
    {
        if (UnloadCurrentLevel())
        {
            LoadLevel(currLevel.ToString());
        }
        else
        {
            RestartLevel();
        }
    }

}
