using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [Header("Next level to load number as string")]
    [SerializeField] public int nextLevelNumber;

    [Header("Current level to load number as string")]
    [SerializeField] public int currentLevelNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if(nextLevelNumber == 0)
            {
                SceneController.Instance.RestartLevel();
            }
            else
            {
                GameController.Instance.ChangeGameState(GameController.GameState.Loading);
            }

        }
    }

}
