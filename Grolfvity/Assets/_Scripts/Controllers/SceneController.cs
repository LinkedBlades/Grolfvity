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
        currLevel = 1;
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        //Unload levels loaded in the editor
        UnloadLevels();

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
        //Check if there is a level to load. If level already loaded it will just reload
        if (nextSceneName != "")
        {
            if (levelNum <= GameController.Instance.levelReached)
            {
                //Unload level
                UnloadCurrentLevel();
                
                //Unload all other levels
                //UnloadLevels();

                //Update current level if loading a lower level
                currLevel = levelNum;
                //Load new level
                AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

                //UnPause game if called from level select button
                GameController.Instance.ChangeGameState(GameController.GameState.Playing);
            }
            else
            {
                SoundController.Instance.PlaySFX(SoundController.Instance.levelSelectError);
                UIcontroller.Instance.ActivateUI(UIcontroller.Instance.levelSelectError);
            }

        }

    }

    public bool UnloadLevel(string levelSuffix)
    {
        try
        {
            Debug.Log(LevelScenePrefix + levelSuffix);
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
            //Debug.Log("Current level to unload: " + LevelScenePrefix + currLevel);
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
