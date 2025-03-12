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
    private const string LevelScenePrefix = "Playtest2_Level";

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
        //Set resolution and neccesary references
        Screen.SetResolution(1920, 1080, true);

        currLevel = 1;
        //StartGame();
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        if (!SceneManager.GetSceneByName("Playtest3_Level1").isLoaded)
        {
            SceneManager.LoadSceneAsync("Playtest3_Level1", LoadSceneMode.Additive);
        }

        //Loop through all scenes and unload every level but level 1 and persistent elements
        for (int i = 0; i < SceneManager.sceneCount; i++)
        { 
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "Playtest3_Level1" && scene.name != "PersistentElements")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

    }

    public void LoadLevel(string levelSuffix)
    {
        string nextSceneName = LevelScenePrefix + levelSuffix;
        //Check if there is a level to load and if its not already loaded
        if (nextSceneName != "" && !SceneManager.GetSceneByName(nextSceneName).isLoaded)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            if(asyncOp.isDone)
            {
                Debug.Log("Loading level ready");
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
