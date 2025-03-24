using System.Collections;
using System.Collections.Generic;
using TMPro;

//using Unity.VisualScripting;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    [Header("Menu Canvas")]
    [SerializeField] public GameObject menuCanvas;

    [Header("Start menu")]
    [SerializeField] public GameObject startMenu;

    [Header("Pause menu")]
    [SerializeField] public GameObject pauseMenu;

    [Header("Level select menu")]
    [SerializeField] public GameObject levelSelectMenu;

    [Header("Level select error message")]
    [SerializeField] public GameObject levelSelectError;

    [Header("End screen")]
    [SerializeField] public GameObject endScreen;

    [Header("Level timer")]
    [SerializeField] public GameObject levelTimer;

    [Header("Shots counter")]
    [SerializeField] public GameObject shotsCounter;

    [Header("End game score")]
    [SerializeField] public GameObject endGameScore;

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
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.GetCurrentState() == GameController.GameState.Playing)
        {
            UpdateTimer();
        }
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
        int totalStrokes = GameController.Instance.totalStrokes;

        TMP_Text shotsText = shotsCounter.GetComponent<TMP_Text>();
        shotsText.text = string.Format("Shots: {0}", totalStrokes);
    }

    public void UpdateEndScore()
    {
        float timer = GameController.Instance.gameTimer;
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        int totalStrokes = GameController.Instance.totalStrokes;

        TMP_Text endScoreText = endGameScore.GetComponent<TMP_Text>();
        endScoreText.text = string.Format("Shots: {0} - Time: {1:00}:{2:00} ", totalStrokes, minutes, seconds);
    }

    public void DestroyElement(GameObject gameObject)
    {
        Destroy(gameObject);
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
