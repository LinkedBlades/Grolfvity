using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{

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


}
