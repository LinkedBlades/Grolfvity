using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [Header("Next level to load number as string")]
    [SerializeField] public int nextLevelNumber;

    [Header("Unused - Current level to load number as string")]
    [SerializeField] public int currentLevelNumber; //Unused

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            GameController.Instance.GetHolesInLevel();
            //Updates game manager and check if there are holes remaining;
            if (GameController.Instance.levelHolesLeft > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                //Final level holes have 0 as their next level
                if (nextLevelNumber == 0)
                {
                    GameController.Instance.ChangeGameState(GameController.GameState.End);
                }
                else
                {
                    GameController.Instance.ChangeGameState(GameController.GameState.Loading);
                }
            }
            
        }
    }

}
