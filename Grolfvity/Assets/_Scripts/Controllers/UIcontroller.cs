using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    [Header("Level timer")]
    [SerializeField] public GameObject levelTimer;

    [Header("Shots counter")]
    [SerializeField] public GameObject shotsCounter;

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
        levelTimer.SetActive(false);
        shotsCounter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.GetCurrentState() == GameController.GameState.Playing)
        {
            UpdateTimer();
        }
    }

    public void LoadLevelName()
    {
        string levelName = SceneController.Instance.currentLevelName;
        /*
         * Should load the Level component in UI
         */
    }

    public void UpdateTimer()
    {
        float timer = GameController.Instance.gameTimer;
        
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        TMP_Text timerText = levelTimer.GetComponent<TMP_Text>();
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void UpdateShotsTaken()
    {
        int levelStrokes = GameController.Instance.levelStrokes;
        int totalStrokes = GameController.Instance.totalStrokes;

        TMP_Text shotsText = shotsCounter.GetComponent<TMP_Text>();
        shotsText.text = string.Format("Shots: {0}", totalStrokes);
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
