using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static SceneController Instance;

    public string currentLevelName { get; private set; }

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

    //This is how levels are named in the ./Scenes folder
    //We just add the level number after this prefix
    private const string LevelScenePrefix = "Playtest2";

    public int currLevel;

    // Start is called before the first frame update
    void Start()
    {
        //Set resolution and neccesary references
        Screen.SetResolution(1920, 1080, true);

        //StartGame();
    }

    //Loads initial scenes for game to start
    public void StartGame()
    {
        if (!SceneManager.GetSceneByName("Playtest2_1").isLoaded)
        {
            SceneManager.LoadSceneAsync("Playtest2_1", LoadSceneMode.Additive);
        }

        //Loop through all scenes and unload every level but level 1 and persistent elements
        for (int i = 0; i < SceneManager.sceneCount; i++)
        { 
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "Playtest2_1" && scene.name != "PersistentElements")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

    }

    public void LoadLevel(string nextLevel)
    {
        string nextSceneName = LevelScenePrefix + nextLevel;
        //Check if there is a level to load and if its not already loaded
        if (nextSceneName != "" && !SceneManager.GetSceneByName(nextSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
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
