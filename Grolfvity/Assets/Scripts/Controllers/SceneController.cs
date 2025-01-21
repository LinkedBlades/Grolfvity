using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Loads first level and permanent of the game
    private void LoadGame()
    {

    }

    public void LoadNextLevel(string levelName)
    {

    }

    public void UnloadPreviousLevel(string levelName)
    {

    }

}
