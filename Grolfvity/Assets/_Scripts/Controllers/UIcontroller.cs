using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    [Header("Menu Canvas")]
    [SerializeField] public GameObject menuCanvas;

    [Header("Pause menu")]
    [SerializeField] public GameObject pauseMenu;

    [Header("Level select menu")]
    [SerializeField] public GameObject levelSelectMenu;

    [Header("Level select error message")]
    [SerializeField] public GameObject levelSelectError;

    public static UIcontroller Instance;

    //Instantiate singleton
    private void Awake()
    {
        if (Instance == null)
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
        pauseMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        levelSelectError.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelName()
    {
        string levelName = SceneController.Instance.currentLevelName;
        /*
         * Should load the Level component in UI
         */
    }

    public void LoadLevelTimer()
    {
        float timer = GameController.Instance.timer;
        /*
         * Should load the timer component in UI
         */ 
    }

    public void LoadShotsTaken()
    {
        int levelStrokes = GameController.Instance.levelStrokes;
        int totalStrokes = GameController.Instance.totalStrokes;
    }

    public void DestroyElement(GameObject gameObject)
    {
        Debug.Log("Destroying element");
        Destroy(gameObject);
    }

    public void PauseButton()
    {
        GameController.Instance.ChangeGameState(GameController.GameState.Pause);
    }

    public void ActivateUI(GameObject element)
    {
        if(!element.activeSelf)
        {
            element.SetActive(true);
        }
    }

    public void DeactivateUI(GameObject element)
    {
        if (element.activeSelf)
        {
            element.SetActive(false);
        }
    }
}
