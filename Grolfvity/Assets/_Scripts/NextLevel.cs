using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [Header("Next level to load number as string")]
    [SerializeField] public string nextLevelNumber;

    [Header("Current level to load number as string")]
    [SerializeField] public string currentLevelNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            GameController.Instance.ChangeGameState(GameController.GameState.Loading);

            //if(SceneController.Instance.UnloadCurrentLevel())
            //{
            //    SceneController.Instance.LoadLevel(nextLevelName);
            //}

        }
    }

}
